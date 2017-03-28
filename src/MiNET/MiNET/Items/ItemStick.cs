using System.Numerics;
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
			if (Log.IsDebugEnabled)
			{
				if (player.IsGliding)
				{
					var currentSpeed = player.CurrentSpeed/20f;
					if (currentSpeed > 35f/20f)
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
					motions.entityId = 0;
					motions.velocity = velocity;

					player.SendPackage(motions);
				}
				else
				{
					McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
					motions.entityId = 0;
					Vector3 velocity = new Vector3(0, 2, 0);
					motions.velocity = velocity;

					player.SendPackage(motions);

					player.IsGliding = true;
					player.Height = 0.6;
					player.BroadcastSetEntityData();
				}
			}
		}
	}
}