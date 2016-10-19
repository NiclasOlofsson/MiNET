using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiNET.Plugins
{
	public class Command
	{
		public List<Version> Versions { get; set; }
	}

	public class Version
	{
		public string Description { get; set; }
		public Dictionary<string, Overload> Overloads { get; set; }
	}


	public class Overload
	{
		//[JsonProperty(ItemConverterType = typeof(ParameterConverter))]
		public Dictionary<string, List<Parameter>> Input { get; set; }

		[JsonProperty(ItemConverterType = typeof(ParameterConverter))]
		public Dictionary<string, List<IParameter>> Output { get; set; }
	}


	public interface IParameter
	{
		//public string Name { get; set; }
		//public string Type { get; set; }
	}

	public class Parameter: IParameter
	{
		//public string Name { get; set; }
		//public string Type { get; set; }
	}

	public class ParameterConverter : CustomCreationConverter<Dictionary<string, List<IParameter>>>
	{
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			Console.WriteLine($"Converter read: {objectType}, {reader.ValueType}");
			return base.ReadJson(reader, objectType, existingValue, serializer);
		}

		public override bool CanConvert(Type objectType)
		{
			Console.WriteLine($"Can convert: {objectType}={base.CanConvert(objectType)}");
			return base.CanConvert(objectType);
		}

		public override Dictionary<string, List<IParameter>> Create(Type objectType)
		{
			Console.WriteLine($"{objectType}");
			return new Dictionary<string, List<IParameter>>();
		}
	}
}