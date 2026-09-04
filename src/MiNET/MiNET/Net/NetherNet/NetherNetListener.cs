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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net.Rtc;
using MiNET.Plugins;
using MiNET.Utils;

namespace MiNET.Net.NetherNet
{
	/// <summary>
	///     Accepts NetherNet connections.
	///     <para>
	///         Signaling is a single HTTP round trip on a TCP port, so this is a small web server
	///         rather than a packet loop: <c>GET /v1/join</c> answers whether we speak NetherNet at
	///         all, and <c>POST /v1/join/{networkId}</c> carries the client's SDP offer and returns
	///         our answer. Everything after that is WebRTC, and the data channels the client opens
	///         become a <see cref="NetherNetSession" />.
	///     </para>
	///     <para>
	///         Written on a raw TcpListener rather than HttpListener because the latter needs a URL
	///         reservation or elevation on Windows for anything but a loopback prefix, and two fixed
	///         routes do not justify that.
	///     </para>
	///     <para>
	///         The gameplay path runs on one <see cref="UdpMux" /> for the listener's whole lifetime,
	///         shared by every negotiated peer, and one <see cref="RtcCertificate" /> is the DTLS
	///         identity every one of them answers with. Negotiating a peer no longer waits on
	///         anything: <see cref="RtcPeer.AcceptOffer" /> has nothing to gather (the mux is already
	///         bound, so there is exactly one local candidate) and returns the answer synchronously.
	///     </para>
	/// </summary>
	public class NetherNetListener
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(NetherNetListener));

		private static readonly Regex JoinRoute = new(@"^/v1/join/(?<networkId>[^/\s?]+)", RegexOptions.Compiled);

		private readonly IPEndPoint _endPoint;

		/// <summary>
		///     Every signaling port, the configured one included: a socket binds one port, so several
		///     ports is several sockets and there is nothing else to it. They share this listener's
		///     mux, certificate and session table, and the SDP answer names the same UDP endpoint
		///     whichever port the offer arrived on, so a port carries no transport meaning at all. It
		///     is only recorded on the session, as the way in that the client was given.
		/// </summary>
		private readonly ConcurrentDictionary<int, TcpListener> _ports = new();
		private CancellationTokenSource _cancellation;
		private UdpMux _mux;
		private RtcCertificate _certificate;

		// The machine's own addresses, enumerated once at Start (EnumerateLocalAddresses): the host
		// candidate set every answer advertises. The client's ICE checks pick the reachable one.
		private IReadOnlyList<IPAddress> _localAddresses;
		private Timer _sweepTimer;

		// The same InactivityTimeout knob the RakNet era used, so configs carry over unchanged.
		private readonly int _inactivityTimeout = Config.GetProperty("InactivityTimeout", 8500);

		// A client that has connected but not yet spoken gets longer: 8.5s here turns a slow join
		// into a failed one, and a session that never speaks is only holding a port, not a slot in
		// anyone's game. Matches the 30s spawn budget the emulator itself allows a join. Also the
		// deadline a negotiated-but-not-yet-attached RtcPeer gets in the pending-peer table below.
		// A constructor override exists so a test can shorten it without touching server.conf.
		private readonly int _connectingTimeout;

		/// <summary>Live sessions by the client's NetworkID.</summary>
		public ConcurrentDictionary<string, NetherNetSession> Sessions { get; } = new();

		/// <summary>
		///     Set before <see cref="Start" /> to have the mux answer server-list discovery pings on
		///     the gameplay UDP port; leave null and the legacy format never touches this listener.
		/// </summary>
		public NetherNetDiscovery Discovery { get; set; }

		/// <summary>
		///     Clear this to stop answering join signaling without tearing the socket down. A
		///     shutdown needs it: transferring or disconnecting players frees the server only if they
		///     cannot immediately come back, and a Bedrock client reconnects within milliseconds, so
		///     otherwise the rejoin races the save and lands mid-shutdown.
		/// </summary>
		public bool AcceptConnections { get; set; } = true;

		/// <summary>
		///     Negotiated peers whose reliable data channel has not opened yet, keyed by the peer
		///     itself (nothing about a client that never gets this far is worth keying on) with the
		///     tick each one must attach by. <see cref="AttachSession" /> removes an entry the moment
		///     it promotes it to a session; <see cref="SweepExpiredPendingPeers" /> disposes whatever
		///     is left past its deadline, so a negotiation nobody ever finishes - ICE that never
		///     nominates, DTLS that never completes, a reliable channel that never opens - cannot hold
		///     its slice of <see cref="_mux" /> forever.
		/// </summary>
		private readonly ConcurrentDictionary<RtcPeer, long> _pendingPeers = new();

		/// <summary>Test visibility only (assembly's InternalsVisibleTo to MiNETTests): how many negotiated peers are still waiting for their reliable channel to open.</summary>
		internal int PendingPeerCount => _pendingPeers.Count;

		/// <summary>Test visibility only (assembly's InternalsVisibleTo to MiNETTests): the TCP endpoint actually bound, once <see cref="Start" /> has run - lets a test bind an ephemeral port (0) and discover which one the OS chose.</summary>
		internal IPEndPoint LocalEndPoint => _ports.TryGetValue(_boundPort, out TcpListener listener) ? (IPEndPoint) listener.LocalEndpoint : null;

		/// <summary>The configured port as actually bound, which differs from the requested one when 0 asked the OS to choose.</summary>
		private int _boundPort;

		/// <summary>
		///     Builds the handler that sits above the transport: the batching, compression and
		///     login path.
		/// </summary>
		public Func<NetherNetSession, ICustomMessageHandler> CustomMessageHandlerFactory { get; set; }

		/// <summary>
		///     Consulted for any request that is not NetherNet's own, so a plugin can serve the server
		///     port. Returns null when nothing matches, which is answered 404. Signaling keeps
		///     <c>/v1/*</c> to itself, checked before this, so no route can shadow the protocol.
		/// </summary>
		public Func<HttpRequest, HttpResponse> RequestHandler { get; set; }

		/// <summary>
		///     The JSON body <c>GET /v1/join</c> answers with: since 1.26.50 that GET is the client's
		///     server-list ping and the body is the status line the server tab renders. Null leaves
		///     the pre-1.26.50 behavior, an empty 200 that only says "we speak NetherNet".
		/// </summary>
		public Func<string> JoinStatusProvider { get; set; }

		/// <summary>
		///     Consulted when a client opens with TLS instead of plaintext: given the ClientHello's
		///     SNI host (null when absent, which is every real Bedrock client) and the connection's
		///     source address, returns the certificate context to complete the handshake with, or
		///     null to refuse the way BDS does so the client falls back to plaintext. Null provider
		///     means TLS is always refused, the behavior before certificates existed.
		/// </summary>
		public Func<string, IPAddress, SslStreamCertificateContext> TlsCertificateProvider { get; set; }

		/// <summary>
		///     Answers ACME HTTP-01 validation on this port: given the token from
		///     <c>GET /.well-known/acme-challenge/{token}</c>, returns the key authorization body, or
		///     null for 404. The route is only claimed while a handler is set, so plugins keep the
		///     path when no certificate machinery runs.
		/// </summary>
		public Func<string, string> AcmeChallengeHandler { get; set; }

		/// <summary>
		///     The long-lived key clients pin us by. Persisted, because regenerating it makes every
		///     returning player see a first-use prompt again.
		/// </summary>
		public NetherNetServerIdentity ServerIdentity { get; set; }

		/// <summary>Where gameplay UDP binds, and what address clients are told to dial.</summary>
		public NetherNetPortMapping PortMapping { get; set; }

		public NetherNetListener(IPEndPoint endPoint, NetherNetServerIdentity serverIdentity = null, NetherNetPortMapping portMapping = null, int? connectingTimeout = null)
		{
			_endPoint = endPoint;
			ServerIdentity = serverIdentity ?? new NetherNetServerIdentity();
			PortMapping = portMapping ?? NetherNetPortMapping.Parse(Config.GetProperty("server-udp-ports", ""));
			_connectingTimeout = connectingTimeout ?? Config.GetProperty("NetherNetConnectingTimeout", 30000);
		}

		public void Start()
		{
			_cancellation = new CancellationTokenSource();

			// One UdpMux and one RtcCertificate for the listener's whole lifetime: every RtcPeer
			// this listener answers with shares both, which is what lets one UDP socket carry every
			// session (ICE demultiplexes by ufrag, DTLS/SCTP by the nominated remote endpoint).
			// Bound to the first port of the configured range: unlike a per-connection socket there
			// is no bind-cursor to walk further into a wider one, port 0 leaves the choice to the OS
			// when nothing was configured.
			_mux = new UdpMux(new IPEndPoint(_endPoint.Address, PortMapping.BindPort ?? 0));

			// The first-contact admission budget, sized here because this layer owns the config.
			// Each joining client burns one admission per advertised candidate its checks arrive
			// through, so the bound scales with candidate count times expected concurrent players.
			UdpMux.MaxUnknownEndpointAdmissions = Config.GetProperty("Mux.MaxUnknownEndpointAdmissions", UdpMux.MaxUnknownEndpointAdmissions);

			// Server-list discovery, if the host wired it up: the mux answers RakNet unconnected
			// pings on the gameplay port so a NetherNet-only server still shows a status line in
			// the client's server tab. Discovery reaches this port only when the gameplay UDP is
			// the port clients ping, 19132, so server-udp-ports has to put it there.
			if (Discovery != null)
			{
				_mux.OfflineResponder = Discovery.HandleOffline;
			}

			_mux.Start();
			_certificate = RtcCertificate.CreateSelfSigned();

			_localAddresses = EnumerateLocalAddresses();

			Log.Info($"NetherNet gameplay UDP bound to {_mux.LocalEndPoint}, advertising host candidates: {string.Join(", ", _localAddresses)}");
			if (PortMapping.RangeStart.HasValue && PortMapping.RangeEnd.HasValue && PortMapping.RangeEnd.Value > PortMapping.RangeStart.Value)
			{
				Log.Warn($"server-udp-ports configures {PortMapping.RangeStart}-{PortMapping.RangeEnd}, but one shared UdpMux binds a single UDP port; only {_mux.LocalEndPoint.Port} is used.");
			}

			_sweepTimer = new Timer(_ => Sweep(), null, 2500, 2500);

			// Last, deliberately. A port that is accepting before the mux and certificate exist can
			// take an offer that AnswerOffer then has nothing to answer with.
			_boundPort = OpenPort(_endPoint.Port);
			if (_boundPort == 0) throw new IOException($"Could not bind the NetherNet signaling port {_endPoint.Port}");
		}

		/// <summary>
		///     Opens a signaling port and starts accepting on it. Arrivals are ordinary sessions that
		///     happen to record which port they came in by.
		///     <para>False when the port is already open here, or the OS refuses the bind.</para>
		/// </summary>
		public bool AddSignalingPort(int port)
		{
			if (_cancellation == null || _cancellation.IsCancellationRequested) return false;

			return OpenPort(port) != 0;
		}

		/// <summary>Binds and starts accepting, returning the port actually bound, or 0 on failure.</summary>
		private int OpenPort(int port)
		{
			if (port != 0 && _ports.ContainsKey(port)) return 0;

			TcpListener listener;
			try
			{
				// Dual stack when no specific address was asked for, which is what BDS does: one IPv6
				// socket with DualMode serves both families. Binding 0.0.0.0 instead means a client
				// that resolves the address to ::1 finds nothing listening and the join simply never
				// arrives, with no error anywhere to explain it.
				if (_endPoint.Address.Equals(IPAddress.Any) && Socket.OSSupportsIPv6)
				{
					listener = new TcpListener(IPAddress.IPv6Any, port);
					listener.Server.DualMode = true;
				}
				else
				{
					listener = new TcpListener(_endPoint.Address, port);
				}

				listener.Start();
			}
			catch (SocketException e)
			{
				Log.Warn($"Could not open signaling port {port}: {e.Message}");
				return 0;
			}

			// Asked for 0, the OS chose, so the table is keyed by what was actually bound rather
			// than by the request.
			int bound = ((IPEndPoint) listener.LocalEndpoint).Port;

			if (!_ports.TryAdd(bound, listener))
			{
				listener.Stop();
				return 0;
			}

			_ = Task.Run(() => AcceptLoop(listener, _cancellation.Token));

			Log.Info($"NetherNet signaling listening on tcp {listener.LocalEndpoint} (dual stack: {listener.Server.DualMode})");
			return bound;
		}

		/// <summary>
		///     Closes a signaling port. Sessions that arrived through it are left alone: they are
		///     established connections on the shared mux and no longer need the way they came in by.
		/// </summary>
		public bool RemoveSignalingPort(int port)
		{
			if (!_ports.TryRemove(port, out TcpListener listener)) return false;

			listener.Stop();
			Log.Info($"NetherNet signaling stopped on tcp port {port}");
			return true;
		}

		/// <summary>Every signaling port currently open, the configured one included.</summary>
		public IReadOnlyCollection<int> SignalingPorts => _ports.Keys.ToArray();

		public void Stop()
		{
			_cancellation?.Cancel();

			foreach (TcpListener listener in _ports.Values) listener.Stop();
			_ports.Clear();

			_sweepTimer?.Dispose();
			_sweepTimer = null;

			foreach (NetherNetSession session in Sessions.Values) session.Close();
			Sessions.Clear();

			foreach (RtcPeer peer in _pendingPeers.Keys)
			{
				if (_pendingPeers.TryRemove(peer, out _)) DisposePendingPeer(peer);
			}

			// Every peer above has already released its slice of the mux (RtcPeer.Dispose removes its
			// own ufrag registration), so the mux and the certificate they all shared can go last.
			_mux?.Dispose();
			_mux = null;
			_certificate?.Dispose();
			_certificate = null;
		}

		// Sweep reentrancy guard: System.Threading.Timer fires on schedule regardless of whether
		// the previous callback finished, and a sweep pass tearing down a large batch of dead
		// sessions takes real time (each close runs the full player disconnect). Overlapping
		// passes once stacked dozens of blocked pool threads and starved every joining player's
		// login (the 85-bot loss, 2026-08-13); a pass that finds the previous one still running
		// simply yields to it.
		private int _sweeping;

		/// <summary>One timer callback covers both liveness backstops: a live session gone silent, and a negotiation that never attached one at all. Both run off the same clock, so one timer serves both.</summary>
		private void Sweep()
		{
			if (Interlocked.Exchange(ref _sweeping, 1) != 0) return;
			try
			{
				SweepInactiveSessions();
				SweepExpiredPendingPeers();
			}
			finally
			{
				_sweeping = 0;
			}
		}

		/// <summary>
		///     The backstop that actually notices a vanished client. SCTP surfaces no remote close and
		///     ICE state can sit in connected forever, but a live client is never silent, so silence
		///     past the timeout is the one signal that always arrives.
		/// </summary>
		private void SweepInactiveSessions()
		{
			foreach (NetherNetSession session in Sessions.Values)
			{
				// A session that closed before OnClosed was wired up would otherwise sit here forever.
				if (session.IsClosed)
				{
					Sessions.TryRemove(new KeyValuePair<string, NetherNetSession>(session.NetworkId, session));
					continue;
				}

				// Connecting means "until login completes", not "until the first byte": a client
				// stuck fetching its auth token has typically already sent RequestNetworkSettings,
				// and judging it on the in-game clock cuts it off mid-recovery.
				bool loggedIn = session.Username != null;
				int timeout = loggedIn ? _inactivityTimeout : _connectingTimeout;
				if (session.MillisSinceLastReceive <= timeout) continue;

				string phase = loggedIn ? "" : session.HasReceived ? " (pre-login)" : " (never spoke)";
				Log.Warn($"NetherNet session for {session.Username ?? session.NetworkId} timed out, no traffic for {timeout}ms{phase}");
				session.Disconnect("Network timeout", false);
			}
		}

		/// <summary>A negotiation whose reliable channel never opened in time never gets a session, so nothing else ever disposes its <see cref="RtcPeer" />; this is the only thing that does.</summary>
		private void SweepExpiredPendingPeers()
		{
			long now = Environment.TickCount64;
			foreach (KeyValuePair<RtcPeer, long> entry in _pendingPeers)
			{
				if (now < entry.Value) continue;
				if (!_pendingPeers.TryRemove(entry.Key, out _)) continue;

				Log.Warn("NetherNet negotiation timed out before its reliable channel opened, discarding the peer");
				DisposePendingPeer(entry.Key);
			}
		}

		private static void DisposePendingPeer(RtcPeer peer)
		{
			try
			{
				peer.Dispose();
			}
			catch (Exception e)
			{
				Log.Debug("Disposing an abandoned NetherNet negotiation", e);
			}
		}

		private async Task AcceptLoop(TcpListener listener, CancellationToken cancellationToken)
		{
			// Read once: it is the door every connection on this loop arrived through, and after
			// RemoveSignalingPort stops the listener it can no longer be asked.
			int port = ((IPEndPoint) listener.LocalEndpoint).Port;

			while (!cancellationToken.IsCancellationRequested)
			{
				try
				{
					TcpClient client = await listener.AcceptTcpClientAsync(cancellationToken);
					// Signaling is one request per connection, so each is handled and dropped.
					_ = Task.Run(() => HandleSignaling(client, port), cancellationToken);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (ObjectDisposedException)
				{
					return; // this door was closed
				}
				catch (SocketException) when (!_ports.ContainsKey(port))
				{
					return; // same, seen as a socket error rather than a disposal
				}
				catch (Exception e)
				{
					if (!cancellationToken.IsCancellationRequested) Log.Error("NetherNet signaling accept failed", e);
				}
			}
		}

		private async Task HandleSignaling(TcpClient client, int signalingPort)
		{
			using (client)
			{
				try
				{
					Stream stream = await NegotiateTransportAsync(client.GetStream());
					if (stream == null) return;
					await using Stream _ = stream;

					(string method, string path, string headers, string body) = await ReadRequest(stream);

					// A peer that closed before completing its headers left nothing to route. Seen
					// live from the real client: it completes a TLS handshake, sends nothing, and
					// hangs up, then rejoins in plaintext. Reaching here means the read returned a
					// clean end-of-stream, which for TLS is a proper close_notify: the client's
					// application chose to leave an intact channel, its stack did not abort (an
					// abort surfaces as an exception, logged in the catch below).
					if (method == null || path == null)
					{
						if (stream is SslStream) Log.Info($"NetherNet signaling TLS connection from {SafePeer(client)} closed cleanly (close_notify) without sending a request");
						else Log.Debug($"NetherNet signaling connection from {SafePeer(client)} closed before a complete request");
						return;
					}

					// Every request logged whole, request line and all headers, whatever it asks
					// for. Signaling is one round trip per connection and the whole negotiation
					// lives in it, so nothing about it is recoverable later: a client that refuses
					// us leaves no other trace, there is no error packet, it simply stops. A route
					// that is NOT ours is worth as much, because a client version that starts
					// asking for a path or sending a header we have never seen shows up here and
					// nowhere else.
					bool isSignaling = path.StartsWith("/v1/", StringComparison.Ordinal);
					Log.Info($"{(isSignaling ? "NetherNet signaling" : "HTTP")} <<< {SafePeer(client)}{(stream is SslStream ? " (tls)" : "")}\n{headers}\n{body}");

					if (!AcceptConnections)
					{
						// No reply at all: the client concludes the server is down, which is the
						// truth a shutting-down server wants told. An error response would make it
						// retry immediately.
						return;
					}

					if (method == "GET" && path.StartsWith("/v1/join", StringComparison.Ordinal))
					{
						// Any 2xx means "yes, we speak NetherNet"; since 1.26.50 the body is also the
						// server-list status the client renders in the server tab.
						string status = JoinStatusProvider?.Invoke();
						if (status != null) await Respond(stream, 200, "application/json", status);
						else await Respond(stream, 200, "text/plain", "");
						return;
					}

					// ACME HTTP-01 validation (and the issue-preflight probe), claimed ahead of the
					// plugin routes only while certificate machinery runs, so plugins keep the path
					// otherwise. Let's Encrypt's validators dial port 80 on the domain; a forward
					// from there is what lands them here.
					Func<string, string> acme = AcmeChallengeHandler;
					if (acme != null && method == "GET" && path.StartsWith(AcmeChallengePrefix, StringComparison.Ordinal))
					{
						string keyAuthorization = acme(path.Substring(AcmeChallengePrefix.Length));
						await Respond(stream, keyAuthorization != null ? 200 : 404, "text/plain", keyAuthorization ?? "");
						return;
					}

					Match route = JoinRoute.Match(path);
					if (method != "POST" || !route.Success)
					{
						// Everything NetherNet does not claim is offered to the plugins, which is why
						// this sits below the /v1 routes and above the 404.
						HttpResponse response = InvokeRequestHandler(method, path, headers, body, client);
						if (response != null)
						{
							await Respond(stream, response.Status, response.ContentType, response.Body);
							return;
						}

						await Respond(stream, 404, "text/plain", "");
						return;
					}

					if (string.IsNullOrWhiteSpace(body))
					{
						await Respond(stream, 400, "text/plain", "Missing SDP offer in request body");
						return;
					}

					string networkId = route.Groups["networkId"].Value;
					string answer = AnswerOffer(networkId, body, ReadHost(headers), signalingPort);

					await Respond(stream, 200, "application/sdp", answer);
				}
				catch (Exception e)
				{
					// Naming the peer matters more than the exception: a bare reset is what a probe
					// looks like, and without an address there is no way to tell a real client
					// checking us from a port scanner.
					Log.Warn($"NetherNet signaling request from {SafePeer(client)} failed: {e.Message}");
				}
			}
		}

		private const string AcmeChallengePrefix = "/.well-known/acme-challenge/";

		/// <summary>
		///     Sorts out how the connection opens. A plaintext request passes straight through
		///     untouched. The real client tries TLS before plaintext, and the ClientHello is answered
		///     one of two ways: a completed handshake when <see cref="TlsCertificateProvider" />
		///     supplies a certificate for the offered SNI, or a fatal handshake_failure alert (null
		///     return, connection done). The refusal mirrors BDS: it must be a refusal the client
		///     understands, because a reset or silence leaves it with a broken handshake and it never
		///     falls back to plaintext and the trust-on-first-use path.
		/// </summary>
		private async Task<Stream> NegotiateTransportAsync(NetworkStream stream)
		{
			// Peek rather than read, so a plain HTTP request keeps its bytes. Enough for a whole
			// ClientHello, which is the only record worth looking at here.
			var head = new byte[4096];
			int peeked = stream.Socket.Receive(head, SocketFlags.Peek);
			if (peeked == 0 || head[0] != 0x16) return stream;

			// The client resolves a transfer's host name before it builds its HTTP request, so the
			// Host header is always an address. The ClientHello is the one place a NAME survives,
			// so SNI is the only thing a certificate can be matched against: a client that dialled
			// by address offers no name and gets the refusal, whatever certificates are held.
			string serverName = ReadSni(head, peeked);
			IPAddress remoteAddress = (stream.Socket.RemoteEndPoint as IPEndPoint)?.Address;

			// The full anatomy of the offer, every time: the real client completes a handshake and
			// then abandons the connection, and what its stack asked for is the evidence trail.
			Log.Info($"NetherNet signaling TLS offer from {remoteAddress}: {DescribeClientHello(head, peeked)}");

			SslStreamCertificateContext certificate = TlsCertificateProvider?.Invoke(serverName, remoteAddress);
			if (certificate == null)
			{
				Log.Info($"NetherNet signaling: client offered TLS (SNI {serverName ?? "absent"}), refusing with handshake_failure so it falls back to plaintext");

				// Alert record: content type 21, TLS 1.0 version for maximum compatibility with a peer
				// whose negotiated version is not yet known, length 2, level fatal (2), handshake_failure (40).
				await stream.WriteAsync(new byte[] {0x15, 0x03, 0x01, 0x00, 0x02, 0x02, 0x28});
				await stream.FlushAsync();

				return null;
			}

			var ssl = new SslStream(stream, leaveInnerStreamOpen: false);
			try
			{
				await ssl.AuthenticateAsServerAsync(new SslServerAuthenticationOptions
				{
					ServerCertificateContext = certificate,
					// http/1.1 only, deliberately: the reader above this stream speaks nothing else,
					// so the negotiation must land there even for a client that would prefer h2.
					// Inert for clients that offer no ALPN.
					ApplicationProtocols = new List<SslApplicationProtocol> {SslApplicationProtocol.Http11},
				});
			}
			catch
			{
				await ssl.DisposeAsync();
				throw;
			}

			SslApplicationProtocol negotiated = ssl.NegotiatedApplicationProtocol;
			Log.Info($"NetherNet signaling TLS established (SNI {serverName ?? "absent"}, source {remoteAddress}): "
					+ $"{ssl.SslProtocol}, cipher {ssl.NegotiatedCipherSuite}, alpn {(negotiated.Protocol.IsEmpty ? "none" : negotiated.ToString())}");
			return ssl;
		}

		/// <summary>
		///     The machine's own IPv4 unicast addresses, enumerated ONCE at <see cref="Start" />: the
		///     mux listens on the wildcard address, so every one of these reaches the same socket, and
		///     the answer advertises all of them as host candidates. The client's own ICE checks decide
		///     which one is reachable; the server never guesses. Link-local (169.254/16) is skipped, it
		///     is never a usable gameplay path; loopback is included last, so a candidate exists even on
		///     a machine with no network at all.
		/// </summary>
		private static IReadOnlyList<IPAddress> EnumerateLocalAddresses()
		{
			var addresses = new List<IPAddress>();
			try
			{
				foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
				{
					if (nic.OperationalStatus != OperationalStatus.Up) continue;
					if (nic.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;

					foreach (UnicastIPAddressInformation unicast in nic.GetIPProperties().UnicastAddresses)
					{
						IPAddress address = unicast.Address;
						if (address.AddressFamily != AddressFamily.InterNetwork) continue;
						if (address.ToString().StartsWith("169.254.", StringComparison.Ordinal)) continue;
						if (!addresses.Contains(address)) addresses.Add(address);
					}
				}
			}
			catch (Exception e)
			{
				Log.Warn("Enumerating network interfaces for NetherNet candidates failed; falling back to loopback only", e);
			}

			addresses.Add(IPAddress.Loopback);
			return addresses;
		}

		/// <summary>A disposed or reset socket throws on RemoteEndPoint, which must not hide the log line.</summary>
		private static string SafePeer(TcpClient client)
		{
			try
			{
				return client.Client?.RemoteEndPoint?.ToString() ?? "unknown";
			}
			catch
			{
				return "unknown";
			}
		}

		/// <summary>
		///     Hands a non-signaling request to whatever registered <see cref="RequestHandler" />,
		///     null when nothing is registered or nothing matched. The handler's own failures are its
		///     caller's to report; anything thrown here is this listener's bug and is answered 500
		///     rather than dropped, because a connection closed without a reply is indistinguishable
		///     from a server that is down.
		/// </summary>
		private HttpResponse InvokeRequestHandler(string method, string path, string headers, string body, TcpClient client)
		{
			Func<HttpRequest, HttpResponse> handler = RequestHandler;
			if (handler == null) return null;

			try
			{
				int question = path.IndexOf('?');

				var request = new HttpRequest
				{
					Method = method,
					Path = question < 0 ? path : path.Substring(0, question),
					Query = question < 0 ? "" : path.Substring(question + 1),
					Headers = ParseHeaders(headers),
					Body = body ?? "",
					RemoteEndPoint = client.Client.RemoteEndPoint as IPEndPoint
				};

				return handler(request);
			}
			catch (Exception e)
			{
				Log.Error($"Routing {method} {path} failed", e);
				return HttpResponse.Empty(500);
			}
		}

		/// <summary>The header block as a lookup, request line skipped, duplicates last-wins.</summary>
		private static IReadOnlyDictionary<string, string> ParseHeaders(string headers)
		{
			var parsed = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

			foreach (string line in (headers ?? "").Split("\r\n", StringSplitOptions.RemoveEmptyEntries).Skip(1))
			{
				int colon = line.IndexOf(':');
				if (colon <= 0) continue;

				parsed[line.Substring(0, colon).Trim()] = line.Substring(colon + 1).Trim();
			}

			return parsed;
		}

		/// <summary>
		///     A one-line forensic description of a TLS ClientHello: offered versions, cipher-suite
		///     count, ALPN protocols, SNI, and the raw extension id list. Exists because the real
		///     Bedrock client completes a handshake and then abandons the connection, and what its
		///     stack asked for is the only clue to why. Hostile-input rules as
		///     <see cref="ReadSni" />: any malformed length degrades the description, never throws.
		/// </summary>
		internal static string DescribeClientHello(byte[] hello, int length)
		{
			try
			{
				int at = 5;
				if (length < at + 4 || hello[at] != 0x01) return "not a ClientHello";

				// legacy_version predates 1.3's supported_versions extension; both are reported.
				at += 4;
				string legacyVersion = length >= at + 2 ? VersionName((hello[at] << 8) | hello[at + 1]) : "?";
				at += 2 + 32; // legacy_version, random

				if (length < at + 1) return $"truncated (legacy {legacyVersion})";
				at += 1 + hello[at]; // legacy_session_id

				if (length < at + 2) return $"truncated (legacy {legacyVersion})";
				int cipherCount = ((hello[at] << 8) | hello[at + 1]) / 2;
				at += 2 + ((hello[at] << 8) | hello[at + 1]); // cipher_suites

				if (length < at + 1) return $"truncated (legacy {legacyVersion}, {cipherCount} ciphers)";
				at += 1 + hello[at]; // legacy_compression_methods

				if (length < at + 2) return $"truncated (legacy {legacyVersion}, {cipherCount} ciphers)";
				int extensionsEnd = at + 2 + ((hello[at] << 8) | hello[at + 1]);
				at += 2;

				var versions = new List<string>();
				var alpn = new List<string>();
				var extensionIds = new List<int>();
				string sni = null;

				while (at + 4 <= Math.Min(length, extensionsEnd))
				{
					int type = (hello[at] << 8) | hello[at + 1];
					int size = (hello[at + 2] << 8) | hello[at + 3];
					at += 4;
					extensionIds.Add(type);
					int end = Math.Min(length, at + size);

					if (type == 0 && at + 5 <= end && hello[at + 2] == 0)
					{
						int nameLength = (hello[at + 3] << 8) | hello[at + 4];
						if (at + 5 + nameLength <= end) sni = Encoding.ASCII.GetString(hello, at + 5, nameLength);
					}
					else if (type == 16 && at + 2 <= end)
					{
						// ALPN: a u16 list of length-prefixed protocol names.
						int walk = at + 2;
						while (walk < end)
						{
							int nameLength = hello[walk];
							if (walk + 1 + nameLength > end) break;
							alpn.Add(Encoding.ASCII.GetString(hello, walk + 1, nameLength));
							walk += 1 + nameLength;
						}
					}
					else if (type == 43 && at + 1 <= end)
					{
						// supported_versions: a u8-length list of u16 versions.
						int walk = at + 1;
						while (walk + 2 <= Math.Min(end, at + 1 + hello[at]))
						{
							versions.Add(VersionName((hello[walk] << 8) | hello[walk + 1]));
							walk += 2;
						}
					}

					at += size;
				}

				return $"versions=[{(versions.Count > 0 ? string.Join(",", versions) : legacyVersion)}]"
					+ $" ciphers={cipherCount}"
					+ $" alpn=[{string.Join(",", alpn)}]"
					+ $" sni={sni ?? "absent"}"
					+ $" extensions=[{string.Join(",", extensionIds)}]";
			}
			catch (Exception e)
			{
				Log.Debug("Could not describe a TLS ClientHello", e);
				return "unparseable";
			}
		}

		private static string VersionName(int wire) => wire switch
		{
			0x0304 => "1.3",
			0x0303 => "1.2",
			0x0302 => "1.1",
			0x0301 => "1.0",
			_ => $"0x{wire:x4}",
		};

		/// <summary>
		///     The server_name extension of a TLS ClientHello, or null if the record is truncated,
		///     malformed or carries no SNI. Parsing runs on the accept path, so every length is
		///     treated as hostile: a bad one returns null rather than throwing.
		/// </summary>
		private static string ReadSni(byte[] hello, int length)
		{
			try
			{
				// Record header (5) then handshake header (4). Only a ClientHello (1) is of interest.
				int at = 5;
				if (length < at + 4 || hello[at] != 0x01) return null;

				at += 4;
				at += 2 + 32; // client_version, random

				if (length < at + 1) return null;
				at += 1 + hello[at]; // legacy_session_id

				if (length < at + 2) return null;
				at += 2 + ((hello[at] << 8) | hello[at + 1]); // cipher_suites

				if (length < at + 1) return null;
				at += 1 + hello[at]; // legacy_compression_methods

				if (length < at + 2) return null;
				int extensionsEnd = at + 2 + ((hello[at] << 8) | hello[at + 1]);
				at += 2;

				while (at + 4 <= Math.Min(length, extensionsEnd))
				{
					int type = (hello[at] << 8) | hello[at + 1];
					int size = (hello[at + 2] << 8) | hello[at + 3];
					at += 4;

					// server_name (0), whose list holds entries of name_type (0 is host_name), a
					// 16 bit length, and the name itself.
					if (type == 0 && at + 5 <= length)
					{
						int nameLength = (hello[at + 3] << 8) | hello[at + 4];
						if (hello[at + 2] != 0 || at + 5 + nameLength > length) return null;

						return Encoding.ASCII.GetString(hello, at + 5, nameLength);
					}

					at += size;
				}
			}
			catch (Exception e)
			{
				Log.Debug("Could not read SNI from a TLS ClientHello", e);
			}

			return null;
		}

		/// <summary>
		///     The host the client dialled, port stripped. It echoes back whatever string was put in
		///     <c>McpeTransfer.serverAddress</c>, a name as a name rather than the address it resolved
		///     to, so a transfer can name which of several front doors a player is arriving through.
		/// </summary>
		private static string ReadHost(string headers)
		{
			Match host = Regex.Match(headers ?? "", @"^Host:\s*(.+?)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			if (!host.Success) return null;

			string value = host.Groups[1].Value;

			// IPv6 literals are bracketed, so the last colon only separates a port outside the brackets.
			int colon = value.LastIndexOf(':');
			if (colon > value.LastIndexOf(']') && colon >= 0) value = value.Substring(0, colon);

			return value.Trim('[', ']');
		}

		/// <summary>
		///     Answers an offer synchronously: <see cref="RtcPeer.AcceptOffer" /> has nothing to
		///     gather (the mux's one local candidate is already known) so there is no wait between
		///     accepting the offer and having a complete answer to sign and return.
		///     <para>
		///         Subscribes <see cref="RtcPeer.OnDataChannel" /> before calling
		///         <see cref="RtcPeer.AcceptOffer" />: the client can start opening its data channels
		///         the instant its own ICE/DTLS/SCTP stack comes up, which can race arbitrarily far
		///         ahead of this method returning the answer, let alone the caller responding with it.
		///     </para>
		/// </summary>
		private string AnswerOffer(string networkId, string offerSdp, string signalingHost, int signalingPort)
		{
			// The identity assertion is ours to validate or ignore by policy, but it must come out
			// before the offer is parsed either way: a=identity is not an attribute WebRTC knows,
			// and implementations reject an SDP carrying attributes they cannot parse.
			string strippedOffer = StripIdentity(offerSdp, out string assertion);
			if (assertion != null) Log.Debug($"NetherNet offer from {networkId} carries an identity assertion ({assertion.Length} chars)");

			RtcPeer peer = RtcPeer.CreateAnswerer(_mux, _certificate);
			_pendingPeers[peer] = Environment.TickCount64 + _connectingTimeout;

			// Guards session/pendingUnreliable below: OnDataChannel and OnTransportClosed can both
			// run on the mux's own receive/tick threads, concurrently with each other.
			var gate = new object();
			NetherNetSession session = null;
			RtcDataChannel pendingUnreliable = null;

			peer.OnDataChannel += channel =>
			{
				try
				{
					lock (gate)
					{
						if (channel.Label == NetherNetClient.UnreliableChannelLabel)
						{
							// May arrive before or after the reliable channel attaches a session; either
							// way it is not lost, only its destination differs.
							if (session != null) session.AttachUnreliableChannel(channel);
							else pendingUnreliable = channel;
							return;
						}

						// Anything not labeled unreliable is the reliable channel, matching the two
						// labels both connectors ever open. A second one, which should never happen,
						// is ignored rather than replacing an already-attached session.
						if (session != null) return;
						if (!_pendingPeers.TryRemove(peer, out _)) return; // already expired or removed

						session = AttachSession(networkId, peer, channel, pendingUnreliable, signalingHost, signalingPort);
					}
				}
				catch (Exception e)
				{
					Log.Error($"NetherNet data channel handling failed for {networkId}", e);
				}
			};

			// A peer whose transport came up and then tore down again before its reliable channel
			// ever opened would otherwise sit in the pending table until the sweep's own timeout:
			// harmless, but pointless. OnTransportClosed never fires for a handshake that never
			// succeeded in the first place (RtcPeer's own contract), so the sweep is still what
			// catches those; this only shortens the wait for the ones that did come up.
			peer.OnTransportClosed += () =>
			{
				lock (gate)
				{
					if (session != null) return; // NetherNetSession owns teardown from here
					if (!_pendingPeers.TryRemove(peer, out _)) return;
				}

				Log.Debug($"NetherNet negotiation for {networkId} tore down before its reliable channel opened");
			};

			string answerSdp = peer.AcceptOffer(strippedOffer);

			// The mux is bound to the wildcard address and RtcPeer advertises its LocalEndPoint
			// verbatim, so the raw answer carries one 0.0.0.0 candidate: undialable. Expand it into
			// one host candidate per machine address (enumerated once at Start); the client's own
			// ICE checks pick the reachable one. The port mapping then ADDS the public reflexive
			// candidate beside them (Apply's own contract), so local, LAN and forwarded clients all
			// find a live pair in the same answer with no per-client guessing here.
			answerSdp = ExpandHostCandidates(answerSdp, _localAddresses);
			answerSdp = PortMapping.Apply(answerSdp);

			// Signed last: a client that finds no valid a=identity refuses the connection outright.
			return NetherNetIdentityAssertion.AddServerAssertionTo(
				answerSdp, ServerIdentity.Key, ServerIdentity.Domain, ServerIdentity.Issuer);
		}

		/// <summary>
		///     Replaces the answer's single wildcard host candidate with one host candidate per local
		///     address, all on the same muxed port. Foundations are distinct (ICE treats candidates
		///     sharing one as the same base) and priorities descend in enumeration order, which only
		///     breaks ties: the client's connectivity checks, not our ordering, decide the pair.
		/// </summary>
		private static string ExpandHostCandidates(string sdp, IReadOnlyList<IPAddress> addresses)
		{
			var lines = new List<string>();
			foreach (string raw in sdp.Split('\n'))
			{
				string line = raw.TrimEnd('\r');

				if (!line.StartsWith("a=candidate:", StringComparison.Ordinal))
				{
					lines.Add(line);
					continue;
				}

				// a=candidate:<foundation> <component> <transport> <priority> <ip> <port> typ host ...
				string[] parts = line.Split(' ');
				if (parts.Length < 8 || !IPAddress.Any.ToString().Equals(parts[4], StringComparison.Ordinal))
				{
					lines.Add(line);
					continue;
				}

				string port = parts[5];
				for (int i = 0; i < addresses.Count; i++)
				{
					// Host type preference 126 per RFC 8445 5.1.2.1; the local preference descends
					// so every candidate's priority is unique.
					long priority = (126L << 24) | ((65535L - (uint) i) << 8) | 255L;
					lines.Add($"a=candidate:{i + 1} 1 udp {priority} {addresses[i]} {port} typ host generation 0");
				}
			}

			return string.Join("\r\n", lines);
		}

		private NetherNetSession AttachSession(string networkId, RtcPeer peer, RtcDataChannel reliable, RtcDataChannel unreliable, string signalingHost, int signalingPort)
		{
			// A returning client reuses its NetworkID, so an entry here is a ghost of a dead
			// connection. Close it rather than overwrite it: overwritten, it would no longer be
			// swept and its RtcPeer would never dispose.
			if (Sessions.TryRemove(networkId, out NetherNetSession stale))
			{
				Log.Warn($"NetherNet session for {networkId} replaced by a new connection, closing the old one");
				stale.Close();
			}

			// The ICE-nominated remote endpoint has no accessor on RtcPeer today, unlike the SIPSorcery
			// path this replaces (peerConnection.AudioDestinationEndPoint); GetClientEndPoint() on this
			// session is therefore not the client's real address yet. Logged, not hidden.
			var session = new NetherNetSession(peer, reliable, unreliable, new IPEndPoint(IPAddress.Any, 0), networkId, signalingHost, signalingPort);
			session.CustomMessageHandler = CustomMessageHandlerFactory?.Invoke(session) ?? new DefaultMessageHandler();
			session.OnClosed = closed => Sessions.TryRemove(new KeyValuePair<string, NetherNetSession>(closed.NetworkId, closed));

			Sessions[networkId] = session;

			Log.Info($"NetherNet session accepted from {networkId} via {signalingHost ?? "(none)"}:{signalingPort}");
			session.CustomMessageHandler.Connected();

			return session;
		}

		/// <summary>
		///     Removes the session level a=identity line, returning it separately so a caller that
		///     wants to authenticate can, while the SDP handed to WebRTC stays parseable.
		/// </summary>
		public static string StripIdentity(string sdp, out string assertion)
		{
			assertion = null;

			var kept = new StringBuilder(sdp.Length);
			foreach (string line in sdp.Split('\n'))
			{
				string trimmed = line.TrimEnd('\r');
				if (trimmed.StartsWith("a=identity:", StringComparison.Ordinal))
				{
					assertion = trimmed.Substring("a=identity:".Length);
					continue;
				}

				kept.Append(trimmed).Append("\r\n");
			}

			return kept.ToString();
		}

		private static async Task<(string method, string path, string headers, string body)> ReadRequest(Stream stream)
		{
			var buffer = new byte[64 * 1024];
			var raw = new StringBuilder();
			int contentLength = 0;
			int headerEnd = -1;

			while (true)
			{
				int read;
				try
				{
					read = await stream.ReadAsync(buffer);
				}
				catch (IOException) when (raw.Length > 0)
				{
					// A peer that resets mid-request has still told us something. Losing those bytes
					// to the exception is what made this look like no contact at all.
					Log.Warn($"NetherNet signaling connection reset after {raw.Length} bytes:\n{raw}");
					throw;
				}

				if (read == 0) break;

				raw.Append(Encoding.UTF8.GetString(buffer, 0, read));
				string text = raw.ToString();

				if (headerEnd < 0)
				{
					headerEnd = text.IndexOf("\r\n\r\n", StringComparison.Ordinal);
					if (headerEnd < 0) continue;

					Match length = Regex.Match(text.Substring(0, headerEnd), @"Content-Length:\s*(\d+)", RegexOptions.IgnoreCase);
					contentLength = length.Success ? int.Parse(length.Groups[1].Value) : 0;
				}

				if (text.Length - (headerEnd + 4) >= contentLength) break;
			}

			string request = raw.ToString();

			// Whatever arrived is returned even when it is not a request we can parse, because a
			// client that gives up mid-handshake leaves nothing else to look at.
			if (headerEnd < 0) return (null, null, request, null);

			string headers = request.Substring(0, headerEnd);
			string[] requestLine = headers.Substring(0, headers.IndexOf("\r\n", StringComparison.Ordinal)).Split(' ');

			return (requestLine[0], requestLine.Length > 1 ? requestLine[1] : "", headers, request.Substring(headerEnd + 4));
		}

		private static async Task Respond(Stream stream, int status, string contentType, string body)
		{
			byte[] payload = Encoding.UTF8.GetBytes(body);
			string head = $"HTTP/1.1 {status} {(status == 200 ? "OK" : status == 404 ? "Not Found" : "Bad Request")}\r\n"
						+ $"Content-Type: {contentType}\r\n"
						+ $"Content-Length: {payload.Length}\r\n"
						+ "Connection: close\r\n\r\n";

			Log.Info($"NetherNet signaling >>> {status}{Environment.NewLine}{head}{body}");

			await stream.WriteAsync(Encoding.UTF8.GetBytes(head));
			await stream.WriteAsync(payload);
			await stream.FlushAsync();
		}
	}
}
