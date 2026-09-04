#region LICENSE

// The contents of this file are subject to the Common Public Attribution// The contents of this file are subject to the Common Public Attribution
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

//
// WARNING: T4 GENERATED CODE - DO NOT EDIT
// 

using System;
using System.Net;
using System.Numerics;
using System.Threading;
using fNbt;
using MiNET.Utils; 
using MiNET.Utils.Skins;
using MiNET.Items;
using MiNET.Crafting;

using little = MiNET.Utils.Int24; // friendly name
using LongString = System.String;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Utils.Nbt;

namespace MiNET.Net
{
	public class McpeProtocolInfo
	{
		public const int ProtocolVersion = 2207;
		public const string GameVersion = "1.26.60";
	}

	public enum CommandPermission
	{
		Normal = 0,
		Operator = 1,
		Host = 2,
		Automation = 3,
		Admin = 4,
	}
	public enum PermissionLevel
	{
		Visitor = 0,
		Member = 1,
		Operator = 2,
		Custom = 3,
	}
	public enum ActionPermissions
	{
		BuildAndMine = 0x1,
		DoorsAndSwitches = 0x2,
		OpenContainers = 0x4,
		AttackPlayers = 0x8,
		AttackMobs = 0x10,
		Operator = 0x20,
		Teleport = 0x80,
		Default = (BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs ),
		All = (BuildAndMine | DoorsAndSwitches | OpenContainers | AttackPlayers | AttackMobs | Operator | Teleport),
	}

	public partial class UnconnectedPing : Packet<UnconnectedPing>
	{

		public long pingId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public long guid; // = null;

		public UnconnectedPing()
		{
			Id = 0x01;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(pingId);
			Write(offlineMessageDataId);
			Write(guid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			pingId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			guid = ReadLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			pingId=default(long);
			guid=default(long);
		}

	}

	public partial class UnconnectedPong : Packet<UnconnectedPong>
	{

		public long pingId; // = null;
		public long serverId; // = null;
		public readonly byte[] offlineMessageDataId = new byte[]{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }; // = { 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 };
		public string serverName; // = null;

		public UnconnectedPong()
		{
			Id = 0x1c;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(pingId);
			Write(serverId);
			Write(offlineMessageDataId);
			WriteFixedString(serverName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			pingId = ReadLong();
			serverId = ReadLong();
			ReadBytes(offlineMessageDataId.Length);
			serverName = ReadFixedString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			pingId=default(long);
			serverId=default(long);
			serverName=default(string);
		}

	}

	public partial class McpeLogin : Packet<McpeLogin>
	{

		public int protocolVersion; // = null;
		public byte[] payload; // = null;

		public McpeLogin()
		{
			Id = 0x01;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteBe(protocolVersion);
			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			protocolVersion = ReadIntBe();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			protocolVersion=default(int);
			payload=default(byte[]);
		}

	}

	public partial class McpePlayStatus : Packet<McpePlayStatus>
	{
		public enum PlayStatus
		{
			LoginSuccess = 0,
			LoginFailedClient = 1,
			LoginFailedServer = 2,
			PlayerSpawn = 3,
			LoginFailedInvalidTenant = 4,
			LoginFailedVanillaEdu = 5,
			LoginFailedEduVanilla = 6,
			LoginFailedServerFull = 7,
			LoginFailedEditorVanillaMismatch = 8,
			LoginFailedVanillaEditorMismatch = 9,
		}

		public int status; // = null;

		public McpePlayStatus()
		{
			Id = 0x02;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteBe(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			status = ReadIntBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			status=default(int);
		}

	}

	public partial class McpeClientToServerHandshake : Packet<McpeClientToServerHandshake>
	{


