using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
    public class UserRole : UserRole<string>
    {

    }
    public class UserRole<TKey>
    {
        public virtual TKey UserId { get; set; }
        
        public virtual TKey RoleId { get; set; }
    }
}
