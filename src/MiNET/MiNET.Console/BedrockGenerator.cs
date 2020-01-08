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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Console
{
	public class BedrockGenerator : IWorldGenerator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockGenerator));

		private MiNetClient _client;
		private Process _bedrock;

		public void Initialize()
		{
			using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				socket.Bind(new IPEndPoint(IPAddress.Any, 19132));

				var startInfo = new ProcessStartInfo(@"C:\Development\Other\bedrock-server-1.14.1.4\bedrock_server.exe");
				startInfo.CreateNoWindow = false;
				startInfo.WindowStyle = ProcessWindowStyle.Normal;
				startInfo.UseShellExecute = false;

				_bedrock = Process.Start(startInfo);
				_client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.4"), 19162), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
				_client.MessageDispatcher = new McpeClientMessageDispatcher(new ChunkGeneratorHandler(_client));
				_client.StartClient();

				if (_client.ServerEndpoint != null)
				{
					while (!_client.HaveServer)
					{
						_client.SendUnconnectedPing();
						Thread.Sleep(100);
					}
				}
			}

			Log.Info("Found server, waiting for spawn");


			Task.Run(BotHelpers.DoWaitForSpawn(_client)).Wait();
			Log.Info("Spawned on bedrock server");
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			var sw = Stopwatch.StartNew();
			var playerCoords = (BlockCoordinates) chunkCoordinates;

			var movePlayerPacket = McpeMovePlayer.CreateObject();
			movePlayerPacket.runtimeEntityId = _client.EntityId;
			movePlayerPacket.x = playerCoords.X;
			movePlayerPacket.y = 255;
			movePlayerPacket.z = playerCoords.Z;
			_client.SendPacket(movePlayerPacket);

			int count = 0;
			ChunkColumn chunk = null;
			while (count++ < 100 && !_client._chunks.TryGetValue(new Tuple<int, int>(chunkCoordinates.X, chunkCoordinates.Z), out chunk))
			{
				Thread.Sleep(50);
			}

			if (chunk == null)
			{
				Log.Warn($"Failed to locate chunk {chunkCoordinates}. Tried {count} times");
			}
			else
			{
				Log.Debug($"Successful return of chunk {chunkCoordinates} in {sw.ElapsedMilliseconds}ms. Tried {count} times");
			}

			return chunk;
		}
	}
}