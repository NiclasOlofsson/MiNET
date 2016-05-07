using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
    public class DefaultUserStore : DefaultUserStore<string,User,Role,UserRole>
    {

        public DefaultUserStore() : base(new List<Role>(), new List<User>(),new List<UserRole>())
        {

        }
        public DefaultUserStore(List<Role> roles, List<User> users,List<UserRole> userRole) : base(roles,users,userRole)
        {

        }
    }
    public class DefaultUserStore<TKey,TUser,TRole,TUserRole> : IUserStore<TUser,TKey>, IUserRoleStore<TUser, TKey>, IUserPasswordStore<TUser, TKey>
        where TKey : IEquatable<TKey>
        where TUser : User<TKey,TUserRole>
        where TRole : Role<TKey,TUserRole>
        where TUserRole : UserRole<TKey>, new()
    {
        private ICollection<TUser> _users;
        private ICollection<TUserRole> _userRoles;
        private ICollection<TRole> _roles;
        private bool _disposed = false;

        public DefaultUserStore(List<TRole> roles,List<TUser> users, List<TUserRole> userRole)
        {
            _roles = roles;
            _userRoles = userRole;
            _users = users;
        }

        public Task CreateAsync(TUser user)
        {
            _users.Add(user);
            return Task.FromResult<TUser>(null);
        }

        public Task UpdateAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var temp = _users.SingleOrDefault(t => t.Id.Equals(user.Id));
            if (temp == null)
            {
                throw new InvalidOperationException($"Unable to find a User called {user.UserName}");
            }
            _users.Remove(temp);
            _users.Add(user);
            return Task.FromResult<TUser>(null);
        }

        public Task DeleteAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            _users.Remove(user);
            return Task.FromResult<TUser>(null);
        }

        public Task<TUser> FindByIdAsync(TKey userId)
        {
            var user = _users.FirstOrDefault(t=>t.Id.Equals(userId));
            return Task.FromResult(user??null);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            var user = _users.FirstOrDefault(t => t.UserName.Equals(userName));
            return Task.FromResult(user??null);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<TUser>(null);
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var role = _roles.SingleOrDefault(t=>t.Name == roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Unable to find a role called {roleName}");
            }
            _userRoles.Add(new TUserRole() { UserId = user.Id, RoleId = role.Id });
            return Task.FromResult<object>(null);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var role = _roles.SingleOrDefault(t => t.Name == roleName);
            if (role == null)
            {
                throw new InvalidOperationException($"Unable to find a role called {roleName}");
            }
            var userRole = _userRoles.SingleOrDefault(t => t.UserId.Equals(user.Id)&&t.RoleId.Equals(role.Id));
            if (userRole == null)
            {
                throw new InvalidOperationException();
            }
            _userRoles.Remove(userRole);
            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var userId = user.Id;
            var query = from userRole in _userRoles
                        where userRole.UserId.Equals(userId)
                        join role in _roles on userRole.RoleId equals role.Id
                        select role.Name;
            IList<string> temp = query.ToList();
            return Task.FromResult(temp);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            var role = _roles.SingleOrDefault(t => t.Name == roleName);
            if (role != null)
            {
                return Task.FromResult(_userRoles.Any(t=>t.RoleId.Equals(role.Id)&&t.UserId.Equals(user.Id)));
            }
            return Task.FromResult(false);
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
                _userRoles = null;
                _users = null;
            }
            _disposed = true;
        }
    }
}