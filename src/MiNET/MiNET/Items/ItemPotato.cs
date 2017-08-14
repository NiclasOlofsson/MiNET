using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemPotato : FoodItem
	{
		public ItemPotato() : base(392, 0, 1, 0.6)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block targetBlock = world.GetBlock(targetCoordinates);
			Block potatos = new Potatoes();
			potatos.Coordinates = targetBlock.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			potatos.Metadata = (byte) Metadata;

			if (!potatos.CanPlace(world, targetCoordinates, face)) return;

			if (potatos.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(potatos);
		}

		public override Item GetSmelt()
		{
			return new ItemBakedPotato();
		}
	}
}