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

using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Projectiles;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Test
{
	/// <summary>
	///     One projectile tick as BDS 1.26.70 runs it (ProjectileSystem and
	///     ProjectileComponent::handleMovementAndGravity): hits are tested on the stretch this tick's
	///     motion covers and the nearest one wins, a block hit leaves the projectile on the hit point,
	///     then move, turn a fifth of the way toward the motion, drag (water only), gravity.
	/// </summary>
	[TestClass]
	public class ProjectileFlightTests
	{
		private static Level NewLevel()
		{
			var provider = new AnvilWorldProvider {MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld)};
			var level = new Level(null, "flight", provider, new EntityManager(), GameMode.Creative, viewDistance: 1) {EnableLevelTicking = false};
			level.Initialize();
			return level;
		}

		private static Arrow Fire(Level level, Vector3 from, Vector3 velocity)
		{
			var arrow = new Arrow(null, level);
			arrow.KnownPosition = new PlayerLocation(from.X, from.Y, from.Z);
			arrow.Velocity = velocity;
			return arrow;
		}

		// An arrow that reaches a block this tick stops on the face it hit, not short of it and not past it.
		[TestMethod]
		public void BlockHit_RestsExactlyOnTheFace()
		{
			Level level = NewLevel();
			level.SetBlock(new Stone {Coordinates = new BlockCoordinates(2, 100, 0)});
			Arrow arrow = Fire(level, new Vector3(0.5f, 100.5f, 0.5f), new Vector3(3, 0, 0));

			arrow.OnTick(new Entity[0]);

			Assert.AreEqual(2f, arrow.KnownPosition.X, 1e-5);
			Assert.AreEqual(100.5f, arrow.KnownPosition.Y, 1e-5);
			Assert.AreEqual(Vector3.Zero, arrow.Velocity);
		}

		// The hit test follows the path the arrow moves this tick. Grazing over a block's top is a miss,
		// even though next tick's gravity would have dipped it into the block.
		[TestMethod]
		public void HitTest_UsesThisTicksMotion()
		{
			Level level = NewLevel();
			level.SetBlock(new Stone {Coordinates = new BlockCoordinates(2, 100, 0)});
			Arrow arrow = Fire(level, new Vector3(0.5f, 101.01f, 0.5f), new Vector3(3, 0, 0));

			arrow.OnTick(new Entity[0]);

			Assert.AreEqual(3.5f, arrow.KnownPosition.X, 1e-5);
			Assert.AreNotEqual(Vector3.Zero, arrow.Velocity);
		}

		// A wall between the arrow and a mob takes the arrow; the nearest hit wins.
		[TestMethod]
		public void WallBeforeMob_TakesTheArrow()
		{
			Level level = NewLevel();
			level.SetBlock(new Stone {Coordinates = new BlockCoordinates(2, 100, 0)});
			var zombie = new Zombie(level) {KnownPosition = new PlayerLocation(3.5f, 100, 0.5f)};
			level.AddEntity(zombie);
			Arrow arrow = Fire(level, new Vector3(0.5f, 100.5f, 0.5f), new Vector3(3, 0, 0));

			arrow.OnTick(new Entity[0]);

			Assert.AreEqual(2f, arrow.KnownPosition.X, 1e-5);
			Assert.AreEqual(Vector3.Zero, arrow.Velocity);
		}

		// In the air: moved by the full motion, drag of 0.99 on every axis, then gravity on the dragged speed.
		[TestMethod]
		public void OpenAir_MovesThenDragsThenFalls()
		{
			Level level = NewLevel();
			Arrow arrow = Fire(level, new Vector3(0.5f, 100.5f, 0.5f), new Vector3(1, 0.5f, 0));

			arrow.OnTick(new Entity[0]);

			Assert.AreEqual(1.5f, arrow.KnownPosition.X, 1e-5);
			Assert.AreEqual(101f, arrow.KnownPosition.Y, 1e-5);
			Assert.AreEqual(0.99f, arrow.Velocity.X, 1e-5);
			Assert.AreEqual(0.5f * 0.99f - 0.05f, arrow.Velocity.Y, 1e-5);
		}

		// The facing turns a fifth of the way per tick toward the flight: from yaw 0 to a flight at yaw 90 is 18.
		[TestMethod]
		public void Rotation_TurnsAFifthOfTheWayPerTick()
		{
			Level level = NewLevel();
			Arrow arrow = Fire(level, new Vector3(0.5f, 100.5f, 0.5f), new Vector3(1, 0, 0));

			arrow.OnTick(new Entity[0]);

			Assert.AreEqual(18f, arrow.KnownPosition.Yaw, 1e-3);
		}

		// Water slows a projectile to 0.6 a tick; lava is not water and drags like air.
		[TestMethod]
		public void LiquidDrag_IsWaterOnly()
		{
			Level level = NewLevel();
			level.SetBlock(new Water {Coordinates = new BlockCoordinates(0, 100, 0)});
			level.SetBlock(new Lava {Coordinates = new BlockCoordinates(0, 100, 4)});
			Arrow inWater = Fire(level, new Vector3(0.5f, 100.5f, 0.5f), new Vector3(0.25f, 0, 0));
			Arrow inLava = Fire(level, new Vector3(0.5f, 100.5f, 4.5f), new Vector3(0.25f, 0, 0));

			inWater.OnTick(new Entity[0]);
			inLava.OnTick(new Entity[0]);

			Assert.AreEqual(0.25f * 0.6f, inWater.Velocity.X, 1e-5);
			Assert.AreEqual(0.25f * 0.99f, inLava.Velocity.X, 1e-5);
		}
	}
}