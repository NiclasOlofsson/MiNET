namespace MiNET.CommandHandler
{
	internal class HelpCommand
	{
		public string Command
		{
			get { return "help"; }
		}

		public string Description
		{
			get { return "Get information about a specific command."; }
		}

		public string Usage
		{
			get { return "/help <COMMAND>"; }
		}

		public string Permission
		{
			get { return "MiNET.help"; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			if (arguments.Length >= 1)
			{
				player.SendMessage(GetCommandHelp(arguments[0]));
				return true;
			}
			return false;
		}

		private string GetCommandHelp(string command, bool usage = false)
		{
			//foreach (var i in CommandManager.PluginCommands)
			//{
			//	CommandAttribute cmd = i.Key;
			//	if (command.Equals(cmd.Command, StringComparison.InvariantCultureIgnoreCase))
			//	{
			//		return string.Format("{0}: {1}\nUsage: {2}", cmd.Command, cmd.Description, cmd.Usage);
			//	}
			//}

			return string.Format("No usage for command: {0}", command);
		}
	}
}