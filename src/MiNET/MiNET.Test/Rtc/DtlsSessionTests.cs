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
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net.Rtc;
using MiNET.Net.Rtc.FastDtls;

namespace MiNET.Test.Rtc
{
	// Counts GC allocations across a whole exchange; any neighbour allocating on another worker
	// lands in the count.
	[TestClass, DoNotParallelize]
	public class DtlsSessionTests
	{
		private const byte HandshakeContentType = 22;
		private const byte ChangeCipherSpecContentType = 20;

		[TestMethod]
		public void Fingerprint_IsStable_AndFormatted()
		{
			using var certificate = RtcCertificate.CreateSelfSigned();
			StringAssert.Matches(certificate.FingerprintSha256, new Regex("^([0-9A-F]{2}:){31}[0-9A-F]{2}$"));
		}

		/// <summary>
		///     Pins the coupling <see cref="DtlsSession.MaxSendPayloadLength" />'s own comment names: the
		///     DTLS layer must accept anything SCTP can ever hand it, since every <see cref="SctpAssociation" />
		///     send path builds into an <see cref="SctpPacket.MaxSize" /> buffer. If this ever shrank below
		///     that, a legitimate full-size SCTP packet would start throwing out of
		///     <see cref="DtlsSession.SendApplicationData" /> instead of being sent.
		/// </summary>
		[TestMethod]
		public void MaxSendPayloadLength_IsAtLeastTheSctpPacketCeiling()
		{
			Assert.IsTrue(DtlsSession.MaxSendPayloadLength >= SctpPacket.MaxSize);
		}

