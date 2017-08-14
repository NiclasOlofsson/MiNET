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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Xsl;
using fNbt;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json.Linq;

namespace MiNET
{
	public class LoginMessageHandler : IMcpeMessageHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (LoginMessageHandler));

		private readonly PlayerNetworkSession _session;

		private object _loginSyncLock = new object();
		private PlayerInfo _playerInfo = new PlayerInfo();

		public LoginMessageHandler(PlayerNetworkSession session)
		{
			_session = session;
		}

		public void Disconnect(string reason, bool sendDisconnect = true)
		{
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
			_playerInfo.Edition = message.edition;

			DecodeCert(message);

			////string fileName = Path.GetTempPath() + "Skin_" + Skin.SkinType + ".png";
			////Log.Info($"Writing skin to filename: {fileName}");
			////Skin.SaveTextureToFile(fileName, Skin.Texture);
		}

		protected void DecodeCert(McpeLogin message)
		{
			byte[] buffer = message.payload;

			if (message.payload.Length != buffer.Length)
			{
				Log.Debug($"Wrong lenght {message.payload.Length} != {message.payload.Length}");
				throw new Exception($"Wrong lenght {message.payload.Length} != {message.payload.Length}");
			}

			if (Log.IsDebugEnabled) Log.Debug("Lenght: " + message.payload.Length + ", Message: " + Convert.ToBase64String(buffer));

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
					dynamic payload = JObject.Parse(JWT.Payload(skinData));

					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Header: {string.Join(";", headers)}");
					if (Log.IsDebugEnabled) Log.Debug($"Skin JWT Payload:\n{payload.ToString()}");

					// Skin JWT Payload: 

					//{
					//  "ADRole": 2,
					//	"ClientRandomId": 1423700530444426768,
					//	"CurrentInputMode": 1,
					//	"DefaultInputMode": 1,
					//	"DeviceModel": "ASUSTeK COMPUTER INC. N550JK",
					//	"DeviceOS": 7,
					//	"GameVersion": "1.1.0",
					//	"GuiScale": 0,
					//	"LanguageCode": "en_US",
					//	"ServerAddress": "192.168.0.3:19132",
					//	"SkinData": "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",
					//	"SkinId": "Standard_Custom",
					//	"TenantId": "",
					//	"UIProfile": 0
					//}

					try
					{
						_playerInfo.ADRole = payload.ADRole;
						_playerInfo.ClientId = payload.ClientRandomId;
						_playerInfo.CurrentInputMode = payload.CurrentInputMode;
						_playerInfo.DefaultInputMode = payload.DefaultInputMode;
						_playerInfo.DeviceModel = payload.DeviceModel;
						_playerInfo.DeviceOS = payload.DeviceOS;
						_playerInfo.GameVersion = payload.GameVersion;
						_playerInfo.GuiScale = payload.GuiScale;
						_playerInfo.LanguageCode = payload.LanguageCode;
						_playerInfo.ServerAddress = payload.ServerAddress;
						_playerInfo.UIProfile = payload.UIProfile;

						_playerInfo.Skin = new Skin()
						{
							SkinType = payload.SkinId,
							Texture = Convert.FromBase64String((string) payload.SkinData),
						};
					}
					catch (Exception e)
					{
						Log.Error("Parsing skin data", e);
					}
				}

				{
					dynamic json = JObject.Parse(certificateChain);

					if (Log.IsDebugEnabled) Log.Debug($"Certificate JSON:\n{json}");

					JArray chain = json.chain;
					//var chainArray = chain.ToArray();

					string validationKey = null;
					string identityPublicKey = null;

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

						if (Log.IsDebugEnabled)
						{
							Log.Debug($"x5u cert (string): {x5u}");
							ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(x5u);
							Log.Debug($"Cert:\n{publicKey.ToXmlString()}");
						}

						// Validate
						CngKey newKey = CryptoUtils.ImportECDsaCngKeyFromString(x5u);
						CertificateData data = JWT.Decode<CertificateData>(token.ToString(), newKey);

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

								_playerInfo.CertificateData = data;
							}
						}
						else
						{
							Log.Error("Not a valid Identity Public Key for decoding");
						}
					}

					//TODO: Implement disconnect here

					{
						_playerInfo.Username = _playerInfo.CertificateData.ExtraData.DisplayName;
						_session.Username = _playerInfo.Username;
						string identity = _playerInfo.CertificateData.ExtraData.Identity;

						if (Log.IsDebugEnabled) Log.Debug($"Connecting user {_playerInfo.Username} with identity={identity}");
						_playerInfo.ClientUuid = new UUID(identity);

						_session.CryptoContext = new CryptoContext
						{
							UseEncryption = Config.GetProperty("UseEncryptionForAll", false) || (Config.GetProperty("UseEncryption", true) && !string.IsNullOrWhiteSpace(_playerInfo.CertificateData.ExtraData.Xuid)),
						};

						if (_session.CryptoContext.UseEncryption)
						{
							ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(_playerInfo.CertificateData.IdentityPublicKey);
							if (Log.IsDebugEnabled) Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

							// Create shared shared secret
							ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(384);
							ecKey.HashAlgorithm = CngAlgorithm.Sha256;
							ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
							ecKey.SecretPrepend = Encoding.UTF8.GetBytes("RANDOM SECRET"); // Server token

							byte[] secret = ecKey.DeriveKeyMaterial(publicKey);

							if (Log.IsDebugEnabled) Log.Debug($"SECRET KEY (b64):\n{Convert.ToBase64String(secret)}");

							{
								RijndaelManaged rijAlg = new RijndaelManaged
								{
									BlockSize = 128,
									Padding = PaddingMode.None,
									Mode = CipherMode.CFB,
									FeedbackSize = 8,
									Key = secret,
									IV = secret.Take(16).ToArray(),
								};

								// Create a decrytor to perform the stream transform.
								ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
								MemoryStream inputStream = new MemoryStream();
								CryptoStream cryptoStreamIn = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);

								ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
								MemoryStream outputStream = new MemoryStream();
								CryptoStream cryptoStreamOut = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);

								_session.CryptoContext.Algorithm = rijAlg;
								_session.CryptoContext.Decryptor = decryptor;
								_session.CryptoContext.Encryptor = encryptor;
								_session.CryptoContext.InputStream = inputStream;
								_session.CryptoContext.OutputStream = outputStream;
								_session.CryptoContext.CryptoStreamIn = cryptoStreamIn;
								_session.CryptoContext.CryptoStreamOut = cryptoStreamOut;
							}

							var response = McpeServerToClientHandshake.CreateObject();
							response.NoBatch = true;
							response.ForceClear = true;
							response.serverPublicKey = Convert.ToBase64String(ecKey.PublicKey.GetDerEncoded());
							response.tokenLength = (short) ecKey.SecretPrepend.Length;
							response.token = ecKey.SecretPrepend;

							_session.SendPackage(response);

							if (Log.IsDebugEnabled) Log.Warn($"Encryption enabled for {_session.Username}");
						}
					}
				}

				if (!_session.CryptoContext.UseEncryption)
				{
					_session.MessageHandler.HandleMcpeClientToServerHandshake(null);
				}
			}
			catch (Exception e)
			{
				Log.Error("Decrypt", e);
			}
		}

		public void HandleMcpeClientToServerHandshake(McpeClientToServerHandshake message)
		{
			IServerManager serverManager = _session.Server.ServerManager;
			IServer server = serverManager.GetServer();

			IMcpeMessageHandler messageHandler = server.CreatePlayer(_session, _playerInfo);
			_session.MessageHandler = messageHandler; // Replace current message handler with real one.

			if (_playerInfo.ProtocolVersion < 111)
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

			_session.MessageHandler.HandleMcpeClientToServerHandshake(null);
		}

		public void HandleMcpeResourcePackClientResponse(McpeResourcePackClientResponse message)
		{
		}

		public void HandleMcpeText(McpeText message)
		{
		}

		public void HandleMcpeMovePlayer(McpeMovePlayer message)
		{
		}

		public void HandleMcpeRemoveBlock(McpeRemoveBlock message)
		{
		}

		public void HandleMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
		}

		public void HandleMcpeEntityEvent(McpeEntityEvent message)
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

		public void HandleMcpeUseItem(McpeUseItem message)
		{
		}

		public void HandleMcpePlayerAction(McpePlayerAction message)
		{
		}

		public void HandleMcpeEntityFall(McpeEntityFall message)
		{
		}

		public void HandleMcpeAnimate(McpeAnimate message)
		{
		}

		public void HandleMcpeRespawn(McpeRespawn message)
		{
		}

		public void HandleMcpeDropItem(McpeDropItem message)
		{
		}

		public void HandleMcpeContainerClose(McpeContainerClose message)
		{
		}

		public void HandleMcpeContainerSetSlot(McpeContainerSetSlot message)
		{
		}

		public void HandleMcpeCraftingEvent(McpeCraftingEvent message)
		{
		}

		public void HandleMcpeAdventureSettings(McpeAdventureSettings message)
		{
		}

		public void HandleMcpeBlockEntityData(McpeBlockEntityData message)
		{
		}

		public void HandleMcpePlayerInput(McpePlayerInput message)
		{
		}

		public void HandleMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		public void HandleMcpeRequestChunkRadius(McpeRequestChunkRadius message)
		{
		}

		public void HandleMcpeItemFrameDropItem(McpeItemFrameDropItem message)
		{
		}

		public void HandleMcpeCommandStep(McpeCommandStep message)
		{
		}

		public void HandleMcpeCommandBlockUpdate(McpeCommandBlockUpdate message)
		{
		}

		public void HandleMcpeResourcePackChunkRequest(McpeResourcePackChunkRequest message)
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