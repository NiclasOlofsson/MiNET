using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class DefaultUserStore : IUserStore<User>, IUserPasswordStore<User>
	{
		private Dictionary<string, User> _users = new Dictionary<string, User>();

		public Task CreateAsync(User user)
		{
			_users.Add(user.UserName, user);

			return Task.FromResult<User>(null);
		}

		public Task UpdateAsync(User user)
		{
			_users[user.UserName] = user;
			return Task.FromResult<User>(null);
		}

		public Task DeleteAsync(User user)
		{
			_users.Remove(user.UserName);
			return Task.FromResult<User>(null);
		}

		public Task<User> FindByIdAsync(string userId)
		{
			WebClient client = new WebClient();
			string key;
			{
				key = client.DownloadString("https://api.inpvp.net/auth/key");
			}

			User user = null;

			client.Headers[HttpRequestHeader.ContentType] = "application/json";
			client.Headers["X-Auth-Key"] = key;
			{
				try
				{
					//TODO: Fix that it first checks if exist, and then later check password.
					client.UploadString("https://api.inpvp.net/auth/validate", 
						"{\"username\":\"" + userId + "\",\"password\":\"" + Base64Encode(userId) + "\",\"ip\":\"\",\"server\":\"0\"}");
					user = new User(userId);
				}
				catch (WebException e)
				{
				}
			}

			return Task.FromResult(user);
		}

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public Task<User> FindByNameAsync(string userName)
		{
			User user;
			_users.TryGetValue(userName, out user);

			return Task.FromResult(user);
		}

		public Task SetPasswordHashAsync(User user, string passwordHash)
		{
			user.PasswordHash = passwordHash;
			return Task.FromResult<User>(null);
		}

		public Task<string> GetPasswordHashAsync(User user)
		{
			return Task.FromResult(user.PasswordHash);
		}

		public Task<bool> HasPasswordAsync(User user)
		{
			return Task.FromResult(user.PasswordHash != null);
		}

		public void Dispose()
		{
		}
	}
}