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
using System.IO;
using System.Numerics;
using System.Reflection;
using log4net;
using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;

namespace TestPlugin.Code4Fun
{
	[Plugin(PluginName = "Code4Fun", Description = "Plugin with mostly fun stuff", PluginVersion = "1.0", Author = "MiNET Team")]
	public class Code4FunPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Code4FunPlugin));

		public const double CubeFilterFactor = 1.3;
		public const float ZTearFactor = 0.01f;
		public static int FakeIndex = 0;

		protected override void OnEnable()
		{
			Context.PluginManager.LoadCommands(new ScreenshotCommand());
			Context.PluginManager.LoadCommands(new VideoCommand());
		}

		[Command]
		public void Melt(Player player)
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			var skin = player.Skin;
			if (string.IsNullOrEmpty(skin.GeometryData))
			{
				string skinString = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));
				skin.GeometryData = skinString;
			}
			else
			{
				string fileName = $"{Path.GetTempPath()}Skin_{player.Username}_{skin.GeometryName}.txt";
				Log.Info($"Writing geometry to filename: {fileName}");
				File.WriteAllText(fileName, skin.GeometryData);
			}

			//GravityGeometryBehavior state = new GravityGeometryBehavior
			//{
			//	Uuid = player.ClientUuid,
			//	Level = player.Level,
			//	Skin = player.Skin,
			//	CurrentModel = Skin.Parse(skin.SkinGeometry),
			//	Position = player.KnownPosition,
			//	ResetOnEnd = true
			//};

			//var geometryTimer = new Timer(MeltTick, state, 0, 50);
			//state.Timer = geometryTimer;
		}

		[Command]
		public void SpawnFake(Player player, string name)
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			//var bytes = Encoding.Default.GetBytes(new string('Z', 8192));
			//byte[] skinData = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "test_skin.png"));
			byte[] skinData = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "IMG_0220.png"));
			string skinString = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));

			var random = new Random();
			string newName = $"geometry.{DateTime.UtcNow.Ticks}.{random.NextDouble()}";
			skinString = skinString.Replace("geometry.humanoid.custom", newName);
			GeometryModel geometryModel = Skin.Parse(skinString);

			PlayerLocation coordinates = player.KnownPosition;
			Vector3 direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection()) * 1.5f;

			var fake = new PlayerMob(string.Empty, player.Level)
			{
				KnownPosition = new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, 0, 0)
				//KnownPosition = new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, coordinates.HeadYaw + 180f, coordinates.Yaw + 180f)
			};

			fake.Skin.Data = skinData;
			fake.Skin.SkinResourcePatch = new SkinResourcePatch() {Geometry = new GeometryIdentifier() {Default = newName}};
			fake.Skin.GeometryName = newName;
			//fake.Skin.GeometryData = skinString;
			fake.Skin.GeometryData = Skin.ToJson(geometryModel);
			;

			fake.SpawnEntity();

			fake.SetPosition(new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, 0, 0), true);

			//var state = new GravityGeometryBehavior(fake, geometryModel);
			//fake.Ticking += state.FakeMeltTicking;
		}

		public class GravityGeometryBehavior
		{
			private static readonly ILog Log = LogManager.GetLogger(typeof(GravityGeometryBehavior));

			public const float Gravity = 0.20f;
			public const float Drag = 0.02f;

			public PlayerMob Mob { get; }
			public GeometryModel CurrentModel { get; private set; }
			public bool ResetOnEnd { get; set; }

			public GravityGeometryBehavior(PlayerMob mob, GeometryModel currentModel)
			{
				Mob = mob;
				CurrentModel = currentModel;
				var geometry = CurrentModel.CollapseToDerived(CurrentModel.FindGeometry(mob.Skin.GeometryName));
				geometry.Subdivide(true, false);

				SetVelocity(geometry, new Random());

				CurrentModel.Geometry.Clear();
				CurrentModel.Geometry.Add(geometry);
			}

			private void SetVelocity(GeometryModel model, Random random)
			{
				foreach (var geometry in model.Geometry)
				{
					SetVelocity(geometry, random);
				}
			}


			private void SetVelocity(Geometry geometry, Random random1)
			{
				Random random = new Random();

				foreach (var bone in geometry.Bones)
				{
					SetVelocity(bone, random);
				}
			}

			private void SetVelocity(Bone bone, Random random)
			{
				if (bone.NeverRender) return;
				if (bone.Cubes == null || bone.Cubes.Count == 0) return;

				foreach (var cube in bone.Cubes)
				{
					SetVelocity(cube, random);
				}
			}

			private Vector3 _origin = new Vector3(0, 4, 10);

			private void SetVelocity(Cube cube, Random random)
			{
				//Quaternion rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitY, (float) -Math.PI/2);

				var pos = new Vector3(cube.Origin[0] / 16f, cube.Origin[1] / 16f, cube.Origin[2] / 16f) + Mob.KnownPosition;
				//var dir = _origin - pos;
				var dir = pos - _origin;
				float distance = dir.Length();
				//Log.Debug("Position: " + pos);

				distance = Math.Max(1, distance);
				distance = distance / (distance * distance);
				if (distance < 0.1) return;

				//Log.Debug("Lenght: " + distance);
				Vector3 force = new Vector3(distance, distance, distance) * 5;
				cube.Velocity = Vector3.Reflect(dir.Normalize() * force, Vector3.UnitZ);

				//+ new Vector3((float) random.NextDouble() - 0.5f, (float) random.NextDouble() - 0.5f, (float) random.NextDouble() - 0.5f)*10/distance;
				//Log.Debug("Velocity: " + cube.Velocity);
				//cube.Velocity = dir.Normalize() + new Vector3((float) random.NextDouble(), (float) random.NextDouble(), (float) random.NextDouble()) * distance;
				//cube.Velocity = new Vector3((float) ((random.NextDouble() - 0.5f)*1.8f), (float) (random.NextDouble()*cube.Origin[1]/10 + 3.8f), (float) ((random.NextDouble() - 0.5f)*1.8f));
			}

			public void FakeMeltTicking(object sender, PlayerEventArgs playerEventArgs)
			{
				var mob = (PlayerMob) sender;

				//Log.Warn("Done. De-register tick.");
				//mob.Ticking -= FakeMeltTicking;
				//return;

				if (CurrentModel == null)
				{
					Log.Warn($"No current model set for mob.");
					return;
				}

				try
				{
					bool stillMoving = false;
					foreach (Geometry geometry in CurrentModel.Geometry)
					{
						foreach (Bone bone in geometry.Bones)
						{
							if (bone.NeverRender) continue;
							if (bone.Cubes == null || bone.Cubes.Count == 0) continue;

							foreach (var cube in bone.Cubes)
							{
								if (cube.Origin[1] <= 0.05f && cube.Velocity.Y <= 0.01)
								{
									cube.Origin[1] = 0f;
									cube.Velocity = Vector3.Zero;
									continue;
								}

								stillMoving = true;

								float x = cube.Origin[0];
								float y = cube.Origin[1];
								float z = cube.Origin[2];

								cube.Origin = new[] {x + cube.Velocity.X, Math.Max(0f, y + cube.Velocity.Y), z + cube.Velocity.Z};
								cube.Velocity -= new Vector3(0, Gravity, 0);
								cube.Velocity *= 1 - Drag;
							}
						}
					}

					if (!stillMoving)
					{
						Log.Warn("Done. De-register tick.");
						mob.Ticking -= FakeMeltTicking;

						// Reset?
						if (ResetOnEnd)
						{
							Skin skin = mob.Skin;

							var updateSkin = McpePlayerSkin.CreateObject();
							updateSkin.NoBatch = true;
							updateSkin.uuid = mob.ClientUuid;
							updateSkin.oldSkinName = mob.Skin.SkinId;
							updateSkin.skinName = mob.Skin.SkinId;
							updateSkin.skin = skin;
							mob.Level.RelayBroadcast(updateSkin);
						}
					}
					else
					{
						Skin skin = mob.Skin;
						Geometry geometry = CurrentModel.FindGeometry(skin.GeometryName);
						geometry.Description.Identifier = $"geometry.{DateTime.UtcNow.Ticks}.{mob.ClientUuid}";
						mob.Skin.SkinResourcePatch = new SkinResourcePatch() {Geometry = new GeometryIdentifier() {Default = geometry.Description.Identifier}};

						CurrentModel.Geometry.Clear();
						CurrentModel.Geometry.Add(geometry);

						skin.GeometryName = geometry.Description.Identifier;
						skin.GeometryData = Skin.ToJson(CurrentModel);

						var updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = mob.ClientUuid;
						updateSkin.oldSkinName = mob.Skin.SkinId;
						updateSkin.skinName = mob.Skin.SkinId;
						updateSkin.skin = skin;
						mob.Level.RelayBroadcast(updateSkin);
					}
				}
				catch (Exception e)
				{
					mob.Ticking -= FakeMeltTicking;
					Log.Error(e);
				}
			}
		}

		//private void StrikeTick(object state)
		//{
		//	if (!Monitor.TryEnter(state)) return;

		//	try
		//	{
		//		GravityGeometryBehavior signal = state as GravityGeometryBehavior;

		//		if (signal == null) return;
		//		if (signal.Timer == null) return;
		//		if (signal.CurrentModel == null) return;

		//		if (signal.Tick++ >= signal.MaxDuration)
		//		{
		//			Log.Warn($"Reached end of animation: {signal.Tick}");
		//			signal.Tick = 0;
		//			signal.Timer.Dispose();
		//			signal.Timer = null;

		//			// Reset?
		//			if (signal.ResetOnEnd)
		//			{
		//				Skin skin = signal.Skin;

		//				McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
		//				updateSkin.NoBatch = true;
		//				updateSkin.uuid = signal.Uuid;
		//				updateSkin.skinId = skin.SkinId;
		//				updateSkin.skinData = skin.SkinData;
		//				updateSkin.capeData = skin.CapeData;
		//				updateSkin.geometryModel = skin.SkinGeometryName;
		//				updateSkin.geometryData = skin.SkinGeometry;
		//				signal.Level.RelayBroadcast(updateSkin);
		//			}

		//			return;
		//		}

		//		try
		//		{
		//			if (signal.Tick == 1)
		//			{
		//				var geometry = signal.CurrentModel.CollapseToDerived(signal.CurrentModel.FindGeometry(signal.Skin.SkinGeometryName));
		//				geometry.Subdivide(false, true, false, true);
		//				signal.CurrentModel.Clear();
		//				signal.CurrentModel.Add(geometry.Name, geometry);
		//			}

		//			int[] flashes = {50, 60, 100, 120, 135, 155, 170, 185, 209, 250, 300, 330, 380, 440};

		//			if (flashes.Contains((int) signal.Tick + 1))
		//			{
		//				signal.Level.StrikeLightning(signal.Position);
		//			}


		//			if (flashes.Contains((int) signal.Tick) || flashes.Contains((int) signal.Tick - 4))
		//			{
		//				string fullName = signal.CurrentModel.Keys.First(m => m.StartsWith(signal.Skin.SkinGeometryName));
		//				signal.CurrentModel[fullName].AnimationArmsOutFront = true;
		//				string skinString = Skin.ToJson(signal.CurrentModel);

		//				string newName = $"geometry.{DateTime.UtcNow.Ticks}.{signal.Uuid}";
		//				skinString = skinString.Replace(fullName, newName);

		//				Skin skin = signal.Skin;

		//				McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
		//				updateSkin.NoBatch = true;
		//				updateSkin.uuid = signal.Uuid;
		//				updateSkin.skinId = skin.SkinId;
		//				updateSkin.skinData = skin.SkinData;
		//				updateSkin.capeData = skin.CapeData;
		//				updateSkin.geometryModel = newName;
		//				updateSkin.geometryData = skinString;
		//				signal.Level.RelayBroadcast(updateSkin);
		//			}

		//			if (flashes.Contains((int) signal.Tick - 2) || flashes.Contains((int) signal.Tick - 6))
		//			{
		//				Skin skin = signal.Skin;

		//				McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
		//				updateSkin.NoBatch = true;
		//				updateSkin.uuid = signal.Uuid;
		//				updateSkin.skinId = skin.SkinId;
		//				updateSkin.skinData = skin.SkinData;
		//				updateSkin.capeData = skin.CapeData;
		//				updateSkin.geometryModel = skin.SkinGeometryName;
		//				updateSkin.geometryData = skin.SkinGeometry;
		//				signal.Level.RelayBroadcast(updateSkin);
		//			}
		//		}
		//		catch (Exception e)
		//		{
		//			Log.Error(e);
		//		}
		//	}
		//	finally
		//	{
		//		Monitor.Exit(state);
		//	}
		//}
	}
}