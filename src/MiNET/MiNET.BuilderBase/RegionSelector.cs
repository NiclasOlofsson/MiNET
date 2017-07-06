using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Blocks;
using MiNET.BuilderBase.Commands;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class RegionSelector
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (RegionSelector));

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
			BoundingBox bbox = new BoundingBox(Position1, Position2);
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
				var editSession = new EditHelper(level, undoRecorder: undoRecorder);

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

		public void DisplaySelection(bool force = false, int particleId = 10)
		{
			if (!force && !ShowSelection) return; // don't render at all

			if (force && ShowSelection) return; // Will be rendered on regular tick instead

			if (!Monitor.TryEnter(_sync)) return;

			try
			{
				BoundingBox box = GetSelection();
				{
					Level level = Player.Level;

					//if ((Math.Abs(box.Width) > 0) || (Math.Abs(box.Height) > 0) || (Math.Abs(box.Depth) > 0))
					{
						var minX = Math.Min(box.Min.X, box.Max.X);
						var maxX = Math.Max(box.Min.X, box.Max.X) + 1;

						var minY = Math.Max(0, Math.Min(box.Min.Y, box.Max.Y));
						var maxY = Math.Min(255, Math.Max(box.Min.Y, box.Max.Y)) + 1;

						var minZ = Math.Min(box.Min.Z, box.Max.Z);
						var maxZ = Math.Max(box.Min.Z, box.Max.Z) + 1;

						// x/y
						for (float x = minX; x <= maxX; x++)
						{
							for (float y = minY; y <= maxY; y++)
							{
								foreach (var z in new float[] {minZ, maxZ})
								{
									if (!level.IsAir(new BlockCoordinates((int) x, (int) y, (int) z))) continue;

									//var particle = new Particle(particleId, Player.Level) {Position = new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f)};
									var particle = new Particle(particleId, Player.Level) {Position = new Vector3(x, y, z)};
									particle.Spawn(new[] {Player});
								}
							}
						}

						// x/z
						//for (float x = minX; x <= maxX; x++)
						//{
						//	foreach (var y in new float[] {minY, maxY})
						//	{
						//		for (float z = minZ; z <= maxZ; z++)
						//		{
						//			if (!level.IsAir(new BlockCoordinates((int) x, (int) y, (int) z))) continue;

						//			//var particle = new Particle(10, Player.Level) {Position = new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f)};
						//			var particle = new Particle(10, Player.Level) {Position = new Vector3(x, y, z)};
						//			particle.Spawn(new[] {Player});
						//		}
						//	}
						//}

						// z/y
						foreach (var x in new float[] {minX, maxX})
						{
							for (float y = minY; y <= maxY; y++)
							{
								for (float z = minZ; z <= maxZ; z++)
								{
									if (!level.IsAir(new BlockCoordinates((int) x, (int) y, (int) z))) continue;

									//var particle = new Particle(10, Player.Level) {Position = new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f)};
									var particle = new Particle(10, Player.Level) {Position = new Vector3(x, y, z)};
									particle.Spawn(new[] {Player});
								}
							}
						}
					}
				}
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
			return (max + min)/2;
		}
	}
}