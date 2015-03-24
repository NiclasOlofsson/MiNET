using MiNET.Worlds;

namespace MiNET
{
	public class LevelFactory
	{
		public virtual Level CreateLevel(string name)
		{
			var level = new Level(name);
			level.Initialize();
			return level;
		}
	}
}