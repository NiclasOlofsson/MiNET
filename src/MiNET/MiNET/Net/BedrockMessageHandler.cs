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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using log4net;
using MiNET.Net.RakNet;
using MiNET.Utils;

namespace MiNET.Net
{
	public class BedrockMessageHandler : ICustomMessageHandler
	{
		private readonly RakSession _session;
		private static readonly ILog Log = LogManager.GetLogger(typeof(BedrockMessageHandler));

		public IMcpeMessageHandler Handler { get; set; }

		public BedrockMessageHandler(RakSession session, IServerManager serverManager)
		{
			_session = session;
			Handler = new LoginMessageHandler(this, session, serverManager);
		}

		public CryptoContext CryptoContext { get; set; }

		public void Connected()
		{
		}

		public void Disconnect(string reason, bool sendDisconnect = true)
		{
			Handler.Disconnect(reason, sendDisconnect);
		}

		public void HandlePacket(Packet message)
		{
			HandleCustomPacket(Handler, message);
		}


		private void HandleCustomPacket(IMcpeMessageHandler handler, Packet message)
		{
			if (message is McpeWrapper wrapper)
			{
				var messages = new List<Packet>();

				// Get bytes to process
				ReadOnlyMemory<byte> payload = wrapper.payload;

				// Decrypt bytes

				if (CryptoContext != null && CryptoContext.UseEncryption)
				{
					// This call copies the entire buffer, but what can we do? It is kind of compensated by not
					// creating a new buffer when parsing the packet (only a mem-slice)
					payload = CryptoUtils.Decrypt(payload, CryptoContext);
				}

				// Decompress bytes

				var stream = new MemoryStreamReader(payload.Slice(0, payload.Length - 4)); // slice away adler
				if (stream.ReadByte() != 0x78)
				{
					if (Log.IsDebugEnabled) Log.Error($"Incorrect ZLib header. Expected 0x78 0x9C 0x{wrapper.Id:X2}\n{Packet.HexDump(wrapper.payload)}");
					if (Log.IsDebugEnabled) Log.Error($"Incorrect ZLib header. Decrypted 0x{wrapper.Id:X2}\n{Packet.HexDump(payload)}");
					throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
				}
				stream.ReadByte();
				using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress, false))
				{
					using var s = new MemoryStream();
					deflateStream.CopyTo(s);
					s.Position = 0;

					// Get actual packet out of bytes
					while (s.Position < s.Length)
					{
						int len = (int) VarInt.ReadUInt32(s);
						Memory<byte> internalBuffer = new byte[len];
						s.Read(internalBuffer.Span);
						int id = internalBuffer.Span[0];
						internalBuffer = internalBuffer.Slice(id > 127 ? 2 : 1); //TODO: This is stupid. Get rid of the id slicing

						//if (Log.IsDebugEnabled)
						//	Log.Debug($"0x{internalBuffer[0]:x2}\n{Packet.HexDump(internalBuffer)}");

						try
						{
							messages.Add(PacketFactory.Create((byte) id, internalBuffer, "mcpe") ??
										new UnknownPacket((byte) id, internalBuffer));
						}
						catch (Exception)
						{
							if (Log.IsDebugEnabled) Log.Warn($"Error parsing packet 0x{wrapper.Id:X2}\n{Packet.HexDump(internalBuffer)}");
							throw;
						}
					}

					if (s.Length > s.Position) throw new Exception("Have more data");
				}

				foreach (Packet msg in messages)
				{
					// Temp fix for performance, take 1.
					var interact = msg as McpeInteract;
					if (interact?.actionId == 4 && interact.targetRuntimeEntityId == 0) continue;

					msg.ReliabilityHeader = new ReliabilityHeader()
					{
						Reliability = wrapper.ReliabilityHeader.Reliability,
						ReliableMessageNumber = wrapper.ReliabilityHeader.ReliableMessageNumber,
						OrderingChannel = wrapper.ReliabilityHeader.OrderingChannel,
						OrderingIndex = wrapper.ReliabilityHeader.OrderingIndex,
					};

					HandleBedrockMessage(handler, msg);
				}

