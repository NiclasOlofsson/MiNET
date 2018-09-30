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

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using log4net;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;
using Newtonsoft.Json.Linq;

namespace MiNET
{
	public class EduTokenManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(EduTokenManager));
		private static readonly ISecurityConfiguration SecurityConfig = ConfigurationProvider.MiNetConfiguration.Security;

		private string _username;
		private string _password;

		private Dictionary<string, string> _signedTokens = new Dictionary<string, string>();

		public EduTokenManager()
		{
			_username = SecurityConfig.EduUsername;
			_password = SecurityConfig.EduPassword;

			if (_username.StartsWith("secure:", StringComparison.InvariantCultureIgnoreCase) && _password.StartsWith("secure:", StringComparison.InvariantCultureIgnoreCase))
			{
				_username = Encoding.UTF8.GetString(ProtectedData.Unprotect(_username.Substring(7).DecodeBase64(), null, DataProtectionScope.LocalMachine));
				_password = Encoding.UTF8.GetString(ProtectedData.Unprotect(_password.Substring(7).DecodeBase64(), null, DataProtectionScope.LocalMachine));
			}
			else
			{
				var protUsername = ProtectedData.Protect(Encoding.UTF8.GetBytes(_username), null, DataProtectionScope.LocalMachine).EncodeBase64();
				var protPassword = ProtectedData.Protect(Encoding.UTF8.GetBytes(_password), null, DataProtectionScope.LocalMachine).EncodeBase64();
				Log.Fatal($"Plaintext credentials in MiNET configuration files are not allowed. Please use the following encrypted configuration for safe usage of Microsoft Account credentials\n" +
						$"AAD.username=secure:{protUsername}\n" +
						$"AAD.password=secure:{protPassword}\n");
				Environment.Exit(-1);
			}
		}

		public string GetSignedToken(string tenantId)
		{
			if (!_signedTokens.TryGetValue(tenantId, out string signedToken))
			{
				signedToken = FetchNewSignedToken(tenantId, _username, _password);
				_signedTokens[tenantId] = signedToken;
			}

			return signedToken;
		}

		public string FetchNewSignedToken(string tenantId, string username, string password)
		{
			/**
			 * MUST FILL IN WITH YOUR STUFF
			 */

			(string IdToken, string AccessToken) result = GetTokens(tenantId, username, password);


			string data = $@"
{{
		    ""accessToken"": ""{result.AccessToken}"",
			""build"": 10028000,
			""clientVersion"": 150,
			""displayVersion"": ""1.0.28"",
			""identityToken"": ""{result.IdToken}"", 
			""osVersion"": ""Win 10.0.16299.15"",
			""platform"": ""Windows 10 UWP Build(x64)"",
			""refreshToken"": ""_""
}}
";

			HttpClient client = new HttpClient();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
			HttpResponseMessage response = client.PostAsync("https://meeservices.azurewebsites.net/signin", content).Result; // Blocking call!
			if (!response.IsSuccessStatusCode) throw new Exception("Failed to get signed token from Microsoft");

			var json = response.Content.ReadAsStringAsync().Result;

			dynamic payload = JObject.Parse(json);

			return payload.signedToken;
		}

		string resourceUri = "https://meeservices.minecraft.net";
		string clientId = "b36b1432-1a1c-4c82-9b76-24de1cab42f2";
		string redirectUri = "https://login.live.com/oauth20_desktop.srf"; // Standard void redirect AAD stuff

		public (string IdToken, string AccessToken) GetTokens(string tenantId, string username, string password)
		{
			(string AccessToken, string RefreshToken) tokens = GetRefreshToken(tenantId, username, password);
			string idToken = GetIdTokenToken(tenantId, tokens.RefreshToken);

			return (idToken, tokens.AccessToken);
		}

		public (string, string) GetRefreshToken(string tenantId, string username, string password)
		{
			string authorityUri = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
			HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, authorityUri);

			string content = $@"
			grant_type = password
			&resource ={resourceUri}
			&username ={username}
			&password ={password}
			&client_id ={clientId}";

			message.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

			HttpClient client = new HttpClient();
			var result = client.SendAsync(message).Result;
			var json = result.Content.ReadAsStringAsync().Result;

			dynamic payload = JObject.Parse(json);

			return (payload.access_token.ToString(), payload.refresh_token.ToString());
		}

		public string GetIdTokenToken(string tenantId, string refreshToken)
		{
			string authorityUri = $"https://login.microsoftonline.com/{tenantId}/oauth2/token";
			HttpClient client = new HttpClient();
			HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, authorityUri);

			string content = $@"
			grant_type = refresh_token
			&refresh_token ={refreshToken}
			&scope = openid";

			message.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

			var result = client.SendAsync(message).Result;
			var json = result.Content.ReadAsStringAsync().Result;

			dynamic payload = JObject.Parse(json);

			return payload.id_token.ToString();
		}
	}
}