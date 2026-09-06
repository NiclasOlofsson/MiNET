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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Vectors;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace MiNET.AgentClient
{
	/// <summary>
	///     A client the agent drives from stdin, one command per line, one result line per command.
	///     It joins like any client, keeps the world the wire delivers, and draws it on request.
	///     Environment: MINET_TARGET host:port (default 127.0.0.1:19132), MINET_USERNAME,
	///     MINET_RADIUS, MINET_BLOCKS_JSON (default: the extraction in the repo).
	/// </summary>
	public static class Program
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

		private static float _yaw;
		private static float _pitch;
		private static Vector3? _eye;
		private static AgentMessageHandler _handler;
		private static int _renderIndex;
		private static long _tick;

		public static int Main(string[] args)
		{
			string baseDirectory = AppContext.BaseDirectory;
			var logConfig = new FileInfo(Path.Combine(baseDirectory, "log4net.xml"));
			XmlConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()), logConfig);
			XmlConfigurator.Configure(LogManager.GetRepository(typeof(MiNetServer).Assembly), logConfig);
			XmlConfigurator.Configure(LogManager.GetRepository(typeof(MiNetClient).Assembly), logConfig);

			string repoRoot = FindRepoRoot(baseDirectory);
			string blocksJson = Environment.GetEnvironmentVariable("MINET_BLOCKS_JSON")
								?? Path.Combine(repoRoot, "src", "MiNET", "MiNET.BdsExtract", "Data", "blocks.json");
			string renderDir = Path.Combine(repoRoot, "temp_auto", "render");
			Directory.CreateDirectory(renderDir);

			var watch = Stopwatch.StartNew();
			BlockColors colors = BlockColors.Load(blocksJson);
			Console.WriteLine($"colors {colors.NamedCount} blocks from {blocksJson} in {watch.ElapsedMilliseconds}ms");

			string username = Environment.GetEnvironmentVariable("MINET_USERNAME") ?? "Agent";
			IPEndPoint target = ParseTarget(Environment.GetEnvironmentVariable("MINET_TARGET"));

			var client = new MiNetClient(target, username);
			if (int.TryParse(Environment.GetEnvironmentVariable("MINET_RADIUS"), out int radius) && radius > 0) client.ChunkRadius = radius;
			var handler = new AgentMessageHandler(client);
			_handler = handler;
			client.MessageHandler = handler;

			// MINET_XBL=signaling puts the Xbox account in the offer's identity assertion while the
			// login stays offline under the bot's name; BDS answers an offer without it with
			// MultiplayerDisabled (37). MINET_XBL=1 logs in as the account itself.
			string xblMode = Environment.GetEnvironmentVariable("MINET_XBL");
			if (xblMode == "1" || xblMode == "signaling")
			{
				var authentication = new XboxAuthentication();
				authentication.DeviceCodeRequired += (uri, code) => Console.WriteLine($"xbl sign in at {uri} with code {code}");
				client.XboxIdentity = authentication.AuthenticateAsync().GetAwaiter().GetResult();
				client.SignalingAuthOnly = xblMode == "signaling";
				Console.WriteLine($"xbl {client.XboxIdentity.DisplayName}{(client.SignalingAuthOnly ? " (signaling only)" : "")}");
			}

			Console.WriteLine($"connecting {target} as {username}");
			if (!client.ConnectNetherNetAsync().GetAwaiter().GetResult())
			{
				Console.WriteLine("err connect failed");
				return 1;
			}

			if (!client.PlayerStatusChangedWaitHandle.WaitOne(30000))
			{
				Console.WriteLine("err spawn timed out");
				client.StopClient();
				return 1;
			}
			client.HasSpawned = true;

			var world = new ClientWorld(client);
			handler.World = world;
			var renderer = new CartoonRenderer(world, colors);

			// A real client sends PlayerAuthInput every tick; a session that sends nothing is swept
			// by the server's InactivityTimeout. One idle input a second keeps the session alive.
			var heartbeat = new Timer(_ =>
			{
				PlayerLocation at = client.CurrentLocation;
				if (at == null || !client.IsConnected) return;
				var input = McpePlayerAuthInput.CreateObject();
				input.playerRotation = new System.Numerics.Vector2(_pitch, _yaw);
				input.playerHeadRotation = _yaw;
				input.position = new Vector3(at.X, at.Y, at.Z);
				input.moveVector = System.Numerics.Vector2.Zero;
				input.inputData = 0;
				input.inputMode = McpePlayerAuthInput.InputMode.Mouse;
				input.playMode = McpePlayerAuthInput.ClientPlayMode.Normal;
				input.newInteractionModel = McpePlayerAuthInput.NewInteractionModel.Touch;
				input.interactRotation = new System.Numerics.Vector2(_pitch, _yaw);
				input.clientTick = Interlocked.Increment(ref _tick);
				input.posDelta = Vector3.Zero;
				input.analogMoveVector = System.Numerics.Vector2.Zero;
				input.rawMoveVector = System.Numerics.Vector2.Zero;
				input.cameraOrientation = new Vector3(_pitch, _yaw, 0);
				client.SendPacket(input);
			}, null, 1000, 1000);

			WaitForChunks(client);
			PlayerLocation location = client.CurrentLocation;
			_yaw = location.Yaw;
			_pitch = location.Pitch;
			Console.WriteLine($"spawned at {Format(location)} columns {world.ColumnCount}");

			foreach (string raw in ReadCommands(client))
			{
				string line = raw.Trim();
				if (line.Length == 0) continue;
				string[] parts = line.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
				string verb = parts[0].ToLowerInvariant();
				string rest = parts.Length > 1 ? parts[1] : "";

				try
				{
					if (!Execute(verb, rest, client, world, renderer, renderDir)) break;
				}
				catch (Exception e)
				{
					Console.WriteLine($"err {verb}: {e.Message}");
					Log.Error($"Command {line}", e);
				}
			}

			client.StopClient();
			Console.WriteLine("bye");
			return 0;
		}

		/// <summary>
		///     Commands come from stdin, or, with MINET_COMMAND_FILE set, from that file as lines are
		///     appended to it, so one session stays joined across many rounds of work. The file is
		///     read from its end at startup: old commands are not replayed.
		/// </summary>
		private static IEnumerable<string> ReadCommands(MiNetClient client)
		{
			string path = Environment.GetEnvironmentVariable("MINET_COMMAND_FILE");
			if (string.IsNullOrWhiteSpace(path))
			{
				string line;
				while ((line = Console.ReadLine()) != null) yield return line;
				yield break;
			}

			// Two lanes: the main file is the build queue, the priority file (same path plus
			// ".priority") is drained before every main line, so chat and probes never wait
			// behind a batch of thousands of cells.
			var main = new TailedFile(path);
			var priority = new TailedFile(path + ".priority");
			Console.WriteLine($"session reading commands appended to {path}, priority lane {priority.Path}");
			while (client.IsConnected)
			{
				bool any = false;
				foreach (string line in priority.ReadLines()) { any = true; yield return line; }
				foreach (string line in main.ReadLines())
				{
					any = true;
					yield return line;
					foreach (string urgent in priority.ReadLines()) yield return urgent;
				}
				if (!any) Thread.Sleep(100);
			}
		}

		/// <summary>A file read from its end at startup and then followed as lines are appended; a truncation restarts it from the top.</summary>
		private sealed class TailedFile
		{
			public string Path { get; }
			private long _offset;
			private readonly System.Text.StringBuilder _pending = new System.Text.StringBuilder();

			public TailedFile(string path)
			{
				Path = path;
				if (!File.Exists(path)) File.WriteAllText(path, "");
				_offset = new FileInfo(path).Length;
			}

			public IEnumerable<string> ReadLines()
			{
				using (var stream = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
				{
					if (stream.Length < _offset) _offset = 0;
					stream.Position = _offset;
					var buffer = new byte[8192];
					int read;
					while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
					{
						_offset += read;
						_pending.Append(System.Text.Encoding.UTF8.GetString(buffer, 0, read));
					}
				}

				string text = _pending.ToString();
				int newline;
				while ((newline = text.IndexOf('\n')) >= 0)
				{
					string line = text.Substring(0, newline).TrimEnd('\r');
					text = text.Substring(newline + 1);
					_pending.Clear().Append(text);
					if (line.Length > 0) yield return line;
				}
			}
		}

		private static bool Execute(string verb, string rest, MiNetClient client, ClientWorld world, CartoonRenderer renderer, string renderDir)
		{
			switch (verb)
			{
				case "quit":
				case "exit":
					return false;

				case "pos":
					Console.WriteLine($"ok {Format(client.CurrentLocation)} yaw {_yaw} pitch {_pitch} columns {world.ColumnCount}");
					break;

				case "find":
				{
					// find <block> <x1> <y1> <z1> <x2> <y2> <z2>: every cell of that block in the box, capped at 2000.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					string wanted = a[0].StartsWith("minecraft:", StringComparison.Ordinal) ? a[0] : "minecraft:" + a[0];
					int[] v = a.Skip(1).Take(6).Select(int.Parse).ToArray();
					var found = new List<string>();
					for (int x = Math.Min(v[0], v[3]); x <= Math.Max(v[0], v[3]) && found.Count < 2000; x++)
					for (int y = Math.Min(v[1], v[4]); y <= Math.Max(v[1], v[4]) && found.Count < 2000; y++)
					for (int z = Math.Min(v[2], v[5]); z <= Math.Max(v[2], v[5]) && found.Count < 2000; z++)
					{
						int id = world.GetRuntimeId(x, y, z);
						if (id >= 0 && MiNET.Blocks.BlockFactory.GetBlockName(id) == wanted) found.Add($"{x},{y},{z}");
					}
					Console.WriteLine($"ok {found.Count} " + string.Join(" ", found));
					break;
				}

				case "players":
				{
					// players: every other player the server has shown us, with their last position.
					var list = _handler?.Players.Values.Select(p => string.Create(CultureInfo.InvariantCulture, $"{p.Name} {p.Position.X:0.0} {p.Position.Y:0.0} {p.Position.Z:0.0}")).ToList() ?? new List<string>();
					Console.WriteLine(list.Count == 0 ? "ok no players known" : "ok " + string.Join(" | ", list));
					break;
				}

				case "eye":
				{
					// eye <x> <y> <z> | eye off: render from here instead of the player's head.
					if (rest.Trim().Equals("off", StringComparison.OrdinalIgnoreCase)) _eye = null;
					else
					{
						string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
						_eye = new Vector3(Parse(a[0]), Parse(a[1]), Parse(a[2]));
					}
					Console.WriteLine(_eye == null ? "ok eye follows the player" : string.Create(CultureInfo.InvariantCulture, $"ok eye {_eye.Value.X} {_eye.Value.Y} {_eye.Value.Z}"));
					break;
				}

				case "look":
				{
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					_yaw = Parse(a[0]);
					_pitch = a.Length > 1 ? Parse(a[1]) : 0;
					Console.WriteLine($"ok yaw {_yaw} pitch {_pitch}");
					break;
				}

				case "goto":
				{
					// A teleport request to the server: it answers with MovePlayer, which the base
					// handler applies to CurrentLocation, and the chunk window follows.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					SendCommand(client, $"tp @s {a[0]} {a[1]} {a[2]}");
					Thread.Sleep(500);
					WaitForChunks(client);
					Console.WriteLine($"ok {Format(client.CurrentLocation)} columns {world.ColumnCount}");
					break;
				}

				case "cmd":
					SendCommand(client, rest);
					Console.WriteLine("ok sent");
					break;

				case "slot":
				{
					// Select a hotbar slot, the way a client scrolling its hotbar tells the server.
					var equipment = McpeMobEquipment.CreateObject();
					equipment.runtimeEntityId = client.EntityId;
					equipment.item = new MiNET.Items.ItemAir();
					equipment.slot = (byte) int.Parse(rest, CultureInfo.InvariantCulture);
					equipment.selectedSlot = equipment.slot;
					equipment.windowsId = 0;
					client.SendPacket(equipment);
					Console.WriteLine($"ok slot {equipment.slot}");
					break;
				}

				case "say":
					client.SendChat(rest);
					Console.WriteLine("ok said");
					break;

				case "wait":
					Thread.Sleep(int.Parse(rest, CultureInfo.InvariantCulture));
					Console.WriteLine("ok waited");
					break;

				case "chunks":
					WaitForChunks(client);
					Console.WriteLine($"ok columns {world.ColumnCount}");
					break;

				case "stats":
				{
					int complete = 0, sections = 0, withSections = 0;
					foreach (CachedChunkColumn column in client.ChunkCache.Columns)
					{
						if (column.IsComplete) complete++;
						if (column.Sections.Count > 0) withSections++;
						sections += column.Sections.Count;
					}
					// stats [chunkX chunkZ]: the column under the player unless one is named.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					PlayerLocation location = client.CurrentLocation;
					var here = a.Length >= 2
						? new ChunkCoordinates(int.Parse(a[0]), int.Parse(a[1]))
						: new ChunkCoordinates((int) MathF.Floor(location.X) >> 4, (int) MathF.Floor(location.Z) >> 4);
					string keys = client.ChunkCache.TryGetColumn(here, out CachedChunkColumn mine)
						? $"{mine.Delivery} outstanding {mine.Outstanding} awaiting {mine.AwaitingRequest} sections [{string.Join(",", mine.Sections.Keys.OrderBy(k => k))}] sizes [{string.Join(",", mine.Sections.OrderBy(s => s.Key).Select(s => s.Value?.Length ?? -1))}]"
						: "(no column)";
					Console.WriteLine($"ok columns {client.ChunkCache.Columns.Count} complete {complete} withSections {withSections} sections {sections} blobs {client.ChunkCache.Blobs.PayloadCount}/{client.ChunkCache.Blobs.Count} legacy {client.Chunks.Count} hashes {client.BlockNetworkIdsAreHashes} here {here.X},{here.Z} {keys}");
					break;
				}

				case "count":
				{
					// count <x1> <z1> <x2> <z2> <y>: what stands on one layer of a rectangle, by name.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					int x1 = Math.Min(int.Parse(a[0]), int.Parse(a[2])), x2 = Math.Max(int.Parse(a[0]), int.Parse(a[2]));
					int z1 = Math.Min(int.Parse(a[1]), int.Parse(a[3])), z2 = Math.Max(int.Parse(a[1]), int.Parse(a[3]));
					int y = int.Parse(a[4]);
					var counts = new Dictionary<string, int>();
					for (int x = x1; x <= x2; x++)
					for (int z = z1; z <= z2; z++)
					{
						int id = world.GetRuntimeId(x, y, z);
						string name = id < 0 ? "(no section)" : MiNET.Blocks.BlockFactory.GetBlockName(id) ?? id.ToString();
						counts[name] = counts.GetValueOrDefault(name) + 1;
					}
					Console.WriteLine("ok " + string.Join(", ", counts.OrderByDescending(p => p.Value).Select(p => $"{p.Key} {p.Value}")));
					break;
				}

				case "block":
				{
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					int id = world.GetRuntimeId(int.Parse(a[0]), int.Parse(a[1]), int.Parse(a[2]));
					Console.WriteLine($"ok {id} {(id >= 0 ? MiNET.Blocks.BlockFactory.GetBlockName(id) : "(no section)")}");
					break;
				}

				case "ray":
				{
					// ray <yaw> <pitch> [max]: narrate one ray from the eye, cell by cell.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					PlayerLocation location = client.CurrentLocation;
					Vector3 eye = _eye ?? new Vector3(location.X, location.Y + 1.62f, location.Z);
					float max = a.Length > 2 ? Parse(a[2]) : 160;
					Console.Write(renderer.TraceDebug(eye, Parse(a[0]), Parse(a[1]), max));
					Console.WriteLine("ok ray done");
					break;
				}

				case "render":
				{
					// render [width height] [fov] [path]
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					int width = 1280, height = 960;
					float fov = 70;
					string path = null;
					int i = 0;
					if (a.Length >= 2 && int.TryParse(a[0], out width) && int.TryParse(a[1], out height)) i = 2;
					else { width = 1280; height = 960; }
					if (a.Length > i && float.TryParse(a[i], NumberStyles.Float, CultureInfo.InvariantCulture, out fov)) i++;
					else fov = 70;
					if (a.Length > i) path = a[i];

					PlayerLocation location = client.CurrentLocation;
					Vector3 eye = _eye ?? new Vector3(location.X, location.Y + 1.62f, location.Z);
					var watch = Stopwatch.StartNew();
					using Image<Rgba32> image = renderer.RenderPerspective(eye, _yaw, _pitch, width, height, fov);
					path = Save(image, path, renderDir, "view");
					Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"ok {path} eye {eye.X:0.0} {eye.Y:0.0} {eye.Z:0.0} yaw {_yaw} pitch {_pitch} {watch.ElapsedMilliseconds}ms"));
					break;
				}

				case "frame":
				{
					// frame <x> <y> <z> <distance> <yaw> <pitch>: put the eye on a sphere around the
					// target and look at it. Yaw and pitch are the view direction, Bedrock convention.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					var target = new Vector3(Parse(a[0]), Parse(a[1]), Parse(a[2]));
					float distance = Parse(a[3]);
					_yaw = Parse(a[4]);
					_pitch = Parse(a[5]);
					float yaw = _yaw * MathF.PI / 180f, pitch = _pitch * MathF.PI / 180f;
					var forward = new Vector3(-MathF.Sin(yaw) * MathF.Cos(pitch), -MathF.Sin(pitch), MathF.Cos(yaw) * MathF.Cos(pitch));
					_eye = target - forward * distance;
					Console.WriteLine(string.Create(CultureInfo.InvariantCulture, $"ok eye {_eye.Value.X:0.0} {_eye.Value.Y:0.0} {_eye.Value.Z:0.0} yaw {_yaw} pitch {_pitch}"));
					break;
				}

				case "elev":
				{
					// elev <n|s|e|w> <x> <y> <z> <halfSize> [pixelsPerBlock] [path]: orthographic
					// side view looking toward that compass direction, centred on the point.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					Vector3 dir = a[0].ToLowerInvariant() switch
					{
						"n" => -Vector3.UnitZ,
						"s" => Vector3.UnitZ,
						"e" => Vector3.UnitX,
						_ => -Vector3.UnitX
					};
					var center = new Vector3(Parse(a[1]), Parse(a[2]), Parse(a[3]));
					int half = int.Parse(a[4]);
					int scale = a.Length > 5 ? int.Parse(a[5]) : 8;
					string path = a.Length > 6 ? a[6] : null;
					var watch = Stopwatch.StartNew();
					using Image<Rgba32> image = renderer.RenderElevation(center, dir, half, scale);
					path = Save(image, path, renderDir, "elev");
					Console.WriteLine($"ok {path} looking {a[0]} at {a[1]},{a[2]},{a[3]} half {half} {watch.ElapsedMilliseconds}ms");
					break;
				}

				case "slice":
				{
					// slice <y> <x> <z> <halfSize> [pixelsPerBlock] [path]: one layer from above.
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					int y = int.Parse(a[0]), cx = int.Parse(a[1]), cz = int.Parse(a[2]), half = int.Parse(a[3]);
					int scale = a.Length > 4 ? int.Parse(a[4]) : 8;
					string path = a.Length > 5 ? a[5] : null;
					var watch = Stopwatch.StartNew();
					using Image<Rgba32> image = renderer.RenderSlice(cx, y, cz, half, scale);
					path = Save(image, path, renderDir, "slice");
					Console.WriteLine($"ok {path} layer {y} center {cx},{cz} half {half} {watch.ElapsedMilliseconds}ms");
					break;
				}

				case "top":
				{
					// top <x> <z> <halfSize> [pixelsPerBlock] [path]
					string[] a = rest.Split(' ', StringSplitOptions.RemoveEmptyEntries);
					int cx = int.Parse(a[0]), cz = int.Parse(a[1]), half = int.Parse(a[2]);
					int scale = a.Length > 3 ? int.Parse(a[3]) : 8;
					string path = a.Length > 4 ? a[4] : null;
					var watch = Stopwatch.StartNew();
					using Image<Rgba32> image = renderer.RenderTop(cx, cz, half, scale);
					path = Save(image, path, renderDir, "top");
					Console.WriteLine($"ok {path} center {cx},{cz} half {half} {watch.ElapsedMilliseconds}ms");
					break;
				}

				default:
					Console.WriteLine($"err unknown command {verb}");
					break;
			}

			return true;
		}

		private static void SendCommand(MiNetClient client, string command)
		{
			var request = McpeCommandRequest.CreateObject();
			request.command = command.StartsWith('/') ? command : "/" + command;
			request.origin = new CommandOriginData(CommandOriginType.Player, new UUID(Guid.NewGuid().ToString()), string.Empty, 0);
			request.version = "latest";
			client.SendPacket(request);
		}

		/// <summary>Waits for the column stream to go quiet: every announced blob held and no new column for a moment.</summary>
		private static void WaitForChunks(MiNetClient client)
		{
			int columns = 0, quiet = 0;
			for (int i = 0; i < 60 && client.IsConnected; i++)
			{
				Thread.Sleep(250);
				int next = client.ChunkCache.Columns.Count;
				bool settled = next > 0 && next == columns && client.ChunkCache.Blobs.PayloadCount == client.ChunkCache.Blobs.Count;
				columns = next;
				if (settled && ++quiet >= 3) break;
				if (!settled) quiet = 0;
			}
		}

		private static string Save(Image<Rgba32> image, string path, string renderDir, string kind)
		{
			if (string.IsNullOrEmpty(path)) path = Path.Combine(renderDir, $"{kind}-{++_renderIndex:000}.png");
			else if (!Path.IsPathRooted(path)) path = Path.Combine(renderDir, path);
			image.SaveAsPng(path);
			return path;
		}

		private static string Format(PlayerLocation location)
		{
			return location == null ? "(nowhere)" : string.Create(CultureInfo.InvariantCulture, $"{location.X:0.0} {location.Y:0.0} {location.Z:0.0}");
		}

		private static float Parse(string value) => float.Parse(value, NumberStyles.Float, CultureInfo.InvariantCulture);

		private static IPEndPoint ParseTarget(string target)
		{
			if (string.IsNullOrWhiteSpace(target)) return new IPEndPoint(IPAddress.Loopback, 19132);
			string[] parts = target.Split(':');
			IPAddress address = IPAddress.TryParse(parts[0], out IPAddress parsed) ? parsed : Dns.GetHostAddresses(parts[0])[0];
			return new IPEndPoint(address, parts.Length > 1 ? int.Parse(parts[1]) : 19132);
		}

		private static string FindRepoRoot(string start)
		{
			var directory = new DirectoryInfo(start);
			while (directory != null)
			{
				if (File.Exists(Path.Combine(directory.FullName, "CLAUDE.md")) && Directory.Exists(Path.Combine(directory.FullName, "src", "MiNET"))) return directory.FullName;
				directory = directory.Parent;
			}
			return Directory.GetCurrentDirectory();
		}
	}
}