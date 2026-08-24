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

using fNbt;
using Newtonsoft.Json.Linq;

namespace MiNET.BlockGen;

/// <summary>
///     Reads the extraction's typed JSON NBT into an fNbt tree. A compound is a map of name to
///     {"type", "value"}; a list is {"type": "List", "value": [ {"type", "value"}, ... ]}.
///     The same reader is in MiNET.Utils, because this tool deliberately does not reference the
///     code it generates for.
/// </summary>
public static class TypedNbt
{
	/// <summary>
	///     Children go in by name, ordinal, because that is the order Bedrock writes a compound in
	///     and the extraction publishes them in whatever order it walked the map.
	/// </summary>
	public static NbtCompound ReadCompound(JObject json, string name)
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

		return type switch
		{
			"Byte" => new NbtByte(name, (byte) value),
			"Short" => new NbtShort(name, (short) value),
			"Int" => new NbtInt(name, (int) value),
			"Long" => new NbtLong(name, (long) value),
			"Float" => new NbtFloat(name, (float) value),
			"Double" => new NbtDouble(name, (double) value),
			"String" => new NbtString(name, (string) value),
			"ByteArray" => new NbtByteArray(name, ((JArray) value).Select(v => (byte) v).ToArray()),
			"IntArray" => new NbtIntArray(name, ((JArray) value).Select(v => (int) v).ToArray()),
			"Compound" => ReadCompound((JObject) value, name),
			"List" => ReadList(name, (JArray) value),
			_ => throw new InvalidDataException($"unknown NBT tag type '{type}'")
		};
	}

	private static NbtList ReadList(string name, JArray elements)
	{
		if (elements.Count == 0) return new NbtList(name, NbtTagType.End);

		var list = new NbtList(name);
		foreach (JToken element in elements) list.Add(ReadTag(null, (JObject) element));
		return list;
	}
}
