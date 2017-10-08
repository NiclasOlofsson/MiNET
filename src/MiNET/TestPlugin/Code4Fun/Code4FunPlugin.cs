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
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.Code4Fun
{
	[Plugin(PluginName = "Code4Fun", Description = "Plugin with mostly fun stuff", PluginVersion = "1.0", Author = "MiNET Team")]
	public class Code4FunPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Code4FunPlugin));

		public const float Gravity = 0.01f;
		public const float Drag = 0.02f;
		public const double CubeFilterFactor = 1.3;
		public static int FakeIndex = 0;

		[Command]
		public void Melt(Player player)
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			var skin = player.Skin;
			if (skin.SkinGeometry == null || skin.SkinGeometry.Length == 0)
			{
				string skinString = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));
				skin.SkinGeometry = Encoding.UTF8.GetBytes(skinString);
			}
			else
			{
				string fileName = $"{Path.GetTempPath()}Skin_{player.Username}_{skin.SkinGeometryName}.txt";
				Log.Info($"Writing geometry to filename: {fileName}");
				File.WriteAllBytes(fileName, skin.SkinGeometry);
			}

			StateObject state = new StateObject
			{
				Uuid = player.ClientUuid,
				Level = player.Level,
				Skin = player.Skin,
				CurrentModel = Skin.Parse(Encoding.UTF8.GetString(skin.SkinGeometry)),
				Position = player.KnownPosition,
				ResetOnEnd = true
			};

			var geometryTimer = new Timer(MeltTick, state, 0, 50);
			state.Timer = geometryTimer;
		}

		[Command]
		public void SpawnFake(Player player, string name)
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			//var bytes = Encoding.Default.GetBytes(new string('Z', 8192));
			byte[] bytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "test_skin.png"));
			//byte[] bytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "IMG_0220.png"));
			string skinString = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));

			var random = new Random();
			string newName = $"geometry.{DateTime.UtcNow.Ticks}.{random.NextDouble()}";
			skinString = skinString.Replace("geometry.humanoid", newName);
			byte[] skinGeometry = Encoding.UTF8.GetBytes(skinString);
			GeometryModel geometryModel = Skin.Parse(skinString);

			var coordinates = player.KnownPosition;
			var direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection())*1.5f;

			PlayerMob fake = new PlayerMob(string.Empty, player.Level)
			{
				Skin = new Skin
				{
					SkinId = "testing",
					Slim = false,
					SkinData = bytes,
					CapeData = new byte[0],
					SkinGeometryName = newName,
					SkinGeometry = skinGeometry
				},
				KnownPosition = new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, coordinates.HeadYaw + 180f, coordinates.Yaw + 180f),
			};

			fake.SpawnEntity();

			StateObject state = new StateObject
			{
				Uuid = fake.Uuid,
				Level = fake.Level,
				Skin = fake.Skin,
				CurrentModel = geometryModel,
				//StartDelay = 10,
				//MaxDuration = 1000,
				Position = fake.KnownPosition
			};

			var geometryTimer = new Timer(MeltTick, state, 0, 100);
			//var geometryTimer = new Timer(StrikeTick, state, 0, 100);
			state.Timer = geometryTimer;
		}

		private class StateObject
		{
			public Timer Timer { get; set; }
			public UUID Uuid { get; set; }
			public Level Level { get; set; }
			public Skin Skin { get; set; }
			public GeometryModel CurrentModel { get; set; }
			public long MaxDuration { get; set; } = 140;
			public long Tick { get; set; }
			public long StartDelay { get; set; } = 40;
			public PlayerLocation Position { get; set; }
			public bool ResetOnEnd { get; set; }
		}

		private void StrikeTick(object state)
		{
			if (!Monitor.TryEnter(state)) return;

			try
			{
				StateObject signal = state as StateObject;

				if (signal == null) return;
				if (signal.Timer == null) return;
				if (signal.CurrentModel == null) return;

				if (signal.Tick++ >= signal.MaxDuration)
				{
					Log.Warn($"Reached end of animation: {signal.Tick}");
					signal.Tick = 0;
					signal.Timer.Dispose();
					signal.Timer = null;

					// Reset?
					if (signal.ResetOnEnd)
					{
						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = skin.SkinGeometryName;
						updateSkin.geometryData = skin.SkinGeometry;
						signal.Level.RelayBroadcast(updateSkin);
					}

					return;
				}

				try
				{
					if (signal.Tick == 1)
					{
						string fullName = signal.CurrentModel.Keys.First(m => m.StartsWith(signal.Skin.SkinGeometryName));
						var geometry = signal.CurrentModel[fullName];

						if (fullName.Contains(":"))
						{
							Log.Warn($"Inheritance detected for {fullName}");

							var baseGeometry = signal.CurrentModel[fullName.Split(':')[1]];
							signal.CurrentModel.Remove(fullName.Split(':')[1]);
							foreach (var bone in baseGeometry.Bones)
							{
								if (geometry.Bones.SingleOrDefault(b => b.Name == bone.Name) == null)
								{
									geometry.Bones.Add(bone);
								}
							}
						}
						else
						{
							Log.Warn($"NO Inheritance detected for {fullName}");
						}

						Subdivide(geometry, false, true, false, true);

						signal.CurrentModel.Clear();
						signal.CurrentModel.Add(fullName, geometry);

						return;
					}

					int[] flashes = {50, 60, 100, 120, 135, 155, 170, 185, 209, 250, 300, 330, 380, 440};

					if (flashes.Contains((int) signal.Tick + 1))
					{
						signal.Level.StrikeLightning(signal.Position);
					}


					if (flashes.Contains((int) signal.Tick) || flashes.Contains((int) signal.Tick - 4))
					{
						string fullName = signal.CurrentModel.Keys.First(m => m.StartsWith(signal.Skin.SkinGeometryName));
						signal.CurrentModel[fullName].AnimationArmsOutFront = true;
						string skinString = Skin.ToJson(signal.CurrentModel);

						string newName = $"geometry.{DateTime.UtcNow.Ticks}.{signal.Uuid}";
						skinString = skinString.Replace(fullName, newName);
						byte[] skinGeometry = Encoding.UTF8.GetBytes(skinString);

						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = newName;
						updateSkin.geometryData = skinGeometry;
						signal.Level.RelayBroadcast(updateSkin);
					}

					if (flashes.Contains((int) signal.Tick - 2) || flashes.Contains((int) signal.Tick - 6))
					{
						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = skin.SkinGeometryName;
						updateSkin.geometryData = skin.SkinGeometry;
						signal.Level.RelayBroadcast(updateSkin);
					}
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
			finally
			{
				Monitor.Exit(state);
			}
		}


		private void MeltTick(object state)
		{
			if (!Monitor.TryEnter(state)) return;

			try
			{
				StateObject signal = state as StateObject;

				if (signal == null) return;
				if (signal.Timer == null) return;
				if (signal.CurrentModel == null) return;

				if (signal.Tick++ > signal.MaxDuration + signal.StartDelay)
				{
					Log.Warn($"Reached end of animation: {signal.Tick}");
					signal.Tick = 0;
					signal.Timer.Dispose();
					signal.Timer = null;

					// Reset?
					if (signal.ResetOnEnd)
					{
						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = skin.SkinGeometryName;
						updateSkin.geometryData = skin.SkinGeometry;
						signal.Level.RelayBroadcast(updateSkin);
					}

					return;
				}

				try
				{
					if (signal.Tick == 1)
					{
						string fullName = signal.CurrentModel.Keys.First(m => m.StartsWith(signal.Skin.SkinGeometryName));
						var geometry = signal.CurrentModel[fullName];

						if (fullName.Contains(":"))
						{
							Log.Warn($"Inheritance detected for {fullName}");

							var baseGeometry = signal.CurrentModel[fullName.Split(':')[1]];
							signal.CurrentModel.Remove(fullName.Split(':')[1]);
							foreach (var bone in baseGeometry.Bones)
							{
								if (geometry.Bones.SingleOrDefault(b => b.Name == bone.Name) == null)
								{
									geometry.Bones.Add(bone);
								}
							}
						}
						else
						{
							Log.Warn($"NO Inheritance detected for {fullName}");
						}

						Subdivide(geometry, false, true);

						signal.CurrentModel.Clear();
						signal.CurrentModel.Add(fullName, geometry);

						string skinString = Skin.ToJson(signal.CurrentModel);

						string newName = $"geometry.{DateTime.UtcNow.Ticks}.{signal.Uuid}";
						skinString = skinString.Replace(fullName, newName);
						byte[] skinGeometry = Encoding.UTF8.GetBytes(skinString);

						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = newName;
						updateSkin.geometryData = skinGeometry;
						signal.Level.RelayBroadcast(updateSkin);

						return;
					}

					if (signal.Tick < signal.StartDelay)
					{
						return;
					}

					bool stillMoving = false;
					foreach (var geometry in signal.CurrentModel.Values)
					{
						foreach (var bone in geometry.Bones)
						{
							if (bone.NeverRender) continue;
							if (bone.Cubes == null || bone.Cubes.Count == 0) continue;

							foreach (var cube in bone.Cubes)
							{
								if (cube.Velocity == Vector3.Zero) continue;

								stillMoving = true;

								float x = cube.Origin[0];
								float y = cube.Origin[1];
								float z = cube.Origin[2];

								cube.Origin = new[] {x + cube.Velocity.X, Math.Max(0f, y + cube.Velocity.Y), z + cube.Velocity.Z};
								cube.Velocity -= new Vector3(0, (float) Gravity, 0);
								float drag = (float) (1 - Drag);
								cube.Velocity *= drag;
								if (cube.Origin[1] <= 0.05f) cube.Velocity = Vector3.Zero;
							}
						}
					}

					if (!stillMoving) signal.Tick = 10000;

					{
						string fullName = signal.CurrentModel.Keys.First(m => m.StartsWith(signal.Skin.SkinGeometryName));
						string skinString = Skin.ToJson(signal.CurrentModel);

						string newName = $"geometry.{DateTime.UtcNow.Ticks}.{signal.Uuid}";
						skinString = skinString.Replace(fullName, newName);
						byte[] skinGeometry = Encoding.UTF8.GetBytes(skinString);

						Skin skin = signal.Skin;

						McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
						updateSkin.NoBatch = true;
						updateSkin.uuid = signal.Uuid;
						updateSkin.skinId = skin.SkinId;
						updateSkin.skinData = skin.SkinData;
						updateSkin.capeData = skin.CapeData;
						updateSkin.geometryModel = newName;
						updateSkin.geometryData = skinGeometry;
						//signal.Level.RelayBroadcast(updateSkin);
					}
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
			finally
			{
				Monitor.Exit(state);
			}
		}

		private void Subdivide(Geometry geometry, bool packInBody = true, bool keepHead = true, bool renderSkin = true, bool renderSkeleton = false)
		{
			List<Cube> newCubes = new List<Cube>();
			var random = new Random();

			foreach (var bone in geometry.Bones)
			{
				if (bone.NeverRender) continue;
				if (bone.Cubes == null || bone.Cubes.Count == 0) continue;

				Log.Warn($"Splitting cubes for {bone.Name}");

				var cubes = bone.Cubes.ToArray();
				bone.Cubes.Clear();
				foreach (var cube in cubes)
				{
					int width = (int) cube.Size[0];
					int height = (int) cube.Size[1];
					int depth = (int) cube.Size[2];

					float u = cube.Uv[0];
					float v = cube.Uv[1];

					//inside
					if (renderSkeleton)
					{
						for (int w = 0; w < width; w++)
						{
							for (int d = 0; d < depth; d++)
							{
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										Cube c = new Cube();
										var cubeOrigin = cube.Origin;
										c.Size = new[] {1f, 1f, 1f};
										c.Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d};
										c.Uv = new float[] {20, 4};
										c.Velocity = Vector3.Zero;
										{
											bool isHead = bone.Name == "head";
											if (packInBody)
											{
												if (keepHead && isHead)
													bone.Cubes.Add(c);
												else
													newCubes.Add(c);
											}
											else
											{
												bone.Cubes.Add(c);
											}
										}
									}
								}
							}
						}
					}

					if (renderSkin)
					{
						//front
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth - 1;
							if (bone.Mirror)
								uvx = u + depth + width - 2;
							for (int d = 0; d < 1; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d - 0.01f};
									c.Uv = bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--};

									//c.Velocity = new Vector3((float)((random.NextDouble() - 0.5f) * 1.8f), (float)(random.NextDouble() * h / 10 + 1.8f), (float)((random.NextDouble() - 0.5f) * 1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									bool isHead = bone.Name == "head";
									if (isHead || random.NextDouble() < CubeFilterFactor)
									{
										if (packInBody)
										{
											if (keepHead && isHead)
											{
												c.Velocity = Vector3.Zero;
												bone.Cubes.Add(c);
											}
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}

						//back
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth + width + depth - 3;
							if (!bone.Mirror)
								uvx = u + depth + width + depth + width - 4;
							for (int d = depth - 1; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h, cubeOrigin[2] + d + 0.01f};
									c.Uv = !bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--};
									//c.Velocity = new Vector3((float)((random.NextDouble() - 0.5f) * 1.8f), (float)(random.NextDouble() * h / 10 + 1.8f), (float)((random.NextDouble() - 0.5f) * 1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == "head";
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// top
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth - 1;
							if (!bone.Mirror)
								uvx = u + depth + width - 2;
							float uvy = v + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								for (int h = height - 1; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h + 0.01f, cubeOrigin[2] + d};
									c.Uv = !bone.Mirror ? new[] {uvx - w, uvy--} : new[] {uvx + w, uvy--};
									//c.Velocity = new Vector3((float)((random.NextDouble() - 0.5f) * 1.8f), (float)(random.NextDouble() * h / 10 + 1.8f), (float)((random.NextDouble() - 0.5f) * 1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == "head";
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// bottom
						for (int w = 0; w < width; w++)
						{
							float uvx = u + depth + width - 2;
							float uvy = v + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								for (int h = 0; h < 1; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w, cubeOrigin[1] + h - 0.01f, cubeOrigin[2] + d};
									c.Uv = new[] {uvx + w, uvy--};
									//c.Velocity = new Vector3((float)((random.NextDouble() - 0.5f) * 1.8f), (float)(random.NextDouble() * h / 10 + 1.8f), (float)((random.NextDouble() - 0.5f) * 1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == "head";
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// Right
						for (int w = 0; w < 1; w++)
						{
							float uvx = u;
							if (!bone.Mirror)
								uvx = u + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									c.Mirror = bone.Mirror;
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w - 0.01f, cubeOrigin[1] + h, cubeOrigin[2] + d};
									c.Uv = !bone.Mirror ? new[] {uvx - d, uvy--} : new[] {uvx + d, uvy--};
									//c.Velocity = new Vector3((float)((random.NextDouble() - 0.5f) * 1.8f), (float)(random.NextDouble() * h / 10 + 1.8f), (float)((random.NextDouble() - 0.5f) * 1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == "head";
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
						// Left
						for (int w = width - 1; w < width; w++)
						{
							float uvx = u + depth + width - 2;
							if (bone.Mirror)
								uvx = u + depth - 1;
							for (int d = 0; d < depth; d++)
							{
								float uvy = v + depth + height - 2;
								for (int h = 0; h < height; h++)
								{
									if ((w > 0 && w < width - 1) && (d > 0 && d < depth - 1) && (h > 0 && h < height - 1))
									{
										uvy--;
										continue;
									}

									Cube c = new Cube();
									c.Mirror = bone.Mirror;
									var cubeOrigin = cube.Origin;
									c.Size = new[] {1f, 1f, 1f};
									c.Origin = new[] {cubeOrigin[0] + w + 0.01f, cubeOrigin[1] + h, cubeOrigin[2] + d};
									c.Uv = bone.Mirror ? new[] {uvx - d, uvy--} : new[] {uvx + d, uvy--};
									//c.Velocity = new Vector3((float) ((random.NextDouble() - 0.5f)*1.8f), (float) (random.NextDouble()*h/10 + 1.8f), (float) ((random.NextDouble() - 0.5f)*1.8f));
									c.Velocity = new Vector3(0, (float) (random.NextDouble()*-0.01), 0);
									if (random.NextDouble() < CubeFilterFactor)
									{
										bool isHead = bone.Name == "head";
										if (packInBody)
										{
											if (keepHead && isHead)
												bone.Cubes.Add(c);
											else
												newCubes.Add(c);
										}
										else
										{
											bone.Cubes.Add(c);
										}
									}
								}
							}
						}
					}
					// done bones
				}
			}

			if (packInBody)
			{
				Bone newBone = new Bone();
				newBone.Name = "body";
				newBone.Pivot = new float[3];
				newBone.Cubes = newCubes;
				Bone head = geometry.Bones.SingleOrDefault(b => b.Name == "head");
				geometry.Bones = new List<Bone>() {newBone};
				if (keepHead && head != null)
				{
					geometry.Bones.Add(head);
				}
			}
			else
			{
			}
		}
	}
}