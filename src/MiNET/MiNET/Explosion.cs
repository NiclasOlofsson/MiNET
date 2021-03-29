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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MiNET.Blocks;
using MiNET.Entities.World;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET
{
	public class Explosion
	{
		private const int Ray = 16;
		private readonly IDictionary<BlockCoordinates, Block> _afectedBlocks = new Dictionary<BlockCoordinates, Block>();
		private readonly float _size;
		private readonly Level _world;
		private BlockCoordinates _centerCoordinates;
		private bool CoordsSet = false;
		private bool Fire = false;

		/// <summary>
		///     Use this for Explosion an explosion only!
		/// </summary>
		/// <param name="world"></param>
		/// <param name="centerCoordinates"></param>
		/// <param name="size"></param>
		/// <param name="fire"></param>
		public Explosion(Level world, BlockCoordinates centerCoordinates, float size, bool fire = false)
		{
			_size = size;
			_centerCoordinates = centerCoordinates;
			_world = world;
			CoordsSet = true;
			Fire = fire;
		}

		/// <summary>
		///     Only use this for SpawnTNT!
		/// </summary>
		public Explosion()
		{
			CoordsSet = false;
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
			if (!CoordsSet) throw new Exception("Please intiate using Explosion(Level, coordinates, size)");
			if (_size < 0.1) return false;

			for (int i = 0; i < Ray; i++)
			{
				for (int j = 0; j < Ray; j++)
				{
					for (int k = 0; k < Ray; k++)
					{
						if (i == 0 || i == Ray - 1 || j == 0 || j == Ray - 1 || k == 0 || k == Ray - 1)
						{
							double x = i / (Ray - 1.0F) * 2.0F - 1.0F;
							double y = j / (Ray - 1.0F) * 2.0F - 1.0F;
							double z = k / (Ray - 1.0F) * 2.0F - 1.0F;
							double dist = Math.Sqrt(x * x + y * y + z * z);

							x /= dist;
							y /= dist;
							z /= dist;
							float blastForce1 = (float) (_size * (0.7F + _world.Random.NextDouble() * 0.6F));

							double cX = _centerCoordinates.X;
							double cY = _centerCoordinates.Y;
							double cZ = _centerCoordinates.Z;

							//for (float blastForce2 = 0.3F; blastForce1 > 0.0F; blastForce1 -= blastForce2*0.75F)
							for (float blastForce2 = 0.3F; blastForce1 > 0.0F; blastForce1 -= 0.225f)
							{
								var bx = (int) Math.Floor(cX);
								var by = (int) Math.Floor(cY);
								var bz = (int) Math.Floor(cZ);
								Block block = _world.GetBlock(bx, by, bz);

								if (!(block is Air))
								{
									float blastForce3 = block.BlastResistance / 5f;
									blastForce1 -= (blastForce3 + 0.3F) * 0.3f;
								}

								if (blastForce1 > 0.0F)
								{
									if (!_afectedBlocks.ContainsKey(block.Coordinates) && !(block is Air)) _afectedBlocks.Add(block.Coordinates, block);
								}

								cX += x * blastForce2;
								cY += y * blastForce2;
								cZ += z * blastForce2;
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

			//new Task(() =>
			//{
			//	var mcpeExplode = McpeExplode.CreateObject();
			//	mcpeExplode.position = _centerCoordinates;
			//	mcpeExplode.radius = (int) (_size * 32);
			//	mcpeExplode.records = records;
			//	_world.RelayBroadcast(mcpeExplode);
			//}).Start();

			foreach (Block block in _afectedBlocks.Values)
			{
				Block block1 = block;
				_world.SetAir(block1.Coordinates);
				//new Task(() => _world.SetBlock(new Air {Coordinates = block1.Coordinates})).Start();
				//new Task(() => block1.BreakBlock(_world)).Start();
				if (block is Tnt)
				{
					new Task(() => SpawnTNT(block1.Coordinates, _world)).Start();
				}
			}

			// Set stuff on fire
			if (Fire)
			{
				Random random = new Random();
				foreach (BlockCoordinates coord in _afectedBlocks.Keys)
				{
					var block = _world.GetBlock(coord.X, coord.Y, coord.Z);
					if (block is Air)
					{
						var blockDown = _world.GetBlock(coord.X, coord.Y - 1, coord.Z);
						if (!(blockDown is Air) && random.Next(3) == 0)
						{
							_world.SetBlock(new Fire {Coordinates = block.Coordinates});
						}
					}
				}
			}

			return true;
		}

		private void SpawnTNT(BlockCoordinates blockCoordinates, Level world)
		{
			var rand = new Random();
			new PrimedTnt(world)
			{
				KnownPosition = new PlayerLocation
				{
					X = blockCoordinates.X + 0.5f,
					Y = blockCoordinates.Y + 0.5f,
					Z = blockCoordinates.Z + 0.5f,
				},
				Fuse = (byte) (rand.Next(0, 20) + 10)
			}.SpawnEntity();
		}
	}
}