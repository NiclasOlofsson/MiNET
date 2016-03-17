using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class DefaultUserStore : IUserStore<User>, IUserRoleStore<User>, IUserPasswordStore<User>
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
			User user;
			_users.TryGetValue(userId, out user);

			return Task.FromResult(user);
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

		// User-Role store implementation

		public Task AddToRoleAsync(User user, string roleName)
		{
			return Task.FromResult<object>(null);
		}

		public Task RemoveFromRoleAsync(User user, string roleName)
		{
			return Task.FromResult<object>(null);
		}

		public Task<IList<string>> GetRolesAsync(User user)
		{
			return Task.FromResult((IList<string>) new List<string>());
		}

		public Task<bool> IsInRoleAsync(User user, string roleName)
		{
			return Task.FromResult<bool>(true);
		}
	}
}