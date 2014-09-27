using MiNET.Network;
using Topshelf;

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
			HostFactory.Run(host =>
			{
				host.Service<MiNetService>(s =>
				{
					s.ConstructUsing(construct => new MiNetService());
					s.WhenStarted(service => service.Start());
					s.WhenStopped(service => service.Stop());
				});

				host.RunAsLocalService();
				host.SetDisplayName("MiNET");
				host.SetDescription("MiNET MineCraft Pocket Edition server.");
				host.SetServiceName("MiNET");
			});
		}
	}
}
