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

using System.IO;
using System.Reflection;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Console.Config.Providers;

namespace MiNET.Console
{
	class Startup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

		static void Main(string[] args)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.xml"));


			var config = CreateConfig();
			int threads;
			int portThreads;
			ThreadPool.GetMinThreads(out threads, out portThreads);
			Log.Info($"Threads: {threads}, Port Threads: {portThreads}");

			var service = new MiNetServer(config);
			Log.Info("Starting MiNET");
			service.StartServer();

			System.Console.WriteLine("MiNET running. Press <enter> to stop service.");
			System.Console.ReadLine();
			service.StopServer();
		}

		private static Configuration CreateConfig()
		{
			var serverConfig = new ServerConfiguration();
			var worldConfig = new WorldConfiguration();
			var securityConfig = new SecurityConfiguration();
			var playerConfig = new PlayerConfiguration(worldConfig);
			var pluginConfig = new PluginConfiguration();
			var debugConfig = new DebugConfiguration();
			var gameRuleConfig = new GameRuleConfiguration();
			return new Configuration(serverConfig, worldConfig, securityConfig, playerConfig, pluginConfig,
				debugConfig, gameRuleConfig);
		}
	}
}