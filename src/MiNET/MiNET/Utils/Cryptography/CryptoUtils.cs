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
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using JetBrains.Annotations;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils.Skins;

namespace MiNET.Utils.Cryptography
{
	public static class CryptoUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CryptoUtils));

		public static byte[] DecodeBase64Url(this string input)
		{
			return Base64Url.Decode(input);
		}

		public static string EncodeBase64Url(this byte[] input)
		{
			return Base64Url.Encode(input);
		}

		public static byte[] DecodeBase64(this string input)
		{
			return Convert.FromBase64String(input);
		}

		public static string EncodeBase64(this byte[] input)
		{
			return Convert.ToBase64String(input);
		}

		public static byte[] ToDerEncoded([NotNull] this ECDiffieHellmanPublicKey key)
		{
			byte[] asn = new byte[24] {0x30, 0x76, 0x30, 0x10, 0x6, 0x7, 0x2a, 0x86, 0x48, 0xce, 0x3d, 0x2, 0x1, 0x6, 0x5, 0x2b, 0x81, 0x4, 0x0, 0x22, 0x3, 0x62, 0x0, 0x4};

			return asn.Concat(key.ToByteArray().Skip(8)).ToArray();
		}

		//public static ECDiffieHellmanPublicKey FromDerEncoded(byte[] keyBytes)
		//{
		//	var clientPublicKeyBlob = FixPublicKey(keyBytes.Skip(23).ToArray());

		//	ECDiffieHellmanPublicKey clientKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);
		//	return clientKey;
		//}

		private static byte[] FixPublicKey(byte[] publicKeyBlob)
		{
			var keyType = new byte[] {0x45, 0x43, 0x4b, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};

			return keyType.Concat(keyLength).Concat(publicKeyBlob.Skip(1)).ToArray();
		}

		public static byte[] ImportECDsaCngKeyFromCngKey(byte[] inKey)
		{
			inKey[2] = 83;
			return inKey;
		}

		// CLIENT TO SERVER STUFF

		/// <summary>The P-384 key a client identifies itself with in its login chain.</summary>
		public static ECDsa GenerateClientKey()
		{
			return ECDsa.Create(ECCurve.NamedCurves.nistP384);
		}

		/// <summary>
		///     The bot's identity: stable per username, so it is the same player on every login, and a
		///     real RFC 4122 v3 UUID rather than a raw MD5 poured into a Guid. Built the way vanilla
		///     builds a player UUID from an XUID (see LoginMessageHandler.DeriveUuidFromXuid), with the
		///     username as the seed because a bot has no Xbox account. Nothing on the server checks the
		///     shape, but this UUID reaches every real client in the player list, and a value with no
		///     version or variant bits is not a UUID.
		/// </summary>
		public static Guid DeriveStableIdentity(string username)
		{
			byte[] hash = MD5.HashData(Encoding.UTF8.GetBytes("minet-auth-1-name:" + username));
			hash[6] = (byte) ((hash[6] & 0x0f) | 0x30); // version 3
			hash[8] = (byte) ((hash[8] & 0x3f) | 0x80); // variant RFC 4122

			// Guid(byte[]) reads the first three groups little-endian; a UUID is big-endian throughout.
			return new Guid(
				(hash[0] << 24) | (hash[1] << 16) | (hash[2] << 8) | hash[3],
				(short) ((hash[4] << 8) | hash[5]),
				(short) ((hash[6] << 8) | hash[7]),
				hash[8], hash[9], hash[10], hash[11], hash[12], hash[13], hash[14], hash[15]);
		}

		// Protocol 944+ offline login: identity moves out of the certificate chain into the
		// envelope's Token field, as a self-signed OIDC-style JWT.
		/// <param name="xuid">
		///     Normally empty, which is what an offline player has. A value is only useful against a
		///     server that keys something on the xuid and takes an offline chain's word for it, which
		///     BDS does for permissions.json when online-mode is off: it is the only way to give an
		///     offline bot operator, since that file has no other handle on a player.
		/// </param>
		public static string EncodeOfflineMultiplayerToken(string username, ECDsa newKey, string xuid = "")
		{
			long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			long exp = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

			ECDsa signKey = newKey;
			string b64Key = newKey.ExportSubjectPublicKeyInfo().EncodeBase64();

			var payload = new Dictionary<string, object>
			{
				["cpk"] = b64Key,

				// Empty by default, not "0". This is an Xbox account id and an offline player has no
				// account; vanilla BDS sends an empty xuid for one, and "0" is a value, not an absence.
				["xid"] = xuid ?? "",
				["xname"] = username,

				// Stable per username. A fresh GUID here made the bot a different person on every
				// login, so nothing on the server or the client could recognise it as the same player
				// twice.
				["identity"] = DeriveStableIdentity(username).ToString(),
				["iat"] = iat,
				["nbf"] = iat,
				["exp"] = exp,
				["iss"] = "self",
				["aud"] = "api://auth-minecraft-services/multiplayer"
			};

			return JWT.Encode(payload, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});
		}

		public static byte[] EncodeJwt(string username, ECDsa newKey, bool isEmulator)
		{
			long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			long exp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();

			ECDsa signKey = newKey;
			string b64Key = newKey.ExportSubjectPublicKeyInfo().EncodeBase64();

			var certificateData = new CertificateData
			{
				Exp = exp,
				Iat = iat,
				ExtraData = new ExtraData
				{
					Xuid = "",
					DisplayName = username,
					Identity = isEmulator ? Guid.NewGuid().ToString() : "85e4febd-3d33-4008-b044-1ad9fb85b26c",
					TitleId = "89692877"
				},
				Iss = "self",
				IdentityPublicKey = b64Key,
				CertificateAuthority = true,
				Nbf = iat,
				RandomNonce = new Random().Next(),
			};

			//			string txt = $@"{{
			//	""exp"": 1467508449,
			//	""extraData"": {{
			//		""displayName"": ""gurunxx"",
			//		""identity"": ""4e0199c6-7cfd-3550-b676-74398e0a5f1a""
			//	}},
			//	""identityPublicKey"": ""{b64Key}"",
			//	""nbf"": 1467508448
			//}}";

			string val = JWT.Encode(certificateData, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});

			Log.Debug(JWT.Payload(val));

			Log.Debug(string.Join(";", JWT.Headers(val)));

			//val = "eyJhbGciOiJFUzM4NCIsIng1dSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFREVLck5xdk93Y25iV3I5aUtVQ0MyeklFRmZ6Q0VnUEhQdG5Kd3VEdnZ3VjVtd1E3QzNkWmhqd0g0amxWc2RDVTlNdVl2QllQRktCTEJkWU52K09ZeW1MTFJGTU9odVFuSDhuZFRRQVV6VjJXRTF4dHdlVG1wSVFzdXdmVzRIdzAifQo.eyJleHAiOjE0Njc1MDg0NDksImV4dHJhRGF0YSI6eyJkaXNwbGF5TmFtZSI6Imd1cnVueHgiLCJpZGVudGl0eSI6IjRlMDE5OWM2LTdjZmQtMzU1MC1iNjc2LTc0Mzk4ZTBhNWYxYSJ9LCJpZGVudGl0eVB1YmxpY0tleSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFREVLck5xdk93Y25iV3I5aUtVQ0MyeklFRmZ6Q0VnUEhQdG5Kd3VEdnZ3VjVtd1E3QzNkWmhqd0g0amxWc2RDVTlNdVl2QllQRktCTEJkWU52K09ZeW1MTFJGTU9odVFuSDhuZFRRQVV6VjJXRTF4dHdlVG1wSVFzdXdmVzRIdzAiLCJuYmYiOjE0Njc1MDg0NDh9Cg.jpCqzTo8nNVEW8ArK1NFBaqLx6kyJV6wPF8cAU6UGav6cfMc60o3m5DjwspN-JcyC14AlcNiPdWX8TEm1QFhtScb-bXo4WOJ0dNYXV8iI_eCTCcXMFjX4vgIHpb9xfjv";
			val = $@"{{ ""chain"": [""{val}""] }}";

			return Encoding.UTF8.GetBytes(val);
		}

		/// <summary>
		///     The bot's Character Creator skin, captured from a real 1.26 client. grey_skin.json is
		///     that same capture with every opaque pixel of the body atlas set to 0x5A, the 'Z' of the
		///     original ZZZ skin, so TheGrey is grey again while still wearing a skin the game accepts.
		///     Embedded here in the core assembly, next to the code that loads it. It used to live in
		///     MiNET.Client, so this method only worked in a process that had loaded the client: the
		///     server itself could not build the bot's appearance, which is exactly what a server-side
		///     stand-in player needs.
		/// </summary>
		private const string BotSkinResource = "MiNET.Data.grey_skin.json";

		/// <summary>
		///     The vanilla player model, base64 as it goes on the wire. This travels with the skin:
		///     naming geometry.humanoid.custom in the resource patch and sending no definition draws
		///     nothing, tested against a real client one field at a time, and that is exactly what the
		///     old ZZZ skin did. Same model Geyser sends to Bedrock clients.
		/// </summary>
		private const string PlayerGeometry = "ewogICAgImZvcm1hdF92ZXJzaW9uIiA6ICIxLjEyLjAiLAogICAgIm1pbmVjcmFmdDpnZW9tZXRyeSIgOiBbCiAgICAgICAgewogICAgICAgICAgICAiYm9uZXMiIDogWwogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJib2R5IiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJ3YWlzdCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMC4wLCAyNC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAid2Fpc3QiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMTIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC01LjAsIDguMCwgMy4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDEwLCAxNiwgMSBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMCwgMCBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJjYXBlIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJib2R5IiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyAwLjAsIDI0LjAsIDMuMCBdLAogICAgICAgICAgICAgICAgICAgICJyb3RhdGlvbiIgOiBbIDAuMCwgMTgwLjAsIDAuMCBdCiAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgIF0sCiAgICAgICAgICAgICJkZXNjcmlwdGlvbiIgOiB7CiAgICAgICAgICAgICAgICAiaWRlbnRpZmllciIgOiAiZ2VvbWV0cnkuY2FwZSIsCiAgICAgICAgICAgICAgICAidGV4dHVyZV9oZWlnaHQiIDogMzIsCiAgICAgICAgICAgICAgICAidGV4dHVyZV93aWR0aCIgOiA2NAogICAgICAgICAgICB9CiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAgICJib25lcyIgOiBbCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJvb3QiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTQuMCwgMTIuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA4LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMTYsIDE2IF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogIndhaXN0IiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyAwLjAsIDI0LjAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJ3YWlzdCIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAicm9vdCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMC4wLCAxMi4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTQuMCwgMjQuMCwgLTQuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA4LCA4LCA4IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAwLCAwIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImhlYWQiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImNhcGUiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQsIDMuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJpbmZsYXRlIiA6IDAuNTAsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTQuMCwgMjQuMCwgLTQuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA4LCA4LCA4IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAzMiwgMCBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJoYXQiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImhlYWQiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIDQuMCwgMTIuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA0LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMzIsIDQ4IF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImxlZnRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDUuMCwgMjIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgImluZmxhdGUiIDogMC4yNTAsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgNC4wLCAxMi4wLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDQsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyA0OCwgNDggXQogICAgICAgICAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgICAgICAgICAgXSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAibGVmdFNsZWV2ZSIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAibGVmdEFybSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgNS4wLCAyMi4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAibGVmdEl0ZW0iLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImxlZnRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDYuMCwgMTUuMCwgMS4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC04LjAsIDEyLjAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgNCwgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDQwLCAxNiBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJyaWdodEFybSIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAiYm9keSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTUuMCwgMjIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgImluZmxhdGUiIDogMC4yNTAsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTguMCwgMTIuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA0LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgNDAsIDMyIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJpZ2h0U2xlZXZlIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJyaWdodEFybSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTUuMCwgMjIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImxvY2F0b3JzIiA6IHsKICAgICAgICAgICAgICAgICAgICAgICAgImxlYWRfaG9sZCIgOiBbIC02LCAxNSwgMSBdCiAgICAgICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAicmlnaHRJdGVtIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJyaWdodEFybSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTYsIDE1LCAxIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC0wLjEwLCAwLjAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgNCwgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDE2LCA0OCBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJsZWZ0TGVnIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJyb290IiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyAxLjkwLCAxMi4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAiaW5mbGF0ZSIgOiAwLjI1MCwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtMC4xMCwgMC4wLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDQsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAwLCA0OCBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJsZWZ0UGFudHMiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImxlZnRMZWciLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDEuOTAsIDEyLjAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtMy45MCwgMC4wLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDQsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAwLCAxNiBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJyaWdodExlZyIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAicm9vdCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTEuOTAsIDEyLjAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJpbmZsYXRlIiA6IDAuMjUwLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC0zLjkwLCAwLjAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgNCwgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDAsIDMyIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJpZ2h0UGFudHMiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogInJpZ2h0TGVnIiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyAtMS45MCwgMTIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgImluZmxhdGUiIDogMC4yNTAsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTQuMCwgMTIuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA4LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMTYsIDMyIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImphY2tldCIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAiYm9keSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMC4wLCAyNC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfQogICAgICAgICAgICBdLAogICAgICAgICAgICAiZGVzY3JpcHRpb24iIDogewogICAgICAgICAgICAgICAgImlkZW50aWZpZXIiIDogImdlb21ldHJ5Lmh1bWFub2lkLmN1c3RvbSIsCiAgICAgICAgICAgICAgICAidGV4dHVyZV9oZWlnaHQiIDogNjQsCiAgICAgICAgICAgICAgICAidGV4dHVyZV93aWR0aCIgOiA2NCwKICAgICAgICAgICAgICAgICJ2aXNpYmxlX2JvdW5kc19oZWlnaHQiIDogMiwKICAgICAgICAgICAgICAgICJ2aXNpYmxlX2JvdW5kc19vZmZzZXQiIDogWyAwLCAxLCAwIF0sCiAgICAgICAgICAgICAgICAidmlzaWJsZV9ib3VuZHNfd2lkdGgiIDogMQogICAgICAgICAgICB9CiAgICAgICAgfSwKICAgICAgICB7CiAgICAgICAgICAgICJib25lcyIgOiBbCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJvb3QiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAid2Fpc3QiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogInJvb3QiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMTIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgOCwgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDE2LCAxNiBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJib2R5IiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJ3YWlzdCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMC4wLCAyNC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTQuMCwgMjQuMCwgLTQuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA4LCA4LCA4IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAwLCAwIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImhlYWQiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgImluZmxhdGUiIDogMC41MCwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtNC4wLCAyNC4wLCAtNC4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDgsIDgsIDggXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDMyLCAwIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImhhdCIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAiaGVhZCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMC4wLCAyNC4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTMuOTAsIDAuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA0LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMCwgMTYgXQogICAgICAgICAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgICAgICAgICAgXSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAicmlnaHRMZWciLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogInJvb3QiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIC0xLjkwLCAxMi4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAiaW5mbGF0ZSIgOiAwLjI1MCwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtMy45MCwgMC4wLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDQsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAwLCAzMiBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJyaWdodFBhbnRzIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJyaWdodExlZyIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTEuOTAsIDEyLjAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtMC4xMCwgMC4wLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDQsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyAxNiwgNDggXQogICAgICAgICAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgICAgICAgICAgXSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAibGVmdExlZyIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAicm9vdCIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgMS45MCwgMTIuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgImluZmxhdGUiIDogMC4yNTAsCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgLTAuMTAsIDAuMCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyA0LCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMCwgNDggXQogICAgICAgICAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgICAgICAgICAgXSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAibGVmdFBhbnRzIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJsZWZ0TGVnIiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyAxLjkwLCAxMi4wLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAib3JpZ2luIiA6IFsgNC4wLCAxMS41MCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyAzLCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgMzIsIDQ4IF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImxlZnRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDUuMCwgMjEuNTAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJpbmZsYXRlIiA6IDAuMjUwLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIDQuMCwgMTEuNTAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgMywgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDQ4LCA0OCBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJsZWZ0U2xlZXZlIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJsZWZ0QXJtIiwKICAgICAgICAgICAgICAgICAgICAicGl2b3QiIDogWyA1LjAsIDIxLjUwLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAibGVmdEl0ZW0iLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImxlZnRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDYsIDE0LjUwLCAxIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgImN1YmVzIiA6IFsKICAgICAgICAgICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC03LjAsIDExLjUwLCAtMi4wIF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAic2l6ZSIgOiBbIDMsIDEyLCA0IF0sCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAidXYiIDogWyA0MCwgMTYgXQogICAgICAgICAgICAgICAgICAgICAgICB9CiAgICAgICAgICAgICAgICAgICAgXSwKICAgICAgICAgICAgICAgICAgICAibmFtZSIgOiAicmlnaHRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIC01LjAsIDIxLjUwLCAwLjAgXQogICAgICAgICAgICAgICAgfSwKICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAiY3ViZXMiIDogWwogICAgICAgICAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgICAgICAgICAiaW5mbGF0ZSIgOiAwLjI1MCwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJvcmlnaW4iIDogWyAtNy4wLCAxMS41MCwgLTIuMCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInNpemUiIDogWyAzLCAxMiwgNCBdLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgInV2IiA6IFsgNDAsIDMyIF0KICAgICAgICAgICAgICAgICAgICAgICAgfQogICAgICAgICAgICAgICAgICAgIF0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJpZ2h0U2xlZXZlIiwKICAgICAgICAgICAgICAgICAgICAicGFyZW50IiA6ICJyaWdodEFybSIsCiAgICAgICAgICAgICAgICAgICAgInBpdm90IiA6IFsgLTUuMCwgMjEuNTAsIDAuMCBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJsb2NhdG9ycyIgOiB7CiAgICAgICAgICAgICAgICAgICAgICAgICJsZWFkX2hvbGQiIDogWyAtNiwgMTQuNTAsIDEgXQogICAgICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogInJpZ2h0SXRlbSIsCiAgICAgICAgICAgICAgICAgICAgInBhcmVudCIgOiAicmlnaHRBcm0iLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIC02LCAxNC41MCwgMSBdCiAgICAgICAgICAgICAgICB9LAogICAgICAgICAgICAgICAgewogICAgICAgICAgICAgICAgICAgICJjdWJlcyIgOiBbCiAgICAgICAgICAgICAgICAgICAgICAgIHsKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJpbmZsYXRlIiA6IDAuMjUwLAogICAgICAgICAgICAgICAgICAgICAgICAgICAgIm9yaWdpbiIgOiBbIC00LjAsIDEyLjAsIC0yLjAgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJzaXplIiA6IFsgOCwgMTIsIDQgXSwKICAgICAgICAgICAgICAgICAgICAgICAgICAgICJ1diIgOiBbIDE2LCAzMiBdCiAgICAgICAgICAgICAgICAgICAgICAgIH0KICAgICAgICAgICAgICAgICAgICBdLAogICAgICAgICAgICAgICAgICAgICJuYW1lIiA6ICJqYWNrZXQiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQuMCwgMC4wIF0KICAgICAgICAgICAgICAgIH0sCiAgICAgICAgICAgICAgICB7CiAgICAgICAgICAgICAgICAgICAgIm5hbWUiIDogImNhcGUiLAogICAgICAgICAgICAgICAgICAgICJwYXJlbnQiIDogImJvZHkiLAogICAgICAgICAgICAgICAgICAgICJwaXZvdCIgOiBbIDAuMCwgMjQsIC0zLjAgXQogICAgICAgICAgICAgICAgfQogICAgICAgICAgICBdLAogICAgICAgICAgICAiZGVzY3JpcHRpb24iIDogewogICAgICAgICAgICAgICAgImlkZW50aWZpZXIiIDogImdlb21ldHJ5Lmh1bWFub2lkLmN1c3RvbVNsaW0iLAogICAgICAgICAgICAgICAgInRleHR1cmVfaGVpZ2h0IiA6IDY0LAogICAgICAgICAgICAgICAgInRleHR1cmVfd2lkdGgiIDogNjQsCiAgICAgICAgICAgICAgICAidmlzaWJsZV9ib3VuZHNfaGVpZ2h0IiA6IDIsCiAgICAgICAgICAgICAgICAidmlzaWJsZV9ib3VuZHNfb2Zmc2V0IiA6IFsgMCwgMSwgMCBdLAogICAgICAgICAgICAgICAgInZpc2libGVfYm91bmRzX3dpZHRoIiA6IDEKICAgICAgICAgICAgfQogICAgICAgIH0KICAgIF0KfQo=";

		private static ClientData LoadPersonaClientData()
		{
			// MINET_SKIN_JSON replaces the bot's appearance with a ClientData document captured from
			// a real client, so a skin that a real client is known to reject can be put on the wire
			// against both a reference server and ours and the two encodings compared.
			string overridePath = Environment.GetEnvironmentVariable("MINET_SKIN_JSON");
			if (!string.IsNullOrEmpty(overridePath))
			{
				return JsonConvert.DeserializeObject<ClientData>(File.ReadAllText(overridePath));
			}

			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				using Stream stream = assembly.GetManifestResourceStream(BotSkinResource);
				if (stream == null) continue;

				using var reader = new StreamReader(stream);
				return JsonConvert.DeserializeObject<ClientData>(reader.ReadToEnd());
			}

			throw new FileNotFoundException($"{BotSkinResource} not found; the bot has no skin to log in with");
		}

		/// <summary>
		///     The bot's skin, ready to send. Recolour <see cref="ClientData.SkinData" /> and call
		///     <see cref="ClientData.ToSkin" /> to make one for McpePlayerSkin; the id has to move with
		///     the pixels or the client keeps showing the skin it already has under that id.
		/// </summary>
		/// <summary>
		///     The humanoid geometry a skin needs to have anything to draw: geometry.cape,
		///     geometry.humanoid.custom and geometry.humanoid.customSlim. Any skin the server builds
		///     itself (NPCs, stand-in players) has to ship this, because naming
		///     geometry.humanoid.custom without defining it leaves the client with nothing to render.
		/// </summary>
		public static string DefaultPlayerGeometry => Encoding.UTF8.GetString(Convert.FromBase64String(PlayerGeometry));

		public static ClientData BuildBotClientData(string username)
		{
			ClientData clientData = LoadPersonaClientData();
			clientData.SkinGeometryData = PlayerGeometry;

			StampSkinId(clientData, username);

			return clientData;
		}

		/// <summary>
		///     The captured skin carries the id of the account it came from, so every bot and that
		///     player would present the same skin identity. Derived from the username and the texture
		///     instead: unique per bot, and the SAME on every login, because a skin id that changes each
		///     time is a new skin to the client every time it sees the player. The texture is in the
		///     hash because the client caches by id and will not look at the pixels again for an id it
		///     already knows, so recolouring without moving the id shows the old skin.
		/// </summary>
		public static void StampSkinId(ClientData clientData, string username)
		{
			byte[] identity = Encoding.UTF8.GetBytes(username + clientData.SkinData);

			clientData.SkinId = $"{DeriveStableIdentity(username)}.{Convert.ToHexString(MD5.HashData(identity)).ToLowerInvariant().Substring(0, 16)}";
		}

		public static byte[] EncodeSkinJwt(ECDsa newKey, string username)
		{
			// The bot's appearance is a real Character Creator skin, captured from a live 1.26 client
			// and shipped as MiNET.Client/Data/persona_skin.json. It used to be a hand-built classic
			// skin: 64x64 of flat grey, a resource patch naming geometry.humanoid.custom, and no
			// geometry to back it. Vanilla BDS relayed it without complaint and the receiving client
			// rendered a default character, because a skin that names a model it does not define has
			// nothing to draw. Rather than keep guessing what a valid classic skin needs, this sends
			// one that a real client authored and that the game already accepts.
			ClientData clientData = BuildBotClientData(username);

			clientData.ClientRandomId = new Random().Next();
			clientData.SelfSignedId = Guid.NewGuid().ToString();
			clientData.ServerAddress = "yodamine.com:19132";
			clientData.ThirdPartyName = username;
			clientData.ThirdPartyNameOnly = false;
			clientData.DeviceId = Guid.NewGuid().ToString();
			clientData.DeviceModel = "MiNET CLIENT";
			clientData.GameVersion = McpeProtocolInfo.GameVersion;

			string skinData = JsonConvert.SerializeObject(clientData);

			ECDsa signKey = newKey;
			string b64Key = newKey.ExportSubjectPublicKeyInfo().EncodeBase64();

			string val = JWT.Encode(skinData, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});

			return Encoding.UTF8.GetBytes(val);
		}

		public static byte[] CompressJwtBytes(byte[] certChain, byte[] skinData, CompressionLevel compressionLevel)
		{
			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				{
					{
						byte[] lenBytes = BitConverter.GetBytes(certChain.Length);
						stream.Write(lenBytes, 0, lenBytes.Length);
						stream.Write(certChain, 0, certChain.Length);
					}
					{
						byte[] lenBytes = BitConverter.GetBytes(skinData.Length);
						stream.Write(lenBytes, 0, lenBytes.Length);
						stream.Write(skinData, 0, skinData.Length);
					}
				}

				var bytes = stream.ToArray();

				return bytes;
			}
		}
	}
}