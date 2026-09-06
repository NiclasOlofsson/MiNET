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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net.Rtc;
using MiNET.Net.Rtc.FastDtls;

namespace MiNET.Test.Rtc
{
	// Counts GC allocations over ten thousand records; any neighbour allocating on another
	// worker lands in the count.
	[TestClass, DoNotParallelize]
	public class DtlsRecordCryptoTests
	{
		private const byte ApplicationData = 23;

		// An arbitrary, distinctly-non-zero starting sequence for tests that are not themselves about
		// the seeding contract: proves nothing here is silently relying on the old built-in headroom
		// default, which the constructor no longer has (see the seeding-contract tests below).
		private const ulong ArbitrarySeed = 12345;

		/// <summary>Fixed, deterministic key material: <see cref="DtlsRecordCrypto" /> is pure spans in/out and does not care where its keys came from, so most of this class exercises it against synthetic vectors rather than paying for a real handshake.</summary>
		private static DtlsNegotiatedKeys CreateTestKeys()
		{
			var keys = new DtlsNegotiatedKeys();
			for (int i = 0; i < keys.ClientWriteKey.Length; i++)
			{
				keys.ClientWriteKey[i] = (byte) (i + 1);
				keys.ServerWriteKey[i] = (byte) (i + 101);
			}
			for (int i = 0; i < 4; i++)
			{
				keys.ClientWriteSalt[i] = (byte) (i + 201);
				keys.ServerWriteSalt[i] = (byte) (i + 211);
			}
			return keys;
		}

		[TestMethod]
		public void EncryptRecord_ThenTryDecryptRecord_RoundTripsAcrossRoles_SequenceIncrementsPerRecord()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			Span<byte> wire = stackalloc byte[5 + DtlsRecordCrypto.RecordOverhead];
			Span<byte> plaintext = stackalloc byte[5];
			for (int i = 0; i < 5; i++)
			{
				byte[] payload = {(byte) i, 1, 2, 3, 4};
				int wireLength = client.EncryptRecord(ApplicationData, payload, wire);

				Assert.AreNotEqual(-1, wireLength);
				Assert.AreEqual(ArbitrarySeed + (ulong) i, ReadSequence(wire));

				bool ok = server.TryDecryptRecord(wire.Slice(0, wireLength), plaintext, out byte contentType, out int length);

				Assert.IsTrue(ok, $"expected record {i} to decrypt");
				Assert.AreEqual(ApplicationData, contentType);
				Assert.AreEqual(payload.Length, length);
				CollectionAssert.AreEqual(payload, plaintext.Slice(0, length).ToArray());
			}
		}

		/// <summary>
		///     The exact-seed contract this constructor now has, replacing the old built-in headroom
		///     default: whatever sequence the caller passes is exactly the sequence the first encrypted
		///     record carries, with no implicit offset applied on top of it. The handshake engine and
		///     this record layer protect records under the same key, so the caller (<see cref="DtlsSession" />)
		///     is the one place that knows where the engine's own send sequence actually left off; a
		///     built-in default here could no longer be trusted to be clear of it.
		/// </summary>
		[TestMethod]
		public void Constructor_ExactSeed_FirstEncryptedRecordCarriesThatExactSequence()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			const ulong seed = 987654;
			using var client = new DtlsRecordCrypto(keys, isServer: false, seed);

