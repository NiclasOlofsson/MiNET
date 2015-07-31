using System.Collections.Generic;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.MobHunt
{
	public class CustomStandingSign : StandingSign
	{
		private static readonly Dictionary<string, Level> Worlds = new Dictionary<string, Level>();

		public override bool Interact(Level currentLevel, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			Sign signEntity = currentLevel.GetBlockEntity(blockCoordinates) as Sign;
			if (signEntity == null) return false;

			string world = signEntity.Text1;

			if (player.Level.LevelId.Equals(world)) return true;

			if (!Worlds.ContainsKey(player.Level.LevelId))
			{
				Worlds.Add(player.Level.LevelId, player.Level);
			}


			if (!Worlds.ContainsKey(world))
			{
				var mobHuntLevel = new MobHuntLevel(world, new FlatlandWorldProvider());
				mobHuntLevel.Initialize();
				Worlds.Add(world, mobHuntLevel);
			}

			Level level = Worlds[world];
			player.SpawnLevel(level);
			level.BroadcastMessage(string.Format("{0} teleported to world <{1}>.", player.Username, level.LevelId));

			return true;
		}
	}
}