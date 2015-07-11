using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class User : IUser<string>
	{
		public string Id { get; private set; }
		public string UserName { get; set; }
		public string PasswordHash { get; set; }
		public bool IsAuthenticated { get; set; }

		public User(string userName) : this(userName, userName)
		{
		}

		public User(string id, string userName)
		{
			Id = id;
			UserName = userName;
		}
	}
}