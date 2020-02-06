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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public class SchematicsCommands : UndoableCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (SchematicsCommands));

		[Command(Name = "schematic list", Description = "List all schematics on server")]
		public string Schematic(Player player)
		{
			string[] list = Directory.EnumerateFiles(@"D:\Downloads\schematics", "*.schematic").ToArray();
			for (int i = 0; i < list.Length; i++)
			{
				list[i] = Path.GetFileNameWithoutExtension(list[i]);
			}

			return $"Schematics: {string.Join(", ", list)}";
		}

		[Command(Name = "schematic load", Description = "Load a schematics into clipboard")]
		public string Load(Player player, string schematicFile)
		{
			string filePath = Path.Combine(@"D:\Downloads\schematics", schematicFile);
			if (!File.Exists(filePath)) return $"Sorry, this schematics did not exist <{schematicFile}>";

			NbtFile file = new NbtFile(filePath);
			NbtCompound schematic = (NbtCompound) file.RootTag;
			var width = schematic["Width"].ShortValue;
			var length = schematic["Length"].ShortValue;
			var height = schematic["Height"].ShortValue;
			bool useAlpha = schematic["Materials"].StringValue.Equals("Alpha");
			byte[] blocks = schematic["Blocks"].ByteArrayValue;
			byte[] blockData = schematic["Data"].ByteArrayValue;

			var buffer = new List<Block>();
			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					for (int z = 0; z < length; ++z)
					{
						int index = y*width*length + z*width + x;
						BlockCoordinates coord = new BlockCoordinates(x, y, z);

						if (blocks[index] == 0) continue;

						int blockId = blocks[index];
						byte data = blockData[index];

						Func<int, byte, byte> dataConverter = (i, b) => b; // Default no-op converter
						if (AnvilWorldProvider.Convert.ContainsKey(blockId))
						{
							dataConverter = AnvilWorldProvider.Convert[blockId].Item2;
							blockId = AnvilWorldProvider.Convert[blockId].Item1;
						}
						else
						{
							if (BlockFactory.GetBlockById((byte) blockId).GetType() == typeof (Block))
							{
								Log.Warn($"No block implemented for block ID={blockId}, Meta={data}");
								//blockId = 57;
							}
						}

						if (blockId > 255)
						{
							Log.Warn($"Failed mapping for block ID={blockId}, Meta={data}");
							blockId = 41;
						}

						var metadata = dataConverter(blockId, data);

						Block block = BlockFactory.GetBlockById((byte) blockId);
						block.Coordinates = coord;
						block.Metadata = metadata;
						buffer.Add(block);
					}
				}
			}

			//NbtList blockEntities = schematic["TileEntities"] as NbtList;
			//if (blockEntities != null)
			//{
			//	foreach (var blockEntity in blockEntities)
			//	{
			//	}
			//}

			Clipboard clipboard = new Clipboard(player.Level, buffer);
			clipboard.Origin = new BlockCoordinates(clipboard.GetMin());
			Selector.Clipboard = clipboard;

			return $"Loaded schematic from file <{schematicFile}>";
		}
	}
}