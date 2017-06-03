using System;
using System.Numerics;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemStick : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemStick));

		public ItemStick() : base(280)
		{
			FuelEfficiency = 5;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			if (player.IsGliding)
			{
				var currentSpeed = player.CurrentSpeed/20f;
				if (currentSpeed > 25f/20f)
				{
					//player.SendMessage($"Speed already over max {player.CurrentSpeed:F2}m/s", MessageType.Raw);
					return;
				}

				Vector3 velocity = Vector3.Normalize(player.KnownPosition.GetHeadDirection())*(float) currentSpeed;
				float factor = (float) (1 + 1/(1 + currentSpeed*2));
				velocity *= factor;

				if (currentSpeed < 7f/20f)
				{
					velocity = Vector3.Normalize(velocity)*1.2f;
				}

				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.runtimeEntityId = EntityManager.EntityIdSelf;
				motions.velocity = velocity;

				player.SendPackage(motions);
			}
			else if (player.Inventory.Chest is ItemElytra)
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.runtimeEntityId = EntityManager.EntityIdSelf;
				Vector3 velocity = new Vector3(0, 2, 0);
				motions.velocity = velocity;
				player.SendPackage(motions);

				SendWithDelay(200, () =>
				{
					player.IsGliding = true;
					player.Height = 0.6;
					player.BroadcastSetEntityData();
				});
			}
		}

		private async Task SendWithDelay(int delay, Action action)
		{
			await Task.Delay(delay);
			action();
		}

	}

}