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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.BuilderBase.Commands;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;

namespace MiNET.BuilderBase
{
	public class RegionSelector
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RegionSelector));

		public static ConcurrentDictionary<Player, RegionSelector> RegionSelectors = new ConcurrentDictionary<Player, RegionSelector>();

		public static RegionSelector GetSelector(Player player)
		{
			RegionSelector selector;
			RegionSelectors.TryGetValue(player, out selector);
			return selector;
		}

		public Player Player { get; set; }
		public bool ShowSelection { get; set; } = true;
		public Dictionary<long, HistoryEntry> History { get; private set; } = new Dictionary<long, HistoryEntry>();
		public Dictionary<long, HistoryEntry> RedoBuffer { get; private set; } = new Dictionary<long, HistoryEntry>();


		public BlockCoordinates Position1 { get; private set; }
		public BlockCoordinates Position2 { get; private set; }
		public Clipboard Clipboard { get; set; }

		public RegionSelector(Player player)
		{
			Player = player;
		}

		public BoundingBox GetSelection()
		{
			var bbox = new BoundingBox(Position1, Position2);
			return bbox;
		}

		public BlockCoordinates[] GetSelectedBlocks()
		{
			BoundingBox box = new BoundingBox(Position1, Position2);

			var minX = Math.Min(box.Min.X, box.Max.X);
			var maxX = Math.Max(box.Min.X, box.Max.X);

			var minY = Math.Min(box.Min.Y, box.Max.Y);
			var maxY = Math.Max(box.Min.Y, box.Max.Y);

			var minZ = Math.Min(box.Min.Z, box.Max.Z);
			var maxZ = Math.Max(box.Min.Z, box.Max.Z);

			List<BlockCoordinates> coords = new List<BlockCoordinates>();

			// x/y
			for (float x = minX; x <= maxX; x++)
			{
				for (float y = minY; y <= maxY; y++)
				{
					for (float z = minZ; z <= maxZ; z++)
					{
						coords.Add(new Vector3(x, y, z));
					}
				}
			}

			return coords.ToArray();
		}

		public void Select(BlockCoordinates primary, BlockCoordinates secondary)
		{
			Position1 = primary;
			Position2 = secondary;

			DisplaySelection(true);
		}

		public void SelectPrimary(BlockCoordinates pos)
		{
			Position1 = pos;
			DisplaySelection(true);
		}

		public void SelectSecondary(BlockCoordinates pos)
		{
			Position2 = pos;

			DisplaySelection(true);
		}

		public void AddHistory(HistoryEntry history, bool keepRedoBuffer = false)
		{
			long time = DateTime.UtcNow.Ticks;

			History.Add(time, history);
			if (!keepRedoBuffer) RedoBuffer.Clear();
		}

		public void Undo(int numberOfUndo = 1)
		{
			for (int i = 0; i < numberOfUndo; i++)
			{
				if (History.Count == 0) return;

				var last = History.OrderBy(kvp => kvp.Key).Last();
				History.Remove(last.Key);
				RedoBuffer.Add(last.Key, last.Value);

				// Undo
				HistoryEntry historyEntry = last.Value;
				var restore = historyEntry.Presnapshot;
				foreach (Block block in restore)
				{
					historyEntry.Level.SetBlock(block);
				}
			}
		}

		public void Redo(int numberOfRedo = 1)
		{
			for (int i = 0; i < numberOfRedo; i++)
			{
				if (RedoBuffer.Count == 0) return;

				var last = RedoBuffer.OrderByDescending(kvp => kvp.Key).Last();
				RedoBuffer.Remove(last.Key);

				var level = last.Value.Level;
				var undoRecorder = new UndoRecorder(level, false);
				var editSession = new EditHelper(level, Player, undoRecorder: undoRecorder);

				// Redo
				var restore = last.Value.Postsnapshot;
				foreach (Block block in restore)
				{
					editSession.SetBlock(block);
				}

				var history = undoRecorder.CreateHistory();
				AddHistory(history, true);
			}
		}

		public void ClearHistory()
		{
			History.Clear();
			RedoBuffer.Clear();
		}

		private object _sync = new object();

		private BoundingBox _currentDisplayedSelection = new BoundingBox();
		private StructureBlock _structureBlock;
		private StructureBlockBlockEntity _structureBlockBlockEntity;

		public void DisplaySelection(bool forceDisplay = false, bool forceHide = false)
		{
			if (!forceDisplay && _structureBlockBlockEntity != null && (forceHide || _structureBlockBlockEntity.ShowBoundingBox != ShowSelection))
			{
				bool showBoundingBox = !forceHide && ShowSelection;
				if (_structureBlockBlockEntity.ShowBoundingBox == showBoundingBox) return;

				_structureBlockBlockEntity.ShowBoundingBox = showBoundingBox;

				var nbt = new Nbt
				{
					NbtFile = new NbtFile
					{
						BigEndian = false,
						UseVarInt = true,
						RootTag = _structureBlockBlockEntity.GetCompound()
					}
				};

				var entityData = McpeBlockEntityData.CreateObject();
				entityData.namedtag = nbt;
				entityData.coordinates = _structureBlockBlockEntity.Coordinates;
				Player.SendPacket(entityData);
			}

			if (forceHide) return;

			if (!forceDisplay && !ShowSelection) return; // don't render at all

			if (forceDisplay && ShowSelection) return; // Will be rendered on regular tick instead

			if (!Monitor.TryEnter(_sync)) return;

			try
			{
				BoundingBox box = GetSelection().GetAdjustedBoundingBox();
				if (!forceDisplay && box == _currentDisplayedSelection) return;
				_currentDisplayedSelection = box;

				int minX = (int) Math.Min(box.Min.X, box.Max.X);
				int maxX = (int) (Math.Max(box.Min.X, box.Max.X) + 1);

				int minY = (int) Math.Max(0, Math.Min(box.Min.Y, box.Max.Y));
				int maxY = (int) (Math.Min(255, Math.Max(box.Min.Y, box.Max.Y)) + 1);

				int minZ = (int) Math.Min(box.Min.Z, box.Max.Z);
				int maxZ = (int) (Math.Max(box.Min.Z, box.Max.Z) + 1);

				int width = maxX - minX;
				int height = maxY - minY;
				int depth = maxZ - minZ;

				if (_structureBlock != null)
				{
					{
						var block = Player.Level.GetBlock(_structureBlock.Coordinates);
						var updateBlock = McpeUpdateBlock.CreateObject();
						updateBlock.blockRuntimeId = (uint) block.GetRuntimeId();
						updateBlock.coordinates = _structureBlock.Coordinates;
						updateBlock.blockPriority = 0xb;
						Player.SendPacket(updateBlock);
					}

					_structureBlock = null;
					_structureBlockBlockEntity = null;
				}

				_structureBlock = new StructureBlock
				{
					StructureBlockType = "save",
					Coordinates = new BlockCoordinates(minX, 255, minZ),
				};

				{
					var updateBlock = McpeUpdateBlock.CreateObject();
					updateBlock.blockRuntimeId = (uint) _structureBlock.GetRuntimeId();
					updateBlock.coordinates = _structureBlock.Coordinates;
					updateBlock.blockPriority = 0xb;
					Player.SendPacket(updateBlock);
				}

				_structureBlockBlockEntity = new StructureBlockBlockEntity
				{
					ShowBoundingBox = true,
					Coordinates = _structureBlock.Coordinates,
					Offset = new BlockCoordinates(0, minY - _structureBlock.Coordinates.Y, 0),
					Size = new BlockCoordinates(width, height, depth)
				};

				{
					Log.Debug($"Structure:\n{box}\n{_structureBlockBlockEntity.GetCompound()}");
					var nbt = new Nbt
					{
						NbtFile = new NbtFile
						{
							BigEndian = false,
							UseVarInt = true,
							RootTag = _structureBlockBlockEntity.GetCompound()
						}
					};

					var entityData = McpeBlockEntityData.CreateObject();
					entityData.namedtag = nbt;
					entityData.coordinates = _structureBlockBlockEntity.Coordinates;
					Player.SendPacket(entityData);
				}

				return;
			}
			catch (Exception e)
			{
				Log.Error("Display selection", e);
			}
			finally
			{
				Monitor.Exit(_sync);
			}
		}

		public BlockCoordinates GetMin()
		{
			return new BlockCoordinates(Math.Min(Position1.X, Position2.X), Math.Min(Position1.Y, Position2.Y), Math.Min(Position1.Z, Position2.Z));
		}

		public BlockCoordinates GetMax()
		{
			return new BlockCoordinates(Math.Max(Position1.X, Position2.X), Math.Max(Position1.Y, Position2.Y), Math.Max(Position1.Z, Position2.Z));
		}

		public Vector3 GetCenter()
		{
			Vector3 max = GetMax();
			Vector3 min = GetMin();
			return min + ((max - min) / 2f);
		}
	}
}