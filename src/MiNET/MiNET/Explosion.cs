using System;
using System.Collections.Generic;
using Craft.Net.Common;
using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET
{
	internal class Explosion
	{
		private List<Block> _afectedBlocks = new List<Block>();
		private const double StepLen = 0.3;
		private const int Ray = 16;
		private double _size = 0;
		private readonly Level _world;
		private Coordinates3D _centerCoordinates;

		public Explosion(Level world, Coordinates3D centerCoordinates, double size)
		{
			_size = size;
			_centerCoordinates = centerCoordinates;
			_world = world;
		}

		public bool Explode()
		{
			if (ExplosionA())
			{
				return ExplosionB();
			}

			return false;
		}

		private bool ExplosionA()
		{
			if (_size < 0.1) return false;

			var vector = new Vector3(0, 0, 0);
			var bvector = new Vector3(0, 0, 0);
			for (var i = 0; i < Ray; i++)
			{
				for (var j = 0; j < Ray; j++)
				{
					for (var k = 0; k < Ray; k++)
					{
						if (i == 0 || i == Ray - 1 || j == 0 || j == Ray - 1 || k == 0 || k == Ray - 1)
						{
							float ray = Ray;
							double x = (double) ((float) i/((float) ray - 1.0F)*2.0F - 1.0F);
							double y = (double) ((float) j/((float) ray - 1.0F)*2.0F - 1.0F);
							double z = (double) ((float) k/((float) ray - 1.0F)*2.0F - 1.0F);
							double d6 = Math.Sqrt(x*x + y*y + z*z);

							x /= d6;
							y /= d6;
							z /= d6;
							float blastForce1 = (float) (_size*(0.7F + new Random().NextDouble()*0.6F));

							double cX = _centerCoordinates.X;
							double cY = _centerCoordinates.Y;
							double cZ = _centerCoordinates.Z;

							for (float blastForce2 = 0.3F; blastForce1 > 0.0F; blastForce1 -= blastForce2*0.75F)
							{
								int bx = (int) Math.Floor(cX);
								int by = (int) Math.Floor(cY);
								int bz = (int) Math.Floor(cZ);
								Block block = _world.GetBlock(bx, by, bz);

								if (block.Id != 0)
								{
									float blastForce3 = block.GetHardness();
									blastForce1 -= (blastForce3 + 0.3F)*blastForce2;
								}

								if (blastForce1 > 0.0F)
								{
									_afectedBlocks.Add(block);
								}

								cX += x*blastForce2;
								cY += y*blastForce2;
								cZ += z*blastForce2;
							}
						}
					}
				}
			}

			_size *= 2.0F;
			return true;
		}

		private bool ExplosionB()
		{
			Vector3 source = new Vector3(_centerCoordinates.X, _centerCoordinates.Y, _centerCoordinates.Z).Floor();
			var yield = (1/_size)*100;
			var explosionSize = _size*2;
			var minX = Math.Floor(_centerCoordinates.X - explosionSize - 1);
			var maxX = Math.Floor(_centerCoordinates.X + explosionSize + 1);
			var minY = Math.Floor(_centerCoordinates.Y - explosionSize - 1);
			var maxY = Math.Floor(_centerCoordinates.Y + explosionSize + 1);
			var minZ = Math.Floor(_centerCoordinates.Z - explosionSize - 1);
			var maxZ = Math.Floor(_centerCoordinates.Z + explosionSize + 1);
			var explosionBB = new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));
			var Air = new BlockAir();

			foreach (var block in _afectedBlocks)
			{
				_world.SetBlock(new BlockAir() {Coordinates = block.Coordinates});
			}
			return true;
		}

		private double VectorLength(Vector3 vector)
		{
			return Math.Sqrt(vector.X*vector.X + vector.Y*vector.Y + vector.Z*vector.Z);
		}
	}
}