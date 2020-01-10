using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.Utils
{
	public class IPAddressConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(IPAddress));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return IPAddress.Parse((string) reader.Value);
		}
	}

	public class IPEndPointConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof(IPEndPoint));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			var ep = (IPEndPoint) value;
			var jo = new JObject();
			if (ep.AddressFamily == AddressFamily.InterNetworkV6)
			{
				jo.Add("Address", JToken.FromObject(ep.Address.ScopeId, serializer));
			}
			else
			{
				jo.Add("Address", JToken.FromObject(ep.Address.ToString(), serializer));
			}
			jo.Add("Port", ep.Port);
			jo.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			var jo = JObject.Load(reader);
			IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
			int port = (int) jo["Port"];
			return new IPEndPoint(address, port);
		}
	}
}
