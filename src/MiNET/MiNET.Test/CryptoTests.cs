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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Jose;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Utils.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using SicStream;

namespace MiNET.Test
{
	[TestClass]
	public class CryptoTests
	{
		[TestMethod]
		public void TestGoMinetSigning()
		{
			byte[] data = new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8};

			byte[] signature = "BZGlTUCpCGPOymtkgnq57fs0Is71CDixo8vMYvXtifJys47R1M3vcuJu7cDVo63sm3CAiCoeK5ysQYra1eAXYLQbZGrb5wbSeyp60UEQLQKkB5etWiq1Qzs4WFk4QvJX".DecodeBase64Url();

			byte[] keybytes = (byte[]) (Array) new sbyte[] {48, 118, 48, 16, 6, 7, 42, -122, 72, -50, 61, 2, 1, 6, 5, 43, -127, 4, 0, 34, 3, 98, 0, 4, 84, -22, 24, 17, 115, 88, 119, -3, 113, -103, -40, 3, -34, -59, 3, 51, 109, 19, 40, 6, -43, 41, 59, 46, 75, 65, 111, 15, 56, 112, 79, 121, 92, 38, 127, -14, 11, 114, 104, -75, 61, -123, -2, -103, 107, -75, -5, -77, 78, -127, -26, -62, -38, -24, 55, -34, 59, -4, -107, -72, 97, -47, 116, -83, -16, 21, -17, 44, -77, -85, -82, -102, 43, 92, 120, 8, -73, -10, 59, -64, 27, 28, 122, 57, 102, -87, -75, -27, 39, 56, -64, 20, 59, 16, -54, -59};

			ECPublicKeyParameters x5KeyParam = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(keybytes);
			Assert.AreEqual("Org.BouncyCastle.Math.EC.Custom.Sec.SecP384R1Curve", x5KeyParam.Parameters.Curve.GetType().FullName);
			Assert.AreEqual("EC", x5KeyParam.AlgorithmName);

			var signParam = new ECParameters
			{
				Curve = ECCurve.NamedCurves.nistP384,
				Q =
				{
					X = x5KeyParam.Q.AffineXCoord.GetEncoded(),
					Y = x5KeyParam.Q.AffineYCoord.GetEncoded()
				},
			};
			signParam.Validate();
			var key = ECDsa.Create(signParam);

			Assert.IsTrue(key.VerifyData(data, signature, HashAlgorithmName.SHA384));
		}