		[TestMethod]
		public async Task Handshake_Completes_AndCarriesApplicationData()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			// Loopback wiring without ICE or sockets: each session's sendToWire feeds the other.
			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => received.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {1, 2, 3, 4});
			CollectionAssert.AreEqual(new byte[] {1, 2, 3, 4}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));
		}

		/// <summary>
		///     One <see cref="RtcCertificate" /> shared across many peers is the normal WebRTC shape for a
		///     server: it negotiates a fresh <see cref="DtlsSession" /> per peer from the same identity.
		///     <see cref="DtlsSession.Dispose" /> must never dispose that shared certificate - if it did,
		///     the first peer disconnecting would leave every other session still using the same
		///     certificate unable to sign its own ServerKeyExchange/CertificateVerify, throwing
		///     <see cref="System.ObjectDisposedException" /> the next time it tried.
		/// </summary>
		[TestMethod]
		public async Task Dispose_NeverDisposesTheSharedCertificate_ASecondSessionOverTheSameCertificateStillCompletes()
		{
			using var sharedServerCert = RtcCertificate.CreateSelfSigned();
			using var firstClientCert = RtcCertificate.CreateSelfSigned();
			using var secondClientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession firstServer = null, firstClient = null;
			firstServer = new DtlsSession(sharedServerCert, firstClientCert.FingerprintSha256, isServer: true, bytes => firstClient.FeedDatagram(bytes));
			firstClient = new DtlsSession(firstClientCert, sharedServerCert.FingerprintSha256, isServer: false, bytes => firstServer.FeedDatagram(bytes));

			Task<bool> firstServerDone = firstServer.DoHandshakeAsync(CancellationToken.None);
			Task<bool> firstClientDone = firstClient.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await firstClientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await firstServerDone.WaitAsync(TimeSpan.FromSeconds(15)));

			firstServer.Dispose();
			firstClient.Dispose();

			// A second, later peer over the SAME shared certificate must still be able to complete its
			// own handshake.
			DtlsSession secondServer = null, secondClient = null;
			secondServer = new DtlsSession(sharedServerCert, secondClientCert.FingerprintSha256, isServer: true, bytes => secondClient.FeedDatagram(bytes));
			secondClient = new DtlsSession(secondClientCert, sharedServerCert.FingerprintSha256, isServer: false, bytes => secondServer.FeedDatagram(bytes));

			Task<bool> secondServerDone = secondServer.DoHandshakeAsync(CancellationToken.None);
			Task<bool> secondClientDone = secondClient.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await secondClientDone.WaitAsync(TimeSpan.FromSeconds(15)), "expected the second peer's handshake to complete despite the first session, sharing the same certificate, already being disposed");
			Assert.IsTrue(await secondServerDone.WaitAsync(TimeSpan.FromSeconds(15)));

			secondServer.Dispose();
			secondClient.Dispose();
		}

		/// <summary>
		///     Pins that the key block <see cref="DtlsSession.CapturedKeys" /> copies out of the
		///     handshake engine is the actual material the engine negotiated, not merely present. Both
		///     sides derive the same key block from the same master secret, so the two copies must agree
		///     field for field; and a manual AES-GCM decrypt of one wire datagram the engine itself
		///     encrypted (its own Finished flight), built entirely from the DTLS 1.2 record format
		///     (record header, explicit nonce, AAD per RFC 5246/RFC 5288) and the captured client write
		///     key/salt, must recover the exact plaintext that was sent.
		/// </summary>
		[TestMethod]
		public async Task Handshake_NegotiatesTheKeyBlock_AndItDecryptsARealRecord()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			byte[] lastClientToServer = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				lastClientToServer = bytes.ToArray();
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			DtlsNegotiatedKeys clientSideKeys = client.CapturedKeys;
			DtlsNegotiatedKeys serverSideKeys = server.CapturedKeys;
			Assert.IsNotNull(clientSideKeys, "expected the client-role handshake to have captured a key block");
			Assert.IsNotNull(serverSideKeys, "expected the server-role handshake to have captured a key block");

			Assert.AreEqual(16, clientSideKeys.ClientWriteKey.Length, "the engine negotiates only TLS_ECDHE_ECDSA_WITH_AES_128_GCM_SHA256");
			Assert.AreEqual(clientSideKeys.ClientWriteKey.Length, clientSideKeys.ServerWriteKey.Length);
			Assert.AreEqual(4, clientSideKeys.ClientWriteSalt.Length);
			Assert.AreEqual(4, clientSideKeys.ServerWriteSalt.Length);

			CollectionAssert.AreEqual(clientSideKeys.ClientWriteKey, serverSideKeys.ClientWriteKey, "both sides derive the same key block from the same master secret");
			CollectionAssert.AreEqual(clientSideKeys.ServerWriteKey, serverSideKeys.ServerWriteKey);
			CollectionAssert.AreEqual(clientSideKeys.ClientWriteSalt, serverSideKeys.ClientWriteSalt);
			CollectionAssert.AreEqual(clientSideKeys.ServerWriteSalt, serverSideKeys.ServerWriteSalt);

			byte[] plaintext = {10, 20, 30, 40, 50};
			client.SendApplicationData(plaintext);
			Assert.IsNotNull(lastClientToServer, "expected to have captured the wire datagram the record layer encrypted");

			// Recovered independently of client.RecordCrypto: proves the key material itself, not just
			// that our own code round-trips with itself.
			byte[] recovered = ManuallyDecryptOneAeadRecord(lastClientToServer, clientSideKeys.ClientWriteSalt, clientSideKeys.ClientWriteKey);
			CollectionAssert.AreEqual(plaintext, recovered);

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     Decodes exactly one DTLS 1.2 AEAD record: header
		///     type(1)|version(2)|epoch(2)|sequence(6)|length(2) (RFC 6347 4.1), fragment
		///     explicit_nonce(8)|ciphertext|tag(16), GCM nonce = salt(4) + explicit_nonce(8), AAD =
		///     epoch(2)|sequence(6)|type(1)|version(2)|plaintext_length(2) (RFC 5246 6.2.3.3, RFC 5288).
		///     The explicit nonce is read off the wire, never reconstructed from the header: RFC 5288
		///     requires a receiver to use the nonce as sent, not to recompute it.
		/// </summary>
		private static byte[] ManuallyDecryptOneAeadRecord(byte[] record, byte[] writeSalt, byte[] writeKey)
		{
			byte contentType = record[0];
			Assert.AreEqual(0xFE, record[1], "DTLS 1.2 record version high byte");
			Assert.AreEqual(0xFD, record[2], "DTLS 1.2 record version low byte");

			int fragmentLength = (record[11] << 8) | record[12];
			ReadOnlySpan<byte> fragment = record.AsSpan(13, fragmentLength);
			ReadOnlySpan<byte> explicitNonce = fragment.Slice(0, 8);
			ReadOnlySpan<byte> ciphertext = fragment.Slice(8, fragment.Length - 8 - 16);
			ReadOnlySpan<byte> tag = fragment.Slice(fragment.Length - 16, 16);

			Span<byte> nonce = stackalloc byte[12];
			writeSalt.CopyTo(nonce);
			explicitNonce.CopyTo(nonce.Slice(4));

			Span<byte> aad = stackalloc byte[13];
			record.AsSpan(3, 8).CopyTo(aad); // epoch(2) + sequence(6), straight off the record header
			aad[8] = contentType;
			aad[9] = record[1];
			aad[10] = record[2];
			aad[11] = (byte) (ciphertext.Length >> 8);
			aad[12] = (byte) ciphertext.Length;

			byte[] plaintext = new byte[ciphertext.Length];
			using var aesGcm = new AesGcm(writeKey, 16);
			aesGcm.Decrypt(nonce, ciphertext, tag, plaintext, aad);
			return plaintext;
		}

		/// <summary>
		///     <see cref="DtlsSession.CapturedKeys" /> holds plaintext copies of both write keys and salts
		///     for the session's whole lifetime; <see cref="DtlsSession.Dispose" /> zeroes them. The
		///     properties keep returning the same array instances afterward - only their contents change.
		/// </summary>
		[TestMethod]
		public async Task Dispose_ZeroesTheCapturedKeyMaterial()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			DtlsNegotiatedKeys keys = server.CapturedKeys;
			Assert.IsTrue(keys.ClientWriteKey.Any(b => b != 0), "sanity: expected real key material before dispose");
			Assert.IsTrue(keys.ServerWriteKey.Any(b => b != 0), "sanity: expected real key material before dispose");

			server.Dispose();

			CollectionAssert.AreEqual(new byte[keys.ClientWriteKey.Length], keys.ClientWriteKey, "expected the client write key to be zeroed on dispose");
			CollectionAssert.AreEqual(new byte[keys.ServerWriteKey.Length], keys.ServerWriteKey, "expected the server write key to be zeroed on dispose");
			CollectionAssert.AreEqual(new byte[keys.ClientWriteSalt.Length], keys.ClientWriteSalt, "expected the client write salt to be zeroed on dispose");
			CollectionAssert.AreEqual(new byte[keys.ServerWriteSalt.Length], keys.ServerWriteSalt, "expected the server write salt to be zeroed on dispose");

			client.Dispose();
		}

		[TestMethod]
		public async Task WrongFingerprint_FailsTheHandshake()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();
			using var imposter = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, imposter.FingerprintSha256, false, bytes => server.FeedDatagram(bytes));

			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsFalse(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.AreEqual(1L, client.HandshakeFailures, "expected the fingerprint mismatch to be counted as a handshake failure");
		}

		/// <summary>
		///     A rejected peer must actually be told why: <see cref="DtlsEngine.Abort" /> puts a fatal
		///     alert (bad_certificate) on the wire before throwing, and that alert must reach
		///     <see cref="WireSender" />, not just get queued and then silently dropped once the session
		///     closes behind it. Without this, a peer we reject burns its own full retransmission timeout
		///     instead of failing fast on the alert.
		/// </summary>
		[TestMethod]
		public async Task WrongFingerprint_EmitsAFatalAlertOnTheWire_BeforeClosing()
		{
			const byte AlertContentType = 21;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();
			using var imposter = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var clientSentDatagrams = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, imposter.FingerprintSha256, false, bytes =>
			{
				clientSentDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsFalse(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));

			bool sawAlert = clientSentDatagrams.Any(d => d.Length > 0 && d[0] == AlertContentType);
			Assert.IsTrue(sawAlert, "expected the client to have put a fatal alert (bad_certificate) on the wire before closing, so a rejected peer is told why instead of silently timing out");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     A well-formed-length ClientKeyExchange whose 65-byte point is not actually on P-256 (only
		///     its length and leading 0x04 tag are checked before the point reaches ECDH point
		///     validation) must never escape <see cref="DtlsSession.FeedDatagram" /> as a raw platform
		///     exception: the receive path treats any handshake failure as normal, adversarial network
		///     life, not a crash. Built by intercepting a real client's own ClientKeyExchange record (so
		///     every other field - lengths, transcript, message sequence - is genuinely valid) and
		///     flipping one byte inside its X coordinate: astronomically unlikely to coincidentally land
		///     back on the curve, so the tampered point is off-curve with overwhelming probability.
		/// </summary>
		[TestMethod]
		public async Task MalformedClientKeyExchange_OffCurvePoint_NeverEscapesFeedDatagram_ClosesAndCountsFailure()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			byte[] tamperedFlight = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				// Intercept the flight carrying ClientKeyExchange instead of delivering it: every earlier
				// datagram (the two ClientHellos) is unaffected and delivered normally, so the handshake
				// reaches this exact point genuinely, through the real protocol.
				if (TryTamperClientKeyExchangePoint(bytes, out byte[] tampered))
				{
					tamperedFlight = tampered;
					return;
				}
				server.FeedDatagram(bytes);
			});

			_ = server.DoHandshakeAsync(CancellationToken.None);
			_ = client.DoHandshakeAsync(CancellationToken.None);

			Assert.IsNotNull(tamperedFlight, "expected to have intercepted the client's ClientKeyExchange-carrying flight");

			long failuresBefore = server.HandshakeFailures;

			try
			{
				server.FeedDatagram(tamperedFlight);
			}
			catch (Exception ex)
			{
				Assert.Fail($"expected no exception to escape FeedDatagram; got {ex.GetType().Name}: {ex.Message}");
			}

			Assert.AreEqual(failuresBefore + 1, server.HandshakeFailures, "expected the off-curve point to be counted as a handshake failure");
			Assert.IsTrue(server.IsClosed, "expected the session to close rather than wedge");

			client.Dispose();
			server.Dispose();
		}

		/// <summary>
		///     Walks <paramref name="datagram" />'s records looking for an epoch-0 Handshake record whose
		///     handshake type is ClientKeyExchange (16); if found, flips one byte inside the embedded
		///     65-byte point's X coordinate (offset: record header 13 + handshake header 12 + 1-byte
		///     point-length prefix, then +1 into the point itself, past the fixed 0x04 uncompressed tag)
		///     and returns the whole datagram, tampered, byte-identical everywhere else.
		/// </summary>
		private static bool TryTamperClientKeyExchangePoint(ReadOnlySpan<byte> datagram, out byte[] tampered)
		{
			const byte HandshakeContentType = 22;
			const byte ClientKeyExchangeType = 16;

			byte[] result = datagram.ToArray();
			int offset = 0;
			bool found = false;
			while (offset < result.Length)
			{
				if (!DtlsRecordCrypto.TryReadRecordHeader(result.AsSpan(offset), out byte contentType, out int epoch, out int fragmentLength)) break;

				if (epoch == 0 && contentType == HandshakeContentType && fragmentLength >= 12 + 1 + 65)
				{
					byte handshakeType = result[offset + DtlsRecordCrypto.HeaderLength];
					if (handshakeType == ClientKeyExchangeType)
					{
						int pointOffset = offset + DtlsRecordCrypto.HeaderLength + 12 + 1;
						result[pointOffset + 1] ^= 0xFF;
						found = true;
					}
				}

				offset += DtlsRecordCrypto.HeaderLength + fragmentLength;
			}

			tampered = found ? result : null;
			return found;
		}

		/// <summary>
		///     Replaying an already-seen ciphertext datagram at the native, post-handshake record layer
		///     must be rejected by the anti-replay window (RFC 6347 4.1.2.6) without taking the session
		///     down or losing anything: <see cref="DtlsSession.FeedDatagram" /> walks the record, finds
		///     the decrypt rejected, drops and counts it, and returns - a straight-line function with no
		///     retry loop of its own to livelock on - and the session must still carry legitimate
		///     application data afterward.
		/// </summary>
		[TestMethod]
		public async Task ReplayedRecord_IsDiscarded_AndSessionKeepsWorking()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			byte[] lastClientToServer = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				lastClientToServer = bytes.ToArray();
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			var firstReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => firstReceived.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {9, 9, 9});
			CollectionAssert.AreEqual(new byte[] {9, 9, 9}, await firstReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)));
			Assert.IsNotNull(lastClientToServer, "expected to have captured the wire datagram carrying the application data");

			byte[] replay = lastClientToServer;
			await Task.Run(() => server.FeedDatagram(replay)).WaitAsync(TimeSpan.FromSeconds(2));

			var secondReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => secondReceived.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {4, 5, 6});
			CollectionAssert.AreEqual(new byte[] {4, 5, 6}, await secondReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     Pins that the <see cref="CancellationToken" /> passed to <see cref="DtlsSession.DoHandshakeAsync" />
		///     resolves the handshake false even when the peer never answers at all: unlike a blocking
		///     handshake driver, the engine here never occupies a thread waiting for a reply, so
		///     cancellation has nothing in-flight to interrupt - it only needs to resolve the pending
		///     result, which happens as soon as the registered callback runs.
		/// </summary>
		[TestMethod]
		public async Task Cancelling_TheHandshake_ResolvesFalse_WithNoPeerEverAnswering()
		{
			using var clientCert = RtcCertificate.CreateSelfSigned();
			using var serverCert = RtcCertificate.CreateSelfSigned();

			// Nobody on the other end: sendToWire goes nowhere, so the engine's first flight is simply
			// never answered.
			using var client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, _ => { });

			using var cts = new CancellationTokenSource();
			Task<bool> handshake = client.DoHandshakeAsync(cts.Token);
			cts.CancelAfter(TimeSpan.FromMilliseconds(300));

			Assert.IsFalse(await handshake.WaitAsync(TimeSpan.FromSeconds(3)));
		}

		/// <summary>
		///     A second call to <see cref="DtlsSession.DoHandshakeAsync" /> must not reach
		///     <see cref="DtlsEngine.Start" /> a second time: the engine itself throws
		///     <see cref="InvalidOperationException" /> ("Already started.") on that, an internal detail a
		///     caller of this class should never see. Guarded at the session boundary by returning the
		///     same task instead.
		/// </summary>
		[TestMethod]
		public async Task DoHandshakeAsync_CalledTwice_ReturnsTheSameTask_NeverReenteringTheEngine()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientFirst = client.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientSecond = client.DoHandshakeAsync(CancellationToken.None);

			Assert.AreSame(clientFirst, clientSecond, "expected a second call to return the same handshake task rather than re-entering the engine");
			Assert.IsTrue(await clientFirst.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     Tearing the session down as soon as an application-data
		///     record signals an abort - calling Dispose synchronously from inside an
		///     <see cref="DtlsSession.OnDecrypted" /> subscriber, which is still further up the same
		///     call stack as FeedDatagram's drain loop - is a realistic pattern (a Monitor lock is
		///     reentrant on the owning thread, so the lock alone does not stop this). Without the
		///     deferred-return guard, DrainPending's `while`
		///     loop would call ReceivePending against the scratch buffer after Dispose had already
		///     returned it to the pool.
		/// </summary>
		[TestMethod]
		public async Task DisposeFromWithinOnDecrypted_DoesNotCorruptTheScratchBuffer()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload =>
			{
				received.TrySetResult(payload.ToArray());
				server.Dispose(); // Reentrant: FeedDatagram is still on the stack, holding _gate.
			};

			// Must not throw: this exercises the exact reentrant path.
			client.SendApplicationData(new byte[] {7, 7, 7});
			CollectionAssert.AreEqual(new byte[] {7, 7, 7}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			// Pool sanity, best effort: renting immediately after must succeed cleanly. This cannot
			// prove an unrelated consumer elsewhere in the process was not corrupted, but a double
			// return or a still-in-use buffer handed back early is exactly the kind of bug that tends
			// to surface as a failure on the very next rent from the same shared pool.
			byte[] probe = ArrayPool<byte>.Shared.Rent(4096);
			ArrayPool<byte>.Shared.Return(probe);

			// The session no longer delivers: FeedDatagram on a disposed session is a no-op.
			bool deliveredAfterDispose = false;
			server.OnDecrypted += _ => deliveredAfterDispose = true;
			client.SendApplicationData(new byte[] {8, 8, 8});
			await Task.Delay(200);
			Assert.IsFalse(deliveredAfterDispose);

			client.Dispose();
		}

		/// <summary>
		///     A caller racing an application send against
		///     <see cref="DtlsSession.Dispose" /> is a benign, expected race this class's teardown design
		///     tolerates (unlike the pre-handshake case above, a caller bug, which throws). Must not throw,
		///     must not touch the wire, once disposed.
		/// </summary>
		[TestMethod]
		public async Task SendApplicationData_AfterDispose_IsSilentlyDropped_DoesNotThrow()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			bool clientDisposed = false;
			bool clientSentAnythingAfterDispose = false;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				if (clientDisposed) clientSentAnythingAfterDispose = true;
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			client.Dispose();
			clientDisposed = true;

			// Must not throw (silently dropped, exactly like the already-covered "no delivery after
			// Dispose" case above, just exercised directly against the disposed side's own send path
			// rather than the peer's receive path) AND must never reach the wire.
			client.SendApplicationData(new byte[] {1, 2, 3});

			Assert.IsFalse(clientSentAnythingAfterDispose, "SendApplicationData must not touch the wire at all once disposed");

			server.Dispose();
		}

		/// <summary>
		///     Pins concurrent <see cref="DtlsSession.FeedDatagram" /> safety under the direct-decrypt
		///     design: with the inbound queue gone, a losing concurrent arrival is no longer buffered for
		///     later delivery, it is dropped and counted by the same reentrancy guard a same-thread
		///     callback reentry hits (see <see cref="FeedDatagram_ReenteredFromWithinOnDecrypted_IsDroppedAndCounted_OuterDeliveryStillCompletesIntact" />).
		///     What must still hold, regardless of how the two threads below interleave, is the
		///     accounting identity: every fed datagram is either delivered exactly once, with its genuine
		///     payload, or dropped and counted - never both, never neither, and never a corrupted payload.
		///     Two threads, synchronized with a <see cref="Barrier" /> to maximise actual overlap, each
		///     feed a disjoint half of a batch of distinct, never-before-delivered wire datagrams (captured
		///     up front rather than replayed, since a genuine replay is deliberately discarded by the
		///     anti-replay window, a different code path already covered by
		///     <see cref="ReplayedRecord_IsDiscarded_AndSessionKeepsWorking" />).
		/// </summary>
		[TestMethod]
		public async Task ConcurrentFeedDatagram_NeverCorrupts_EveryDatagramIsEitherDeliveredOnceOrSafelyDropped()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			bool captureOnly = false;
			var captured = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				if (captureOnly)
				{
					captured.Add(bytes.ToArray());
				}
				else
				{
					server.FeedDatagram(bytes);
				}
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// Produce 2 * perThread distinct, valid, never-yet-delivered wire datagrams: each carries
			// its own DTLS sequence number, so the anti-replay window accepts all of them regardless of
			// the order the two threads below happen to feed them in.
			const int perThread = 25;
			var payloads = new List<byte[]>();
			captureOnly = true;
			for (int i = 0; i < perThread * 2; i++)
			{
				byte[] payload = {(byte) i, (byte) (i >> 8), 0xAA};
				payloads.Add(payload);
				client.SendApplicationData(payload);
			}
			captureOnly = false;
			Assert.AreEqual(perThread * 2, captured.Count, "expected to have captured one wire datagram per SendApplicationData call");

			var receivedPayloads = new ConcurrentBag<byte[]>();
			server.OnDecrypted += payload => receivedPayloads.Add(payload.ToArray());

			using var barrier = new Barrier(2);

			void Feed(int startIndex)
			{
				barrier.SignalAndWait();
				for (int i = 0; i < perThread; i++)
				{
					server.FeedDatagram(captured[startIndex + i]);
				}
			}

			Task t1 = Task.Run(() => Feed(0));
			Task t2 = Task.Run(() => Feed(perThread));
			await Task.WhenAll(t1, t2).WaitAsync(TimeSpan.FromSeconds(10));

			// Every delivered payload must be one of the genuine ones sent, and never delivered twice:
			// the only corruption this design could still produce.
			HashSet<string> validHex = payloads.Select(Convert.ToHexString).ToHashSet();
			List<string> deliveredHex = receivedPayloads.Select(Convert.ToHexString).ToList();
			Assert.IsTrue(deliveredHex.All(validHex.Contains), "expected every delivered payload to be one of the genuine datagrams sent, never a corrupted one");
			Assert.AreEqual(deliveredHex.Count, deliveredHex.Distinct().Count(), "expected no payload to be delivered more than once");

			// Every datagram that was not delivered must be accounted for as a reentrancy-guard drop,
			// not silently lost some other way.
			Assert.AreEqual(perThread * 2, receivedPayloads.Count + server.ReentrantFeedsDropped, "expected every fed datagram to be either delivered exactly once or dropped and counted - never lost silently");

			// The session is still fully alive for a genuinely new record afterward.
			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => received.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {9, 9, 9});
			CollectionAssert.AreEqual(new byte[] {9, 9, 9}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     Once the handshake is done, the record layer is native on both sides for the rest of the
		///     session. Both sessions exercise it at once, in both directions, well past the 64-wide
		///     replay window and the low sequence numbers the handshake's own Finished flight used.
		/// </summary>
		[TestMethod]
		public async Task NativeRecordLayer_ExchangesOneThousandDatagramsEachWay_AllDeliveredIntact()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			var serverReceived = new List<byte[]>();
			var clientReceived = new List<byte[]>();
			server.OnDecrypted += payload => serverReceived.Add(payload.ToArray());
			client.OnDecrypted += payload => clientReceived.Add(payload.ToArray());

			const int count = 1000;
			for (int i = 0; i < count; i++)
			{
				client.SendApplicationData(new byte[] {(byte) i, (byte) (i >> 8), 0xAB});
			}
			for (int i = 0; i < count; i++)
			{
				server.SendApplicationData(new byte[] {(byte) i, (byte) (i >> 8), 0xCD});
			}

			Assert.AreEqual(count, serverReceived.Count, "expected every one of the 1000 client->server datagrams to be delivered");
			Assert.AreEqual(count, clientReceived.Count, "expected every one of the 1000 server->client datagrams to be delivered");
			for (int i = 0; i < count; i++)
			{
				CollectionAssert.AreEqual(new byte[] {(byte) i, (byte) (i >> 8), 0xAB}, serverReceived[i]);
				CollectionAssert.AreEqual(new byte[] {(byte) i, (byte) (i >> 8), 0xCD}, clientReceived[i]);
			}

			Assert.AreEqual(0L, server.RecordCrypto.DecryptFailures);
			Assert.AreEqual(0L, server.RecordCrypto.ReplayDrops);
			Assert.AreEqual(0L, client.RecordCrypto.DecryptFailures);
			Assert.AreEqual(0L, client.RecordCrypto.ReplayDrops);

			server.Dispose();
			client.Dispose();
		}

		/// <summary>Finds the last datagram in <paramref name="datagrams" /> whose first record declares epoch 0: for a live handshake capture, the peer's own ChangeCipherSpec, the shape a real final-flight retransmission carries at its front.</summary>
		private static byte[] FindLastEpochZeroDatagram(List<byte[]> datagrams)
		{
			return datagrams.Last(d => DtlsRecordCrypto.TryReadRecordHeader(d, out _, out int epoch, out _) && epoch == 0);
		}

		/// <summary>
		///     After a completed server-role handshake, an epoch-0 record (a peer retransmitting a final
		///     handshake flight it believes was lost) must make the handshake engine rebuild and re-send
		///     its own last flight - fresh epoch-1 sequences, not a byte-identical replay of what the wire
		///     originally observed, since a retransmission that reused a sequence the record layer might
		///     already have consumed for application data would be a nonce reuse under the shared
		///     AES-GCM key.
		/// </summary>
		[TestMethod]
		public async Task EpochZeroRecord_TriggersAnEngineResendOfItsLastFlight_WithFreshEpoch1Sequences()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var serverToClientDatagrams = new List<byte[]>();
			var clientToServerDatagrams = new List<byte[]>();
			bool deliverServerSendsToClient = true;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				serverToClientDatagrams.Add(bytes.ToArray());
				if (deliverServerSendsToClient) client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				clientToServerDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			byte[] triggerDatagram = FindLastEpochZeroDatagram(clientToServerDatagrams);

			ulong sequenceBeforeResend = server.RecordCrypto.NextSendSequence;
			long resendsBefore = server.ResendsPerformed;
			int preTriggerCount = serverToClientDatagrams.Count;

			// Undelivered for the trigger itself: a client that actually received this resend would
			// (correctly, per its own identical epoch-0 handling) answer with its own resend right
			// back, which would confuse this assertion's own counting. That cross-session cascade is
			// proven, deliberately, by EpochZeroRecord_ClientRole_... below; this test is about the
			// server's own resend in isolation.
			deliverServerSendsToClient = false;
			server.FeedDatagram(triggerDatagram);
			deliverServerSendsToClient = true;

			Assert.AreEqual(resendsBefore + 1, server.ResendsPerformed);
			List<byte[]> resent = serverToClientDatagrams.Skip(preTriggerCount).ToList();
			Assert.IsTrue(resent.Count > 0, "expected the resend to actually put at least one datagram on the wire");

			// Small handshake messages (this profile's certificate, key exchange, and verify data) all
			// fit comfortably under one MTU, so the engine coalesces the whole flight - ChangeCipherSpec
			// and Finished included - into as few datagrams as fit, potentially just one: every record
			// in every resent datagram must be walked, not just each datagram's first.
			bool sawEpoch1Record = false;
			foreach (byte[] datagram in resent)
			{
				int offset = 0;
				while (offset < datagram.Length)
				{
					Assert.IsTrue(DtlsRecordCrypto.TryReadRecordHeader(datagram.AsSpan(offset), out byte contentType, out int epoch, out int fragmentLength), "expected every record in a resent datagram to have a well-formed header");
					Assert.IsTrue(contentType == HandshakeContentType || contentType == ChangeCipherSpecContentType, $"expected a Handshake or ChangeCipherSpec record, got content type {contentType}");

					if (epoch == 1)
					{
						sawEpoch1Record = true;
						ulong sequence = ReadSequence(datagram.AsSpan(offset));
						Assert.IsTrue(sequence >= sequenceBeforeResend, "expected every epoch-1 sequence in the resend to be at or above the record layer's own counter at the moment of the resend");
					}

					offset += DtlsRecordCrypto.HeaderLength + fragmentLength;
				}
			}
			Assert.IsTrue(sawEpoch1Record, "expected the server's last flight (Finished, at epoch 1) to be part of the resend");

			// The record layer itself must still work normally afterward, continuing forward from
			// wherever the resend left its own counter.
			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			client.OnDecrypted += payload => received.TrySetResult(payload.ToArray());
			server.SendApplicationData(new byte[] {1, 2, 3});
			CollectionAssert.AreEqual(new byte[] {1, 2, 3}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     Two epoch-0 triggers inside the same 1-second window must produce exactly one resend; only
		///     advancing the clock seam (<see cref="DtlsSession.ClockNowMillis" />) past that window
		///     re-arms it. Fully clock-driven, no real wall-clock wait.
		/// </summary>
		[TestMethod]
		public async Task EpochZeroRecord_RateLimitsResends_ToAtMostOnePerSecond_AdvancingTheClockReArms()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var clientToServerDatagrams = new List<byte[]>();
			bool deliverServerSendsToClient = true;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				if (deliverServerSendsToClient) client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				clientToServerDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			byte[] triggerDatagram = FindLastEpochZeroDatagram(clientToServerDatagrams);

			// Undelivered from here on: a client that actually received these resends would (correctly,
			// per its own identical epoch-0 handling) answer with resends of its own right back,
			// confusing this test's own counting of the SERVER's rate limit in isolation. See the
			// identical remark on EpochZeroRecord_TriggersAnEngineResendOfItsLastFlight_....
			deliverServerSendsToClient = false;

			long fakeNow = 1_000_000;
			server.ClockNowMillis = () => fakeNow;

			server.FeedDatagram(triggerDatagram);
			Assert.AreEqual(1L, server.ResendsPerformed, "expected the first trigger to resend");

			// The trigger datagram coalesces the client's whole second flight, so it can carry more
			// than one epoch-0 record (Certificate, ClientKeyExchange, and CertificateVerify are all
			// epoch 0 too, ahead of ChangeCipherSpec): only the first one produces the resend above,
			// every other one in that same datagram is already drop-and-counted by the once-per-datagram
			// guard, independent of the rate limit this test is actually about. Capture the count here,
			// after the one genuine resend, as the baseline the rate-limited second trigger is checked
			// against, rather than assuming a specific number of coalesced records.
			long droppedAfterFirstTrigger = server.EpochZeroRecordsDropped;

			server.FeedDatagram(triggerDatagram);
			Assert.AreEqual(1L, server.ResendsPerformed, "expected the second trigger inside the 1-second window to be rate-limited, not resent");
			Assert.IsTrue(server.EpochZeroRecordsDropped > droppedAfterFirstTrigger, "expected the rate-limited second trigger to drop at least one more record than the first trigger did");

			fakeNow += 1000; // exactly the boundary: "at least 1 second has passed" must re-arm here.
			server.FeedDatagram(triggerDatagram);
			Assert.AreEqual(2L, server.ResendsPerformed, "expected advancing the clock seam past the 1-second window to re-arm the resend");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     RFC 5246 7.2.1's "any data received after a closure alert is ignored" has a counterpart
		///     here: an epoch-0 record must not cost the peer its other, legitimate records in the same
		///     datagram. A coalesced datagram - the exact shape a real retransmitted final flight has
		///     (ChangeCipherSpec at epoch 0 immediately followed by Finished at epoch 1) - must still
		///     deliver whatever valid epoch-1 record follows the epoch-0 one.
		/// </summary>
		[TestMethod]
		public async Task CoalescedDatagram_EpochZeroRecordFollowedByValidRecord_DeliversTheValidRecordNormally()
		{
			const byte ApplicationDataContentType = 23;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var clientToServerDatagrams = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				clientToServerDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			byte[] epochZeroRecord = FindLastEpochZeroDatagram(clientToServerDatagrams);

			using var forger = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0);
			forger.SetSendSequenceForTesting(1);
			byte[] appPayload = {9, 9, 9};
			Span<byte> appWire = stackalloc byte[appPayload.Length + DtlsRecordCrypto.RecordOverhead];
			int appLength = forger.EncryptRecord(ApplicationDataContentType, appPayload, appWire);
			Assert.AreNotEqual(-1, appLength);

			byte[] datagram = new byte[epochZeroRecord.Length + appLength];
			epochZeroRecord.CopyTo(datagram, 0);
			appWire.Slice(0, appLength).CopyTo(datagram.AsSpan(epochZeroRecord.Length));

			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => received.TrySetResult(payload.ToArray());

			server.FeedDatagram(datagram);

			CollectionAssert.AreEqual(appPayload, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));
			Assert.AreEqual(1L, server.ResendsPerformed, "expected the epoch-0 record ahead of the valid one to still trigger its own resend");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     An epoch-0 header (13 bytes) declaring a fragment length that consumes the rest of the
		///     datagram as garbage, with nothing valid before or after it, is a junk-prefix attack shape:
		///     the routing decision reads an unauthenticated header, so anyone able to reach this
		///     session can prefix 13 junk bytes declaring epoch 0 to any datagram. That header alone must
		///     never cost more than a rate-limited resend, never throw, and never allocate once the rate
		///     limit is active - the rate-limit check itself runs before the engine is ever touched, so a
		///     rate-limited trigger never reaches the flight-rebuild allocation at all. A fixed fake clock
		///     (never advancing) makes every trigger past the first provably rate-limited, regardless of
		///     how fast or slow the machine running this test is.
		/// </summary>
		[TestMethod]
		public async Task JunkPrefixDatagram_EpochZeroHeaderThenGarbage_HandledFully_AtMostOneResend_ZeroAllocationOnTheDropPath()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			const int garbageLength = 32;
			byte[] junkDatagram = new byte[13 + garbageLength];
			junkDatagram[0] = 22; // an arbitrary content type; the epoch-0 path never reads it
			junkDatagram[1] = 0xFE;
			junkDatagram[2] = 0xFD;
			// epoch (bytes 3-4) left at 0; sequence (bytes 5-10) left at 0, neither read by the epoch-0 path.
			junkDatagram[11] = (byte) (garbageLength >> 8);
			junkDatagram[12] = (byte) garbageLength;
			for (int i = 13; i < junkDatagram.Length; i++) junkDatagram[i] = 0xAA;

			long fakeNow = 5000;
			server.ClockNowMillis = () => fakeNow;

			server.FeedDatagram(junkDatagram);
			Assert.AreEqual(1L, server.ResendsPerformed, "expected the first-ever trigger to resend");

			// Warm this exact path once more before measuring, then measure allocation across many more:
			// every one of these is rate-limited (same fakeNow), so this is the pure drop-and-count path.
			server.FeedDatagram(junkDatagram);

			const int iterations = 500;
			// Per-thread, not process-wide: FeedDatagram's drop path is synchronous on this thread,
			// and the class-parallel suite would otherwise fail this bracket on a neighbor's garbage.
			long before = GC.GetAllocatedBytesForCurrentThread();
			for (int i = 0; i < iterations; i++)
			{
				server.FeedDatagram(junkDatagram);
			}
			long after = GC.GetAllocatedBytesForCurrentThread();

			Assert.AreEqual(0L, after - before, "expected zero heap allocation on the rate-limited drop path");
			Assert.AreEqual(1L, server.ResendsPerformed, "expected at most one resend total across every trigger in this test");

			// The session is still fully alive: a genuinely new record still delivers.
			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => received.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {1, 2, 3});
			CollectionAssert.AreEqual(new byte[] {1, 2, 3}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     The client role answers a post-establishment epoch-0 record too, the DTLS-correct response
		///     to a server that believes its final flight was lost: it re-sends its own second flight
		///     (Certificate, ClientKeyExchange, CertificateVerify, ChangeCipherSpec, Finished) via the
		///     handshake engine, exactly like the server role re-sends its last flight.
		/// </summary>
		[TestMethod]
		public async Task EpochZeroRecord_ClientRole_AlsoTriggersAnEngineResend_OfItsOwnSecondFlight()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var serverToClientDatagrams = new List<byte[]>();
			var clientToServerDatagrams = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				serverToClientDatagrams.Add(bytes.ToArray());
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				clientToServerDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// The server's own ChangeCipherSpec is an epoch-0 record fed to the client here - the
			// closest this harness's synchronous, one-record-per-datagram pump gets to a real peer
			// retransmitting a lost final flight.
			byte[] serverEpochZeroDatagram = FindLastEpochZeroDatagram(serverToClientDatagrams);

			long resendsBefore = client.ResendsPerformed;
			int preTriggerCount = clientToServerDatagrams.Count;

			client.FeedDatagram(serverEpochZeroDatagram);

			Assert.AreEqual(resendsBefore + 1, client.ResendsPerformed, "expected a client-role session to also answer a post-establishment epoch-0 record with a resend, not drop it");
			Assert.IsTrue(clientToServerDatagrams.Count > preTriggerCount, "expected the resend to put the client's own second flight back on the wire");

			var received = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			client.OnDecrypted += payload => received.TrySetResult(payload.ToArray());
			server.SendApplicationData(new byte[] {7, 7, 7});
			CollectionAssert.AreEqual(new byte[] {7, 7, 7}, await received.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     The single-owner invariant <see cref="DtlsSession.HandleEpochZeroRecordLocked" />'s own
		///     remarks describe: the handshake engine and the record layer protect records under the same
		///     AES-GCM key, so every epoch-1 sequence either of them ever puts on the wire must be unique,
		///     across both sources, for the whole life of the session. Proven here by interleaving real
		///     application-data sends (which advance the record layer) with an engine-triggered resend
		///     (which advances the engine), and checking every epoch-1 sequence observed on the wire is
		///     seen exactly once.
		/// </summary>
		[TestMethod]
		public async Task EngineResend_NeverReusesAnEpoch1SequenceTheRecordLayerHasSentOrWillSend()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var serverToClientDatagrams = new List<byte[]>();
			var clientToServerDatagrams = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				serverToClientDatagrams.Add(bytes.ToArray());
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				clientToServerDatagrams.Add(bytes.ToArray());
				server.FeedDatagram(bytes);
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// Establishment itself is the seed that makes the rest of this test meaningful: the record
			// layer's own counter must continue exactly where the engine's own epoch-1 sends (its
			// Finished message) left off, never from 0 - a seed of 0 here would be a real AES-GCM nonce
			// reuse the moment the record layer sends its first record, and nothing else in this test
			// would catch it without this direct check.
			Assert.AreEqual(server.EngineNextEpoch1SendSequence, server.RecordCrypto.NextSendSequence, "expected the record layer to be seeded from exactly the engine's own next epoch-1 sequence");
			Assert.AreNotEqual(0UL, server.RecordCrypto.NextSendSequence, "expected the seed to be non-zero: the engine's own Finished message already consumed at least one epoch-1 sequence under this key");

			byte[] triggerDatagram = FindLastEpochZeroDatagram(clientToServerDatagrams);

			// Deliberately NOT cleared: the uniqueness set below must span the whole session, including
			// the handshake engine's own Finished record, or a seed-from-zero bug (the record layer
			// silently colliding with the engine's own already-sent sequence) would be invisible here.
			var received1 = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			client.OnDecrypted += payload => received1.TrySetResult(payload.ToArray());
			server.SendApplicationData(new byte[] {1});
			await received1.Task.WaitAsync(TimeSpan.FromSeconds(5));

			server.FeedDatagram(triggerDatagram);
			Assert.AreEqual(1L, server.ResendsPerformed);

			var received2 = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			client.OnDecrypted += payload => received2.TrySetResult(payload.ToArray());
			server.SendApplicationData(new byte[] {2});
			await received2.Task.WaitAsync(TimeSpan.FromSeconds(5));

			var seenSequences = new HashSet<ulong>();
			int epoch1RecordCount = 0;
			foreach (byte[] datagram in serverToClientDatagrams)
			{
				int offset = 0;
				while (offset < datagram.Length)
				{
					if (!DtlsRecordCrypto.TryReadRecordHeader(datagram.AsSpan(offset), out _, out int epoch, out int fragmentLength)) break;
					if (epoch == 1)
					{
						epoch1RecordCount++;
						ulong sequence = ReadSequence(datagram.AsSpan(offset));
						Assert.IsTrue(seenSequences.Add(sequence), $"expected every epoch-1 sequence to be unique on the wire; {sequence} was seen twice - a nonce reuse under the shared AES-GCM key");
					}
					offset += DtlsRecordCrypto.HeaderLength + fragmentLength;
				}
			}

			Assert.IsTrue(epoch1RecordCount >= 4, "expected at least the handshake's own Finished, the two application-data sends, and the resend's own Finished record to have carried epoch-1");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     <see cref="DtlsSession.OnTick" /> drives the handshake engine's retransmission timer at a
		///     300ms cadence over the host's 10ms tick, counting ticks rather than using a timer of its
		///     own: 29 ticks must not retransmit, the 30th must, and the count resets afterward for the
		///     next window.
		/// </summary>
		[TestMethod]
		public void OnTick_DrivesHandshakeRetransmission_AtA300MsCadenceOverTheHostsTick()
		{
			using var clientCert = RtcCertificate.CreateSelfSigned();
			using var serverCert = RtcCertificate.CreateSelfSigned();

			int sendCount = 0;
			using var client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, _ => sendCount++);

			_ = client.DoHandshakeAsync(CancellationToken.None);
			Assert.AreEqual(1, sendCount, "expected Start() to have sent exactly one datagram");

			for (int i = 0; i < 29; i++) client.OnTick();
			Assert.AreEqual(1, sendCount, "expected no retransmission before the 30th tick (300ms over a 10ms tick)");

			client.OnTick();
			Assert.AreEqual(2, sendCount, "expected the 30th tick to retransmit the unanswered first flight");

			for (int i = 0; i < 29; i++) client.OnTick();
			Assert.AreEqual(2, sendCount, "expected the tick count to have reset after the previous retransmission");

			// A second consecutive timeout at the same MTU is also the engine's own signal (RakNet-
			// style: two tries per size) to step the MTU ladder down and re-fragment the still-buffered
			// ClientHello, which can turn this one retransmission into more than one outgoing datagram;
			// this test only needs to prove OnTick keeps driving the timer on cadence, not pin the
			// engine's own MTU-probing datagram count, so it asserts growth, not an exact total.
			int sendCountBeforeSecondWindow = sendCount;
			client.OnTick();
			Assert.IsTrue(sendCount > sendCountBeforeSecondWindow, "expected a second 300ms window to retransmit again");
		}

		/// <summary>
		///     An encrypted fatal alert is indistinguishable, on the wire, from a normal application-data
		///     record except for its content type; the receiving session must decrypt it, recognise it,
		///     and tear itself down exactly as if <see cref="DtlsSession.Dispose" /> had been called: no
		///     more sends reach the wire, and no more datagrams are delivered. The alert is forged with an
		///     independent <see cref="DtlsRecordCrypto" /> built from the real captured key block (the
		///     same cross-boundary pattern <see cref="DtlsRecordCryptoTests" /> uses), seeded well past
		///     anything the real session has sent so the server's replay window admits it.
		/// </summary>
		[TestMethod]
		public async Task EncryptedFatalAlert_TearsDownTheSession_SendsDropSilently_FeedDatagramBecomesNoOp()
		{
			const byte AlertContentType = 21;
			const byte AlertLevelFatal = 2;
			const byte AlertDescriptionUnexpectedMessage = 10;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			bool serverClosed = false;
			bool serverSentAfterClose = false;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				if (serverClosed) serverSentAfterClose = true;
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// Sanity: the pipe works before teardown.
			var firstReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => firstReceived.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {1, 2, 3});
			CollectionAssert.AreEqual(new byte[] {1, 2, 3}, await firstReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			using (var forger = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0))
			{
				forger.SetSendSequenceForTesting(5000);
				Span<byte> wire = stackalloc byte[2 + DtlsRecordCrypto.RecordOverhead];
				int wireLength = forger.EncryptRecord(AlertContentType, new byte[] {AlertLevelFatal, AlertDescriptionUnexpectedMessage}, wire);
				Assert.AreNotEqual(-1, wireLength);
				server.FeedDatagram(wire.Slice(0, wireLength));
			}

			serverClosed = true;

			// Sends drop silently: must not throw, must never reach the wire.
			server.SendApplicationData(new byte[] {9, 9, 9});
			Assert.IsFalse(serverSentAfterClose, "SendApplicationData must not touch the wire at all once a fatal alert has torn the session down");

			// FeedDatagram becomes a no-op: a subsequent legitimate datagram is not delivered.
			bool deliveredAfterClose = false;
			server.OnDecrypted += _ => deliveredAfterClose = true;
			client.SendApplicationData(new byte[] {4, 5, 6});
			await Task.Delay(200);
			Assert.IsFalse(deliveredAfterClose);

			client.Dispose();
			server.Dispose();
		}

		/// <summary>
		///     RFC 5246 7.2.1: a close_notify ends the connection in both directions immediately, and the
		///     recipient sends its own close_notify back before closing. The response must ride the native
		///     record layer's live send sequence, not a stale one: proven here by sending one real message
		///     first (establishing a known baseline sequence) and asserting the response is exactly one
		///     past it, on the same session's own live counter.
		/// </summary>
		[TestMethod]
		public async Task EncryptedCloseNotify_ClosesTheSession_AndRespondsWithACloseNotifyAtTheLiveSequence()
		{
			const byte AlertContentType = 21;
			const byte AlertLevelWarning = 1;
			const byte AlertDescriptionCloseNotify = 0;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			byte[] lastServerToClient = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				lastServerToClient = bytes.ToArray();
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// One real send first: the response's sequence is checked against this known live baseline,
			// not just "greater than zero".
			var firstReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			client.OnDecrypted += payload => firstReceived.TrySetResult(payload.ToArray());
			server.SendApplicationData(new byte[] {1, 2, 3});
			CollectionAssert.AreEqual(new byte[] {1, 2, 3}, await firstReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)));
			Assert.IsNotNull(lastServerToClient, "expected to have captured the sanity send's wire bytes");
			ulong sanitySequence = ReadSequence(lastServerToClient);

			// Seeded well below the server's own live sequence (already past the sanity send above): the
			// replay window admits anything strictly ahead of the highest sequence seen so far with no
			// distance limit, so a low, distinct sequence here is enough to be accepted as a genuine new
			// record without needing to coordinate with the server's own counter.
			using (var forger = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0))
			{
				forger.SetSendSequenceForTesting(1);
				Span<byte> wire = stackalloc byte[2 + DtlsRecordCrypto.RecordOverhead];
				int wireLength = forger.EncryptRecord(AlertContentType, new byte[] {AlertLevelWarning, AlertDescriptionCloseNotify}, wire);
				Assert.AreNotEqual(-1, wireLength);
				server.FeedDatagram(wire.Slice(0, wireLength));
			}

			Assert.IsTrue(server.IsClosed, "expected the server session to close on receiving close_notify");

			byte responseContentType = lastServerToClient[0];
			Assert.AreEqual(AlertContentType, responseContentType, "expected the server's own close_notify response to be the last thing it sent");
			ulong responseSequence = ReadSequence(lastServerToClient);
			Assert.AreEqual(sanitySequence + 1, responseSequence, "expected the response to ride the native layer's live sequence, one past the last real send - never a stale low sequence");

			using var verifier = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0);
			Span<byte> plaintext = stackalloc byte[2];
			bool decrypted = verifier.TryDecryptRecord(lastServerToClient, plaintext, out byte contentType, out int length);
			Assert.IsTrue(decrypted, "expected the response to decrypt cleanly with the real key block");
			Assert.AreEqual(AlertContentType, contentType);
			Assert.AreEqual(2, length);
			Assert.AreEqual(AlertDescriptionCloseNotify, plaintext[1], "expected description close_notify(0) in the response body");

			client.Dispose();
			server.Dispose();
		}

		/// <summary>
		///     RFC 5246 7.2.1's "any data received after a closure alert is ignored" applies within one
		///     datagram, not just across datagrams: a close_notify record followed by an application-data
		///     record in the SAME datagram must stop the walk at the alert, never reaching the record
		///     after it.
		/// </summary>
		[TestMethod]
		public async Task CloseNotify_StopsProcessingTheRestOfTheDatagram_LaterRecordInTheSameDatagramNotDelivered()
		{
			const byte AlertContentType = 21;
			const byte ApplicationDataContentType = 23;
			const byte AlertLevelWarning = 1;
			const byte AlertDescriptionCloseNotify = 0;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			bool delivered = false;
			server.OnDecrypted += _ => delivered = true;

			using (var forger = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0))
			{
				forger.SetSendSequenceForTesting(1);
				Span<byte> alertWire = stackalloc byte[2 + DtlsRecordCrypto.RecordOverhead];
				int alertLength = forger.EncryptRecord(AlertContentType, new byte[] {AlertLevelWarning, AlertDescriptionCloseNotify}, alertWire);
				Assert.AreNotEqual(-1, alertLength);

				byte[] appPayload = {9, 9, 9};
				Span<byte> appWire = stackalloc byte[appPayload.Length + DtlsRecordCrypto.RecordOverhead];
				int appLength = forger.EncryptRecord(ApplicationDataContentType, appPayload, appWire);
				Assert.AreNotEqual(-1, appLength);

				byte[] datagram = new byte[alertLength + appLength];
				alertWire.Slice(0, alertLength).CopyTo(datagram);
				appWire.Slice(0, appLength).CopyTo(datagram.AsSpan(alertLength));

				server.FeedDatagram(datagram);
			}

			Assert.IsTrue(server.IsClosed, "expected the server session to close on the close_notify record");
			Assert.IsFalse(delivered, "expected the application-data record after the close_notify, in the same datagram, to never be delivered");

			client.Dispose();
			server.Dispose();
		}

		/// <summary>
		///     RFC 5246 7.2.1 requires nothing further on the wire once a close_notify has gone out: this
		///     asserts on every datagram the server sends across teardown, not just the last one.
		///     <see cref="DtlsSession.Dispose" /> emits exactly one close_notify, ours, at the native
		///     record layer's live sequence, and nothing else ever reaches the wire once it has.
		/// </summary>
		[TestMethod]
		public async Task Dispose_EmitsExactlyOneCloseNotify_NothingElseAfterIt()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var sentByServer = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				sentByServer.Add(bytes.ToArray());
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			ulong establishedSequence = server.RecordCrypto.NextSendSequence;
			sentByServer.Clear(); // only datagrams from teardown onward matter here.
			server.Dispose();

			AssertExactlyOneLiveCloseNotify(sentByServer, client.CapturedKeys, establishedSequence);

			client.Dispose();
		}

		/// <summary>
		///     The other order: a peer's close_notify already closed this side (sending our own
		///     response in the process, per RFC 5246 7.2.1) before <see cref="DtlsSession.Dispose" /> is
		///     ever called on it. Without a send-once guard, <see cref="DtlsSession.Dispose" /> would try
		///     to say goodbye a second time - <see cref="RequestClose" /> having already run inside the
		///     close_notify handler means a plain <see cref="DtlsSession._closed" /> check at the top of
		///     <see cref="DtlsSession.Dispose" /> could not tell the two cases apart, which is why the
		///     guard lives on the send itself, not on whether the session was already closed.
		/// </summary>
		[TestMethod]
		public async Task InboundCloseNotify_ThenDispose_EmitsExactlyOneCloseNotifyTotal()
		{
			const byte AlertContentType = 21;
			const byte AlertLevelWarning = 1;
			const byte AlertDescriptionCloseNotify = 0;

			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			var sentByServer = new List<byte[]>();
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes =>
			{
				sentByServer.Add(bytes.ToArray());
				client.FeedDatagram(bytes);
			});
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			ulong establishedSequence = server.RecordCrypto.NextSendSequence;
			sentByServer.Clear();

			using (var forger = new DtlsRecordCrypto(client.CapturedKeys, isServer: false, sendSequenceSeed: 0))
			{
				forger.SetSendSequenceForTesting(1);
				Span<byte> wire = stackalloc byte[2 + DtlsRecordCrypto.RecordOverhead];
				int wireLength = forger.EncryptRecord(AlertContentType, new byte[] {AlertLevelWarning, AlertDescriptionCloseNotify}, wire);
				Assert.AreNotEqual(-1, wireLength);
				server.FeedDatagram(wire.Slice(0, wireLength));
			}

			Assert.IsTrue(server.IsClosed, "expected the inbound close_notify to have closed the server session already");

			// The leak this guards against: a caller disposing a session some other event already closed.
			server.Dispose();

			AssertExactlyOneLiveCloseNotify(sentByServer, client.CapturedKeys, establishedSequence);

			client.Dispose();
		}

		/// <summary>
		///     Shared by the two close_notify-ordering tests above: every datagram captured must be
		///     exactly one record, a close_notify, riding the native layer's live sequence (at or above
		///     <paramref name="minimumSequence" />, the record layer's own counter as of establishment -
		///     never something lower, which would mean a stale or reset counter), and there must be
		///     exactly one such datagram total.
		/// </summary>
		private static void AssertExactlyOneLiveCloseNotify(List<byte[]> sentDatagrams, DtlsNegotiatedKeys peerKeys, ulong minimumSequence)
		{
			const byte AlertContentType = 21;

			Assert.AreEqual(1, sentDatagrams.Count, "expected exactly one datagram total: our close_notify, and nothing else");

			byte[] onlyDatagram = sentDatagrams[0];
			Assert.AreEqual(AlertContentType, onlyDatagram[0], "expected an alert record");

			ulong sequence = ReadSequence(onlyDatagram);
			Assert.IsTrue(sequence >= minimumSequence, $"expected the live native sequence ({minimumSequence}+), never a stale or reset one; got {sequence}");

			using var verifier = new DtlsRecordCrypto(peerKeys, isServer: false, sendSequenceSeed: 0);
			Span<byte> plaintext = stackalloc byte[2];
			bool decrypted = verifier.TryDecryptRecord(onlyDatagram, plaintext, out byte contentType, out int length);
			Assert.IsTrue(decrypted, "expected the close_notify to decrypt cleanly with the real key block");
			Assert.AreEqual(AlertContentType, contentType);
			Assert.AreEqual(2, length);
			Assert.AreEqual(0, plaintext[1], "expected description close_notify(0)");
		}

		private static ulong ReadSequence(ReadOnlySpan<byte> record)
		{
			ReadOnlySpan<byte> sequence = record.Slice(5, 6);
			ulong value = 0;
			foreach (byte b in sequence) value = (value << 8) | b;
			return value;
		}

		/// <summary>
		///     A single corrupted byte on the wire must never take the session down or lose a delivery
		///     count: it is drop-and-count, exactly like <see cref="DtlsRecordCryptoTests" /> already
		///     proves for the record layer in isolation, but exercised through the full session so the
		///     dispatch in <see cref="DtlsSession.FeedDatagram" /> is what is actually under test, not
		///     just <see cref="DtlsRecordCrypto" /> on its own.
		/// </summary>
		[TestMethod]
		public async Task TamperedRecord_IsDroppedAndCounted_SessionStaysAlive_NextCleanRecordDelivers()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			bool interceptOnly = false;
			byte[] intercepted = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				if (interceptOnly)
				{
					intercepted = bytes.ToArray();
				}
				else
				{
					server.FeedDatagram(bytes);
				}
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// Capture one clean record's wire bytes without delivering it, so its sequence has never
			// been seen by the server's replay window: tampering an already-delivered sequence would be
			// rejected as a replay, not a bad tag, and would prove nothing about DecryptFailures.
			interceptOnly = true;
			client.SendApplicationData(new byte[] {1, 1, 1});
			interceptOnly = false;
			Assert.IsNotNull(intercepted, "expected to have captured the wire bytes of an undelivered record");

			byte[] tampered = (byte[]) intercepted.Clone();
			tampered[tampered.Length - 1] ^= 0xFF; // last byte of the record is always the final tag byte

			long decryptFailuresBefore = server.RecordCrypto.DecryptFailures;
			int deliveredCount = 0;
			server.OnDecrypted += _ => deliveredCount++;

			server.FeedDatagram(tampered);

			Assert.AreEqual(0, deliveredCount, "a tampered record must never be delivered");
			Assert.AreEqual(decryptFailuresBefore + 1, server.RecordCrypto.DecryptFailures);

			// The replay window must not have advanced on the rejected tamper attempt: the clean
			// original, at the same sequence, still decrypts.
			var firstReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => firstReceived.TrySetResult(payload.ToArray());
			server.FeedDatagram(intercepted);
			CollectionAssert.AreEqual(new byte[] {1, 1, 1}, await firstReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)));

			// And the session is still fully alive for a genuinely new record afterward.
			var secondReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload => secondReceived.TrySetResult(payload.ToArray());
			client.SendApplicationData(new byte[] {2, 2, 2});
			CollectionAssert.AreEqual(new byte[] {2, 2, 2}, await secondReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)), "the session must still deliver a clean record after a rejected tamper attempt");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     The one real service the deleted inbound-queue machinery provided was buffer-lifetime
		///     safety if <see cref="DtlsSession.FeedDatagram" /> re-enters from inside a delivery
		///     callback: a programming error in production (the mux thread is the only caller) that must
		///     still fail safe rather than corrupt <c>_receiveScratch</c>. Calling
		///     <see cref="DtlsSession.FeedDatagram" /> again, synchronously, from inside an
		///     <see cref="DtlsSession.OnDecrypted" /> subscriber must be dropped and counted, must not
		///     throw, and must not disturb the outer call still on the stack: the outer delivery still
		///     completes and hands back the original payload untouched.
		/// </summary>
		[TestMethod]
		public async Task FeedDatagram_ReenteredFromWithinOnDecrypted_IsDroppedAndCounted_OuterDeliveryStillCompletesIntact()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			bool interceptOnly = false;
			byte[] interceptedDatagram = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes =>
			{
				if (interceptOnly)
				{
					interceptedDatagram = bytes.ToArray();
				}
				else
				{
					server.FeedDatagram(bytes);
				}
			});

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// Captured, never delivered: a genuinely valid, still-fresh record for the reentrant call
			// below to feed back in.
			interceptOnly = true;
			client.SendApplicationData(new byte[] {6, 6, 6});
			interceptOnly = false;
			Assert.IsNotNull(interceptedDatagram, "expected to have captured an undelivered wire datagram to replay reentrantly");

			long reentrantDropsBefore = server.ReentrantFeedsDropped;
			int deliveredCount = 0;
			var outerReceived = new TaskCompletionSource<byte[]>(TaskCreationOptions.RunContinuationsAsynchronously);
			server.OnDecrypted += payload =>
			{
				deliveredCount++;
				outerReceived.TrySetResult(payload.ToArray());

				// Reentrant: FeedDatagram is still on the stack, having released _gate for this exact
				// call. Must not throw, must not deliver, must not corrupt the outer call's own delivery.
				server.FeedDatagram(interceptedDatagram);
			};

			client.SendApplicationData(new byte[] {7, 7, 7});

			CollectionAssert.AreEqual(new byte[] {7, 7, 7}, await outerReceived.Task.WaitAsync(TimeSpan.FromSeconds(5)), "expected the outer delivery to complete intact despite the reentrant call inside its own callback");
			Assert.AreEqual(reentrantDropsBefore + 1, server.ReentrantFeedsDropped, "expected the reentrant call to be dropped and counted");
			Assert.AreEqual(1, deliveredCount, "expected the reentrantly-fed datagram to never be delivered");

			server.Dispose();
			client.Dispose();
		}

		/// <summary>
		///     With the handshake engine out of the post-handshake path entirely, this must allocate
		///     nothing on our side. Both sessions' native record layers, both directions, 10k datagrams
		///     each way, well past JIT/AesGcm warmup.
		/// </summary>
		[TestMethod]
		public async Task NativeRecordLayer_TenThousandDatagramsBothDirections_AllocatesNothingOnOurSide()
		{
			using var serverCert = RtcCertificate.CreateSelfSigned();
			using var clientCert = RtcCertificate.CreateSelfSigned();

			DtlsSession server = null, client = null;
			server = new DtlsSession(serverCert, clientCert.FingerprintSha256, isServer: true, bytes => client.FeedDatagram(bytes));
			client = new DtlsSession(clientCert, serverCert.FingerprintSha256, isServer: false, bytes => server.FeedDatagram(bytes));

			Task<bool> serverDone = server.DoHandshakeAsync(CancellationToken.None);
			Task<bool> clientDone = client.DoHandshakeAsync(CancellationToken.None);
			Assert.IsTrue(await clientDone.WaitAsync(TimeSpan.FromSeconds(15)));
			Assert.IsTrue(await serverDone.WaitAsync(TimeSpan.FromSeconds(15)));

			// The subscriber owns the delivered buffer (OnDecrypted's contract) and returns it; that
			// return is what keeps the pool primed and the steady state allocation-free, so this test
			// exercises the ownership loop itself, not just the decrypt.
			static void ConsumeAndReturn(ReadOnlyMemory<byte> payload)
			{
				if (System.Runtime.InteropServices.MemoryMarshal.TryGetArray(payload, out ArraySegment<byte> segment) && segment.Array != null && segment.Offset == 0)
				{
					System.Buffers.ArrayPool<byte>.Shared.Return(segment.Array);
				}
			}

			server.OnDecrypted += ConsumeAndReturn;
			client.OnDecrypted += ConsumeAndReturn;

			byte[] payload = {1, 2, 3, 4, 5, 6, 7, 8};

			// Warmup: JIT and any AesGcm first-use setup happen here, not inside the measured bracket.
			for (int i = 0; i < 100; i++)
			{
				client.SendApplicationData(payload);
				server.SendApplicationData(payload);
			}

			// Per-thread, not process-wide: the send-encrypt-feed-decrypt-deliver chain is one
			// synchronous call stack on this thread, and the class-parallel suite would otherwise
			// fail this bracket on a neighbor's garbage.
			long before = GC.GetAllocatedBytesForCurrentThread();
			for (int i = 0; i < 10000; i++)
			{
				client.SendApplicationData(payload);
				server.SendApplicationData(payload);
			}
			long after = GC.GetAllocatedBytesForCurrentThread();

			Assert.AreEqual(0L, after - before, "expected zero heap allocation across 10k datagrams each way, post-handshake");

			server.Dispose();
			client.Dispose();
		}
	}
}