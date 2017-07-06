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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class ResourceTest
	{
		[Test, Ignore("")]
		public void SerializeObjectModelRoundtripTest()
		{
			string commandJson = File.ReadAllText(@"D:\Downloads\Vanilla_Behavior_Pack_1.1.0\entities\bat.json");

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			//settings.Converters.Add(new CustomConverter());

			var entity = JsonConvert.DeserializeObject<MinecraftRoot>(commandJson, settings);

			//CommandSet toSerialize = new CommandSet();
			//toSerialize["difficulty"] = command;

			//var output = JsonConvert.SerializeObject(toSerialize, settings);
			//Console.WriteLine($"{output}");
		}
	}


	public class MinecraftRoot
	{
		[JsonProperty("minecraft:entity")]
		public MinecraftEntity MinecraftEntity { get; set; }
	}

	public class MinecraftEntity
	{
		[JsonProperty("format_version")]
		public string FormatVersion { get; set; }

		[JsonProperty("component_groups")]
		public Dictionary<string, ComponentGroup> ComponentGroups { get; set; }

		[JsonProperty("components")]
		[JsonConverter(typeof(ComponentConverter))]
		public IDictionary<string, Component> Components { get; set; }

		public Dictionary<string, Event> Events { get; set; }
	}

	public class ComponentConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Console.WriteLine($"Path: {reader.Path}, {reader.TokenType}");
			Console.WriteLine($"Token: {reader.Read()}, {reader.TokenType}");
			Console.WriteLine($"Token: {reader.Read()}, {reader.TokenType}");
			Console.WriteLine($"Token: {reader.Read()}, {reader.TokenType}");

			while (reader.TokenType != JsonToken.EndObject)
			{
				Console.WriteLine($"Token: {reader.Read()}, {reader.TokenType}");
				//Console.WriteLine($"Key: {reader.ReadAsString()}, {serializer.Deserialize(reader)}");
			}



			//IDictionary<string, object> result = (IDictionary<string, object>) serializer.Deserialize(reader, typeof (IDictionary<string, object>));

			//foreach (var kv in result)
			//{
			//	Console.WriteLine($"{kv.Key}, {kv.Value}");
			//	string typeName = $"";
			//}

			//return result;
			return new object();
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (Dictionary<string, Component>);
		}
	}

	public class Event
	{
	}

	public class Component
	{
		public string Id { get; set; }

		public string[] Family { get; set; }

		public int TotalSupply { get; set; }
		public int SuffocateTime { get; set; }
		//[JsonProperty("minecraft:breathable")]
		//public Breathable Breathable { get; set; }

		public double Width { get; set; }
		public double Height { get; set; }
		public double Value { get; set; }

		[JsonProperty("can_float")]
		public bool CanFloat { get; set; }

		[JsonProperty("xz_dist")]
		public double XzDist { get; set; }

		[JsonProperty("y_dist")]
		public double YDist { get; set; }

		[JsonProperty("y_offset")]
		public double YOffset { get; set; }

		public double Max { get; set; }
	}

	public class Breathable
	{
		public int TotalSupply { get; set; }
		public int SuffocateTime { get; set; }
	}

	public class ComponentGroup
	{
	}
}