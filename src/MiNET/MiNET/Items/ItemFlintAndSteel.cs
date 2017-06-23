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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Entities.World;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFlintAndSteel : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemFlintAndSteel));

		public ItemFlintAndSteel() : base(259)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var block = world.GetBlock(blockCoordinates);
			if (block is Tnt)
			{
				world.SetBlock(new Air() {Coordinates = block.Coordinates});
				new PrimedTnt(world)
				{
					KnownPosition = new PlayerLocation(blockCoordinates.X, blockCoordinates.Y, blockCoordinates.Z),
					Fuse = (byte) (new Random().Next(0, 20) + 10)
				}.SpawnEntity();
			}
			else if (block is Obsidian)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, face));
				if (affectedBlock.Id == 0)
				{
					List<Portal> blocks = new List<Portal>();
					int count = Fill(world, affectedBlock, 0, BlockFace.North, blocks);
					if (count < 6 || count > 23*23)
					{
						blocks.Clear();
						count = Fill(world, affectedBlock, 0, BlockFace.East, blocks);
						if (count < 6 || count > 23*23)
						{
							blocks.Clear();
						}
					}
					foreach (var portal in blocks)
					{
						world.SetBlock(portal);
					}
				}
			}
			else if (block.IsSolid)
			{
				var affectedBlock = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Up));
				if (affectedBlock.Id == 0)
				{
					var fire = new Fire
					{
						Coordinates = affectedBlock.Coordinates
					};
					world.SetBlock(fire);
				}
			}
		}

		private int Fill(Level level, Block currentBlock, int count, BlockFace direction, List<Portal> blocks)
		{
			if (count > 23*23) return count;

			if (IsValid(level, currentBlock.Coordinates, blocks) != 0) return count;

			count++;

			if (!(currentBlock is Air))
			{
				return count + 23*23;
			}

			blocks.Add(new Portal {Coordinates = currentBlock.Coordinates});

			int c = 0;
			c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.Up, blocks);
			c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.Down, blocks);
			if (direction == BlockFace.East)
			{
				c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.West, blocks);
				c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.East, blocks);
			}
			else
			{
				c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.South, blocks);
				c += IsValid(level, currentBlock.Coordinates + BlockCoordinates.North, blocks);
			}

			if (c == 0 || c >= 30)
			{
				return count + 23*23;
			}

			count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.Up), count, direction, blocks);
			count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.Down), count, direction, blocks);
			if (direction == BlockFace.East)
			{
				count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.West), count, direction, blocks);
				count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.East), count, direction, blocks);
			}
			else
			{
				count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.South), count, direction, blocks);
				count = Fill(level, level.GetBlock(currentBlock.Coordinates + BlockCoordinates.North), count, direction, blocks);
			}
			return count;
		}

		private int IsValid(Level level, BlockCoordinates coord, List<Portal> portals)
		{
			Block block = level.GetBlock(coord);
			return block is Obsidian ? 10 : portals.FirstOrDefault(b => b.Coordinates.Equals(coord)) != null ? 1 : 0;
		}
	}
}