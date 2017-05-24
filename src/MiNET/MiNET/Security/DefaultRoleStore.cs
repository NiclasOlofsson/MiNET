using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
    public class DefaultRoleStore : DefaultRoleStore<string,Role,UserRole>
    {
        public DefaultRoleStore() : base(new List<Role>())
        {
        }
        public DefaultRoleStore(List<Role> roles) : base(roles)
        {

        }
    }

    public class DefaultRoleStore<TKey,TRole,TUserRole> : IRoleStore<TRole,TKey>
        where TRole : Role<TKey, TUserRole>
        where TUserRole : UserRole<TKey>,new()
    {
        private ICollection<TRole> _roles;
        private bool _disposed;

        public DefaultRoleStore(List<TRole> roles)
        {
            _roles = roles;
            _disposed = false;
        }

        public Task CreateAsync(TRole role)
        {
            _roles.Add(role);
            return Task.FromResult<TRole>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            _roles.Remove(role);
            return Task.FromResult<TRole>(null);
        }

        public Task<TRole> FindByIdAsync(TKey roleId)
        {
            TRole role = _roles.SingleOrDefault(t => t.Id.Equals(roleId));
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.FromResult(role);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole role = _roles.SingleOrDefault(t => t.Name == roleName);
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            return Task.FromResult(role);
        }

        public Task UpdateAsync(TRole role)
        {
            var temp = _roles.SingleOrDefault(t => t.Id.Equals(role.Id));
            if (temp == null)
            {
                throw new InvalidOperationException($"Unable to find a Role called {role.Name}");
            }
            _roles.Remove(temp);
            _roles.Add(role);
            return Task.FromResult<TRole>(null);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _roles = null;
            }
            _disposed = true;
        }
    }
}
