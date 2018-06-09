using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemCoal : Item
	{
		public ItemCoal() : base(263)
		{
			MaxStackSize = 64;
			FuelEfficiency = 80;
		}

        public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
        {
            player.SetGameMode(player.GameMode == GameMode.C ? GameMode.S : GameMode.C);
        }
    }
}