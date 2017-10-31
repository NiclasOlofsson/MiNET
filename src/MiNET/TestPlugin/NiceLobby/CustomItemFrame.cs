using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.ImageProviders;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.NiceLobby
{
	public class CustomItemItemFrame : ItemItemFrame
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CustomItemItemFrame));

		private readonly List<MapEntity> _frames;
		private readonly FrameTicker _frameTicker;

		public CustomItemItemFrame(List<MapEntity> frames, FrameTicker frameTicker)
		{
			_frames = frames;
			_frameTicker = frameTicker;
		}

		public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Log.Warn("Using custom item frame");

			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);

			ItemFrameBlockEntity itemFrameBlockEntity = new ItemFrameBlockEntity
			{
				Coordinates = coor
			};

			CustomItemFrame itemFrame = new CustomItemFrame(_frames, itemFrameBlockEntity, world, _frameTicker)
			{
				Coordinates = coor,
			};

			if (!itemFrame.CanPlace(world, blockCoordinates, face)) return;

			itemFrame.PlaceBlock(world, player, coor, face, faceCoords);

			// Then we create and set the sign block entity that has all the intersting data

			world.SetBlockEntity(itemFrameBlockEntity);
		}
	}

	public class CustomItemFrame : ItemFrame
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CustomItemFrame));

		private readonly List<MapEntity> _frames;
		private readonly ItemFrameBlockEntity _itemFrameBlockEntity;
		private readonly Level _level;
		private readonly FrameTicker _frameTicker;

		private Timer _timer;

		public CustomItemFrame(List<MapEntity> frames, ItemFrameBlockEntity itemFrameBlockEntity, Level level, FrameTicker frameTicker) : base()
		{
			Log.Error("Created new Custom Item Frame");
			_frames = frames;
			_itemFrameBlockEntity = itemFrameBlockEntity;
			_level = level;
			_frameTicker = frameTicker;
			_timer = new Timer(Tick, null, 33, 33);
			_frameTicker.Register(this);
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			byte direction = player.GetDirection();

			switch (face)
			{
				case BlockFace.South: // ok
					Metadata = 0;
					break;
				case BlockFace.North:
					Metadata = 1;
					break;
				case BlockFace.West:
					Metadata = 2;
					break;
				case BlockFace.East: // ok
					Metadata = 3;
					break;
			}

			Log.Warn($"Direction={direction}, face={face}, metadata={Metadata}");

			world.SetBlock(this);

			return true;
		}

		private int _frame = 0;
		
		private object _tickSync = new object();

		private void Tick(object state)
		{
			if (!Monitor.TryEnter(_tickSync)) return;

			try
			{
				if (_frames.Count == 0) return;

				var currentFrame = _frameTicker.GetCurrentFrame(this);
				if (currentFrame >= _frames.Count) return;

				ItemMap map = new ItemMap(_frames[currentFrame].EntityId);

				ItemFrameBlockEntity blockEntity = _itemFrameBlockEntity;
				if (blockEntity != null)
				{
					blockEntity.SetItem(map);
					_level.SetBlockEntity(blockEntity);
				}
			}
			finally
			{
				Monitor.Exit(_tickSync);	
			}
		}
	}
}