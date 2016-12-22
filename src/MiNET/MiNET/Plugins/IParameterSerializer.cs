namespace MiNET.Plugins
{
	public interface IParameterSerializer
	{
		void Deserialize(Player player, string input);
	}
}