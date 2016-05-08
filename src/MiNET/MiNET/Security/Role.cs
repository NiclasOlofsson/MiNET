using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
    public class Role : Role<string, UserRole>
    {

        public Role(string name)
            : this(name, name)
        {
        }

        public Role(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class Role<TKey,TUserRole> : IRole<TKey>
        where TUserRole : UserRole<TKey>
	{
		public TKey Id { get; set; }
		public string Name { get; set; }
        
        public virtual ICollection<UserRole> Users { get; private set; }
	}
}