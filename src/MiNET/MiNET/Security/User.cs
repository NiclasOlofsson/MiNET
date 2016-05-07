using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
    public class User : User<string,UserRole>
    {
        public User(string userName) : this(userName, userName)
        {

        }

        public User(string id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }

	public class User<TKey,TUserRole> : IUser<TKey>
        where TUserRole : UserRole<TKey>
	{
        public virtual TKey Id { get; set; }
        public virtual string UserName { get; set; }
		public string PasswordHash { get; set; }
		public bool IsAuthenticated { get; set; }
        public ICollection<TUserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new List<TUserRole>();
        }
	}
}