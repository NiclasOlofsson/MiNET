using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Jose;
using NUnit.Framework;

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
			ECDiffieHellmanPublicKey certKey = ImportEccPublicKeyFromCertificate(c);
			Console.WriteLine(certKey.ToXmlString());

			ECDiffieHellmanPublicKey clientKey = CreateEcDiffieHellmanPublicKey("MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs");

			//ECDiffieHellmanCng bob = new ECDiffieHellmanCng(clientKey)
			//{
			//	KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
			//	HashAlgorithm = CngAlgorithm.Rsa
			//};
			//byte[] bobKey = bob.DeriveKeyMaterial(bob.PublicKey);

			Console.WriteLine(clientKey.ToXmlString());
		}

		private ECDiffieHellmanPublicKey CreateEcDiffieHellmanPublicKey(string clientPubKeyString)
		{
			byte[] clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);
			clientPublicKeyBlob = FixPublicKey(clientPublicKeyBlob.Skip(23).ToArray());

			ECDiffieHellmanPublicKey clientKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);
			return clientKey;
		}

		private byte[] FixPublicKey(byte[] publicKeyBlob)
		{
			var keyType = new byte[] { 0x45, 0x43, 0x4b, 0x33 };
			var keyLength = new byte[] { 0x30, 0x00, 0x00, 0x00 };

			return keyType.Concat(keyLength).Concat(publicKeyBlob.Skip(1)).ToArray();
		}


		private static ECDiffieHellmanPublicKey ImportEccPublicKeyFromCertificate(X509Certificate2 cert)
		{
			var keyType = new byte[] { 0x45, 0x43, 0x4b, 0x33 };
			var keyLength = new byte[] { 0x30, 0x00, 0x00, 0x00 };
			var key = cert.PublicKey.EncodedKeyValue.RawData.Skip(1);
			var keyImport = keyType.Concat(keyLength).Concat(key).ToArray();

			//Assert.AreEqual(privateKey, keyImport);

			return ECDiffieHellmanCngPublicKey.FromByteArray(keyImport, CngKeyBlobFormat.EccPublicBlob);
		}

		//		[Test]
		//		public void TestKeyMangling()
		//		{
		//			string certString = @"MHQCAQEEIKG5lvBghT6E/yPuAP5e8HFIev0WrvrUWk2AQHK9JGvXoAcGBSuBBAAK
		//oUQDQgAEkAJfIFbTAEq0+7tPRmEoQhxzYzB3rd/gsORnd7ILCEqCx3dxPUJMvNqp
		//9LowF6yjrE921Zxlxfrp99pYaSWfrg==";

		//			//CngKey.Import(Convert.FromBase64String(certString), CngKeyBlobFormat.Pkcs8PrivateBlob);
		//		}

		//[Test]
		//public void TestDecrytp()
		//{
		//	for (int i = 0; i < 10000; i++)
		//	{

		//		ECDiffieHellmanCng bob = new ECDiffieHellmanCng
		//		{
		//			KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash,
		//			HashAlgorithm = CngAlgorithm.Rsa
		//		};

		//		string clientPubKeyString = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEDEKneqEvcqUqqFMM1HM1A4zWjJC+I8Y+aKzG5dl+6wNOHHQ4NmG2PEXRJYhujyodFH+wO0dEr4GM1WoaWog8xsYQ6mQJAC0eVpBM96spUB1eMN56+BwlJ4H3Qx4TAvAs";
		//		var clientPublicKeyBlob = Base64Url.Decode(clientPubKeyString);

		//		GetCryptoServiceProvider(clientPublicKeyBlob);


		//		//clientPublicKeyBlob = FixPublicKey(clientPublicKeyBlob);

		//		Assert.AreEqual(120, clientPublicKeyBlob.Length);

		//		//var clientPubKey = ECDiffieHellmanCngPublicKey.FromByteArray(clientPublicKeyBlob, CngKeyBlobFormat.EccPublicBlob);

		//		//string serverSecKeyString = "MB8CAQAwEAYHKoZIzj0CAQYFK4EEACIECDAGAgEBBAEB";
		//		//var serverSecKeyBlob = Base64Url.Decode(serverSecKeyString);
		//		//serverSecKeyBlob = FixPublicKey(serverSecKeyBlob);
		//		//Assert.AreEqual(40, serverSecKeyBlob.Length);
		//		//var serverPrivKey = ECDiffieHellmanCngPublicKey.FromByteArray(serverSecKeyBlob, CngKeyBlobFormat.Pkcs8PrivateBlob);


		//		byte[] bobKey = bob.DeriveKeyMaterial(bob.PublicKey);

		//		using (RijndaelManaged rijAlg = new RijndaelManaged())
		//		{
		//			//rijAlg.GenerateKey();
		//			//rijAlg.GenerateIV();

		//			rijAlg.Padding = PaddingMode.None;
		//			rijAlg.Mode = CipherMode.CFB;
		//			rijAlg.FeedbackSize = 8;

		//			rijAlg.Key = Base64Url.Decode("ZOBpyzki/M8UZv5tiBih048eYOBVPkQE3r5Fl0gmUP4=");
		//			rijAlg.IV = Base64Url.Decode("ZOBpyzki/M8UZv5tiBih0w==");

		//			// Create a decrytor to perform the stream transform.
		//			ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

		//			SoapHexBinary shb = SoapHexBinary.Parse("4B4FCA0C2A4114155D67F8092154AAA5EF");

		//			// Create the streams used for decryption.
		//			using (MemoryStream msDecrypt = new MemoryStream(shb.Value))
		//			{
		//				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
		//				{
		//					using (StreamReader srDecrypt = new StreamReader(csDecrypt))
		//					{
		//						// Read the decrypted bytes from the decrypting stream
		//						// and place them in a string.
		//						string plaintext = srDecrypt.ReadToEnd();
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

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