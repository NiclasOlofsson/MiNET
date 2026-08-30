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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     One class named by one clang signature literal: the template argument the compiler
///     substituted, the owner the instantiation belongs to, and every string in the image that
///     names that pair.
/// </summary>
/// <param name="Name">The template argument verbatim, namespace and all, without its own template arguments.</param>
/// <param name="Args">The template arguments split off the name, or null when the name carries none.</param>
/// <param name="Family">The owner text: the last qualified name component before the function name.</param>
/// <param name="SignatureRvas">The RVA of every literal that names this pair.</param>
public sealed record InventoryClass(string Name, string Args, string Family, IReadOnlyList<uint> SignatureRvas);

/// <summary>
///     The class inventory the build states about itself.
///
///     Clang bakes __PRETTY_FUNCTION__ into the assertion and diagnostic paths of templated code,
///     so every instantiation the linker kept leaves a literal naming both the template that was
///     instantiated and the type it was instantiated with: "const T *BlockComponentStorage::
///     tryGetComponent() const [T = BlockChestObstructionComponent]". Reading those literals out of
///     the data sections is the complete list of component classes for this build, stated by the
///     build rather than carried in a table this tool maintains.
///
///     Two spellings of the same construct are read, because the two component sides use different
///     templates. The block side substitutes a parameter named T ("[T = X]"), the item side goes
///     through cereal and entt, whose parameter is named Type ("[Type = X]"). A literal may carry
///     more parameters after the first ("[Len = 16, Type = X]", "[T = X, Args = &lt;&gt;]"); the
///     first parameter of each spelling is the class, the rest are recorded as Args.
///
///     Nothing is filtered here. Every family the templates name comes out, and which family a
///     caller wants is the caller's call.
/// </summary>
public static class SignatureStrings
{
	private static readonly byte[][] Markers = [Encoding.ASCII.GetBytes(" [T = "), Encoding.ASCII.GetBytes(" [Type = ")];

	public static IReadOnlyList<InventoryClass> Find(PeImage image)
	{
		var found = new Dictionary<(string Name, string Args, string Family), List<uint>>();

		foreach (PeSection section in image.Sections)
		{
			// The literals live in the read-only data sections. .text is skipped because a byte
			// pattern that spells " [T = " inside code is not a string, and every real literal is
			// reachable from a data section anyway.
			if (section.Name == ".text") continue;
			if (section.RawSize <= 0) continue;

			ReadOnlySpan<byte> bytes = image.Bytes(section.VirtualAddress, section.RawSize);
			foreach (byte[] marker in Markers)
			{
				for (int at = 0; at < bytes.Length;)
				{
					int hit = bytes.Slice(at).IndexOf(marker);
					if (hit < 0) break;
					int markerAt = at + hit;
					at = markerAt + 1;

					// The literal is the NUL-terminated string the marker sits inside. Walk back to
					// the byte after the previous NUL and forward to the next one, so the bounds
					// are the string's own and not a window this code chose.
					int start = markerAt;
					while (start > 0 && bytes[start - 1] != 0) start--;
					int end = markerAt + marker.Length;
					while (end < bytes.Length && bytes[end] != 0) end++;
					if (end >= bytes.Length) continue;

					string literal = Encoding.ASCII.GetString(bytes.Slice(start, end - start));
					int markerInLiteral = markerAt - start;
					if (!TryParse(literal, markerInLiteral, marker.Length, out string name, out string args, out string family)) continue;

					uint rva = section.VirtualAddress + (uint) start;
					var key = (name, args, family);
					if (!found.TryGetValue(key, out List<uint> list)) found[key] = list = new List<uint>();
					if (!list.Contains(rva)) list.Add(rva);
				}
			}
		}

		return found
			.Select(f => new InventoryClass(f.Key.Name, f.Key.Args, f.Key.Family, f.Value))
			.OrderBy(c => c.Family, StringComparer.Ordinal)
			.ThenBy(c => c.Name, StringComparer.Ordinal)
			.ToList();
	}

