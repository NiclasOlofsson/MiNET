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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Jose;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace MiNET.Test
{
	[TestClass]
	public class CryptoTests
	{
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
	}
}