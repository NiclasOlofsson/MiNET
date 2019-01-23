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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using log4net;
using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Skins;
using TestPlugin.NiceLobby;

namespace TestPlugin.Code4Fun
{
	public class ScreenshotCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ScreenshotCommand));

		Dictionary<Tuple<int, int>, PlayerMob> mobs = new Dictionary<Tuple<int, int>, PlayerMob>();
		private int _width = 3;
		private int _height = 2;

		[Command]
		public void Screenshot(Player player)
		{
			var coordinates = player.KnownPosition;
			var direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection()) * 1.5f;

			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			byte[] skinBytes = Skin.GetTextureFromFile(Path.Combine(pluginDirectory, "test_skin.png"));

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					var skinGeometryName = "geometry.flat." + Guid.NewGuid();
					GeometryModel model = new GeometryModel()
					{
						{
							skinGeometryName, new Geometry()
							{
								Name = skinGeometryName,
								Bones = new List<Bone>()
								{
									new Bone()
									{
										Name = BoneName.Body,
										Pivot = new float[3],
										Cubes = new List<Cube>()
										{
											new Cube()
											{
												Origin = new float[3],
												Size = new float[] {64, 64, 1f},
												Uv = new float[] {0, 0}
											}
										}
									}
								}
							}
						},
					};

					PlayerMob fake = new PlayerMob(string.Empty, player.Level)
					{
						Width = 0.1,
						Length = 0.1,
						Height = 0.1,

						Skin = new Skin
						{
							SkinId = "testing" + new Guid(),
							Slim = false,
							SkinData = skinBytes,
							CapeData = new byte[0],
							SkinGeometryName = skinGeometryName,
							SkinGeometry = Skin.ToJson(model),
						},
						KnownPosition = new PlayerLocation(coordinates.X + direction.X + (x * 4), coordinates.Y + (y * 4), coordinates.Z + direction.Z, 0, 0)
					};
					mobs.Add(new Tuple<int, int>(x, y), fake);
					fake.SpawnEntity();
				}
			}
			mobs.First().Value.Ticking += PlayerOnTicking;
		}

		private void PlayerOnTicking(object sender, PlayerEventArgs playerEventArgs)
		{
			{
				var player = (PlayerMob) sender;
				if (player.Level.TickTime % 4 != 0) return;
			}

			using (Bitmap bmpScreenCapture = new Bitmap(1118, 801))
			{
				using (Graphics g = Graphics.FromImage(bmpScreenCapture))
				{
					g.CopyFromScreen(620, 101, 0, 0, bmpScreenCapture.Size, CopyPixelOperation.SourceCopy);
				}

				using (Bitmap srcBitmap = new Bitmap(bmpScreenCapture, new Size((_width) * 64, (_height) * 64)))
				{
					foreach (var mobCoord in mobs)
					{
						PlayerMob mob = mobCoord.Value;
						mob.AddToPlayerList();

						int offsetx = (mobCoord.Key.Item1) * 64;
						int offsety = (_height - mobCoord.Key.Item2 - 1) * 64;
						using (Bitmap bitmap = NiceLobbyPlugin.CropImage(srcBitmap, new Rectangle(offsetx, offsety, 64, 64)))
						{
							var bytes = NiceLobbyPlugin.BitmapToBytes(bitmap, true);

							var skin = mob.Skin;
							skin.SkinData = bytes;

							McpePlayerSkin updateSkin = McpePlayerSkin.CreateObject();
							updateSkin.uuid = mob.ClientUuid;
							updateSkin.skinId = skin.SkinId + new Guid();
							updateSkin.skinData = skin.SkinData;
							updateSkin.capeData = skin.CapeData;
							updateSkin.geometryModel = skin.SkinGeometryName;
							updateSkin.geometryData = skin.SkinGeometry;
							mob.Level.RelayBroadcast(updateSkin);
						}

						mob.RemoveFromPlayerList();
					}
				}
			}
		}
	}
}