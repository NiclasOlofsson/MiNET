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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.Plotter
{
	public class PlotConfig
	{
		public PlotPlayer[] PlotPlayers { get; set; } = new PlotPlayer[0];
		public Plot[] Plots { get; set; } = new Plot[0];
	}

	public class PlotPlayer
	{
		public DateTime CreateDate { get; set; } = DateTime.UtcNow;
		public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

		public UUID Xuid { get; set; }
		public string Username { get; set; }
		public PlayerLocation Home { get; set; }
		public PlayerLocation LastPosition { get; set; }
	}

	public class Plot
	{
		public DateTime CreateDate { get; set; } = DateTime.UtcNow;
		public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

		public PlotCoordinates Coordinates { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public UUID Owner { get; set; }
		public UUID[] AllowedPlayers { get; set; } = new UUID[0];

		public int Time { get; set; } = 5000;
		public int Downfall { get; set; }
	}

	public class UuidConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType)
		{
			return (objectType == typeof (UUID));
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			JToken t = JToken.FromObject(((UUID) value).ToString());
			t.WriteTo(writer);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			JToken token = JToken.Load(reader);
			return new UUID(token.Value<string>());
		}
	}
}