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
using Newtonsoft.Json.Linq;

namespace MiNET.ProtocolGen;

/// <summary>One pdu entry from MCPE Protocol.xml, as the registry emitter needs it.</summary>
public class XmlPdu
{
	public string Id;
	public string Name;
	public string Namespace;
	public bool Client;
	public bool Server;

	public static List<XmlPdu> LoadAll(XmlDocument doc)
	{
		var result = new List<XmlPdu>();
		foreach (XmlNode pdu in doc.SelectNodes("//pdu"))
		{
			result.Add(new XmlPdu
			{
				Id = pdu.Attributes["id"].Value,
				Name = pdu.Attributes["name"].Value,
				Namespace = pdu.Attributes["namespace"]?.Value ?? "mcpe",
				Client = pdu.Attributes["client"]?.Value == "true",
				Server = pdu.Attributes["server"]?.Value == "true",
			});
		}
		return result;
	}
}

public class Roster
{
	public List<RosterEntry> Packets = new();

	public static Roster Load(string path)
	{
		var json = JObject.Parse(File.ReadAllText(path));
		var roster = new Roster();
		foreach (JObject p in json["packets"])
		{
			roster.Packets.Add(new RosterEntry
			{
				Name = (string) p["name"],
				Id = (string) p["id"],
				Online = (bool?) p["online"] ?? false,
				Client = (bool?) p["client"] ?? false,
				Server = (bool?) p["server"] ?? false,
				Schema = (string) p["schema"],
			});
		}
		return roster;
	}
}

public class RosterEntry
{
	public string Name;
	public string Id;
	public bool Online;
	public bool Client;
	public bool Server;
	public string Schema;

	public string TypeName => CodeNames.CodeTypeName(Name);
}

public class Overrides
{
	public Dictionary<string, TypeMapping> Types = new();
	public Dictionary<string, PacketOverride> Packets = new();

	public static Overrides Load(string path)
	{
		var json = JObject.Parse(File.ReadAllText(path));
		var overrides = new Overrides();
		foreach (var prop in ((JObject) json["types"]).Properties())
		{
			var t = (JObject) prop.Value;
			overrides.Types[prop.Name] = new TypeMapping
			{
				CsType = (string) t["csType"],
				Write = (string) t["write"],
				Read = (string) t["read"],
			};
		}
		foreach (var prop in ((JObject) json["packets"]).Properties())
		{
			var p = new PacketOverride();
			p.TypeName = (string) ((JObject) prop.Value)["typeName"];
			var fields = (JObject) ((JObject) prop.Value)["fields"];
			if (fields != null)
			{
				foreach (var f in fields.Properties())
				{
					var o = (JObject) f.Value;
					if (o["name"] != null) p.FieldNames[f.Name] = (string) o["name"];
					if (o["enum"] != null) p.FieldEnums[f.Name] = ((JArray) o["enum"]).Select(v => (string) v).ToList();
					if (o["optional"] != null) p.FieldOptional[f.Name] = (bool) o["optional"];
					if (o["array"] != null) p.FieldArray[f.Name] = (bool) o["array"];
					if (o["presenceBytes"] != null) p.FieldPresenceBytes[f.Name] = (int) o["presenceBytes"];
					if (o["presentWhen"] != null)
					{
						var when = (JObject) o["presentWhen"];
						p.FieldPresentWhen[f.Name] = new FieldCondition
						{
							Field = (string) when["field"],
							Values = ((JArray) (when["in"] ?? when["notIn"])).Select(v => (int) v).ToList(),
							Negated = when["notIn"] != null
						};
					}
					if (o["variantPayloadGate"] != null) p.FieldVariantPayloadGate[f.Name] = (bool) o["variantPayloadGate"];
					if (o["variantConstIsByte"] != null) p.FieldVariantConstIsByte[f.Name] = (bool) o["variantConstIsByte"];
					if (o["type"] != null)
					{
						var t = (JObject) o["type"];
						p.FieldTypes[f.Name] = new TypeMapping
						{
							CsType = (string) t["csType"],
							Write = (string) t["write"],
							Read = (string) t["read"],
						};
					}
				}
			}
			overrides.Packets[prop.Name] = p;
		}
		return overrides;
	}
}