				wrapper.PutPool();
			}
			else if (message is UnknownPacket unknownPacket)
			{
				if (Log.IsDebugEnabled) Log.Warn($"Received unknown packet 0x{unknownPacket.Id:X2}\n{Packet.HexDump(unknownPacket.Message)}");

				unknownPacket.PutPool();
			}
			else
			{
				Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {_session.Username}, IP {_session.EndPoint.Address}");
				if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
			}
		}

		private void HandleBedrockMessage(IMcpeMessageHandler handler, Packet message)
		{
			RakProcessor.TraceReceive(message);

			//if (handler is Player player)
			//{
			//	Packet result = Server.PluginManager.PluginPacketHandler(message, true, player);
			//	if (result != message) message.PutPool();
			//	message = result;
			//}

			switch (message)
			{
				case McpeClientToServerHandshake msg:
					// Start encryption
					handler.HandleMcpeClientToServerHandshake(msg);
					break;
				case McpeResourcePackClientResponse msg:
					handler.HandleMcpeResourcePackClientResponse(msg);
					break;
				case McpeResourcePackChunkRequest msg:
					handler.HandleMcpeResourcePackChunkRequest(msg);
					break;
				case McpeSetLocalPlayerAsInitializedPacket msg:
					handler.HandleMcpeSetLocalPlayerAsInitializedPacket(msg);
					break;
				case McpeScriptCustomEventPacket msg:
					handler.HandleMcpeScriptCustomEventPacket(msg);
					break;
				case McpeUpdateBlock _:
					// DO NOT USE. Will dissapear from MCPE any release. 
					// It is a bug that it leaks these messages.
					break;
				case McpeLevelSoundEvent msg:
					handler.HandleMcpeLevelSoundEvent(msg);
					break;
				case McpeClientCacheStatus msg:
					handler.HandleMcpeClientCacheStatus(msg);
					break;
				case McpeAnimate msg:
					handler.HandleMcpeAnimate(msg);
					break;
				case McpeEntityFall msg:
					handler.HandleMcpeEntityFall(msg);
					break;
				case McpeEntityEvent msg:
					handler.HandleMcpeEntityEvent(msg);
					break;
				case McpeText msg:
					handler.HandleMcpeText(msg);
					break;
				case McpeRemoveEntity _:
					// Do nothing right now, but should clear out the entities and stuff
					// from this players internal structure.
					break;
				case McpeLogin msg:
					handler.HandleMcpeLogin(msg);
					break;
				case McpeMovePlayer msg:
					handler.HandleMcpeMovePlayer(msg);
					break;
				case McpeInteract msg:
					handler.HandleMcpeInteract(msg);
					break;
				case McpeRespawn msg:
					handler.HandleMcpeRespawn(msg);
					break;
				case McpeBlockEntityData msg:
					handler.HandleMcpeBlockEntityData(msg);
					break;
				case McpeAdventureSettings msg:
					handler.HandleMcpeAdventureSettings(msg);
					break;
				case McpePlayerAction msg:
					handler.HandleMcpePlayerAction(msg);
					break;
				case McpeContainerClose msg:
					handler.HandleMcpeContainerClose(msg);
					break;
				case McpeMobEquipment msg:
					handler.HandleMcpeMobEquipment(msg);
					break;
				case McpeMobArmorEquipment msg:
					handler.HandleMcpeMobArmorEquipment(msg);
					break;
				case McpeCraftingEvent msg:
					handler.HandleMcpeCraftingEvent(msg);
					break;
				case McpeInventoryTransaction msg:
					handler.HandleMcpeInventoryTransaction(msg);
					break;
				case McpeServerSettingsRequest msg:
					handler.HandleMcpeServerSettingsRequest(msg);
					break;
				case McpeSetPlayerGameType msg:
					handler.HandleMcpeSetPlayerGameType(msg);
					break;
				case McpePlayerHotbar msg:
					handler.HandleMcpePlayerHotbar(msg);
					break;
				case McpeInventoryContent msg:
					handler.HandleMcpeInventoryContent(msg);
					break;
				case McpeRequestChunkRadius msg:
					handler.HandleMcpeRequestChunkRadius(msg);
					break;
				case McpeMapInfoRequest msg:
					handler.HandleMcpeMapInfoRequest(msg);
					break;
				case McpeItemFrameDropItem msg:
					handler.HandleMcpeItemFrameDropItem(msg);
					break;
				case McpePlayerInput msg:
					handler.HandleMcpePlayerInput(msg);
					break;
				case McpeRiderJump msg:
					handler.HandleMcpeRiderJump(msg);
					break;
				case McpeCommandRequest msg:
					handler.HandleMcpeCommandRequest(msg);
					break;
				case McpeBlockPickRequest msg:
					handler.HandleMcpeBlockPickRequest(msg);
					break;
				case McpeEntityPickRequest msg:
					handler.HandleMcpeEntityPickRequest(msg);
					break;
				case McpeModalFormResponse msg:
					handler.HandleMcpeModalFormResponse(msg);
					break;
				case McpeCommandBlockUpdate msg:
					handler.HandleMcpeCommandBlockUpdate(msg);
					break;
				case McpeMoveEntity msg:
					handler.HandleMcpeMoveEntity(msg);
					break;
				case McpeSetEntityMotion msg:
					handler.HandleMcpeSetEntityMotion(msg);
					break;
				case McpePhotoTransfer msg:
					handler.HandleMcpePhotoTransfer(msg);
					break;
				case McpeSetEntityData msg:
					handler.HandleMcpeSetEntityData(msg);
					break;
				case McpeTickSync msg:
					handler.HandleMcpeTickSync(msg);
					break;
				case McpeNpcRequest msg:
					handler.HandleMcpeNpcRequest(msg);
					break;
				case McpeNetworkStackLatencyPacket msg:
					handler.HandleMcpeNetworkStackLatencyPacket(msg);
					break;
				default:
				{
					Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {_session.Username}, IP {_session.EndPoint.Address}");
					if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
					break;
				}
			}
		}
	}
}