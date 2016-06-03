using System;
using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemCommand : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemCommand));

		public Action<ItemCommand, Level, Player, BlockCoordinates> Action { get; set; }
		public bool NeedBlockRevert { get; set; }

		public ItemCommand(short id, short metadata, Action<ItemCommand, Level, Player, BlockCoordinates> action) : base(id, metadata)
		{
			Action = action;
			if (action == null) throw new ArgumentNullException("action");
			Item realItem = ItemFactory.GetItem(id, metadata);
			NeedBlockRevert = realItem is ItemBlock;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (NeedBlockRevert)
			{
				var coord = GetNewCoordinatesFromFace(blockCoordinates, face);

				Log.Info("Reset block");
				// Resend the block to removed the new one
				Block block = world.GetBlock(coord);
				world.SetBlock(block);
			}

			Action(this, world, player, blockCoordinates);
		}
	}
}