public class PacketOverride
{
	/// <summary>Emitted type name for this schema struct, where the schema title collides with or misnames the house type (schema "CameraPresets" is one preset, emitted as CameraPreset).</summary>
	public string TypeName;

	/// <summary>Wire field name -> MiNET member name, where the schema-derived name is not the one the code should carry.</summary>
	public Dictionary<string, string> FieldNames = new();

	/// <summary>Wire field name -> forced type mapping, for fields the schema leaves untyped (NBT payloads and the like).</summary>
	public Dictionary<string, TypeMapping> FieldTypes = new();

	/// <summary>Wire field name -> replacement enum value list, for inline enums the schema gets wrong.</summary>
	public Dictionary<string, List<string>> FieldEnums = new();

	/// <summary>
	///     Wire field name -> forced optionality, for fields the schema's "required" list contradicts.
	///     A field that is optional carries a presence byte, so getting this wrong desyncs the rest
	///     of the struct; the schema has been wrong about it, so the changelog wins.
	/// </summary>
	public Dictionary<string, bool> FieldOptional = new();

	/// <summary>
	///     Wire field name -> forced vector-ness, for fields the schema declares as a single struct
	///     while the wire carries a varint-counted list of them (CameraPresets' payload).
	/// </summary>
	public Dictionary<string, bool> FieldArray = new();

	/// <summary>
	///     Wire field name -> how many presence bytes gate an optional field, default one. Mojang
	///     encodes a cross-field invariant ("Containers is present iff Result == Success") as a byte
	///     of its own in front of the optional's own presence byte, so the field is absent when
	///     either is false. The schema says only "optional", so the count comes from here.
	/// </summary>
	public Dictionary<string, int> FieldPresenceBytes = new();

	/// <summary>Wire field name -> the condition under which the field is on the wire at all.</summary>
	public Dictionary<string, FieldCondition> FieldPresentWhen = new();

	/// <summary>Wire field name -> the variant's payload carries its own presence byte after the tag.</summary>
	public Dictionary<string, bool> FieldVariantPayloadGate = new();

	/// <summary>Wire field name -> the options' discriminator const is the tag as a byte, not the name.</summary>
	public Dictionary<string, bool> FieldVariantConstIsByte = new();
}

public class TypeMapping
{
	public string CsType;
	public string Write;
	public string Read;
}

/// <summary>Loads and caches the Mojang JSON schema files, resolving "./X.json" refs.</summary>
public class SchemaRepo
{
	private readonly string _dir;
	private readonly Dictionary<string, JObject> _cache = new();

	public SchemaRepo(string dir)
	{
		_dir = dir;
	}

	public JObject Get(string name)
	{
		if (_cache.TryGetValue(name, out JObject cached)) return cached;

		string path = Path.Combine(_dir, name + ".json");
		if (!File.Exists(path)) throw new FileNotFoundException($"Schema not found: {path}");
		var schema = JObject.Parse(File.ReadAllText(path));
		_cache[name] = schema;
		return schema;
	}

	/// <summary>Turns "./BlockPos.json" (or "/BlockPos.json") into the schema name "BlockPos".</summary>
	public static string RefName(string reference)
	{
		string name = reference;
		if (name.StartsWith("./")) name = name.Substring(2);
		if (name.StartsWith("/")) name = name.Substring(1);
		if (name.EndsWith(".json")) name = name.Substring(0, name.Length - 5);
		return name;
	}
}

public enum FieldKind
{
	/// <summary>Primitive or mapped type: one write call, one read call.</summary>
	Plain,
	/// <summary>A generated data class (multi-field schema struct).</summary>
	Struct,
	/// <summary>A varint-counted vector of structs or primitives.</summary>
	Array,
	/// <summary>A tagged variant (oneOf): varint tag in declaration order, then the selected payload.</summary>
	Variant,
	/// <summary>A varint-counted string-keyed dictionary: each entry is the key string, then the value.</summary>
	Map,
}

