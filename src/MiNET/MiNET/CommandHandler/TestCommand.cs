namespace MiNET.CommandHandler
{
	internal class TestCommand : ICommandHandler
	{
		public string Command
		{
			get { return "test"; }
		}

		public string Description
		{
			get { return "A MiNET Test command."; }
		}

		public string Usage
		{
			get { return "/test"; }
		}

		public string Permission
		{
			get { return "MiNET.test"; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			player.Level.BroadcastTextMessage("Hello World", player);
			return true;
		}
	}
}