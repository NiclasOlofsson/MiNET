//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using Craft.Net.Anvil;
//using Craft.Net.Common;
//using log4net;
//using MiNET.Utils;
//using Vector3 = MiNET.Utils.Vector3;

//namespace MiNET.Worlds
//{
//	public class CraftNetAnvilWorldProvider : IWorldProvider
//	{
//		private static readonly ILog Log = LogManager.GetLogger(typeof (CraftNetAnvilWorldProvider));

//		private readonly Dictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new Dictionary<ChunkCoordinates, ChunkColumn>();
//		private Craft.Net.Anvil.Level _level;
//		private List<int> _gaps;
//		private List<int> _ignore;
//		private byte _offsetY;

//		public bool IsCaching { get; private set; }


//		public CraftNetAnvilWorldProvider()
//		{
//			IsCaching = true;
//		}

//		public void Initialize()
//		{
//			_level = Craft.Net.Anvil.Level.LoadFrom(ConfigParser.GetProperty("PCWorldFolder", "World"));
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Development\Csharp\world2\Sandstone Test World\"); _offsetY = 0;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\Mountain Sky Village\§4§kd§  Mountain Sky Village §4§kd§"); _offsetY = 0;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\KingsLanding1\KingsLanding1"); _offsetY = 30;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Development\Csharp\CruiseShipV2.0\whatsthis"); _offsetY = 0;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Development\Csharp\Royal Navy"); _offsetY = 0;
////			_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Development\Csharp\FunLand+3_1\FunLand 3.1"); _offsetY = 0;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\[CREATIVE]_Eldaria_V3.1_by_Aurelien_Sama\[CREATIVE]_Eldaria_V3.1_by_Aurelien_Sama"); _offsetY = 60;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\parrotkingdom\parrotkingdom"); _offsetY = 60;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\BlugoughTown\Blugough"); _offsetY = 30;
//			//_level = Craft.Net.Anvil.Level.LoadFrom(@"C:\Users\nicke_000\Downloads\spawn IceSebos\spawn IceSebos"); _offsetY = 30;

//			Debug.WriteLine("FlowingWater level: " + _level.DefaultWorld);

//			_ignore = new List<int>();
//			_ignore.Add(23);
//			_ignore.Add(25);
//			_ignore.Add(28);
//			_ignore.Add(29);
//			_ignore.Add(33);
//			_ignore.Add(34);
//			_ignore.Add(36);
//			_ignore.Add(55);
//			_ignore.Add(69);
//			_ignore.Add(70);
//			_ignore.Add(71);
//			_ignore.Add(72);
////			_ignore.Add(75);
////			_ignore.Add(76);
//			_ignore.Add(77);
//			_ignore.Add(84);
//			_ignore.Add(87);
//			_ignore.Add(88);
//			_ignore.Add(93);
//			_ignore.Add(94);
//			_ignore.Add(97);
//			_ignore.Add(113);
//			_ignore.Add(115);
//			_ignore.Add(117);
//			_ignore.Add(118);
////			_ignore.Add(123);
//			_ignore.Add(131);
//			_ignore.Add(132);
//			_ignore.Add(138);
//			_ignore.Add(140);
//			_ignore.Add(143);
//			_ignore.Add(144);
//			_ignore.Add(145);
//			_ignore.Sort();

//			_gaps = new List<int>();
//			_gaps.Add(23);
//			_gaps.Add(25);
////			_gaps.Add(27);
//			_gaps.Add(28);
//			_gaps.Add(29);
//			_gaps.Add(33);
//			_gaps.Add(34);
//			_gaps.Add(36);
//			_gaps.Add(55);
////			_gaps.Add(66);
//			_gaps.Add(69);
//			_gaps.Add(70);
//			_gaps.Add(72);
//			_gaps.Add(75);
//			_gaps.Add(76);
//			_gaps.Add(77);
//			_gaps.Add(84);
////			_gaps.Add(87);
//			_gaps.Add(88);
//			_gaps.Add(90);
//			_gaps.Add(93);
//			_gaps.Add(94);
//			_gaps.Add(95);
//			_gaps.Add(97);
////			_gaps.Add(99);
////			_gaps.Add(100);
////			_gaps.Add(106);
////			_gaps.Add(111);
//			_gaps.Add(115);
//			_gaps.Add(116);
//			_gaps.Add(117);
//			_gaps.Add(118);
//			_gaps.Add(119);
////			_gaps.Add(120);
////			_gaps.Add(121);
//			_gaps.Add(122);
//			_gaps.Add(123);
//			_gaps.Add(124);
//			_gaps.Add(125);
//			_gaps.Add(126);
////			_gaps.Add(127);
//			_gaps.Add(130);
//			_gaps.Add(131);
//			_gaps.Add(132);
//			_gaps.Add(137);
//			_gaps.Add(138);
//			_gaps.Add(140);
//			_gaps.Add(143);
//			_gaps.Add(144);
//			_gaps.Add(145);
//			_gaps.Add(146);
//			_gaps.Add(147);
//			_gaps.Add(148);
//			_gaps.Add(149);
//			_gaps.Add(150);
//			_gaps.Add(151);
//			_gaps.Add(152);
//			_gaps.Add(153);
//			_gaps.Add(154);
//			_gaps.Add(160);
//			_gaps.Add(165);
//			_gaps.Add(166);
//			_gaps.Add(167);
//			_gaps.Add(168);
//			_gaps.Add(169);
//			_gaps.Sort();
//		}

