using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MiNET.Network
{
	public class FlatGenerator
	{
		public List<Chunk2> GenerateFlatWorld(int xDimension, int zDimension)
		{
			List<Chunk2> chunks = new List<Chunk2>();
			for (int x = 0; x < xDimension; x++)
			{
				for (int z = 0; z < zDimension; z++)
				{
					Chunk2 chunk = new Chunk2 { x = x, z = z };
					PopulateChunk(chunk);
					chunks.Add(chunk);
				}
			}

			return chunks;
		}

		public void PopulateChunk(Chunk2 chunk)
		{
			var random = new CryptoRandom();
			var stones = new byte[16*16*128];

			for (int i = 0; i < stones.Length; i = i + 128)
			{
				stones[i] = 7; // Bedrock
				switch (random.Next(0, 20))
				{
					case 0:
						stones[i + 1] = 3; // Dirt
						stones[i + 2] = 3;
						break;
					case 1:
						stones[i + 1] = 1; // Stone
						stones[i + 2] = 1; // Stone
						break;
					case 2:
						stones[i + 1] = 13; // Gravel
						stones[i + 2] = 13; // Gravel
						break;
					case 3:
						stones[i + 1] = 14; // Gold
						stones[i + 2] = 14; // Gold
						break;
					case 4:
						stones[i + 1] = 16; // Cole
						stones[i + 2] = 16; // Cole
						break;
					case 5:
						stones[i + 1] = 56; // Dimond
						stones[i + 2] = 56; // Dimond
						break;
					default:
						stones[i + 1] = 1; // Stone
						stones[i + 2] = 1; // Stone
						break;
				}
				stones[i + 3] = 2; // Grass
			}

			chunk.blocks = stones;

			//chunk.SetBlock(0, 0, 0, 9); // water
			//chunk.SetBlock(1, 1, 1, 10); // lava
			//chunk.SetBlock(2, 2, 2, 9); // lava
			//chunk.SetBlock(15, 127, 15, 9); // lava
		}
	}

	public class CryptoRandom : RandomNumberGenerator
	{
		private static RandomNumberGenerator r;

		/// <summary>
		///     Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data.
		/// </summary>
		public CryptoRandom()
		{
			r = RandomNumberGenerator.Create();
		}

		/// <summary>
		///     Fills the elements of a specified array of bytes with random numbers.
		/// </summary>
		/// <param name=” buffer”>An array of bytes to contain random numbers.</param>
		public override void GetBytes(byte[] buffer)
		{
			r.GetBytes(buffer);
		}

		/// <summary>
		///     Returns a random number between 0.0 and 1.0.
		/// </summary>
		public double NextDouble()
		{
			byte[] b = new byte[4];
			r.GetBytes(b);
			return (double) BitConverter.ToUInt32(b, 0)/UInt32.MaxValue;
		}

		/// <summary>
		///     Returns a random number within the specified range.
		/// </summary>
		/// <param name=” minValue”>The inclusive lower bound of the random number returned.</param>
		/// <param name=” maxValue”>The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
		public int Next(int minValue, int maxValue)
		{
			return (int) Math.Round(NextDouble()*(maxValue - minValue - 1)) + minValue;
		}

		/// <summary>
		///     Returns a nonnegative random number.
		/// </summary>
		public int Next()
		{
			return Next(0, Int32.MaxValue);
		}

		/// <summary>
		///     Returns a nonnegative random number less than the specified maximum
		/// </summary>
		/// <param name=” maxValue”>The inclusive upper bound of the random number returned. maxValue must be greater than or equal 0</param>
		public int Next(int maxValue)
		{
			return Next(0, maxValue);
		}
	}
}
