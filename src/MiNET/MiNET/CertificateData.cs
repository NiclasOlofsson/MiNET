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

using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiNET
{
	//
	// XBOX login
	//

	//{
	//	"nbf": 1465304604,
	//	"randomNonce": 2876920962471578546,
	//	"iss": "RealmsAuthorization",
	//	"exp": 1465391064,
	//	"iat": 1465304664,
	//	"certificateAuthority": true,
	//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAEr935ZYD18b9p1mgmwoMTWmBhJ/eTmqX9CmcZb1wsVZg20za1JRGro9kcHxJo5VW11HbJev3T+a0/WxpoLKxN9dwDl+USHuzlzWcMdzHdJLymiLQScJJ522DykllRM4Pe"
	//}
	//{
	//	"nbf": 1466694143,
	//	"extraData": {
	//		"identity": "6cdfec82-45b6-3322-9111-084cd74e32f0",
	//		"displayName": "gurunx",
	//		"XUID": "2535410512372218"
	//	},
	//	"randomNonce": -453593381138004104,
	//	"iss": "RealmsAuthorization",
	//	"exp": 1466780603,
	//	"iat": 1466694203,
	//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAECj+h2Z1+bnF1vnfkRJ9GFJhZrORvImXo7j4YozPjIIKuVXPlKsvAB5JXSzYpVG3gCXVprEw02a2SumqqGPTwJLce2YSVmuyQsD65jjXFIJUGlKYcb/kLRlpwO1uw5/t6"
	//}


	//
	// No XBOX login
	//

	//{
	//	"exp": 1464983845,
	//	"extraData": {
	//		"displayName": "gurunx",
	//		"identity": "af6f7c5e-fcea-3e43-bf3a-e005e400e578"
	//	},
	//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7nnZpCfxmCrSwDdBv7eBXXMtKhroxOriEr3hmMOJAuw/ZpQXj1K5GGtHS4CpFNttd1JYAKYoJxYgaykpie0EyAv3qiK6utIH2qnOAt3VNrQYXfIZJS/VRe3Il8Pgu9CB",
	//	"nbf": 1464983844
	//}

	public class HandshakeData
	{
		public string salt { get; set; }

		public string signedToken { get; set; }
	}

	public class CertificateData
	{
		public const string MojangRootKey = "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE8ELkixyLcwlZryUQcu1TvPOmI2B7vX83ndnWRUaXm74wFfa5f/lwQNTfrLVHa2PmenpGI6JhIMUJaWZrjmMj90NoKNFSNBuKdm8rYiXsfaz3K36x/1U26HpG0ZxK/V1V";

		public long Nbf { get; set; }

		public ExtraData ExtraData { get; set; }

		public long RandomNonce { get; set; }

		public string Iss { get; set; }

		public long Exp { get; set; }

		public long Iat { get; set; }

		public bool CertificateAuthority { get; set; }

		public string IdentityPublicKey { get; set; }
	}

	public class ExtraData
	{
		public string Identity { get; set; }

		public string DisplayName { get; set; }

		public string Xuid { get; set; }

		public string TitleId { get; set; }
	}

	public class NewtonsoftMapper : IJsonMapper
	{
		static NewtonsoftMapper()
		{
			JWT.DefaultSettings.JsonMapper = new NewtonsoftMapper();
		}

		public string Serialize(object obj)
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
			};

			return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
		}

		public T Parse<T>(string json)
		{
			var settings = new JsonSerializerSettings
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				NullValueHandling = NullValueHandling.Ignore,
			};

			return JsonConvert.DeserializeObject<T>(json, settings);
		}
	}
}