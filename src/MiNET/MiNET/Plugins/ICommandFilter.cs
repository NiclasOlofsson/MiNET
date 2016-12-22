namespace MiNET.Plugins
{
	public interface ICommandFilter
	{
		void OnCommandExecuting(Player player);
		void OnCommandExecuted();
	}
}