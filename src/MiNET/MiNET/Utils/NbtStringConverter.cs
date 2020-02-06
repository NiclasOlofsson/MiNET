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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

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
			return (objectType == typeof(NbtString));
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
			return objectType == typeof(NbtLong) || objectType == typeof(NbtInt) || objectType == typeof(NbtShort) || objectType == typeof(NbtByte) || objectType == typeof(NbtByteArray) || objectType == typeof(NbtFloat);
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			if (objectType == typeof(NbtLong))
			{
				return token.Value<long>();
			}

			if (objectType == typeof(NbtInt))
			{
				return token.Value<int>();
			}

			if (objectType == typeof(NbtShort))
			{
				return token.Value<short>();
			}

			if (objectType == typeof(NbtByte))
			{
				return token.Value<byte>();
			}

			if (objectType == typeof(NbtByteArray))
			{
				return token.Value<byte[]>();
			}

			if (objectType == typeof(NbtFloat))
			{
				return token.Value<float>();
			}

			return 0;
		}
	}
}