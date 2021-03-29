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
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Commands;
using MiNET.BuilderBase.Masks;
using MiNET.BuilderBase.Patterns;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;
using Newtonsoft.Json;

namespace MiNET.BuilderBase.Tools
{
	public class BrushTool : ItemIronShovel
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BrushTool));

		public int Radius { get; set; } = 4;
		public int Height { get; set; } = 1;
		public int Range { get; set; } = 300;
		[JsonIgnore]
		public Pattern Pattern { get; set; } = new Pattern(1, 0);
		[JsonIgnore]
		public Mask Mask { get; set; } = new AnyBlockMask();
		public bool Filled { get; set; } = true;

		public int BrushType { get; set; } = 0;


		private NbtCompound _extraData;

		public override NbtCompound ExtraData
		{
			get
			{
				UpdateExtraData();
				return _extraData;
			}
			set { _extraData = value; }
		}

		private void UpdateExtraData()
		{
			string s = string.Empty;
			switch (BrushType)
			{
				case 0:
					s = "Sphere";
					break;
				case 1:
					s = "Cylinder";
					break;
				case 2:
					s = "Melt";
					break;
				case 3:
					s = "Fill";
					break;
			}
			_extraData = new NbtCompound
			{
				{
					new NbtCompound("display")
					{
						new NbtString("Name", ChatFormatting.Reset + ChatColors.Yellow + $"{s} Brush"),
						new NbtList("Lore")
						{
							new NbtString(ChatFormatting.Reset + ChatFormatting.Italic + ChatColors.White + $"A {s.ToLower()} brush."),
							new NbtString(ChatFormatting.Reset + ChatColors.Green + "Mask: " + Mask.OriginalMask),
							new NbtString(ChatFormatting.Reset + ChatColors.Green + "Pattern: " + Pattern.OriginalPattern),
							new NbtString(ChatFormatting.Reset + ChatColors.Green + "Radius: " + Radius),
						}
					}
				}
			};
		}


		public void UpdateDisplay(Player player)
		{
			player.Inventory.SendSetSlot(player.Inventory.InHandSlot);
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			player.Inventory.SendSetSlot(player.Inventory.InHandSlot);
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			player.Inventory.SendSetSlot(player.Inventory.InHandSlot);

			var selector = RegionSelector.GetSelector(player);
			var undoRecorder = new UndoRecorder(player.Level);
			var editSession = new EditHelper(player.Level, player, Mask, undoRecorder: undoRecorder);

			var target = new EditHelper(world, player, Mask).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return;
			}

			try
			{
				switch (BrushType)
				{
					case 0:
						editSession.MakeSphere(target.Coordinates, Pattern, Radius, Radius, Radius, Filled);
						break;
					case 1:
						editSession.MakeCylinder(target.Coordinates, Pattern, Radius, Radius, Height, Filled);
						break;
					case 2:

						// public ErosionParameters(final int erosionFaces, final int erosionRecursion, final int fillFaces, final int fillRecursion) {
						//MELT(new ErosionParameters(2, 1, 5, 1)),
						//FILL(new ErosionParameters(5, 1, 2, 1)),
						//SMOOTH(new ErosionParameters(3, 1, 3, 1)),
						//LIFT(new ErosionParameters(6, 0, 1, 1)),
						//FLOATCLEAN(new ErosionParameters(6, 1, 6, 1));

						Erosion(editSession, Radius, 2, 1, 5, 1, world, target.Coordinates); // melt
						break;
					case 3:

						Erosion(editSession, Radius, 5, 1, 2, 1, world, target.Coordinates); // fill
						break;
				}
			}
			catch (Exception e)
			{
				Log.Error("Brush use item", e);
			}

			var history = undoRecorder.CreateHistory();
			selector.AddHistory(history);
		}

		public override bool Animate(Level world, Player player)
		{
			player.Inventory.SendSetSlot(player.Inventory.InHandSlot);

			var selector = RegionSelector.GetSelector(player);
			var undoRecorder = new UndoRecorder(player.Level);
			var editSession = new EditHelper(player.Level, player, Mask, undoRecorder: undoRecorder);

			var target = new EditHelper(world, player).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return false;
			}

			try
			{
				switch (BrushType)
				{
					case 0:
						editSession.MakeSphere(target.Coordinates, Pattern, Radius, Radius, Radius, Filled);
						break;
					case 1:
						editSession.MakeCylinder(target.Coordinates, Pattern, Radius, Radius, Height, Filled);
						break;
					case 3:

						// public ErosionParameters(final int erosionFaces, final int erosionRecursion, final int fillFaces, final int fillRecursion) {
						//MELT(new ErosionParameters(2, 1, 5, 1)),
						//FILL(new ErosionParameters(5, 1, 2, 1)),
						//SMOOTH(new ErosionParameters(3, 1, 3, 1)),
						//LIFT(new ErosionParameters(6, 0, 1, 1)),
						//FLOATCLEAN(new ErosionParameters(6, 1, 6, 1));

						Erosion(editSession, Radius, 2, 1, 5, 1, world, target.Coordinates); // melt
						break;
					case 2:

						Erosion(editSession, Radius, 5, 1, 2, 1, world, target.Coordinates); // fill
						break;
				}
			}
			catch (Exception e)
			{
				Log.Error("Brush use item", e);
			}

			var history = undoRecorder.CreateHistory();
			selector.AddHistory(history);

			return true;
		}

		private class BlockBuffer : Dictionary<BlockCoordinates, Block>
		{
		}

		List<BlockCoordinates> FACES_TO_CHECK = new List<BlockCoordinates> {Level.South, Level.North, Level.East, Level.West, Level.Up, Level.Down};

		protected void Erosion(EditHelper editSession, int brushSize, int erodeFaces, int erodeRec, int fillFaces, int fillRec, Level level, BlockCoordinates targetBlock)
		{
			double brushSizeSquared = brushSize*brushSize;

			int tx = targetBlock.X;
			int ty = targetBlock.Y;
			int tz = targetBlock.Z;

			BlockBuffer buffer1 = new BlockBuffer();
			BlockBuffer buffer2 = new BlockBuffer();

			for (int x = -brushSize - 1; x <= brushSize + 1; x++)
			{
				int x0 = x + tx;
				for (int y = -brushSize - 1; y <= brushSize + 1; y++)
				{
					int y0 = y + ty;
					for (int z = -brushSize - 1; z <= brushSize + 1; z++)
					{
						int z0 = z + tz;
						var block = level.GetBlock(new BlockCoordinates(x0, y0, z0));
						buffer1[new BlockCoordinates(x, y, z)] = block;
						buffer2[new BlockCoordinates(x, y, z)] = block;
					}
				}
			}

			int swap = 0;
			for (int i = 0; i < erodeRec; ++i)
			{
				ErosionIteration(brushSize, erodeFaces, swap%2 == 0 ? buffer1 : buffer2, swap%2 == 1 ? buffer1 : buffer2);
				swap++;
			}

			for (int i = 0; i < fillRec; ++i)
			{
				FillIteration(brushSize, fillFaces, swap%2 == 0 ? buffer1 : buffer2, swap%2 == 1 ? buffer1 : buffer2);
				swap++;
			}

			BlockBuffer finalBuffer = swap%2 == 0 ? buffer1 : buffer2;

			// apply the buffer to the world
			for (int x = -brushSize; x <= brushSize; x++)
			{
				int x0 = x + tx;
				for (int y = -brushSize; y <= brushSize; y++)
				{
					int y0 = y + ty;
					for (int z = -brushSize; z <= brushSize; z++)
					{
						int z0 = z + tz;
						var coord = new BlockCoordinates(x, y, z);
						if (x*x + y*y + z*z <= brushSizeSquared && finalBuffer.ContainsKey(coord))
						{
							var block = finalBuffer[coord];
							block.Coordinates = new BlockCoordinates(x0, y0, z0);
							Block old = level.GetBlock(block.Coordinates);

							if (block.Id == old.Id && block.Metadata == old.Metadata) continue;

							editSession.SetBlock(block);
						}
					}
				}
			}
		}

		private void FillIteration(int inbrushSize, int fillFaces, BlockBuffer current, BlockBuffer target)
		{
			double brushSizeSquared = inbrushSize*inbrushSize;
			int brushSize = inbrushSize + 1;
			Dictionary<int, int> frequency = new Dictionary<int, int>();

			for (int x = -brushSize; x <= brushSize; x++)
			{
				for (int y = -brushSize; y <= brushSize; y++)
				{
					for (int z = -brushSize; z <= brushSize; z++)
					{
						BlockCoordinates coord = new BlockCoordinates(x, y, z);
						target[coord] = current[coord];
						if (x*x + y*y + z*z >= brushSizeSquared)
						{
							continue;
						}

						Block state = current[coord];
						if (state.IsSolid)
						{
							continue;
						}

						int total = 0;
						int highest = 1;
						Block highestState = state;
						frequency.Clear();
						foreach (var offs in FACES_TO_CHECK)
						{
							Block next = current[coord + offs];
							if (!next.IsSolid)
							{
								continue;
							}

							total++;

							int count;
							if (!frequency.ContainsKey(next.Id))
							{
								count = 1;
							}
							else
							{
								count = frequency[next.Id];
								count++;
							}

							if (count >= highest)
							{
								highest = count;
								highestState = next;
							}
							frequency[next.Id] = count;
						}

						if (total >= fillFaces)
						{
							target[coord] = highestState;
						}
					}
				}
			}
		}

		private void ErosionIteration(int inbrushSize, int erodeFaces, BlockBuffer current, BlockBuffer target)
		{
			double brushSizeSquared = inbrushSize*inbrushSize;
			int brushSize = inbrushSize + 1;
			Dictionary<int, int> frequency = new Dictionary<int, int>();

			for (int x = -brushSize; x <= brushSize; x++)
			{
				for (int y = -brushSize; y <= brushSize; y++)
				{
					for (int z = -brushSize; z <= brushSize; z++)
					{
						BlockCoordinates coord = new BlockCoordinates(x, y, z);
						target[coord] = (Block) current[coord];

						if (x*x + y*y + z*z >= brushSizeSquared)
						{
							continue;
						}
						Block state = current[coord];
						if (state is Air)
						{
							continue;
						}
						int total = 0;
						int highest = 1;
						int highestState = state.Id;
						frequency.Clear();
						foreach (var offs in FACES_TO_CHECK)
						{
							Block next = current[coord + offs];
							if (!(next is Air))
							{
								continue;
							}
							total++;

							int count;
							if (!frequency.ContainsKey(next.Id))
							{
								count = 1;
							}
							else
							{
								count = frequency[next.Id];
								count++;
							}

							if (count > highest)
							{
								highest = count;
								highestState = next.Id;
							}
							frequency[next.Id] = count;
						}
						if (total > erodeFaces)
						{
							target[coord] = BlockFactory.GetBlockById(highestState);
						}
					}
				}
			}
		}

		private Block Default(Type t)
		{
			var ctor = t.GetConstructor(Type.EmptyTypes);
			return (Block) ctor.Invoke(null);
		}
	}
}