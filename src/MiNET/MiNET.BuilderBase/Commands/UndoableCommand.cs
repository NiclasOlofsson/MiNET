using System.Collections.Generic;
using System.Linq;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Commands
{
	public abstract class UndoableCommand : ICommandFilter
	{
		public UndoRecorder UndoRecorder { get; private set; }
		public EditHelper EditSession { get; private set; }
		public RegionSelector Selector { get; private set; }

		public void OnCommandExecuting(Player player)
		{
			Selector = RegionSelector.GetSelector(player);
			UndoRecorder = new UndoRecorder(player.Level);
			EditSession = new EditHelper(player.Level, undoRecorder: UndoRecorder);
		}

		public void OnCommandExecuted()
		{
			var history = UndoRecorder.CreateHistory();
			Selector.AddHistory(history);
		}
	}

	public class UndoRecorder
	{
		private readonly Level _level;
		public bool CheckForDuplicates { get; set; }
		public List<Block> UndoBuffer { get; set; } = new List<Block>();
		public List<Block> RedoBuffer { get; set; } = new List<Block>();

		public UndoRecorder(Level level, bool checkForDuplicates = true)
		{
			_level = level;
			CheckForDuplicates = checkForDuplicates;
		}

		public void RecordUndo(Block block)
		{
			if (CheckForDuplicates && UndoBuffer.Any(b => b.Coordinates == block.Coordinates)) return;
			UndoBuffer.Add(block);
		}

		public void RecordRedo(Block block)
		{
			RedoBuffer.Add(block);
		}

		public HistoryEntry CreateHistory()
		{
			return new HistoryEntry(_level, UndoBuffer, RedoBuffer);
		}
	}
}