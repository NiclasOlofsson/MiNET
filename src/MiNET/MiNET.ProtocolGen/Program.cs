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

using System.Xml;

namespace MiNET.ProtocolGen;

/// <summary>
///     Writes MiNET's protocol registry (handler interfaces, dispatcher, packet factory) and the
///     Cereal packet classes. Two generated paths feed the same compiled result: the T4 template
///     still emits the packet classes that live in MCPE Protocol.xml, while this tool emits the
///     packets Mojang has migrated to Cereal serialization, generated from the JSON schemas BDS
///     writes about itself. The registry covers both sides, so the compiled shapes are identical to
///     what the single T4 path produced before. Over time packets leave the XML for the roster,
///     until the T4 path is empty and this tool owns the whole protocol.
///     <para>
///         Schemas come from a local BDS of the version in MCPE Protocol.xml, which writes them
///         itself when started with a test_config.json of {"generate_documentation":true}. The path
///         is passed in; the schemas are not committed.
///     </para>
///     Same rule as MiNET.BlockGen: no reference to MiNET itself, so the tool can always run even
///     when its own previous output does not compile.
/// </summary>
public static class Program
{
	public static int Main(string[] args)
	{
		string repoRoot = args.Length > 0 ? args[0] : FindRepoRoot();
		string netDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET", "Net");
		string xmlPath = Path.Combine(netDir, "MCPE Protocol.xml");
		string genDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET.ProtocolGen");

		// The schema folder of a local BDS of the version we target: <bds>/docs/json_schemas/protocol.
		string schemaDir = args.Length > 1 ? args[1] : Environment.GetEnvironmentVariable("MINET_SCHEMA_DIR");

		if (!File.Exists(xmlPath))
		{
			Console.Error.WriteLine($"Protocol XML not found: {xmlPath}");
			return 1;
		}
		if (string.IsNullOrWhiteSpace(schemaDir))
		{
			Console.Error.WriteLine("No schema directory given. Pass it as the second argument or set MINET_SCHEMA_DIR.");
			Console.Error.WriteLine(@"Use <bds>\docs\json_schemas\protocol from a BDS matching MCPE Protocol.xml.");
			return 1;
		}
		if (!Directory.Exists(schemaDir))
		{
			Console.Error.WriteLine($"Schema directory not found: {schemaDir}");
			return 1;
		}

		var doc = new XmlDocument();
		doc.Load(xmlPath);
		var xmlPdus = XmlPdu.LoadAll(doc);

		var roster = Roster.Load(Path.Combine(genDir, "roster.json"));
		var overrides = Overrides.Load(Path.Combine(genDir, "overrides.json"));
		var schemas = new SchemaRepo(schemaDir);

		var packets = new List<CerealPacket>();
		var structs = new Dictionary<string, CerealStruct>();
		foreach (RosterEntry entry in roster.Packets)
		{
			packets.Add(CerealPacket.Resolve(entry, schemas, overrides, structs));
		}

		string registryPath = Path.Combine(netDir, "MCPE Protocol Registry.cs");
		RegistryEmitter.Emit(registryPath, xmlPdus, roster.Packets);
		Console.WriteLine($"MCPE Protocol Registry.cs: {xmlPdus.Count} XML pdus + {roster.Packets.Count} Cereal packets");

		string cerealPath = Path.Combine(netDir, "MCPE Protocol Cereal.cs");
		CerealEmitter.Emit(cerealPath, packets, structs);
		Console.WriteLine($"MCPE Protocol Cereal.cs: {packets.Count} packets, {packets.Sum(p => p.Fields.Count)} fields, {structs.Count} structs");

		return 0;
	}

	private static string FindRepoRoot()
	{
		var dir = new DirectoryInfo(AppContext.BaseDirectory);
		while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, ".git"))) dir = dir.Parent;
		return dir?.FullName ?? Directory.GetCurrentDirectory();
	}
}

/// <summary>
///     Name conversion, ported from the T4 template's CodeName/CodeTypeName so a wire name with
///     separators comes out identical on both generated paths. Schema names are often CamelCase
///     with no separators, so a lower-to-upper step is a word break here as well.
/// </summary>
public static class CodeNames
{
	public static string CodeTypeName(string name)
	{
		if (name.StartsWith("ID_")) name = name.Substring(3);
		return CodeName(name, true);
	}

	public static string CodeName(string name, bool firstUpper = false)
	{
		string original = name;
		name = name.ToLowerInvariant();

		string result = string.Empty;
		bool upperCase = firstUpper;

		for (int i = 0; i < name.Length; i++)
		{
			// An apostrophe belongs to its word: "Sender's XUID" is sendersXuid.
			if (name[i] == '\'') continue;

			// Wire names are prose, not identifiers: "Y-Head Rotation" and "Trigger?" both appear.
			// Anything a C# name cannot carry acts as a word break and is dropped.
			if (name[i] == ' ' || name[i] == '_' || !(char.IsLetterOrDigit(name[i]) || name[i] == '[' || name[i] == ']'))
			{
				upperCase = true;
			}
			else
			{
				// "DisplaySlotName" breaks where "Display Slot Name" does.
				if (i > 0 && char.IsUpper(original[i]) && (char.IsLower(original[i - 1]) || char.IsDigit(original[i - 1]))) upperCase = true;

				if ((i == 0 && firstUpper) || upperCase)
				{
					result += name[i].ToString().ToUpperInvariant();
					upperCase = false;
				}
				else
				{
					result += name[i];
				}
			}
		}

		result = result.Replace(@"[]", "s");

		// RequestAbility has fields literally named "Bool" and "Float". A verbatim identifier is
		// what C# offers for that, and it does not change the name we emit anywhere else.
		if (Keywords.Contains(result)) result = "@" + result;

		return result;
	}

	private static readonly HashSet<string> Keywords = new HashSet<string>
	{
		"abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked", "class", "const",
		"continue", "decimal", "default", "delegate", "do", "double", "else", "enum", "event", "explicit", "extern",
		"false", "finally", "fixed", "float", "for", "foreach", "goto", "if", "implicit", "in", "int", "interface",
		"internal", "is", "lock", "long", "namespace", "new", "null", "object", "operator", "out", "override",
		"params", "private", "protected", "public", "readonly", "ref", "return", "sbyte", "sealed", "short",
		"sizeof", "stackalloc", "static", "string", "struct", "switch", "this", "throw", "true", "try", "typeof",
		"uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual", "void", "volatile", "while",
	};
}
