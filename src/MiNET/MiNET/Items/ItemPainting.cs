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
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemPainting : Item
	{
		public class PaintingData
		{
			public string Title { get; }

			public int Width { get; } = 4;
			public int WidthOffset { get; } = 1;
			public int Height { get; } = 4;
			public int HeightOffset { get; } = 1;

			public PaintingData(string title, int width, int widthOffset, int height, int heightOffset)
			{
				Title = title;
				Width = width;
				WidthOffset = widthOffset;
				Height = height;
				HeightOffset = heightOffset;
			}
		}

		private static List<PaintingData> _paintings = new List<PaintingData>()
		{
			new PaintingData("Pointer", 4, 1, 4, 1),
			new PaintingData("Pigscene", 4, 1, 4, 1),
			new PaintingData("Flaming Skull", 4, 1, 4, 1),
			new PaintingData("DonkeyKong", 4, 1, 3, 1),
			new PaintingData("Skeleton", 4, 1, 3, 1),
			new PaintingData("Fighters", 4, 1, 2, 0),
			new PaintingData("Match", 2, 0, 2, 0),
			new PaintingData("Bust", 2, 0, 2, 0),
			new PaintingData("Stage", 2, 0, 2, 0),
			new PaintingData("Void", 2, 0, 2, 0),
			new PaintingData("SkullAndRoses", 2, 0, 2, 0),
			new PaintingData("Wither", 2, 0, 2, 0),
			new PaintingData("Wanderer", 1, 0, 2, 0),
			new PaintingData("Graham", 1, 0, 2, 0),
			new PaintingData("Pool", 2, 0, 1, 0),
			new PaintingData("Courbet", 2, 0, 1, 0),
			new PaintingData("Sunset", 2, 0, 1, 0),
			new PaintingData("Sea", 2, 0, 1, 0),
			new PaintingData("Creebet", 2, 0, 1, 0),
			new PaintingData("Kebab", 1, 0, 1, 0),
			new PaintingData("Aztec", 1, 0, 1, 0),
			new PaintingData("Alban", 1, 0, 1, 0),
			new PaintingData("Aztec2", 1, 0, 1, 0),
			new PaintingData("Bomb", 1, 0, 1, 0),
			new PaintingData("Plant", 1, 0, 1, 0),
			new PaintingData("Wasteland", 1, 0, 1, 0),
		};

		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemPainting));

		public ItemPainting() : base("minecraft:painting", 321)
		{
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block block = world.GetBlock(targetCoordinates);
			var emptyCoordinates = block.IsReplaceable ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);
			var coordinates = targetCoordinates;


			if (face == BlockFace.Up || face == BlockFace.Down) return;

			var paintings = FindPaintings(world, emptyCoordinates, face);
			if (paintings.Count == 0) return;

			var paintingTuple = paintings[new Random().Next(paintings.Count)];
			var paintingData = paintingTuple.Item1;

			Painting painting = new Painting(world, paintingData);
			painting.Bbox = paintingTuple.Item2;
			painting.KnownPosition = coordinates;

			int width = paintingData.Width;
			int widthOffset = paintingData.WidthOffset;
			int height = paintingData.Height;
			int heightOffset = paintingData.HeightOffset;

			BoundingBox bbox;
			switch (face)
			{
				case BlockFace.North:
					painting.FacingDirection = 2;
					bbox = new BoundingBox(new Vector3(-(width - 1 - widthOffset), -heightOffset, 0), new Vector3(widthOffset, height - 1 - heightOffset, 0));
					bbox = bbox.OffsetBy(emptyCoordinates);
					PaintBbox(world, bbox);
					bbox = bbox.OffsetBy(BlockCoordinates.South);
					PaintBbox(world, bbox);
					break;
				case BlockFace.East:
					painting.FacingDirection = 3;
					bbox = new BoundingBox(new Vector3(0, -heightOffset, -(width - 1 - widthOffset)), new Vector3(0, height - 1 - heightOffset, widthOffset));
					bbox = bbox.OffsetBy(emptyCoordinates);
					PaintBbox(world, bbox);
					bbox = bbox.OffsetBy(BlockCoordinates.West);
					PaintBbox(world, bbox);
					break;
				case BlockFace.South:
					painting.FacingDirection = 0;
					bbox = new BoundingBox(new Vector3(-widthOffset, -heightOffset, 0), new Vector3(width - 1 - widthOffset, height - 1 - heightOffset, 0));
					bbox = bbox.OffsetBy(emptyCoordinates);
					PaintBbox(world, bbox);
					bbox = bbox.OffsetBy(BlockCoordinates.North);
					PaintBbox(world, bbox);
					break;
				case BlockFace.West:
					painting.FacingDirection = 1;
					bbox = new BoundingBox(new Vector3(0, -heightOffset, -widthOffset), new Vector3(0, height - 1 - heightOffset, width - 1 - widthOffset));
					bbox = bbox.OffsetBy(emptyCoordinates);
					PaintBbox(world, bbox);
					bbox = bbox.OffsetBy(BlockCoordinates.East);
					PaintBbox(world, bbox);
					break; // South 
			}

			painting.SpawnEntity();

			if (player.GameMode == GameMode.Survival)
			{
				var itemInHand = player.Inventory.GetItemInHand();
				itemInHand.Count--;
				player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, itemInHand);
			}
		}

		private List<(PaintingData, BoundingBox)> FindPaintings(Level world, BlockCoordinates emptyCoordinates, BlockFace face)
		{
			var paintings = new List<(PaintingData, BoundingBox)>();

			int currentSize = 0;
			foreach (var painting in _paintings.OrderByDescending(pd => pd.Height * pd.Width))
			{
				int width = painting.Width;
				int widthOffset = painting.WidthOffset;
				int height = painting.Height;
				int heightOffset = painting.HeightOffset;

				if (paintings.Count > 0 && currentSize > width * height) break;

				currentSize = height * width;

				BoundingBox bbox = new BoundingBox();
				switch (face)
				{
					case BlockFace.North:
						bbox = new BoundingBox(new Vector3(-(width - 1 - widthOffset), -heightOffset, 0), new Vector3(widthOffset, height - 1 - heightOffset, 0));
						bbox = bbox.OffsetBy(emptyCoordinates);
						if (!IsSpawnAreaClear(world, bbox)) continue;
						if (!IsSpawnAreaSolid(world, bbox.OffsetBy(BlockCoordinates.South))) continue;
						if (CollidesWithPainting(world, bbox)) continue;
						break;
					case BlockFace.East:
						bbox = new BoundingBox(new Vector3(0, -heightOffset, -(width - 1 - widthOffset)), new Vector3(0, height - 1 - heightOffset, widthOffset));
						bbox = bbox.OffsetBy(emptyCoordinates);
						if (!IsSpawnAreaClear(world, bbox)) continue;
						if (!IsSpawnAreaSolid(world, bbox.OffsetBy(BlockCoordinates.West))) continue;
						if (CollidesWithPainting(world, bbox)) continue;
						break;
					case BlockFace.South:
						bbox = new BoundingBox(new Vector3(-widthOffset, -heightOffset, 0), new Vector3(width - 1 - widthOffset, height - 1 - heightOffset, 0));
						bbox = bbox.OffsetBy(emptyCoordinates);
						if (!IsSpawnAreaClear(world, bbox)) continue;
						if (!IsSpawnAreaSolid(world, bbox.OffsetBy(BlockCoordinates.North))) continue;
						if (CollidesWithPainting(world, bbox)) continue;
						break;
					case BlockFace.West:
						bbox = new BoundingBox(new Vector3(0, -heightOffset, -widthOffset), new Vector3(0, height - 1 - heightOffset, width - 1 - widthOffset));
						bbox = bbox.OffsetBy(emptyCoordinates);
						if (!IsSpawnAreaClear(world, bbox)) continue;
						if (!IsSpawnAreaSolid(world, bbox.OffsetBy(BlockCoordinates.East))) continue;
						if (CollidesWithPainting(world, bbox)) continue;
						break; // South 
				}

				paintings.Add(ValueTuple.Create<PaintingData, BoundingBox>(painting, bbox));
			}

			return paintings;
		}

		private bool CollidesWithPainting(Level world, BoundingBox bbox)
		{
			return world.Entities.Any(pair => pair.Value is Painting && pair.Value.GetBoundingBox().Intersects(bbox));
		}

		private void PaintBbox(Level level, BoundingBox bbox)
		{
			return;

			/*BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x <= max.X; x++)
			{
				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int z = min.Z; z <= max.Z; z++)
					{
						// Check this again. Might be that we want to check solids instead?
						level.SetBlock(new StainedGlass() {Coordinates = new BlockCoordinates(x, y, z)});
					}
				}
			}

			level.SetBlock(new StainedGlass()
			{
				Color = "lime",
				Coordinates = bbox.Max
			});
			level.SetBlock(new StainedGlass()
			{
				Color = "gray",
				Coordinates = bbox.Min
			});*/
		}

		private bool IsSpawnAreaSolid(Level level, BoundingBox bbox)
		{
			return CheckSpawnArea(level, bbox, false);
		}

		private bool IsSpawnAreaClear(Level level, BoundingBox bbox)
		{
			return CheckSpawnArea(level, bbox, true);
		}

		private bool CheckSpawnArea(Level level, BoundingBox bbox, bool checkForAir)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x <= max.X; x++)
			{
				for (int y = min.Y; y <= max.Y; y++)
				{
					for (int z = min.Z; z <= max.Z; z++)
					{
						// Check this again. Might be that we want to check solids instead?
						var isAir = level.IsAir(new BlockCoordinates(x, y, z));
						if (checkForAir & !isAir) return false;
						if (!checkForAir & isAir) return false;
					}
				}
			}

			return true;
		}
	}
}