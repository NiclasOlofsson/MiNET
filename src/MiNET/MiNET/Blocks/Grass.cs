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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Grass : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Grass));

		public Grass() : base(2)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override void DoPhysics(Level level)
		{
			if (level.GameMode == GameMode.Creative) return;

			if (level.GetSubtractedLight(Coordinates.BlockUp()) < 4)
			{
				Block dirt = BlockFactory.GetBlockById(3);
				dirt.Coordinates = Coordinates;
				level.SetBlock(dirt, true, false, false);
			}
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (level.GameMode == GameMode.Creative) return;
			if (!isRandom) return;

			var lightLevel = level.GetSubtractedLight(Coordinates.BlockUp());
			if (lightLevel < 4 /* && check opacity */)
			{
				Block dirt = BlockFactory.GetBlockById(3);
				dirt.Coordinates = Coordinates;
				level.SetBlock(dirt, true, false, false);
			}
			else
			{
				if (lightLevel >= 9)
				{
					Random random = new Random();
					for (int i = 0; i < 4; i++)
					{
						var coordinates = Coordinates + new BlockCoordinates(random.Next(3) - 1, random.Next(5) - 3, random.Next(3) - 1);
						if (level.GetBlock(coordinates) is Dirt next && next.DirtType == "normal")
						{
							Block nextUp = level.GetBlock(coordinates.BlockUp());
							if (nextUp.IsTransparent && (nextUp.BlockLight >= 4 || nextUp.SkyLight >= 4))
							{
								level.SetBlock(new Grass {Coordinates = coordinates});
							}
						}
					}
				}
			}
		}

		public override bool Interact(Level level, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			var itemInHand = player.Inventory.GetItemInHand();
			if (itemInHand is ItemDye && itemInHand.Metadata == 15)
			{
				// If bone meal is used on a grass block, 0–8(double) tall grass, 8–24 grass and 0–8 flowers form on the
				// targeted block and on randomly-selected adjacent grass blocks up to 7 blocks away (taxicab distance).
				// The flowers that appear are dependent on the biome, meaning that in order to obtain specific flowers,
				// the player must travel to biomes where the flowers are found naturally. See Flower § Flower biomes
				// for more information.
				//TODO: Grow grass and flowers randomly
				var random = new RandomWeighted<int>(new List<RandomRange<int>>()
				{
					new RandomRange<int>(0, 216),
					new RandomRange<int>(1, 24),
					new RandomRange<int>(2, 8),
					new RandomRange<int>(3, 8),
				});

				int grassPlanted = 0;
				int flowersPlanted = 0;

				var rnd = new Random();
				for (int i = 0; i < 128; i++)
				{
					BlockCoordinates coord = blockCoordinates;
					bool shouldContinue = false;
					for (int j = 0; j < i / 16; j++)
					{
						coord += new BlockCoordinates(rnd.Next(3) - 1, (rnd.Next(3) - 1) * (rnd.Next(3) / 2), rnd.Next(3) - 1);
						if (!level.GetBlock(coord).IsSolid)
						{
							shouldContinue = true;
							break;
						}
					}
					if (shouldContinue) continue;

					if (!(level.GetBlock(coord) is Grass)) continue;
					coord += BlockCoordinates.Up;
					Block growthBlock = level.GetBlock(coord);

					if (growthBlock is Tallgrass tallGrass)
					{
						if (grassPlanted >= 24) continue;

						if (tallGrass.TallGrassType == "default" || tallGrass.TallGrassType == "tall")
						{
							if (rnd.Next(10) == 0)
							{
								var block = new DoublePlant();
								block.DoublePlantType = "grass";
								block.Coordinates = coord;
								level.SetBlock(block);
								grassPlanted++;
							}
						}
					} else if (growthBlock is Air)
					{
						if (rnd.Next(8) == 0)
						{
							if(flowersPlanted >= 8) continue;

							Block block = null;
							int biomeId = level.GetBiomeId(coord);
							switch (biomeId)
							{ 
								// [StateEnum(
								// "tulip_pink",
								// "houstonia",
								// "lily_of_the_valley",
								// "tulip_white",
								// "allium",
								// "tulip_red",
								// "poppy",
								// "cornflower",
								// "tulip_orange",
								// "oxeye",
								// "orchid")]
								case 1: // plains
								{
									if (rnd.Next(2) == 0)
									{
										var flower = new RedFlower();
										flower.FlowerType = "poppy";
										block = flower;
									}
									else
									{
										var flower = new YellowFlower();
										block = flower;
									}
									break;
								}
								default:
									break;
							}
							if(block != null)
							{
								block.Coordinates = coord;
								level.SetBlock(block);
							}

							flowersPlanted++;
						}
						else
						{
							if(grassPlanted >= 24) continue;

							var block = new Tallgrass();
							block.TallGrassType = rnd.Next(10) != 0 ? "tall" : "fern";
							block.Coordinates = coord;
							level.SetBlock(block);
							grassPlanted++;
						}
					}
				}

				return true;
			}

			return false; // not handled
		}

		private void DoSpawn()
		{
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {new ItemBlock(new Dirt(), 0) {Count = 1}}; //Drop dirt block
		}
	}

	public class RandomWeighted<T>
	{
		private readonly List<RandomRange<T>> _items;
		private Random _random;

		public RandomWeighted(List<RandomRange<T>> items)
		{
			_items = items;
			_random = new Random();
		}

		public T Next()
		{
			int targetWeight = _random.Next(_items.Sum(i => i.Weight) + 1);
			int currentWeight = 0;
			foreach (RandomRange<T> range in _items)
			{
				currentWeight += range.Weight;

				if (targetWeight < currentWeight) return range.Item;
			}

			return default;
		}
	}

	public class RandomRange<T>
	{
		public T Item { get; }
		public int Weight { get; }

		public RandomRange(T item, int weight)
		{
			Item = item;
			Weight = weight;
		}
	}
}