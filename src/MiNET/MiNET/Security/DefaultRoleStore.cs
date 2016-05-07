using System;
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
        private bool _disposed = false;

        public DefaultRoleStore(List<TRole> roles)
        {
            _roles = roles;
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
            throw new NotImplementedException();
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TRole role)
        {
            throw new NotImplementedException();
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
