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
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using Jose;
using log4net;
using MiNET.Net;
using MiNET.Utils.Skins;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static byte[] Encrypt(ReadOnlyMemory<byte> payload, CryptoContext cryptoContext)
		{
			// hash
			int hashPoolLen = 8 + payload.Length + cryptoContext.Key.Length;
			var hashBufferPooled = ArrayPool<byte>.Shared.Rent(hashPoolLen);
			Span<byte> hashBuffer = hashBufferPooled.AsSpan();
			BitConverter.GetBytes(Interlocked.Increment(ref cryptoContext.SendCounter)).CopyTo(hashBuffer.Slice(0, 8));
			payload.Span.CopyTo(hashBuffer.Slice(8));
			cryptoContext.Key.CopyTo(hashBuffer.Slice(8 + payload.Length));
			using var hasher =  SHA256.Create();
			Span<byte> validationCheckSum = hasher.ComputeHash(hashBufferPooled, 0, hashPoolLen).AsSpan(0, 8);
			ArrayPool<byte>.Shared.Return(hashBufferPooled);

			IBufferedCipher cipher = cryptoContext.Encryptor;
			var encrypted = new byte[payload.Length + 8];
			int length = cipher.ProcessBytes(payload.ToArray(), encrypted, 0);
			cipher.ProcessBytes(validationCheckSum.ToArray(), encrypted, length);

			return encrypted;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReadOnlyMemory<byte> Decrypt(ReadOnlyMemory<byte> payload, CryptoContext cryptoContext)
		{
			IBufferedCipher cipher = cryptoContext.Decryptor;

			ReadOnlyMemory<byte> clear = cipher.ProcessBytes(payload.ToArray());
			//TODO: Verify hash!
			return clear.Slice(0, clear.Length - 8);
		}

		// CLIENT TO SERVER STUFF

		public static AsymmetricCipherKeyPair GenerateClientKey()
		{
			var generator = new ECKeyPairGenerator("ECDH");
			generator.Init(new ECKeyGenerationParameters(new DerObjectIdentifier("1.3.132.0.34"), SecureRandom.GetInstance("SHA256PRNG")));
			return generator.GenerateKeyPair();
		}

		public static byte[] EncodeJwt(string username, AsymmetricCipherKeyPair newKey, bool isEmulator)
		{
			long iat = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			long exp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();

			ECDsa signKey = ConvertToSingKeyFormat(newKey);
			string b64Key = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(newKey.Public).GetEncoded().EncodeBase64();

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

		public static byte[] EncodeSkinJwt(AsymmetricCipherKeyPair newKey, string username)
		{
			var resourcePatch = new SkinResourcePatch() {Geometry = new GeometryIdentifier() {Default = "geometry.humanoid.customSlim"}};
			var skin = new Skin
			{
				SkinId = $"{Guid.NewGuid().ToString()}.CustomSlim",
				SkinResourcePatch = resourcePatch,
				Slim = true,
				Height = 32,
				Width = 64,
				Data = Encoding.Default.GetBytes(new string('Z', 8192)),
			};

			string resourcePatchData = Convert.ToBase64String(Encoding.Default.GetBytes(Skin.ToJson(resourcePatch)));
			string skin64 = Convert.ToBase64String(skin.Data);


			string skinData = $@"
{{
	""AnimatedImageData"": [],
	""ArmSize"": """",
	""CapeData"": """",
	""CapeId"": """",
	""CapeImageHeight"": 0,
	""CapeImageWidth"": 0,
	""CapeOnClassicSkin"": false,
	""ClientRandomId"": {new Random().Next()},
	""CurrentInputMode"": 1,
	""DefaultInputMode"": 1,
	""DeviceId"": ""{Guid.NewGuid().ToString()}"",
	""DeviceModel"": ""MiNET CLIENT"",
	""DeviceOS"": 7,
	""GameVersion"": ""{McpeProtocolInfo.GameVersion}"",
	""GuiScale"": -1,
	""LanguageCode"": ""en_US"",
	""PersonaPieces"": [],
	""PersonaSkin"": false,
	""PieceTintColors"": [],
	""PlatformOfflineId"": """",
	""PlatformOnlineId"": """",
	""PlayFabId"": """",
	""PremiumSkin"": false,
	""SelfSignedId"": ""{Guid.NewGuid().ToString()}"",
	""ServerAddress"": ""yodamine.com:19132"",
	""SkinAnimationData"": """",
	""SkinColor"": ""#0"",
	""SkinData"": ""{skin64}"",
	""SkinGeometryData"": """",
	""SkinId"": ""{skin.SkinId}"",
	""SkinImageHeight"": {skin.Height},
	""SkinImageWidth"": {skin.Width},
	""SkinResourcePatch"": ""{resourcePatchData}"",
	""ThirdPartyName"": ""{username}"",
	""ThirdPartyNameOnly"": false,
	""UIProfile"": 0
}}
";

			ECDsa signKey = ConvertToSingKeyFormat(newKey);
			string b64Key = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(newKey.Public).GetEncoded().EncodeBase64();

			string val = JWT.Encode(skinData, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});

			return Encoding.UTF8.GetBytes(val);
		}

		private static ECDsa ConvertToSingKeyFormat(AsymmetricCipherKeyPair key)
		{
			ECPublicKeyParameters pubAsyKey = (ECPublicKeyParameters) key.Public;
			ECPrivateKeyParameters privAsyKey = (ECPrivateKeyParameters) key.Private;

			var signParam = new ECParameters
			{
				Curve = ECCurve.NamedCurves.nistP384,
				Q =
				{
					X = pubAsyKey.Q.AffineXCoord.GetEncoded(),
					Y = pubAsyKey.Q.AffineYCoord.GetEncoded()
				}
			};
			signParam.D = FixDSize(privAsyKey.D.ToByteArrayUnsigned(), signParam.Q.X.Length);
			signParam.Validate();

			return ECDsa.Create(signParam);
		}

		public static byte[] FixDSize(byte[] input, int expectedSize)
		{
			if (input.Length == expectedSize)
			{
				return input;
			}

			byte[] tmp;

			if (input.Length < expectedSize)
			{
				tmp = new byte[expectedSize];
				Buffer.BlockCopy(input, 0, tmp, expectedSize - input.Length, input.Length);
				return tmp;
			}

			if (input.Length > expectedSize + 1 || input[0] != 0)
			{
				throw new InvalidOperationException();
			}

			tmp = new byte[expectedSize];
			Buffer.BlockCopy(input, 1, tmp, 0, expectedSize);
			return tmp;
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