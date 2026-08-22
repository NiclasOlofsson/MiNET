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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

namespace MiNET.BdsExtract;

using System.Text;
using System.Text.Json;

/// <summary>
///     The values every block state is known to have, read from Assets/reference-block-states.json.
///     <para>
///         A state is keyed by what it IS, its block's name with its property values, never by its
///         place in the palette. The palette's order is the server's own and two builds do not have
///         to agree on it, so keying on the index would compare one build's state against another
///         build's neighbour and call the difference a moved field.
///     </para>
/// </summary>
public static class StateSentinels
{
	private static Dictionary<string, (int Emission, int Dampening)> _known;

	/// <summary>Every known state, by what it is, with the light it gives off and takes away.</summary>
	public static Dictionary<string, (int Emission, int Dampening)> Known => _known ??= Load();

	/// <summary>
	///     What a state is, as one string: its block's name and every property value it carries.
	///     Sorted, because the order the properties come back in is not part of what the state is.
	/// </summary>
	public static string Key(string name, IEnumerable<StateProperty> properties)
	{
		var text = new StringBuilder(name);
		foreach (string property in properties.Select(p => $"{p.Name}={p.ToJson()}").OrderBy(p => p, StringComparer.Ordinal))
		{
			text.Append('|').Append(property);
		}
		return text.ToString();
	}

	private static Dictionary<string, (int, int)> Load()
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference-block-states.json");
		var known = new Dictionary<string, (int, int)>(StringComparer.Ordinal);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; no state field can be located without it");
			return known;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		if (!document.RootElement.TryGetProperty("states", out JsonElement states)) return known;

		foreach (JsonElement state in states.EnumerateArray())
		{
			if (!state.TryGetProperty("name", out JsonElement name)) continue;
			if (!state.TryGetProperty("lightEmission", out JsonElement emission)) continue;
			if (!state.TryGetProperty("lightDampening", out JsonElement dampening)) continue;

			var text = new StringBuilder(name.GetString() ?? "");
			if (state.TryGetProperty("states", out JsonElement properties))
			{
				foreach (string property in properties.EnumerateObject()
					.Select(p => $"{p.Name}={p.Value.GetRawText()}").OrderBy(p => p, StringComparer.Ordinal))
				{
					text.Append('|').Append(property);
				}
			}

			// A state that appears twice is one the key does not separate, so neither copy is used:
			// a value that could belong to either proves nothing about where it was read from.
			string key = text.ToString();
			if (!known.TryAdd(key, (emission.GetInt32(), dampening.GetInt32()))) known.Remove(key);
		}

		return known;
	}
}