		public McpeClientToServerHandshake()
		{
			Id = 0x04;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeDisconnect : Packet<McpeDisconnect>
	{

		public int reason; // = null;
		public bool hideDisconnectReason; // = null;

		public McpeDisconnect()
		{
			Id = 0x05;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(reason);
			Write(hideDisconnectReason);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			reason = ReadSignedVarInt();
			hideDisconnectReason = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			reason=default(int);
			hideDisconnectReason=default(bool);
		}

	}

	public partial class McpeResourcePackStack : Packet<McpeResourcePackStack>
	{

		public bool mustAccept; // = null;
		public ResourcePackIdVersions resourcepackidversions; // = null;
		public string gameVersion; // = null;
		public Experiments experiments; // = null;
		public bool hasEditorPacks; // = null;

		public McpeResourcePackStack()
		{
			Id = 0x07;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(mustAccept);
			Write(resourcepackidversions);
			Write(gameVersion);
			Write(experiments);
			Write(hasEditorPacks);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mustAccept = ReadBool();
			resourcepackidversions = ReadResourcePackIdVersions();
			gameVersion = ReadString();
			experiments = ReadExperiments();
			hasEditorPacks = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mustAccept=default(bool);
			resourcepackidversions=default(ResourcePackIdVersions);
			gameVersion=default(string);
			experiments=default(Experiments);
			hasEditorPacks=default(bool);
		}

	}

	public partial class McpeText : Packet<McpeText>
	{
		public enum ChatTypes
		{
			Raw = 0,
			Chat = 1,
			Translation = 2,
			Popup = 3,
			Jukeboxpopup = 4,
			Tip = 5,
			System = 6,
			Whisper = 7,
			Announcement = 8,
			Jsonwhisper = 9,
			Json = 10,
			Jsonannouncement = 11,
		}


		public McpeText()
		{
			Id = 0x09;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeSetTime : Packet<McpeSetTime>
	{

		public int time; // = null;

		public McpeSetTime()
		{
			Id = 0x0a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(time);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			time = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			time=default(int);
		}

	}

	public partial class McpeRemoveEntity : Packet<McpeRemoveEntity>
	{

		public long entityIdSelf; // = null;

		public McpeRemoveEntity()
		{
			Id = 0x0e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
		}

	}

	public partial class McpeServerPlayerPostMovePosition : Packet<McpeServerPlayerPostMovePosition>
	{

		public Vector3 position; // = null;

		public McpeServerPlayerPostMovePosition()
		{
			Id = 0x10;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(position);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			position = ReadVector3();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			position=default(Vector3);
		}

	}

	public partial class McpeTakeItemEntity : Packet<McpeTakeItemEntity>
	{

		public long runtimeEntityId; // = null;
		public long target; // = null;

		public McpeTakeItemEntity()
		{
			Id = 0x11;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteUnsignedVarLong(target);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			target = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			target=default(long);
		}

	}

	public partial class McpeMoveEntity : Packet<McpeMoveEntity>
	{

		public long runtimeEntityId; // = null;
		public byte flags; // = null;
		public PlayerLocation position; // = null;

		public McpeMoveEntity()
		{
			Id = 0x12;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(flags);
			Write(position);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			flags = ReadByte();
			position = ReadPlayerLocation();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			flags=default(byte);
			position=default(PlayerLocation);
		}

	}

	public partial class McpeUpdateBlock : Packet<McpeUpdateBlock>
	{
		public enum Flags
		{
			None = 0,
			Neighbors = 1,
			Network = 2,
			Nographic = 4,
			Priority = 8,
			All = (Neighbors | Network),
			AllPriority = (All | Priority),
		}

		public BlockCoordinates coordinates; // = null;
		public uint blockRuntimeId; // = null;
		public uint blockPriority; // = null;
		public uint storage; // = null;

		public McpeUpdateBlock()
		{
			Id = 0x15;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(blockRuntimeId);
			WriteUnsignedVarInt(blockPriority);
			WriteUnsignedVarInt(storage);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			blockRuntimeId = ReadUnsignedVarInt();
			blockPriority = ReadUnsignedVarInt();
			storage = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			blockRuntimeId=default(uint);
			blockPriority=default(uint);
			storage=default(uint);
		}

	}

	public partial class McpeAddPainting : Packet<McpeAddPainting>
	{

		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public Vector3 coordinates; // = null;
		public int direction; // = null;
		public string title; // = null;

		public McpeAddPainting()
		{
			Id = 0x16;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			Write(coordinates);
			WriteSignedVarInt(direction);
			Write(title);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			coordinates = ReadVector3();
			direction = ReadSignedVarInt();
			title = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			coordinates=default(Vector3);
			direction=default(int);
			title=default(string);
		}

	}

	public partial class McpeLevelEvent : Packet<McpeLevelEvent>
	{

		public int eventId; // = null;
		public Vector3 position; // = null;
		public int data; // = null;

		public McpeLevelEvent()
		{
			Id = 0x19;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(eventId);
			Write(position);
			WriteSignedVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventId = ReadSignedVarInt();
			position = ReadVector3();
			data = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventId=default(int);
			position=default(Vector3);
			data=default(int);
		}

	}

	public partial class McpeBlockEvent : Packet<McpeBlockEvent>
	{

		public BlockCoordinates coordinates; // = null;
		public int case1; // = null;
		public int case2; // = null;

		public McpeBlockEvent()
		{
			Id = 0x1a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteSignedVarInt(case1);
			WriteSignedVarInt(case2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			case1 = ReadSignedVarInt();
			case2 = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			case1=default(int);
			case2=default(int);
		}

	}

	public partial class McpeEntityEvent : Packet<McpeEntityEvent>
	{

		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int data; // = null;

		public McpeEntityEvent()
		{
			Id = 0x1b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(eventId);
			WriteSignedVarInt(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			data = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventId=default(byte);
			data=default(int);
		}

	}

	public partial class McpeMobEffect : Packet<McpeMobEffect>
	{

		public long runtimeEntityId; // = null;
		public byte eventId; // = null;
		public int effectId; // = null;
		public int amplifier; // = null;
		public bool particles; // = null;
		public int duration; // = null;
		public long tick; // = null;
		public bool ambient; // = null;

		public McpeMobEffect()
		{
			Id = 0x1c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(eventId);
			WriteSignedVarInt(effectId);
			WriteSignedVarInt(amplifier);
			Write(particles);
			WriteSignedVarInt(duration);
			WriteUnsignedVarLong(tick);
			Write(ambient);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventId = ReadByte();
			effectId = ReadSignedVarInt();
			amplifier = ReadSignedVarInt();
			particles = ReadBool();
			duration = ReadSignedVarInt();
			tick = ReadUnsignedVarLong();
			ambient = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventId=default(byte);
			effectId=default(int);
			amplifier=default(int);
			particles=default(bool);
			duration=default(int);
			tick=default(long);
			ambient=default(bool);
		}

	}

	public partial class McpeUpdateAttributes : Packet<McpeUpdateAttributes>
	{

		public long runtimeEntityId; // = null;
		public PlayerAttributes attributes; // = null;
		public long tick; // = null;

		public McpeUpdateAttributes()
		{
			Id = 0x1d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(attributes);
			WriteUnsignedVarLong(tick);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			attributes = ReadPlayerAttributes();
			tick = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			attributes=default(PlayerAttributes);
			tick=default(long);
		}

	}

	public partial class McpeInteract : Packet<McpeInteract>
	{
		public enum Actions
		{
			RightClick = 1,
			LeftClick = 2,
			LeaveVehicle = 3,
			MouseOver = 4,
			OpenNpc = 5,
			OpenInventory = 6,
		}

		public byte actionId; // = null;
		public long targetRuntimeEntityId; // = null;

		public McpeInteract()
		{
			Id = 0x21;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(actionId);
			WriteUnsignedVarLong(targetRuntimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actionId = ReadByte();
			targetRuntimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actionId=default(byte);
			targetRuntimeEntityId=default(long);
		}

	}

	public partial class McpeBlockPickRequest : Packet<McpeBlockPickRequest>
	{

		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool addUserData; // = null;
		public byte selectedSlot; // = null;

		public McpeBlockPickRequest()
		{
			Id = 0x22;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(x);
			WriteSignedVarInt(y);
			WriteSignedVarInt(z);
			Write(addUserData);
			Write(selectedSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			x = ReadSignedVarInt();
			y = ReadSignedVarInt();
			z = ReadSignedVarInt();
			addUserData = ReadBool();
			selectedSlot = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			x=default(int);
			y=default(int);
			z=default(int);
			addUserData=default(bool);
			selectedSlot=default(byte);
		}

	}

	public partial class McpeEntityPickRequest : Packet<McpeEntityPickRequest>
	{

		public ulong runtimeEntityId; // = null;
		public byte selectedSlot; // = null;
		public bool addUserData; // = null;

		public McpeEntityPickRequest()
		{
			Id = 0x23;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(runtimeEntityId);
			Write(selectedSlot);
			Write(addUserData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUlong();
			selectedSlot = ReadByte();
			addUserData = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(ulong);
			selectedSlot=default(byte);
			addUserData=default(bool);
		}

	}

	public partial class McpePlayerAction : Packet<McpePlayerAction>
	{

		public long runtimeEntityId; // = null;
		public int actionId; // = null;
		public BlockCoordinates coordinates; // = null;
		public BlockCoordinates resultCoordinates; // = null;
		public int face; // = null;

		public McpePlayerAction()
		{
			Id = 0x24;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(actionId);
			Write(coordinates);
			Write(resultCoordinates);
			WriteSignedVarInt(face);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			actionId = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			resultCoordinates = ReadBlockCoordinates();
			face = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			actionId=default(int);
			coordinates=default(BlockCoordinates);
			resultCoordinates=default(BlockCoordinates);
			face=default(int);
		}

	}

	public partial class McpeHurtArmor : Packet<McpeHurtArmor>
	{

		public int cause; // = null;
		public int health; // = null;
		public long armorSlotFlags; // = null;

		public McpeHurtArmor()
		{
			Id = 0x26;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(cause);
			WriteSignedVarInt(health);
			WriteUnsignedVarLong(armorSlotFlags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cause = ReadSignedVarInt();
			health = ReadSignedVarInt();
			armorSlotFlags = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cause=default(int);
			health=default(int);
			armorSlotFlags=default(long);
		}

	}

	public partial class McpeSetEntityMotion : Packet<McpeSetEntityMotion>
	{

		public long runtimeEntityId; // = null;
		public Vector3 velocity; // = null;

		public McpeSetEntityMotion()
		{
			Id = 0x28;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(velocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			velocity = ReadVector3();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			velocity=default(Vector3);
		}

	}

	public partial class McpeSetEntityLink : Packet<McpeSetEntityLink>
	{
		public enum LinkActions
		{
			Remove = 0,
			Ride = 1,
			Passenger = 2,
		}

		public long riddenId; // = null;
		public long riderId; // = null;
		public byte linkType; // = null;
		public bool immediate; // = null;
		public bool riderInitiated; // = null;
		public float vehicleAngularVelocity; // = null;

		public McpeSetEntityLink()
		{
			Id = 0x29;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(riddenId);
			WriteSignedVarLong(riderId);
			Write(linkType);
			Write(immediate);
			Write(riderInitiated);
			Write(vehicleAngularVelocity);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			riddenId = ReadSignedVarLong();
			riderId = ReadSignedVarLong();
			linkType = ReadByte();
			immediate = ReadBool();
			riderInitiated = ReadBool();
			vehicleAngularVelocity = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			riddenId=default(long);
			riderId=default(long);
			linkType=default(byte);
			immediate=default(bool);
			riderInitiated=default(bool);
			vehicleAngularVelocity=default(float);
		}

	}

	public partial class McpeSetHealth : Packet<McpeSetHealth>
	{

		public int health; // = null;

		public McpeSetHealth()
		{
			Id = 0x2a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(health);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			health = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			health=default(int);
		}

	}

	public partial class McpeSetSpawnPosition : Packet<McpeSetSpawnPosition>
	{

		public int spawnType; // = null;
		public BlockCoordinates coordinates; // = null;
		public int dimension; // = null;
		public BlockCoordinates unknownCoordinates; // = null;

		public McpeSetSpawnPosition()
		{
			Id = 0x2b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(spawnType);
			Write(coordinates);
			WriteSignedVarInt(dimension);
			Write(unknownCoordinates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			spawnType = ReadSignedVarInt();
			coordinates = ReadBlockCoordinates();
			dimension = ReadSignedVarInt();
			unknownCoordinates = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			spawnType=default(int);
			coordinates=default(BlockCoordinates);
			dimension=default(int);
			unknownCoordinates=default(BlockCoordinates);
		}

	}

	public partial class McpeAnimate : Packet<McpeAnimate>
	{

		public byte actionId; // = null;
		public long runtimeEntityId; // = null;

		public McpeAnimate()
		{
			Id = 0x2c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(actionId);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actionId = ReadByte();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actionId=default(byte);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeRespawn : Packet<McpeRespawn>
	{
		public enum RespawnState
		{
			Search = 0,
			Ready = 1,
			ClientReady = 2,
		}

		public float x; // = null;
		public float y; // = null;
		public float z; // = null;
		public byte state; // = null;
		public long runtimeEntityId; // = null;

		public McpeRespawn()
		{
			Id = 0x2d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(x);
			Write(y);
			Write(z);
			Write(state);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			x = ReadFloat();
			y = ReadFloat();
			z = ReadFloat();
			state = ReadByte();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			x=default(float);
			y=default(float);
			z=default(float);
			state=default(byte);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeContainerOpen : Packet<McpeContainerOpen>
	{

		public byte windowId; // = null;
		public byte type; // = null;
		public BlockCoordinates coordinates; // = null;
		public long actorUniqueId; // = null;

		public McpeContainerOpen()
		{
			Id = 0x2e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(type);
			Write(coordinates);
			WriteSignedVarLong(actorUniqueId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			type = ReadByte();
			coordinates = ReadBlockCoordinates();
			actorUniqueId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			type=default(byte);
			coordinates=default(BlockCoordinates);
			actorUniqueId=default(long);
		}

	}

	public partial class McpeContainerClose : Packet<McpeContainerClose>
	{

		public byte windowId; // = null;
		public byte windowType; // = null;
		public bool server; // = null;

		public McpeContainerClose()
		{
			Id = 0x2f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(windowType);
			Write(server);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			windowType = ReadByte();
			server = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			windowType=default(byte);
			server=default(bool);
		}

	}

	public partial class McpePlayerHotbar : Packet<McpePlayerHotbar>
	{

		public uint selectedSlot; // = null;
		public byte windowId; // = null;
		public bool selectSlot; // = null;

		public McpePlayerHotbar()
		{
			Id = 0x30;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(selectedSlot);
			Write(windowId);
			Write(selectSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			selectedSlot = ReadUnsignedVarInt();
			windowId = ReadByte();
			selectSlot = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			selectedSlot=default(uint);
			windowId=default(byte);
			selectSlot=default(bool);
		}

	}

	public partial class McpeContainerSetData : Packet<McpeContainerSetData>
	{

		public byte windowId; // = null;
		public int property; // = null;
		public int value; // = null;

		public McpeContainerSetData()
		{
			Id = 0x33;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			WriteSignedVarInt(property);
			WriteSignedVarInt(value);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			property = ReadSignedVarInt();
			value = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			property=default(int);
			value=default(int);
		}

	}

	public partial class McpeCraftingData : Packet<McpeCraftingData>
	{

		public Recipes recipes; // = null;
		public PotionTypeRecipe[] potionTypeRecipes; // = null;
		public PotionContainerChangeRecipe[] potionContainerRecipes; // = null;
		public MaterialReducerRecipe[] materialReducerRecipes; // = null;
		public bool isClean; // = null;

		public McpeCraftingData()
		{
			Id = 0x34;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(recipes);
			Write(potionTypeRecipes);
			Write(potionContainerRecipes);
			Write(materialReducerRecipes);
			Write(isClean);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			recipes = ReadRecipes();
			potionTypeRecipes = ReadPotionTypeRecipes();
			potionContainerRecipes = ReadPotionContainerChangeRecipes();
			materialReducerRecipes = ReadMaterialReducerRecipes();
			isClean = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			recipes=default(Recipes);
			potionTypeRecipes=default(PotionTypeRecipe[]);
			potionContainerRecipes=default(PotionContainerChangeRecipe[]);
			materialReducerRecipes=default(MaterialReducerRecipe[]);
			isClean=default(bool);
		}

	}

	public partial class McpeGuiDataPickItem : Packet<McpeGuiDataPickItem>
	{

		public string itemName; // = null;
		public string itemEffects; // = null;
		public int hotbarSlot; // = null;

		public McpeGuiDataPickItem()
		{
			Id = 0x36;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(itemName);
			Write(itemEffects);
			Write(hotbarSlot);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			itemName = ReadString();
			itemEffects = ReadString();
			hotbarSlot = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			itemName=default(string);
			itemEffects=default(string);
			hotbarSlot=default(int);
		}

	}

	public partial class McpeBlockEntityData : Packet<McpeBlockEntityData>
	{

		public BlockCoordinates coordinates; // = null;
		public Nbt namedtag; // = null;

		public McpeBlockEntityData()
		{
			Id = 0x38;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			namedtag=default(Nbt);
		}

	}

	public partial class McpeSetCommandsEnabled : Packet<McpeSetCommandsEnabled>
	{

		public bool enabled; // = null;

		public McpeSetCommandsEnabled()
		{
			Id = 0x3b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(enabled);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			enabled = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			enabled=default(bool);
		}

	}

	public partial class McpeSetDifficulty : Packet<McpeSetDifficulty>
	{

		public uint difficulty; // = null;

		public McpeSetDifficulty()
		{
			Id = 0x3c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(difficulty);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			difficulty = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			difficulty=default(uint);
		}

	}

	public partial class McpeChangeDimension : Packet<McpeChangeDimension>
	{

		public int dimension; // = null;
		public Vector3 position; // = null;
		public bool respawn; // = null;

		public McpeChangeDimension()
		{
			Id = 0x3d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(dimension);
			Write(position);
			Write(respawn);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			dimension = ReadSignedVarInt();
			position = ReadVector3();
			respawn = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			dimension=default(int);
			position=default(Vector3);
			respawn=default(bool);
		}

	}

	public partial class McpeSetPlayerGameType : Packet<McpeSetPlayerGameType>
	{

		public int gamemode; // = null;

		public McpeSetPlayerGameType()
		{
			Id = 0x3e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(gamemode);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			gamemode = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			gamemode=default(int);
		}

	}

	public partial class McpeSimpleEvent : Packet<McpeSimpleEvent>
	{

		public ushort eventType; // = null;

		public McpeSimpleEvent()
		{
			Id = 0x40;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(eventType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventType = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventType=default(ushort);
		}

	}

	public partial class McpeTelemetryEvent : Packet<McpeTelemetryEvent>
	{

		public long runtimeEntityId; // = null;
		public int eventData; // = null;
		public byte eventType; // = null;
		public byte[] auxData; // = null;

		public McpeTelemetryEvent()
		{
			Id = 0x41;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(eventData);
			Write(eventType);
			Write(auxData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			eventData = ReadSignedVarInt();
			eventType = ReadByte();
			auxData = ReadBytes(0, true);

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			eventData=default(int);
			eventType=default(byte);
			auxData=default(byte[]);
		}

	}

	public partial class McpeSpawnExperienceOrb : Packet<McpeSpawnExperienceOrb>
	{

		public Vector3 position; // = null;
		public int count; // = null;

		public McpeSpawnExperienceOrb()
		{
			Id = 0x42;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(position);
			WriteSignedVarInt(count);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			position = ReadVector3();
			count = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			position=default(Vector3);
			count=default(int);
		}

	}

	public partial class McpeMapInfoRequest : Packet<McpeMapInfoRequest>
	{

		public long mapId; // = null;

		public McpeMapInfoRequest()
		{
			Id = 0x44;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(mapId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			mapId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			mapId=default(long);
		}

	}

	public partial class McpeRequestChunkRadius : Packet<McpeRequestChunkRadius>
	{

		public int chunkRadius; // = null;
		public byte maxRadius; // = null;

		public McpeRequestChunkRadius()
		{
			Id = 0x45;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);
			Write(maxRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();
			maxRadius = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkRadius=default(int);
			maxRadius=default(byte);
		}

	}

	public partial class McpeChunkRadiusUpdate : Packet<McpeChunkRadiusUpdate>
	{

		public int chunkRadius; // = null;

		public McpeChunkRadiusUpdate()
		{
			Id = 0x46;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(chunkRadius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			chunkRadius = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			chunkRadius=default(int);
		}

	}

	public partial class McpeGameRulesChanged : Packet<McpeGameRulesChanged>
	{


		public McpeGameRulesChanged()
		{
			Id = 0x48;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCamera : Packet<McpeCamera>
	{

		public long unknown1; // = null;
		public long unknown2; // = null;

		public McpeCamera()
		{
			Id = 0x49;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(unknown1);
			WriteSignedVarLong(unknown2);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown1 = ReadSignedVarLong();
			unknown2 = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown1=default(long);
			unknown2=default(long);
		}

	}

	public partial class McpeShowCredits : Packet<McpeShowCredits>
	{

		public long runtimeEntityId; // = null;
		public int status; // = null;

		public McpeShowCredits()
		{
			Id = 0x4b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(status);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			status = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			status=default(int);
		}

	}

	public partial class McpeAvailableCommands : Packet<McpeAvailableCommands>
	{


		public McpeAvailableCommands()
		{
			Id = 0x4c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCommandRequest : Packet<McpeCommandRequest>
	{

		public string command; // = null;
		public CommandOriginData origin; // = null;
		public bool isInternal; // = null;
		public string version; // = null;

		public McpeCommandRequest()
		{
			Id = 0x4d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(command);
			Write(origin);
			Write(isInternal);
			Write(version);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			command = ReadString();
			origin = ReadCommandOriginData();
			isInternal = ReadBool();
			version = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			command=default(string);
			origin=default(CommandOriginData);
			isInternal=default(bool);
			version=default(string);
		}

	}

	public partial class McpeCommandBlockUpdate : Packet<McpeCommandBlockUpdate>
	{

		public bool isBlock; // = null;

		public McpeCommandBlockUpdate()
		{
			Id = 0x4e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(isBlock);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			isBlock = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			isBlock=default(bool);
		}

	}

	public partial class McpeCommandOutput : Packet<McpeCommandOutput>
	{


		public McpeCommandOutput()
		{
			Id = 0x4f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeUpdateTrade : Packet<McpeUpdateTrade>
	{

		public byte windowId; // = null;
		public byte windowType; // = null;
		public int size; // = null;
		public int tradeTier; // = null;
		public long traderEntityId; // = null;
		public long playerEntityId; // = null;
		public string displayName; // = null;
		public bool useNewTradeScreen; // = null;
		public bool usingEconomyTrade; // = null;
		public Nbt namedtag; // = null;

		public McpeUpdateTrade()
		{
			Id = 0x50;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(windowType);
			WriteSignedVarInt(size);
			WriteSignedVarInt(tradeTier);
			WriteSignedVarLong(traderEntityId);
			WriteSignedVarLong(playerEntityId);
			Write(displayName);
			Write(useNewTradeScreen);
			Write(usingEconomyTrade);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			windowType = ReadByte();
			size = ReadSignedVarInt();
			tradeTier = ReadSignedVarInt();
			traderEntityId = ReadSignedVarLong();
			playerEntityId = ReadSignedVarLong();
			displayName = ReadString();
			useNewTradeScreen = ReadBool();
			usingEconomyTrade = ReadBool();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			windowType=default(byte);
			size=default(int);
			tradeTier=default(int);
			traderEntityId=default(long);
			playerEntityId=default(long);
			displayName=default(string);
			useNewTradeScreen=default(bool);
			usingEconomyTrade=default(bool);
			namedtag=default(Nbt);
		}

	}

	public partial class McpeUpdateEquipment : Packet<McpeUpdateEquipment>
	{

		public byte windowId; // = null;
		public byte windowType; // = null;
		public int size; // = null;
		public long entityId; // = null;
		public Nbt namedtag; // = null;

		public McpeUpdateEquipment()
		{
			Id = 0x51;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(windowId);
			Write(windowType);
			WriteSignedVarInt(size);
			WriteSignedVarLong(entityId);
			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			windowId = ReadByte();
			windowType = ReadByte();
			size = ReadSignedVarInt();
			entityId = ReadSignedVarLong();
			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			windowId=default(byte);
			windowType=default(byte);
			size=default(int);
			entityId=default(long);
			namedtag=default(Nbt);
		}

	}

	public partial class McpeResourcePackDataInfo : Packet<McpeResourcePackDataInfo>
	{

		public string packageId; // = null;
		public uint maxChunkSize; // = null;
		public uint chunkCount; // = null;
		public ulong compressedPackageSize; // = null;
		public byte[] hash; // = null;
		public bool isPremium; // = null;
		public byte packType; // = null;

		public McpeResourcePackDataInfo()
		{
			Id = 0x52;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(maxChunkSize);
			Write(chunkCount);
			Write(compressedPackageSize);
			WriteByteArray(hash);
			Write(isPremium);
			Write(packType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			maxChunkSize = ReadUint();
			chunkCount = ReadUint();
			compressedPackageSize = ReadUlong();
			hash = ReadByteArray();
			isPremium = ReadBool();
			packType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			maxChunkSize=default(uint);
			chunkCount=default(uint);
			compressedPackageSize=default(ulong);
			hash=default(byte[]);
			isPremium=default(bool);
			packType=default(byte);
		}

	}

	public partial class McpeResourcePackChunkData : Packet<McpeResourcePackChunkData>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;
		public ulong progress; // = null;
		public byte[] payload; // = null;

		public McpeResourcePackChunkData()
		{
			Id = 0x53;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(chunkIndex);
			Write(progress);
			WriteByteArray(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			chunkIndex = ReadUint();
			progress = ReadUlong();
			payload = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			chunkIndex=default(uint);
			progress=default(ulong);
			payload=default(byte[]);
		}

	}

	public partial class McpeResourcePackChunkRequest : Packet<McpeResourcePackChunkRequest>
	{

		public string packageId; // = null;
		public uint chunkIndex; // = null;

		public McpeResourcePackChunkRequest()
		{
			Id = 0x54;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(packageId);
			Write(chunkIndex);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			packageId = ReadString();
			chunkIndex = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			packageId=default(string);
			chunkIndex=default(uint);
		}

	}

	public partial class McpeStopSound : Packet<McpeStopSound>
	{

		public string name; // = null;
		public bool stopAll; // = null;
		public bool stopMusicLegacy; // = null;

		public McpeStopSound()
		{
			Id = 0x57;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);
			Write(stopAll);
			Write(stopMusicLegacy);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();
			stopAll = ReadBool();
			stopMusicLegacy = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
			stopAll=default(bool);
			stopMusicLegacy=default(bool);
		}

	}

	public partial class McpeSetTitle : Packet<McpeSetTitle>
	{

		public int type; // = null;
		public string text; // = null;
		public int fadeInTime; // = null;
		public int stayTime; // = null;
		public int fadeOutTime; // = null;
		public string xuid; // = null;
		public string platformOnlineId; // = null;
		public string filteredTitleText; // = null;

		public McpeSetTitle()
		{
			Id = 0x58;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(type);
			Write(text);
			WriteSignedVarInt(fadeInTime);
			WriteSignedVarInt(stayTime);
			WriteSignedVarInt(fadeOutTime);
			Write(xuid);
			Write(platformOnlineId);
			Write(filteredTitleText);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadSignedVarInt();
			text = ReadString();
			fadeInTime = ReadSignedVarInt();
			stayTime = ReadSignedVarInt();
			fadeOutTime = ReadSignedVarInt();
			xuid = ReadString();
			platformOnlineId = ReadString();
			filteredTitleText = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(int);
			text=default(string);
			fadeInTime=default(int);
			stayTime=default(int);
			fadeOutTime=default(int);
			xuid=default(string);
			platformOnlineId=default(string);
			filteredTitleText=default(string);
		}

	}

	public partial class McpeAddBehaviorTree : Packet<McpeAddBehaviorTree>
	{

		public string behaviortree; // = null;

		public McpeAddBehaviorTree()
		{
			Id = 0x59;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(behaviortree);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			behaviortree = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			behaviortree=default(string);
		}

	}

	public partial class McpeShowStoreOffer : Packet<McpeShowStoreOffer>
	{

		public UUID offerId; // = null;
		public byte redirectType; // = null;

		public McpeShowStoreOffer()
		{
			Id = 0x5b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(offerId);
			Write(redirectType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			offerId = ReadUUID();
			redirectType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			offerId=default(UUID);
			redirectType=default(byte);
		}

	}

	public partial class McpePurchaseReceipt : Packet<McpePurchaseReceipt>
	{


		public McpePurchaseReceipt()
		{
			Id = 0x5c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeSubClientLogin : Packet<McpeSubClientLogin>
	{

		public byte[] connectionRequest; // = null;

		public McpeSubClientLogin()
		{
			Id = 0x5e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteByteArray(connectionRequest);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			connectionRequest = ReadByteArray();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			connectionRequest=default(byte[]);
		}

	}

	public partial class McpeInitiateWebSocketConnection : Packet<McpeInitiateWebSocketConnection>
	{

		public string server; // = null;

		public McpeInitiateWebSocketConnection()
		{
			Id = 0x5f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(server);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			server = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			server=default(string);
		}

	}

	public partial class McpeSetLastHurtBy : Packet<McpeSetLastHurtBy>
	{

		public int unknown; // = null;

		public McpeSetLastHurtBy()
		{
			Id = 0x60;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteVarInt(unknown);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			unknown = ReadVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			unknown=default(int);
		}

	}

	public partial class McpeBookEdit : Packet<McpeBookEdit>
	{

		public int inventorySlot; // = null;
		public uint type; // = null;

		public McpeBookEdit()
		{
			Id = 0x61;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(inventorySlot);
			WriteUnsignedVarInt(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			inventorySlot = ReadSignedVarInt();
			type = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			inventorySlot=default(int);
			type=default(uint);
		}

	}

	public partial class McpeNpcRequest : Packet<McpeNpcRequest>
	{

		public long runtimeEntityId; // = null;
		public byte unknown0; // = null;
		public string unknown1; // = null;
		public byte unknown2; // = null;
		public string sceneName; // = null;

		public McpeNpcRequest()
		{
			Id = 0x62;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(unknown0);
			Write(unknown1);
			Write(unknown2);
			Write(sceneName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			unknown0 = ReadByte();
			unknown1 = ReadString();
			unknown2 = ReadByte();
			sceneName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			unknown0=default(byte);
			unknown1=default(string);
			unknown2=default(byte);
			sceneName=default(string);
		}

	}

	public partial class McpeModalFormRequest : Packet<McpeModalFormRequest>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeModalFormRequest()
		{
			Id = 0x64;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeModalFormResponse : Packet<McpeModalFormResponse>
	{

		public uint formId; // = null;

		public McpeModalFormResponse()
		{
			Id = 0x65;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(formId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
		}

	}

	public partial class McpeServerSettingsRequest : Packet<McpeServerSettingsRequest>
	{


		public McpeServerSettingsRequest()
		{
			Id = 0x66;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerSettingsResponse : Packet<McpeServerSettingsResponse>
	{

		public uint formId; // = null;
		public string data; // = null;

		public McpeServerSettingsResponse()
		{
			Id = 0x67;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(formId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUnsignedVarInt();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
			data=default(string);
		}

	}

	public partial class McpeShowProfile : Packet<McpeShowProfile>
	{

		public string xuid; // = null;

		public McpeShowProfile()
		{
			Id = 0x68;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(xuid);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			xuid = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			xuid=default(string);
		}

	}

	public partial class McpeSetDefaultGameType : Packet<McpeSetDefaultGameType>
	{

		public int gamemode; // = null;

		public McpeSetDefaultGameType()
		{
			Id = 0x69;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(gamemode);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			gamemode = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			gamemode=default(int);
		}

	}

	public partial class McpeSetDisplayObjective : Packet<McpeSetDisplayObjective>
	{

		public string displaySlot; // = null;
		public string objectiveName; // = null;
		public string displayName; // = null;
		public string criteriaName; // = null;
		public int sortOrder; // = null;

		public McpeSetDisplayObjective()
		{
			Id = 0x6b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(displaySlot);
			Write(objectiveName);
			Write(displayName);
			Write(criteriaName);
			WriteSignedVarInt(sortOrder);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			displaySlot = ReadString();
			objectiveName = ReadString();
			displayName = ReadString();
			criteriaName = ReadString();
			sortOrder = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			displaySlot=default(string);
			objectiveName=default(string);
			displayName=default(string);
			criteriaName=default(string);
			sortOrder=default(int);
		}

	}

	public partial class McpeLabTable : Packet<McpeLabTable>
	{

		public byte uselessByte; // = null;
		public int labTableX; // = null;
		public int labTableY; // = null;
		public int labTableZ; // = null;
		public byte reactionType; // = null;

		public McpeLabTable()
		{
			Id = 0x6d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(uselessByte);
			WriteSignedVarInt(labTableX);
			WriteSignedVarInt(labTableY);
			WriteSignedVarInt(labTableZ);
			Write(reactionType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			uselessByte = ReadByte();
			labTableX = ReadSignedVarInt();
			labTableY = ReadSignedVarInt();
			labTableZ = ReadSignedVarInt();
			reactionType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			uselessByte=default(byte);
			labTableX=default(int);
			labTableY=default(int);
			labTableZ=default(int);
			reactionType=default(byte);
		}

	}

	public partial class McpeUpdateBlockSynced : Packet<McpeUpdateBlockSynced>
	{

		public BlockCoordinates coordinates; // = null;
		public uint blockRuntimeId; // = null;
		public uint blockPriority; // = null;
		public uint dataLayerId; // = null;
		public long unknown0; // = null;
		public long unknown1; // = null;

		public McpeUpdateBlockSynced()
		{
			Id = 0x6e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(blockRuntimeId);
			WriteUnsignedVarInt(blockPriority);
			WriteUnsignedVarInt(dataLayerId);
			WriteUnsignedVarLong(unknown0);
			WriteUnsignedVarLong(unknown1);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			blockRuntimeId = ReadUnsignedVarInt();
			blockPriority = ReadUnsignedVarInt();
			dataLayerId = ReadUnsignedVarInt();
			unknown0 = ReadUnsignedVarLong();
			unknown1 = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			blockRuntimeId=default(uint);
			blockPriority=default(uint);
			dataLayerId=default(uint);
			unknown0=default(long);
			unknown1=default(long);
		}

	}

	public partial class McpeSetLocalPlayerAsInitialized : Packet<McpeSetLocalPlayerAsInitialized>
	{

		public long runtimeEntityId; // = null;

		public McpeSetLocalPlayerAsInitialized()
		{
			Id = 0x71;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
		}

	}

	public partial class McpeUpdateSoftEnum : Packet<McpeUpdateSoftEnum>
	{


		public McpeUpdateSoftEnum()
		{
			Id = 0x72;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeNetworkStackLatency : Packet<McpeNetworkStackLatency>
	{

		public ulong timestamp; // = null;
		public byte unknownFlag; // = null;

		public McpeNetworkStackLatency()
		{
			Id = 0x73;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(timestamp);
			Write(unknownFlag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			timestamp = ReadUlong();
			unknownFlag = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			timestamp=default(ulong);
			unknownFlag=default(byte);
		}

	}

	public partial class McpeSpawnParticleEffect : Packet<McpeSpawnParticleEffect>
	{

		public byte dimensionId; // = null;
		public long entityId; // = null;
		public Vector3 position; // = null;
		public string particleName; // = null;

		public McpeSpawnParticleEffect()
		{
			Id = 0x76;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(dimensionId);
			WriteSignedVarLong(entityId);
			Write(position);
			Write(particleName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			dimensionId = ReadByte();
			entityId = ReadSignedVarLong();
			position = ReadVector3();
			particleName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			dimensionId=default(byte);
			entityId=default(long);
			position=default(Vector3);
			particleName=default(string);
		}

	}

	public partial class McpeAvailableEntityIdentifiers : Packet<McpeAvailableEntityIdentifiers>
	{

		public Nbt namedtag; // = null;

		public McpeAvailableEntityIdentifiers()
		{
			Id = 0x77;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			namedtag=default(Nbt);
		}

	}

	public partial class McpeNetworkChunkPublisherUpdate : Packet<McpeNetworkChunkPublisherUpdate>
	{

		public BlockCoordinates coordinates; // = null;
		public uint radius; // = null;

		public McpeNetworkChunkPublisherUpdate()
		{
			Id = 0x79;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(coordinates);
			WriteUnsignedVarInt(radius);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			coordinates = ReadBlockCoordinates();
			radius = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			coordinates=default(BlockCoordinates);
			radius=default(uint);
		}

	}

	public partial class McpeBiomeDefinitionList : Packet<McpeBiomeDefinitionList>
	{


		public McpeBiomeDefinitionList()
		{
			Id = 0x7a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeLevelSoundEvent : Packet<McpeLevelSoundEvent>
	{

		public string soundId; // = null;
		public Vector3 position; // = null;
		public int blockId; // = null;
		public string entityType; // = null;
		public bool isBabyMob; // = null;
		public bool isGlobal; // = null;

		public McpeLevelSoundEvent()
		{
			Id = 0x7b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(soundId);
			Write(position);
			WriteSignedVarInt(blockId);
			Write(entityType);
			Write(isBabyMob);
			Write(isGlobal);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			soundId = ReadString();
			position = ReadVector3();
			blockId = ReadSignedVarInt();
			entityType = ReadString();
			isBabyMob = ReadBool();
			isGlobal = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			soundId=default(string);
			position=default(Vector3);
			blockId=default(int);
			entityType=default(string);
			isBabyMob=default(bool);
			isGlobal=default(bool);
		}

	}

	public partial class McpeLevelEventGeneric : Packet<McpeLevelEventGeneric>
	{

		public int eventId; // = null;
		public NbtCompound eventData; // = null;

		public McpeLevelEventGeneric()
		{
			Id = 0x7c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(eventId);
			WriteNbtBody(eventData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			eventId = ReadSignedVarInt();
			eventData = ReadNbtBody();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			eventId=default(int);
			eventData=default(NbtCompound);
		}

	}

	public partial class McpeLecternUpdate : Packet<McpeLecternUpdate>
	{

		public byte page; // = null;
		public byte totalPages; // = null;
		public BlockCoordinates blockPosition; // = null;

		public McpeLecternUpdate()
		{
			Id = 0x7d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(page);
			Write(totalPages);
			Write(blockPosition);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			page = ReadByte();
			totalPages = ReadByte();
			blockPosition = ReadBlockCoordinates();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			page=default(byte);
			totalPages=default(byte);
			blockPosition=default(BlockCoordinates);
		}

	}

	public partial class McpeClientCacheStatus : Packet<McpeClientCacheStatus>
	{

		public bool enabled; // = null;

		public McpeClientCacheStatus()
		{
			Id = 0x81;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(enabled);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			enabled = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			enabled=default(bool);
		}

	}

	public partial class McpeOnScreenTextureAnimation : Packet<McpeOnScreenTextureAnimation>
	{

		public uint effectId; // = null;

		public McpeOnScreenTextureAnimation()
		{
			Id = 0x82;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(effectId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			effectId = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			effectId=default(uint);
		}

	}

	public partial class McpeMapCreateLockedCopy : Packet<McpeMapCreateLockedCopy>
	{

		public long originalMapId; // = null;
		public long newMapId; // = null;

		public McpeMapCreateLockedCopy()
		{
			Id = 0x83;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(originalMapId);
			WriteSignedVarLong(newMapId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			originalMapId = ReadSignedVarLong();
			newMapId = ReadSignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			originalMapId=default(long);
			newMapId=default(long);
		}

	}

	public partial class McpeStructureTemplateDataExportRequest : Packet<McpeStructureTemplateDataExportRequest>
	{

		public string name; // = null;
		public BlockCoordinates position; // = null;
		public StructureSettings settings; // = null;
		public byte requestType; // = null;

		public McpeStructureTemplateDataExportRequest()
		{
			Id = 0x84;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);
			Write(position);
			Write(settings);
			Write(requestType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();
			position = ReadBlockCoordinates();
			settings = ReadStructureSettings();
			requestType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
			position=default(BlockCoordinates);
			settings=default(StructureSettings);
			requestType=default(byte);
		}

	}

	public partial class McpeStructureTemplateDataExportResponse : Packet<McpeStructureTemplateDataExportResponse>
	{

		public string name; // = null;

		public McpeStructureTemplateDataExportResponse()
		{
			Id = 0x85;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
		}

	}

	public partial class McpeClientCacheBlobStatus : Packet<McpeClientCacheBlobStatus>
	{


		public McpeClientCacheBlobStatus()
		{
			Id = 0x87;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientCacheMissResponse : Packet<McpeClientCacheMissResponse>
	{


		public McpeClientCacheMissResponse()
		{
			Id = 0x88;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeEducationSettings : Packet<McpeEducationSettings>
	{

		public string codeBuilderDefaultUri; // = null;
		public string codeBuilderTitle; // = null;
		public bool canResizeCodeBuilder; // = null;
		public bool disableLegacyTitleBar; // = null;
		public string postProcessFilter; // = null;
		public string screenshotBorderResourcePath; // = null;

		public McpeEducationSettings()
		{
			Id = 0x89;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(codeBuilderDefaultUri);
			Write(codeBuilderTitle);
			Write(canResizeCodeBuilder);
			Write(disableLegacyTitleBar);
			Write(postProcessFilter);
			Write(screenshotBorderResourcePath);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			codeBuilderDefaultUri = ReadString();
			codeBuilderTitle = ReadString();
			canResizeCodeBuilder = ReadBool();
			disableLegacyTitleBar = ReadBool();
			postProcessFilter = ReadString();
			screenshotBorderResourcePath = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			codeBuilderDefaultUri=default(string);
			codeBuilderTitle=default(string);
			canResizeCodeBuilder=default(bool);
			disableLegacyTitleBar=default(bool);
			postProcessFilter=default(string);
			screenshotBorderResourcePath=default(string);
		}

	}

	public partial class McpeEmote : Packet<McpeEmote>
	{

		public long runtimeEntityId; // = null;
		public string emoteId; // = null;
		public uint emoteLengthTicks; // = null;
		public string xboxUserId; // = null;
		public string platformChatId; // = null;
		public byte flags; // = null;

		public McpeEmote()
		{
			Id = 0x8a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(emoteId);
			WriteUnsignedVarInt(emoteLengthTicks);
			Write(xboxUserId);
			Write(platformChatId);
			Write(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			emoteId = ReadString();
			emoteLengthTicks = ReadUnsignedVarInt();
			xboxUserId = ReadString();
			platformChatId = ReadString();
			flags = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			emoteId=default(string);
			emoteLengthTicks=default(uint);
			xboxUserId=default(string);
			platformChatId=default(string);
			flags=default(byte);
		}

	}

	public partial class McpeMultiplayerSettings : Packet<McpeMultiplayerSettings>
	{

		public int action; // = null;

		public McpeMultiplayerSettings()
		{
			Id = 0x8b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(action);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			action = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			action=default(int);
		}

	}

	public partial class McpeSettingsCommand : Packet<McpeSettingsCommand>
	{

		public string command; // = null;
		public bool suppressOutput; // = null;

		public McpeSettingsCommand()
		{
			Id = 0x8c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(command);
			Write(suppressOutput);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			command = ReadString();
			suppressOutput = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			command=default(string);
			suppressOutput=default(bool);
		}

	}

	public partial class McpeCompletedUsingItem : Packet<McpeCompletedUsingItem>
	{

		public short itemId; // = null;
		public int action; // = null;

		public McpeCompletedUsingItem()
		{
			Id = 0x8e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(itemId);
			Write(action);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			itemId = ReadShort();
			action = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			itemId=default(short);
			action=default(int);
		}

	}

	public partial class McpeNetworkSettings : Packet<McpeNetworkSettings>
	{
		public enum Compressionalgorithm
		{
			Zlib = 0,
			Snappy = 1,
			None = 65535,
		}

		public ushort compressionThreshold; // = null;
		public ushort compressionAlgorithm; // = null;
		public bool clientThrottleEnabled; // = null;
		public byte clientThrottleThreshold; // = null;
		public float clientThrottleScalar; // = null;

		public McpeNetworkSettings()
		{
			Id = 0x8f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(compressionThreshold);
			Write(compressionAlgorithm);
			Write(clientThrottleEnabled);
			Write(clientThrottleThreshold);
			Write(clientThrottleScalar);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			compressionThreshold = ReadUshort();
			compressionAlgorithm = ReadUshort();
			clientThrottleEnabled = ReadBool();
			clientThrottleThreshold = ReadByte();
			clientThrottleScalar = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			compressionThreshold=default(ushort);
			compressionAlgorithm=default(ushort);
			clientThrottleEnabled=default(bool);
			clientThrottleThreshold=default(byte);
			clientThrottleScalar=default(float);
		}

	}

	public partial class McpePlayerEnchantOptions : Packet<McpePlayerEnchantOptions>
	{

		public EnchantOptions enchantOptions; // = null;

		public McpePlayerEnchantOptions()
		{
			Id = 0x92;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(enchantOptions);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			enchantOptions = ReadEnchantOptions();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			enchantOptions=default(EnchantOptions);
		}

	}

	public partial class McpePlayerArmorDamage : Packet<McpePlayerArmorDamage>
	{


		public McpePlayerArmorDamage()
		{
			Id = 0x95;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCodeBuilder : Packet<McpeCodeBuilder>
	{

		public string url; // = null;
		public bool openCodeBuilder; // = null;

		public McpeCodeBuilder()
		{
			Id = 0x96;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(url);
			Write(openCodeBuilder);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			url = ReadString();
			openCodeBuilder = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			url=default(string);
			openCodeBuilder=default(bool);
		}

	}

	public partial class McpeUpdatePlayerGameType : Packet<McpeUpdatePlayerGameType>
	{

		public int playerGameType; // = null;
		public long targetPlayerUniqueId; // = null;
		public long tick; // = null;

		public McpeUpdatePlayerGameType()
		{
			Id = 0x97;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(playerGameType);
			WriteSignedVarLong(targetPlayerUniqueId);
			WriteUnsignedVarLong(tick);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			playerGameType = ReadSignedVarInt();
			targetPlayerUniqueId = ReadSignedVarLong();
			tick = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			playerGameType=default(int);
			targetPlayerUniqueId=default(long);
			tick=default(long);
		}

	}

	public partial class McpeEmoteList : Packet<McpeEmoteList>
	{

		public long runtimeEntityId; // = null;

		public McpeEmoteList()
		{
			Id = 0x98;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
		}

	}

	public partial class McpePositionTrackingDbServerBroadcast : Packet<McpePositionTrackingDbServerBroadcast>
	{

		public byte action; // = null;
		public int trackingId; // = null;
		public NbtCompound nbt; // = null;

		public McpePositionTrackingDbServerBroadcast()
		{
			Id = 0x99;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(action);
			WriteSignedVarInt(trackingId);
			WriteNbtBody(nbt);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			action = ReadByte();
			trackingId = ReadSignedVarInt();
			nbt = ReadNbtBody();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			action=default(byte);
			trackingId=default(int);
			nbt=default(NbtCompound);
		}

	}

	public partial class McpePositionTrackingDbClientRequest : Packet<McpePositionTrackingDbClientRequest>
	{

		public byte action; // = null;
		public int trackingId; // = null;

		public McpePositionTrackingDbClientRequest()
		{
			Id = 0x9a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(action);
			WriteSignedVarInt(trackingId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			action = ReadByte();
			trackingId = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			action=default(byte);
			trackingId=default(int);
		}

	}

	public partial class McpeDebugInfo : Packet<McpeDebugInfo>
	{

		public long actorUniqueId; // = null;
		public string data; // = null;

		public McpeDebugInfo()
		{
			Id = 0x9b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(actorUniqueId);
			Write(data);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actorUniqueId = ReadSignedVarLong();
			data = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actorUniqueId=default(long);
			data=default(string);
		}

	}

	public partial class McpePacketViolationWarning : Packet<McpePacketViolationWarning>
	{

		public int violationType; // = null;
		public int severity; // = null;
		public int packetId; // = null;
		public string reason; // = null;

		public McpePacketViolationWarning()
		{
			Id = 0x9c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(violationType);
			WriteSignedVarInt(severity);
			WriteSignedVarInt(packetId);
			Write(reason);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			violationType = ReadSignedVarInt();
			severity = ReadSignedVarInt();
			packetId = ReadSignedVarInt();
			reason = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			violationType=default(int);
			severity=default(int);
			packetId=default(int);
			reason=default(string);
		}

	}

	public partial class McpeMotionPredictionHints : Packet<McpeMotionPredictionHints>
	{

		public long runtimeEntityId; // = null;
		public Vector3 motion; // = null;
		public bool onGround; // = null;

		public McpeMotionPredictionHints()
		{
			Id = 0x9d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			Write(motion);
			Write(onGround);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			motion = ReadVector3();
			onGround = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			motion=default(Vector3);
			onGround=default(bool);
		}

	}

	public partial class McpeAnimateEntity : Packet<McpeAnimateEntity>
	{

		public string animation; // = null;
		public string nextState; // = null;
		public string stopExpression; // = null;
		public int stopExpressionVersion; // = null;
		public string controller; // = null;
		public float blendOutTime; // = null;

		public McpeAnimateEntity()
		{
			Id = 0x9e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(animation);
			Write(nextState);
			Write(stopExpression);
			Write(stopExpressionVersion);
			Write(controller);
			Write(blendOutTime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			animation = ReadString();
			nextState = ReadString();
			stopExpression = ReadString();
			stopExpressionVersion = ReadInt();
			controller = ReadString();
			blendOutTime = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			animation=default(string);
			nextState=default(string);
			stopExpression=default(string);
			stopExpressionVersion=default(int);
			controller=default(string);
			blendOutTime=default(float);
		}

	}

	public partial class McpePlayerFog : Packet<McpePlayerFog>
	{


		public McpePlayerFog()
		{
			Id = 0xa0;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCorrectPlayerMovePrediction : Packet<McpeCorrectPlayerMovePrediction>
	{

		public byte predictionType; // = null;
		public Vector3 position; // = null;
		public Vector3 delta; // = null;
		public float rotationPitch; // = null;
		public float rotationYaw; // = null;

		public McpeCorrectPlayerMovePrediction()
		{
			Id = 0xa1;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(predictionType);
			Write(position);
			Write(delta);
			Write(rotationPitch);
			Write(rotationYaw);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			predictionType = ReadByte();
			position = ReadVector3();
			delta = ReadVector3();
			rotationPitch = ReadFloat();
			rotationYaw = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			predictionType=default(byte);
			position=default(Vector3);
			delta=default(Vector3);
			rotationPitch=default(float);
			rotationYaw=default(float);
		}

	}

	public partial class McpeItemComponent : Packet<McpeItemComponent>
	{

		public ItemComponentList entries; // = null;

		public McpeItemComponent()
		{
			Id = 0xa2;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(entries);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entries = ReadItemComponentList();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entries=default(ItemComponentList);
		}

	}

	public partial class McpeClientboundDebugRenderer : Packet<McpeClientboundDebugRenderer>
	{

		public string type; // = null;

		public McpeClientboundDebugRenderer()
		{
			Id = 0xa4;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(string);
		}

	}

	public partial class McpeSyncEntityProperty : Packet<McpeSyncEntityProperty>
	{

		public Nbt namedtag; // = null;

		public McpeSyncEntityProperty()
		{
			Id = 0xa5;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(namedtag);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			namedtag = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			namedtag=default(Nbt);
		}

	}

	public partial class McpeAddVolumeEntity : Packet<McpeAddVolumeEntity>
	{

		public uint entityNetworkId; // = null;
		public Nbt data; // = null;
		public string jsonIdentifier; // = null;
		public string instanceName; // = null;
		public BlockCoordinates minBounds; // = null;
		public BlockCoordinates maxBounds; // = null;
		public int dimension; // = null;
		public string engineVersion; // = null;

		public McpeAddVolumeEntity()
		{
			Id = 0xa6;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(entityNetworkId);
			Write(data);
			Write(jsonIdentifier);
			Write(instanceName);
			Write(minBounds);
			Write(maxBounds);
			WriteSignedVarInt(dimension);
			Write(engineVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityNetworkId = ReadUnsignedVarInt();
			data = ReadNbt();
			jsonIdentifier = ReadString();
			instanceName = ReadString();
			minBounds = ReadBlockCoordinates();
			maxBounds = ReadBlockCoordinates();
			dimension = ReadSignedVarInt();
			engineVersion = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityNetworkId=default(uint);
			data=default(Nbt);
			jsonIdentifier=default(string);
			instanceName=default(string);
			minBounds=default(BlockCoordinates);
			maxBounds=default(BlockCoordinates);
			dimension=default(int);
			engineVersion=default(string);
		}

	}

	public partial class McpeRemoveVolumeEntity : Packet<McpeRemoveVolumeEntity>
	{

		public uint entityNetworkId; // = null;
		public int dimension; // = null;

		public McpeRemoveVolumeEntity()
		{
			Id = 0xa7;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(entityNetworkId);
			WriteSignedVarInt(dimension);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityNetworkId = ReadUnsignedVarInt();
			dimension = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityNetworkId=default(uint);
			dimension=default(int);
		}

	}

	public partial class McpeSimulationType : Packet<McpeSimulationType>
	{

		public byte simulationType; // = null;

		public McpeSimulationType()
		{
			Id = 0xa8;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(simulationType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			simulationType = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			simulationType=default(byte);
		}

	}

	public partial class McpeNpcDialogue : Packet<McpeNpcDialogue>
	{

		public long npcUniqueId; // = null;
		public int actionType; // = null;
		public string dialogue; // = null;
		public string sceneName; // = null;
		public string npcName; // = null;
		public string actionJson; // = null;

		public McpeNpcDialogue()
		{
			Id = 0xa9;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteLe(npcUniqueId);
			WriteSignedVarInt(actionType);
			Write(dialogue);
			Write(sceneName);
			Write(npcName);
			Write(actionJson);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			npcUniqueId = ReadLongLe();
			actionType = ReadSignedVarInt();
			dialogue = ReadString();
			sceneName = ReadString();
			npcName = ReadString();
			actionJson = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			npcUniqueId=default(long);
			actionType=default(int);
			dialogue=default(string);
			sceneName=default(string);
			npcName=default(string);
			actionJson=default(string);
		}

	}

	public partial class McpeEduUriResource : Packet<McpeEduUriResource>
	{

		public string buttonName; // = null;
		public string linkUri; // = null;

		public McpeEduUriResource()
		{
			Id = 0xaa;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(buttonName);
			Write(linkUri);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			buttonName = ReadString();
			linkUri = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			buttonName=default(string);
			linkUri=default(string);
		}

	}

	public partial class McpeCreatePhoto : Packet<McpeCreatePhoto>
	{

		public long entityUniqueId; // = null;
		public string photoName; // = null;
		public string photoItemName; // = null;

		public McpeCreatePhoto()
		{
			Id = 0xab;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteLe(entityUniqueId);
			Write(photoName);
			Write(photoItemName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityUniqueId = ReadLongLe();
			photoName = ReadString();
			photoItemName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityUniqueId=default(long);
			photoName=default(string);
			photoItemName=default(string);
		}

	}

	public partial class McpeUpdateSubChunkBlocksPacket : Packet<McpeUpdateSubChunkBlocksPacket>
	{

		public BlockCoordinates subchunkCoordinates; // = null;
		public UpdateSubChunkBlocksPacketEntry[] layerZeroUpdates; // = null;
		public UpdateSubChunkBlocksPacketEntry[] layerOneUpdates; // = null;

		public McpeUpdateSubChunkBlocksPacket()
		{
			Id = 0xac;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(subchunkCoordinates);
			Write(layerZeroUpdates);
			Write(layerOneUpdates);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			subchunkCoordinates = ReadBlockCoordinates();
			layerZeroUpdates = ReadUpdateSubChunkBlocksPacketEntrys();
			layerOneUpdates = ReadUpdateSubChunkBlocksPacketEntrys();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			subchunkCoordinates=default(BlockCoordinates);
			layerZeroUpdates=default(UpdateSubChunkBlocksPacketEntry[]);
			layerOneUpdates=default(UpdateSubChunkBlocksPacketEntry[]);
		}

	}

	public partial class McpeSubChunkRequestPacket : Packet<McpeSubChunkRequestPacket>
	{


		public McpeSubChunkRequestPacket()
		{
			Id = 0xaf;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpePlayerStartItemCooldown : Packet<McpePlayerStartItemCooldown>
	{

		public string itemCategory; // = null;
		public int cooldownTicks; // = null;

		public McpePlayerStartItemCooldown()
		{
			Id = 0xb0;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(itemCategory);
			WriteSignedVarInt(cooldownTicks);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			itemCategory = ReadString();
			cooldownTicks = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			itemCategory=default(string);
			cooldownTicks=default(int);
		}

	}

	public partial class McpeScriptMessage : Packet<McpeScriptMessage>
	{

		public string messageId; // = null;
		public string messageValue; // = null;

		public McpeScriptMessage()
		{
			Id = 0xb1;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(messageId);
			Write(messageValue);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			messageId = ReadString();
			messageValue = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			messageId=default(string);
			messageValue=default(string);
		}

	}

	public partial class McpeCodeBuilderSource : Packet<McpeCodeBuilderSource>
	{

		public byte operation; // = null;
		public byte category; // = null;
		public byte codeStatus; // = null;

		public McpeCodeBuilderSource()
		{
			Id = 0xb2;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(operation);
			Write(category);
			Write(codeStatus);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			operation = ReadByte();
			category = ReadByte();
			codeStatus = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			operation=default(byte);
			category=default(byte);
			codeStatus=default(byte);
		}

	}

	public partial class McpeTickingAreasLoadStatus : Packet<McpeTickingAreasLoadStatus>
	{

		public bool waitingForPreload; // = null;

		public McpeTickingAreasLoadStatus()
		{
			Id = 0xb3;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(waitingForPreload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			waitingForPreload = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			waitingForPreload=default(bool);
		}

	}

	public partial class McpeAgentActionEvent : Packet<McpeAgentActionEvent>
	{

		public string requestId; // = null;
		public int action; // = null;
		public string responseJson; // = null;

		public McpeAgentActionEvent()
		{
			Id = 0xb5;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(requestId);
			Write(action);
			Write(responseJson);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			requestId = ReadString();
			action = ReadInt();
			responseJson = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			requestId=default(string);
			action=default(int);
			responseJson=default(string);
		}

	}

	public partial class McpeChangeMobProperty : Packet<McpeChangeMobProperty>
	{

		public long actorUniqueId; // = null;
		public string propertyName; // = null;
		public bool boolValue; // = null;
		public string stringValue; // = null;
		public int intValue; // = null;
		public float floatValue; // = null;

		public McpeChangeMobProperty()
		{
			Id = 0xb6;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarLong(actorUniqueId);
			Write(propertyName);
			Write(boolValue);
			Write(stringValue);
			WriteSignedVarInt(intValue);
			Write(floatValue);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actorUniqueId = ReadSignedVarLong();
			propertyName = ReadString();
			boolValue = ReadBool();
			stringValue = ReadString();
			intValue = ReadSignedVarInt();
			floatValue = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actorUniqueId=default(long);
			propertyName=default(string);
			boolValue=default(bool);
			stringValue=default(string);
			intValue=default(int);
			floatValue=default(float);
		}

	}

	public partial class McpeLessonProgress : Packet<McpeLessonProgress>
	{

		public int action; // = null;
		public int score; // = null;
		public string activityId; // = null;

		public McpeLessonProgress()
		{
			Id = 0xb7;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(action);
			WriteSignedVarInt(score);
			Write(activityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			action = ReadSignedVarInt();
			score = ReadSignedVarInt();
			activityId = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			action=default(int);
			score=default(int);
			activityId=default(string);
		}

	}

	public partial class McpeRequestPermissions : Packet<McpeRequestPermissions>
	{

		public long targetActorUniqueId; // = null;
		public int playerPermission; // = null;
		public ushort customFlags; // = null;

		public McpeRequestPermissions()
		{
			Id = 0xb9;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteLe(targetActorUniqueId);
			WriteSignedVarInt(playerPermission);
			Write(customFlags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			targetActorUniqueId = ReadLongLe();
			playerPermission = ReadSignedVarInt();
			customFlags = ReadUshort();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			targetActorUniqueId=default(long);
			playerPermission=default(int);
			customFlags=default(ushort);
		}

	}

	public partial class McpeToastRequest : Packet<McpeToastRequest>
	{

		public string title; // = null;
		public string content; // = null;

		public McpeToastRequest()
		{
			Id = 0xba;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(title);
			Write(content);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			title = ReadString();
			content = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			title=default(string);
			content=default(string);
		}

	}

	public partial class McpeUpdateAbilities : Packet<McpeUpdateAbilities>
	{

		public long entityUniqueId; // = null;
		public byte permissionLevel; // = null;
		public byte commandPermission; // = null;

		public McpeUpdateAbilities()
		{
			Id = 0xbb;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteLe(entityUniqueId);
			Write(permissionLevel);
			Write(commandPermission);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			entityUniqueId = ReadLongLe();
			permissionLevel = ReadByte();
			commandPermission = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			entityUniqueId=default(long);
			permissionLevel=default(byte);
			commandPermission=default(byte);
		}

	}

	public partial class McpeUpdateAdventureSettings : Packet<McpeUpdateAdventureSettings>
	{

		public bool noPvm; // = null;
		public bool noMvp; // = null;
		public bool immutableWorld; // = null;
		public bool showNameTags; // = null;
		public bool autoJump; // = null;

		public McpeUpdateAdventureSettings()
		{
			Id = 0xbc;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(noPvm);
			Write(noMvp);
			Write(immutableWorld);
			Write(showNameTags);
			Write(autoJump);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			noPvm = ReadBool();
			noMvp = ReadBool();
			immutableWorld = ReadBool();
			showNameTags = ReadBool();
			autoJump = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			noPvm=default(bool);
			noMvp=default(bool);
			immutableWorld=default(bool);
			showNameTags=default(bool);
			autoJump=default(bool);
		}

	}

	public partial class McpeDeathInfo : Packet<McpeDeathInfo>
	{

		public string cause; // = null;

		public McpeDeathInfo()
		{
			Id = 0xbd;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(cause);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cause = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cause=default(string);
		}

	}

	public partial class McpeEditorNetwork : Packet<McpeEditorNetwork>
	{

		public bool routeToManager; // = null;
		public NbtCompound payload; // = null;

		public McpeEditorNetwork()
		{
			Id = 0xbe;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(routeToManager);
			WriteNbtBody(payload);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			routeToManager = ReadBool();
			payload = ReadNbtBody();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			routeToManager=default(bool);
			payload=default(NbtCompound);
		}

	}

	public partial class McpeFeatureRegistry : Packet<McpeFeatureRegistry>
	{


		public McpeFeatureRegistry()
		{
			Id = 0xbf;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerStats : Packet<McpeServerStats>
	{

		public float serverTime; // = null;
		public float networkTime; // = null;

		public McpeServerStats()
		{
			Id = 0xc0;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(serverTime);
			Write(networkTime);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			serverTime = ReadFloat();
			networkTime = ReadFloat();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			serverTime=default(float);
			networkTime=default(float);
		}

	}

	public partial class McpeRequestNetworkSettings : Packet<McpeRequestNetworkSettings>
	{

		public int protocolVersion; // = null;

		public McpeRequestNetworkSettings()
		{
			Id = 0xc1;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteBe(protocolVersion);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			protocolVersion = ReadIntBe();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			protocolVersion=default(int);
		}

	}

	public partial class McpeGameTestRequest : Packet<McpeGameTestRequest>
	{

		public int maxTestsPerBatch; // = null;
		public int repeatCount; // = null;
		public byte rotation; // = null;
		public bool stopOnFailure; // = null;
		public BlockCoordinates testPosition; // = null;
		public int testsPerRow; // = null;
		public string testName; // = null;

		public McpeGameTestRequest()
		{
			Id = 0xc2;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(maxTestsPerBatch);
			WriteSignedVarInt(repeatCount);
			Write(rotation);
			Write(stopOnFailure);
			Write(testPosition);
			WriteSignedVarInt(testsPerRow);
			Write(testName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			maxTestsPerBatch = ReadSignedVarInt();
			repeatCount = ReadSignedVarInt();
			rotation = ReadByte();
			stopOnFailure = ReadBool();
			testPosition = ReadBlockCoordinates();
			testsPerRow = ReadSignedVarInt();
			testName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			maxTestsPerBatch=default(int);
			repeatCount=default(int);
			rotation=default(byte);
			stopOnFailure=default(bool);
			testPosition=default(BlockCoordinates);
			testsPerRow=default(int);
			testName=default(string);
		}

	}

	public partial class McpeGameTestResults : Packet<McpeGameTestResults>
	{

		public bool success; // = null;
		public string error; // = null;
		public string testName; // = null;

		public McpeGameTestResults()
		{
			Id = 0xc3;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(success);
			Write(error);
			Write(testName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			success = ReadBool();
			error = ReadString();
			testName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			success=default(bool);
			error=default(string);
			testName=default(string);
		}

	}

	public partial class McpeUpdateClientInputLocks : Packet<McpeUpdateClientInputLocks>
	{

		public uint flags; // = null;

		public McpeUpdateClientInputLocks()
		{
			Id = 0xc4;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(flags);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			flags = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			flags=default(uint);
		}

	}

	public partial class McpeUnlockedRecipes : Packet<McpeUnlockedRecipes>
	{

		public uint type; // = null;

		public McpeUnlockedRecipes()
		{
			Id = 0xc7;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(uint);
		}

	}

	public partial class McpeTrimData : Packet<McpeTrimData>
	{


		public McpeTrimData()
		{
			Id = 0x12e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeOpenSign : Packet<McpeOpenSign>
	{

		public BlockCoordinates blockPosition; // = null;
		public bool front; // = null;

		public McpeOpenSign()
		{
			Id = 0x12f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(blockPosition);
			Write(front);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			blockPosition = ReadBlockCoordinates();
			front = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			blockPosition=default(BlockCoordinates);
			front=default(bool);
		}

	}

	public partial class McpeAgentAnimation : Packet<McpeAgentAnimation>
	{

		public byte animationType; // = null;
		public long runtimeEntityId; // = null;

		public McpeAgentAnimation()
		{
			Id = 0x130;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(animationType);
			WriteUnsignedVarLong(runtimeEntityId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			animationType = ReadByte();
			runtimeEntityId = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			animationType=default(byte);
			runtimeEntityId=default(long);
		}

	}

	public partial class McpeRefreshEntitlements : Packet<McpeRefreshEntitlements>
	{


		public McpeRefreshEntitlements()
		{
			Id = 0x131;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpePlayerToggleCrafterSlotRequest : Packet<McpePlayerToggleCrafterSlotRequest>
	{

		public int posX; // = null;
		public int posY; // = null;
		public int posZ; // = null;
		public byte slotIndex; // = null;
		public bool isDisabled; // = null;

		public McpePlayerToggleCrafterSlotRequest()
		{
			Id = 0x132;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(posX);
			Write(posY);
			Write(posZ);
			Write(slotIndex);
			Write(isDisabled);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			posX = ReadInt();
			posY = ReadInt();
			posZ = ReadInt();
			slotIndex = ReadByte();
			isDisabled = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			posX=default(int);
			posY=default(int);
			posZ=default(int);
			slotIndex=default(byte);
			isDisabled=default(bool);
		}

	}

	public partial class McpeSetPlayerInventoryOptions : Packet<McpeSetPlayerInventoryOptions>
	{

		public int leftTab; // = null;
		public int rightTab; // = null;
		public bool filtering; // = null;
		public int layout; // = null;
		public int craftingLayout; // = null;

		public McpeSetPlayerInventoryOptions()
		{
			Id = 0x133;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(leftTab);
			WriteSignedVarInt(rightTab);
			Write(filtering);
			WriteSignedVarInt(layout);
			WriteSignedVarInt(craftingLayout);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			leftTab = ReadSignedVarInt();
			rightTab = ReadSignedVarInt();
			filtering = ReadBool();
			layout = ReadSignedVarInt();
			craftingLayout = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			leftTab=default(int);
			rightTab=default(int);
			filtering=default(bool);
			layout=default(int);
			craftingLayout=default(int);
		}

	}

	public partial class McpeSetHud : Packet<McpeSetHud>
	{


		public McpeSetHud()
		{
			Id = 0x134;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeAwardAchievement : Packet<McpeAwardAchievement>
	{

		public int achievementId; // = null;

		public McpeAwardAchievement()
		{
			Id = 0x135;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(achievementId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			achievementId = ReadInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			achievementId=default(int);
		}

	}

	public partial class McpeClientboundCloseForm : Packet<McpeClientboundCloseForm>
	{


		public McpeClientboundCloseForm()
		{
			Id = 0x136;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerBoundLoadingScreen : Packet<McpeServerBoundLoadingScreen>
	{

		public int type; // = null;

		public McpeServerBoundLoadingScreen()
		{
			Id = 0x138;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteSignedVarInt(type);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			type = ReadSignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			type=default(int);
		}

	}

	public partial class McpeJigsawStructureData : Packet<McpeJigsawStructureData>
	{

		public Nbt structureData; // = null;

		public McpeJigsawStructureData()
		{
			Id = 0x139;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(structureData);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			structureData = ReadNbt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			structureData=default(Nbt);
		}

	}

	public partial class McpeCurrentStructureFeature : Packet<McpeCurrentStructureFeature>
	{

		public string currentFeature; // = null;

		public McpeCurrentStructureFeature()
		{
			Id = 0x13a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(currentFeature);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			currentFeature = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			currentFeature=default(string);
		}

	}

	public partial class McpeCameraAimAssist : Packet<McpeCameraAimAssist>
	{

		public string presetId; // = null;
		public Vector2 viewAngle; // = null;
		public float distance; // = null;
		public byte targetMode; // = null;
		public byte actionType; // = null;
		public bool showDebugRender; // = null;

		public McpeCameraAimAssist()
		{
			Id = 0x13c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(presetId);
			Write(viewAngle);
			Write(distance);
			Write(targetMode);
			Write(actionType);
			Write(showDebugRender);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			presetId = ReadString();
			viewAngle = ReadVector2();
			distance = ReadFloat();
			targetMode = ReadByte();
			actionType = ReadByte();
			showDebugRender = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			presetId=default(string);
			viewAngle=default(Vector2);
			distance=default(float);
			targetMode=default(byte);
			actionType=default(byte);
			showDebugRender=default(bool);
		}

	}

	public partial class McpeContainerRegistryCleanup : Packet<McpeContainerRegistryCleanup>
	{


		public McpeContainerRegistryCleanup()
		{
			Id = 0x13d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeMovementEffect : Packet<McpeMovementEffect>
	{

		public long runtimeEntityId; // = null;
		public uint effectType; // = null;
		public uint duration; // = null;
		public long tick; // = null;

		public McpeMovementEffect()
		{
			Id = 0x13e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarLong(runtimeEntityId);
			WriteUnsignedVarInt(effectType);
			WriteUnsignedVarInt(duration);
			WriteUnsignedVarLong(tick);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			runtimeEntityId = ReadUnsignedVarLong();
			effectType = ReadUnsignedVarInt();
			duration = ReadUnsignedVarInt();
			tick = ReadUnsignedVarLong();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			runtimeEntityId=default(long);
			effectType=default(uint);
			duration=default(uint);
			tick=default(long);
		}

	}

	public partial class McpeClientCameraAimAssist : Packet<McpeClientCameraAimAssist>
	{

		public string presetId; // = null;
		public byte action; // = null;
		public bool allowAimAssist; // = null;

		public McpeClientCameraAimAssist()
		{
			Id = 0x141;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(presetId);
			Write(action);
			Write(allowAimAssist);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			presetId = ReadString();
			action = ReadByte();
			allowAimAssist = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			presetId=default(string);
			action=default(byte);
			allowAimAssist=default(bool);
		}

	}

	public partial class McpeCameraAimAssistPresets : Packet<McpeCameraAimAssistPresets>
	{


		public McpeCameraAimAssistPresets()
		{
			Id = 0x140;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientMovementPredictionSync : Packet<McpeClientMovementPredictionSync>
	{

		public float scale; // = null;
		public float width; // = null;
		public float height; // = null;
		public float movementSpeed; // = null;
		public float underwaterMovementSpeed; // = null;
		public float lavaMovementSpeed; // = null;
		public float jumpStrength; // = null;
		public float health; // = null;
		public float hunger; // = null;
		public float frictionModifier; // = null;
		public float bounciness; // = null;
		public float airDragModifier; // = null;
		public long actorUniqueId; // = null;
		public bool actorFlyingState; // = null;

		public McpeClientMovementPredictionSync()
		{
			Id = 0x142;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(scale);
			Write(width);
			Write(height);
			Write(movementSpeed);
			Write(underwaterMovementSpeed);
			Write(lavaMovementSpeed);
			Write(jumpStrength);
			Write(health);
			Write(hunger);
			Write(frictionModifier);
			Write(bounciness);
			Write(airDragModifier);
			WriteSignedVarLong(actorUniqueId);
			Write(actorFlyingState);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			scale = ReadFloat();
			width = ReadFloat();
			height = ReadFloat();
			movementSpeed = ReadFloat();
			underwaterMovementSpeed = ReadFloat();
			lavaMovementSpeed = ReadFloat();
			jumpStrength = ReadFloat();
			health = ReadFloat();
			hunger = ReadFloat();
			frictionModifier = ReadFloat();
			bounciness = ReadFloat();
			airDragModifier = ReadFloat();
			actorUniqueId = ReadSignedVarLong();
			actorFlyingState = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			scale=default(float);
			width=default(float);
			height=default(float);
			movementSpeed=default(float);
			underwaterMovementSpeed=default(float);
			lavaMovementSpeed=default(float);
			jumpStrength=default(float);
			health=default(float);
			hunger=default(float);
			frictionModifier=default(float);
			bounciness=default(float);
			airDragModifier=default(float);
			actorUniqueId=default(long);
			actorFlyingState=default(bool);
		}

	}

	public partial class McpeUpdateClientOptions : Packet<McpeUpdateClientOptions>
	{


		public McpeUpdateClientOptions()
		{
			Id = 0x143;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpePlayerVideoCapture : Packet<McpePlayerVideoCapture>
	{

		public bool recording; // = null;

		public McpePlayerVideoCapture()
		{
			Id = 0x144;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(recording);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			recording = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			recording=default(bool);
		}

	}

	public partial class McpeClientboundControlSchemeSet : Packet<McpeClientboundControlSchemeSet>
	{

		public byte controlScheme; // = null;

		public McpeClientboundControlSchemeSet()
		{
			Id = 0x147;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(controlScheme);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			controlScheme = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			controlScheme=default(byte);
		}

	}

	public partial class McpePrimitiveShapes : Packet<McpePrimitiveShapes>
	{


		public McpePrimitiveShapes()
		{
			Id = 0x148;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientboundDataStore : Packet<McpeClientboundDataStore>
	{


		public McpeClientboundDataStore()
		{
			Id = 0x14a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeGraphicsOverrideParameter : Packet<McpeGraphicsOverrideParameter>
	{


		public McpeGraphicsOverrideParameter()
		{
			Id = 0x14b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerboundDataStore : Packet<McpeServerboundDataStore>
	{

		public string name; // = null;
		public string property; // = null;
		public string path; // = null;

		public McpeServerboundDataStore()
		{
			Id = 0x14c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(name);
			Write(property);
			Write(path);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			name = ReadString();
			property = ReadString();
			path = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			name=default(string);
			property=default(string);
			path=default(string);
		}

	}

	public partial class McpeClientboundDataDrivenUiShowScreen : Packet<McpeClientboundDataDrivenUiShowScreen>
	{

		public string screenId; // = null;
		public uint formId; // = null;

		public McpeClientboundDataDrivenUiShowScreen()
		{
			Id = 0x14d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(screenId);
			Write(formId);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			screenId = ReadString();
			formId = ReadUint();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			screenId=default(string);
			formId=default(uint);
		}

	}

	public partial class McpeClientboundDataDrivenUiCloseScreen : Packet<McpeClientboundDataDrivenUiCloseScreen>
	{


		public McpeClientboundDataDrivenUiCloseScreen()
		{
			Id = 0x14e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientboundDataDrivenUiReload : Packet<McpeClientboundDataDrivenUiReload>
	{


		public McpeClientboundDataDrivenUiReload()
		{
			Id = 0x14f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeClientboundTextureShift : Packet<McpeClientboundTextureShift>
	{

		public byte actionId; // = null;
		public string collectionName; // = null;
		public string fromStep; // = null;
		public string toStep; // = null;

		public McpeClientboundTextureShift()
		{
			Id = 0x150;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(actionId);
			Write(collectionName);
			Write(fromStep);
			Write(toStep);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			actionId = ReadByte();
			collectionName = ReadString();
			fromStep = ReadString();
			toStep = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			actionId=default(byte);
			collectionName=default(string);
			fromStep=default(string);
			toStep=default(string);
		}

	}

	public partial class McpeVoxelShapes : Packet<McpeVoxelShapes>
	{


		public McpeVoxelShapes()
		{
			Id = 0x151;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCameraSpline : Packet<McpeCameraSpline>
	{


		public McpeCameraSpline()
		{
			Id = 0x152;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCameraAimAssistActorPriority : Packet<McpeCameraAimAssistActorPriority>
	{


		public McpeCameraAimAssistActorPriority()
		{
			Id = 0x153;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeResourcePacksReadyForValidation : Packet<McpeResourcePacksReadyForValidation>
	{


		public McpeResourcePacksReadyForValidation()
		{
			Id = 0x154;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCameraInstruction : Packet<McpeCameraInstruction>
	{


		public McpeCameraInstruction()
		{
			Id = 0x12c;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeCameraShake : Packet<McpeCameraShake>
	{

		public float intensity; // = null;
		public float duration; // = null;
		public byte type; // = null;
		public byte action; // = null;

		public McpeCameraShake()
		{
			Id = 0x9f;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(intensity);
			Write(duration);
			Write(type);
			Write(action);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			intensity = ReadFloat();
			duration = ReadFloat();
			type = ReadByte();
			action = ReadByte();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			intensity=default(float);
			duration=default(float);
			type=default(byte);
			action=default(byte);
		}

	}

	public partial class McpeLocatorBar : Packet<McpeLocatorBar>
	{


		public McpeLocatorBar()
		{
			Id = 0x155;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpePartyChanged : Packet<McpePartyChanged>
	{

		public string partyId; // = null;
		public bool partyLeader; // = null;

		public McpePartyChanged()
		{
			Id = 0x156;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(partyId);
			Write(partyLeader);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			partyId = ReadString();
			partyLeader = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			partyId=default(string);
			partyLeader=default(bool);
		}

	}

	public partial class McpeServerboundDataDrivenScreenClosed : Packet<McpeServerboundDataDrivenScreenClosed>
	{

		public uint formId; // = null;
		public string closeReason; // = null;

		public McpeServerboundDataDrivenScreenClosed()
		{
			Id = 0x157;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(formId);
			Write(closeReason);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			formId = ReadUint();
			closeReason = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			formId=default(uint);
			closeReason=default(string);
		}

	}

	public partial class McpeSyncWorldClocks : Packet<McpeSyncWorldClocks>
	{

		public uint payloadType; // = null;

		public McpeSyncWorldClocks()
		{
			Id = 0x158;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(payloadType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			payloadType = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			payloadType=default(uint);
		}

	}

	public partial class McpeClientboundAttributeLayerSync : Packet<McpeClientboundAttributeLayerSync>
	{

		public uint payloadType; // = null;

		public McpeClientboundAttributeLayerSync()
		{
			Id = 0x159;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			WriteUnsignedVarInt(payloadType);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			payloadType = ReadUnsignedVarInt();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			payloadType=default(uint);
		}

	}

	public partial class McpeServerStoreInfo : Packet<McpeServerStoreInfo>
	{


		public McpeServerStoreInfo()
		{
			Id = 0x15a;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeServerPresenceInfo : Packet<McpeServerPresenceInfo>
	{


		public McpeServerPresenceInfo()
		{
			Id = 0x15b;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class McpeSendPartyDestinationCookie : Packet<McpeSendPartyDestinationCookie>
	{

		public string cookie; // = null;
		public string intent; // = null;
		public string destinationName; // = null;

		public McpeSendPartyDestinationCookie()
		{
			Id = 0x15d;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(cookie);
			Write(intent);
			Write(destinationName);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cookie = ReadString();
			intent = ReadString();
			destinationName = ReadString();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cookie=default(string);
			intent=default(string);
			destinationName=default(string);
		}

	}

	public partial class McpePartyDestinationCookieResponse : Packet<McpePartyDestinationCookieResponse>
	{

		public string cookie; // = null;
		public bool accepted; // = null;

		public McpePartyDestinationCookieResponse()
		{
			Id = 0x15e;
			IsMcpe = true;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(cookie);
			Write(accepted);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			cookie = ReadString();
			accepted = ReadBool();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			cookie=default(string);
			accepted=default(bool);
		}

	}

	public partial class McpeWrapper : Packet<McpeWrapper>
	{


		public McpeWrapper()
		{
			Id = 0xfe;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();


			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();


			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

		}

	}

	public partial class FtlCreatePlayer : Packet<FtlCreatePlayer>
	{

		public string username; // = null;
		public UUID clientuuid; // = null;
		public string serverAddress; // = null;
		public long clientId; // = null;
		public Skin skin; // = null;

		public FtlCreatePlayer()
		{
			Id = 0x01;
			IsMcpe = false;
		}

		protected override void EncodePacket()
		{
			base.EncodePacket();

			BeforeEncode();

			Write(username);
			Write(clientuuid);
			Write(serverAddress);
			Write(clientId);
			Write(skin);

			AfterEncode();
		}

		partial void BeforeEncode();
		partial void AfterEncode();

		protected override void DecodePacket()
		{
			base.DecodePacket();

			BeforeDecode();

			username = ReadString();
			clientuuid = ReadUUID();
			serverAddress = ReadString();
			clientId = ReadLong();
			skin = ReadSkin();

			AfterDecode();
		}

		partial void BeforeDecode();
		partial void AfterDecode();

		protected override void ResetPacket()
		{
			base.ResetPacket();

			username=default(string);
			clientuuid=default(UUID);
			serverAddress=default(string);
			clientId=default(long);
			skin=default(Skin);
		}

	}

}

