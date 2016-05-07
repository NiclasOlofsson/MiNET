using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class DefaultRoleStore : IRoleStore<Role>
	{
		private Dictionary<string, Role> _roles = new Dictionary<string, Role>();
		
		public Task CreateAsync(Role role)
		{
			_roles.Add(role.Id,role);
                        return Task.FromResult<Role>(null);
		}

		public Task UpdateAsync(Role role)
		{
			_roles[role.Id] = role;
                        return Task.FromResult<Role>(null);
		}

		public Task DeleteAsync(Role role)
		{
			_roles.Remove(role.Id);
                        return Task.FromResult<Role>(null);
		}

		public Task<Role> FindByIdAsync(string roleId)
		{
			Role role;
                        _roles.TryGetValue(roleId,out role);
                        return Task.FromResult(role);
		}

		public Task<Role> FindByNameAsync(string roleName)
		{
			Role role;
                        _roles.TryGetValue(roleName, out role);
                        return Task.FromResult(role);
		}

		public void Dispose()
		{
			_roles = null;
			GC.SuppressFinalize(this);
		}
	}
}
