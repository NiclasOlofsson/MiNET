using System;
using log4net;
using log4net.Config;
using Topshelf;

// Configure log4net using the .config file

[assembly: XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called TestApp.exe.config in the application base
// directory (i.e. the directory containing TestApp.exe)
// The config file will be watched for changes.

namespace MiNET.Ftl.Service
{
	public class MiNetFtlService
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetFtlService));

		private MiNetFtlServer _server;

		/// <summary>
		///     Starts this instance.
		/// </summary>
		private void Start()
		{
			Log.Info("Starting MiNET FTL");
			_server = new MiNetFtlServer();
			_server.StartServer();
		}

		/// <summary>
		///     Stops this instance.
		/// </summary>
		private void Stop()
		{
			Log.Info("Stopping MiNET FTL");
			_server.StopServer();
		}

		/// <summary>
		///     The programs entry point.
		/// </summary>
		/// <param name="args">The arguments.</param>
		private static void Main(string[] args)
		{
			HostFactory.Run(host =>
			{
				host.Service<MiNetFtlService>(s =>
				{
					s.ConstructUsing(construct => new MiNetFtlService());
					s.WhenStarted(service => service.Start());
					s.WhenStopped(service => service.Stop());
				});

				host.RunAsLocalService();
				host.SetDisplayName("MiNET FTL Service");
				host.SetDescription("MiNET faster-than-light server.");
				host.SetServiceName("MiNET");
			});
		}

		/// <summary>
		///     Determines whether is running on mono.
		/// </summary>
		/// <returns></returns>
		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}
	}
}