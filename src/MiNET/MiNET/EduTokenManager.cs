using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.SqlServer.Server;
using MiNET.Utils;
using Newtonsoft.Json.Linq;

namespace MiNET
{
	public class EduTokenManager
	{
		private string _username;
		private string _password;

		private Dictionary<string, string> _signedTokens = new Dictionary<string, string>();

		public EduTokenManager()
		{
			_username = Config.GetProperty("AAD.username", "");
			_password = Config.GetProperty("AAD.password", "");
		}

		public string GetSignedToken(string tenantId)
		{
			if (!_signedTokens.TryGetValue(tenantId, out string signedToken))
			{
				signedToken = FetchNewSignedToken(_username, _password);
				_signedTokens[tenantId] = signedToken;
			}

			return signedToken;
		}

		public static string FetchNewSignedToken(string username, string password)
		{
			string authorityUri = "https://login.windows.net/common/oauth2/authorize";
			string resourceUri = "https://meeservices.minecraft.net";
			string clientId = "b36b1432-1a1c-4c82-9b76-24de1cab42f2";
			string redirectUri = "https://login.live.com/oauth20_desktop.srf"; // Standard void redirect AAD stuff

			/**
			 * MUST FILL IN WITH YOUR STUFF
			 */

			AuthenticationContext authContext = new AuthenticationContext(authorityUri);
			var result = authContext.AcquireTokenAsync(resourceUri, clientId, new UserPasswordCredential(username, password)).Result;

			// Get the next part (this is calling a MCEE specific service

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
			HttpResponseMessage response = client.PostAsync("https://meeservices.azurewebsites.net/signin", content).Result;  // Blocking call!
			if (!response.IsSuccessStatusCode) throw new Exception("Failed to get signed token from Microsoft");

			var json = response.Content.ReadAsStringAsync().Result;

			dynamic payload = JObject.Parse(json);

			return payload.signedToken;
		}
	}
}