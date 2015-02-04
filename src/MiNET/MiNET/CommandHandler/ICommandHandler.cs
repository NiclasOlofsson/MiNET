namespace MiNET.CommandHandler
{
	internal interface ICommandHandler
	{
		string Command { get; }
		string Description { get; }
		string Usage { get; }
		string Permission { get; }
		bool Execute(Player player, string[] arguments);
	}
}