public class CerealField
{
	public string WireName;
	public string FieldName;
	public int Ordinal;
	public bool Optional;
	public FieldKind Kind;
	public TypeMapping Type;
	public CerealStruct Struct;
	public CerealEnum Enum;
	/// <summary>For arrays: the element, itself a Plain or Struct field.</summary>
	public CerealField Element;
	/// <summary>For variants: the abstract base plus the options in tag order.</summary>
	public CerealVariant Variant;

	/// <summary>
	///     How many wire slots a variant field occupies. Mojang declares one oneOf property per
	///     option and binds them all to a single field, so the payload appears N times and each read
	///     overwrites the last. One field, N slots, the last one present wins.
	/// </summary>
	public int VariantSlots = 1;

	/// <summary>How many presence bytes gate this field when it is optional. See PacketOverride.FieldPresenceBytes.</summary>
	public int PresenceBytes = 1;

	/// <summary>
	///     A variant whose payload carries its own presence byte after the tag. Mojang's docs
	///     collapse a pair of C++ optionals, one for the discriminator and one for the data, into a
	///     single oneOf, so the wire is presence, tag, presence, payload where the schema shows only
	///     a required variant.
	/// </summary>
	/// <summary>Set when the field is conditional on another field's value.</summary>
	public FieldCondition PresentWhen;

	public bool VariantPayloadGate;

	/// <summary>
	///     The options' discriminator const is a byte carrying the variant tag, not the name spelled
	///     out. Both forms exist: item stack request actions write the byte (CloudburstMC reads it
	///     with byteBuf.readByte after the tag), SetScore entries write the name.
	/// </summary>
	public bool VariantConstIsByte;

	/// <summary>For const-discriminator fields inside variant payloads: the literal written on the wire.</summary>
	public string ConstValue;

	/// <summary>
	///     Qualified reference to the field's nested enum type ("LevelSettings.Xboxlivebroadcastsetting"),
	///     stamped once the owning class is known. Enum-schema'd fields are declared with this type so an
	///     out-of-enum constant cannot be assigned silently; the wire keeps the underlying primitive.
	/// </summary>
	public string EnumRef;

	// Vector2/Vector3 are structs too: leaving them out made an optional Vec2 emit as a bare
	// Vector2 whose "!= null" presence check lifts to always-true, so the presence bool wrote
	// true for a field the sender never set.
	private static readonly HashSet<string> ValueTypes = new() {"bool", "byte", "sbyte", "short", "ushort", "int", "uint", "long", "ulong", "float", "Vector2", "Vector3"};

	public bool IsValueType => Kind == FieldKind.Plain && (Enum != null || ValueTypes.Contains(Type.CsType));

	public string CsType => Kind switch
	{
		FieldKind.Struct => Struct.Name,
		FieldKind.Array => $"List<{Element.CsType}>",
		FieldKind.Map => $"Dictionary<string, {Element.CsType}>",
		FieldKind.Variant => Variant.BaseName,
		_ when Enum != null => Optional ? (EnumRef ?? Enum.Name) + "?" : EnumRef ?? Enum.Name,
		_ => Optional && IsValueType ? Type.CsType + "?" : Type.CsType,
	};
}

/// <summary>A oneOf tagged variant: an abstract base class and one struct per option, tag = declaration index.</summary>
/// <summary>A field that is only on the wire for certain values of a field before it. The schema has
/// no way to say this, and reading such a field unconditionally takes the bytes of whatever follows.</summary>
public class FieldCondition
{
	public string Field;
	public List<int> Values = new();

	/// <summary>The field is present for every value EXCEPT the ones listed.</summary>
	public bool Negated;
}

public class CerealVariant
{
	public string BaseName;
	public List<CerealStruct> Options = new();
}

/// <summary>A multi-field schema struct, emitted as a plain data class plus Packet read/write methods.</summary>
public class CerealStruct
{
	public string Name;
	/// <summary>Set when this struct is a variant option: the abstract base it extends.</summary>
	public string BaseName;
	public List<CerealField> Fields = new();
	public List<CerealEnum> Enums = new();
}

/// <summary>An inline Enum-as-Value enum, emitted nested in the packet class, house style.</summary>
public class CerealEnum
{
	public string Name;
	public List<string> Values = new();
}

