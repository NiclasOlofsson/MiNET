using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using SharpAvi;
using SharpAvi.Output;

namespace MiNET.Worlds
{
	public class SkyLightCalculations
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (BlockLightCalculations));


		// Debug tracking, don't enable unless you really need to "see it".

		private bool _trackResults;
		public ConcurrentDictionary<BlockCoordinates, int> Visits { get; } = new ConcurrentDictionary<BlockCoordinates, int>();
		public long StartTimeInMilliseconds { get; set; }

		ConcurrentDictionary<ChunkColumn, bool> _visitedColumns = new ConcurrentDictionary<ChunkColumn, bool>();

		public SkyLightCalculations(bool trackResults = false)
		{
			_trackResults = trackResults;
		}

		public static void Calculate(Level level)
		{
			var chunks = level.GetLoadedChunks().OrderBy(column => column.x).ThenBy(column => column.z);

			_chunkCount = chunks.Count();

			if (_chunkCount == 0) return;

			CheckIfSpawnIsMiddle(chunks, level.SpawnPoint.GetCoordinates3D());

			Stopwatch sw = new Stopwatch();
			sw.Start();
			foreach (var pair in chunks)
			{
				pair.RecalcHeight();
			}
			Log.Debug($"Recalc height for {_chunkCount} chunks, {_chunkCount*16*16*256} blocks. Time {sw.ElapsedMilliseconds}ms");


			SkyLightCalculations calculator = new SkyLightCalculations(Config.GetProperty("CalculateLights.MakeMovie", false));

			int midX = calculator.GetMidX(chunks.ToArray());
			int width = calculator.GetWidth(chunks.ToArray());

			sw.Restart();

			var tickerHighPrecisionTimer = new HighPrecisionTimer(100, _ => calculator.SnapshotVisits());

			calculator.StartTimeInMilliseconds = Environment.TickCount;

			var t0 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenBy(pair => pair.z).Where(chunk => chunk.x <= midX).OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			var t5 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).Where(chunk => chunk.x > midX).OrderBy(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			var t1 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			var t2 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			var t3 = Task.Run(() =>
			{
				var pairs = chunks.OrderByDescending(pair => pair.x).ThenBy(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			var t4 = Task.Run(() =>
			{
				var pairs = chunks.OrderBy(pair => pair.x).ThenByDescending(pair => pair.z).ToArray();
				calculator.CalculateSkyLights(level, pairs);
			});

			Task.WaitAll(t0, t1, t2, t3, t4, t5);

			Log.Debug($"Recalc skylight for {_chunkCount}({_chunkCount}) chunks, {_chunkCount*16*16*256:N0} blocks. Time {sw.ElapsedMilliseconds}ms");

			Task.Run(() =>
			{
				tickerHighPrecisionTimer.Dispose();
				calculator.SnapshotVisits();
				calculator.SnapshotVisits();


				// Start with an end-frame (twitter thumbs)
				var last = calculator.RenderingTasks.Last();
				calculator.RenderingTasks.Remove(last);
				calculator.RenderingTasks.Insert(0, last);

				calculator.RenderVideo();

				Log.Debug($"Movie rendered.");
			});

			//foreach (var chunk in chunks)
			//{
			//	calculator.ShowHeights(chunk);
			//}

			//var chunkColumn = chunks.First(column => column.x == -1 && column.z == 0 );
			//if (chunkColumn != null)
			//{
			//	Log.Debug($"Heights:\n{Package.HexDump(chunkColumn.height)}");
			//	Log.Debug($"skylight.Data:\n{Package.HexDump(chunkColumn.skyLight.Data, 64)}");
			//}
		}

		private int CalculateSkyLights(Level level, ChunkColumn[] chunks)
		{
			int calcCount = 0;
			Stopwatch calcTime = new Stopwatch();
			int lastCount = 0;

			foreach (var chunk in chunks)
			{
				if (!_visitedColumns.TryAdd(chunk, true)) continue;

				if (chunk == null) continue;
				if (chunk.isAllAir) continue;

				calcTime.Restart();
				if (RecalcSkyLight(chunk, level))
				{
					calcCount++;

					var elapsedMilliseconds = calcTime.ElapsedMilliseconds;
					var c = Visits.Sum(pair => pair.Value);
					if (elapsedMilliseconds > 0) Log.Debug($"Recalc skylight for #{calcCount} (air={chunk.isAllAir}) chunks. Time {elapsedMilliseconds}ms and {c - lastCount} visits");
					lastCount = c;
					//PrintVisits();
				}
			}

			Log.Debug($"Recalc skylight for #{calcCount} chunk.");

			return calcCount;
		}

		public bool RecalcSkyLight(ChunkColumn chunk, Level level)
		{
			if (chunk == null) return false;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					if (chunk.isAllAir && !IsOnChunkBorder(x, z))
					{
						continue;
					}

					int height = GetHigestSurrounding(x, z, chunk, level);
					if (height == 0)
					{
						continue;
					}

					//var skyLight = chunk.GetSkyLight(x, height, z);
					//if (skyLight == 15)
					{
						Block block = level.GetBlock(new BlockCoordinates(x + (chunk.x*16), height, z + (chunk.z*16)));
						Calculate(level, block);
					}
					//else
					//{
					//	Log.Error($"Block with wrong light level. Expected 15 but was {skyLight}");
					//}
				}
			}

			return true;
		}

		private void Calculate(Level level, Block block)
		{
			try
			{
				if (block.SkyLight != 15)
				{
					Log.Error($"Block at {block.Coordinates} had unexpected light level. Expected 15 but was {block.SkyLight}");
				}

				Queue<BlockCoordinates> lightBfsQueue = new Queue<BlockCoordinates>();

				/*if (!lightBfsQueue.Contains(block.Coordinates)) */
				lightBfsQueue.Enqueue(block.Coordinates);
				while (lightBfsQueue.Count > 0)
				{
					ProcessNode(level, lightBfsQueue.Dequeue(), lightBfsQueue);
				}
			}
			catch (Exception e)
			{
				Log.Error("Calculation", e);
			}
		}

		private void ProcessNode(Level level, BlockCoordinates coordinates, Queue<BlockCoordinates> lightBfsQueue)
		{
			byte currentSkyLight = level.GetSkyLight(coordinates);

			byte maxSkyLight = currentSkyLight;
			if (coordinates.Y < 255)
			{
				var up = coordinates + BlockCoordinates.Up;
				maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, up, currentSkyLight, up: true));
			}

			if (coordinates.Y > 0)
			{
				var down = coordinates + BlockCoordinates.Down;
				maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, down, currentSkyLight, down: true));
			}

			var west = coordinates + BlockCoordinates.West;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, west, currentSkyLight));


			var east = coordinates + BlockCoordinates.East;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, east, currentSkyLight));


			var south = coordinates + BlockCoordinates.South;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, south, currentSkyLight));

			var north = coordinates + BlockCoordinates.North;
			maxSkyLight = Math.Max(maxSkyLight, SetLightLevel(level, lightBfsQueue, north, currentSkyLight));

			if (level.IsAir(coordinates) && currentSkyLight != 15)
			{
				maxSkyLight = (byte) Math.Max(currentSkyLight, maxSkyLight - 1);

				if (maxSkyLight > currentSkyLight)
				{
					level.SetSkyLight(coordinates, maxSkyLight);

					/*if (!lightBfsQueue.Contains(coordinates)) */
					lightBfsQueue.Enqueue(coordinates);
				}
			}
		}

		private byte SetLightLevel(Level level, Queue<BlockCoordinates> lightBfsQueue, BlockCoordinates coordinates, byte lightLevel, bool down = false, bool up = false)
		{
			if (_trackResults) MakeVisit(coordinates);

			if (!down && !up && coordinates.Y >= level.GetHeight(coordinates))
			{
				level.SetSkyLight(coordinates, 15);
				return 15;
			}

			bool isTransparent = level.IsTransparent(coordinates);
			byte skyLight = level.GetSkyLight(coordinates);

			if (down && isTransparent && lightLevel == 15)
			{
				if (skyLight != 15)
				{
					level.SetSkyLight(coordinates, 15);
				}

				/*if (!lightBfsQueue.Contains(coordinates)) */
				lightBfsQueue.Enqueue(coordinates);

				return 15;
			}

			if (isTransparent && skyLight + 2 <= lightLevel)
			{
				byte newLevel = (byte) (lightLevel - 1);
				level.SetSkyLight(coordinates, newLevel);

				/*if (!lightBfsQueue.Contains(coordinates)) */
				lightBfsQueue.Enqueue(coordinates);
				return newLevel;
			}

			return skyLight;
		}

		private void MakeVisit(BlockCoordinates inc)
		{
			BlockCoordinates coordinates = new BlockCoordinates(inc.X, 0, inc.Z);
			if (Visits.ContainsKey(coordinates))
			{
				Visits[coordinates] = Visits[coordinates] + 1;
			}
			else
			{
				Visits.TryAdd(coordinates, 1);
			}
		}

		public static void CheckIfSpawnIsMiddle(IOrderedEnumerable<ChunkColumn> chunks, Vector3 spawnPoint)
		{
			int xMin = chunks.OrderBy(kvp => kvp.x).First().x;
			int xMax = chunks.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			int zMin = chunks.OrderBy(kvp => kvp.z).First().z;
			int zMax = chunks.OrderByDescending(kvp => kvp.z).First().z;
			int zd = Math.Abs(zMax - zMin);

			int xm = (int) ((xd/2f) + xMin);
			int zm = (int) ((zd/2f) + zMin);

			if (xm != (int) spawnPoint.X >> 4) Log.Warn($"Wrong spawn X={xm}, {(int) spawnPoint.X >> 4}");
			if (zm != (int) spawnPoint.Z >> 4) Log.Warn($"Wrong spawn Z={zm}, {(int) spawnPoint.Z >> 4}");

			if (zm == (int) spawnPoint.Z >> 4 && xm == (int) spawnPoint.X >> 4) Log.Warn($"Spawn correct {xm}, {zm} and {(int) spawnPoint.X >> 4}, {(int) spawnPoint.Z >> 4}");
		}

		private object _imageSync = new object();
		private static int _chunkCount;

		public List<Task<Bitmap>> RenderingTasks { get; } = new List<Task<Bitmap>>();

		public void SnapshotVisits()
		{
			lock (_imageSync)
			{
				if (!_trackResults) return;

				var visits1 = Visits.ToArray();

				if (visits1.Length == 0) return;

				long time = Environment.TickCount;

				Task<Bitmap> t = new Task<Bitmap>(v =>
				{
					var fileId = time;

					try
					{
						var visits = v as KeyValuePair<BlockCoordinates, int>[];

						int valMax = Visits.OrderByDescending(kvp => kvp.Value).First().Value;
						int valMin = visits.OrderBy(kvp => kvp.Value).First().Value;

						int xMin = visits.OrderBy(kvp => kvp.Key.X).First().Key.X;
						int xMax = visits.OrderByDescending(kvp => kvp.Key.X).First().Key.X;
						int xd = Math.Abs(xMax - xMin);

						int zMin = visits.OrderBy(kvp => kvp.Key.Z).First().Key.Z;
						int zMax = visits.OrderByDescending(kvp => kvp.Key.Z).First().Key.Z;
						int zd = Math.Abs(zMax - zMin);

						int zMov = zMin < 0 ? Math.Abs(zMin) : zMin*-1;
						int xMov = xMin < 0 ? Math.Abs(xMin) : xMin*-1;

						//Bitmap bitmap = new Bitmap(xd + 1, zd + 1, PixelFormat.Format32bppArgb);
						Bitmap bitmap = new Bitmap(GetWidth(), GetHeight(), PixelFormat.Format32bppArgb);

						foreach (var visit in visits)
						{
							try
							{
								double logBase = 4;
								double min = Math.Abs(Math.Ceiling(Math.Log(1, logBase)));
								if (visit.Value == 0) continue;
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, new ColorHeatMap().GetColorForValue(visit.Value, valMax));
								bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, new ColorHeatMap().GetColorForValue(Math.Log(visit.Value, logBase) + min, Math.Log(valMax, logBase) + min));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Log(visit.Value, logBase) + min, Math.Log(valMax, logBase) + min));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Log(visit.Value + 3), Math.Log(valMax + 3)));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(Math.Pow(visit.Value, 10), Math.Pow(valMax, 10)));
								//bitmap.SetPixel(visit.Key.X + xMov, visit.Key.Z + zMov, CreateHeatColor(visit.Value, valMax));
							}
							catch (Exception e)
							{
								Log.Error($"{xd}, {zd}, {xMin}, {zMin}, {xMax}, {zMax}, X={visit.Key.X}, Z={visit.Key.Z}, {xMov}, {zMov}", e);
								break;
							}
						}
						//byte[] bytes = new byte[xd*zd*4];
						//int i = 0;
						//for (int x = 0; x < xd; x++)
						//{
						//    for (int z = 0; z < zd; z++)
						//    {
						//        bytes[i++*4] = image[x, z];
						//    }
						//}


						//var interval = valMax/zd;
						//for (int i = 0; i < zd; i++)
						//{
						//    //var value = (i * interval) + 3;
						//    //var max = valMax;
						//    var value = Math.Log((i*interval) + 3);
						//    var max = Math.Log(valMax);

						//    bitmap.SetPixel(0, i, CreateHeatColor((int) value, (decimal) max));
						//    bitmap.SetPixel(1, i, CreateHeatColor((int) value, (decimal) max));
						//}


						//using (Graphics g = Graphics.FromImage(bitmap))
						//{
						//    int tz = 0;
						//    for (int i = 10 - 1; i >= 0; i--)
						//    {
						//        var d = i*(valMax/10);
						//        g.DrawString($"{d}={Math.Log(d) :##.00}", new Font("Arial", 8), new SolidBrush(Color.White), 2, (tz++)*zd/10f); // requires font, brush etc
						//    }
						//}

						using (Graphics g = Graphics.FromImage(bitmap))
						{
							g.DrawString($"MiNET skylight calculation\nTime (ms): {fileId - StartTimeInMilliseconds :N0}\n{_chunkCount :N0} chunks with {(_chunkCount*16*16*256) :N0} blocks\n{visits.Sum(pair => pair.Value):N0} visits", new Font("Arial", 8), new SolidBrush(Color.White), 1, 0); // requires font, brush etc
						}

						//Directory.CreateDirectory(@"D:\Temp\Light\");

						//lock (_imageSync)
						//{
						//	bitmap.Save(@"D:\Temp\Light\test-" + $"{fileId :00000}.bmp", ImageFormat.Bmp);
						//}

						//bitmap.Dispose();
						//GC.Collect();

						//foreach (var visit in visits)
						//{
						//	Log.Debug($"Visit {visit.Key} {visit.Value} times");
						//}

						Log.Debug($"Made a total of {visits.Sum(pair => pair.Value) :N0} visits");
						return bitmap;
					}
					catch (Exception e)
					{
						Log.Error("Rendering", e);
					}

					return null;
				}, visits1);
				RenderingTasks.Add(t);
			}
		}

		private int GetMidX(ChunkColumn[] chunks)
		{
			if (!_trackResults) return 0;

			var visits = chunks.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.x).First().x;
			int xMax = visits.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			return xMin + xd/2;
		}

		private int GetWidth(ChunkColumn[] chunks)
		{
			if (!_trackResults) return 0;

			var visits = chunks.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.x).First().x;
			int xMax = visits.OrderByDescending(kvp => kvp.x).First().x;
			int xd = Math.Abs(xMax - xMin);

			return xd;
		}

		private int GetWidth()
		{
			if (!_trackResults) return 0;

			var visits = Visits.ToArray();

			int xMin = visits.OrderBy(kvp => kvp.Key.X).First().Key.X;
			int xMax = visits.OrderByDescending(kvp => kvp.Key.X).First().Key.X;
			int xd = Math.Abs(xMax - xMin);

			return xd + 1;
		}

		private int GetHeight()
		{
			if (!_trackResults) return 0;

			var visits = Visits.ToArray();

			int zMin = visits.OrderBy(kvp => kvp.Key.Z).First().Key.Z;
			int zMax = visits.OrderByDescending(kvp => kvp.Key.Z).First().Key.Z;
			int zd = Math.Abs(zMax - zMin);

			return zd + 1;
		}

		private void RenderVideo()
		{
			try
			{
				if (!_trackResults) return;


				Log.Debug($"Generated all images, now rendering movie.");

				//var files = Directory.EnumerateFiles(@"D:\Temp\Light\", "*.bmp");
				//files = files.OrderBy(s => s);

				//int fps = (int) (RenderingTasks.Count()/10f); // Movie should last 5 seconds
				int fps = 10;

				var writer = new AviWriter(@"D:\Temp\Light\test.avi")
				{
					FramesPerSecond = fps,
					// Emitting AVI v1 index in addition to OpenDML index (AVI v2)
					// improves compatibility with some software, including 
					// standard Windows programs like Media Player and File Explorer
					EmitIndex1 = true
				};

				var stream = writer.AddVideoStream();
				stream.Width = GetWidth();
				stream.Height = GetHeight();
				stream.Codec = KnownFourCCs.Codecs.Uncompressed;

				stream.BitsPerPixel = BitsPerPixel.Bpp32;

				Log.Debug($"Waiting for image rendering of {RenderingTasks.Count} images to complete");
				foreach (var renderingTask in RenderingTasks)
				{
					renderingTask.RunSynchronously();
					Bitmap image = renderingTask.Result;
					//}

					//foreach (var file in files)
					//{
					lock (_imageSync)
					{
						//Bitmap image = (Bitmap) Image.FromFile(file);
						//image = new Bitmap(image, stream.Width, stream.Height);

						byte[] imageData = (byte[]) ToByteArray(image, ImageFormat.Bmp);

						if (imageData == null)
						{
							Log.Warn($"No image data for file.");
							continue;
						}

						if (imageData.Length != stream.Height*stream.Width*4)
						{
							imageData = imageData.Skip(imageData.Length - (stream.Height*stream.Width*4)).ToArray();
						}

						// fill frameData with image

						// write data to a frame
						stream.WriteFrame(true, // is key frame? (many codecs use concept of key frames, for others - all frames are keys)
							imageData, // array with frame data
							0, // starting index in the array
							imageData.Length // length of the data
							);
					}
				}

				writer.Close();
			}
			catch (Exception e)
			{
				Log.Error("Rendering movie", e);
			}
		}

		public static byte[] ToByteArray(Image image, ImageFormat format)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				image.Save(ms, format);
				return ms.ToArray();
			}
		}

		static Color CreateHeatColor(double value, double max)
		{
			//if (value < 0) value = 0;

			//Log.Debug($"Before Value={value}, min={min}, max={max}");

			double pct = value/max;
			if (pct < 0) pct = 0;

			//Log.Debug($"Value={v :F2}, max={m:F2}, pct={pct:F2}");

			return Color.FromArgb(255, (byte) (255.0f*pct), (byte) (255.0f*(1 - pct)), 0);

			//int A = 255;
			//int R;
			//int G;
			//int B;
			//if (pct < 0.34d)
			//{
			//	R = 255;
			//	G = (byte) (255*Math.Min(3*(pct - 0.333333d), 1d));
			//	B = 0;
			//}
			//else if (pct < 0.67d)
			//{
			//	R = (byte) (255*Math.Min(3*(1d - pct), 1d));
			//	G = 255;
			//	B = 0;
			//}
			//else
			//{
			//	R = (byte) (128 + (127*Math.Min(3*pct, 1d)));
			//	G = 0;
			//	B = 0;
			//}

			//return Color.FromArgb(A, R, G, B);
		}

		private static bool IsOnChunkBorder(int x, int z)
		{
			return !(x > 0 && x < 15 && z > 0 && z < 15);
		}

		private static int GetHigestSurrounding(int x, int z, ChunkColumn chunk, Level level)
		{
			int h = chunk.GetHeight(x, z);
			if (h == 255) return h;

			if (x == 0 || x == 15 || z == 0 || z == 15)
			{
				var coords = new BlockCoordinates(x + (chunk.x*16), h, z + (chunk.z*16));

				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.Up));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.West));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.East));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.North));
				h = Math.Max(h, level.GetHeight(coords + BlockCoordinates.South));
				if (h > 255) h = 255;
				if (h < 0) h = 0;
				return h;
			}

			//if (z < 15) h = Math.Max(h, chunk.GetHeight(x, z + 1));
			//if (z > 0) h = Math.Max(h, chunk.GetHeight(x, z - 1));
			//if (x < 15) h = Math.Max(h, chunk.GetHeight(x + 1, z));
			//if (x < 15 && z > 0) h = Math.Max(h, chunk.GetHeight(x + 1, z - 1));
			//if (x < 15 && z < 15) h = Math.Max(h, chunk.GetHeight(x + 1, z + 1));
			//if (x > 0) h = Math.Max(h, chunk.GetHeight(x - 1, z));
			//if (x > 0 && z > 0) h = Math.Max(h, chunk.GetHeight(x - 1, z - 1));
			//if (x > 0 && z < 15) h = Math.Max(h, chunk.GetHeight(x - 1, z + 1));

			h = Math.Max(h, chunk.GetHeight(x, z + 1));
			h = Math.Max(h, chunk.GetHeight(x, z - 1));
			h = Math.Max(h, chunk.GetHeight(x + 1, z));
			h = Math.Max(h, chunk.GetHeight(x + 1, z - 1));
			h = Math.Max(h, chunk.GetHeight(x + 1, z + 1));
			h = Math.Max(h, chunk.GetHeight(x - 1, z));
			h = Math.Max(h, chunk.GetHeight(x - 1, z - 1));
			h = Math.Max(h, chunk.GetHeight(x - 1, z + 1));

			return h;
		}

		public void ShowHeights(ChunkColumn chunk)
		{
			if (chunk == null) return;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (byte y = 255; y > 0; y--)
					{
						if (chunk.GetSkylight(x, y, z) == 0)
						{
							chunk.SetBlock(x, y, z, 41);
						}
					}
				}
			}
		}
	}

	public class ColorHeatMap
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ColorHeatMap));

		public ColorHeatMap()
		{
			InitColorsBlocks();
		}

		public ColorHeatMap(byte alpha)
		{
			this.Alpha = alpha;
			InitColorsBlocks();
		}

		private void InitColorsBlocks()
		{
			ColorsOfMap.AddRange(new Color[]
			{
				Color.FromArgb(Alpha, 0, 0, 0), //Black
				Color.FromArgb(Alpha, 0, 0, 0xFF), //Blue
				Color.FromArgb(Alpha, 0, 0xFF, 0xFF), //Cyan
				Color.FromArgb(Alpha, 0, 0xFF, 0), //Green
				Color.FromArgb(Alpha, 0xFF, 0xFF, 0), //Yellow
				Color.FromArgb(Alpha, 0xFF, 0, 0), //Red
				Color.FromArgb(Alpha, 0xFF, 0xFF, 0xFF) // White
			});
		}

		public Color GetColorForValue(double val, double maxVal)
		{
			double valPerc = val/maxVal; // value%
			if (valPerc < 0) valPerc = 0.1;
			if (valPerc > 1.0) valPerc = 1;
			double colorPerc = 1d/(ColorsOfMap.Count - 2); // % of each block of color. the last is the "100% Color"
			double blockOfColor = valPerc/colorPerc; // the integer part repersents how many block to skip
			int blockIdx = (int) Math.Truncate(blockOfColor); // Idx of 
			double valPercResidual = valPerc - (blockIdx*colorPerc); //remove the part represented of block 
			double percOfColor = valPercResidual/colorPerc; // % of color of this block that will be filled

			Color cTarget = ColorsOfMap[blockIdx];
			Color cNext = ColorsOfMap[blockIdx + 1];

			var deltaR = cNext.R - cTarget.R;
			var deltaG = cNext.G - cTarget.G;
			var deltaB = cNext.B - cTarget.B;

			var R = cTarget.R + (deltaR*percOfColor);
			var G = cTarget.G + (deltaG*percOfColor);
			var B = cTarget.B + (deltaB*percOfColor);

			Color c = ColorsOfMap[0];
			try
			{
				c = Color.FromArgb(Alpha, (byte) R, (byte) G, (byte) B);
			}
			catch (Exception ex)
			{
			}
			return c;
		}

		public byte Alpha = 0xff;
		public List<Color> ColorsOfMap = new List<Color>();
	}
}