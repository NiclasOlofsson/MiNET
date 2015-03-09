using System;
using System.Linq;
using System.Security.Principal;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		private string _roles;
		private string[] _rolesSplit = new string[0];
		private string _users;
		private string[] _usersSplit = new string[0];

		public string Roles
		{
			get { return _roles ?? String.Empty; }
			set
			{
				_roles = value;
				_rolesSplit = SplitString(value);
			}
		}

		public string Users
		{
			get { return _users ?? String.Empty; }
			set
			{
				_users = value;
				_usersSplit = SplitString(value);
			}
		}

		protected virtual bool AuthorizeCore(GenericPrincipal player)
		{
			if (player == null) throw new ArgumentNullException("player");

			if (_usersSplit.Length > 0 && !_usersSplit.Contains(player.Identity.Name, StringComparer.OrdinalIgnoreCase))
			{
				return false;
			}

			if (_rolesSplit.Length > 0 && !_rolesSplit.Any(player.IsInRole))
			{
				return false;
			}

			return true;
		}


		public virtual bool OnAuthorization(GenericPrincipal player)
		{
			return AuthorizeCore(player);
		}

		internal static string[] SplitString(string original)
		{
			if (String.IsNullOrEmpty(original))
			{
				return new string[0];
			}

			var split = from piece in original.Split(',')
				let trimmed = piece.Trim()
				where !String.IsNullOrEmpty(trimmed)
				select trimmed;
			return split.ToArray();
		}
	}
}