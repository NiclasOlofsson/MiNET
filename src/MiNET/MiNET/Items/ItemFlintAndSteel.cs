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
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.World;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFlintAndSteel : Item
	{
		public static int MaxPortalHeight = 30;
		public static int MaxPortalWidth = 30;

		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFlintAndSteel));

		public ItemFlintAndSteel() : base("minecraft:flint_and_steel", 259)
		{
			MaxStackSize = 1;
			ItemType = ItemType.FlintAndSteel;
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var block = world.GetBlock(blockCoordinates);
			if (block is Tnt)
			{
				world.SetAir(block.Coordinates);
				new PrimedTnt(world)
				{
					KnownPosition = new PlayerLocation
					{
						X = blockCoordinates.X + 0.5f,
						Y = blockCoordinates.Y + 0.5f,
						Z = blockCoordinates.Z + 0.5f,
					},
					Fuse = 80
				}.SpawnEntity();
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
			}
			else if (block is Obsidian)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, face));
				if (affectedBlock.Id == 0)
				{
					var blocks = Fill(world, affectedBlock.Coordinates, 10, BlockFace.West);
					if (blocks.Count == 0)
					{
						blocks = Fill(world, affectedBlock.Coordinates, 10, BlockFace.North);
					}

					if (blocks.Count > 0)
					{
						foreach (var portal in blocks.FindAll(b => b is Portal))
						{
							world.SetBlock(portal);
						}
					}
					else
					{
						if (face == BlockFace.Up)
						{
							affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
							if (affectedBlock.Id == 0)
							{
								var fire = new Fire {Coordinates = affectedBlock.Coordinates};
								world.SetBlock(fire);
							}
						}
					}
				}
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
			}
			else if (block.IsSolid)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
				if (affectedBlock.Id == 0)
				{
					var fire = new Fire {Coordinates = affectedBlock.Coordinates};
					world.SetBlock(fire);
				}
				player.Inventory.DamageItemInHand(ItemDamageReason.BlockInteract, null, block);
			}
		}

		public List<Block> Fill(Level level, BlockCoordinates origin, int radius, BlockFace direction)
		{
			var blocks = new List<Block>();
			var length = new Vector2(MaxPortalHeight, MaxPortalWidth).Length();

			Queue<BlockCoordinates> visits = new Queue<BlockCoordinates>();

			visits.Enqueue(origin); // Kick it off with some good stuff

			while (visits.Count > 0)
			{
				var coordinates = visits.Dequeue();

				if (origin.DistanceTo(coordinates) >= length) return new List<Block>();

				if (level.IsAir(coordinates) && blocks.FirstOrDefault(b => b.Coordinates.Equals(coordinates)) == null)
				{
					Visit(coordinates, blocks, direction);

					if (direction == BlockFace.West)
					{
						visits.Enqueue(coordinates + Level.North);
						visits.Enqueue(coordinates + Level.South);
					}
					else if (direction == BlockFace.North)
					{
						visits.Enqueue(coordinates + Level.West);
						visits.Enqueue(coordinates + Level.East);
					}

					visits.Enqueue(coordinates + Level.Up);
					visits.Enqueue(coordinates + Level.Down);
				}
				else
				{
					var block = level.GetBlock(coordinates);
					if (!IsValid(block, blocks)) return new List<Block>();
				}
			}

			return blocks;
		}

		private void Visit(BlockCoordinates coordinates, List<Block> blocks, BlockFace direction)
		{
			var dir = direction switch
			{
				BlockFace.Down  => BlockAxis.X,
				BlockFace.Up    => BlockAxis.X,
				BlockFace.North => BlockAxis.X,
				BlockFace.South => BlockAxis.X,
				BlockFace.West  => BlockAxis.Z,
				BlockFace.East  => BlockAxis.Z,
				BlockFace.None  => default,
				_               => default
			};

			blocks.Add(new Portal
			{
				Coordinates = coordinates,
				PortalAxis = dir.ToString().ToLower()
			});
		}

		private bool IsValid(Block block, List<Block> portals)
		{
			return block is Obsidian || portals.FirstOrDefault(b => b.Coordinates.Equals(block.Coordinates) && b is Portal) != null;
		}

		public override bool DamageItem(Player player, ItemDamageReason reason, Entity target, Block block)
		{
			switch (reason)
			{
				case ItemDamageReason.BlockInteract:
				{
					Metadata++;
					return Metadata >= GetMaxUses() - 1;
				}
				default:
					return false;
			}
		}

		protected override int GetMaxUses()
		{
			return 65;
		}
	}
}