/// <summary>A packet resolved from its schema into an ordered list of emittable fields.</summary>
public class CerealPacket
{
	public RosterEntry Entry;
	public List<CerealField> Fields = new();
	public List<CerealEnum> Enums = new();

	public static CerealPacket Resolve(RosterEntry entry, SchemaRepo schemas, Overrides overrides, Dictionary<string, CerealStruct> structs)
	{
		JObject packetSchema = schemas.Get(entry.Schema);

		string payloadRef = (string) packetSchema["$ref"];
		if (payloadRef == null) throw new InvalidOperationException($"{entry.Schema}: packet schema has no $ref payload");
		JObject payload = schemas.Get(SchemaRepo.RefName(payloadRef));

		var packet = new CerealPacket {Entry = entry};
		overrides.Packets.TryGetValue(entry.Schema, out PacketOverride packetOverride);

		foreach (CerealField field in CollapseVariantSlots(entry.Schema, ResolveFields(entry.Schema, payload, schemas, overrides, structs, packetOverride)))
		{
			AttachEnum(field, entry.TypeName, packet.Enums);
			packet.Fields.Add(field);
		}

		return packet;
	}

	/// <summary>
	///     Folds a run of adjacent variant fields that share one option set into a single field with
	///     that many wire slots. The schema names each slot after one of its own options, which would
	///     otherwise emit the option classes once per slot and give the base a name it collides with,
	///     so the surviving field and its base are named after the packet instead.
	/// </summary>
	private static IEnumerable<CerealField> CollapseVariantSlots(string owner, IEnumerable<CerealField> fields)
	{
		List<CerealField> all = fields.ToList();

		for (int i = 0; i < all.Count; i++)
		{
			CerealField field = all[i];
			if (field.Kind != FieldKind.Variant)
			{
				yield return field;
				continue;
			}

			string Signature(CerealField f) => f.Kind != FieldKind.Variant ? null : string.Join(",", f.Variant.Options.Select(o => o.Name));

			string signature = Signature(field);
			int slots = 1;
			while (i + slots < all.Count && Signature(all[i + slots]) == signature) slots++;

			if (slots > 1)
			{
				string name = SanitizeTypeName(owner.Replace("Packet", "").Replace("Payload", ""));
				field.Variant.BaseName = name + "ParamBase";
				field.FieldName = char.ToLowerInvariant(field.Variant.BaseName[0]) + field.Variant.BaseName.Substring(1);

				// The option classes are shared across the slots and recorded their base as each
				// slot resolved, so the last slot's name is on them. Point them at the real base.
				foreach (CerealStruct option in field.Variant.Options) option.BaseName = field.Variant.BaseName;
				field.VariantSlots = slots;
				i += slots - 1;
			}

			yield return field;
		}
	}

