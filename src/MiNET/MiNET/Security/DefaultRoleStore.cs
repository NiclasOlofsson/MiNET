using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class DefaultRoleStore : IRoleStore<Role>
	{
		public Task CreateAsync(Role role)
		{
			return Task.FromResult<object>(null);
		}

		public Task UpdateAsync(Role role)
		{
			return Task.FromResult<object>(null);
		}

		public Task DeleteAsync(Role role)
		{
			return Task.FromResult<object>(null);
		}

		public Task<Role> FindByIdAsync(string roleId)
		{
			return Task.FromResult(new Role(roleId));
		}

		public Task<Role> FindByNameAsync(string roleName)
		{
			return Task.FromResult(new Role(roleName));
		}

		public void Dispose()
		{
		}
	}
}