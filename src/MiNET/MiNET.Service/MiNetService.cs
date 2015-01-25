using System;
using Topshelf;
using MiNET.Utils;

namespace MiNET.Service
{
	public class MiNetService
	{
		private MiNetServer _server;

		private void Start()
		{
			_server = new MiNetServer();
			_server.StartServer();
		}

		private void Stop()
		{
			_server.StopServer();
		}

		private static void Main(string[] args)
		{

            ConfigParser.ConfigFile = "server.config";

            ConfigParser.InitialValue = new string[] {
                "#DO NOT REMOVE THIS LINE - MiNET Config",
                "UsePCWorld=false",
                "WorldFolder=PathToWorld",
				"DefaultGamemode=Creative",
				"Difficulty=Peaceful",
            };

            ConfigParser.Check ();

            if (IsRunningOnMono ())
            {
                var service = new MiNetService ();
                service.Start ();
                Console.WriteLine ("MiNET runing. Press <enter> to stop service..");
                Console.ReadLine ();
                service.Stop ();
            } 

                HostFactory.Run (host =>
                {
                    host.Service<MiNetService> (s =>
                    {
                        s.ConstructUsing (construct => new MiNetService ());
                        s.WhenStarted (service => service.Start ());
                        s.WhenStopped (service => service.Stop ());
                    });

                    host.RunAsLocalService ();
                    host.SetDisplayName ("MiNET Service");
                    host.SetDescription ("MiNET MineCraft Pocket Edition server.");
                    host.SetServiceName ("MiNET");
                });
		}

		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}
	}
}