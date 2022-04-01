#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using log4net;

namespace MiNET.Utils
{
	public class UUID
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(UUID));

		private ulong _a;
		private ulong _b;

		private readonly Guid _guid;
		public UUID(byte[] rfc4122Bytes)
		{
			//Log.Warn($"Input hex\n{Package.HexDump(rfc4122Bytes)}");
			_a = BitConverter.ToUInt64(rfc4122Bytes.Skip(0).Take(8).Reverse().ToArray(), 0);
			_b = BitConverter.ToUInt64(rfc4122Bytes.Skip(8).Take(8).Reverse().ToArray(), 0);
			_guid = new Guid(GetBytes());
		}

		public UUID(string uuidString)
		{
			uuidString = uuidString.Replace("-", "");
			var bytes = StringToByteArray(uuidString);
			_a = BitConverter.ToUInt64(bytes.Skip(0).Take(8).ToArray(), 0);
			_b = BitConverter.ToUInt64(bytes.Skip(8).Take(8).ToArray(), 0);
			_guid = new Guid(GetBytes());
		}

		public static byte[] StringToByteArray(string hex)
		{
			return Enumerable.Range(0, hex.Length)
				.Where(x => x % 2 == 0)
				.Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
				.ToArray();
		}

		public byte[] GetBytes()
		{
			var bytes = new byte[0];
			return bytes.Concat(BitConverter.GetBytes(_a).Reverse())
				.Concat(BitConverter.GetBytes(_b).Reverse())
				.ToArray();
		}

		public static implicit operator Guid(UUID uuid)
		{
			return uuid._guid;
		}

		public static explicit operator UUID(Guid guid)
		{
			return new UUID(guid.ToByteArray());
		}

		protected bool Equals(UUID other)
		{
			return _a == other._a && _b == other._b;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((UUID) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (_a.GetHashCode() * 397) ^ _b.GetHashCode();
			}
		}

		public override string ToString()
		{
			var bytes = new byte[0];
			bytes = bytes.Concat(BitConverter.GetBytes(_a))
				.Concat(BitConverter.GetBytes(_b))
				.ToArray();

			string hex = string.Join("", bytes.Select(b => b.ToString("x2")));

			return hex.Substring(0, 8) + "-" + hex.Substring(8, 4) + "-" + hex.Substring(12, 4) + "-" + hex.Substring(16, 4) + "-" + hex.Substring(20, 12);
			//xxxxxxxx-xxxx-Mxxx-Nxxx-xxxxxxxxxxxx 8-4-4-12
			//return Id.ToString();
		}
	}
}