	private static IEnumerable<CerealField> ResolveFields(string owner, JObject objectSchema, SchemaRepo schemas, Overrides overrides, Dictionary<string, CerealStruct> structs, PacketOverride packetOverride)
	{
		var required = new HashSet<string>(((JArray) objectSchema["required"] ?? new JArray()).Select(t => (string) t));
		var properties = ((JObject) objectSchema["properties"])?.Properties().ToList() ?? new List<JProperty>();

		foreach (JProperty prop in properties.OrderBy(p => (int) ((JObject) p.Value)["x-ordinal-index"]))
		{
			string memberName = null;
			packetOverride?.FieldNames.TryGetValue(prop.Name, out memberName);
			memberName ??= CodeNames.CodeName(prop.Name);

			bool optional = !required.Contains(prop.Name);
			if (packetOverride != null && packetOverride.FieldOptional.TryGetValue(prop.Name, out bool forcedOptional))
			{
				optional = forcedOptional;
			}

			int presenceBytes = 1;
			packetOverride?.FieldPresenceBytes.TryGetValue(prop.Name, out presenceBytes);

			var field = new CerealField
			{
				WireName = prop.Name,
				FieldName = memberName,
				Ordinal = (int) ((JObject) prop.Value)["x-ordinal-index"],
				Optional = optional,
				PresenceBytes = presenceBytes < 1 ? 1 : presenceBytes,
				PresentWhen = packetOverride != null && packetOverride.FieldPresentWhen.TryGetValue(prop.Name, out FieldCondition when) ? when : null,
				VariantPayloadGate = packetOverride != null && packetOverride.FieldVariantPayloadGate.TryGetValue(prop.Name, out bool gate) && gate,
				VariantConstIsByte = packetOverride != null && packetOverride.FieldVariantConstIsByte.TryGetValue(prop.Name, out bool cb) && cb,
			};

			TypeMapping forced = null;
			packetOverride?.FieldTypes.TryGetValue(prop.Name, out forced);
			if (forced?.CsType != null)
			{
				field.Kind = FieldKind.Plain;
				field.Type = forced;
			}
			else
			{
				ResolveType(owner, field, (JObject) prop.Value, schemas, overrides, structs);

				// An override with a read and a write but no csType corrects how the field travels
				// while keeping the type the schema resolved, which is the only way to fix the codec
				// of a field whose type is an inline enum: naming a csType suppresses the enum.
				if (forced != null && field.Type != null)
				{
					field.Type = new TypeMapping
					{
						CsType = field.Type.CsType,
						Write = forced.Write ?? field.Type.Write,
						Read = forced.Read ?? field.Type.Read
					};
				}
			}

			if (field.Enum != null && packetOverride != null && packetOverride.FieldEnums.TryGetValue(prop.Name, out List<string> enumValues))
			{
				field.Enum.Values = enumValues.Select(v => CodeNames.CodeName(v, true)).ToList();
			}

			if (packetOverride != null && packetOverride.FieldArray.TryGetValue(prop.Name, out bool forceArray) && forceArray && field.Kind != FieldKind.Array)
			{
				// The schema declared one value where the wire carries a varint-counted list of
				// them: rewrap what ResolveType produced as this field's element.
				field.Element = new CerealField
				{
					WireName = field.WireName + " element",
					FieldName = "item",
					Kind = field.Kind,
					Struct = field.Struct,
					Type = field.Type,
					Enum = field.Enum,
					EnumRef = field.EnumRef,
					Variant = field.Variant,
				};
				field.Kind = FieldKind.Array;
				field.Struct = null;
				field.Type = null;
				field.Enum = null;
				field.Variant = null;
			}

			yield return field;
		}
	}

