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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Linq;
using fNbt;
using Newtonsoft.Json.Linq;

namespace MiNET.Utils
{
	/// <summary>
	///     Reads NBT written as typed JSON into an fNbt tree. A compound is a map of name to
	///     {"type", "value"}; a list is {"type": "List", "value": [ {"type", "value"}, ... ]}.
	///     This is the shape the BDS extraction publishes and the shape the generated data files
	///     under Items/Data carry, so a stack's stored enchantment or firework reads as itself in the
	///     file instead of as a base64 blob.
	/// </summary>
	public static class TypedNbtJson
	{
		/// <summary>
		///     Children go in by name, ordinal, because that is the order Bedrock writes a compound
		///     in, and a data file may list them in any order.
		/// </summary>
		public static NbtCompound ReadCompound(JObject json, string name = "")
		{
			var compound = new NbtCompound(name);
			foreach (JProperty property in json.Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
			{
				compound.Add(ReadTag(property.Name, (JObject) property.Value));
			}

			return compound;
		}

		private static NbtTag ReadTag(string name, JObject typed)
		{
			string type = (string) typed["type"];
			JToken value = typed["value"];

			switch (type)
			{
				case "Byte": return new NbtByte(name, (byte) value);
				case "Short": return new NbtShort(name, (short) value);
				case "Int": return new NbtInt(name, (int) value);
				case "Long": return new NbtLong(name, (long) value);
				case "Float": return new NbtFloat(name, (float) value);
				case "Double": return new NbtDouble(name, (double) value);
				case "String": return new NbtString(name, (string) value);
				case "ByteArray": return new NbtByteArray(name, ((JArray) value).Select(v => (byte) v).ToArray());
				case "IntArray": return new NbtIntArray(name, ((JArray) value).Select(v => (int) v).ToArray());
				case "Compound": return ReadCompound((JObject) value, name);
				case "List": return ReadList(name, (JArray) value);
				default: throw new FormatException($"unknown NBT tag type '{type}'");
			}
		}

		private static NbtList ReadList(string name, JArray elements)
		{
			if (elements.Count == 0) return new NbtList(name, NbtTagType.End);

			var list = new NbtList(name);
			foreach (JToken element in elements) list.Add(ReadTag(null, (JObject) element));
			return list;
		}
	}
}
