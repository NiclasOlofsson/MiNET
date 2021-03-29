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
using System.Threading;
using MiNET;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace TestPlugin
{
	[Plugin(PluginName = "PushToTheLimit", Description = "Commands that pushed MCPE to the limit.", PluginVersion = "1.0", Author = "MiNET Team")]
	public class PushToTheLimitCommands : Plugin
	{
		[Command]
		public void SpawnBat(Player player, int numberOfBats = 100, BlockPos spawnPos = null)
		{
			var coordinates = player.KnownPosition;
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			int limit = (int) Math.Sqrt(numberOfBats);
			for (int x = 0; x < limit; x++)
			{
				for (int z = 0; z < limit; z++)
				{
					var bat = new Bat(player.Level);
					bat.NoAi = false;
					bat.KnownPosition = coordinates + new Vector3(x / 2f, 1, z / 2f);
					bat.SpawnEntity();
				}
			}
		}

		[Command]
		public void SpawnDragon(Player player, int numberOfBats = 100, BlockPos spawnPos = null)
		{
			var coordinates = player.KnownPosition;
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			int limit = (int) Math.Sqrt(numberOfBats);
			for (int x = 0; x < limit; x++)
			{
				for (int z = 0; z < limit; z++)
				{
					var dragon = new Dragon(player.Level);
					dragon.NoAi = false;
					dragon.KnownPosition = coordinates + new Vector3(x * 15, 0, z * 15);
					dragon.SpawnEntity();
				}
			}
		}


		[Command]
		public void SpawnGhast(Player player, int numberOfBats = 100, BlockPos spawnPos = null)
		{
			var coordinates = player.KnownPosition;
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			int limit = (int) Math.Sqrt(numberOfBats);
			for (int x = 0; x < limit; x++)
			{
				for (int z = 0; z < limit; z++)
				{
					var entity = new Ghast(player.Level)
					{
						NoAi = false,
						KnownPosition = coordinates + new Vector3(x, 0, z),
						Scale = 0.1
					};
					entity.SpawnEntity();
				}
			}
		}

		[Command]
		public void SpawnWither(Player player, int numberOfBats = 100, BlockPos spawnPos = null)
		{
			var coordinates = player.KnownPosition;
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			//int limit = (int) Math.Sqrt(numberOfBats);
			//for (int x = 0; x < limit; x++)
			//{
			//	for (int z = 0; z < limit; z++)
			//	{
			//		var wither = new Wither(player.Level);
			//		wither.NoAi = false;
			//		wither.KnownPosition = coordinates + new Vector3(x*2, 0, z*2);
			//		wither.SpawnEntity();
			//	}
			//}

			double radius = numberOfBats;
			do
			{
				for (int angle = 0; angle < 360; angle = (int) (angle + (360 / radius)))
				{
					double rad = VectorHelpers.ToRadians(angle);

					double x = radius * Math.Cos(rad);
					double z = radius * Math.Sin(rad);

					var wither = new Wither(player.Level);
					wither.NoAi = false;
					wither.KnownPosition = coordinates + new Vector3((float) x, 0, (float) z);
					wither.KnownPosition.Yaw = angle + 90;
					wither.SpawnEntity();
					Thread.Sleep(50);
				}
				radius -= 4;
			} while (radius > 7);
		}

		[Command]
		public void SpawnPlayers(Player player, int numberOfEntities = 100, BlockPos spawnPos = null)
		{
			var coordinates = player.KnownPosition;
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			int limit = (int) Math.Sqrt(numberOfEntities);
			for (int x = 0; x < limit; x++)
			{
				for (int z = 0; z < limit; z++)
				{
					var entity = new PlayerMob($"TheGrey {z + (x * limit)}", player.Level)
					{
						NoAi = true,
						KnownPosition = coordinates + new Vector3(x, 0, z),
						IsAlwaysShowName = false,
						HideNameTag = false,
					};
					entity.SpawnEntity();
					//Thread.Sleep(50);
				}
			}
		}
	}
}