		[TestMethod]
		public void TestGoMinetX5u()
		{
			string rawGomint = "eyJ4NXUiOiJNSFl3RUFZSEtvWkl6ajBDQVFZRks0RUVBQ0lEWWdBRWVUMWdtalVYUTZVdHdsZUVWbVFFcEhQa1lOczhXMVBqbVBBVmFkNDJnaWdxdG1PdTZaUkU5ZVZJYnROUU10ZDc2WE1NK3ozaUZyWEpvb0dZUUVnNUhTeFpZb3p5XC9EZ1wvY3hSTXdVbWo4RU5kK2FEeW1qQVZjQ1VRajM4bGFzY1EiLCJhbGciOiJFUzM4NCJ9.eyJuYmYiOjE1MzAzODE0MTksInBheWxvYWQiOiJkeWxhbiBsb3ZlcyBwaHAuIHBzc3N0dHR0dHR0dHQiLCJpc3MiOiJzZWxmIiwiZXhwIjoxNTMwNDY3ODIwLCJpYXQiOjE1MzA0Njc4MjAsImNlcnRpZmljYXRlQXV0aG9yaXR5Ijp0cnVlLCJpZGVudGl0eVB1YmxpY0tleSI6Ik1IWXdFQVlIS29aSXpqMENBUVlGSzRFRUFDSURZZ0FFZVQxZ21qVVhRNlV0d2xlRVZtUUVwSFBrWU5zOFcxUGptUEFWYWQ0MmdpZ3F0bU91NlpSRTllVklidE5RTXRkNzZYTU0rejNpRnJYSm9vR1lRRWc1SFN4WllvenlcL0RnXC9jeFJNd1VtajhFTmQrYUR5bWpBVmNDVVFqMzhsYXNjUSJ9.F_8cdmLiQFJi4HWdJi3Ifj5VIhUH-DYti4pgpIqajUR6jYUAPnRmzL2XL3YB5TLGaPWrqwzHFMm9Pr5N6hQgZhMs-7knMAkeNPBK7v_XP0VpvMlZqMLoldHZofHj5ONW";
			string x5uGomint = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEeT1gmjUXQ6UtwleEVmQEpHPkYNs8W1PjmPAVad42gigqtmOu6ZRE9eVIbtNQMtd76XMM+z3iFrXJooGYQEg5HSxZYozy/Dg/cxRMwUmj8ENd+aDymjAVcCUQj38lascQ";

			string rawMinet = "ew0KICAiYWxnIjogIkVTMzg0IiwNCiAgIng1dSI6ICJNSFl3RUFZSEtvWkl6ajBDQVFZRks0RUVBQ0lEWWdBRXpUT2d6MHBad2JIK1VFdDRxcTY2ZVQydlhqNEtxb0lrQUlIdm5paEZwaG1yeUFtczNTYkdmQ3MzcWJHNjlvVm9TVWhwT01MK3VvY3d3N1VieWE4M2R6Tkp2QkpvMXNVbGJmbHdlOTRnN3hIclAwT1huU1pRSCtwL3ZJSnFYMWVHIg0KfQ.ew0KICAibmJmIjogMTUzMDM3Mzg1OCwNCiAgImV4dHJhRGF0YSI6IHsNCiAgICAiaWRlbnRpdHkiOiAiOGQxMzZmZDItZjViNy00Yzc2LThlOTItN2M3YWQwNjhkM2M1IiwNCiAgICAiZGlzcGxheU5hbWUiOiAiVGhlR3JleTAwMSINCiAgfSwNCiAgInJhbmRvbU5vbmNlIjogMjAwNDg3NDg5NywNCiAgImlzcyI6ICJzZWxmIiwNCiAgImV4cCI6IDE1MzA0NjAyNTgsDQogICJpYXQiOiAxNTMwMzczODU4LA0KICAiY2VydGlmaWNhdGVBdXRob3JpdHkiOiB0cnVlLA0KICAiaWRlbnRpdHlQdWJsaWNLZXkiOiAiTUhZd0VBWUhLb1pJemowQ0FRWUZLNEVFQUNJRFlnQUV6VE9nejBwWndiSCtVRXQ0cXE2NmVUMnZYajRLcW9Ja0FJSHZuaWhGcGhtcnlBbXMzU2JHZkNzM3FiRzY5b1ZvU1VocE9NTCt1b2N3dzdVYnlhODNkek5KdkJKbzFzVWxiZmx3ZTk0Zzd4SHJQME9YblNaUUgrcC92SUpxWDFlRyINCn0.eXGlCHYQ6-cXhcoo-raaKl-kR8xLz4F5xAgr1hQs6JCEVVlTRRPmFtS7GbRL_r1XbbyTbSvYaIVDGggVMqqN5ayalRfbluVB1zVVxbPclXXb6rZVuLgKMevNYJYGqKEx";
			string x5uMinet = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEzTOgz0pZwbH+UEt4qq66eT2vXj4KqoIkAIHvnihFphmryAms3SbGfCs3qbG69oVoSUhpOML+uocww7Ubya83dzNJvBJo1sUlbflwe94g7xHrP0OXnSZQH+p/vIJqX1eG";

			string rawVanilla = "eyJ4NXUiOiJNSFl3RUFZSEtvWkl6ajBDQVFZRks0RUVBQ0lEWWdBRUpqV0JCR2VqWDY0STc0UDN6c0M4SElIUFRsek1mczYwdGcrbFV5ZjZsSEpuc2FGMnJUSVRkNGhSb1NMSlROQTMxT1FVSlFRUnFPZWNJSVBiQ0NRTzhvb0FyMkpXZ21zazNtU2FSdzlLQ1wvXC9EUXFuV3JZU21TYjlVUWRKT2FlemMiLCJhbGciOiJFUzM4NCJ9.eyJuYmYiOjE1MzAzNzY4NTEsImV4dHJhRGF0YSI6eyJYVUlEIjoiMjUzNTQxMDUxMjM3MjIxOCIsImlkZW50aXR5IjoiNmNkZmVjODItNDViNi0zMzIyLTkxMTEtMDg0Y2Q3NGUzMmYwIiwiZGlzcGxheU5hbWUiOiJndXJ1bngifSwicmFuZG9tTm9uY2UiOi03MzE5MDQzODAxMjcwMTE2MDM3LCJpc3MiOiJNb2phbmciLCJleHAiOjE1MzA0NjMzMTEsImlhdCI6MTUzMDM3NjkxMSwiaWRlbnRpdHlQdWJsaWNLZXkiOiJNSFl3RUFZSEtvWkl6ajBDQVFZRks0RUVBQ0lEWWdBRTZUdCtjQlJpbE8zbERTaG54anhVdDBObkNuZjZBblNPK3NBN25Lc0pUSGltZVp6MU12SEJrYVM0M0N1SVRRdEordlNUNVJcL2NtS1Fad0tNVmF5SnJvcFNYaXFrWlRsZjhXMUpWOWlRQ09BbFREcUJqXC94b1k3XC9iRzlxRndzSWJwIn0.m8qIpAT2DgjJxbOulK1Vu-A4jyF9ksD9MWxoNJOLRlyVsWlp8-xb8McmEdhSNqPTTDJjlS7Nqfy-rWLngwu-t0fGM5ISO5YIliIbB_uGqjV3jnX4HSt1OLVIl41_SHcp";
			string x5uVanilla = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEJjWBBGejX64I74P3zsC8HIHPTlzMfs60tg+lUyf6lHJnsaF2rTITd4hRoSLJTNA31OQUJQQRqOecIIPbCCQO8ooAr2JWgmsk3mSaRw9KC//DQqnWrYSmSb9UQdJOaezc";


			//string raw = rawMinet;
			//string x5u = x5uMinet;

			string raw = rawGomint;
			string x5u = x5uGomint;

			//string raw = rawVanilla;
			//string x5u = x5uVanilla;


			ECPublicKeyParameters x5KeyParam = (ECPublicKeyParameters) PublicKeyFactory.CreateKey(x5u.DecodeBase64());
			Assert.AreEqual("Org.BouncyCastle.Math.EC.Custom.Sec.SecP384R1Curve", x5KeyParam.Parameters.Curve.GetType().FullName);
			Assert.AreEqual("EC", x5KeyParam.AlgorithmName);

			var signParam = new ECParameters
			{
				Curve = ECCurve.NamedCurves.nistP384,
				Q =
				{
					X = x5KeyParam.Q.AffineXCoord.GetEncoded(),
					Y = x5KeyParam.Q.AffineYCoord.GetEncoded()
				},
			};
			signParam.Validate();
			ECDsa key = ECDsa.Create(signParam);

			Assert.AreEqual(384, key.KeySize);
			Assert.AreEqual(null, key.KeyExchangeAlgorithm);
			Assert.AreEqual("ECDsa", key.SignatureAlgorithm);

			//CertificateData data = JWT.Decode<CertificateData>(raw, ECDsa.Create(signParam));

			Assert.AreEqual(raw, Serialize(Parse(raw)));

			DecodeBytes(raw, key);
		}

