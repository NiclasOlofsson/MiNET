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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2021 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using fNbt;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.IO;
using MiNET.Utils.Skins;
using Newtonsoft.Json.Linq;

namespace MiNET
{
	public class LoginMessageHandler : IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LoginMessageHandler));

		private readonly BedrockMessageHandler _bedrockHandler;
		private readonly INetworkHandler _session;
		private readonly IServerManager _serverManager;

		private object _loginSyncLock = new object();
		private PlayerInfo _playerInfo = new PlayerInfo();

		public LoginMessageHandler(BedrockMessageHandler bedrockHandler, INetworkHandler session, IServerManager serverManager)
		{
			_bedrockHandler = bedrockHandler;
			_session = session;
			_serverManager = serverManager;
		}

		public void Disconnect(string reason, bool sendDisconnect = true)
		{
		}

		public virtual void HandleMcpeRequestNetworkSettings(McpeRequestNetworkSettings message)
		{
			_playerInfo.ProtocolVersion = message.protocolVersion;

			var settings = McpeNetworkSettings.CreateObject();
			// The same rule both directions: at or below the threshold a batch ships raw under the
			// 0xff compressor id. Advertising 1 here would order the client to deflate every tiny
			// input packet it sends us; advertising the real threshold spares both sides.
			settings.compressionThreshold = Compression.CompressionThresholdBytes;
			settings.compressionAlgorithm = (ushort) McpeNetworkSettings.Compressionalgorithm.Zlib;
			settings.clientThrottleEnabled = false;
			settings.clientThrottleThreshold = 0;
			settings.clientThrottleScalar = 0;

			// Pre-wrap the reply so the async send queue can't compress it. This is the one payload
			// that carries neither compression nor a compressor id byte: the client only starts
			// expecting them with the packet after this one. Compression starts with the first
			// packet after this one, in both directions.
			var wrapper = McpeWrapper.CreateObject();
			wrapper.SetPayload(Compression.PackPacketsForWrapper([settings]));
			wrapper.EncodeAsMemory();
			_session.SendPacket(wrapper);
		}

		public virtual void HandleMcpeLogin(McpeLogin message)
		{
			// Only one login!
			lock (_loginSyncLock)
			{
				if (_session.Username != null)
				{
					Log.Info($"Player {_session.Username} doing multiple logins");
					return; // Already doing login
				}

				_session.Username = string.Empty;
			}

			_playerInfo.ProtocolVersion = message.protocolVersion;
			//_playerInfo.Edition = message.edition;

			DecodeCert(message);

			////string fileName = Path.GetTempPath() + "Skin_" + Skin.SkinType + ".png";
			////Log.Info($"Writing skin to filename: {fileName}");
			////Skin.SaveTextureToFile(fileName, Skin.Texture);
		}

		/// <summary>
		///     The player UUID for an Xbox-authenticated client, whose login token carries an XUID but
		///     no identity GUID. This is not ours to invent: the client already knows what UUID it has
		///     and matches the player list against it, so a made-up one makes every entry we send be
		///     somebody else. Vanilla derives an RFC 4122 v3 (MD5) UUID from the XUID, and this
		///     reproduces the exact value BDS 1.26.34 put on the wire for a live client (PMMP's
		///     calculateUuidFromXuid does the same).
		/// </summary>
		private static Guid DeriveUuidFromXuid(string xuid)
		{
			if (string.IsNullOrEmpty(xuid)) return Guid.NewGuid();

			byte[] hash = System.Security.Cryptography.MD5.HashData(Encoding.UTF8.GetBytes("pocket-auth-1-xuid:" + xuid));
			hash[6] = (byte) ((hash[6] & 0x0f) | 0x30); // version 3
			hash[8] = (byte) ((hash[8] & 0x3f) | 0x80); // variant RFC 4122

			// Guid(byte[]) reads the first three groups little-endian; the UUID is big-endian.
			return new Guid(
				(hash[0] << 24) | (hash[1] << 16) | (hash[2] << 8) | hash[3],
				(short) ((hash[4] << 8) | hash[5]),
				(short) ((hash[6] << 8) | hash[7]),
				hash[8], hash[9], hash[10], hash[11], hash[12], hash[13], hash[14], hash[15]);
		}

		public void DecodeCert(McpeLogin message)
		{
			byte[] buffer = message.payload;

			if (message.payload.Length != buffer.Length)
			{
				Log.Debug($"Wrong lenght {message.payload.Length} != {message.payload.Length}");
				throw new Exception($"Wrong lenght {message.payload.Length} != {message.payload.Length}");
			}

			if (Log.IsDebugEnabled) Log.Debug("Lenght: " + message.payload.Length + ", Message: " + buffer.EncodeBase64());

			string certificateChain;
			string skinData;

			try
			{
				var destination = new MemoryStream(buffer);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, false);

				var countCertData = reader.ReadInt32();
				certificateChain = Encoding.UTF8.GetString(reader.ReadBytes(countCertData));
				if (Log.IsDebugEnabled) Log.Debug($"Certificate Chain (Lenght={countCertData})\n{certificateChain}");

				var countSkinData = reader.ReadInt32();
				skinData = Encoding.UTF8.GetString(reader.ReadBytes(countSkinData));
				if (Log.IsDebugEnabled) Log.Debug($"Skin data (Lenght={countSkinData})\n{skinData}");
			}
			catch (Exception e)
			{
				Log.Error("Parsing login", e);
				return;
			}

			try
			{
				{
					IDictionary<string, dynamic> headers = JWT.Headers(skinData);
					string clientDataJson = JWT.Payload(skinData);
					ClientData clientData = ClientData.FromJson(clientDataJson);

					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Header: {string.Join(";", headers)}");
					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Payload:\n{clientDataJson}");

					// Reverse-engineering aid: dump the full clientData JSON of a real client so
					// the bot's login (CryptoUtils.EncodeSkinJwt) can be mirrored field by field.
					string dumpDir = Environment.GetEnvironmentVariable("MINET_DUMP_CLIENTDATA");
					if (!string.IsNullOrEmpty(dumpDir))
					{
						System.IO.Directory.CreateDirectory(dumpDir);
						System.IO.File.WriteAllText(System.IO.Path.Combine(dumpDir, $"clientdata-{DateTime.UtcNow:HHmmss}.json"), clientDataJson);
					}

					// Skin JWT Payload: 

					//{
					//	"CapeData": "",
					//	"ClientRandomId": 1423700530444426768,
					//	"CurrentInputMode": 1,
					//	"DefaultInputMode": 1,
					//	"DeviceModel": "ASUSTeK COMPUTER INC. N550JK",
					//	"DeviceOS": 7,
					//	"GameVersion": "1.2.0.18",
					//	"GuiScale": 0,
					//	"LanguageCode": "en_US",
					//	"ServerAddress": "yodamine.com:19132",
					//	"SkinData": "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",
					//	"SkinGeometry": "ew0KICAiZ2VvbWV0cnkuaHVtYW5vaWQiOiB7DQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJib2R5IiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTQuMCwgMTIuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDgsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDE2LCAxNiBdDQogICAgICAgICAgfQ0KICAgICAgICBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogIndhaXN0IiwNCiAgICAgICAgIm5ldmVyUmVuZGVyIjogdHJ1ZSwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDEyLjAsIDAuMCBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImhlYWQiLA0KICAgICAgICAicGl2b3QiOiBbIDAuMCwgMjQuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtNC4wLCAyNC4wLCAtNC4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgOCwgOCwgOCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAwIF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAiaGF0IiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTQuMCwgMjQuMCwgLTQuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDgsIDgsIDggXSwNCiAgICAgICAgICAgICJ1diI6IFsgMzIsIDAgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC41DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiB0cnVlDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0QXJtIiwNCiAgICAgICAgInBpdm90IjogWyAtNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC04LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyA0MCwgMTYgXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInBpdm90IjogWyA1LjAsIDIyLjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgNC4wLCAxMi4wLCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgNCwgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtaXJyb3IiOiB0cnVlDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0TGVnIiwNCiAgICAgICAgInBpdm90IjogWyAtMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0zLjksIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAibGVmdExlZyIsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtaXJyb3IiOiB0cnVlDQogICAgICB9DQogICAgXQ0KICB9LA0KDQogICJnZW9tZXRyeS5jYXBlIjogew0KICAgICJ0ZXh0dXJld2lkdGgiOiA2NCwNCiAgICAidGV4dHVyZWhlaWdodCI6IDMyLA0KDQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJjYXBlIiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIC0zLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC01LjAsIDguMCwgLTMuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDEwLCAxNiwgMSBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAwIF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0NCiAgICBdDQogIH0sDQogICJnZW9tZXRyeS5odW1hbm9pZC5jdXN0b206Z2VvbWV0cnkuaHVtYW5vaWQiOiB7DQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJoYXQiLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiBmYWxzZSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdDQogICAgICB9LA0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDUuMCwgMjIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyA0LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAzMiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJyaWdodEFybSIsDQogICAgICAgICJyZXNldCI6IHRydWUsDQogICAgICAgICJwaXZvdCI6IFsgLTUuMCwgMjIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtOC4wLCAxMi4wLCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgNCwgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRJdGVtIiwNCiAgICAgICAgInBpdm90IjogWyAtNiwgMTUsIDEgXSwNCiAgICAgICAgIm5ldmVyUmVuZGVyIjogdHJ1ZSwNCiAgICAgICAgInBhcmVudCI6ICJyaWdodEFybSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAibGVmdFNsZWV2ZSIsDQogICAgICAgICJwaXZvdCI6IFsgNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIDQuMCwgMTIuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDQ4LCA0OCBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0U2xlZXZlIiwNCiAgICAgICAgInBpdm90IjogWyAtNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC04LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyA0MCwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0TGVnIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMC4xLCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0UGFudHMiLA0KICAgICAgICAicGl2b3QiOiBbIDEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMC4xLCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCA0OCBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAicG9zIjogWyAxLjksIDEyLCAwIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgLTEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMy45LCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAicG9zIjogWyAtMS45LCAxMiwgMCBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImphY2tldCIsDQogICAgICAgICJwaXZvdCI6IFsgMC4wLCAyNC4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA4LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfQ0KICAgIF0NCiAgfSwNCiAgImdlb21ldHJ5Lmh1bWFub2lkLmN1c3RvbVNsaW06Z2VvbWV0cnkuaHVtYW5vaWQiOiB7DQoNCiAgICAiYm9uZXMiOiBbDQogICAgICB7DQogICAgICAgICJuYW1lIjogImhhdCIsDQogICAgICAgICJuZXZlclJlbmRlciI6IGZhbHNlLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDUuMCwgMjEuNSwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyA0LjAsIDExLjUsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyAzLCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAzMiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJyaWdodEFybSIsDQogICAgICAgICJyZXNldCI6IHRydWUsDQogICAgICAgICJwaXZvdCI6IFsgLTUuMCwgMjEuNSwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtNy4wLCAxMS41LCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgMywgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgInBpdm90IjogWyAtNiwgMTQuNSwgMSBdLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiB0cnVlLA0KICAgICAgICAibmFtZSI6ICJyaWdodEl0ZW0iLA0KICAgICAgICAicGFyZW50IjogInJpZ2h0QXJtIg0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0U2xlZXZlIiwNCiAgICAgICAgInBpdm90IjogWyA1LjAsIDIxLjUsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgNC4wLCAxMS41LCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgMywgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDgsIDQ4IF0sDQogICAgICAgICAgICAiaW5mbGF0ZSI6IDAuMjUNCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRTbGVldmUiLA0KICAgICAgICAicGl2b3QiOiBbIC01LjAsIDIxLjUsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTcuMCwgMTEuNSwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDMsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDQwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImxlZnRMZWciLA0KICAgICAgICAicmVzZXQiOiB0cnVlLA0KICAgICAgICAibWlycm9yIjogZmFsc2UsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDE2LCA0OCBdDQogICAgICAgICAgfQ0KICAgICAgICBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImxlZnRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDQ4IF0sDQogICAgICAgICAgICAiaW5mbGF0ZSI6IDAuMjUNCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgLTEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMy45LCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImphY2tldCIsDQogICAgICAgICJwaXZvdCI6IFsgMC4wLCAyNC4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA4LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfQ0KICAgIF0NCiAgfQ0KDQp9DQo=",
					//	"SkinGeometryName": "geometry.humanoid.custom",
					//	"SkinId": "c18e65aa-7b21-4637-9b63-8ad63622ef01_Custom",
					//	"UIProfile": 0

					//}

					//	"CapeData": "////AAFHif8BMmH/ASNE/wEjRP8BI0T/ASNE/wEjRP8BI0T/ATJh/wFHif8BDhz/AQ4c/wEOHP8BDhz/AQ4c/wEOHP8BDhz/AQ4c/wEOHP8BDhz/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BR4n/AW3N/wFHif8BMmH/ATJh/wEyYf8BR4n/AUeJ/wFtzf8BR4n/ASNE/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wABI0T/ATJh/wFHif8Bbc3/AV6x/wFesf8BXrH/AV6x/wFtzf8BR4n/ATJh/wEjRP8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8AASNE/wE2af8BI0T/ASNE/wEjRP8BMmH/ATJh/wEyYf8BI0T/ASNE/wE2af8BI0T/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/////wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BR4n/ATJh/wEjRP8BI0T/ASNE/wEyYf8BNmn/ATZp/wEyYf8BNmn/ASNE/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/8BFiv/ARYr/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wABI0T/AUeJ/wEyYf8BNmn/ATJh/7aAGv9sWDT/AUeJ/wFHif8BMmH/AUeJ/wEjRP8BFiv/ARYr/wEWK/8BFiv/ARYr/wEdOf8BFiv/ARYr/wEWK/8BFiv/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8AASNE/wFHif8BMmH/toAa//m7Ff/5uxX/+bsV//m7Ff+2gBr/ATJh/wFHif8BI0T/ARYr/wEWK/8BFiv/ARYr/wEWK/8BHTn/ARYr/wEWK/8BFiv/ARYr/////wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BR4n/toAa//m7Ff+2gBr/bFg0/2xYNP+2gBr/+bsV/7aAGv8BR4n/ASNE/wEWK/8BFiv/ARYr/wEWK/8BFiv/AR05/wEWK/8BFiv/ARYr/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wABI0T/ATZp//m7Ff9sWDT/ATZp/7aAGv9sWDT/AW3N/2xYNP/5uxX/AUeJ/wEjRP8BFiv/ARYr/wEWK/8BFiv/ARYr/wEdOf8BHTn/ARYr/wEWK/8BFiv/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8AASNE/wE2af8BMmH/AUeJ/wFHif/5uxX/toAa/wFtzf8BR4n/ATZp/wFHif8BI0T/ARYr/wEWK/8BFiv/AR05/wEWK/8BHTn/AR05/wEWK/8BFiv/ARYr/////wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BMmH/ATZp/wFHif8BR4n/+bsV/7aAGv8Oju//AW3N/wE2af8BNmn/ASNE/wEWK/8BFiv/ARYr/wEdOf8BFiv/AR05/wEdOf8BFiv/ARYr/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wABI0T/ATJh/wE2af8Bbc3/AUeJ//m7Ff+2gBr/Do7v/wFtzf8BNmn/ATZp/wEjRP8BFiv/AR05/wEWK/8BHTn/ARYr/wEdOf8BHTn/ARYr/wEWK/8BFiv/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8AASNE/wEyYf8BR4n/AW3N/wFHif/5uxX/toAa/w6O7/8Bbc3/AUeJ/wE2af8BI0T/ARYr/wEdOf8BFiv/AR05/wEdOf8BHTn/AR05/wEWK/8BFiv/ARYr/////wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BMmH/AW3N/w6O7/8Bbc3/+bsV/7aAGv8Oju//AW3N/wFHif8BNmn/ASNE/wEWK/8BHTn/ARYr/wEdOf8BHTn/AR05/wEdOf8BHTn/ARYr/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wABI0T/ATZp/wFtzf8Oju//Do7v/wFtzf8Oju//Do7v/w6O7/8BR4n/ATZp/wEjRP8BFiv/AR05/wEdOf8BHTn/AR05/wEdOf8BHTn/AR05/wEWK/8BFiv/////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8AASNE/wE2af8Oju//Do7v/w6O7/8Bbc3/Do7v/w6O7/8Oju//AW3N/wE2af8BI0T/ARYr/wEdOf8BHTn/AR05/wEdOf8BHTn/AR05/wEdOf8BFiv/ARYr/////wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AAEjRP8BR4n/Do7v/w6O7/8Oju//AW3N/w6O7/8Oju//Do7v/wFtzf8BR4n/ASNE/wEWK/8BHTn/AR05/wEdOf8BHTn/AR05/wEdOf8BHTn/AR05/wEWK/////8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wA=",
					//	"SkinGeometryName": "geometry.humanoid.custom",
					//	"SkinId": "c18e65aa-7b21-4637-9b63-8ad63622ef01_Custom",
					//	"SkinData": "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",
					//	"SkinGeometry": "ew0KICAiZ2VvbWV0cnkuaHVtYW5vaWQiOiB7DQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJib2R5IiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTQuMCwgMTIuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDgsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDE2LCAxNiBdDQogICAgICAgICAgfQ0KICAgICAgICBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogIndhaXN0IiwNCiAgICAgICAgIm5ldmVyUmVuZGVyIjogdHJ1ZSwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDEyLjAsIDAuMCBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImhlYWQiLA0KICAgICAgICAicGl2b3QiOiBbIDAuMCwgMjQuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtNC4wLCAyNC4wLCAtNC4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgOCwgOCwgOCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAwIF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAiaGF0IiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTQuMCwgMjQuMCwgLTQuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDgsIDgsIDggXSwNCiAgICAgICAgICAgICJ1diI6IFsgMzIsIDAgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC41DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiB0cnVlDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0QXJtIiwNCiAgICAgICAgInBpdm90IjogWyAtNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC04LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyA0MCwgMTYgXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInBpdm90IjogWyA1LjAsIDIyLjAsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgNC4wLCAxMi4wLCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgNCwgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtaXJyb3IiOiB0cnVlDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0TGVnIiwNCiAgICAgICAgInBpdm90IjogWyAtMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0zLjksIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAibGVmdExlZyIsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtaXJyb3IiOiB0cnVlDQogICAgICB9DQogICAgXQ0KICB9LA0KDQogICJnZW9tZXRyeS5jYXBlIjogew0KICAgICJ0ZXh0dXJld2lkdGgiOiA2NCwNCiAgICAidGV4dHVyZWhlaWdodCI6IDMyLA0KDQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJjYXBlIiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIC0zLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC01LjAsIDguMCwgLTMuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDEwLCAxNiwgMSBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAwIF0NCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0NCiAgICBdDQogIH0sDQogICJnZW9tZXRyeS5odW1hbm9pZC5jdXN0b206Z2VvbWV0cnkuaHVtYW5vaWQiOiB7DQogICAgImJvbmVzIjogWw0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJoYXQiLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiBmYWxzZSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIiwNCiAgICAgICAgInBpdm90IjogWyAwLjAsIDI0LjAsIDAuMCBdDQogICAgICB9LA0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDUuMCwgMjIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyA0LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAzMiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJyaWdodEFybSIsDQogICAgICAgICJyZXNldCI6IHRydWUsDQogICAgICAgICJwaXZvdCI6IFsgLTUuMCwgMjIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtOC4wLCAxMi4wLCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgNCwgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRJdGVtIiwNCiAgICAgICAgInBpdm90IjogWyAtNiwgMTUsIDEgXSwNCiAgICAgICAgIm5ldmVyUmVuZGVyIjogdHJ1ZSwNCiAgICAgICAgInBhcmVudCI6ICJyaWdodEFybSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAibGVmdFNsZWV2ZSIsDQogICAgICAgICJwaXZvdCI6IFsgNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIDQuMCwgMTIuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDQ4LCA0OCBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogInJpZ2h0U2xlZXZlIiwNCiAgICAgICAgInBpdm90IjogWyAtNS4wLCAyMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC04LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyA0MCwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0TGVnIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMC4xLCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0UGFudHMiLA0KICAgICAgICAicGl2b3QiOiBbIDEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMC4xLCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCA0OCBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAicG9zIjogWyAxLjksIDEyLCAwIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgLTEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMy45LCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAicG9zIjogWyAtMS45LCAxMiwgMCBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImphY2tldCIsDQogICAgICAgICJwaXZvdCI6IFsgMC4wLCAyNC4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA4LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfQ0KICAgIF0NCiAgfSwNCiAgImdlb21ldHJ5Lmh1bWFub2lkLmN1c3RvbVNsaW06Z2VvbWV0cnkuaHVtYW5vaWQiOiB7DQoNCiAgICAiYm9uZXMiOiBbDQogICAgICB7DQogICAgICAgICJuYW1lIjogImhhdCIsDQogICAgICAgICJuZXZlclJlbmRlciI6IGZhbHNlLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0QXJtIiwNCiAgICAgICAgInJlc2V0IjogdHJ1ZSwNCiAgICAgICAgIm1pcnJvciI6IGZhbHNlLA0KICAgICAgICAicGl2b3QiOiBbIDUuMCwgMjEuNSwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyA0LjAsIDExLjUsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyAzLCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAzMiwgNDggXQ0KICAgICAgICAgIH0NCiAgICAgICAgXQ0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJyaWdodEFybSIsDQogICAgICAgICJyZXNldCI6IHRydWUsDQogICAgICAgICJwaXZvdCI6IFsgLTUuMCwgMjEuNSwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtNy4wLCAxMS41LCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgMywgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDAsIDE2IF0NCiAgICAgICAgICB9DQogICAgICAgIF0NCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgInBpdm90IjogWyAtNiwgMTQuNSwgMSBdLA0KICAgICAgICAibmV2ZXJSZW5kZXIiOiB0cnVlLA0KICAgICAgICAibmFtZSI6ICJyaWdodEl0ZW0iLA0KICAgICAgICAicGFyZW50IjogInJpZ2h0QXJtIg0KICAgICAgfSwNCg0KICAgICAgew0KICAgICAgICAibmFtZSI6ICJsZWZ0U2xlZXZlIiwNCiAgICAgICAgInBpdm90IjogWyA1LjAsIDIxLjUsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgNC4wLCAxMS41LCAtMi4wIF0sDQogICAgICAgICAgICAic2l6ZSI6IFsgMywgMTIsIDQgXSwNCiAgICAgICAgICAgICJ1diI6IFsgNDgsIDQ4IF0sDQogICAgICAgICAgICAiaW5mbGF0ZSI6IDAuMjUNCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRTbGVldmUiLA0KICAgICAgICAicGl2b3QiOiBbIC01LjAsIDIxLjUsIDAuMCBdLA0KICAgICAgICAiY3ViZXMiOiBbDQogICAgICAgICAgew0KICAgICAgICAgICAgIm9yaWdpbiI6IFsgLTcuMCwgMTEuNSwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDMsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDQwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImxlZnRMZWciLA0KICAgICAgICAicmVzZXQiOiB0cnVlLA0KICAgICAgICAibWlycm9yIjogZmFsc2UsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDE2LCA0OCBdDQogICAgICAgICAgfQ0KICAgICAgICBdDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImxlZnRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgMS45LCAxMi4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC0wLjEsIDAuMCwgLTIuMCBdLA0KICAgICAgICAgICAgInNpemUiOiBbIDQsIDEyLCA0IF0sDQogICAgICAgICAgICAidXYiOiBbIDAsIDQ4IF0sDQogICAgICAgICAgICAiaW5mbGF0ZSI6IDAuMjUNCiAgICAgICAgICB9DQogICAgICAgIF0sDQogICAgICAgICJtYXRlcmlhbCI6ICJhbHBoYSINCiAgICAgIH0sDQoNCiAgICAgIHsNCiAgICAgICAgIm5hbWUiOiAicmlnaHRQYW50cyIsDQogICAgICAgICJwaXZvdCI6IFsgLTEuOSwgMTIuMCwgMC4wIF0sDQogICAgICAgICJjdWJlcyI6IFsNCiAgICAgICAgICB7DQogICAgICAgICAgICAib3JpZ2luIjogWyAtMy45LCAwLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA0LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAwLCAzMiBdLA0KICAgICAgICAgICAgImluZmxhdGUiOiAwLjI1DQogICAgICAgICAgfQ0KICAgICAgICBdLA0KICAgICAgICAibWF0ZXJpYWwiOiAiYWxwaGEiDQogICAgICB9LA0KDQogICAgICB7DQogICAgICAgICJuYW1lIjogImphY2tldCIsDQogICAgICAgICJwaXZvdCI6IFsgMC4wLCAyNC4wLCAwLjAgXSwNCiAgICAgICAgImN1YmVzIjogWw0KICAgICAgICAgIHsNCiAgICAgICAgICAgICJvcmlnaW4iOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwNCiAgICAgICAgICAgICJzaXplIjogWyA4LCAxMiwgNCBdLA0KICAgICAgICAgICAgInV2IjogWyAxNiwgMzIgXSwNCiAgICAgICAgICAgICJpbmZsYXRlIjogMC4yNQ0KICAgICAgICAgIH0NCiAgICAgICAgXSwNCiAgICAgICAgIm1hdGVyaWFsIjogImFscGhhIg0KICAgICAgfQ0KICAgIF0NCiAgfQ0KDQp9DQo=",

					try
					{
						//foreach (var x in payload)
						//{
						//	if (((string)x.Name).StartsWith("AnimatedImageData") || x.Name == "PersonaSkin" || x.Name == "PremiumSkin")
						//	{
						//		System.Console.WriteLine("-------------------------- {0}", x.Name);
						//		System.Console.WriteLine(x.Value);
						//	}
						//}

						//-------------------------- ClientRandomId
						//-------------------------- CurrentInputMode
						//-------------------------- DefaultInputMode
						//-------------------------- DeviceModel
						//-------------------------- DeviceOS
						//-------------------------- GameVersion
						//-------------------------- GuiScale
						//-------------------------- LanguageCode
						//-------------------------- PlatformOnlineId
						//-------------------------- ServerAddress
						//-------------------------- UIProfile
						//-------------------------- ThirdPartyName

						//-------------------------- CapeId
						//-------------------------- CapeData
						//-------------------------- CapeImageHeight
						//-------------------------- CapeImageWidth
						//-------------------------- CapeOnClassicSkin

						//-------------------------- SkinId
						//-------------------------- SkinResourcePatch [base64][json] contains GeometryName
						//-------------------------- SkinImageHeight
						//-------------------------- SkinImageWidth
						//-------------------------- SkinData
						//-------------------------- SkinGeometryData
						//-------------------------- PremiumSkin
						//-------------------------- PersonaSkin
						//-------------------------- SkinAnimationData
						//-------------------------- AnimatedImageData

						//-------------------------- DeviceId

						// --------------------------------------------------------------

						// Unused
						//-------------------------- SelfSignedId
						//-------------------------- ThirdPartyNameOnly
						//-------------------------- PlatformOfflineId

						_playerInfo.ClientId = clientData.ClientRandomId;
						_playerInfo.CurrentInputMode = clientData.CurrentInputMode;
						_playerInfo.DefaultInputMode = clientData.DefaultInputMode;
						_playerInfo.DeviceModel = clientData.DeviceModel;
						_playerInfo.DeviceOS = clientData.DeviceOS;
						_playerInfo.GameVersion = clientData.GameVersion;
						_playerInfo.GuiScale = clientData.GuiScale;
						_playerInfo.LanguageCode = clientData.LanguageCode;
						_playerInfo.PlatformChatId = clientData.PlatformOnlineId;
						_playerInfo.ServerAddress = clientData.ServerAddress;
						_playerInfo.UIProfile = clientData.UIProfile;
						_playerInfo.ThirdPartyName = clientData.ThirdPartyName;
						_playerInfo.TenantId = clientData.TenantId;
						_playerInfo.DeviceId = clientData.DeviceId;
						
						_playerInfo.Skin = clientData.ToSkin();
					}
					catch (Exception e)
					{
						Log.Error("Parsing skin data", e);
					}
				}

				{
					dynamic json = JObject.Parse(certificateChain);

					if (Log.IsDebugEnabled) Log.Debug($"Certificate JSON:\n{json}");

					// Protocol 1.21.90+ wraps the login identity in an authentication
					// envelope: {Certificate, AuthenticationType, Token}. Identity comes from
					// the Token, which is a PlayFab/OIDC JWT; Certificate carries the legacy
					// chain, which a real client still sends alongside it.
					string multiplayerToken = null;
					// Kept for diagnostics: json is reassigned to the Certificate contents below,
					// which loses the envelope, and the envelope is what a failed login needs named.
					string authenticationType = null;
					if (json["AuthenticationType"] != null)
					{
						// 1.21.90+ authentication envelope: {AuthenticationType, Token[, Certificate]}.
						// Online auth (type 0) omits the cert chain entirely; identity is in the Token.
						// Offline (type 2) carries an empty chain and our self-signed OIDC Token.
						authenticationType = (string) json["AuthenticationType"];
						multiplayerToken = (string) json["Token"];
						string certificate = (string) json["Certificate"];
						if (!string.IsNullOrEmpty(certificate)) json = JObject.Parse(certificate);
					}

					JArray chain = json.chain;
					//var chainArray = chain.ToArray();

					string validationKey = null;
					string identityPublicKey = null;

					// The Token is the identity whenever there is one, chain or no chain. A real
					// 1.26.40 client authenticating for real (type 0) sends BOTH a Token and a
					// three-token legacy chain, so gating this on an empty chain sent those logins
					// down the chain walk, where nothing matches the Mojang root any more and every
					// token is skipped. Vanilla and gophertunnel both read the token first and only
					// consult the chain afterwards, for the numeric title id the OIDC token omits,
					// which is not something we use.
					if (!string.IsNullOrEmpty(multiplayerToken))
					{
						// Identity comes from the Token. Two shapes: our offline OIDC token has
						// {cpk, xname, identity}; the real client's online token (Full auth) has
						// {cpk, xname, xid, mid, tid, ipt} and carries its identity GUID as leguuid.
						//
						// Nothing below proves any of it. The payload is read without checking the
						// signature, so the gamertag and XUID are whatever the client typed. With
						// ForceXBLAuthentication on we verify the token against the issuer's
						// published keys first and refuse the login if it does not hold up; the
						// claims are then Mojang-issued, and the encryption handshake proves this
						// connection holds the private half of the cpk they name.
						if (Config.GetProperty("ForceXBLAuthentication", false))
						{
							FranchiseTokenValidator.Identity verified = FranchiseTokenValidator.Validate(multiplayerToken);
							if (verified?.Xuid == null)
							{
								Log.Warn($"Rejecting login from {_session.GetClientEndPoint()}: the multiplayer token is not a valid Xbox Live identity");
								_session.Disconnect(Config.GetProperty("ForceXBLLogin", "You must authenticate to XBOX Live to join this server."));
								return;
							}

							// The identity is genuine; this ties the rest of the login to it. The
							// client data (skin, device, language) travels in its own document, and
							// unverified it can be swapped for anything: a captured token would let
							// someone wear another player's identity with their own everything else.
							if (!FranchiseTokenValidator.VerifyClientData(skinData, verified.ClientPublicKey))
							{
								Log.Warn($"Rejecting login from {_session.GetClientEndPoint()}: client data is not signed by {verified.DisplayName}'s key");
								_session.Disconnect(Config.GetProperty("ForceXBLLogin", "You must authenticate to XBOX Live to join this server."));
								return;
							}

							Log.Info($"Verified Xbox Live identity {verified.DisplayName} (XUID {verified.Xuid})");
						}

						dynamic tokenPayload = JObject.Parse(JWT.Payload(multiplayerToken));
						string xuid = (string) tokenPayload.xid;
						// leguuid is the legacy identity UUID a real client's token carries; only
						// our own offline token writes it as "identity". Reading just the latter
						// sent every real login down the derive-from-XUID fallback below, giving the
						// player a UUID that matches neither Mojang's nor any other server's.
						string identity = (string) tokenPayload.leguuid ?? (string) tokenPayload.identity;
						if (string.IsNullOrEmpty(identity))
						{
							string seed = !string.IsNullOrEmpty(xuid) ? xuid : (string) tokenPayload.mid ?? (string) tokenPayload.sub;
							identity = DeriveUuidFromXuid(seed).ToString();
						}

						_playerInfo.CertificateData = new CertificateData
						{
							IdentityPublicKey = (string) tokenPayload.cpk,
							ExtraData = new ExtraData
							{
								DisplayName = (string) tokenPayload.xname,
								Identity = identity,
								Xuid = string.IsNullOrEmpty(xuid) ? null : xuid
							}
						};
					}
					// chain is null whenever the envelope carried no Certificate, which is every
					// online (type 0) login. Reaching the loop with it null threw before the
					// identity check below could report anything.
					else if (chain != null)
					foreach (JToken token in chain)
					{
						IDictionary<string, dynamic> headers = JWT.Headers(token.ToString());

						if (Log.IsDebugEnabled)
						{
							Log.Debug("Raw chain element:\n" + token.ToString());
							Log.Debug($"JWT Header: {string.Join(";", headers)}");

							dynamic jsonPayload = JObject.Parse(JWT.Payload(token.ToString()));
							Log.Debug($"JWT Payload:\n{jsonPayload}");
						}

						// Mojang root x5u cert (string): MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE8ELkixyLcwlZryUQcu1TvPOmI2B7vX83ndnWRUaXm74wFfa5f/lwQNTfrLVHa2PmenpGI6JhIMUJaWZrjmMj90NoKNFSNBuKdm8rYiXsfaz3K36x/1U26HpG0ZxK/V1V

						if (!headers.ContainsKey("x5u")) continue;

						string x5u = headers["x5u"];

						if (identityPublicKey == null)
						{
							if (CertificateData.MojangRootKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								Log.Debug("Key is ok, and got Mojang root");
							}
							else if (chain.Count > 1)
							{
								Log.Debug("Got client cert (client root)");
								continue;
							}
							else if (chain.Count == 1)
							{
								Log.Debug("Selfsigned chain");
							}
						}
						else if (identityPublicKey.Equals(x5u))
						{
							Log.Debug("Derived Key is ok");
						}

						// x5u is a DER SubjectPublicKeyInfo, which ECDsa imports directly; the curve and
						// the point come out of the encoding rather than being restated here.
						using var x5Key = ECDsa.Create();
						x5Key.ImportSubjectPublicKeyInfo(x5u.DecodeBase64(), out _);
						ECParameters signParam = x5Key.ExportParameters(false);
						signParam.Validate();

						// REFCT: Jose-JWT takes and returns strings, and Newtonsoft deserializes from a
						// string, so a login chain materializes its base64 and its JSON as strings. Those
						// are what reach the LOH on a join burst (traced: [LOH] System.String and
						// [LOH] System.Char[], nothing else). System.Text.Json reads UTF8 straight off a
						// span (Utf8JsonReader), and Microsoft.IdentityModel.JsonWebTokens is the JWT
						// reader built on it, so the strings never have to exist.
						CertificateData data = JWT.Decode<CertificateData>(token.ToString(), ECDsa.Create(signParam));

						// Validate

						if (data != null)
						{
							identityPublicKey = data.IdentityPublicKey;

							if (Log.IsDebugEnabled) Log.Debug("Decoded token success");

							if (CertificateData.MojangRootKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								Log.Debug("Got Mojang key. Is valid = " + data.CertificateAuthority);
								validationKey = data.IdentityPublicKey;
							}
							else if (validationKey != null && validationKey.Equals(x5u, StringComparison.InvariantCultureIgnoreCase))
							{
								//TODO: Remove. Just there to be able to join with same XBL multiple times without crashing the server.
								//data.ExtraData.Identity = Guid.NewGuid().ToString();
								_playerInfo.CertificateData = data;
							}
							else
							{
								if (data.ExtraData == null) continue;

								// Self signed, make sure they don't fake XUID
								if (data.ExtraData.Xuid != null)
								{
									Log.Warn("Received fake XUID from " + data.ExtraData.DisplayName);
									data.ExtraData.Xuid = null;
								}

								//TODO: Remove. Just there to be able to join with same XBL multiple times without crashing the server.
								//data.ExtraData.Identity = Guid.NewGuid().ToString();
								_playerInfo.CertificateData = data;
							}
						}
						else
						{
							Log.Error("Not a valid Identity Public Key for decoding");
						}
					}

					// Neither path above produced an identity: an authentication envelope with no
					// usable chain and no Token lands here, as does a chain whose every token
					// failed validation. This used to fall straight into the dereference below and
					// take the decode path down with an NRE, which closed the session telling
					// neither the client nor the log anything about why.
					if (_playerInfo.CertificateData?.ExtraData == null)
					{
						Log.Warn(
							$"Rejecting login from {_session.GetClientEndPoint()}: the login carried no readable identity. "
							+ $"AuthenticationType={authenticationType ?? "<absent>"}, "
							+ $"Token={(string.IsNullOrEmpty(multiplayerToken) ? "<absent>" : $"{multiplayerToken.Length} chars")}, "
							+ $"chain={(chain == null ? "<absent>" : $"{chain.Count} token(s)")}");

						_session.Disconnect("Could not read your login identity.");
						return;
					}

					{
						_playerInfo.Username = _playerInfo.CertificateData.ExtraData.DisplayName;
						_session.Username = _playerInfo.Username;
						string identity = _playerInfo.CertificateData.ExtraData.Identity;

						// The transport is named rather than inferred: with two of them live, which one a
						// player arrived on is otherwise only visible as a side effect, such as whether
						// encryption got negotiated.
						Log.Info($"Login: {_playerInfo.Username} over {_session.TransportName} from {_session.GetClientEndPoint()} on protocol {_playerInfo.ProtocolVersion}");
						if (Log.IsDebugEnabled) Log.Debug($"Connecting user {_playerInfo.Username} with identity={identity} on protocol version={_playerInfo.ProtocolVersion}");
						_playerInfo.ClientUuid = new UUID(identity);

						// No application-layer encryption handshake. The transport is DTLS, so the link
						// is already encrypted, and running Bedrock's AES-CTR over it would encrypt
						// twice. The ECDH exchange, the ServerToClientHandshake and the ClientToServer
						// reply it waited for belonged to RakNet, which is gone: the client is told
						// nothing and the login continues straight to the handshake handler.
					}
				}

				_bedrockHandler.Handler.HandleMcpeClientToServerHandshake(null);
			}
			catch (Exception e)
			{
				Log.Error("Decrypt", e);
			}
		}

		public void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
		{
			Log.Warn($"Connection established with {_playerInfo.Username} using MC version {_playerInfo.GameVersion} with protocol version {_playerInfo.ProtocolVersion}");

			IServer server = _serverManager.GetServer();

			IMcpeMessageHandler messageHandler = server.CreatePlayer(_session, _playerInfo);
			_bedrockHandler.Handler = messageHandler; // Replace current message handler with real one.

			if (_playerInfo.ProtocolVersion < McpeProtocolInfo.ProtocolVersion || _playerInfo.ProtocolVersion > 65535)
			{
				Log.Warn($"Wrong version ({_playerInfo.ProtocolVersion}) of Minecraft. Upgrade to join this server.");
				_session.Disconnect($"Wrong version ({_playerInfo.ProtocolVersion}) of Minecraft. Upgrade to join this server.");
				return;
			}

			if (Config.GetProperty("ForceXBLAuthentication", false) && _playerInfo.CertificateData.ExtraData.Xuid == null)
			{
				Log.Warn($"You must authenticate to XBOX Live to join this server.");
				_session.Disconnect(Config.GetProperty("ForceXBLLogin", "You must authenticate to XBOX Live to join this server."));

				return;
			}

			_bedrockHandler.Handler.HandleMcpeClientToServerHandshake(null);
		}

		public void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
		}

		public void HandleMcpeText(McpeText message)
		{
		}

		public void HandleMcpeMoveEntity(McpeMoveEntity message)
		{
		}

		public void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
		}

		public void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
		}

		public void HandleMcpeClientCacheBlobStatus(McpeClientCacheBlobStatus message)
		{
		}

		public void HandleMcpeEmoteList(McpeEmoteList message)
		{
		}

		public void HandleMcpeClientCacheStatus(McpeClientCacheStatus message)
		{
		}

		public void HandleMcpeNetworkSettings(McpeNetworkSettings message)
		{
		}

		public void HandleMcpeEmote(McpeEmote message)
		{
		}

		public void HandleMcpeMultiplayerSettings(McpeMultiplayerSettings message)
		{
		}

		public void HandleMcpeSettingsCommand(McpeSettingsCommand message)
		{
		}

		public void HandleMcpeAnvilDamage(McpeAnvilDamage message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpePlayerAuthInput(McpePlayerAuthInput message)
		{
			
		}

		public void HandleMcpeItemStackRequest(McpeItemStackRequest message)
		{
		}

		public void HandleMcpeUpdatePlayerGameType(McpeUpdatePlayerGameType message)
		{
		}

		public void HandleMcpePositionTrackingDbClientRequest(McpePositionTrackingDbClientRequest message)
		{
		}

		public void HandleMcpeDebugInfo(McpeDebugInfo message)
		{
		}

		public void HandleMcpePacketViolationWarning(McpePacketViolationWarning message)
		{
		}

		/// <inheritdoc />
		public void HandleMcpeUpdateSubChunkBlocksPacket(McpeUpdateSubChunkBlocksPacket message)
		{
			
		}

		/// <inheritdoc />
		public void HandleMcpeSubChunkRequestPacket(McpeSubChunkRequestPacket message)
		{
			
		}

		public void HandleMcpeEntityEvent(McpeEntityEvent message)
		{
		}

		public void HandleMcpeInventoryTransaction(McpeInventoryTransaction message)
		{
		}

		public void HandleMcpeMobEquipment(McpeMobEquipment message)
		{
		}

		public void HandleMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		public void HandleMcpeInteract(McpeInteract message)
		{
		}

		public void HandleMcpeBlockPickRequest(McpeBlockPickRequest message)
		{
		}

		public void HandleMcpeEntityPickRequest(McpeEntityPickRequest message)
		{
		}

		public void HandleMcpePlayerAction(McpePlayerAction message)
		{
		}

		public void HandleMcpeSetEntityData(McpeSetEntityData message)
		{
		}

		public void HandleMcpeSetEntityMotion(McpeSetEntityMotion message)
		{
		}

		public void HandleMcpeAnimate(McpeAnimate message)
		{
		}

		public void HandleMcpeRespawn(McpeRespawn message)
		{
		}

		public void HandleMcpeContainerClose(McpeContainerClose message)
		{
		}

		public void HandleMcpePlayerHotbar(McpePlayerHotbar message)
		{
		}

		public void HandleMcpeInventoryContent(McpeInventoryContent message)
		{
		}

		public void HandleMcpeInventorySlot(McpeInventorySlot message)
		{
		}

		public void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
		}

		public void HandleMcpeSetPlayerGameType(McpeSetPlayerGameType message)
		{
		}

		public void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		public void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
		}

		public void HandleMcpeCommandRequest(McpeCommandRequest message)
		{
		}

		public void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message)
		{
		}

		public void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message)
		{
		}

		public void HandleMcpePurchaseReceipt(McpePurchaseReceipt message)
		{
		}

		public void HandleMcpePlayerSkin(McpePlayerSkin message)
		{
		}

		public void HandleMcpeNpcRequest(McpeNpcRequest message)
		{
		}

		public void HandleMcpePhotoTransfer(McpePhotoTransfer message)
		{
		}

		public void HandleMcpeModalFormResponse(McpeModalFormResponse message)
		{
		}

		public void HandleMcpeServerSettingsRequest(McpeServerSettingsRequest message)
		{
		}

		public void HandleMcpeLabTable(McpeLabTable messae)
		{
		}

		public void HandleMcpeSetLocalPlayerAsInitialized(McpeSetLocalPlayerAsInitialized message)
		{
		}

		public void HandleMcpePlayerToggleCrafterSlotRequest(McpePlayerToggleCrafterSlotRequest message)
		{
		}

		public void HandleMcpeServerBoundLoadingScreen(McpeServerBoundLoadingScreen message)
		{
		}

		public void HandleMcpeServerBoundDiagnostics(McpeServerBoundDiagnostics message)
		{
		}

		public void HandleMcpeClientCameraAimAssist(McpeClientCameraAimAssist message)
		{
		}

		public void HandleMcpeClientMovementPredictionSync(McpeClientMovementPredictionSync message)
		{
		}

		public void HandleMcpeUpdateClientOptions(McpeUpdateClientOptions message)
		{
		}

		public void HandleMcpeServerboundPackSettingChange(McpeServerboundPackSettingChange message)
		{
		}

		public void HandleMcpeSetPlayerFurnaceOptions(McpeSetPlayerFurnaceOptions message)
		{
		}

		public void HandleMcpeServerboundDataStore(McpeServerboundDataStore message)
		{
		}

		public void HandleMcpePartyDestinationCookieResponse(McpePartyDestinationCookieResponse message)
		{
		}

		public void HandleMcpeResourcePacksReadyForValidation(McpeResourcePacksReadyForValidation message)
		{
		}

		public void HandleMcpePartyChanged(McpePartyChanged message)
		{
		}

		public void HandleMcpeServerboundDataDrivenScreenClosed(McpeServerboundDataDrivenScreenClosed message)
		{
		}

		public void HandleMcpeSetPlayerInventoryOptions(McpeSetPlayerInventoryOptions message)
		{
		}

		public void HandleMcpeCreatePhoto(McpeCreatePhoto message)
		{
		}

		public void HandleMcpeNetworkStackLatency(McpeNetworkStackLatency message)
		{
		}

		public void HandleMcpeScriptMessage(McpeScriptMessage message)
		{
		}

		public void HandleMcpeCodeBuilderSource(McpeCodeBuilderSource message)
		{
		}

		public void HandleMcpeChangeMobProperty(McpeChangeMobProperty message)
		{
		}

		public void HandleMcpeRequestAbility(McpeRequestAbility message)
		{
		}

		public void HandleMcpeRequestPermissions(McpeRequestPermissions message)
		{
		}

		public void HandleMcpeEditorNetwork(McpeEditorNetwork message)
		{
		}

		public void HandleMcpeGameTestRequest(McpeGameTestRequest message)
		{
		}

	}

	public interface IServerManager
	{
		IServer GetServer();
	}

	public interface IServer
	{
		IMcpeMessageHandler CreatePlayer(INetworkHandler session, PlayerInfo playerInfo);
	}

	public class PlayerInfo
	{
		public int ADRole { get; set; }
		public CertificateData CertificateData { get; set; }
		public string Username { get; set; }
		public UUID ClientUuid { get; set; }
		public string ServerAddress { get; set; }
		public long ClientId { get; set; }
		public Skin Skin { get; set; }
		public int CurrentInputMode { get; set; }
		public int DefaultInputMode { get; set; }
		public string DeviceModel { get; set; }
		public string GameVersion { get; set; }
		public int DeviceOS { get; set; }
		public string DeviceId { get; set; }
		public int GuiScale { get; set; }
		public int UIProfile { get; set; }
		public int Edition { get; set; }
		public int ProtocolVersion { get; set; }
		public string LanguageCode { get; set; }
		public string PlatformChatId { get; set; }
		public string ThirdPartyName { get; set; }
		public string TenantId { get; set; }
	}

	public class DefaultServerManager : IServerManager
	{
		private readonly MiNetServer _miNetServer;
		private IServer _getServer;

		protected DefaultServerManager()
		{
		}

		public DefaultServerManager(MiNetServer miNetServer)
		{
			_miNetServer = miNetServer;
			_getServer = new DefaultServer(miNetServer);
		}

		public virtual IServer GetServer()
		{
			return _getServer;
		}
	}

	public class DefaultServer : IServer
	{
		private readonly MiNetServer _server;

		protected DefaultServer()
		{
		}

		public DefaultServer(MiNetServer server)
		{
			_server = server;
		}

		public virtual IMcpeMessageHandler CreatePlayer(INetworkHandler session, PlayerInfo playerInfo)
		{
			Player player = _server.PlayerFactory.CreatePlayer(_server, session.GetClientEndPoint(), playerInfo);
			player.NetworkHandler = session;
			player.CertificateData = playerInfo.CertificateData;
			player.Username = playerInfo.Username;
			player.ClientUuid = playerInfo.ClientUuid;
			player.ServerAddress = playerInfo.ServerAddress;
			player.ClientId = playerInfo.ClientId;
			player.Skin = playerInfo.Skin;
			player.PlayerInfo = playerInfo;

			return player;
		}
	}
}