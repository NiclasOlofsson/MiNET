using System;
using System.Collections.Generic;
using System.Text;
using LibNoise;
using LibNoise.Filter;
using LibNoise.Modifier;
using LibNoise.Primitive;
using MiNET.Utils;
using MiNET.Utils.Noise;

namespace MiNET.Worlds.Generators.Survival
{
	public class NetherGenerator : IWorldGenerator
	{
		private IModule3D MainNoise { get; }
		public NetherGenerator()
		{
			int seed = (int)(OverworldGenerator.CalculateHash(Config.GetProperty("seed", "YoHoMotherducker!")) % int.MaxValue);
			var noise1 = new OpenSimplexNoise(seed);
			var noise2 = new SumFractal()
			{
				Primitive3D = noise1,
				OctaveCount = 8,
				Frequency = 0.0285f,
			};

			var noise3 = new HybridMultiFractal()
			{
				Frequency = 0.28f,
				Primitive3D = noise1,
				OctaveCount = 4
			};

			var noise4 = new Blend(new RidgedMultiFractal()
			{
				Primitive3D = new OpenSimplexNoise(seed - 20)
			}, noise2, noise3);
				//var noise4 = new NoiseBlender(new RidgedMultiFractal()
			//{
			//	Primitive3D = new OpenSimplexNoise(seed - 20)
			//}, noise2, noise3);

			MainNoise = new ScaleableNoise()
			{
				Primitive3D = noise4,
				XScale = 1 / 35f,
				YScale = 1 / 35f,
				ZScale = 1 / 35f
			};
		}

		public void Initialize()
		{

		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			ChunkColumn chunk = new ChunkColumn();
			chunk.x = chunkCoordinates.X;
			chunk.z = chunkCoordinates.Z;

			double threshold = 0.5;
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					int realX = x + chunkCoordinates.X * 16;
					int realZ = z + chunkCoordinates.Z * 16;
					for (int y = 1; y < 32; y++)
					{
						chunk.SetBlock(x, y, z, 11);
					}

					for (int y = 1; y < 255; y++)
					{
						if (MainNoise.GetValue(realX, y, realZ) > threshold)
						{
							chunk.SetBlock(x, y, z, 87);
						}
					}

					chunk.SetBlock(x, 0, z, 7);
					chunk.SetBlock(x, 255, z, 7);
				}
			}

			return chunk;
		}
	}
}