	private static void ResolveType(string owner, CerealField field, JObject prop, SchemaRepo schemas, Overrides overrides, Dictionary<string, CerealStruct> structs)
	{
		if (prop["const"] != null)
		{
			// Variant discriminator: a constant written on the wire, not a class member. The schema
			// types it uint8, which is wrong: the value is the name, spelled out as a string, and it
			// follows the variant's own varint tag. CloudburstMC's SetScoreSerializer_v2168 writes
			// both, VarInts.writeUnsignedInt(type.ordinal()) then writeString("changefakeplayer"),
			// and recipe ingredients are the same shape against live BDS bytes. Emitting the byte
			// the schema's type implies makes a 2168 client drop the connection 55ms after the
			// packet lands (measured on SetScore, 2026-08-09).
			field.ConstValue = (string) prop["const"];
			field.Kind = FieldKind.Plain;
			field.Type = new TypeMapping {CsType = "string", Write = $"Write(\"{(string) prop["const"]}\");", Read = "ReadString();"};
			return;
		}

		if ((string) prop["type"] == "object" && prop["additionalProperties"] != null)
		{
			// A string-keyed map (DimensionData's Definitions and friends): varint count, then per
			// entry the key string followed by the value. The schema spells the key constraint in
			// propertyNames; anything but a plain string key has no known wire form.
			if ((string) prop["propertyNames"]?["type"] != "string")
				throw new NotImplementedException($"{owner}.{field.WireName}: map with non-string keys is not implemented yet");

			var mapElement = new CerealField
			{
				WireName = field.WireName + " element",
				FieldName = "item",
			};
			ResolveType(owner, mapElement, (JObject) prop["additionalProperties"], schemas, overrides, structs);
			if (mapElement.Kind is FieldKind.Array or FieldKind.Map or FieldKind.Variant)
				throw new NotImplementedException($"{owner}.{field.WireName}: map values of kind {mapElement.Kind} are not implemented yet");
			field.Kind = FieldKind.Map;
			field.Element = mapElement;
			return;
		}

		if ((string) prop["type"] == "array")
		{
			// The variant flags belong to the field the override names, but for a vector the oneOf
			// sits on the element, so they have to travel with it.
			var element = new CerealField
			{
				WireName = field.WireName + " element",
				FieldName = "item",
				VariantConstIsByte = field.VariantConstIsByte,
				VariantPayloadGate = field.VariantPayloadGate,
			};
			ResolveType(owner, element, (JObject) prop["items"], schemas, overrides, structs);
			if (element.Kind == FieldKind.Array) throw new NotImplementedException($"{owner}.{field.WireName}: nested arrays are not implemented yet");
			field.Kind = FieldKind.Array;
			field.Element = element;
			return;
		}

		string reference = (string) prop["$ref"];
		if (reference != null)
		{
			string name = SchemaRepo.RefName(reference);
			if (overrides.Types.TryGetValue(name, out TypeMapping mapped))
			{
				field.Kind = FieldKind.Plain;
				field.Type = mapped;
				return;
			}

			JObject target = schemas.Get(name);

			if (target["enum"] != null)
			{
				// A standalone enum schema (ActorLinkType and friends): value = declaration index,
				// written as the underlying primitive.
				field.Enum = new CerealEnum
				{
					Name = SanitizeTypeName((string) target["title"]),
					Values = ((JArray) target["enum"]).Select(v => CodeNames.CodeName((string) v, true)).ToList(),
				};
				field.Kind = FieldKind.Plain;
				// Compression is a property of the reference, not of the enum: GameType.json says
				// nothing, while AddPlayer's Player Game Type asks for it and StartGame's does too.
				bool refCompressed = HasOption(prop, "Compression") || HasOption(target, "Compression");
				field.Type = Primitive(owner, field.WireName, (string) target["x-underlying-type"], refCompressed);
				return;
			}

			var targetProps = ((JObject) target["properties"])?.Properties().ToList();
			if (targetProps != null && targetProps.Count == 1)
			{
				// Single-field wrapper (ActorRuntimeID and friends): flatten to its primitive.
				ResolveType(owner, field, (JObject) targetProps[0].Value, schemas, overrides, structs);
				return;
			}

			if (targetProps != null && targetProps.Count > 1)
			{
				field.Kind = FieldKind.Struct;
				field.Struct = ResolveStruct(name, target, schemas, overrides, structs);
				return;
			}

			throw new NotImplementedException($"{owner}.{field.WireName}: type {name} has no properties; needs a types override");
		}

		if (prop["oneOf"] != null)
		{
			var options = ((JArray) prop["oneOf"]).Cast<JObject>().ToList();
			if (options.Any(o => o["$ref"] == null))
				throw new NotImplementedException($"{owner}.{field.WireName}: inline variant options (a bare 'null' alternative and the like) are not implemented yet");

			var refs = options.Select(o => SchemaRepo.RefName((string) o["$ref"])).ToList();
			var titles = refs.Select(r => (string) schemas.Get(r)["title"]).ToList();

			// Base name from the options' common title prefix ("Resource Pack Client Response - Cancel" ...).
			string prefix = titles.Aggregate((a, b) =>
			{
				int i = 0;
				while (i < a.Length && i < b.Length && a[i] == b[i]) i++;
				return a.Substring(0, i);
			}).TrimEnd(' ', '-');
			// Options are usually named after the choice they belong to ("Resource Pack Client
			// Response - Cancel"), so the shared prefix names the base. Where they are not
			// (EmptyItemDescriptor / ItemNameDescriptor / MolangItemDescriptor share no prefix at
			// all), the field's own wire name is the choice, so use that.
			var variant = new CerealVariant
			{
				// The Base suffix is not decoration. Mojang also names options after their owner
				// ("ItemStackRequestTakeAction"), so the shared prefix is then the owning type's own
				// name, and the base would be emitted twice: once as the abstract base, once as the
				// struct that holds the list. Same convention as the block family bases.
				BaseName = SanitizeTypeName((prefix.Length > 0 ? prefix : field.WireName).Replace("-", " ")) + "Base"
			};
			for (int i = 0; i < refs.Count; i++)
			{
				string optionName = SanitizeTypeName(titles[i].Replace("-", " "));
				CerealStruct option = ResolveStruct(refs[i], schemas.Get(refs[i]), schemas, overrides, structs);
				option.Name = optionName;

				// ResolveStruct qualified any nested enums against the schema-derived name, which
				// the line above just replaced. Re-qualify, or the class and its own enum
				// references disagree and neither resolves.
				foreach (CerealField optionField in option.Fields)
				{
					foreach (CerealField carrier in new[] {optionField, optionField.Element})
					{
						if (carrier?.Enum != null) carrier.EnumRef = $"{optionName}.{carrier.Enum.Name}";
					}
				}

				// Where the discriminator is a byte it repeats the tag, which is only known here.
				if (field.VariantConstIsByte)
				{
					foreach (CerealField optionField in option.Fields)
					{
						if (optionField.ConstValue != null) optionField.Type = new TypeMapping {CsType = "byte", Write = $"Write((byte) {i});", Read = "ReadByte();"};
					}
				}

				option.BaseName = variant.BaseName;
				variant.Options.Add(option);
			}

			field.Kind = FieldKind.Variant;
			field.Variant = variant;
			return;
		}

		if (prop["enum"] != null)
		{
			if (!HasOption(prop, "Enum-as-Value"))
				throw new NotImplementedException($"{owner}.{field.WireName}: enum without Enum-as-Value is not implemented yet");

			field.Enum = new CerealEnum
			{
				// Named from the wire name, not the field name: the field name is one lower-case
				// token by then (and may have been renamed by an override), so it would come back
				// out as "Creativecategory" where the wire name still carries the word breaks.
				Name = CodeNames.CodeTypeName(field.WireName),
				Values = ((JArray) prop["enum"]).Select(v => CodeNames.CodeName((string) v, true)).ToList(),
			};
			field.Kind = FieldKind.Plain;
			field.Type = Primitive(owner, field.WireName, (string) prop["x-underlying-type"], HasOption(prop, "Compression"));
			return;
		}

		// Strings sometimes carry no x-underlying-type; the JSON type is enough.
		string underlying = (string) prop["x-underlying-type"] ?? (string) prop["type"];
		if (underlying == null)
			throw new NotImplementedException($"{owner}.{field.WireName}: field has no type information; needs a field type override (NBT payload?)");
		field.Kind = FieldKind.Plain;
		field.Type = Primitive(owner, field.WireName, underlying, HasOption(prop, "Compression"));
	}

