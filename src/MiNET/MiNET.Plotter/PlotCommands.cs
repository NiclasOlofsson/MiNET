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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Utils;

namespace MiNET.Plotter
{
	public class PlotCommands
	{
		private readonly PlotManager _plotManager;

		public PlotCommands(PlotManager plotManager)
		{
			_plotManager = plotManager;
		}

		[Command(Name = "plot auto")]
		public VanillaCommands.SimpleResponse PlotAuto(Player player)
		{
			BlockCoordinates coords = (BlockCoordinates) player.KnownPosition;

			if (PlotWorldGenerator.IsXRoad(coords.X, true) || PlotWorldGenerator.IsZRoad(coords.Z, true)) return new VanillaCommands.SimpleResponse() {Body = "Not able to claim plot at this position."};

			int plotX = coords.X/PlotWorldGenerator.PlotAreaWidth + Math.Sign(coords.X);
			int plotZ = coords.Z/PlotWorldGenerator.PlotAreaDepth + Math.Sign(coords.Z);

			return new VanillaCommands.SimpleResponse($"Claimed plot {plotX}:{plotZ} at {coords}");
		}

		[Command(Name = "plot claim")]
		public VanillaCommands.SimpleResponse PlotClaim(Player player)
		{
			PlotCoordinates coords = _plotManager.ConvertToPlotCoordinates(player.KnownPosition);
			if (coords == null) return new VanillaCommands.SimpleResponse() {Body = "Not able to claim plot at this position."};

			Plot plot;
			if (!_plotManager.TryClaim(coords, player, out plot)) return new VanillaCommands.SimpleResponse() {Body = "Not able to claim plot at this position."};

			return new VanillaCommands.SimpleResponse($"Claimed plot {plot.Coordinates.X}:{plot.Coordinates.Z} at {coords}");
		}

		[Command(Name = "plot home")]
		public VanillaCommands.SimpleResponse PlotHome(Player player)
		{
			return new VanillaCommands.SimpleResponse("Not implemented");
		}

		[Command(Name = "plot tp")]
		public VanillaCommands.SimpleResponse PlotTeleport(Player player, int plotX = -1, int plotZ = -1)
		{
			int x = (Math.Abs(plotX) - 1)*PlotWorldGenerator.PlotAreaWidth + PlotWorldGenerator.PlotWidth/2;
			int z = (Math.Abs(plotZ) - 1)*PlotWorldGenerator.PlotAreaDepth + PlotWorldGenerator.PlotDepth/2;

			x *= Math.Sign(plotX);
			z *= Math.Sign(plotZ);

			BlockCoordinates coords = new BlockCoordinates(x, 0, z);
			coords.Y = player.Level.GetHeight(coords);
			player.Teleport(coords);

			return new VanillaCommands.SimpleResponse($"Teleported home to plot {plotX}:{plotZ} at {coords}");
		}
	}
}