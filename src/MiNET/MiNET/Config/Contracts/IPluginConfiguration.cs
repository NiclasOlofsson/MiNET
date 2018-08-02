namespace MiNET.Config.Contracts
{
	public interface IPluginConfiguration
	{
		string PluginDirectoryPaths { get; }
		bool PluginEnabled(string pluginName);
		bool PluginDisabled { get; }
	}
}