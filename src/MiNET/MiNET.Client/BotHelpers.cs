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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Client
{
	public class BotHelpers
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BotHelpers));

		public static PlayerLocation LookAt(Vector3 sourceLocation, Vector3 targetLocation)
		{
			var dx = targetLocation.X - sourceLocation.X;
			var dz = targetLocation.Z - sourceLocation.Z;

			var pos = new PlayerLocation(sourceLocation.X, sourceLocation.Y, sourceLocation.Z);
			if (dx > 0 || dz > 0)
			{
				double tanOutput = 90 - RadianToDegree(Math.Atan(dx / (dz)));
				double thetaOffset = 270d;
				if (dz < 0)
				{
					thetaOffset = 90;
				}
				var yaw = thetaOffset + tanOutput;

				double bDiff = Math.Sqrt((dx * dx) + (dz * dz));
				var dy = (sourceLocation.Y) - (targetLocation.Y);
				double pitch = RadianToDegree(Math.Atan(dy / (bDiff)));

				pos.Yaw = (float) yaw;
				pos.HeadYaw = (float) yaw;
				pos.Pitch = (float) pitch;
			}

			return pos;
		}

		private static double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}

		public static Action DoWaitForSpawn(MiNetClient client)
		{
			return () =>
			{
				while (!client.HasSpawned)
				{
					Thread.Sleep(50);
				}
			};
		}

		public static Action<Task, Item, int> DoMobEquipment(MiNetClient client)
		{
			Action<Task, Item, int> doMobEquipmentTask = (t, item, selectedSlot) =>
			{
				McpeMobEquipment message = new McpeMobEquipment();
				message.runtimeEntityId = client.EntityId;
				message.item = item;
				message.selectedSlot = (byte) selectedSlot;
				message.slot = (byte) (selectedSlot + 9);
				client.SendPacket(message);
			};
			return doMobEquipmentTask;
		}

		public static Action<Task, string> DoSendCommand(MiNetClient client)
		{
			Action<Task, string> doUseItem = (t, command) =>
			{
				//McpeCommandRequest commandStep = McpeCommandRequest.CreateObject();
				//commandStep.commandName = "fill";
				//commandStep.commandOverload = "replace";
				//commandStep.unknown1 = 0;
				//commandStep.currentStep = 0;
				//commandStep.isOutput = false;
				//commandStep.clientId = client.ClientId;
				////commandStep.commandInputJson = "{\n   \"tileName\" : \"dirt\",\n   \"from\" : {\n      \"x\" : 0,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 0,\n      \"zrelative\" : false\n   },\n   \"to\" : {\n      \"x\" : 10,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 10,\n      \"zrelative\" : false\n   }\n}\n";
				//commandStep.commandInputJson = "{\n   \"from\" : {\n      \"x\" : 0,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 0,\n      \"zrelative\" : false\n   },\n   \"tileName\" : \"dirt\",\n   \"to\" : {\n      \"x\" : 10,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 10,\n      \"zrelative\" : false\n   }\n}\n";
				////   "commandInputJson": "{\n   \"from\" : {\n      \"x\" : 0,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 0,\n      \"zrelative\" : false\n   },\n   \"tileName\" : \"dirt\",\n   \"to\" : {\n      \"x\" : 10,\n      \"xrelative\" : false,\n      \"y\" : 10,\n      \"yrelative\" : false,\n      \"z\" : 10,\n      \"zrelative\" : false\n   }\n}\n",

				////commandStep.commandInputJson = "null\n";
				//commandStep.commandOutputJson = "null\n";
				//commandStep.unknown7 = 0;
				//commandStep.unknown8 = 0;
				//commandStep.entityIdSelf = client.NetworkEntityId;
				////Log.Error($"Entity ID used={commandStep.entityIdSelf}\n{Package.HexDump(commandStep.Encode())}");
				//client.SendPackage(commandStep);
				McpeCommandRequest request = new McpeCommandRequest();
				request.command = command;
				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
				client.SendPacket(request);
			};
			return doUseItem;
		}

		public static Action<Task, PlayerLocation> DoMoveTo(MiNetClient client)
		{
			Action<Task, PlayerLocation> doMoveTo = (t, loc) =>
			{
				Vector3 originalPosition = client.CurrentLocation.ToVector3();
				Vector3 targetPosition = loc.ToVector3();

				PlayerLocation lookAtPos = LookAt(originalPosition + new Vector3(0, 1.62f, 0), targetPosition);

				{
					// First just rotate towards target pos
					McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
					movePlayerPacket.runtimeEntityId = client.EntityId;
					movePlayerPacket.x = client.CurrentLocation.X;
					movePlayerPacket.y = client.CurrentLocation.Y;
					movePlayerPacket.z = client.CurrentLocation.Z;
					movePlayerPacket.yaw = lookAtPos.Yaw;
					movePlayerPacket.pitch = lookAtPos.Pitch;
					movePlayerPacket.headYaw = lookAtPos.HeadYaw;
				}
				float lenght = Math.Abs((originalPosition - targetPosition).Length());

				float stepLen = 0.5f;
				float weight;

				while (true)
				{
					if (Math.Abs((targetPosition - client.CurrentLocation.ToVector3()).Length()) > stepLen)
					{
						float lenLeft = Math.Abs((client.CurrentLocation.ToVector3() - targetPosition).Length());
						weight = Math.Abs((float) ((lenLeft - stepLen) / lenght));

						client.CurrentLocation = new PlayerLocation(Vector3.Lerp(originalPosition, targetPosition, 1 - weight));

						McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
						movePlayerPacket.runtimeEntityId = client.EntityId;
						movePlayerPacket.x = client.CurrentLocation.X;
						movePlayerPacket.y = client.CurrentLocation.Y;
						movePlayerPacket.z = client.CurrentLocation.Z;
						movePlayerPacket.yaw = lookAtPos.Yaw;
						movePlayerPacket.pitch = lookAtPos.Pitch;
						movePlayerPacket.headYaw = lookAtPos.HeadYaw;

						client.SendPacket(movePlayerPacket);

						Thread.Sleep(50);
						continue;
					}
					{
						client.CurrentLocation = new PlayerLocation(targetPosition);

						McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
						movePlayerPacket.runtimeEntityId = client.EntityId;
						movePlayerPacket.x = client.CurrentLocation.X;
						movePlayerPacket.y = client.CurrentLocation.Y;
						movePlayerPacket.z = client.CurrentLocation.Z;
						movePlayerPacket.yaw = lookAtPos.Yaw;
						movePlayerPacket.pitch = lookAtPos.Pitch;
						movePlayerPacket.headYaw = lookAtPos.HeadYaw;

						client.SendPacket(movePlayerPacket);
					}
					break;
				}
			};
			return doMoveTo;
		}
	}
}