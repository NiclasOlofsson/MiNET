using System;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BuilderBase.Tools
{
	public class DistanceWand : ItemIronAxe
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DistanceWand));
		private NbtCompound _extraData;

		public override NbtCompound ExtraData
		{
			get { return _extraData; }
			set { _extraData = value; }
		}

		private void UpdateExtraData(Player player)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			_extraData = new NbtCompound
			{
				{
					new NbtCompound("display")
					{
						new NbtString("Name", ChatFormatting.Reset + ChatColors.Blue + "Distance Wand"),
						new NbtList("Lore")
						{
							new NbtString(ChatFormatting.Reset + ChatFormatting.Italic + ChatColors.White + "Wand that selects all blocks between pos1 and pos2."),
							new NbtString(ChatFormatting.Reset + ChatColors.Yellow + "Pos1: " + selector.Position1),
							new NbtString(ChatFormatting.Reset + ChatColors.Green + "Pos2: " + selector.Position2),
							//new NbtString(ChatFormatting.Reset + ChatColors.Gold + "Lore3"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Yellow + "Lore4"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Green + "Lore5"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Gold + "Lore6"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Yellow + "Lore7"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Green + "Lore8"),
							//new NbtString(ChatFormatting.Reset + ChatColors.Gold + "Lore9"),
						}
					}
				}
			};
		}


		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			SetPosition2(player, blockCoordinates);
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			var target = new EditHelper(world).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return;
			}

			if (target.Coordinates.DistanceTo((BlockCoordinates) player.KnownPosition) < 6) return;

			SetPosition2(player, target.Coordinates);
		}

		public override bool Animate(Level world, Player player)
		{
			var target = new EditHelper(world).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true);
			if (target == null)
			{
				player.SendMessage("No block in range");
				return true;
			}

			if (target.Coordinates.DistanceTo((BlockCoordinates) player.KnownPosition) < 6) return true;

			SetPosition1(player, target.Coordinates);

			return true;
		}

		public override bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
		{
			SetPosition1(player, block.Coordinates);
			Log.Warn("Break");

			return false; // Will revert block break;
		}

		public void SetPosition1(Player player, BlockCoordinates pos)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			if (selector.Position1.DistanceTo(pos) < 0.01)
			{
				pos.Y = (int) Math.Floor(player.KnownPosition.Y - 1.62f);
			}
			selector.SelectPrimary(pos);

			player.SendMessage($"First position set to {pos}");
			UpdateExtraData(player);
			player.Inventory.SendSetSlot(player.Inventory.ItemHotbar[player.Inventory.InHandSlot]);
		}

		public void SetPosition2(Player player, BlockCoordinates pos)
		{
			RegionSelector selector = RegionSelector.GetSelector(player);

			if (selector.Position2.DistanceTo(pos) < 0.01)
			{
				pos.Y = (int) Math.Floor(player.KnownPosition.Y - 1.62f);
			}
			selector.SelectSecondary(pos);

			player.SendMessage($"Second position set to {pos}");
			UpdateExtraData(player);
			player.Inventory.SendSetSlot(player.Inventory.ItemHotbar[player.Inventory.InHandSlot]);
		}
	}
}