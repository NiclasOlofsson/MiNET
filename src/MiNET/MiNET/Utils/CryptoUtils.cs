using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Jose;
using log4net;
using MiNET.Net;

namespace MiNET.Utils
{
	public static class CryptoUtils
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CryptoUtils));

		public static byte[] GetDerEncoded(this ECDiffieHellmanPublicKey key)
		{
			byte[] asn = new byte[24]
			{
				0x30, 0x76, 0x30, 0x10, 0x6, 0x7, 0x2a, 0x86, 0x48, 0xce, 0x3d, 0x2,
				0x1, 0x6, 0x5, 0x2b, 0x81, 0x4, 0x0, 0x22, 0x3, 0x62, 0x0, 0x4
			};

			return asn.Concat(key.ToByteArray().Skip(8)).ToArray();
		}

		public static ECDiffieHellmanPublicKey CreateEcDiffieHellmanPublicKey(string clientPubKeyString)
		{
			byte[] clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);
			clientPublicKeyBlob = FixPublicKey(clientPublicKeyBlob.Skip(23).ToArray());

			ECDiffieHellmanPublicKey clientKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);
			return clientKey;
		}

		private static byte[] FixPublicKey(byte[] publicKeyBlob)
		{
			var keyType = new byte[] {0x45, 0x43, 0x4b, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};

			return keyType.Concat(keyLength).Concat(publicKeyBlob.Skip(1)).ToArray();
		}

		public static ECDiffieHellmanPublicKey ImportEccPublicKeyFromCertificate(X509Certificate2 cert)
		{
			var keyType = new byte[] {0x45, 0x43, 0x4b, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};
			var key = cert.PublicKey.EncodedKeyValue.RawData.Skip(1);
			var keyImport = keyType.Concat(keyLength).Concat(key).ToArray();

			//Assert.AreEqual(privateKey, keyImport);

			return ECDiffieHellmanCngPublicKey.FromByteArray(keyImport, CngKeyBlobFormat.EccPublicBlob);
		}

		/// <summary>
		/// Used to create a CngKey that can be used to verify JWT content.
		/// </summary>
		/// <param name="clientPubKeyString"></param>
		/// <returns></returns>
		public static CngKey ImportECDsaCngKeyFromString(string clientPubKeyString)
		{
			byte[] clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);
			byte[] key = clientPublicKeyBlob.Skip(23).ToArray();

			var keyType = new byte[] {0x45, 0x43, 0x53, 0x33};
			var keyLength = new byte[] {0x30, 0x00, 0x00, 0x00};

			var keyImport = keyType.Concat(keyLength).Concat(key.Skip(1)).ToArray();

			var cngKey = CngKey.Import(keyImport, CngKeyBlobFormat.EccPublicBlob);
			var crypto = new ECDsaCng(cngKey);

			return crypto.Key;
		}

		public static byte[] ImportECDsaCngKeyFromCngKey(byte[] inKey)
		{
			inKey[2] = 83;
			return inKey;
		}

		public static byte[] Encrypt(byte[] payload, CryptoContext cryptoContext)
		{
			var csEncrypt = cryptoContext.CryptoStreamOut;
			var output = cryptoContext.OutputStream;
			output.Position = 0;
			output.SetLength(0);

			using (MemoryStream hashStream = new MemoryStream())
			{
				// hash

				SHA256Managed crypt = new SHA256Managed();

				hashStream.Write(BitConverter.GetBytes(Interlocked.Increment(ref cryptoContext.SendCounter)), 0, 8);
				hashStream.Write(payload, 0, payload.Length);
				hashStream.Write(cryptoContext.Algorithm.Key, 0, cryptoContext.Algorithm.Key.Length);
				var hashBuffer = hashStream.ToArray();

				byte[] validationCheckSum = crypt.ComputeHash(hashBuffer, 0, hashBuffer.Length);

				byte[] content = payload.Concat(validationCheckSum.Take(8)).ToArray();

				csEncrypt.Write(content, 0, content.Length);
				csEncrypt.Flush();
			}

			return output.ToArray();
		}

		public static byte[] Decrypt(byte[] payload, CryptoContext cryptoContext)
		{
			byte[] checksum;
			byte[] clearBytes;

			using (MemoryStream clearBuffer = new MemoryStream())
			{
				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Full payload\n{Package.HexDump(payload)}");

				var input = cryptoContext.InputStream;
				var csDecrypt = cryptoContext.CryptoStreamIn;

				input.Position = 0;
				input.SetLength(0);
				input.Write(payload, 0, payload.Length);
				input.Position = 0;

				var buffer = new byte[payload.Length];
				var read = csDecrypt.Read(buffer, 0, buffer.Length);
				if (read <= 0) Log.Warn("Read 0 lenght from crypto stream");
				clearBuffer.Write(buffer, 0, read);
				csDecrypt.Flush();

				var fullResult = clearBuffer.ToArray();

				//if (Log.IsDebugEnabled)
				//	Log.Debug($"Full content\n{Package.HexDump(fullResult)}");

				clearBytes = (byte[]) fullResult.Take(fullResult.Length - 8).ToArray();
				checksum = fullResult.Skip(fullResult.Length - 8).ToArray();
			}

			return clearBytes;
		}


		// CLIENT TO SERVER STUFF

		public static CngKey GenerateClientKey()
		{
			CngKey newKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP384, null, new CngKeyCreationParameters() {ExportPolicy = CngExportPolicies.AllowPlaintextExport, KeyUsage = CngKeyUsages.AllUsages});
			return newKey;
		}

		public static byte[] EncodeJwt(string username, CngKey newKey)
		{
			byte[] t = ImportECDsaCngKeyFromCngKey(newKey.Export(CngKeyBlobFormat.EccPrivateBlob));
			CngKey tk = CngKey.Import(t, CngKeyBlobFormat.EccPrivateBlob);

			ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(newKey);
			ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;

			string b64Key = Convert.ToBase64String(ecKey.PublicKey.GetDerEncoded());

			long exp = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds();

			CertificateData certificateData = new CertificateData
			{
				Exp = exp,
				Iat = exp,
				ExtraData = new ExtraData
				{
					DisplayName = username,
					//Identity = "af6f7c5e-fcea-3e43-bf3a-e005e400e579",
					Identity = Guid.NewGuid().ToString(),
				},
				Iss = "self",
				IdentityPublicKey = b64Key,
				CertificateAuthority = true,
				Nbf = exp,
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
			string val = JWT.Encode(certificateData, tk, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});

			Log.Warn(JWT.Payload(val));

			Log.Warn(string.Join(";", JWT.Headers(val)));

			//val = "eyJhbGciOiJFUzM4NCIsIng1dSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFREVLck5xdk93Y25iV3I5aUtVQ0MyeklFRmZ6Q0VnUEhQdG5Kd3VEdnZ3VjVtd1E3QzNkWmhqd0g0amxWc2RDVTlNdVl2QllQRktCTEJkWU52K09ZeW1MTFJGTU9odVFuSDhuZFRRQVV6VjJXRTF4dHdlVG1wSVFzdXdmVzRIdzAifQo.eyJleHAiOjE0Njc1MDg0NDksImV4dHJhRGF0YSI6eyJkaXNwbGF5TmFtZSI6Imd1cnVueHgiLCJpZGVudGl0eSI6IjRlMDE5OWM2LTdjZmQtMzU1MC1iNjc2LTc0Mzk4ZTBhNWYxYSJ9LCJpZGVudGl0eVB1YmxpY0tleSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFREVLck5xdk93Y25iV3I5aUtVQ0MyeklFRmZ6Q0VnUEhQdG5Kd3VEdnZ3VjVtd1E3QzNkWmhqd0g0amxWc2RDVTlNdVl2QllQRktCTEJkWU52K09ZeW1MTFJGTU9odVFuSDhuZFRRQVV6VjJXRTF4dHdlVG1wSVFzdXdmVzRIdzAiLCJuYmYiOjE0Njc1MDg0NDh9Cg.jpCqzTo8nNVEW8ArK1NFBaqLx6kyJV6wPF8cAU6UGav6cfMc60o3m5DjwspN-JcyC14AlcNiPdWX8TEm1QFhtScb-bXo4WOJ0dNYXV8iI_eCTCcXMFjX4vgIHpb9xfjv";
			val = $@"{{ ""chain"": [""{val}""] }}";
			
			return Encoding.UTF8.GetBytes(val);
		}

		public static byte[] EncodeSkinJwt(CngKey newKey)
		{
			byte[] t = ImportECDsaCngKeyFromCngKey(newKey.Export(CngKeyBlobFormat.EccPrivateBlob));
			CngKey tk = CngKey.Import(t, CngKeyBlobFormat.EccPrivateBlob);

			ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(newKey);
			ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;

			var b64Key = Base64Url.Encode(ecKey.PublicKey.GetDerEncoded());

			Skin skin = new Skin
			{
				Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192)),
				SkinType = "Standard_Custom"
			};

			string skin64 = Convert.ToBase64String(skin.Texture);

			string skinData = $@"
{{
	""AdRole"": 2,
	""ClientRandomId"": {new Random().Next()},
	""ServerAddress"": ""pe.mineplex.com:19132"",
	""SkinData"": ""{skin64}"",
	""SkinId"": ""{skin.SkinType}"",
	""TenantId"": """"
}}";

			string val = JWT.Encode(skinData, tk, JwsAlgorithm.ES384, new Dictionary<string, object> { { "x5u", b64Key } });

			return Encoding.UTF8.GetBytes(val);
		}

		public static byte[] CompressJwtBytes(byte[] certChain, byte[] skinData, CompressionLevel compressionLevel)
		{
			MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream();
			stream.WriteByte(0x78);
			int checksum;
			switch (compressionLevel)
			{
				case CompressionLevel.Optimal:
					stream.WriteByte(0xda);
					break;
				case CompressionLevel.Fastest:
					stream.WriteByte(0x9c);
					break;
				case CompressionLevel.NoCompression:
					stream.WriteByte(0x01);
					break;
			}
			using (var compressStream = new ZLibStream(stream, compressionLevel, true))
			{
				{
					byte[] lenBytes = BitConverter.GetBytes(certChain.Length);
					//Array.Reverse(lenBytes);
					compressStream.Write(lenBytes, 0, lenBytes.Length); // ??
					compressStream.Write(certChain, 0, certChain.Length);
				}
				{
					byte[] lenBytes = BitConverter.GetBytes(skinData.Length);
					//Array.Reverse(lenBytes);
					compressStream.Write(lenBytes, 0, lenBytes.Length); // ??
					compressStream.Write(skinData, 0, skinData.Length);
				}
				checksum = compressStream.Checksum;
			}

			byte[] checksumBytes = BitConverter.GetBytes(checksum);
			if (BitConverter.IsLittleEndian)
			{
				// Adler32 checksum is big-endian
				Array.Reverse(checksumBytes);
			}
			stream.Write(checksumBytes, 0, checksumBytes.Length);

			var bytes = stream.ToArray();
			stream.Close();

			return bytes;
		}

	}
}