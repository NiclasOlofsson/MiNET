using Microsoft.AspNet.Identity;

namespace MiNET.Security
{
	public class Role : IRole<string>
	{
		public string Id { get; private set; }
		public string Name { get; set; }

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
}