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

using System;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Entities;
using MiNET.Utils.Skins;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using TestPlugin.Code4Fun;

namespace MiNET.Test
{
	[TestClass]
	public class SkinTests
	{
		[TestMethod]
		public void ParseGeometryTest()
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			string json = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var fake = new PlayerMob(string.Empty, null);

			string newName = $"geometry.{DateTime.UtcNow.Ticks}.{new Random().NextDouble()}";
			json = json.Replace("geometry.humanoid.custom", newName);

			fake.Skin.GeometryName = newName;
			fake.Skin.GeometryData = json;

			GeometryModel geometryModel = JsonConvert.DeserializeObject<GeometryModel>(json, settings);

			var state = new Code4FunPlugin.GravityGeometryBehavior(fake, geometryModel);
			state.FakeMeltTicking(fake, new PlayerEventArgs(null));
		}

		[TestMethod]
		public void GeometryToJsonTest()
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			string json = File.ReadAllText(Path.Combine(pluginDirectory, "geometry.json"));

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			settings.Converters.Add(new StringEnumConverter {NamingStrategy = new CamelCaseNamingStrategy()});

			GeometryModel geometryModel = JsonConvert.DeserializeObject<GeometryModel>(json, settings);

			var jsonOut = JsonConvert.SerializeObject(geometryModel.Clone(), Formatting.Indented, settings);

			string fileName = Path.GetTempPath() + "Geometry_" + Guid.NewGuid() + ".json";
			File.WriteAllText(fileName, jsonOut);
		}
	}
}