	private static bool HasOption(JObject schema, string option)
	{
		return ((JArray) schema["x-serialization-options"])?.Any(o => (string) o == option) ?? false;
	}

	private static CerealStruct ResolveStruct(string name, JObject schema, SchemaRepo schemas, Overrides overrides, Dictionary<string, CerealStruct> structs)
	{
		if (structs.TryGetValue(name, out CerealStruct existing)) return existing;

		overrides.Packets.TryGetValue(name, out PacketOverride structOverride);
		var result = new CerealStruct {Name = structOverride?.TypeName ?? SanitizeTypeName(name)};
		structs[name] = result;

		foreach (CerealField field in ResolveFields(name, schema, schemas, overrides, structs, structOverride))
		{
			AttachEnum(field, result.Name, result.Enums);
			result.Fields.Add(field);
		}

		return result;
	}

	/// <summary>
	///     Qualifies a field's (or array element's) enum against its owning class and registers the
	///     declaration, deduplicated by name so two fields sharing an enum schema emit it once.
	/// </summary>
	private static void AttachEnum(CerealField field, string ownerTypeName, List<CerealEnum> declarations)
	{
		foreach (CerealField carrier in new[] {field, field.Element})
		{
			if (carrier?.Enum == null) continue;
			carrier.EnumRef = $"{ownerTypeName}.{carrier.Enum.Name}";
			if (declarations.All(e => e.Name != carrier.Enum.Name)) declarations.Add(carrier.Enum);
		}
	}