			byte[] payload = {1, 2, 3};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);

			Assert.AreNotEqual(-1, wireLength);
			Assert.AreEqual(seed, ReadSequence(wire), "expected the first record to carry exactly the seed passed to the constructor");

			int secondWireLength = client.EncryptRecord(ApplicationData, payload, wire);
			Assert.AreNotEqual(-1, secondWireLength);
			Assert.AreEqual(seed + 1, ReadSequence(wire), "expected the second record to carry the seed plus one");
		}

		/// <summary>
		///     <see cref="DtlsRecordCrypto.SeedSendSequenceForward" /> mirrors
		///     <see cref="DtlsEngine.SeedEpoch1SendSequence" />: it is the one-directional half of the
		///     single-owner invariant that keeps a post-handshake handshake-engine retransmission and this
		///     record layer from ever emitting the same (epoch, sequence) pair twice under their shared
		///     key (see <see cref="DtlsSession.HandleEpochZeroRecordLocked" />). Seeding backward, to a
		///     value at or behind the current sequence, must be a no-op: only forward motion is ever safe.
		/// </summary>
		[TestMethod]
		public void SeedSendSequenceForward_MovesOnlyForward()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, sendSequenceSeed: 100);

			client.SeedSendSequenceForward(50); // behind the current sequence: must be ignored
			Assert.AreEqual(100UL, client.NextSendSequence);

			client.SeedSendSequenceForward(100); // exactly at the current sequence: must be ignored
			Assert.AreEqual(100UL, client.NextSendSequence);

			client.SeedSendSequenceForward(500); // ahead: must move
			Assert.AreEqual(500UL, client.NextSendSequence);

			byte[] payload = {1, 2, 3};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			Assert.AreNotEqual(-1, wireLength);
			Assert.AreEqual(500UL, ReadSequence(wire), "expected the next encrypted record to carry the seeded-forward sequence");
		}

		[TestMethod]
		public void EncryptRecord_DestinationTooSmall_ReturnsMinusOne_WithoutAdvancingSequence()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4};
			Span<byte> tooSmall = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead - 1];
			Assert.AreEqual(-1, client.EncryptRecord(ApplicationData, payload, tooSmall));

			// The sequence must not have moved: a record encrypted right after must still carry the
			// original seed, not one past it.
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			Assert.AreNotEqual(-1, wireLength);
			Assert.AreEqual(ArbitrarySeed, ReadSequence(wire));
		}

		[TestMethod]
		public void TryDecryptRecord_DestinationTooSmall_ReturnsFalse_MalformedRecordsCounted()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4, 5, 6, 7, 8};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);

			Span<byte> tooSmall = stackalloc byte[payload.Length - 1];
			long before = server.MalformedRecords;
			bool ok = server.TryDecryptRecord(wire.Slice(0, wireLength), tooSmall, out _, out _);

			Assert.IsFalse(ok);
			Assert.AreEqual(before + 1, server.MalformedRecords);
		}

		private static void AssertTamperRejectedThenCleanRecordStillDecrypts(int tamperByteIndex)
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4, 5, 6, 7, 8};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			byte[] clean = wire.Slice(0, wireLength).ToArray();

			byte[] tampered = (byte[]) clean.Clone();
			tampered[tamperByteIndex] ^= 0xFF;

			Span<byte> plaintext = stackalloc byte[payload.Length];
			long before = server.DecryptFailures;
			bool tamperedOk = server.TryDecryptRecord(tampered, plaintext, out _, out _);

			Assert.IsFalse(tamperedOk, $"expected tampering byte {tamperByteIndex} to fail authentication");
			Assert.AreEqual(before + 1, server.DecryptFailures);

			// The replay window must not have advanced: the same sequence, presented clean, still decrypts.
			bool cleanOk = server.TryDecryptRecord(clean, plaintext, out byte contentType, out int length);
			Assert.IsTrue(cleanOk, "a clean record at the same sequence must still decrypt after a rejected tamper attempt");
			Assert.AreEqual(ApplicationData, contentType);
			CollectionAssert.AreEqual(payload, plaintext.Slice(0, length).ToArray());
		}

		[TestMethod]
		public void TryDecryptRecord_TamperedCiphertextByte_IsRejected_WindowNotAdvanced()
		{
			// First byte of the ciphertext: header(13) + explicit nonce(8).
			AssertTamperRejectedThenCleanRecordStillDecrypts(13 + 8);
		}

		[TestMethod]
		public void TryDecryptRecord_TamperedTagByte_IsRejected_WindowNotAdvanced()
		{
			// Last byte of the record is always the final tag byte.
			byte[] payload = {1, 2, 3, 4, 5, 6, 7, 8};
			int lastByteIndex = payload.Length + DtlsRecordCrypto.RecordOverhead - 1;
			AssertTamperRejectedThenCleanRecordStillDecrypts(lastByteIndex);
		}

		[TestMethod]
		public void TryDecryptRecord_TamperedAadCoveredHeaderByte_IsRejected_WindowNotAdvanced()
		{
			// Byte 0 of the header is the content type, which is part of the AAD.
			AssertTamperRejectedThenCleanRecordStillDecrypts(0);
		}

		[TestMethod]
		public void TryDecryptRecord_DuplicateRecord_SecondIsRejected_ReplayDropsCounted()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			byte[] record = wire.Slice(0, wireLength).ToArray();

			Span<byte> plaintext = stackalloc byte[payload.Length];
			Assert.IsTrue(server.TryDecryptRecord(record, plaintext, out _, out _));

			long before = server.ReplayDrops;
			bool replayed = server.TryDecryptRecord(record, plaintext, out _, out _);

			Assert.IsFalse(replayed);
			Assert.AreEqual(before + 1, server.ReplayDrops);
		}

		[TestMethod]
		public void TryDecryptRecord_OutOfOrderWithinWindow_IsAdmitted()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			const int count = 5;
			var records = new byte[count][];
			Span<byte> wire = stackalloc byte[4 + DtlsRecordCrypto.RecordOverhead];
			for (int i = 0; i < count; i++)
			{
				byte[] payload = {(byte) i, 1, 2, 3};
				int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
				records[i] = wire.Slice(0, wireLength).ToArray();
			}

			int[] deliveryOrder = {4, 2, 0, 3, 1};
			Span<byte> plaintext = stackalloc byte[4];
			foreach (int sequence in deliveryOrder)
			{
				bool ok = server.TryDecryptRecord(records[sequence], plaintext, out _, out _);
				Assert.IsTrue(ok, $"expected out-of-order sequence {sequence} to be admitted within the replay window");
			}
		}

		[TestMethod]
		public void TryDecryptRecord_SequenceSixtyFourOrMoreBehindHighest_Rejects_ReplayDropsCounted()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4};
			Span<byte> firstWire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int firstLength = client.EncryptRecord(ApplicationData, payload, firstWire);
			byte[] earliestRecord = firstWire.Slice(0, firstLength).ToArray();

			byte[] farRecord = null;
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			for (int i = 1; i <= 70; i++)
			{
				int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
				if (i == 70) farRecord = wire.Slice(0, wireLength).ToArray();
			}

			Span<byte> plaintext = stackalloc byte[payload.Length];
			Assert.IsTrue(server.TryDecryptRecord(farRecord, plaintext, out _, out _), "expected the 71st record to establish the window's high water mark");

			long before = server.ReplayDrops;
			bool ok = server.TryDecryptRecord(earliestRecord, plaintext, out _, out _);

			Assert.IsFalse(ok, "the earliest record is 70 behind the highest received, which is >= the 64-wide window");
			Assert.AreEqual(before + 1, server.ReplayDrops);
		}

		[TestMethod]
		public void TryReadRecordHeader_ValidRecord_ReturnsFieldsAndTrue()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);

			byte[] payload = {1, 2, 3};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);

			bool ok = DtlsRecordCrypto.TryReadRecordHeader(wire.Slice(0, wireLength), out byte contentType, out int epoch, out int fragmentLength);

			Assert.IsTrue(ok);
			Assert.AreEqual(ApplicationData, contentType);
			Assert.AreEqual(1, epoch);
			Assert.AreEqual(wireLength - 13, fragmentLength);
		}

		[TestMethod]
		public void TryReadRecordHeader_TooShortForHeader_ReturnsFalse()
		{
			Span<byte> tooShort = stackalloc byte[12];
			bool ok = DtlsRecordCrypto.TryReadRecordHeader(tooShort, out _, out _, out _);
			Assert.IsFalse(ok);
		}

		[TestMethod]
		public void TryDecryptRecord_MalformedInputs_NeverThrow_MalformedRecordsCounted()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);

			byte[] payload = {1, 2, 3, 4, 5, 6, 7, 8};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			byte[] validRecord = wire.Slice(0, wireLength).ToArray();

			Span<byte> plaintext = stackalloc byte[payload.Length];

			// Every truncation length of a valid record must be rejected without throwing.
			for (int truncateTo = 0; truncateTo < validRecord.Length; truncateTo++)
			{
				using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);
				long before = server.MalformedRecords;
				bool ok = server.TryDecryptRecord(validRecord.AsSpan(0, truncateTo), plaintext, out _, out _);

				Assert.IsFalse(ok, $"expected a record truncated to {truncateTo} bytes to be rejected");
				Assert.AreEqual(before + 1, server.MalformedRecords, $"expected MalformedRecords to be counted for a {truncateTo}-byte truncation");
			}

			// Epoch 2 instead of 1.
			{
				using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);
				byte[] wrongEpoch = (byte[]) validRecord.Clone();
				wrongEpoch[4] = 2;
				long before = server.MalformedRecords;
				Assert.IsFalse(server.TryDecryptRecord(wrongEpoch, plaintext, out _, out _));
				Assert.AreEqual(before + 1, server.MalformedRecords);
			}

			// Wrong record version.
			{
				using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);
				byte[] wrongVersion = (byte[]) validRecord.Clone();
				wrongVersion[1] = 0x03;
				wrongVersion[2] = 0x03;
				long before = server.MalformedRecords;
				Assert.IsFalse(server.TryDecryptRecord(wrongVersion, plaintext, out _, out _));
				Assert.AreEqual(before + 1, server.MalformedRecords);
			}

			// Declared length past the end of the buffer, header otherwise intact.
			{
				using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);
				byte[] lengthPastEnd = (byte[]) validRecord.Clone();
				lengthPastEnd[11] = 0xFF;
				lengthPastEnd[12] = 0xFF;
				long before = server.MalformedRecords;
				Assert.IsFalse(server.TryDecryptRecord(lengthPastEnd, plaintext, out _, out _));
				Assert.AreEqual(before + 1, server.MalformedRecords);
			}
		}

		[TestMethod]
		public void EncryptRecord_AtSequenceExhaustion_ReturnsMinusOne()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			client.SetSendSequenceForTesting((1UL << 48) - 1);

			byte[] payload = {1, 2, 3};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];

			Assert.AreEqual(-1, client.EncryptRecord(ApplicationData, payload, wire));
			// Exhaustion is permanent: a second attempt must still fail, never wrap back to 0.
			Assert.AreEqual(-1, client.EncryptRecord(ApplicationData, payload, wire));
		}

		/// <summary>
		///     The accept side of the boundary above: one sequence short of exhaustion still succeeds,
		///     and only the record after that hits it. An off-by-one that rejected one sequence early
		///     would fail this test without ever touching the exhaustion point itself.
		/// </summary>
		[TestMethod]
		public void EncryptRecord_OneBeforeSequenceExhaustion_Succeeds_ThenNextReturnsMinusOne()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			client.SetSendSequenceForTesting((1UL << 48) - 2);

			byte[] payload = {1, 2, 3};
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];

			int wireLength = client.EncryptRecord(ApplicationData, payload, wire);
			Assert.AreNotEqual(-1, wireLength, "expected the last non-exhausted sequence to still encrypt successfully");
			Assert.AreEqual((1UL << 48) - 2, ReadSequence(wire.Slice(0, wireLength)));

			Assert.AreEqual(-1, client.EncryptRecord(ApplicationData, payload, wire), "expected the very next record to hit sequence exhaustion");
		}

		[TestMethod]
		public void EncryptThenDecrypt_TenThousandRecords_AllocatesNothingAfterWarmup()
		{
			DtlsNegotiatedKeys keys = CreateTestKeys();
			using var client = new DtlsRecordCrypto(keys, isServer: false, ArbitrarySeed);
			using var server = new DtlsRecordCrypto(keys, isServer: true, ArbitrarySeed);

			Span<byte> payload = stackalloc byte[8];
			for (int i = 0; i < payload.Length; i++) payload[i] = (byte) i;
			Span<byte> wire = stackalloc byte[payload.Length + DtlsRecordCrypto.RecordOverhead];
			Span<byte> plaintext = stackalloc byte[payload.Length];

			// Warmup: JIT and any AesGcm first-use setup happen here, not inside the measured bracket.
			for (int i = 0; i < 100; i++)
			{
				int len = client.EncryptRecord(ApplicationData, payload, wire);
				server.TryDecryptRecord(wire.Slice(0, len), plaintext, out _, out _);
			}

			bool allOk = true;
			// Per-thread, not process-wide: the loop is fully synchronous on this thread, and the
			// suite runs test classes in parallel, so a process-wide bracket would count some
			// unrelated class's allocations and fail this test for a neighbor's garbage.
			long before = GC.GetAllocatedBytesForCurrentThread();
			for (int i = 0; i < 10000; i++)
			{
				int len = client.EncryptRecord(ApplicationData, payload, wire);
				allOk &= server.TryDecryptRecord(wire.Slice(0, len), plaintext, out _, out _);
			}
			long after = GC.GetAllocatedBytesForCurrentThread();

			Assert.IsTrue(allOk, "expected every one of the 10k records to decrypt successfully");
			Assert.AreEqual(0, after - before, "expected zero heap allocation across 10k encrypt+decrypt cycles");
		}

		private static ulong ReadSequence(ReadOnlySpan<byte> record)
		{
			ReadOnlySpan<byte> sequence = record.Slice(5, 6);
			ulong value = 0;
			foreach (byte b in sequence) value = (value << 8) | b;
			return value;
		}
	}
}