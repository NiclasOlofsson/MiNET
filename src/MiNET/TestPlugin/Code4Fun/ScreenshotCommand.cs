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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using log4net;
using MiNET;
using MiNET.Entities;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Skins;
using MiNET.Utils.Vectors;
using TestPlugin.NiceLobby;

namespace TestPlugin.Code4Fun
{
	public class ScreenshotCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ScreenshotCommand));

		ConcurrentDictionary<Tuple<int, int>, PlayerMob> mobs = new ConcurrentDictionary<Tuple<int, int>, PlayerMob>();
		private int _width = 3;
		private int _height = 2;

		[Command]
		public void Screenshot(Player player)
		{
			var coordinates = (BlockCoordinates) player.KnownPosition;

			BlockCoordinates direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection()) * 1.5f;

			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			//byte[] skinBytes = Encoding.ASCII.GetBytes(new string('Z', 64*64*4));
			byte[] skinBytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "test_skin.png"));
			Log.Warn($"Size {skinBytes.Length}");

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					var skinGeometryName = "geometry.flat." + Guid.NewGuid();
					var model = new GeometryModel()
					{
						FormatVersion = "1.12.0",
						Geometry = new List<Geometry>
						{
							new Geometry
							{
								Description = new Description
								{
									Identifier = skinGeometryName,
									TextureHeight = 64,
									TextureWidth = 64,
									VisibleBoundsHeight = 0,
									VisibleBoundsWidth = 0,
									VisibleBoundsOffset = new int[3]
								},
								Name = skinGeometryName,
								Bones = new List<Bone>
								{
									new Bone
									{
										Name = BoneName.Body.ToString(),
										Cubes = new List<Cube>()
										{
											new Cube
											{
												Origin = new float[3],
												//Size = new float[] {68.4f, 68.4f, 0.1f},
												Size = new float[] {62, 62, 1},
												Uv = new float[2] {0, 0}
											}
										}
									}
								}
							}
						},
					};


					//string fileName = Path.GetTempPath() + "Geometry_" + Guid.NewGuid() + ".json";
					//File.WriteAllText(fileName, Skin.ToJson(model));

					var fake = new PlayerMob(string.Empty, player.Level)
					{
						Scale = 1.1,
						Width = 0.01,
						Length = 0.01,
						Height = 0.01,
						Skin = new Skin
						{
							SkinId = "testing" + new Guid(),
							Slim = false,
							Height = 64,
							Width = 64,
							Data = skinBytes,
							GeometryName = skinGeometryName,
							GeometryData = Skin.ToJson(model),
							SkinResourcePatch = new SkinResourcePatch() {Geometry = new GeometryIdentifier() {Default = skinGeometryName}}
						},
						KnownPosition = new PlayerLocation(coordinates.X + direction.X + (x * 4), coordinates.Y + (y * 4), coordinates.Z + direction.Z, 0, 0)
					};
					mobs.TryAdd(new Tuple<int, int>(x, y), fake);
					fake.SpawnEntity();
					//fake.AddToPlayerList();
					//Thread.Sleep(500);
				}
			}
			mobs.First().Value.Ticking += PlayerOnTicking;
		}

		private void PlayerOnTicking(object sender, PlayerEventArgs playerEventArgs)
		{
			{
				var player = (PlayerMob) sender;
				if (player.Level.TickTime % 20 != 0) return;
			}

			using var bmpScreenCapture = new Bitmap(2150, 1519);
			using (var g = Graphics.FromImage(bmpScreenCapture))
			{
				g.CopyFromScreen(1669, 90, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
			}

			using var srcBitmap = new Bitmap(bmpScreenCapture, new Size((_width) * 62, (_height) * 62));
			foreach (var mobCoord in mobs)
			{
				Log.Debug($"Updating {mobCoord.Key.Item1}, {mobCoord.Key.Item2}");
				PlayerMob mob = mobCoord.Value;
				mob.AddToPlayerList();

				int offsetx = (mobCoord.Key.Item1) * 62;
				int offsety = (_height - mobCoord.Key.Item2 - 1) * 62;
				using Bitmap croppedImage = VideoCommand.CropImage(srcBitmap, new Rectangle(offsetx, offsety, 62, 62));
				using Bitmap textureImage = new Bitmap(64, 64);
				var gfx = Graphics.FromImage(textureImage);
				gfx.FillRectangle(Brushes.Black, new Rectangle(0, 0, 64, 64));
				gfx.DrawImageUnscaled(croppedImage, new Point(1, 1));
				var bytes = VideoCommand.BitmapToBytes(textureImage, true);
				//var stream = new MemoryStream();
				//textureImage.Save(stream, ImageFormat.MemoryBmp);
				//var bytes = stream.ToArray();

				string oldSkinId = mob.Skin.SkinId;
				var skin = (Skin) mob.Skin.Clone();
				//var skin = (Skin) mob.Skin;
				skin.Data = bytes;
				skin.SkinId = "testing" + new Guid();
				mob.Skin = skin;

				// Below update doesn't work properly for unknown reasons.

				//var updateSkin = McpePlayerSkin.CreateObject();
				//updateSkin.uuid = mob.ClientUuid;
				//updateSkin.oldSkinName = oldSkinId;
				//updateSkin.skinName = mob.Skin.SkinId;
				//updateSkin.skin = mob.Skin;
				//mob.Level.RelayBroadcast(updateSkin);

				mob.RemoveFromPlayerList();
			}
		}
	}
}