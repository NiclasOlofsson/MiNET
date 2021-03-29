using System.Collections.Generic;
using System.Linq;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Vectors;
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
			EditSession = new EditHelper(player.Level, player, undoRecorder: UndoRecorder);
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
		public Dictionary<BlockCoordinates, Block> UndoBuffer { get; set; } = new Dictionary<BlockCoordinates, Block>();
		public Dictionary<BlockCoordinates, Block> RedoBuffer { get; set; } = new Dictionary<BlockCoordinates, Block>();

		public UndoRecorder(Level level, bool checkForDuplicates = true)
		{
			_level = level;
			CheckForDuplicates = checkForDuplicates;
		}

		public void RecordUndo(Block block)
		{
			//if (CheckForDuplicates && UndoBuffer.ContainsKey(block.Coordinates))
			//{
			//	return;
			//}
			UndoBuffer[block.Coordinates] = block;
		}

		public void RecordRedo(Block block)
		{
			RedoBuffer[block.Coordinates] = block;
		}

		public HistoryEntry CreateHistory()
		{
			return new HistoryEntry(_level, UndoBuffer.Values.ToList(), RedoBuffer.Values.ToList());
		}
	}
}