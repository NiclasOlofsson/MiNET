using MiNET.Plugins.Attributes;

namespace MiNET.BuilderBase.Commands
{
	public class HistoryCommands
	{
		[Command(Description = "Undo the last action")]
		public void Undo(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.Undo();
		}

		[Command(Description = "Redo the last action (from history)")]
		public void Redo(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.Redo();
		}
		[Command(Description = "Redo the last action (from history)")]
		public void Speed(Player player, int speed = 1)
		{
			player.MovementSpeed = speed / 10f;
			player.SendUpdateAttributes();
		}
	}
}