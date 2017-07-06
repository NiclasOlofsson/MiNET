using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using fNbt;
using Jose;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;
using NUnit.Framework;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace MiNET
{
	[TestFixture]
	public class MinetCrytopTest
	{
		[Test]
		public void TestCertMangling()
		{
			string certString = @"MIICSjCCAdECCQDje/no7mXkVzAKBggqhkjOPQQDAjCBjjELMAkGA1UEBhMCVVMx
EzARBgNVBAgMCkNhbGlmb3JuaWExFjAUBgNVBAcMDU1vdW50YWluIFZpZXcxFDAS
BgNVBAoMC0dvb2dsZSwgSW5jMRcwFQYDVQQDDA53d3cuZ29vZ2xlLmNvbTEjMCEG
CSqGSIb3DQEJARYUZ29sYW5nLWRldkBnbWFpbC5jb20wHhcNMTIwNTIxMDYxMDM0
WhcNMjIwNTE5MDYxMDM0WjCBjjELMAkGA1UEBhMCVVMxEzARBgNVBAgMCkNhbGlm
b3JuaWExFjAUBgNVBAcMDU1vdW50YWluIFZpZXcxFDASBgNVBAoMC0dvb2dsZSwg
SW5jMRcwFQYDVQQDDA53d3cuZ29vZ2xlLmNvbTEjMCEGCSqGSIb3DQEJARYUZ29s
YW5nLWRldkBnbWFpbC5jb20wdjAQBgcqhkjOPQIBBgUrgQQAIgNiAARRuzRNIKRK
jIktEmXanNmrTR/q/FaHXLhWRZ6nHWe26Fw7Rsrbk+VjGy4vfWtNn7xSFKrOu5ze
qxKnmE0h5E480MNgrUiRkaGO2GMJJVmxx20aqkXOk59U8yGA4CghE6MwCgYIKoZI
zj0EAwIDZwAwZAIwBZEN8gvmRmfeP/9C1PRLzODIY4JqWub2PLRT4mv9GU+yw3Gr
PU9A3CHMdEcdw/MEAjBBO1lId8KOCh9UZunsSMfqXiVurpzmhWd6VYZ/32G+M+Mh
3yILeYQzllt/g0rKVRk=";

			X509Certificate2 c = new X509Certificate2();
			c.Import(Convert.FromBase64String(certString));
			Assert.AreEqual("E=golang-dev@gmail.com, CN=www.google.com, O=\"Google, Inc\", L=Mountain View, S=California, C=US", c.Issuer);
			//Assert.AreEqual("CN=Microsoft Corporate Root CA, O=Microsoft Corporation", c.Subject);
			Assert.AreEqual("X509", c.GetFormat());
			Assert.AreEqual("1.2.840.10045.2.1", c.GetKeyAlgorithm());
			Assert.AreEqual("06052B81040022", c.GetKeyAlgorithmParametersString());
			Assert.AreEqual("ECC", c.PublicKey.Oid.FriendlyName);
			ECDiffieHellmanPublicKey certKey = CryptoUtils.ImportEccPublicKeyFromCertificate(c);
			//Console.WriteLine(certKey.ToXmlString());

			// https://blogs.msdn.microsoft.com/shawnfa/2007/01/22/elliptic-curve-diffie-hellman/
			// http://stackoverflow.com/questions/11266711/using-cngkey-to-generate-rsa-key-pair-in-pem-dkim-compatible-using-c-simi
			{
				string input = "eyJhbGciOiJFUzM4NCIsIng1dSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFN25uWnBDZnhtQ3JTd0RkQnY3ZUJYWE10S2hyb3hPcmlFcjNobU1PSkF1dy9acFFYajFLNUdHdEhTNENwRk50dGQxSllBS1lvSnhZZ2F5a3BpZTBFeUF2M3FpSzZ1dElIMnFuT0F0M1ZOclFZWGZJWkpTL1ZSZTNJbDhQZ3U5Q0IifQo.eyJleHAiOjE0NjQ5ODM4NDUsImV4dHJhRGF0YSI6eyJkaXNwbGF5TmFtZSI6Imd1cnVueCIsImlkZW50aXR5IjoiYWY2ZjdjNWUtZmNlYS0zZTQzLWJmM2EtZTAwNWU0MDBlNTc4In0sImlkZW50aXR5UHVibGljS2V5IjoiTUhZd0VBWUhLb1pJemowQ0FRWUZLNEVFQUNJRFlnQUU3bm5acENmeG1DclN3RGRCdjdlQlhYTXRLaHJveE9yaUVyM2htTU9KQXV3L1pwUVhqMUs1R0d0SFM0Q3BGTnR0ZDFKWUFLWW9KeFlnYXlrcGllMEV5QXYzcWlLNnV0SUgycW5PQXQzVk5yUVlYZklaSlMvVlJlM0lsOFBndTlDQiIsIm5iZiI6MTQ2NDk4Mzg0NH0K.4OrvYYbX09iwOkz-7_N_5yEejuATcUogEbe69fB-kr7r6sH_qSu6bxp9L64SEgABb0rU7tyYCLVnaCSQjd9Dvb34WI9EducgOPJ92qHspcpXr7j716LDfhZE31ksMtWQ";

				ECDiffieHellmanPublicKey rootKey = CryptoUtils.CreateEcDiffieHellmanPublicKey("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE8ELkixyLcwlZryUQcu1TvPOmI2B7vX83ndnWRUaXm74wFfa5f/lwQNTfrLVHa2PmenpGI6JhIMUJaWZrjmMj90NoKNFSNBuKdm8rYiXsfaz3K36x/1U26HpG0ZxK/V1V");

				Console.WriteLine($"Root Public Key:\n{rootKey.ToXmlString()}");
				CngKey key = CngKey.Import(rootKey.ToByteArray(), CngKeyBlobFormat.EccPublicBlob);

				Console.WriteLine("Key family: " + key.AlgorithmGroup);
				//   "identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7nnZpCfxmCrSwDdBv7eBXXMtKhroxOriEr3hmMOJAuw/ZpQXj1K5GGtHS4CpFNttd1JYAKYoJxYgaykpie0EyAv3qiK6utIH2qnOAt3VNrQYXfIZJS/VRe3Il8Pgu9CB",

				var newKey = CryptoUtils.ImportECDsaCngKeyFromString("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7nnZpCfxmCrSwDdBv7eBXXMtKhroxOriEr3hmMOJAuw/ZpQXj1K5GGtHS4CpFNttd1JYAKYoJxYgaykpie0EyAv3qiK6utIH2qnOAt3VNrQYXfIZJS/VRe3Il8Pgu9CB");
				string decoded = JWT.Decode(input, newKey);
				//Assert.AreEqual("", decoded);


				//ECDsaCng t = new ECDsaCng();
				//t.HashAlgorithm = CngAlgorithm.ECDiffieHellmanP384;
				//t.KeySize = 384;
				//byte[] test = t.Key.Export(CngKeyBlobFormat.EccPublicBlob);
				//Assert.AreEqual(test, newKey);

				//string decoded = JWT.Decode(input, t.Key);
			}

			// Private key (in reality this is not necessary since we will generate it)
			AsymmetricKeyParameter privKey = PrivateKeyFactory.CreateKey(Base64Url.Decode("MB8CAQAwEAYHKoZIzj0CAQYFK4EEACIECDAGAgEBBAEB"));
			PrivateKeyInfo privKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privKey);
			byte[] derKey = privKeyInfo.GetDerEncoded();
			CngKey privCngKey = CngKey.Import(derKey, CngKeyBlobFormat.Pkcs8PrivateBlob);


			Console.WriteLine(privKeyInfo.PrivateKeyAlgorithm.Algorithm);
			Console.WriteLine(privCngKey.Algorithm.Algorithm);

			// Public key
			ECDiffieHellmanPublicKey clientKey = CryptoUtils.CreateEcDiffieHellmanPublicKey("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs");

			// EC key to generate shared secret

			ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(privCngKey);
			ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
			ecKey.SecretPrepend = new byte[128]; // Server token
			//ecKey.SecretPrepend = new byte[0]; // Server token

			Console.WriteLine(ecKey.HashAlgorithm);
			Console.WriteLine(ecKey.KeyExchangeAlgorithm);

			byte[] secret = ecKey.DeriveKeyMaterial(clientKey);

			Console.WriteLine(Package.HexDump(secret));
			Console.WriteLine(Package.HexDump(Base64Url.Decode("ZOBpyzki/M8UZv5tiBih048eYOBVPkQE3r5Fl0gmUP4=")));
			Console.WriteLine(Package.HexDump(Base64Url.Decode("DEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyod")));

			//Console.WriteLine(Package.HexDump(Base64Url.Decode("DEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyod")));
		}

		[Test]
		public void TestGenerateSecret()
		{
			string keyString = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs";
			byte[] keyBytes = Base64Url.Decode(keyString);

			ECDiffieHellmanPublicKey clientKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(keyString);

			ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(384);
			ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
			ecKey.SecretPrepend = new byte[128]; // Server token
			//ecKey.SecretPrepend = new byte[0]; // Server token

			Console.WriteLine(ecKey.HashAlgorithm);
			Console.WriteLine(ecKey.KeyExchangeAlgorithm);

			byte[] secret = ecKey.DeriveKeyMaterial(clientKey);
		}

		[Test]
		public void TestKeyMangling()
		{
			string keyString = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs";
			byte[] keyBytes = Base64Url.Decode(keyString);

			ECDiffieHellmanPublicKey clientKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(keyString);

			byte[] outBytes = clientKey.GetDerEncoded();

			string outString = Convert.ToBase64String(outBytes);

			Console.WriteLine(Package.HexDump(keyBytes));
			Console.WriteLine(Package.HexDump(outBytes));

			Assert.AreEqual(keyBytes, outBytes);

			Assert.AreEqual(keyString, outString);
		}

		[Test]
		public void TestJWTHandling()
		{
			CngKey newKey = CngKey.Create(CngAlgorithm.ECDiffieHellmanP384, null, new CngKeyCreationParameters() {ExportPolicy = CngExportPolicies.AllowPlaintextExport, KeyUsage = CngKeyUsages.AllUsages});

			byte[] t = CryptoUtils.ImportECDsaCngKeyFromCngKey(newKey.Export(CngKeyBlobFormat.EccPrivateBlob));
			CngKey tk = CngKey.Import(t, CngKeyBlobFormat.EccPrivateBlob);
			Assert.AreEqual(CngAlgorithmGroup.ECDsa, tk.AlgorithmGroup);

			ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(newKey);
			ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;

			var b64Key = Base64Url.Encode(ecKey.PublicKey.GetDerEncoded());
			string test = $@"
{{ 
	""exp"": 1464983845, 
	""extraData"": {{ 
		""displayName"": ""gurunx"",	
		""identity"": ""af6f7c5e -fcea-3e43-bf3a-e005e400e578""	
	}},	
	""identityPublicKey"": ""{b64Key}"",
	""nbf"": 1464983844
}}";
			CertificateData certificateData = new CertificateData
			{
				Exp = 1464983845,
				ExtraData = new ExtraData
				{
					DisplayName = "gurun",
					Identity = "af6f7c5e -fcea-3e43-bf3a-e005e400e578",

				},
				IdentityPublicKey = b64Key,
				Nbf = 1464983844,
			};

			JWT.JsonMapper = new NewtonsoftMapper();

			string val = JWT.Encode(certificateData, tk, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64Key}});
			Console.WriteLine(val);

			Assert.AreEqual(b64Key, JWT.Headers(val)["x5u"]);
			//Assert.AreEqual("", string.Join(";", JWT.Headers(val)));
			//Assert.AreEqual(test, JWT.Payload(val));

			Console.WriteLine(JWT.Payload(val));


			IDictionary<string, dynamic> headers = JWT.Headers(val);
			if (headers.ContainsKey("x5u"))
			{
				string certString = headers["x5u"];

				// Validate
				CngKey importKey = CryptoUtils.ImportECDsaCngKeyFromString(certString);
				CertificateData data = JWT.Decode<CertificateData>(val, importKey);
				Assert.NotNull(data);
				Assert.AreEqual(certificateData.Exp, data.Exp);
				Assert.AreEqual(certificateData.IdentityPublicKey, data.IdentityPublicKey);
				Assert.AreEqual(certificateData.Nbf, data.Nbf);
				Assert.NotNull(data.ExtraData);
				Assert.AreEqual(certificateData.ExtraData.DisplayName, data.ExtraData.DisplayName);
				Assert.AreEqual(certificateData.ExtraData.Identity, data.ExtraData.Identity);
			}

		}


		[Test]
		public void TestDecrytp()
		{
			//for (int i = 0; i < 10000; i++)
			{
				ECDiffieHellmanCng bob = new ECDiffieHellmanCng
				{
					KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
					HashAlgorithm = CngAlgorithm.Rsa
				};

				string clientPubKeyString = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs";
				var clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);

				//GetCryptoServiceProvider(clientPublicKeyBlob);


				//clientPublicKeyBlob = FixPublicKey(clientPublicKeyBlob);

				Assert.AreEqual(120, clientPublicKeyBlob.Length);

				//var clientPubKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);

				//string serverSecKeyString = "MB8CAQAwEAYHKoZIzj0CAQYFK4EEACIECDAGAgEBBAEB";
				//var serverSecKeyBlob = Base64Url.Decode(serverSecKeyString);
				//serverSecKeyBlob = FixPublicKey(serverSecKeyBlob);
				//Assert.AreEqual(40, serverSecKeyBlob.Length);
				//var serverPrivKey = ECDiffieHellmanCngPublicKey.FromByteArray(serverSecKeyBlob, CngKeyBlobFormat.Pkcs8PrivateBlob);


				//byte[] bobKey = bob.DeriveKeyMaterial(bob.PublicKey);

				using (RijndaelManaged rijAlg = new RijndaelManaged())
				{
					rijAlg.BlockSize = 128;
					rijAlg.Padding = PaddingMode.None;
					rijAlg.Mode = CipherMode.CFB;
					rijAlg.FeedbackSize = 8;

					rijAlg.Key = Base64Url.Decode("ZOBpyzki/M8UZv5tiBih048eYOBVPkQE3r5Fl0gmUP4=");
					rijAlg.IV = Base64Url.Decode("ZOBpyzki/M8UZv5tiBih0w==");

					Assert.AreEqual(32, rijAlg.Key.Length);

					Assert.AreEqual(rijAlg.Key.Take(16).ToArray(), rijAlg.IV);

					// Create a decrytor to perform the stream transform.
					ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

					// Create the streams used for decryption.
					using (MemoryStream msDecrypt = new MemoryStream())
					{
						byte[] buffer1 = SoapHexBinary.Parse("4B4FCA0C2A4114155D67F8092154AAA5EF").Value;
						byte[] buffer2 = SoapHexBinary.Parse("DF53B9764DB48252FA1AE3AEE4").Value;
						msDecrypt.Write(buffer1, 0, buffer1.Length);
						msDecrypt.Position = 0;
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							byte[] result1 = new byte[17];
							csDecrypt.Read(result1, 0, 17);

							msDecrypt.Position = 0;
							msDecrypt.SetLength(0);
							;

							msDecrypt.Write(buffer2, 0, buffer2.Length);
							msDecrypt.Position = 0;

							byte[] result2 = new byte[13];
							csDecrypt.Read(result2, 0, 13);

							Assert.AreEqual(SoapHexBinary.Parse("0400000000499602D2FC2FCB233F34D5DD").Value, result1);
							Assert.AreEqual(SoapHexBinary.Parse("3C000000085A446D11C0C7AA5A").Value, result2);


							// Hashing
							MemoryStream hashStream = new MemoryStream();
							Assert.True(BitConverter.IsLittleEndian);
							hashStream.Write(BitConverter.GetBytes(1L), 0, 8);
							byte[] text = SoapHexBinary.Parse("3C00000008").Value;
							hashStream.Write(text, 0, text.Length);
							hashStream.Write(rijAlg.Key, 0, rijAlg.Key.Length);

							SHA256Managed crypt = new SHA256Managed();
							var buffer = hashStream.ToArray();
							byte[] crypto = crypt.ComputeHash(buffer, 0, buffer.Length).Take(8).ToArray();
							Assert.AreEqual(SoapHexBinary.Parse("5A446D11C0C7AA5A").Value, crypto);
						}
					}
					using (MemoryStream msDecrypt = new MemoryStream())
					{
						byte[] buffer1 = SoapHexBinary.Parse("4B4FCA0C2A4114155D67F8092154AAA5EF").Value;
						byte[] buffer2 = SoapHexBinary.Parse("DF53B9764DB48252FA1AE3AEE4").Value;
						msDecrypt.Write(buffer1, 0, buffer1.Length);
						msDecrypt.Write(buffer2, 0, buffer2.Length);
						msDecrypt.Position = 0;
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							byte[] result1 = new byte[buffer1.Length];
							csDecrypt.Read(result1, 0, result1.Length);

							byte[] result2 = new byte[13];
							csDecrypt.Read(result2, 0, 13);

							Assert.AreEqual(SoapHexBinary.Parse("0400000000499602D2FC2FCB233F34D5DD").Value, result1);
							Assert.AreEqual(SoapHexBinary.Parse("3C000000085A446D11C0C7AA5A").Value, result2);


							// Hashing
							MemoryStream hashStream = new MemoryStream();
							Assert.True(BitConverter.IsLittleEndian);
							hashStream.Write(BitConverter.GetBytes(1L), 0, 8);
							byte[] text = SoapHexBinary.Parse("3C00000008").Value;
							hashStream.Write(text, 0, text.Length);
							hashStream.Write(rijAlg.Key, 0, rijAlg.Key.Length);

							SHA256Managed crypt = new SHA256Managed();
							var buffer = hashStream.ToArray();
							byte[] crypto = crypt.ComputeHash(buffer, 0, buffer.Length).Take(8).ToArray();
							Assert.AreEqual(SoapHexBinary.Parse("5A446D11C0C7AA5A").Value, crypto);
						}
					}
				}
			}
		}

		[Test]
		public void TestUuidConvert()
		{
			//		"identity": "af6f7c5e-fcea-3e43-bf3a-e005e400e578"

			Assert.AreEqual(new Guid("af6f7c5e-fcea-3e43-bf3a-e005e400e578").ToString(), "af6f7c5e-fcea-3e43-bf3a-e005e400e578");
		}

		[Test, Ignore("")]
		public void TestRealDecrytp()
		{
			// YFtS5MGIU/UQ2w2n3RdqMoBcHOzqEQqISOyKD+W9Prk=

			using (RijndaelManaged rijAlg = new RijndaelManaged())
			{
				rijAlg.BlockSize = 128;
				rijAlg.Padding = PaddingMode.None;
				rijAlg.Mode = CipherMode.CFB;
				rijAlg.FeedbackSize = 8;

				rijAlg.Key = Base64Url.Decode("Tv9JFj4vhftDXgcjpNWNocWZrVKaVpF+icEh51M8MvI=");
				rijAlg.IV = rijAlg.Key.Take(16).ToArray();

				Assert.AreEqual(rijAlg.Key.Take(16).ToArray(), rijAlg.IV);

				// Create a decrytor to perform the stream transform.
				ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

				// Create the streams used for decryption.
				using (MemoryStream msDecrypt = new MemoryStream())
				{
					byte[] buffer1 = SoapHexBinary.Parse("172e0592aba239d86b7ca2384cfad4e4fa5b883ff6db73931ecd").Value;
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						msDecrypt.Write(buffer1, 0, buffer1.Length);
						msDecrypt.Position = 0;
						byte[] checksum;
						byte[] clearBytes;
						using (var clearBuffer = new MemoryStream())
						{
							var buffer = new byte[1024];
							var read = csDecrypt.Read(buffer, 0, buffer.Length);
							while (read > 0)
							{
								clearBuffer.Write(buffer, 0, read);
								read = csDecrypt.Read(buffer, 0, buffer.Length);
							}
							csDecrypt.Flush();

							var fullResult = clearBuffer.ToArray();
							clearBytes = (byte[]) fullResult.Take(fullResult.Length - 8).ToArray();
							checksum = fullResult.Skip(fullResult.Length - 8).ToArray();
						}

						Assert.AreEqual(6, clearBytes[0]);

						Package message = PackageFactory.CreatePackage(clearBytes[0], clearBytes, "mcpe");
						Assert.NotNull(message);
						Assert.AreEqual(typeof (McpeWrapper), message.GetType());

						List<Package> messages = HandleBatch((McpeWrapper) message);
						McpeClientToServerHandshake magic = (McpeClientToServerHandshake) messages.FirstOrDefault();
						Assert.AreEqual(typeof (McpeClientToServerHandshake), magic?.GetType());

						//Hashing - Checksum - Validation
						MemoryStream hashStream = new MemoryStream();
						Assert.True(BitConverter.IsLittleEndian);
						hashStream.Write(BitConverter.GetBytes(0L), 0, 8);
						hashStream.Write(clearBytes, 0, clearBytes.Length);
						hashStream.Write(rijAlg.Key, 0, rijAlg.Key.Length);
						SHA256Managed crypt = new SHA256Managed();
						var hashBuffer = hashStream.ToArray();
						byte[] validationCheckSum = crypt.ComputeHash(hashBuffer, 0, hashBuffer.Length).Take(8).ToArray();
						Assert.AreEqual(checksum, validationCheckSum);
					}
				}
			}
		}

		private List<Package> HandleBatch(McpeWrapper batch)
		{
			var messages = new List<Package>();

			// Get bytes
			byte[] payload = batch.payload;
			// Decompress bytes

			Console.WriteLine("Package:\n" + Package.HexDump(payload));

			MemoryStream stream = new MemoryStream(payload);
			if (stream.ReadByte() != 0x78)
			{
				throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
			}
			stream.ReadByte();
			using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
			{
				// Get actual package out of bytes
				MemoryStream destination = new MemoryStream();
				defStream2.CopyTo(destination);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, true);
				int len = reader.ReadInt32();
				byte[] internalBuffer = reader.ReadBytes(len);

				Console.WriteLine($"Package [len={len}:\n" + Package.HexDump(internalBuffer));

				messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer, "mcpe") ?? new UnknownPackage(internalBuffer[0], internalBuffer));
				if (destination.Length > destination.Position) throw new Exception("Have more data");
			}

			return messages;
		}

		[Test]
		public void RoundTripTest()
		{
			string serverKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAElakYLA/xXdxMgBY+A6v/hOca33Lnz1Dr56XQuTOUdWN6z8mbg5DjoBL+hc3t4gG+GdIGLcBew+56UJRfm313HZIhR6zpNnhqyA9GJsbCsBTq1D3A2zp+jpUZmrzuQBR/";

			ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(serverKey);

			string b64Key = Convert.ToBase64String(publicKey.GetDerEncoded());

			Assert.AreEqual(serverKey, b64Key);

		}
	}
}