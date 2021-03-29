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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities.ImageProviders;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace TestPlugin.NiceLobby
{
	public class CustomItemFrame : ItemFrame
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CustomItemFrame));

		private readonly List<MapEntity> _frames;
		private readonly FrameTicker _frameTicker;

		public CustomItemFrame(List<MapEntity> frames, FrameTicker frameTicker)
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

			CustomFrame frame = new CustomFrame(_frames, itemFrameBlockEntity, world, _frameTicker)
			{
				Coordinates = coor,
			};

			if (!frame.CanPlace(world, player, blockCoordinates, face)) return;

			frame.PlaceBlock(world, player, coor, face, faceCoords);

			// Then we create and set the sign block entity that has all the intersting data

			world.SetBlockEntity(itemFrameBlockEntity);
		}
	}

	public class CustomFrame : Frame
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CustomFrame));

		private readonly List<MapEntity> _frames;
		private readonly ItemFrameBlockEntity _itemFrameBlockEntity;
		private readonly Level _level;
		private readonly FrameTicker _frameTicker;

		private Timer _timer;

		public CustomFrame(List<MapEntity> frames, ItemFrameBlockEntity itemFrameBlockEntity, Level level, FrameTicker frameTicker) : base()
		{
			Log.Debug("Created new Custom Item Frame");
			_frames = frames;
			_itemFrameBlockEntity = itemFrameBlockEntity;
			_level = level;
			_frameTicker = frameTicker;
			_timer = new Timer(Tick, null, 33, 33);
			_frameTicker.Register(this);
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			FacingDirection = (int) face;

			return false;
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

				var map = new ItemMap(_frames[currentFrame].EntityId);

				ItemFrameBlockEntity blockEntity = _itemFrameBlockEntity;
				if (blockEntity != null)
				{
					blockEntity.SetItem(map, 0);
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