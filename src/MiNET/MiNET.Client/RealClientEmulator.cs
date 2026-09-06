#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Client
{
	/// <summary>
	///		Post-spawn behavioral script that mirrors a real 1.26.33 Bedrock client session: the same
	///		packet types and cadence a real client sends, built entirely through MiNET's own packet
	///		classes and encoders (never replayed captured bytes). Enabled by setting the MINET_EMULATE=1
	///		environment variable before starting the client (see <see cref="Startup"/>).
	///
	///		The script mirrors the distribution captured in temp_auto/trace-client-in/*.bin from a real
	///		1.26.33 client: PlayerAuthInput every tick, loading-screen/diagnostics/inventory-options
	///		bookkeeping packets, and a handful of interaction packets (animate, player action, interact,
	///		mob equipment, block pick, item stack request, inventory transaction, container close).
	/// </summary>
	public static class RealClientEmulator
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RealClientEmulator));

		private const int TicksPerSecond = 20;
		private const int RunSeconds = 30;
		private const int TotalTicks = TicksPerSecond * RunSeconds;

		public static void Run(MiNetClient client)
		{
			Log.Warn("RealClientEmulator: starting post-spawn emulation");

			// Loading screen open/close and set_local_player_as_initialized are part of the core
			// client spawn sequence now (see McpeClientMessageHandlerBase/BedrockTraceHandler),
			// sent by every bot session, emulated or not.
			SendInventoryOptions(client);

			// A real client sends these two after the join burst; without them the server side
			// they exercise is never reached. Values are what a real 1.26.40 client puts on the
			// wire: game type 5, and three identical aim-assist frames.
			SendSetPlayerGameType(client);
			for (int i = 0; i < 3; i++) SendClientCameraAimAssist(client);

			Vector3 position = client.CurrentLocation.ToVector3();
			Vector3 lastPosition = position;
			float yaw = client.CurrentLocation.Yaw;
			float pitch = client.CurrentLocation.Pitch;
			float headYaw = client.CurrentLocation.HeadYaw;

			// Block below spawn (for the break sequence) and the block next to it (for the place).
			var spawnBlock = new BlockCoordinates((int) Math.Floor(position.X), (int) Math.Floor(position.Y) - 1, (int) Math.Floor(position.Z));
			var placeBlock = new BlockCoordinates(spawnBlock.X + 1, spawnBlock.Y, spawnBlock.Z);

			long tick = 1;

			for (int i = 0; i < TotalTicks; i++)
			{
				AuthInputFlags flags = AuthInputFlags.BlockBreakingDelayEnabled;
				Vector2 moveVector = Vector2.Zero;

				// Walk forward a few blocks between t=5s and t=9s, sprinting for the middle stretch.
				bool walking = i is >= 100 and < 180;
				if (walking)
				{
					flags |= AuthInputFlags.WalkForwards;
					moveVector = new Vector2(0, 1);
					position += new Vector3(0, 0, -0.13f); // ~4 blocks over 80 ticks
				}
				if (i == 120) flags |= AuthInputFlags.StartSprinting;
				if (i is >= 120 and < 160) flags |= AuthInputFlags.Sprinting;
				if (i == 160) flags |= AuthInputFlags.StopSprinting;

				var input = McpePlayerAuthInput.CreateObject();
				input.playerRotation = new Vector2(pitch, yaw);
				input.playerHeadRotation = headYaw;
				input.position = position;
				input.moveVector = moveVector;
				input.inputData = flags;
				input.inputMode = McpePlayerAuthInput.InputMode.Mouse;
				input.playMode = McpePlayerAuthInput.ClientPlayMode.Normal;
				input.newInteractionModel = McpePlayerAuthInput.NewInteractionModel.Touch;
				input.interactRotation = new Vector2(pitch, yaw);
				input.clientTick = tick++;
				input.posDelta = position - lastPosition;
				input.analogMoveVector = moveVector;
				input.cameraOrientation = new Vector3(pitch, yaw, 0);
				input.rawMoveVector = moveVector;
				// Since 2168 block breaking rides inside auth input (a standalone PlayerAction
				// StartBreak gets the session kicked for exploiting by BDS's anticheat).
				if (i == 21) input.playerBlockActions = new List<PlayerBlockActionData> {new PlayerBlockActionData {playerActionType = (PlayerBlockActionData.PlayerActionType) 0, position = spawnBlock, facing = (int) BlockFace.Up}};
				if (i == 30) input.playerBlockActions = new List<PlayerBlockActionData> {new PlayerBlockActionData {playerActionType = (PlayerBlockActionData.PlayerActionType) 1, position = spawnBlock, facing = (int) BlockFace.Up}};

				client.SendPacket(input);

				lastPosition = position;
				client.CurrentLocation = new PlayerLocation(position, headYaw, yaw, pitch);

				if (i % 100 == 0) SendDiagnostics(client);
				if (i == 20) SendAnimate(client, McpeAnimate.ActorSwingSource.Mine);
					if (i == 31) SendAnimate(client, McpeAnimate.ActorSwingSource.None);
				if (i == 40) SendInteractMouseOver(client);
				if (i == 45) SendInteractOpenInventory(client);
				if (i == 50) SendMobEquipment(client);
				if (i == 55) SendBlockPickRequest(client, spawnBlock);
				if (i == 60) SendItemStackRequest(client);
				if (i == 65) SendInventoryTransactionPlace(client, placeBlock, position);
				if (i == TotalTicks - 1) SendContainerClose(client);

				Thread.Sleep(1000 / TicksPerSecond);
			}

			Log.Warn("RealClientEmulator: emulation complete");
		}

		private static void SendSetPlayerGameType(MiNetClient client)
		{
			var packet = McpeSetPlayerGameType.CreateObject();
			packet.gamemode = 5; // GameType.Default, what the real client sends at join
			client.SendPacket(packet);
		}

		private static void SendClientCameraAimAssist(MiNetClient client)
		{
			var packet = McpeClientCameraAimAssist.CreateObject();
			packet.presetId = string.Empty;
			packet.action = 1;
			packet.allowAimAssist = false;
			client.SendPacket(packet);
		}

		private static void SendInventoryOptions(MiNetClient client)
		{
			// No captured real-client frame for this packet exists in the trace; values are
			// conservative defaults (no tab selected, no filtering, default layout).
			var packet = McpeSetPlayerInventoryOptions.CreateObject();
			packet.leftTab = 0;
			packet.rightTab = 0;
			packet.filtering = false;
			packet.layout = 0;
			packet.craftingLayout = 0;
			client.SendPacket(packet);
		}

		private static void SendDiagnostics(MiNetClient client)
		{
			var packet = McpeServerBoundDiagnostics.CreateObject();
			packet.averageFramesPerSecond = 60f;
			packet.averageServerSimTickTime = 0f;
			packet.averageClientSimTickTime = 0.0005f;
			packet.averageBeginFrameTime = 0.04f;
			packet.averageInputTime = 0.08f;
			packet.averageRenderTime = 0.4f;
			packet.averageEndFrameTime = 6.5f;
			packet.averageRemainderTimePercent = 50f;
			packet.averageUnaccountedTimePercent = 4.5f;
			client.SendPacket(packet);
		}

		private static void SendAnimate(MiNetClient client, McpeAnimate.ActorSwingSource swingSource)
		{
			var packet = McpeAnimate.CreateObject();
			packet.actionId = McpeAnimate.AnimatePacketPayloadAction.Swing;
			packet.runtimeEntityId = client.EntityId;
			packet.data = 0f;
			packet.swingSource = swingSource;
			packet.hand = McpeAnimate.HandSlot.Mainhand;
			client.SendPacket(packet);
		}

		private static void SendPlayerAction(MiNetClient client, PlayerAction action, BlockCoordinates coordinates, BlockFace face)
		{
			var packet = McpePlayerAction.CreateObject();
			packet.runtimeEntityId = client.EntityId;
			packet.actionId = (int) action;
			packet.coordinates = coordinates;
			packet.resultCoordinates = default; // matches captured real-client frames (always zeroed)
			packet.face = (int) face;
			client.SendPacket(packet);
		}

		private static void SendInteractMouseOver(MiNetClient client)
		{
			var packet = McpeInteract.CreateObject();
			packet.actionId = (byte) McpeInteract.Actions.MouseOver;
			packet.targetRuntimeEntityId = 0;
			packet.Position = null;
			client.SendPacket(packet);
		}

		// What a real client sends when the player opens their own inventory/creative screen
		// (observed live: actionId 6, target = own runtime id). Used to probe what the server
		// answers - vanilla's reply (or silence) is the reference for MiNET's handler.
		private static void SendInteractOpenInventory(MiNetClient client)
		{
			var packet = McpeInteract.CreateObject();
			packet.actionId = (byte) McpeInteract.Actions.OpenInventory;
			packet.targetRuntimeEntityId = client.EntityId;
			packet.Position = null;
			client.SendPacket(packet);
		}

		private static void SendMobEquipment(MiNetClient client)
		{
			var packet = McpeMobEquipment.CreateObject();
			packet.runtimeEntityId = client.EntityId;
			packet.item = new ItemAir();
			packet.slot = 0;
			packet.selectedSlot = 0;
			packet.windowsId = 0;
			client.SendPacket(packet);
		}

		private static void SendBlockPickRequest(MiNetClient client, BlockCoordinates coordinates)
		{
			var packet = McpeBlockPickRequest.CreateObject();
			packet.x = coordinates.X;
			packet.y = coordinates.Y;
			packet.z = coordinates.Z;
			packet.addUserData = false;
			packet.selectedSlot = 0;
			client.SendPacket(packet);
		}

		// A creative-mode drag: create the item from the creative catalog, then take it into hotbar
		// slot 0. Container id 60 = creative output; 0 = the player's own inventory (hotbar slot 9
		// maps to hotbar 0). Mirrors PMMP ItemStackRequest::read / minecraft-data 1001 exactly (see
		// Packet.Write(ItemStackRequests) and Write(StackRequestSlotInfo)).
		private static ItemStackRequestSlotInfo Slot(int container, byte slot)
		{
			return new ItemStackRequestSlotInfo
			{
				fullContainerName = new FullContainerName {containerName = (FullContainerName.ContainerEnumName) container},
				slot = slot
			};
		}

		private static void SendItemStackRequest(MiNetClient client)
		{
			var packet = McpeItemStackRequest.CreateObject();
			packet.requests = new List<ItemStackRequest>();

			var actions = new ItemStackRequest
			{
				clientRequestId = -1,
				actions = new List<ItemStackRequestBase>
				{
					new ItemStackRequestCraftCreativeAction {creativeItemNetId = 1},
					new ItemStackRequestTakeAction
					{
						amount = 1,
						source = Slot(60, 50),
						destination = Slot(0, 9)
					}
				}
			};
			packet.requests.Add(actions);

			client.SendPacket(packet);
		}

		private static void SendInventoryTransactionPlace(MiNetClient client, BlockCoordinates position, Vector3 playerPosition)
		{
			var packet = McpeInventoryTransaction.CreateObject();
			var transaction = new ItemUseInventoryTransaction
			{
				actionType = ItemUseInventoryTransaction.ItemUseActionType.Place,
				triggerType = ItemUseInventoryTransaction.ItemUseTriggerType.PlayerInput,
				position = position,
				face = (byte) BlockFace.Up,
				slot = 0,
				item = ItemFactory.GetItemByName("minecraft:dirt"),
				fromPosition = playerPosition,
				clickPosition = new Vector3(0.5f, 1f, 0.5f),
				targetBlockId = 0,
				actions = new List<InventoryAction>()
			};
			packet.legacyRequestId = 0;
			packet.transaction = transaction;
			client.SendPacket(packet);
		}

		private static void SendContainerClose(MiNetClient client)
		{
			var packet = McpeContainerClose.CreateObject();
			packet.windowId = 0;
			packet.windowType = 0xf7;
			packet.server = false;
			client.SendPacket(packet);
		}
	}
}
