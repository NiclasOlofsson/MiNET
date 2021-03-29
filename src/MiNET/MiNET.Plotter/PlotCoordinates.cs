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
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Plotter
{
	public class PlotCoordinates
	{
		public int X { get; set; }
		public int Z { get; set; }


		public PlotCoordinates()
		{
		}

		public PlotCoordinates(int x, int z)
		{
			X = x;
			Z = z;
		}


		public static PlotCoordinates operator +(PlotCoordinates a, PlotCoordinates b)
		{
			return new PlotCoordinates(a.X + b.X, a.Z + b.Z);
		}

		public static explicit operator PlotCoordinates(PlayerLocation coords)
		{
			return (PlotCoordinates) (BlockCoordinates) coords;
		}

		public static explicit operator PlotCoordinates(BlockCoordinates coords)
		{
			if (PlotWorldGenerator.IsXRoad(coords.X, true) || PlotWorldGenerator.IsZRoad(coords.Z, true)) return null;

			int plotX = coords.X/PlotWorldGenerator.PlotAreaWidth + (Math.Sign(coords.X));
			int plotZ = coords.Z/PlotWorldGenerator.PlotAreaDepth + (Math.Sign(coords.Z));

			return new PlotCoordinates(plotX, plotZ);
		}

		protected bool Equals(PlotCoordinates other)
		{
			return X == other.X && Z == other.Z;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((PlotCoordinates) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (X*397) ^ Z;
			}
		}

		public override string ToString()
		{
			return $"{X},{Z}";
		}
	}
}