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
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace MiNET.Utils.Skins
{
	public class GeometryModel : ICloneable
	{
		[JsonProperty(PropertyName = "format_version")]
		public string FormatVersion { get; set; } = "1.12.0";

		[JsonProperty(PropertyName = "minecraft:geometry")]
		public List<Geometry> Geometry { get; set; } = new List<Geometry>();

		public Geometry FindGeometry(string geometryName, bool matchPartial = true)
		{
			string fullName = Geometry.FirstOrDefault(g => matchPartial ? 
				g.Description.Identifier.StartsWith(geometryName, StringComparison.InvariantCultureIgnoreCase) 
				: g.Description.Identifier.Equals(geometryName, StringComparison.InvariantCultureIgnoreCase))
				?.Description.Identifier;

			if (fullName == null) return null;

			Geometry geometry = Geometry.First(g => g.Description.Identifier == fullName);
			geometry.Name = fullName;

			//if (fullName.Contains(":"))
			//{
			//	geometry.BaseGeometry = fullName.Split(':')[1];
			//}

			return geometry;
		}

		public Geometry CollapseToDerived(Geometry derived)
		{
			if(derived == null) throw new ArgumentNullException(nameof(derived));

			return derived;

			/*var collapsed = (Geometry) derived.Clone();

			if (collapsed.BaseGeometry != null)
			{
				Geometry baseGeometry = (Geometry) FindGeometry(collapsed.BaseGeometry).Clone();

				foreach (var bone in baseGeometry.Bones)
				{
					if (collapsed.Bones.SingleOrDefault(b => b.Name == bone.Name) == null)
					{
						collapsed.Bones.Add(bone);
					}
				}
			}

			return collapsed;*/
		}

		public object Clone()
		{
			var model = (GeometryModel) MemberwiseClone();
			model.Geometry = new List<Geometry>();
			foreach (var records in Geometry)
			{
				model.Geometry.Add((Geometry) records.Clone());
			}

			return model;
		}
	}
}