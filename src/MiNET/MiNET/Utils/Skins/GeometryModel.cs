using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Net;

namespace MiNET.Utils.Skins
{
	public class GeometryModel : Dictionary<string, Geometry>, ICloneable
	{
		public Geometry FindGeometry(string geometryName, bool matchPartial = true)
		{
			string fullName = Keys.FirstOrDefault(m => matchPartial ? m.StartsWith(geometryName) : m.Equals(geometryName, StringComparison.InvariantCultureIgnoreCase));
			if (fullName == null) return null;

			var geometry = this[fullName];
			geometry.Name = fullName;

			if (fullName.Contains(":"))
			{
				geometry.BaseGeometry = fullName.Split(':')[1];
			}

			return geometry;
		}

		public Geometry CollapseToDerived(Geometry derived)
		{
			Geometry collapsed = (Geometry) derived.Clone();

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

			return collapsed;
		}

		public object Clone()
		{
			var model = (GeometryModel) MemberwiseClone();
			foreach (var records in this)
			{
				model.Add(records.Key, (Geometry) records.Value.Clone());
			}

			return model;
		}
	}
}