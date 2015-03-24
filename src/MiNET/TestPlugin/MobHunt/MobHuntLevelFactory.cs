using MiNET;
using MiNET.Worlds;

namespace TestPlugin.MobHunt
{
	public class MobHuntLevelFactory : LevelFactory
	{
		public override Level CreateLevel(string name)
		{
			Level level = new MobHuntLevel(name);
			level.Initialize();
			return level;
		}
	}
}