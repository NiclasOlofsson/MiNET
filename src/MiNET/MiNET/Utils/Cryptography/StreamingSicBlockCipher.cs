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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2021 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Modes;

namespace SicStream
{
	/**
	 * * A class that implements an online Sic (segmented integer counter mode, or just counter (CTR) mode for short).
	 * * This class buffers one encrypted counter (representing the key stream) at a time.
	 * * The encryption of the counter is only performed when required, so that no key stream blocks are generated while they are not required.
	 * *
	 * * From: https://stackoverflow.com/questions/51286633/java-bc-sicblockcipher-direct-output-equivalent-in-c-sharp
	 */
	public class StreamingSicBlockCipher : BufferedCipherBase
	{
		private SicStreamCipher parent;
		private int blockSize;

		public StreamingSicBlockCipher(SicBlockCipher parent)
		{
			this.parent = new SicStreamCipher(parent);
			this.blockSize = parent.GetBlockSize();
		}

		public override string AlgorithmName
		{
			get
			{
				return parent.AlgorithmName;
			}
		}

		public override byte[] DoFinal()
		{
			// returns no bytes at all, as there is no input
			return new byte[0];
		}

		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			byte[] result = ProcessBytes(input, inOff, length);

			Reset();

			return result;
		}

		public override int GetBlockSize()
		{
			return blockSize;
		}

		public override int GetOutputSize(int inputLen)
		{
			return inputLen;
		}

		public override int GetUpdateOutputSize(int inputLen)
		{
			return inputLen;
		}

		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			parent.Init(forEncryption, parameters);
		}

		public override byte[] ProcessByte(byte input)
		{
			return new byte[] {parent.ReturnByte(input)};
		}

		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			byte[] result = new byte[length];
			parent.ProcessBytes(input, inOff, length, result, 0);
			return result;
		}

		public override void Reset()
		{
			parent.Reset();
		}
	}

	public class SicStreamCipher : IStreamCipher
	{
		private SicBlockCipher parent;
		private int blockSize;

		private byte[] zeroBlock;

		private byte[] blockBuffer;
		private int processed;

		public SicStreamCipher(SicBlockCipher parent)
		{
			this.parent = parent;
			this.blockSize = parent.GetBlockSize();

			this.zeroBlock = new byte[blockSize];

			this.blockBuffer = new byte[blockSize];
			// indicates that no bytes are available: lazy generation of counter blocks (they may not be needed)
			this.processed = blockSize;
		}

		public string AlgorithmName
		{
			get
			{
				return parent.AlgorithmName;
			}
		}

		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			parent.Init(forEncryption, parameters);

			Array.Clear(blockBuffer, 0, blockBuffer.Length);
			processed = blockSize;
		}

		public void ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			int inputProcessed = 0;
			while (inputProcessed < length)
			{
				// NOTE can be optimized further
				// the number of available bytes can be pre-calculated; too much branching
				if (processed == blockSize)
				{
					// lazilly create a new block of key stream
					parent.ProcessBlock(zeroBlock, 0, blockBuffer, 0);
					processed = 0;
				}

				output[outOff + inputProcessed] = (byte) (input[inOff + inputProcessed] ^ blockBuffer[processed]);

				processed++;
				inputProcessed++;
			}
		}

		public void Reset()
		{
			parent.Reset();

			Array.Clear(blockBuffer, 0, blockBuffer.Length);
			this.processed = blockSize;
		}

		public byte ReturnByte(byte input)
		{
			if (processed == blockSize)
			{
				// lazily create a new block of key stream
				parent.ProcessBlock(zeroBlock, 0, blockBuffer, 0);
				processed = 0;
			}
			return (byte) (input ^ blockBuffer[processed++]);
		}
	}
}