	/// <summary>
	///     Splits one literal into the class it names and the owner it belongs to. The text after
	///     the marker is the substituted parameter up to the first top-level comma or the closing
	///     bracket; the text before the marker is the function signature, whose owner is everything
	///     up to the last "::" that sits outside template brackets, with the return type dropped.
	/// </summary>
	private static bool TryParse(string literal, int markerAt, int markerLength, out string name, out string args, out string family)
	{
		name = null;
		args = null;
		family = null;

		int valueStart = markerAt + markerLength;
		int valueEnd = -1;
		int depth = 0;
		for (int i = valueStart; i < literal.Length; i++)
		{
			char c = literal[i];
			if (c == '<' || c == '(' || c == '[') depth++;
			else if (c == '>' || c == ')') depth--;
			else if (c == ']')
			{
				if (depth == 0) { valueEnd = i; break; }
				depth--;
			}
			else if (c == ',' && depth == 0) { valueEnd = i; break; }
		}
		if (valueEnd < 0 || valueEnd <= valueStart) return false;

		string value = literal.Substring(valueStart, valueEnd - valueStart).Trim();
		if (value.Length == 0) return false;

		// The class's own template arguments are split off, so BasicFactory<X> and X are one name
		// with different Args rather than two unrelated classes.
		int angle = TopLevelAngle(value);
		if (angle > 0)
		{
			args = value.Substring(angle);
			name = value.Substring(0, angle);
		}
		else
		{
			name = value;
		}
		if (name.Length == 0) return false;

		family = Owner(literal.Substring(0, markerAt));
		return family != null;
	}

	/// <summary>The index of the first top-level '&lt;' in a type name, or -1 when it carries none.</summary>
	private static int TopLevelAngle(string value)
	{
		for (int i = 0; i < value.Length; i++)
		{
			if (value[i] == '<') return i;
		}
		return -1;
	}

	/// <summary>
	///     The owner of a function signature: the qualified name up to the last "::" outside
	///     template brackets, with the return type dropped. "const T *BlockComponentStorage::
	///     tryGetComponent() const" gives "BlockComponentStorage"; "static T *Scripting::internal::
	///     ObjectRegistryUtilities::tryGet" gives "Scripting::internal::ObjectRegistryUtilities".
	///     The namespace stays on, because the last component alone collides: entt::internal and
	///     cereal::internal are two owners spelled "internal". A signature with no "::" outside
	///     template brackets has no owner and yields null.
	/// </summary>
	private static string Owner(string signature)
	{
		// The parameter list can hold its own "::", so only the head up to the first top-level '('
		// is the qualified function name.
		int depth = 0;
		int head = signature.Length;
		for (int i = 0; i < signature.Length; i++)
		{
			char c = signature[i];
			if (c == '<') depth++;
			else if (c == '>') depth--;
			else if (c == '(' && depth == 0) { head = i; break; }
		}

		depth = 0;
		int lastSeparator = -1;
		for (int i = 0; i + 1 < head; i++)
		{
			char c = signature[i];
			if (c == '<') depth++;
			else if (c == '>') depth--;
			else if (c == ':' && depth == 0 && signature[i + 1] == ':') lastSeparator = i;
		}
		if (lastSeparator < 0) return null;

		string qualified = signature.Substring(0, lastSeparator);

		// Drop the return type, which is whatever sits before the last space, '*' or '&' outside
		// template brackets: "const T *BlockComponentStorage" is a return type and an owner.
		depth = 0;
		int nameStart = 0;
		for (int i = 0; i < qualified.Length; i++)
		{
			char c = qualified[i];
			if (c == '<') depth++;
			else if (c == '>') depth--;
			else if (depth == 0 && (c == ' ' || c == '*' || c == '&')) nameStart = i + 1;
		}
		string owner = qualified.Substring(nameStart);
		return owner.Length == 0 ? null : owner;
	}
}
