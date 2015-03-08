using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class DefaultUserStore : IUserStore<User>
	{
		public Task CreateAsync(User user)
		{
			return Task.FromResult<User>(null);
		}

		public Task UpdateAsync(User user)
		{
			return Task.FromResult<User>(null);
		}

		public Task DeleteAsync(User user)
		{
			return Task.FromResult<User>(null);
		}

		public Task<User> FindByIdAsync(string userId)
		{
			return Task.FromResult(new User(userId));
		}

		public Task<User> FindByNameAsync(string userName)
		{
			//if (userName.Equals("gurun", StringComparison.InvariantCultureIgnoreCase)) return Task.FromResult<User>(null);

			return Task.FromResult(new User(userName));
		}

		public void Dispose()
		{
		}
	}
}