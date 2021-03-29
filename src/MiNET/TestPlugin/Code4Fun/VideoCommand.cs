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
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities.ImageProviders;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Utils.Vectors;
using TestPlugin.NiceLobby;

namespace TestPlugin.Code4Fun
{
	public class VideoCommand
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(VideoCommand));

		/// <summary>
		///     This command will send each fram one time in a unique map. And then replace
		///     the maps in the frame on ticks.
		/// </summary>
		[Command]
		[Authorize(Permission = (int) CommandPermission.Admin)]
		public void Video2X(Player player, int numberOfFrames, bool color)
		{
			Task.Run(delegate
			{
				try
				{
					var entities = new ConcurrentDictionary<Tuple<int, int>, MapEntity[]>();

					int width = 6;
					int height = 3;
					int frameCount = numberOfFrames;
					//int frameOffset = 0;
					int frameOffset = 120;

					var frameTicker = new FrameTicker(frameCount);

					// 768x384
					Parallel.For(frameOffset, frameOffset + frameCount, (frame) =>
					{
						Log.Info($"Generating frame {frame}");

						string file = Path.Combine(@"C:\Development\Other\Smash Heroes 3x6 (128)\Smash Heroes 3x6 (128)", $"Smash Heroes Trailer{frame:D4}.bmp");
						//string file = Path.Combine(@"D:\Development\Other\2 by 1 PE test app ad for Gurun-2\exported frames 2", $"pe app ad{frame:D2}.bmp");
						if (!File.Exists(file))
						{
							Log.Warn($"Couldn't find file: {file}");
							return;
						}

						var image = new Bitmap((Bitmap) Image.FromFile(file), width * 128, height * 128);

						for (int x = 0; x < width; x++)
						{
							for (int y = 0; y < height; y++)
							{
								var key = new Tuple<int, int>(x, y);
								var frames = entities.GetOrAdd(key, new MapEntity[frameCount]);

								var croppedImage = CropImage(image, new Rectangle(new Point(x * 128, y * 128), new Size(128, 128)));
								byte[] bitmapToBytes = BitmapToBytes(croppedImage, color);

								if (bitmapToBytes.Length != 128 * 128 * 4) return;

								var entity = new MapEntity(player.Level);
								var cachedPacket = CreateCachedPacket(entity.EntityId, bitmapToBytes);

								player.SendPacket(cachedPacket);
								entity.ImageProvider = new MapImageProvider {Batch = cachedPacket};
								entity.SpawnEntity();

								frames[frame - frameOffset] = entity;
							}
						}
					});

					//int i = 0;
					//player.Inventory.Slots[i++] = new ItemBlock(new Planks(), 0) {Count = 64, UniqueId = Environment.TickCount};
					//foreach (var entites in entities.Values)
					//{
					//	player.Inventory.Slots[i++] = new CustomItemFrame(entites.Keys.ToList(), frameTicker) {Count = 64, UniqueId = Environment.TickCount};
					//}
					//player.SendPlayerInventory();

					player.SendMessage("Done generating video.", MessageType.Raw);

					BlockCoordinates center = player.KnownPosition.GetCoordinates3D();
					var level = player.Level;

					for (int x = 0; x < width; x++)
					{
						for (int y = 0; y < height; y++)
						{
							var key = new Tuple<int, int>(x, y);
							var frames = new List<MapEntity>(entities[key]);
							frames = frames.Where(f => f != null).ToList();

							var bc = new BlockCoordinates(center.X - x, center.Y + height - y - 1, center.Z + 2);
							var wood = new Planks {Coordinates = bc};
							level.SetBlock(wood);

							var frambc = new BlockCoordinates(center.X - x, center.Y + height - y - 1, center.Z + 1);
							var itemFrameBlockEntity = new ItemFrameBlockEntity {Coordinates = frambc};

							var itemFrame = new CustomFrame(frames, itemFrameBlockEntity, level, frameTicker)
							{
								Coordinates = frambc,
								FacingDirection = (int) BlockFace.North
							};
							level.SetBlock(itemFrame);
							level.SetBlockEntity(itemFrameBlockEntity);
						}
					}
				}
				catch (Exception e)
				{
					Log.Error("Aborted video generation", e);
				}
			});

			player.SendMessage("Generating video...", MessageType.Raw);
		}

		/// <summary>
		///     This command will update the map image each frame. So the maps are constant and only one map per
		///     coordinate, and frames update the actual map.
		/// </summary>
		[Command]
		[Authorize(Permission = (int) CommandPermission.Admin)]
		public void VideoX(Player player, int numberOfFrames, bool color)
		{
			Task.Run(delegate
			{
				try
				{
					Dictionary<Tuple<int, int>, MapEntity> entities = new Dictionary<Tuple<int, int>, MapEntity>();

					int width = 6;
					int height = 3;
					int frameCount = numberOfFrames;
					//int frameOffset = 0;
					int frameOffset = 120;

					var frameTicker = new FrameTicker(frameCount);


					// 768x384
					for (int frame = frameOffset; frame < frameCount + frameOffset; frame++)
					{
						Log.Info($"Generating frame {frame}");

						string file = Path.Combine(@"C:\Development\Other\Smash Heroes 3x6 (128)\Smash Heroes 3x6 (128)", $"Smash Heroes Trailer{frame:D4}.bmp");
						//string file = Path.Combine(@"D:\Development\Other\2 by 1 PE test app ad for Gurun-2\exported frames 2", $"pe app ad{frame:D2}.bmp");
						if (!File.Exists(file)) continue;

						Bitmap image = new Bitmap((Bitmap) Image.FromFile(file), width * 128, height * 128);

						for (int x = 0; x < width; x++)
						{
							for (int y = 0; y < height; y++)
							{
								var key = new Tuple<int, int>(x, y);
								if (!entities.ContainsKey(key))
								{
									entities.Add(key, new MapEntity(player.Level) {ImageProvider = new VideoImageProvider(frameTicker)});
								}

								var croppedImage = CropImage(image, new Rectangle(new Point(x * 128, y * 128), new Size(128, 128)));
								byte[] bitmapToBytes = BitmapToBytes(croppedImage, color);

								if (bitmapToBytes.Length != 128 * 128 * 4) return;

								((VideoImageProvider) entities[key].ImageProvider).Frames.Add(CreateCachedPacket(entities[key].EntityId, bitmapToBytes));
							}
						}
					}

					int i = 0;

					player.Inventory.Slots[i++] = new ItemBlock(new Planks(), 0) {Count = 64};
					player.Inventory.Slots[i++] = new ItemFrame {Count = 64};

					foreach (MapEntity entity in entities.Values)
					{
						entity.SpawnEntity();
						player.Inventory.Slots[i++] = new ItemMap(entity.EntityId);
					}

					player.SendPlayerInventory();
					player.SendMessage("Done generating video.", MessageType.Raw);
				}
				catch (Exception e)
				{
					Log.Error("Aborted video generation", e);
				}
			});

			player.SendMessage("Generating video...", MessageType.Raw);
		}

		private static byte[] ReadFrame(string filename)
		{
			Bitmap bitmap;
			try
			{
				bitmap = new Bitmap(filename);
			}
			catch (Exception e)
			{
				Log.Error("Failed reading file " + filename);
				bitmap = new Bitmap(128, 128);
			}

			byte[] bytes = BitmapToBytes(bitmap);

			return bytes;
		}


		public static Bitmap CropImage(Bitmap img, Rectangle cropArea)
		{
			return img.Clone(cropArea, img.PixelFormat);
		}

		public static byte[] BitmapToBytes(Bitmap bitmap, bool useColor = false)
		{
			byte[] bytes;
			bytes = new byte[bitmap.Height * bitmap.Width * 4];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color color = bitmap.GetPixel(x, y);
					if (!useColor)
					{
						byte rgb = (byte) ((color.R + color.G + color.B) / 3);
						bytes[i++] = rgb;
						bytes[i++] = rgb;
						bytes[i++] = rgb;
						bytes[i++] = 0xff;
					}
					else
					{
						bytes[i++] = color.R;
						bytes[i++] = color.G;
						bytes[i++] = color.B;
						bytes[i++] = 0xff;
					}
				}
			}
			return bytes;
		}

		private Bitmap GrayScale(Bitmap bmp)
		{
			for (int y = 0; y < bmp.Height; y++)
			{
				for (int x = 0; x < bmp.Width; x++)
				{
					var c = bmp.GetPixel(x, y);
					var rgb = (int) ((c.R + c.G + c.B) / 3);
					bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
				}
			}
			return bmp;
		}

		private McpeWrapper CreateCachedPacket(long mapId, byte[] bitmapToBytes)
		{
			MapInfo mapInfo = new MapInfo
			{
				MapId = mapId,
				UpdateType = 2,
				Scale = 0,
				X = 0,
				Z = 0,
				Col = 128,
				Row = 128,
				XOffset = 0,
				ZOffset = 0,
				Data = bitmapToBytes,
			};

			var packet = McpeClientboundMapItemData.CreateObject();
			packet.mapinfo = mapInfo;
			var batch = CreateMcpeBatch(packet.Encode());

			return batch;
		}

		internal static McpeWrapper CreateMcpeBatch(byte[] bytes)
		{
			McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, (int) bytes.Length), CompressionLevel.Optimal, true);
			batch.MarkPermanent();
			batch.Encode();
			return batch;
		}
	}
}