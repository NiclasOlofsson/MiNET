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
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Console
{
	public class BedrockGenerator : IWorldGenerator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockGenerator));

		private MiNetClient _client;
		private Process _bedrock;

		public BedrockGenerator()
		{
		}

		public void Initialize(IWorldProvider worldProvider)
		{
			Process blocker;
			{
				// Block port!
				var startInfo = new ProcessStartInfo("MiNET.Console.exe", "listener");
				startInfo.CreateNoWindow = false;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
				startInfo.UseShellExecute = false;
				blocker = Process.Start(startInfo);
			}
			{
				var startInfo = new ProcessStartInfo(Path.Combine(Config.GetProperty("BDSPath", null), Config.GetProperty("BDSExe", null)));
				startInfo.WorkingDirectory = Config.GetProperty("BDSPath", null);
				startInfo.CreateNoWindow = false;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
				startInfo.UseShellExecute = false;
				startInfo.RedirectStandardInput = true;

				_bedrock = Process.Start(startInfo);

				_client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.10.178"), 19162), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
				_client.MessageHandler = new ChunkGeneratorHandler(_client, worldProvider);
				//_client.UseBlobCache = true;
				_client.StartClient();

				if (_client.ServerEndPoint != null)
				{
					while (!_client.FoundServer)
					{
						_client.SendUnconnectedPing();
						Thread.Sleep(100);
					}
				}
			}

			Log.Info("Found server, waiting for spawn");


			Task.Run(BotHelpers.DoWaitForSpawn(_client)).Wait();
			Log.Info("Spawned on bedrock server");

			blocker?.Kill(); // no need to block further once we have spawned our bot.

			// Shutdown hook. Must use to flush in memory log of LevelDB.
			AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
			{
				Log.Warn("Closing bedrock dedicated server (BDS)");
				_bedrock.StandardInput.WriteLine("stop");
				_bedrock.WaitForExit(1000);
				_bedrock.Kill();
			};

		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			var sw = Stopwatch.StartNew();
			var playerCoords = (BlockCoordinates) chunkCoordinates;

			if (_client.Chunks.TryGetValue(chunkCoordinates, out ChunkColumn chunk))
			{
				Log.Debug($"Successful return of chunk {chunkCoordinates} from cache.");
				_client.Chunks.TryRemove(chunkCoordinates, out _);
				return chunk;
			}

			_client.Chunks.TryAdd(chunkCoordinates, null); // register to receive chunks. 

			var movePlayerPacket = McpeMovePlayer.CreateObject();
			movePlayerPacket.runtimeEntityId = _client.EntityId;
			movePlayerPacket.x = playerCoords.X;
			movePlayerPacket.y = 255;
			movePlayerPacket.z = playerCoords.Z;
			_client.SendPacket(movePlayerPacket);

			while (sw.ElapsedMilliseconds < 2000)
			{
				_client.Chunks.TryGetValue(chunkCoordinates, out chunk);
				if (chunk != null) break;
				Thread.Sleep(50);
			}

			if (chunk == null)
			{
				Log.Warn($"Failed to locate chunk {chunkCoordinates}. Tried {sw.ElapsedMilliseconds}ms");
			}
			else
			{
				Log.Debug($"Successful return of chunk {chunkCoordinates} in {sw.ElapsedMilliseconds}ms. Have {_client.Chunks.Count} chunks in memory now.");
			}

			return chunk;
		}
	}
}