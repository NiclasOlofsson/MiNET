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
using System.Numerics;

namespace MiNET.Utils.Vectors
{
	public static class VectorHelpers
	{
		public static double GetYaw(this Vector3 vector)
		{
			return ToDegrees(Math.Atan2(vector.X, vector.Z));
		}

		public static double GetPitch(this Vector3 vector)
		{
			var distance = Math.Sqrt((vector.X * vector.X) + (vector.Z * vector.Z));
			return ToDegrees(Math.Atan2(vector.Y, distance));
		}

		public static double ToRadians(this float angle)
		{
			return (Math.PI / 180.0f) * angle;
		}

		public static double ToRadians(this double angle)
		{
			return (Math.PI / 180.0f) * angle;
		}

		public static double ToDegrees(this double angle)
		{
			return angle * (180.0f / Math.PI);
		}

		public static Vector3 Normalize(this Vector3 vec)
		{
			return Vector3.Normalize(vec);
		}
	}
}