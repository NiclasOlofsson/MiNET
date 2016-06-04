using System;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Jose;
using MiNET.Net;
using MiNET.Utils;
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
			// Public key
			ECDiffieHellmanPublicKey clientKey = CryptoUtils.CreateEcDiffieHellmanPublicKey("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs");

			// Private key
			AsymmetricKeyParameter privKey = PrivateKeyFactory.CreateKey(Base64Url.Decode("MB8CAQAwEAYHKoZIzj0CAQYFK4EEACIECDAGAgEBBAEB"));
			PrivateKeyInfo privKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privKey);
			byte[] derKey = privKeyInfo.GetDerEncoded();
			CngKey privCngKey = CngKey.Import(derKey, CngKeyBlobFormat.Pkcs8PrivateBlob);


			Console.WriteLine(privKeyInfo.PrivateKeyAlgorithm.Algorithm);
			Console.WriteLine(privCngKey.Algorithm.Algorithm);

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


		//		[Test]
		//		public void TestKeyMangling()
		//		{
		//			string certString = @"MHQCAQEEIKG5lvBghT6E/yPuAP5e8HFIev0WrvrUWk2AQHK9JGvXoAcGBSuBBAAK
		//oUQDQgAEkAJfIFbTAEq0+7tPRmEoQhxzYzB3rd/gsORnd7ILCEqCx3dxPUJMvNqp
		//9LowF6yjrE921Zxlxfrp99pYaSWfrg==";

		//			//CngKey.Import(Convert.FromBase64String(certString), CngKeyBlobFormat.Pkcs8PrivateBlob);
		//		}

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

					Assert.AreEqual(rijAlg.Key.Take(16).ToArray(), rijAlg.IV);

					// Create a decrytor to perform the stream transform.
					ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

					// Create the streams used for decryption.
					using (MemoryStream msDecrypt = new MemoryStream())
					{
						byte[] buffer1 = SoapHexBinary.Parse("4B4FCA0C2A4114155D67F8092154AAA5EF").Value;
						byte[] buffer2 = SoapHexBinary.Parse("DF53B9764DB48252FA1AE3AEE4").Value;
						msDecrypt.Write(buffer1, 0, buffer1.Length);
						msDecrypt.Write(buffer2, 0, buffer2.Length);
						msDecrypt.Position = 0;
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
						{
							byte[] result1 = new byte[17];
							csDecrypt.Read(result1, 0, 17);

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

		private static readonly byte[] _nullAsnBytes = new byte[] {0, 5};

		public RSACryptoServiceProvider GetCryptoServiceProvider(byte[] asnDerPublicKey)
		{
			var nullAsnValue = new AsnEncodedData(_nullAsnBytes);

			//X509Certificate2 t;
			//t.PublicKey

			//string friendlyName = this.GetKeyAlgorithm();
			//byte[] parameters = this.GetKeyAlgorithmParameters();
			//byte[] keyValue = this.GetPublicKey();
			//Oid oid = new Oid(friendlyName, OidGroup.PublicKeyAlgorithm, true);
			//m_publicKey = new PublicKey(oid, new AsnEncodedData(oid, parameters), new AsnEncodedData(oid, keyValue));

			ECDiffieHellmanCngPublicKey.FromByteArray(asnDerPublicKey.Skip(17).Take(104).ToArray(), CngKeyBlobFormat.EccPublicBlob);

			//var publi = new PublicKey(new Oid("1.2.840.10045.2.1"), new AsnEncodedData(new Oid("1.3.132.0.34"), _nullAsnBytes), new AsnEncodedData(new Oid("1.2.840.10045.2.1"), asnDerPublicKey));
			//return (RSACryptoServiceProvider) publi.Key;

			//const string RSA_OID = "1.2.840.113549.1.1.1";
			//var oid = new Oid(RSA_OID);
			//var asnPublicKey = new AsnEncodedData(oid, asnDerPublicKey);
			//var publicKey = new PublicKey(oid, nullAsnValue, asnPublicKey);
			//Assert.IsTrue(publicKey.Key is RSACryptoServiceProvider);
			//return publicKey.Key as RSACryptoServiceProvider;

			return null;
		}
	}
}