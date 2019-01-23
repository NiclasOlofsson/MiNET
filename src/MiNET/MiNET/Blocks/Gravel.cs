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

using System;
using System.Numerics;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Gravel : Block
	{
		private int _tickRate = 1;

		public Gravel() : base(13)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			world.ScheduleBlockTick(this, _tickRate);
			return false;
		}

		public override void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
		{
			world.ScheduleBlockTick(this, _tickRate);
		}

		public override void DoPhysics(Level level)
		{
			level.ScheduleBlockTick(this, _tickRate);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) return;

			if (!level.GetBlock(Coordinates + Level.Down).IsSolid)
			{
				level.SetAir(Coordinates);

				var bbox = GetBoundingBox();
				var d = (bbox.Max - bbox.Min) / 2;

				new FallingBlock(level, this)
				{
					KnownPosition = new PlayerLocation(Coordinates.X + d.X, Coordinates.Y - 0.01f, Coordinates.Z + d.Z)
				}.SpawnEntity();
			}
		}


		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() <= 0.1)
			{
				return new[] {ItemFactory.GetItem(318)};
			}

			return base.GetDrops(tool);
		}
	}
}