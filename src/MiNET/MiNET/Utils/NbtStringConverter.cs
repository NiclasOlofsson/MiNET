using System;
using fNbt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.Utils
{
	public class NbtStringConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof (NbtString));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			return token.Value<string>();
		}
	}

	public class NbtIntConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof (NbtLong) || objectType == typeof (NbtInt) || objectType == typeof (NbtShort) || objectType == typeof (NbtByte);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			if (objectType == typeof (NbtLong))
			{
				return token.Value<long>();
			}

			if (objectType == typeof (NbtInt))
			{
				return token.Value<int>();
			}

			if (objectType == typeof (NbtShort))
			{
				return token.Value<short>();
			}

			if (objectType == typeof (NbtByte))
			{
				return token.Value<byte>();
			}

			return 0;
		}
	}
}