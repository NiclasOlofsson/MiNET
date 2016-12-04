using MiNET.Plugins.Attributes;

namespace MiNET.BuilderBase.Commands
{
	public class HistoryCommands // MUST NOT EXTEND RedoableCommand base.
	{
		[Command(Description = "Undo the last action")]
		public void Undo(Player player, int numberOfUndo = 1)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.Undo(numberOfUndo);
		}

		[Command(Description = "Redo the last action (from history)")]
		public void Redo(Player player, int numberOfRedo = 1)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);
			selector.Redo(numberOfRedo);
		}
	}
}