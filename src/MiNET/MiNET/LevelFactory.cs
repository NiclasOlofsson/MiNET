using MiNET.Worlds;

namespace MiNET
{
	public class LevelFactory
	{
		private AnvilWorldProvider _worldProvider;

		public LevelFactory()
		{
			_worldProvider = new AnvilWorldProvider();
		}

		public virtual Level CreateLevel(string name)
		{
			var level = new Level(name, _worldProvider);
			level.Initialize();
			return level;
		}
	}
}