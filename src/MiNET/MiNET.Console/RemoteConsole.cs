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
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Console
{
	/// <summary>
	///     Runs server commands from off-box, so a protocol change can be exercised without a client
	///     connected and without typing into the game. Commands run as a <see cref="ConsolePlayer" />,
	///     which needs nobody online.
	/// </summary>
	public class RemoteConsole : IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RemoteConsole));

		/// <summary>How long a shutdown waits for sessions to close before saving anyway.</summary>
		private static readonly TimeSpan DrainTimeout = TimeSpan.FromSeconds(5);

		private readonly MiNetServer _server;
		private readonly string _secret;
		private readonly Action _requestStop;
		private readonly TcpListener _listener;
		private readonly CancellationTokenSource _cancellation = new CancellationTokenSource();

		private RemoteConsole(MiNetServer server, string secret, Action requestStop, IPEndPoint endPoint)
		{
			_server = server;
			_secret = secret;
			_requestStop = requestStop;
			_listener = new TcpListener(endPoint);
		}

		/// <param name="requestStop">Runs the host's own shutdown, which is the path that saves the level.</param>
		/// <returns>The running console, or null when it is switched off or misconfigured.</returns>
		public static RemoteConsole StartIfEnabled(MiNetServer server, Action requestStop)
		{
			if (!Config.GetProperty("RemoteConsole.Enabled", false)) return null;

			string secret = Config.GetProperty("RemoteConsole.Secret", "");
			if (string.IsNullOrWhiteSpace(secret))
			{
				// Refusing to run is the only safe option: the alternative is an open command channel.
				Log.Warn($"RemoteConsole.Enabled is set but RemoteConsole.Secret is empty, so the remote console will not start. "
						+ $"Add a secret to the config, for example RemoteConsole.Secret={Convert.ToHexString(RandomNumberGenerator.GetBytes(32))}");
				return null;
			}

			string address = Config.GetProperty("RemoteConsole.BindAddress", "127.0.0.1");
			int port = Config.GetProperty("RemoteConsole.Port", 19140);

			if (!IPAddress.TryParse(address, out IPAddress bind))
			{
				Log.Warn($"RemoteConsole.BindAddress '{address}' is not an IP address, so the remote console will not start");
				return null;
			}

			var console = new RemoteConsole(server, secret, requestStop, new IPEndPoint(bind, port));
			console.Start();
			return console;
		}

		private void Start()
		{
			_listener.Start();
			Log.Info($"Remote console listening on {_listener.LocalEndpoint}");

			Task.Run(AcceptLoop);
		}

		private async Task AcceptLoop()
		{
			try
			{
				while (!_cancellation.IsCancellationRequested)
				{
					TcpClient client = await _listener.AcceptTcpClientAsync(_cancellation.Token);
					_ = Task.Run(() => HandleClient(client));
				}
			}
			catch (OperationCanceledException)
			{
				// Shutting down.
			}
			catch (Exception e)
			{
				if (!_cancellation.IsCancellationRequested) Log.Error("Remote console accept loop failed", e);
			}
		}

		private async Task HandleClient(TcpClient client)
		{
			EndPoint peer = client.Client.RemoteEndPoint;

			try
			{
				using (client)
				{
					await using var stream = client.GetStream();

					string nonce = RemoteConsoleProtocol.CreateNonce();
					await RemoteConsoleProtocol.WriteFrameAsync(stream, nonce, _cancellation.Token);

					string answer = await RemoteConsoleProtocol.ReadFrameAsync(stream, _cancellation.Token);
					if (!RemoteConsoleProtocol.AnswerIsValid(_secret, nonce, answer))
					{
						Log.Warn($"Remote console rejected {peer}: bad secret");
						await RemoteConsoleProtocol.WriteFrameAsync(stream, RemoteConsoleProtocol.Denied, _cancellation.Token);
						return;
					}

					await RemoteConsoleProtocol.WriteFrameAsync(stream, RemoteConsoleProtocol.Accepted, _cancellation.Token);
					Log.Info($"Remote console accepted {peer}");

					while (!_cancellation.IsCancellationRequested)
					{
						string line = await RemoteConsoleProtocol.ReadFrameAsync(stream, _cancellation.Token);
						if (line == null) break;

						if (IsShutdownCommand(line))
						{
							// Answer before going down, otherwise the caller only sees a dropped
							// connection and cannot tell a clean shutdown from a crash.
							await RemoteConsoleProtocol.WriteFrameAsync(stream, Shutdown(line), _cancellation.Token);
							_requestStop();
							return;
						}

						await RemoteConsoleProtocol.WriteFrameAsync(stream, Execute(line), _cancellation.Token);
					}
				}
			}
			catch (Exception e)
			{
				if (!_cancellation.IsCancellationRequested) Log.Debug($"Remote console connection {peer} ended", e);
			}
		}

		private static bool IsShutdownCommand(string line)
		{
			string verb = line?.Trim().Split(' ')[0];

			return string.Equals(verb, "stop", StringComparison.OrdinalIgnoreCase)
					|| string.Equals(verb, "restart", StringComparison.OrdinalIgnoreCase);
		}

		/// <summary>
		///     Moves players off with a transfer rather than a disconnect, then stops. A transferred
		///     client drops its session and immediately starts trying to reach the address it was
		///     given, retrying for around thirty seconds, which is longer than a stop and start takes.
		///     So pointing it back at this same server means it sits waiting through the downtime and
		///     lands on the new process by itself, with no rejoin by hand.
		///     <para>
		///         The accept gate has to close first. Without it the client is back inside
		///         milliseconds, on the server that is in the middle of shutting down.
		///     </para>
		///     <para>Only stops. Whatever ran this is expected to start the server again.</para>
		/// </summary>
		private string Restart(string address, int port)
		{
			_server.AcceptConnections = false;

			int transferred = TransferEveryone(address, port);

			string state = WaitForSessionsToDrain();

			Log.Info($"Remote console restart: refused new connections, transferred {transferred} player(s) to {address}:{port}, {state}");

			return $"Restarting: transferred {transferred} player(s) to {address}:{port}, {state}. Saving the level. Start the server again and they will reconnect on their own.";
		}

		/// <summary>
		///     Sends every spawned player to another server and changes nothing else: this server keeps
		///     accepting, keeps its level loaded, and stays up.
		///     <para>
		///     That is what makes a parking server work. A transfer across a RESTART cannot hold anyone
		///     on NetherNet: signaling hands the client a ufrag and DTLS fingerprint belonging to the
		///     process that answered, so once that process exits the client is probing credentials
		///     nothing will ever recognise, and a dead listener refuses its next signaling attempt
		///     outright rather than making it wait. A second process that stays up has none of those
		///     problems - it answers immediately with credentials of its own.
		///     </para>
		/// </summary>
		private string Transfer(string address, int port)
		{
			int transferred = TransferEveryone(address, port);

			Log.Info($"Remote console transfer: sent {transferred} player(s) to {address}:{port}, still accepting");

			return $"Transferred {transferred} player(s) to {address}:{port}. This server is still up and still accepting.";
		}

		private int TransferEveryone(string address, int port)
		{
			int transferred = 0;
			foreach (Level level in _server.LevelManager?.Levels.ToArray() ?? Array.Empty<Level>())
			{
				// Everyone, not only the spawned: a player mid-join, or one sitting on a death screen,
				// is not "spawned" and would be silently left behind by a transfer that is trying to
				// empty the server. Found by parking a player in a world with no floor.
				foreach (Player player in level.GetAllPlayers())
				{
					McpeTransfer transfer = McpeTransfer.CreateObject();
					transfer.serverAddress = address;
					transfer.port = (ushort) port;

					// The client ignores a transfer that asks it to reload the world, so this has to
					// stay false for the packet to do anything at all.
					transfer.reloadWorld = false;

					player.SendPacket(transfer);
					transferred++;
				}
			}

			return transferred;
		}

		/// <summary>
		///     Drives the accept gate by hand, without shutting anything down. With it closed the
		///     server keeps every established session but answers no connection request, so a client
		///     that has just been transferred sits retrying against a silent socket. That is the state
		///     a restart needs to hold a player through, and being able to enter it on its own is what
		///     lets us measure how long a client will actually wait.
		/// </summary>
		private string SetAccepting(bool accepting)
		{
			_server.AcceptConnections = accepting;
			Log.Info($"Remote console set accepting connections to {accepting}");

			return accepting
				? "Accepting connections"
				: "Refusing connections. Established sessions are untouched.";
		}

		/// <summary>
		///     Empties the server before the host saves and exits. The order matters: a Bedrock client
		///     reconnects within milliseconds of being disconnected, so unless new connections are
		///     refused first, the player is back and mid-join by the time the level is being written.
		/// </summary>
		private string Shutdown()
		{
			_server.AcceptConnections = false;

			int disconnected = 0;
			foreach (Level level in _server.LevelManager?.Levels.ToArray() ?? Array.Empty<Level>())
			{
				foreach (Player player in level.GetAllPlayers())
				{
					player.Disconnect("Server is shutting down");
					disconnected++;
				}
			}

			string state = WaitForSessionsToDrain();

			Log.Info($"Remote console shutdown: refused new connections, disconnected {disconnected} player(s), {state}");

			return $"Stopping: disconnected {disconnected} player(s), {state}. Saving the level.";
		}

		private string WaitForSessionsToDrain()
		{
			DateTime deadline = DateTime.UtcNow + DrainTimeout;
			while (_server.LiveSessionCount > 0 && DateTime.UtcNow < deadline)
			{
				Thread.Sleep(100);
			}

			int remaining = _server.LiveSessionCount;

			return remaining == 0 ? "all sessions closed" : $"{remaining} session(s) still open after {DrainTimeout.TotalSeconds:F0}s";
		}

		/// <summary>Routes "stop" and "restart [address] [port]" to the right shutdown.</summary>
		private string Shutdown(string line)
		{
			string[] parts = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (!string.Equals(parts[0], "restart", StringComparison.OrdinalIgnoreCase)) return Shutdown();

			(string address, int port) = TransferTarget(parts);

			return Restart(address, port);
		}

		/// <summary>
		///     The address and port a "restart" or "transfer" sends players to: the command's own
		///     arguments when given, otherwise <c>RemoteConsole.TransferAddress</c>, which may carry
		///     its own port as <c>host:port</c>. A configured address without a port, or no address
		///     at all, falls back to this server's port.
		///     <para>
		///         Defaults to this server, since coming straight back to it is the point. It has to
		///         be an address the CLIENT can resolve, not one that works from here: with players
		///         joining over the internet, sending them to 127.0.0.1 points them at their own
		///         machine. The port matters as soon as the target is a parking server: those listen
		///         on their protocol number, never on this server's port.
		///     </para>
		/// </summary>
		private (string address, int port) TransferTarget(string[] parts)
		{
			int ownPort = _server.Endpoint?.Port ?? 19132;

			if (parts.Length > 1) return (parts[1], parts.Length > 2 ? int.Parse(parts[2]) : ownPort);

			string configured = Config.GetProperty("RemoteConsole.TransferAddress", "127.0.0.1");
			int colon = configured.LastIndexOf(':');
			if (colon > 0 && int.TryParse(configured.AsSpan(colon + 1), out int configuredPort))
			{
				return (configured.Substring(0, colon), configuredPort);
			}

			return (configured, ownPort);
		}

		private string Execute(string line)
		{
			line = line?.Trim();
			if (string.IsNullOrEmpty(line)) return "";

			if (string.Equals(line, "accept off", StringComparison.OrdinalIgnoreCase)) return SetAccepting(false);
			if (string.Equals(line, "accept on", StringComparison.OrdinalIgnoreCase)) return SetAccepting(true);

			// "transfer [address] [port]" - move everyone elsewhere and stay up. Same defaults as
			// restart, so a parking server configured with the dev box's address needs no arguments.
			if (line.StartsWith("transfer", StringComparison.OrdinalIgnoreCase))
			{
				(string target, int targetPort) = TransferTarget(line.Split(' ', StringSplitOptions.RemoveEmptyEntries));

				return Transfer(target, targetPort);
			}

			Level level = _server.LevelManager?.Levels.FirstOrDefault();
			if (level == null) return "No level is loaded yet";

			var player = new ConsolePlayer(level);

			try
			{
				Log.Info($"Remote console command: {line}");

				object result = _server.PluginManager.HandleCommand(player, line);
				string output = player.TakeOutput();

				// HandleCommand hands back the command's own return value, which for most commands is
				// the message it would have shown, and null for the ones that report via SendMessage.
				if (result is string text && !string.IsNullOrEmpty(text)) output += text;

				return string.IsNullOrWhiteSpace(output) ? $"Executed: {line}" : output.TrimEnd();
			}
			catch (Exception e)
			{
				Log.Error($"Remote console command failed: {line}", e);
				return $"Command failed: {e.Message}";
			}
		}

		public void Dispose()
		{
			_cancellation.Cancel();

			try
			{
				_listener.Stop();
			}
			catch (Exception e)
			{
				Log.Debug("Remote console listener did not stop cleanly", e);
			}

			_cancellation.Dispose();
		}
	}
}
