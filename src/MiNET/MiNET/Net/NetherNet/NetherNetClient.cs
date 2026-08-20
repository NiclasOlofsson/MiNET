#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net.Rtc;
using MiNET.Utils.Cryptography;

namespace MiNET.Net.NetherNet
{
	/// <summary>
	///     One outgoing NetherNet connection, which means being the WebRTC offerer: the client owns
	///     the two data channels and the offer; the server only answers. Signaling is a single HTTP
	///     round trip, so there is no socket to keep open and no trickle.
	///     <para>
	///         The whole per-connection transport lives and dies with this object: the UDP socket,
	///         the DTLS certificate, the peer, and the session. One socket per connection is not
	///         waste, it is the only thing address-based demultiplexing permits: two connections from
	///         one socket to one server are indistinguishable on the wire below the ICE layer, so the
	///         socket cannot be shared the way the server's listener-wide mux is. The
	///         <see cref="UdpMux" /> here is therefore mux-capable but never muxes: it serves as the
	///         socket, receive loop and tick source for exactly one peer.
	///     </para>
	/// </summary>
	public sealed class NetherNetClient : IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(NetherNetClient));

		public const string ReliableChannelLabel = "ReliableDataChannel";
		public const string UnreliableChannelLabel = "UnreliableDataChannel";

		/// <summary>
		///     How long the whole transport bring-up (ICE, DTLS, SCTP, DCEP channel open) may take
		///     after signaling succeeds. Generous: a loaded server misses STUN checks without being
		///     dead, and a failed attempt below this is cheap to retry.
		/// </summary>
		private static readonly TimeSpan TransportTimeout = TimeSpan.FromSeconds(20);

		private readonly UdpMux _mux;
		private readonly RtcCertificate _certificate;
		private readonly RtcPeer _peer;

		private int _disposed;

		/// <summary>The connected session, an <see cref="INetworkHandler" /> like any other.</summary>
		public NetherNetSession Session { get; }

		private NetherNetClient(UdpMux mux, RtcCertificate certificate, RtcPeer peer, NetherNetSession session)
		{
			_mux = mux;
			_certificate = certificate;
			_peer = peer;
			Session = session;

			// A remote teardown closes the session (its own OnTransportClosed wiring), but the session
			// only owns the peer; the socket and certificate are this object's, so it cleans itself up.
			// Deferred off the transport's own callback thread: Dispose tears the mux's timer and
			// receive loop down, and doing that inline from a thread they own can deadlock.
			peer.OnTransportClosed += () => Task.Run(Dispose);
		}

		/// <summary>
		///     Connects to a NetherNet server and returns a client whose <see cref="Session" /> has its
		///     reliable channel open.
		/// </summary>
		/// <summary>
		///     The whole exchange, both halves and verbatim: the request line and every header we
		///     sent, then the status line, every header that came back, and the body. The 26.50
		///     signaling grew (server-list status over HTTP, TLS, domain support) and its body is
		///     now the interesting half of a GET, so a headers-only log drops exactly the evidence
		///     this exists to collect. Mirrors what NetherNetListener logs on the serving side, so
		///     the two directions read the same.
		/// </summary>
		private static async Task LogSignalingAsync(string what, HttpResponseMessage response, CancellationToken cancellationToken)
		{
			HttpRequestMessage request = response.RequestMessage;

			// Content is buffered by default, so reading it here does not consume it for the caller.
			string body = await response.Content.ReadAsStringAsync(cancellationToken);

			Log.Info($"NetherNet signaling >>> {what}\n{HeaderLines(request?.Headers, request?.Content?.Headers)}\n"
					+ $"NetherNet signaling <<< HTTP/{response.Version} {(int) response.StatusCode} {response.ReasonPhrase}\n"
					+ $"{HeaderLines(response.Headers, response.Content.Headers)}\n{body}");
		}

		private static string HeaderLines(params HttpHeaders[] headers) =>
			string.Join("\n", headers.Where(h => h != null).SelectMany(h => h).SelectMany(h => h.Value.Select(v => $"{h.Key}: {v}")));

		/// <param name="host">Host running the signaling endpoint, which is the BDS server-port.</param>
		/// <param name="port">The signaling port. Under NetherNet, server-port is TCP, not UDP.</param>
		/// <param name="networkId">Our own NetworkID. Opaque to the server; generated when omitted.</param>
		public static async Task<NetherNetClient> ConnectAsync(string host, int port, string networkId = null, CancellationToken cancellationToken = default,
			XboxIdentity identity = null, string issuerDomain = "authorization.franchise.minecraft-services.net")
		{
			networkId ??= NewNetworkId();

			string baseUrl = $"http://{host}:{port}";
			using var http = new HttpClient {Timeout = TimeSpan.FromSeconds(15)};

			// The client checks for the endpoint before it spends anything on WebRTC. A non-2xx here
			// means the server is not speaking NetherNet, which on BDS means transport=raknet.
			HttpResponseMessage capability = await http.GetAsync($"{baseUrl}/v1/join", cancellationToken);
			await LogSignalingAsync($"GET {baseUrl}/v1/join", capability, cancellationToken);
			if (!capability.IsSuccessStatusCode) throw new IOException($"{host}:{port} does not accept NetherNet connections (GET /v1/join returned {(int) capability.StatusCode})");

			// OS-ephemeral port: a client needs no forwardable address, and every connection gets its
			// own socket (see the class remarks for why sharing one is impossible).
			var mux = new UdpMux(new IPEndPoint(IPAddress.Any, 0));
			RtcCertificate certificate = RtcCertificate.CreateSelfSigned();
			RtcPeer peer = null;

			try
			{
				mux.Start();
				peer = RtcPeer.CreateOfferer(mux, certificate);

				string offerSdp = peer.CreateOffer();

				// The assertion has to be added after the offer is generated, because it signs the
				// DTLS fingerprint line, and that does not exist until then.
				offerSdp = NetherNetIdentityAssertion.AddTo(offerSdp, identity, issuerDomain);

				if (Log.IsDebugEnabled) Log.Debug($"NetherNet offer for {networkId}:\n{offerSdp}");

				var content = new StringContent(offerSdp);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/sdp");

				HttpResponseMessage response = await http.PostAsync($"{baseUrl}/v1/join/{networkId}", content, cancellationToken);
				await LogSignalingAsync($"POST {baseUrl}/v1/join/{networkId}", response, cancellationToken);
				if (!response.IsSuccessStatusCode) throw new IOException($"NetherNet signaling rejected the offer with {(int) response.StatusCode} {response.ReasonPhrase}");

				string answerSdp = await response.Content.ReadAsStringAsync(cancellationToken);
				if (Log.IsDebugEnabled) Log.Debug($"NetherNet answer for {networkId}:\n{answerSdp}");

				peer.AcceptAnswer(answerSdp);

				// The association exists once the answer is applied, so the channels can be created
				// now: their DATA_CHANNEL_OPEN messages queue and ride out the moment the association
				// establishes (CreateDataChannel's own contract), matching the real client, whose
				// OPENs arrive with establishment.
				RtcDataChannel reliable = peer.CreateDataChannel(ReliableChannelLabel, ordered: true, maxRetransmits: -1);
				RtcDataChannel unreliable = peer.CreateDataChannel(UnreliableChannelLabel, ordered: false, maxRetransmits: 0);

				var channelOpen = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
				reliable.OnOpen += () => channelOpen.TrySetResult(true);
				if (reliable.IsOpen) channelOpen.TrySetResult(true);

				// ICE checks, DTLS and SCTP all happen here; the reliable channel's DCEP ACK is the
				// only signal that the whole stack, channels included, actually came up.
				// The caller gets what actually happened, not just that it did not work: which pair
				// ICE gave up on, and which of the peer's addresses were passed over because this
				// socket cannot reach that family. Without this the two are indistinguishable, and
				// they need opposite fixes.
				if (!await peer.WaitForTransportAsync(TransportTimeout))
				{
					string ignored = peer.IgnoredCandidates.Count == 0 ? "none" : string.Join(", ", peer.IgnoredCandidates);
					throw new IOException(
						$"NetherNet transport to {host}:{port} did not come up within {TransportTimeout.TotalSeconds:0}s. " +
						$"ICE: {peer.TransportFailureReason ?? "still checking, no verdict yet"}. " +
						$"Candidates this socket could not address: {ignored}. " +
						$"Datagrams dropped for the same reason: {mux.UnreachableFamilyDrops}.");
				}
				await WithCancellation(channelOpen.Task, cancellationToken);

				IPEndPoint remote = ResolveRemote(host, port);
				Log.Info($"NetherNet connected to {remote} as network id {networkId}");

				var session = new NetherNetSession(peer, reliable, unreliable, remote, networkId);
				return new NetherNetClient(mux, certificate, peer, session);
			}
			catch
			{
				peer?.Dispose();
				mux.Dispose();
				certificate.Dispose();
				throw;
			}
		}

		/// <summary>
		///     The NetworkID is documented as opaque and currently a 64 bit unsigned integer rendered
		///     as decimal, so it is generated in that shape without depending on it meaning anything.
		/// </summary>
		public static string NewNetworkId()
		{
			Span<byte> bytes = stackalloc byte[8];
			RandomNumberGenerator.Fill(bytes);
			return BitConverter.ToUInt64(bytes).ToString();
		}

		/// <summary>
		///     The gameplay path is the ICE-nominated pair, not the address we dialled for signaling,
		///     but the nominated endpoint has no accessor on RtcPeer yet, so this reports the dialled
		///     address as the best available stand-in (an IP literal parses; a hostname falls back to
		///     the wildcard address). Diagnostic only: nothing routes by this value.
		/// </summary>
		private static IPEndPoint ResolveRemote(string host, int port)
		{
			return IPEndPoint.TryParse($"{host}:{port}", out IPEndPoint parsed) ? parsed : new IPEndPoint(IPAddress.Any, port);
		}

		private static async Task WithCancellation(Task task, CancellationToken cancellationToken)
		{
			var cancelled = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
			await using (cancellationToken.Register(() => cancelled.TrySetCanceled()))
			{
				await await Task.WhenAny(task, cancelled.Task);
			}
		}

		public void Dispose()
		{
			if (Interlocked.Exchange(ref _disposed, 1) != 0) return;

			try
			{
				// Idempotent, and disposes the peer; a session already closed by a remote teardown
				// makes this a no-op.
				Session?.Close();
			}
			catch (Exception e)
			{
				Log.Debug("Closing NetherNet client session", e);
			}

			try
			{
				_mux.Dispose();
				_certificate.Dispose();
			}
			catch (Exception e)
			{
				Log.Debug("Disposing NetherNet client transport", e);
			}
		}
	}
}
