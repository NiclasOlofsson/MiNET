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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Blocks;
using MiNET.Camera;
using MiNET.Net;
using MiNET.Net.NetherNet;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Parking
{
	/// <summary>
	///     The world of the parking server: nothing but the MiNET mark, drawn as a block big enough
	///     to hold a spectator at its centre, who is held there and cannot reach any of it.
	/// </summary>
	[Plugin(PluginName = "Parking", Description = "Holds players while the development server restarts", PluginVersion = "1.0", Author = "MiNET Team")]
	public class ParkingPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ParkingPlugin));

		/// <summary>Seconds since each player's lap started, so the spline can be re-sent when it ends.</summary>
		private readonly ConcurrentDictionary<Player, long> _orbitSeconds = new ConcurrentDictionary<Player, long>();

		/// <summary>Seconds left before an arrival is sent back on its own, for doors that asked for it.</summary>
		private readonly ConcurrentDictionary<Player, long> _autoReturnSeconds = new ConcurrentDictionary<Player, long>();

		/// <summary>
		///     This plugin's clock. The level does not tick, so nothing else calls us: everything
		///     parking needs is a spline re-send and a countdown, and neither wants 20Hz per player.
		/// </summary>
		private Timer _pluginTimer;

		private DoorRegistry _doors;
		private TokenRegistry _tokens;

		/// <summary>
		///     Midnight on day 4. The moon phase is not a setting, it is the day count: phase is
		///     (time / 24000) % 8, and 4 is the new moon, which draws nothing. Day 0 is a full moon.
		/// </summary>
		private const long NewMoonMidnight = 4 * 24000 + 18000;

		/// <summary>How far a player may drift before being put back.</summary>
		private const double DriftTolerance = 0.5;

		/// <summary>
		///     Creative and free to fly, for looking at the build from outside it. Off is what parking
		///     is for: spectator, held on the anchor, camera on the orbit.
		/// </summary>
		private const bool FreeRoam = false;

		/// <summary>How far out the camera flies. Comfortably clear of the cube's half diagonal.</summary>
		private const float OrbitRadius = 46f;

		/// <summary>
		///     How far the camera rises and falls over one lap. It rises and falls once, in step with
		///     going round, which tilts the ring out of the horizontal. Twice per lap makes a figure
		///     of eight instead, whose segments differ wildly in length and so run at uneven speed.
		/// </summary>
		private const float OrbitRise = 22f;

		/// <summary>Seconds for one lap. The spline is re-sent when it runs out.</summary>
		private const float OrbitSeconds = 48f;

		/// <summary>Control points around the lap. Catmull-Rom needs at least four.</summary>
		private const int OrbitPoints = 24;

		/// <summary>
		///     Bottom of the cube, and it sits ON zero deliberately: The End's floor is fixed there,
		///     and anything below it arrives but is never drawn.
		/// </summary>
		private const int CubeBaseY = 0;

		/// <summary>Edge of the cube, in blocks. One block, drawn big enough to stand inside.</summary>
		private const int CubeSize = 32;

		/// <summary>Thickness of the drawn edge. One block reads as a hairline at this distance.</summary>
		private const int CubeStroke = 2;

		/// <summary>Dead centre of the cube, which is also what the camera looks at.</summary>
		private static readonly PlayerLocation Anchor = new PlayerLocation(0.5, CubeBaseY + CubeSize / 2, 0.5);

		/// <summary>
		///     The hash grid, sized so it lands evenly on a face. The interior of a face is 28 across,
		///     which is one block of margin, three glyphs of eight with a block between them, and one
		///     block of margin again.
		/// </summary>
		private const int HashMargin = 1;

		private const int HashGlyph = 8;
		private const int HashCell = HashGlyph + 1;

		protected override void OnEnable()
		{
			MiNetServer server = Context.Server;

			string range = Config.GetProperty("Parking.PortRange", "19500-19600");
			string[] bounds = range.Split('-');
			int first = int.Parse(bounds[0]);
			int last = bounds.Length > 1 ? int.Parse(bounds[1]) : first;

			_doors = new DoorRegistry(Path.Combine(AppContext.BaseDirectory, "doors.json"), first, last, Config.GetProperty("Parking.MaxDoorsPerUser", 10));
			_tokens = new TokenRegistry(Path.Combine(AppContext.BaseDirectory, "tokens.json"));

			_suppressed = new HashSet<string>(
				Config.GetProperty("Parking.Suppress", "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
				StringComparer.OrdinalIgnoreCase);

			if (_suppressed.Count > 0) Log.Info($"Suppressing {_suppressed.Count} packet types: {string.Join(", ", _suppressed)}");

			_suppressedIn = new HashSet<string>(
				Config.GetProperty("Parking.SuppressIn", "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries),
				StringComparer.OrdinalIgnoreCase);

			if (_suppressedIn.Count > 0) Log.Info($"Dropping {_suppressedIn.Count} incoming packet types: {string.Join(", ", _suppressedIn)}");

			_measureTraffic = Config.GetProperty("Parking.MeasureTraffic", false);
			if (_measureTraffic) Log.Info("Traffic tally is ON: per-packet counters, not for a measurement run");

			_pluginTimer = new Timer(OnPluginTick, null, 1000, 1000);

			// Not in OnEnable and not on LevelCreated: both run before the transport exists, so
			// there would be nothing to open the ports on.
			server.ServerStarted += (_, _) => _doors.Restore(server.NetherNetListener);

			server.LevelManager.LevelCreated += (sender, args) =>
			{
				Level level = args.Level;

				// The End: no sun, moon or stars. Its vertical range is fixed at 0..255 and cannot be
				// declared otherwise, so everything here is built at or above zero.
				level.Dimension = Dimension.TheEnd;

				level.SpawnPoint = (PlayerLocation) Anchor.Clone();
				level.WorldTime = NewMoonMidnight;
				level.DoDaylightcycle = false;
				level.RainLevel = 0;
				level.LightningLevel = 0;
				level.DoWeathercycle = false;
				level.AllowBuild = false;
				level.AllowBreak = false;

				BuildCube(level);
			};

			server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				args.Player.PlayerJoin += OnPlayerJoin;
				args.Player.PlayerLeave += (_, left) =>
				{
					_orbitSeconds.TryRemove(left.Player, out long _);
					_autoReturnSeconds.TryRemove(left.Player, out long _);
				};
			};
		}

		public override void OnDisable()
		{
			_pluginTimer?.Dispose();
			_pluginTimer = null;
		}

		/// <summary>
		///     The icon: one block drawn at 32 blocks a side, as its twelve edges and its hashes and
		///     nothing else. Axis aligned, not on the isometric tilt the flat logo needs to suggest
		///     depth. The parked player stands on its floor and the mark surrounds them; hollow, so
		///     they see every face of it from inside.
		/// </summary>
		private static void BuildCube(Level level)
		{
			int minX = -CubeSize / 2;
			int minZ = -CubeSize / 2;

			int minY = CubeBaseY;

			for (int x = 0; x < CubeSize; x++)
			{
				for (int y = 0; y < CubeSize; y++)
				{
					for (int z = 0; z < CubeSize; z++)
					{
						// An edge is where two of the three axes are both at a face: one axis alone
						// is a face, three is a corner, two is the line between them.
						int atFace = (IsEdge(x) ? 1 : 0) + (IsEdge(y) ? 1 : 0) + (IsEdge(z) ? 1 : 0);
						if (atFace < 2) continue;

						var block = new PurpleConcrete {Coordinates = new BlockCoordinates(minX + x, minY + y, minZ + z)};
						level.SetBlock(block, false, false, true);
					}
				}
			}

			BuildHashes(level, minX, minY, minZ);

			Log.Info($"MiNET cube built {CubeSize} a side, y={minY} to {minY + CubeSize - 1}, edges and hashes.");
		}

		private static bool IsEdge(int i) => i < CubeStroke || i >= CubeSize - CubeStroke;

		/// <summary>
		///     Whether this spot on a face is on a hash: inside one of the glyph boxes, and on a bar
		///     of it in either direction.
		/// </summary>
		private static bool IsHash(int u, int v)
		{
			return InGlyph(u, out int a) && InGlyph(v, out int b) && (IsBar(a) || IsBar(b));
		}

		/// <summary>Position within this glyph, or false for the margin and the gaps between glyphs.</summary>
		private static bool InGlyph(int i, out int within)
		{
			within = 0;
			int offset = i - HashMargin;
			if (offset < 0) return false;

			within = offset % HashCell;
			return within < HashGlyph;
		}

		/// <summary>
		///     Bars one thick, on the outer edge of where the thick pair used to be, so thinning them
		///     opened the square in the middle rather than closing it. The arms still run to the
		///     glyph's edges, so the mark keeps its footprint.
		/// </summary>
		private static bool IsBar(int within) => within == 1 || within == 6;

		/// <summary>
		///     The hashes the logo tiles across its faces. The top is turf, so it is covered; the
		///     sides get a single row hung from the top edge, which is the overhang a grass block has
		///     and all the side of one ever shows. One block thick, so the cube stays see-through.
		/// </summary>
		private static void BuildHashes(Level level, int minX, int minY, int minZ)
		{
			int lo = CubeStroke;
			int hi = CubeSize - CubeStroke - 1;
			int far = CubeSize - 1;

			for (int a = lo; a <= hi; a++)
			{
				for (int b = lo; b <= hi; b++)
				{
					if (!IsHash(a - lo, b - lo)) continue;

					Place(level, minX + a, minY + far, minZ + b);
				}
			}

			// Depth measured DOWN from the top edge, so the row hangs from it rather than sitting
			// wherever the tiling happens to land.
			for (int across = lo; across <= hi; across++)
			{
				for (int down = 0; down < HashCell; down++)
				{
					if (!IsHash(across - lo, down)) continue;

					int y = minY + hi - down;
					Place(level, minX + across, y, minZ);
					Place(level, minX + across, y, minZ + far);
					Place(level, minX, y, minZ + across);
					Place(level, minX + far, y, minZ + across);
				}
			}
		}

		private static void Place(Level level, int x, int y, int z)
		{
			var block = new PurpleConcrete {Coordinates = new BlockCoordinates(x, y, z)};
			level.SetBlock(block, false, false, true);
		}

		private void OnPlayerJoin(object sender, PlayerEventArgs e)
		{
			Player player = e.Player;

			// No SetGameMode here. The old note said the burst put the mode back to the level's own,
			// which was true and is now the point: GameMode in the config makes the level's own mode
			// the one we want, StartGame carries it as gameType Default, and correcting it afterwards
			// only produces an UpdatePlayerGameType and two AdventureSettings saying what was already
			// said. FreeRoam still needs the override, since the level is not creative.
			if (FreeRoam)
			{
				player.SetGameMode(GameMode.Creative);
				player.SendAdventureSettings();
			}

			int port = SignalingPortOf(player);
			Door door = _doors.ByPort(port);

			// Checked on arrival rather than at signaling, because a name only exists once the login
			// has run and signaling happens well before that. So a refused visitor does reach us;
			// they just do not stay.
			if (door != null && !door.Admits(player.Username))
			{
				Log.Info($"{player.Username} refused at door {port}, owned by {door.OwnerName}.");
				player.Disconnect($"You are not on the list for this door.");
				return;
			}

			// ServerAddress is the login's own record of what the client was told to connect to, a
			// different layer from the signaling Host header entirely. Logged because it decides
			// whether a name can survive a transfer at all.
			Log.Info($"{player.Username} parked as {player.GameMode}, arrived on port {port}, login ServerAddress '{player.PlayerInfo?.ServerAddress}', returns to {DestinationFor(player)}.");

			if (door != null && door.AutoSeconds > 0) _autoReturnSeconds[player] = door.AutoSeconds;

			// No SetPosition here: StartGame already carries SpawnPosition, which is the anchor, so
			// placing the player again only sends a MovePlayer saying what they were just told. The
			// player is held by NoAi and the camera, so nothing has to put them back.

			if (FreeRoam) return;

			player.SetNoAi(true);

			// Registers them with the plugin's clock as well as starting the lap: the timer walks
			// this map, so a player absent from it is a player nothing ever comes back for.
			_orbitSeconds[player] = 0;
			// Parking.Orbit=false parks without the camera lap. A bisect switch: the camera
			// instructions are the last packets a fresh arrival receives.
			if (Config.GetProperty("Parking.Orbit", true)) StartOrbit(player);
		}

		/// <summary>
		///     Sends the player back where they came from, for when the development server is up
		///     again and nobody is driving the remote console. Where that is comes from the route
		///     table, keyed by the host the client dialled to get here.
		/// </summary>
		[Command(Name = "back", Description = "Go back to the server you came from")]
		public void BackCommand(Player player)
		{
			SendTo(player, DestinationFor(player));
		}

		/// <summary>
		///     Where this player goes when they leave. The door they arrived through decides it, and
		///     the front entrance is not a door: anyone who walked in the front is a developer on
		///     their own machine, and the only thing we can honestly offer them is their own machine.
		/// </summary>
		private string DestinationFor(Player player)
		{
			Door door = _doors.ByPort(SignalingPortOf(player));
			return door != null ? $"{door.Address}:{door.ReturnPort}" : Config.GetProperty("Parking.DefaultBack", "127.0.0.1:19132");
		}

		/// <summary>
		///     Sends players back, over HTTP, so whatever put them here can take them back without a
		///     console on this machine.
		///     <para>
		///         <c>POST /transfer/{port}/{player}</c> with <c>Authorization: Bearer</c> carrying
		///         a key from <c>/token</c>. Through a door the caller owns, a name moves one player
		///         and <c>*</c> moves everyone who came through it; on the front entrance the key
		///         moves exactly its own name. The destination is never the caller's to choose: a
		///         door's arrivals go where the door says, front arrivals go to
		///         <c>Parking.DefaultBack</c>.
		///     </para>
		///     <para>
		///         The key is the credential, issued in world, which is already an authenticated
		///         act, and it doubles as the scope: the token record says which doors are the
		///         caller's and which one name is theirs to move.
		///     </para>
		/// </summary>
		[HttpHandler("GET", "/transfer/{port}/{player}")]
		[HttpHandler("POST", "/transfer/{port}/{player}")]
		public HttpResponse TransferRequest(HttpRequest request)
		{
			if (!int.TryParse(request.RouteValues["port"], out int port)) return HttpResponse.Text("Port must be a number", 400);

			string name = request.RouteValues["player"];

			// The key from /token is the caller. Without one the API answers nobody: the token
			// record carries both the scope (which doors are theirs) and the one name they may
			// move on the front entrance, so authentication and authorization are the same lookup.
			// The header is the proper carrier; ?key= exists so the whole call fits in a clickable
			// URL, at the price every query credential pays (history, logs, referrers). At worst a
			// leaked key transfers its owner's players to where they were going anyway.
			string bearer = request.Header("Authorization");
			string key = bearer != null && bearer.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? bearer.Substring(7).Trim() : QueryKey(request.Query);
			AccessToken token = _tokens.Find(key);
			if (token == null) return HttpResponse.Text("A personal access key is required: type /token in the parking lot, then send Authorization: Bearer <key> (or append ?key=<key>)", 401);

			int frontPort = Config.GetProperty("port", 19132);
			Door door = _doors.ByPort(port);
			if (door == null && port != frontPort) return HttpResponse.Text($"No door on port {port}", 404);

			string refusal = token.RefusalFor(door, name);
			if (refusal != null) return HttpResponse.Text(refusal, 403);

			Level level = Context.Server.LevelManager.Levels.FirstOrDefault();
			if (level == null) return HttpResponse.Text("No level", 503);

			// Everyone, not just the spawned: a player still joining is exactly the one a caller
			// racing a transfer means to catch. Scoped to this door, so holding one port never
			// moves somebody who arrived through another.
			Player[] moving = level.GetAllPlayers()
				.Where(player => SignalingPortOf(player) == port)
				.Where(player => name == "*" || string.Equals(player.Username, name, StringComparison.OrdinalIgnoreCase))
				.ToArray();

			string destination = door != null ? $"{door.Address}:{door.ReturnPort}" : Config.GetProperty("Parking.DefaultBack", "127.0.0.1:19132");

			foreach (Player player in moving) SendTo(player, destination);

			Log.Info($"HTTP transfer of {name} through {(door != null ? $"door {port}" : "the front entrance")} to {destination} by {token.OwnerName} from {request.RemoteEndPoint}, moved {moving.Length}.");

			return moving.Length == 0
				? HttpResponse.Text($"Nobody matching {name} came through door {port}", 404)
				: HttpResponse.Text($"Transferred {moving.Length} to {destination}");
		}

		/// <summary>
		///     Registers a way in. Called in world, so the caller is whoever the login says they are
		///     and there is nothing further to authenticate: the port they get back is the credential
		///     for everything else.
		/// </summary>
		[Command(Name = "register", Description = "Get a port whose arrivals go back to the address you name")]
		public void RegisterCommand(Player player, string address, int port = 19132, int timeout = 0)
		{
			NetherNetListener listener = Context.Server.NetherNetListener;
			if (listener == null)
			{
				player.SendMessage("No transport, cannot open a door.");
				return;
			}

			Door door = _doors.Register(OwnerIdOf(player), player.Username, address.Trim(), port, timeout, listener, out string refusal);
			if (door == null)
			{
				player.SendMessage(refusal);
				return;
			}

			string advertised = Config.GetProperty("Parking.PublicAddress", "127.0.0.1");

			player.SendMessage($"Door open on {advertised}:{door.Port}");
			player.SendMessage($"Transfer players there, and /back sends them to {door.Address}:{door.ReturnPort}");
			if (door.AutoSeconds > 0) player.SendMessage($"They also leave on their own after {door.AutoSeconds}s");

			Log.Info($"{player.Username} registered door {door}.");
		}

		/// <summary>
		///     Hands out the caller's personal API key. Asking again is the revocation mechanism:
		///     there is one key per account and the new one takes the old one's place, so a leaked
		///     key dies the moment its owner types this again. The chat message is the only
		///     plaintext copy that will ever exist; the registry keeps hashes.
		/// </summary>
		[Command(Name = "token", Description = "Get your personal API key. Replaces and invalidates your previous one")]
		public void TokenCommand(Player player)
		{
			string key = _tokens.Issue(OwnerIdOf(player), player.Username);

			player.SendMessage("Your personal API key. Your previous one, if any, just stopped working:");
			player.SendMessage(key);
			player.SendMessage("Send it as a header, Authorization: Bearer <key>, or right in the URL:");
			player.SendMessage($"http://{Config.GetProperty("Parking.PublicAddress", "127.0.0.1")}/transfer/<port>/<player>?key=...");
			player.SendMessage($"It moves {player.Username} home from the front entrance, and anyone through doors you own.");

			Log.Info($"{player.Username} issued themselves a fresh API key.");
		}

		[Command(Name = "mydoors", Description = "List the doors you registered")]
		public void MyDoorsCommand(Player player)
		{
			IReadOnlyCollection<Door> doors = _doors.ByOwner(OwnerIdOf(player));
			if (doors.Count == 0)
			{
				player.SendMessage("You have no doors. /register <address> to open one.");
				return;
			}

			string advertised = Config.GetProperty("Parking.PublicAddress", "127.0.0.1");
			foreach (Door door in doors) player.SendMessage($"{advertised}:{door.Port} -> {door.Address}:{door.ReturnPort}" + (door.AutoSeconds > 0 ? $", auto after {door.AutoSeconds}s" : ""));
		}

		/// <summary>
		///     Names, not accounts. An arrival through a door need not be Xbox authenticated, so the
		///     name is the only thing every visitor is guaranteed to have, and it is therefore also
		///     only as trustworthy as the login policy that let them in.
		/// </summary>
		[Command(Name = "allow", Description = "Let a name through one of your doors. The first name makes the door private")]
		public void AllowCommand(Player player, int port, string username)
		{
			Amend(player, port, door =>
			{
				door.Denied.RemoveAll(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase));
				if (!door.Allowed.Any(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase))) door.Allowed.Add(username);

				return door.Allowed.Count == 1
					? $"{username} allowed on {port}. The door is now private, only listed names get in."
					: $"{username} allowed on {port}, {door.Allowed.Count} names listed.";
			});
		}

		[Command(Name = "deny", Description = "Refuse a name at one of your doors")]
		public void DenyCommand(Player player, int port, string username)
		{
			Amend(player, port, door =>
			{
				door.Allowed.RemoveAll(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase));
				if (!door.Denied.Any(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase))) door.Denied.Add(username);

				return $"{username} denied on {port}.";
			});
		}

		[Command(Name = "unlist", Description = "Take a name off both lists on one of your doors")]
		public void UnlistCommand(Player player, int port, string username)
		{
			Amend(player, port, door =>
			{
				int removed = door.Allowed.RemoveAll(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase))
					+ door.Denied.RemoveAll(name => string.Equals(name, username, StringComparison.OrdinalIgnoreCase));

				if (removed == 0) return $"{username} was not listed on {port}.";

				return door.Allowed.Count == 0
					? $"{username} unlisted. Door {port} is open to everyone again."
					: $"{username} unlisted on {port}.";
			});
		}

		private void Amend(Player player, int port, Func<Door, string> change)
		{
			string reply = null;

			bool amended = _doors.Amend(OwnerIdOf(player), port, door => reply = change(door));

			player.SendMessage(amended ? reply : $"You hold no door on port {port}.");
		}

		[Command(Name = "release", Description = "Give a door back, or all of them when no port is named")]
		public void ReleaseCommand(Player player, int port = 0)
		{
			int released = _doors.Release(OwnerIdOf(player), port, Context.Server.NetherNetListener);

			player.SendMessage(released == 0
				? port > 0 ? $"You hold no door on port {port}." : "You have no doors."
				: $"Released {released} door" + (released == 1 ? "." : "s."));
		}

		/// <summary>
		///     The account a door belongs to. The XUID when the login carried one, which is the only
		///     form that cannot be chosen by the person logging in; without Xbox authentication the
		///     name is all there is, and a door is then only as safe as the server's login policy.
		/// </summary>
		/// <summary>Outgoing byte totals and counts per packet type, since the plugin loaded.</summary>
		private readonly ConcurrentDictionary<string, long[]> _traffic = new ConcurrentDictionary<string, long[]>();

		/// <summary>
		///     Packet type names never sent from here, by class name. Config rather than code so the
		///     list can be cut back the moment a client refuses to spawn without one of them, which
		///     is the only way to find out which are truly optional.
		/// </summary>
		private HashSet<string> _suppressed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		/// <summary>Packet type names dropped on arrival, by class name.</summary>
		private HashSet<string> _suppressedIn = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		/// <summary>
		///     Whether the traffic tally runs. Off by default: it is a per-packet string concat, a
		///     dictionary lookup and an interlocked write on a shared cache line, which is the cost
		///     EngineMetrics keeps off packet-rate paths. Turn it on to aim the suppress lists, and
		///     off again before measuring anything.
		/// </summary>
		private bool _measureTraffic;

		/// <summary>
		///     Prices every outgoing packet. <c>EncodeAsMemory</c> caches its result and the
		///     compression pass calls it again on the way out, so measuring here costs the tally and
		///     nothing else: that encode was going to happen regardless.
		///     <para>
		///         Passing the packet back unchanged sends it. Returning null drops it, which is the
		///         suppression this measurement exists to aim.
		///     </para>
		/// </summary>
		[PacketHandler(PacketType = typeof(Packet))]
		[Send]
		public Packet MeasureOutgoing(Packet packet, Player player)
		{
			string name = packet.GetType().Name;

			// Dropped before measuring, so the tally reports what actually left rather than what
			// would have. A suppressed packet is counted separately under its own name.
			if (_suppressed.Contains(name))
			{
				if (_measureTraffic)
				{
					long[] dropped = _traffic.GetOrAdd("(suppressed) " + name, _ => new long[2]);
					Interlocked.Increment(ref dropped[0]);
				}

				return null;
			}

			if (!_measureTraffic) return packet;

			try
			{
				long size = packet.EncodeAsMemory().Length;
				long[] tally = _traffic.GetOrAdd(name, _ => new long[2]);

				Interlocked.Increment(ref tally[0]);
				Interlocked.Add(ref tally[1], size);
			}
			catch (Exception e)
			{
				// Measuring must never cost a packet. An encode that throws here would have thrown
				// in compression too, but that is the send path's failure to report, not ours to
				// turn into a dropped batch.
				Log.Debug($"Could not measure {packet.GetType().Name}", e);
			}

			return packet;
		}

		/// <summary>
		///     Counts every packet the client sends us. Inbound is the expensive side once a player is
		///     parked: outgoing is a join burst and then silence, while a client keeps sending input
		///     for as long as it is connected.
		///     <para>Counted after decode, so this measures what arrives, not what it cost to arrive.</para>
		/// </summary>
		[PacketHandler(PacketType = typeof(Packet))]
		[Receive]
		public Packet MeasureIncoming(Packet packet, Player player)
		{
			string name = packet.GetType().Name;

			if (_suppressedIn.Contains(name))
			{
				if (_measureTraffic)
				{
					long[] dropped = _traffic.GetOrAdd("<- (dropped) " + name, _ => new long[2]);
					Interlocked.Increment(ref dropped[0]);
				}

				// HandleBedrockMessage returns on null, so this is as early as a plugin can stop a
				// packet. The decrypt, decompress and decode are already spent by here; what it
				// saves is the dispatch and the handler.
				return null;
			}

			if (_measureTraffic)
			{
				long[] tally = _traffic.GetOrAdd("<- " + name, _ => new long[2]);
				Interlocked.Increment(ref tally[0]);
			}

			return packet;
		}

		[Command(Name = "traffic", Description = "What has been sent, by packet type, biggest first. Name a substring to filter")]
		public void TrafficCommand(Player player, string filter = "", int top = 12)
		{
			KeyValuePair<string, long[]>[] rows = _traffic
				.Where(row => filter.Length == 0 || row.Key.Contains(filter, StringComparison.OrdinalIgnoreCase))
				.OrderByDescending(row => row.Value[1])
				.ToArray();
			if (rows.Length == 0)
			{
				player.SendMessage("Nothing measured yet.");
				return;
			}

			long total = rows.Sum(row => row.Value[1]);

			player.SendMessage($"{total / 1024.0:F1} KB over {rows.Length} types");
			foreach (KeyValuePair<string, long[]> row in rows.Take(top))
			{
				player.SendMessage($"{row.Value[1] / 1024.0,7:F1} KB x{row.Value[0],-4} {row.Key}");
			}
		}

		[Command(Name = "trafficreset", Description = "Start the traffic tally again")]
		public void TrafficResetCommand(Player player)
		{
			_traffic.Clear();
			player.SendMessage("Traffic tally cleared.");
		}

		/// <summary>The key= value out of a raw query string, or null. The only query parsing this plugin needs, so no framework.</summary>
		private static string QueryKey(string query)
		{
			foreach (string pair in (query ?? "").Split('&', StringSplitOptions.RemoveEmptyEntries))
			{
				if (pair.StartsWith("key=", StringComparison.Ordinal)) return Uri.UnescapeDataString(pair.Substring(4));
			}

			return null;
		}

		private static string OwnerIdOf(Player player)
		{
			string xuid = player.CertificateData?.ExtraData?.Xuid;
			return string.IsNullOrEmpty(xuid) ? "name:" + player.Username.ToLowerInvariant() : "xuid:" + xuid;
		}

		/// <summary>
		///     Which signaling port this player arrived on. Only NetherNet has one: RakNet has no
		///     signaling connection to have arrived through.
		/// </summary>
		private static int SignalingPortOf(Player player)
		{
			return (player.NetworkHandler as NetherNetSession)?.SignalingPort ?? 0;
		}

		/// <summary>Transfers to a <c>host:port</c> route, or to port 19132 when it names only a host.</summary>
		private static void SendTo(Player player, string route)
		{
			string host = route;
			ushort port = 19132;

			int colon = route.LastIndexOf(':');
			if (colon > 0 && ushort.TryParse(route.Substring(colon + 1), out ushort parsed))
			{
				host = route.Substring(0, colon);
				port = parsed;
			}

			McpeTransfer transfer = McpeTransfer.CreateObject();
			transfer.serverAddress = host;
			transfer.port = port;
			player.SendPacket(transfer);

			Log.Info($"{player.Username} sent on to {host}:{port}.");
		}

		/// <summary>
		///     Puts the camera on a lap around the cube, looking inward the whole way. The player
		///     stays still at the centre, so what appears to turn is the cube.
		/// </summary>
		private static void StartOrbit(Player player)
		{
			var centre = new Vector3((float) Anchor.X, (float) Anchor.Y, (float) Anchor.Z);
			List<CameraSplinePoint> path = BuildOrbit(centre);

			// The free preset first: a spline is only valid on it. Placed at the path's own first
			// point so the lap does not begin with a jump.
			player.CameraManager.SetCamera(
				CameraPresets.Free,
				position: path[0].Position,
				facing: centre,
				ignoreStartingValues: true);

			player.CameraManager.FollowSpline(path);
		}

		/// <summary>
		///     One lap: a ring at <see cref="OrbitRadius" />, tilted out of the horizontal by
		///     <see cref="OrbitRise" />. Tilted rather than wavy on purpose, because a tilted circle
		///     is still a circle: every segment is the same length, so equal time per segment is also
		///     equal speed. Every point looks back at the centre.
		/// </summary>
		private static List<CameraSplinePoint> BuildOrbit(Vector3 centre)
		{
			var path = new List<CameraSplinePoint>(OrbitPoints + 1);
			double previousYaw = 0;

			// One extra point closing back onto the first, so the lap ends where it started and the
			// re-send does not jump.
			for (int i = 0; i <= OrbitPoints; i++)
			{
				double turn = 2 * Math.PI * i / OrbitPoints;

				var position = new Vector3(
					centre.X + (float) (Math.Sin(turn) * OrbitRadius),
					centre.Y + (float) (Math.Sin(turn) * OrbitRise),
					centre.Z + (float) (Math.Cos(turn) * OrbitRadius));

				Vector3 delta = centre - position;
				double flat = Math.Sqrt(delta.X * delta.X + delta.Z * delta.Z);

				double pitch = Math.Atan2(delta.Y, flat) * 180 / Math.PI;
				double yaw = Math.Atan2(delta.X, delta.Z) * 180 / Math.PI + 180;

				// Unwrapped against the previous point: the client interpolates raw yaw, so a
				// wrap from 359 to 1 is read as a whole revolution the other way.
				if (i > 0)
				{
					while (yaw - previousYaw > 180) yaw -= 360;
					while (yaw - previousYaw < -180) yaw += 360;
				}
				previousYaw = yaw;

				path.Add(new CameraSplinePoint
				{
					Position = position,
					Rotation = new Vector3((float) pitch, (float) yaw, 0),
					Time = OrbitSeconds * i / OrbitPoints,
					Ease = CameraEaseType.Linear
				});
			}

			return path;
		}

		/// <summary>
		///     Movement is client-authoritative, so a player cannot be forbidden to move, only put
		///     back. This also re-sends the orbit, because a spline runs once and stops.
		/// </summary>
		/// <summary>
		///     The plugin's own clock, replacing Player.OnTick. The level does not tick here, so
		///     nothing else calls us: what is actually needed is a spline re-send every
		///     <see cref="OrbitSeconds" /> and a countdown for doors that asked for one, neither of
		///     which wants 20Hz per player. One timer for the whole server beats a tick per player.
		/// </summary>
		private void OnPluginTick(object state)
		{
			// A pass that overruns its interval must not stack on the next one; the work is short,
			// so a skipped beat costs a second of countdown accuracy and nothing else.
			if (Interlocked.Exchange(ref _ticking, 1) != 0) return;

			try
			{
				foreach (Player player in _orbitSeconds.Keys)
				{
					if (_autoReturnSeconds.TryGetValue(player, out long remaining))
					{
						if (remaining <= 1)
						{
							_autoReturnSeconds.TryRemove(player, out _);
							SendTo(player, DestinationFor(player));
							continue;
						}

						_autoReturnSeconds[player] = remaining - 1;
					}

					if (FreeRoam) continue;

					long elapsed = _orbitSeconds.AddOrUpdate(player, 1, (_, seconds) => seconds + 1);
					if (elapsed < (long) OrbitSeconds) continue;

					_orbitSeconds[player] = 0;
					StartOrbit(player);
				}
			}
			catch (Exception e)
			{
				// The timer is the only thing driving this server. A throw that escapes it would
				// stop every orbit and every countdown, silently.
				Log.Error("Parking tick failed", e);
			}
			finally
			{
				_ticking = 0;
			}
		}

		private int _ticking;
	}
}
