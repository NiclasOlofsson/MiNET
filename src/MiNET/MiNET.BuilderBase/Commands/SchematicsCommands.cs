using System;
using System.Collections.Generic;
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

		[Command(Description = "Load a schematics into clipboard")]
		public void Load(Player player, params string[] schematicFile)
		{
			string filename = string.Join(" ", schematicFile);

			NbtFile file = new NbtFile(@"D:\Downloads\schematics\medieval-castle.schematic");
			NbtCompound schematic = file.RootTag;
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
							if (BlockFactory.GetBlockById((byte)blockId).GetType() == typeof(Block))
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
			player.SendMessage("Loaded schematic from file");
		}
	}
}