		public static byte[][] Parse(string token)
		{
			string[] strArray = token.Split('.');
			byte[][] numArray = new byte[strArray.Length][];
			for (int index = 0; index < strArray.Length; ++index)
				numArray[index] = Base64Url.Decode(strArray[index]);
			return numArray;
		}


		public static string Serialize(params byte[][] parts)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (byte[] part in parts)
				stringBuilder.Append(Base64Url.Encode(part)).Append(".");
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}

		private static byte[] DecodeBytes(string token, object key = null)
		{
			Ensure.IsNotEmpty(token, "Incoming token expected to be in compact serialization form, not empty, whitespace or null.", Array.Empty<object>());
			byte[][] parts = Compact.Parse(token);
			byte[] bytes1 = parts[0];
			byte[] numArray = parts[1];
			byte[] signature = parts[2];
			byte[] bytes2 = Encoding.UTF8.GetBytes(Compact.Serialize(bytes1, numArray));
			//JwtSettings settings1 = new JwtSettings();
			//string headerValue = (string)settings1.JsonMapper.Parse<Dictionary<string, object>>(Encoding.UTF8.GetString(bytes1))["alg"];
			//JwsAlgorithm alg = settings1.JwsAlgorithmFromHeader(headerValue);
			//if (expectedJwsAlg.HasValue)
			//{
			//	JwsAlgorithm? nullable = expectedJwsAlg;
			//	JwsAlgorithm jwsAlgorithm = alg;
			//	if ((nullable.GetValueOrDefault() == jwsAlgorithm ? (!nullable.HasValue ? 1 : 0) : 1) != 0)
			//		throw new InvalidAlgorithmException("The algorithm type passed to the Decode method did not match the algorithm type in the header.");
			//}
			//IJwsAlgorithm jwsAlgorithm1 = settings1.Jws(alg);
			//if (jwsAlgorithm1 == null)
			//	throw new JoseException(string.Format("Unsupported JWS algorithm requested: {0}", (object)headerValue));
			//Assert.AreEqual("Jose.netstandard1_4.EcdsaUsingSha", jwsAlgorithm1.GetType().FullName);

			if (!((ECDsa) key).VerifyData(bytes2, signature, HashAlgorithmName.SHA384))
				throw new IntegrityException("Invalid signature.");

			//if (!jwsAlgorithm1.Verify(signature, bytes2, key))
			//throw new IntegrityException("Invalid signature.");
			return numArray;
		}


		[TestMethod]
		public void TestMethod0()
		{
			int metadataMax = 5;
			for (int i = metadataMax; i != 0; i = i >> 1)
			{
				metadataMax |= i;
			}

			Assert.AreEqual(7, metadataMax);
		}

		[TestMethod]
		public void TestMethod1()
		{
			string b64InputKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEVQxN/wFsMiYihwv1psUgKRIhgX02OPBQl0aKYNtKXoCk67hE/lsR8UC77Fqm1HPuMALWG8RcihSHoZwx2HfOz11QkwvlKEf8UuMrbp0yt/mQNJx6QQm6CiZ7e63sYqdV";

			ECPublicKeyParameters asyKey = (ECPublicKeyParameters)
				PublicKeyFactory.CreateKey(b64InputKey.DecodeBase64Url());

			ECParameters param = new ECParameters();
			param.Curve = ECCurve.NamedCurves.nistP384;
			param.Q.X = asyKey.Q.AffineXCoord.GetEncoded();
			param.Q.Y = asyKey.Q.AffineYCoord.GetEncoded();

			var ecPublicKey = ECDiffieHellman.Create(param).PublicKey;

			var effi = ECDiffieHellman.Create(ECCurve.NamedCurves.nistP384);
			ECParameters privateKey = effi.ExportParameters(true);
			var secretPrepend = Encoding.UTF8.GetBytes("RANDOM SECRET");
			byte[] secret = effi.DeriveKeyFromHash(ecPublicKey, HashAlgorithmName.SHA256, secretPrepend, new byte[0]);

			IBufferedCipher decryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
			IBufferedCipher encryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");

			decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

			encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

			string b64PublicKey = effi.PublicKey.ToDerEncoded().EncodeBase64();
			var handshakeJson = new HandshakeData() {salt = secretPrepend.EncodeBase64()};

			var signKey = ECDsa.Create(privateKey);

			string val = JWT.Encode(handshakeJson, signKey, JwsAlgorithm.ES384, new Dictionary<string, object> {{"x5u", b64PublicKey}});
			//Log.Warn($"Headers: {string.Join(";", JWT.Headers(val))}");
			//Log.Warn($"Return salt:\n{JWT.Payload(val)}");
		}

		[TestMethod]
		public void TestAES_GCM()
		{
			byte[] secret = "YYpS7Z92bpTazDa9exkclGF1RZoObERCfk6T123PELA=".DecodeBase64();
			Assert.AreEqual(32, secret.Length);

			//Assert.AreEqual("", decryptor.GetType().Name);
			//var encryptor = (SicBlockCipher) CipherUtilities.GetCipher("AES/CTR/NoPadding");
			//Assert.AreEqual("BufferedBlockCipher", encryptor.GetType().Name);
			//Assert.AreEqual("AES/SIC", encryptor.AlgorithmName);
			//var decryptor = CipherUtilities.GetCipher("AES/CTR/NoPadding") as BufferedBlockCipher;
			//decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));
			//encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));


			var encryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
			var decryptor = new StreamingSicBlockCipher(new SicBlockCipher(new AesEngine()));
			decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));
			encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(12).Concat(new byte[] {0, 0, 0, 2}).ToArray()));

			//Log.Warn($"Headers: {string.Join(";", JWT.Headers(val))}");
			//Log.Warn($"Return salt:\n{JWT.Payload(val)}");
			byte[] payload = Encoding.UTF8.GetBytes("gurun");
			Assert.AreEqual(5, payload.Length);


			//encrypted = encryptor.ProcessBytes(payload, 0, payload.Length);
			int originalLen = payload.Length;
			//payload = PaddTo16(payload);
			//Assert.AreEqual(16, payload.Length);
			var encrypted = new byte[payload.Length];
			var encrypted2 = new byte[payload.Length];
			var encrypted3 = new byte[payload.Length];
			encryptor.ProcessBytes(payload, 0, payload.Length, encrypted, 0);
			encryptor.ProcessBytes(payload, 0, payload.Length, encrypted2, 0);
			encryptor.ProcessBytes(payload, 0, payload.Length, encrypted3, 0);

			Assert.AreEqual("41EgP6s=", encrypted.Take(originalLen).ToArray().EncodeBase64());
			Assert.AreEqual("IDeJqk8=", encrypted2.Take(originalLen).ToArray().EncodeBase64());
			Assert.AreEqual("6SAjD+8=", encrypted3.Take(originalLen).ToArray().EncodeBase64());

			Assert.AreEqual(5, encrypted.Length);
			Assert.AreEqual("gurun", Encoding.UTF8.GetString(decryptor.ProcessBytes(encrypted)));
			Assert.AreEqual("gurun", Encoding.UTF8.GetString(decryptor.ProcessBytes(encrypted2)));
			Assert.AreEqual("gurun", Encoding.UTF8.GetString(decryptor.ProcessBytes(encrypted3)));
		}

		private static byte[] PaddTo16(byte[] payload)
		{
			int len = payload.Length;
			int padding = (16 - len) % 16;
			return (byte[]) payload.Concat(new byte[padding]).ToArray();
		}
	}
}