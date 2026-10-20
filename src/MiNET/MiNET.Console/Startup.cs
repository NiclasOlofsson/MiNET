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
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Utils;

namespace MiNET.Console
{
	class Startup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

		static int Main(string[] args)
		{
			// Client mode: talk to a server that already runs, rather than being one.
			if (args.Length > 0 && args[0] == "remote")
			{
				return RemoteConsoleClient.Run(args).GetAwaiter().GetResult();
			}

			if (args.Length > 0 && args[0] == "listener")
			{
				// This is a brutal hack to block BDS to use the ports we are using. So we start this, and basically block BDS
				// while it is starting. Then we close down this process again, and continue on our way.
				var reset = new ManualResetEvent(false);
				using var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) {ExclusiveAddressUse = true};
				socket.Bind(new IPEndPoint(IPAddress.Any, 19132));
				System.Console.WriteLine("LISTENING!");
				reset.WaitOne();
				System.Console.WriteLine("EXIT!");
				return 0;
			}

			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			// Watch the deployed log4net.xml so log levels can be changed while the server runs,
			// e.g. dropping to INFO during load tests (per-packet TRACE serializes on the appender
			// lock) and back to VERBOSE for protocol work, without a restart.
			XmlConfigurator.ConfigureAndWatch(logRepository, new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.xml")));

			Log.Info(MiNetServer.MiNET);
			System.Console.WriteLine(MiNetServer.MiNET);

			// Processor affinity exists only on Windows and Linux. macOS throws PlatformNotSupportedException.
			if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
			{
				var currentProcess = Process.GetCurrentProcess();
				currentProcess.ProcessorAffinity = (IntPtr) Config.GetProperty("ProcessorAffinity", (int) currentProcess.ProcessorAffinity);
			}

			var service = new MiNetServer();
			Log.Info("Starting...");

			if (Config.GetProperty("UserBedrockGenerator", false))
			{
				service.LevelManager = new LevelManager();
				service.LevelManager.Generator = new BedrockGenerator();
			}

			service.StartServer();

			// A non-interactive host has no stdin to wait on, and used to sleep forever: the only
			// way out was a kill, which skips StopServer and with it the level save, losing
			// everything built since the last save interval. Ctrl+C, a close request, the remote
			// console and a shutdown all land here instead and stop the server properly.
			using var stopping = new ManualResetEventSlim(false);

			void RequestStop(PosixSignalContext context)
			{
				context.Cancel = true;
				stopping.Set();
			}

			using RemoteConsole remoteConsole = RemoteConsole.StartIfEnabled(service, () => stopping.Set());

			System.Console.WriteLine("MiNET running. Press <enter> to stop service.");

			// A headless run has no console to signal and no stdin to type into, so it also watches
			// for a file. Dropping temp_auto/stop-server next to the working directory stops the
			// server the same way pressing enter would, which is the only way the level actually
			// gets saved: a killed process never reaches StopServer.
			string stopFile = Path.Combine(Directory.GetCurrentDirectory(), "temp_auto", "stop-server");
			if (File.Exists(stopFile)) File.Delete(stopFile);

			using (PosixSignalRegistration.Create(PosixSignal.SIGINT, RequestStop))
			using (PosixSignalRegistration.Create(PosixSignal.SIGTERM, RequestStop))
			{
				if (System.Console.ReadLine() == null)
				{
					while (!stopping.IsSet && !File.Exists(stopFile)) stopping.Wait(500);
					if (File.Exists(stopFile)) File.Delete(stopFile);
				}
			}

			Log.Info("Shutting down, saving the level...");
			service.StopServer();

			return 0;
		}
	}
}