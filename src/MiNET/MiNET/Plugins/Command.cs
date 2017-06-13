using System.Collections.Generic;
using System.Reflection;
using MiNET.Entities;
using Newtonsoft.Json;

namespace MiNET.Plugins
{
	public class CommandSet : Dictionary<string, Command>
	{
	}

	public class Command
	{
		[JsonIgnore]
		public string Name { get; set; }

		public Version[] Versions { get; set; }
	}

	public class Version
	{
		[JsonProperty(propertyName: "version")]
		public int CommandVersion { get; set; }

		public string[] Aliases { get; set; }
		public string Description { get; set; }
		public string Permission { get; set; }
		public bool RequiresTellPerms { get; set; }
		public bool RequiresChatPerms { get; set; }
		public bool OutputToSpeech { get; set; }

		[JsonProperty(propertyName: "requires_edu")]
		public bool RequiresEdu { get; set; }

		[JsonProperty(propertyName: "allows_indirect_exec")]
		public bool AllowsIndirectExec { get; set; }

		[JsonProperty(propertyName: "is_hidden")]
		public bool IsHidden { get; set; }

		public Dictionary<string, Overload> Overloads { get; set; }
	}


	public class Overload
	{
		[JsonIgnore]
		public MethodInfo Method { get; set; }

		[JsonIgnore]
		public string Description { get; set; }

		public Input Input { get; set; }
		public Output Output { get; set; }
		public Parser Parser { get; set; }
	}

	public class Input
	{
		public Parameter[] Parameters { get; set; }
	}

	public class Output
	{
		[JsonProperty(propertyName: "format_strings")]
		public FormatString[] FormatStrings { get; set; }

		public Parameter[] Parameters { get; set; }
	}

	public class FormatString
	{
		public string Color { get; set; }
		public string Format { get; set; }

		[JsonProperty(propertyName: "params_to_use")]
		public string[] ParamsToUse { get; set; }

		[JsonProperty(propertyName: "should_show")]
		public FormatRule ShouldShow { get; set; }
	}

	public class FormatRule
	{
		[JsonProperty(propertyName: "not_empty")]
		public string[] NotEmpty { get; set; }
		[JsonProperty(propertyName: "is_true")]
		public string[] IsTrue { get; set; }
	}

	public class Parser
	{
		public string Tokens { get; set; }
	}

	public class Parameter
	{
		public string Name { get; set; }
		public string Type { get; set; }

		[JsonProperty(propertyName: "enum_type")]
		public string EnumType { get; set; }

		[JsonProperty(propertyName: "enum_values")]
		public string[] EnumValues { get; set; }

		public bool Optional { get; set; }

		[JsonProperty(propertyName: "target_data")]
		public TargetData TargetData { get; set; }
	}

	public class TargetData
	{
		[JsonProperty(propertyName: "players_only")]
		public bool PlayersOnly { get; set; }

		[JsonProperty(propertyName: "main_target")]
		public bool MainTarget { get; set; }

		[JsonProperty(propertyName: "allow_dead_players")]
		public bool AllowDeadPlayers { get; set; }
	}


	public class BlockPos
	{
		public int X { get; set; }
		public bool XRelative { get; set; }

		public int Y { get; set; }
		public bool YRelative { get; set; }

		public int Z { get; set; }
		public bool ZRelative { get; set; }

		public override string ToString()
		{
			return $"X: {X}, XRelative: {XRelative}, Y: {Y}, YRelative: {YRelative}, Z: {Z}, ZRelative: {ZRelative}";
		}
	}

	public class Target
	{
		public class Rule
		{
			public bool Inverted { get; set; }
			public string Name { get; set; }
			public string Value { get; set; }
		}

		public Rule[] Rules { get; set; }
		public string Selector { get; set; }

		public Player[] Players { get; set; }
		public Entity[] Entities { get; set; }

		public override string ToString()
		{
			string body = string.Empty;

			if (Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in Players)
				{
					names.Add(p.Username);
				}
				body = string.Join(", ", names);
			}

			return body;
		}
	}


	public abstract class EnumBase
	{
		public string Value { get; set; }
	}

	// enchantmentType
	public class EnchantmentTypeEnum : EnumBase
	{
	}

	// gameRuleTypes
	public class GameRuleEnum : EnumBase
	{
	}

	// dimension
	public class DimensionTypesEnum : EnumBase
	{
	}

	// itemType
	public class ItemTypeEnum : EnumBase
	{
	}

	// commandName
	public class CommandNameEnum : EnumBase
	{
	}

	// entityType
	public class EntityTypeEnum : EnumBase
	{
	}

	// blockType
	public class BlockTypeEnum : EnumBase
	{
	}


	//"rules": [
	//    {
	//    "inverted": false,
	//    "name": "name",
	//    "value": "gurunx"
	//	}
	//],
	//"selector": "nearestPlayer"
}