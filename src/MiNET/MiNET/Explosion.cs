using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Craft.Net.Common;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	internal class Explosion
	{
		private const int Ray = 16;
		private readonly IDictionary<Coordinates3D, Block> _afectedBlocks = new Dictionary<Coordinates3D, Block>();
		private readonly float _size;
		private readonly Level _world;
		private Coordinates3D _centerCoordinates;

		public Explosion(Level world, Coordinates3D centerCoordinates, float size)
		{
			_size = size;
			_centerCoordinates = centerCoordinates;
			_world = world;
		}

		public bool Explode()
		{
			if (PrimaryExplosion())
			{
				return SecondaryExplosion();
			}

			return false;
		}

		private bool PrimaryExplosion()
		{
			if (_size < 0.1) return false;

			for (int i = 0; i < Ray; i++)
			{
				for (int j = 0; j < Ray; j++)
				{
					for (int k = 0; k < Ray; k++)
					{
						if (i == 0 || i == Ray - 1 || j == 0 || j == Ray - 1 || k == 0 || k == Ray - 1)
						{
							double x = i/(Ray - 1.0F)*2.0F - 1.0F;
							double y = j/(Ray - 1.0F)*2.0F - 1.0F;
							double z = k/(Ray - 1.0F)*2.0F - 1.0F;
							double d6 = Math.Sqrt(x*x + y*y + z*z);

							x /= d6;
							y /= d6;
							z /= d6;
							var blastForce1 = (float) (_size*(0.7F + new Random().NextDouble()*0.6F));

							double cX = _centerCoordinates.X;
							double cY = _centerCoordinates.Y;
							double cZ = _centerCoordinates.Z;

							for (float blastForce2 = 0.3F; blastForce1 > 0.0F; blastForce1 -= blastForce2*0.75F)
							{
								var bx = (int) Math.Floor(cX);
								var by = (int) Math.Floor(cY);
								var bz = (int) Math.Floor(cZ);
								Block block = _world.GetBlock(bx, by, bz);

								if (block.Id != 0)
								{
									float blastForce3 = block.GetHardness();
									blastForce1 -= (blastForce3 + 0.3F)*blastForce2;
								}

								if (blastForce1 > 0.0F)
								{
									if (!_afectedBlocks.ContainsKey(block.Coordinates) && block.Id != 0) _afectedBlocks.Add(block.Coordinates, block);
								}

								cX += x*blastForce2;
								cY += y*blastForce2;
								cZ += z*blastForce2;
							}
						}
					}
				}
			}

			//_size *= 2.0F;
			return true;
		}

		private bool SecondaryExplosion()
		{
			//Vector3 source = new Vector3(_centerCoordinates.X, _centerCoordinates.Y, _centerCoordinates.Z).Floor();
			//var yield = (1/_size)*100;
			//var explosionSize = _size*2;
			//var minX = Math.Floor(_centerCoordinates.X - explosionSize - 1);
			//var maxX = Math.Floor(_centerCoordinates.X + explosionSize + 1);
			//var minY = Math.Floor(_centerCoordinates.Y - explosionSize - 1);
			//var maxY = Math.Floor(_centerCoordinates.Y + explosionSize + 1);
			//var minZ = Math.Floor(_centerCoordinates.Z - explosionSize - 1);
			//var maxZ = Math.Floor(_centerCoordinates.Z + explosionSize + 1);
			//var explosionBB = new BoundingBox(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));

			var records = new Records();
			foreach (Block block in _afectedBlocks.Values)
			{
				records.Add(block.Coordinates - _centerCoordinates);
			}

			new Task(() =>
				_world.RelayBroadcast(new McpeExplode
				{
					x = _centerCoordinates.X,
					y = _centerCoordinates.Y,
					z = _centerCoordinates.Z,
					radius = _size,
					records = records
				})).Start();

			//For some reason we need to keep this list
			//Seems otherwise we would get duplicated. TODO: Fix!
			foreach (Block block in _afectedBlocks.Values)
			{
				Block block1 = block;
				new Task(() => _world.SetBlock(new Air {Coordinates = block1.Coordinates})).Start();

				if (block.Id == 46)
				{
					new Task(() => SpawnTNT(block1.Coordinates, _world)).Start();
				}
			}

			return true;
		}

		private void SpawnTNT(Coordinates3D blockCoordinates, Level world)
		{
			var rand = new Random();
			new PrimedTnt(world)
			{
				KnownPosition = new PlayerPosition3D
				{
					X = blockCoordinates.X,
					Y = blockCoordinates.Y,
					Z = blockCoordinates.Z,
				},
				Fuse = (byte) (rand.Next(0, 20) + 10)
			}.SpawnEntity();
		}
	}
}