//		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
//		{
//			lock (_chunkCache)
//			{
//				ChunkColumn cachedChunk;
//				_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk);

//				if (cachedChunk != null)
//				{
//					return cachedChunk;
//				}

//				Coordinates2D anvilCoord = new Coordinates2D(chunkCoordinates.X, chunkCoordinates.Z);

//				Chunk anvilChunk = _level.DefaultWorld.GetChunk(anvilCoord);
//				ChunkColumn chunk = new ChunkColumn {x = chunkCoordinates.X, z = chunkCoordinates.Z};

//				chunk.biomeId = anvilChunk.Biomes;
//				for (int i = 0; i < chunk.biomeId.Length; i++)
//				{
//					if (chunk.biomeId[i] > 22) chunk.biomeId[i] = 0;
//				}
//				if (chunk.biomeId.Length > 256) throw new Exception();

//				Stopwatch stopwatch = new Stopwatch();
//				stopwatch.Restart();
//				for (byte xi = 0; xi < 16; xi++)
//				{
//					for (byte zi = 0; zi < 16; zi++)
//					{
//						for (byte yi = 0; yi < 128; yi++)
//						{
//							int yoffsetted = yi + _offsetY;

//							byte blockId = (byte) anvilChunk.GetBlockId(new Coordinates3D(xi, yoffsetted, zi));
//							TileEntity blockEntity = anvilChunk.GetTileEntity(new Coordinates3D(anvilCoord.X*16 + xi, yoffsetted, anvilCoord.Z*16 + zi));

//							// Anvil to PE friendly converstion
//							if (blockId == 125) blockId = 5;
//							else if (blockId == 126) blockId = 158;
//							else if (blockId == 75) blockId = 50;
//							else if (blockId == 76) blockId = 50;
//							else if (blockId == 123) blockId = 89;
//							else if (blockId == 124) blockId = 89;
//							else if (_ignore.BinarySearch(blockId) >= 0) blockId = 0;
//							else if (_gaps.BinarySearch(blockId) >= 0)
//							{
//								Debug.WriteLine("Missing material: " + blockId);
//								blockId = 133;
//							}

//							if (blockId > 255) blockId = 41;

//							if (yi == 127 && blockId != 0) blockId = 30;
//							if (yi == 0 && (blockId == 8 || blockId == 9 || blockId == 0)) blockId = 7;

//							chunk.SetBlock(xi, yi, zi, blockId);
//							chunk.SetBlocklight(xi, yi, zi, anvilChunk.GetBlockLight(new Coordinates3D(xi, yoffsetted, zi)));
//							chunk.SetMetadata(xi, yi, zi, anvilChunk.GetMetadata(new Coordinates3D(xi, yoffsetted, zi)));
//							chunk.SetSkylight(xi, yi, zi, anvilChunk.GetSkyLight(new Coordinates3D(xi, yoffsetted, zi)));

//							if (blockEntity != null)
//							{
//								chunk.SetBlockEntity(new BlockCoordinates(xi, yi, zi), null);
//							}
//						}
//					}
//				}

//				Log.DebugFormat("Chunk from region in: {0} ms", stopwatch.ElapsedMilliseconds);

//				for (int i = 0; i < chunk.skylight.Length; i++)
//					chunk.skylight[i] = 0xff;

//				for (int i = 0; i < chunk.biomeColor.Length; i++)
//					chunk.biomeColor[i] = 8761930;

//				_chunkCache.Add(chunkCoordinates, chunk);

//				return chunk;
//			}
//		}

//		public Vector3 GetSpawnPoint()
//		{
//			var spawnPoint = new Vector3(_level.Spawn.X, _level.Spawn.Y, _level.Spawn.Z);
//			;
//			spawnPoint.Y += 2; // Compensate for point being at head
//			if (spawnPoint.Y > 127) spawnPoint.Y = 127;
//			return spawnPoint;
//		}

//		public void SaveChunks()
//		{
//		}
//	}
//}