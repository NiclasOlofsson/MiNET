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
using System.Collections.Generic;
using System.Numerics;
using log4net;
using MiNET.Utils;

namespace MiNET.Plotter
{
	public class PlotManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlotManager));

		Dictionary<PlotCoordinates, Plot> _plots = new Dictionary<PlotCoordinates, Plot>();

		public PlotManager()
		{
		}

		public bool HasClaim(PlotCoordinates coords, Player player)
		{
			if (player == null) return false;

			Plot plot = null;
			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot != null && Equals(plot.Owner, player.ClientUuid)) return true;

			return false;
		}

		public bool TryGetPlot(PlotCoordinates coords, Player player, out Plot plot)
		{
			plot = null;
			if (!_plots.ContainsKey(coords))
			{
				return false;
			}

			plot = _plots[coords];

			return true;
		}

		public bool TryClaim(PlotCoordinates coords, Player player, out Plot plot)
		{
			plot = null;
			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot != null) return false;

			plot = new Plot
			{
				Coordinates = coords,
				Owner = player.ClientUuid
			};
			_plots.Add(coords, plot);


			var offset = ConvertToBlockCoordinates(coords);
			int xOffset = offset.X;
			int zOffset = offset.Z;

			//player.Level.SetBlock(new GoldBlock {Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset)});
			//player.Level.SetBlock(new EmeraldBlock
			//{
			//	Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset) + new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 1, PlotWorldGenerator.PlotDepth - 1)
			//});


			Vector3 to = (Vector3) offset + (Vector3) new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 256, PlotWorldGenerator.PlotDepth - 1);
			BoundingBox bbox = new BoundingBox(offset, to).GetAdjustedBoundingBox();



			for (int x = (int) bbox.Min.X; x <= (int) bbox.Max.X; x++)
			{
				for (int z = (int) bbox.Min.Z; z <= (int) bbox.Max.Z; z++)
				{
					if (player.Level.IsTransparent(new BlockCoordinates(x, PlotWorldGenerator.PlotHeight, z)))
					{
						player.Level.SetAir(x, PlotWorldGenerator.PlotHeight, z);
					}
				}
			}

			return true;
		}

		public static BoundingBox GetBoundingBoxForPlot(PlotCoordinates coords)
		{
			var offset = ConvertToBlockCoordinates(coords);
			Vector3 to = (Vector3) offset + (Vector3) new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 256, PlotWorldGenerator.PlotDepth - 1);
			return new BoundingBox(offset, to).GetAdjustedBoundingBox();
		}

		//public static PlotCoordinates ConvertToPlotCoordinates(PlayerLocation location)
		//{
		//	return ConvertToPlotCoordinates((BlockCoordinates) location);
		//}

		//public static PlotCoordinates ConvertToPlotCoordinates(BlockCoordinates coords)
		//{
		//	if (PlotWorldGenerator.IsXRoad(coords.X, true) || PlotWorldGenerator.IsZRoad(coords.Z, true)) return null;

		//	int plotX = coords.X/PlotWorldGenerator.PlotAreaWidth + (Math.Sign(coords.X));
		//	int plotZ = coords.Z/PlotWorldGenerator.PlotAreaDepth + (Math.Sign(coords.Z));

		//	return new PlotCoordinates(plotX, plotZ);
		//}

		public static BlockCoordinates ConvertToBlockCoordinates(PlotCoordinates plotCoordinates)
		{
			var xFactor = plotCoordinates.X > 0 ? plotCoordinates.X - 1 : Math.Abs(plotCoordinates.X);
			var zFactor = plotCoordinates.Z > 0 ? plotCoordinates.Z - 1 : Math.Abs(plotCoordinates.Z);

			int xOffset = xFactor*PlotWorldGenerator.PlotAreaWidth;
			int zOffset = zFactor*PlotWorldGenerator.PlotAreaDepth;
			xOffset *= Math.Sign(plotCoordinates.X);
			zOffset *= Math.Sign(plotCoordinates.Z);
			/*if (xOffset < 0)*/
			xOffset += PlotWorldGenerator.RoadWidth;
			/*if (zOffset < 0) */
			zOffset += PlotWorldGenerator.RoadWidth;

			return new BlockCoordinates(xOffset, 0, zOffset);
		}
	}
}