	/// <summary>Schema titles are mostly PascalCase already; snake_case ones (server_config) get converted without disturbing existing casing.</summary>
	private static string SanitizeTypeName(string name)
	{
		// Nested schema titles carry their owner ("RequestAbilityPacketPayload::Type"). We emit the
		// type nested in that same owner, so only the last segment is the name.
		int nested = name.LastIndexOf("::", StringComparison.Ordinal);
		if (nested >= 0) name = name.Substring(nested + 2);

		// Hyphens join like underscores: "Aim-Assist_Target_Mode" is not a legal identifier as is.
		string joined = name.Contains('_') || name.Contains(' ') || name.Contains('-')
			? string.Concat(name.Split('_', ' ', '-').Where(p => p.Length > 0).Select(p => char.ToUpperInvariant(p[0]) + p.Substring(1)))
			: name;
		return char.ToUpperInvariant(joined[0]) + joined.Substring(1);
	}

	private static TypeMapping Primitive(string owner, string fieldName, string underlying, bool compressed)
	{
		switch (underlying)
		{
			case "boolean":
				return new TypeMapping {CsType = "bool", Write = "Write({0});", Read = "{0} = ReadBool();"};
			case "uint8":
				return new TypeMapping {CsType = "byte", Write = "Write({0});", Read = "{0} = ReadByte();"};
			case "float":
				return new TypeMapping {CsType = "float", Write = "Write({0});", Read = "{0} = ReadFloat();"};
			case "string":
				return new TypeMapping {CsType = "string", Write = "Write({0});", Read = "{0} = ReadString();"};
			case "int8":
				// One byte is one byte; Compression cannot shrink it and BDS confirms it stays raw
				// (Player Permissions in StartGame: 0x02 on the wire, not zigzag 0x04). Signed,
				// because subchunk offsets genuinely go negative.
				return new TypeMapping {CsType = "sbyte", Write = "Write((byte) {0});", Read = "{0} = (sbyte) ReadByte();"};
			case "int32" when compressed:
				return new TypeMapping {CsType = "int", Write = "WriteSignedVarInt({0});", Read = "{0} = ReadSignedVarInt();"};
			case "uint32" when compressed:
				return new TypeMapping {CsType = "uint", Write = "WriteUnsignedVarInt({0});", Read = "{0} = ReadUnsignedVarInt();"};
			case "int64" when compressed:
				return new TypeMapping {CsType = "long", Write = "WriteSignedVarLong({0});", Read = "{0} = ReadSignedVarLong();"};
			case "uint64" when compressed:
				return new TypeMapping {CsType = "long", Write = "WriteUnsignedVarLong({0});", Read = "{0} = ReadUnsignedVarLong();"};
			case "int32":
				// Write(int)/ReadInt() are little-endian by default, which is the Cereal raw int32.
				return new TypeMapping {CsType = "int", Write = "Write({0});", Read = "{0} = ReadInt();"};
			case "uint32":
				// Unsigned twin of int32, through the same little-endian pair.
				return new TypeMapping {CsType = "uint", Write = "Write((int) {0});", Read = "{0} = (uint) ReadInt();"};
			case "uint64":
				// Raw 64-bit is little-endian on the wire; Write(ulong)/ReadUlong() are the LE pair
				// (Write(long)/ReadLong() byte-swap, despite the name).
				return new TypeMapping {CsType = "ulong", Write = "Write({0});", Read = "{0} = ReadUlong();"};
			case "int64":
				// Signed twin of the above, through the same little-endian pair for the same reason.
				return new TypeMapping {CsType = "long", Write = "Write((ulong) {0});", Read = "{0} = (long) ReadUlong();"};
			case "int16":
				return new TypeMapping {CsType = "short", Write = "Write({0});", Read = "{0} = ReadShort();"};
			case "uint16":
				return new TypeMapping {CsType = "ushort", Write = "Write({0});", Read = "{0} = ReadUshort();"};
			default:
				throw new NotImplementedException($"{owner}.{fieldName}: primitive {underlying}{(compressed ? "+Compression" : "")} is not implemented yet");
		}
	}
}
