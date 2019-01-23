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
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MiNET.Utils.Skins
{
	public enum BoneName
	{
		Unknown,
		Body,
		Waist,
		Head,
		Hat,
		LeftArm,
		RightArm,
		LeftLeg,
		RightLeg,
		Cape,
		LeftItem,
		RightItem,
		LeftSleeve,
		RightSleeve,
		LeftPants,
		RightPants,
		Jacket
	}

	public class Bone : ICloneable
	{
		public BoneName Name { get; set; }

		[JsonProperty(PropertyName = "META_BoneType")]
		public string BoneType { get; set; }

		public string Material { get; set; }

		public string Parent { get; set; }
		public float[] Pivot { get; set; } = new float[3];
		public float[] Pos { get; set; } = new float[3];
		public float[] Rotation { get; set; } = new float[3];
		public List<Cube> Cubes { get; set; }
		public bool NeverRender { get; set; }
		public bool Reset { get; set; }
		public bool Mirror { get; set; }

		public object Clone()
		{
			var bone = (Bone) MemberwiseClone();

			bone.Pivot = (float[]) Pivot?.Clone();
			bone.Pos = (float[]) Pos?.Clone();
			bone.Rotation = (float[]) Rotation?.Clone();

			if (Cubes != null)
			{
				bone.Cubes = new List<Cube>();
				foreach (var cube in Cubes)
				{
					bone.Cubes.Add((Cube) cube.Clone());
				}
			}

			return bone;
		}
	}
}