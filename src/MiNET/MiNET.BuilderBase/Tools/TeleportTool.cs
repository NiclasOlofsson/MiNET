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
	public class TeleportTool : ItemCompass
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (DistanceWand));

		private NbtCompound _extraData;

		public override NbtCompound ExtraData
		{
			get
			{
				UpdateExtraData();
				return _extraData;
			}
			set { _extraData = value; }
		}

		private void UpdateExtraData()
		{
			_extraData = new NbtCompound
			{
				{
					new NbtCompound("display")
					{
						new NbtString("Name", ChatFormatting.Reset + ChatColors.Blue + $"Teleport tool"),
						new NbtList("Lore")
						{
							new NbtString(ChatFormatting.Reset + ChatFormatting.Italic + ChatColors.White + $"Left click to teleport."),
							new NbtString(ChatFormatting.Reset + ChatFormatting.Italic + ChatColors.White + $"Right teleport but stay on same Y."),
						}
					}
				}
			};
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Teleport(player, true);
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			Teleport(player, true);
		}

		public override bool Animate(Level world, Player player)
		{
			Teleport(player, false);
			return true;
		}

		public override bool BreakBlock(Level world, Player player, Block block, BlockEntity blockEntity)
		{
			Teleport(player, false);
			return false; // Will revert block break;
		}

		private void Teleport(Player player, bool stayOnY)
		{
			var target = new EditHelper(player.Level).GetBlockInLineOfSight(player.Level, player.KnownPosition, returnLastAir: true, limitHeight: false);
			if (target == null)
			{
				player.SendMessage("No position in range");
				return;
			}

			var pos = target.Coordinates;
			var known = player.KnownPosition;
			var newPosition = new PlayerLocation(pos.X, stayOnY ? known.Y - 1.62f : pos.Y + 1.62f, pos.Z, known.HeadYaw, known.Yaw, known.Pitch);
			player.SendMessage($"Wrooom to {target.Id} {newPosition}");
			player.Teleport(newPosition);
		}
	}
}