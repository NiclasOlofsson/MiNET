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
using System.Collections.Generic;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace MiNET.Plotter
{
	[Plugin(PluginName = "Plotter", Description = "Basic plot server plugin for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class PlotterPlugin : Plugin, IStartup
	{
		public void Configure(MiNetServer server)
		{
			server.LevelManager = new PlotterLevelManager();
		}

		protected override void OnEnable()
		{
			var server = Context.Server;

			PlotManager plotManager = new PlotManager();
			Context.PluginManager.LoadCommands(new PlotCommands(plotManager));
		}
	}

	public class PlotManager
	{
		Dictionary<PlotCoordinates, Plot> _plots = new Dictionary<PlotCoordinates, Plot>();

		public PlotManager()
		{
		}

		public bool TryClaim(PlotCoordinates coords, Player player, out Plot plot)
		{
			plot = null;
			if (_plots.ContainsKey(coords))
			{
				plot = _plots[coords];
			}

			if (plot != null) return false;

			plot = new Plot() {Coordinates = coords};
			_plots.Add(coords, plot);


			var offset = ConvertToBlockCoordinates(coords);
			int xOffset = offset.X;
			int zOffset = offset.Z;

			player.Level.SetBlock(new GoldBlock {Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset)});
			player.Level.SetBlock(new EmeraldBlock
			{
				Coordinates = new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight + 2, zOffset) + new BlockCoordinates(PlotWorldGenerator.PlotWidth - 1, 1, PlotWorldGenerator.PlotDepth - 1)
			});


			var to = offset + new BlockCoordinates(PlotWorldGenerator.PlotWidth, 0, PlotWorldGenerator.PlotDepth);
			BoundingBox bbox = BoundingBox.CreateFromPoints(new List<Vector3> {offset, to});

			for (int x = (int) bbox.Min.X; x < bbox.Max.X; x++)
			{
				for (int z = (int) bbox.Min.Z; z < bbox.Min.Z; z++)
				{
					player.Level.SetAir(x, PlotWorldGenerator.PlotHeight, z);
				}
			}

			return true;
		}

		public PlotCoordinates ConvertToPlotCoordinates(PlayerLocation location)
		{
			return ConvertToPlotCoordinates((BlockCoordinates) location);
		}

		public PlotCoordinates ConvertToPlotCoordinates(BlockCoordinates coords)
		{
			if (PlotWorldGenerator.IsXRoad(coords.X, true) || PlotWorldGenerator.IsZRoad(coords.Z, true)) return null;

			int plotX = coords.X/PlotWorldGenerator.PlotAreaWidth + (Math.Sign(coords.X));
			int plotZ = coords.Z/PlotWorldGenerator.PlotAreaDepth + (Math.Sign(coords.Z));

			return new PlotCoordinates(plotX, plotZ);
		}

		public BlockCoordinates ConvertToBlockCoordinates(PlotCoordinates plotCoordinates)
		{
			int xOffset = (Math.Abs(plotCoordinates.X) - 1)*PlotWorldGenerator.PlotAreaWidth + PlotWorldGenerator.RoadWidth;
			int zOffset = (Math.Abs(plotCoordinates.Z) - 1)*PlotWorldGenerator.PlotAreaDepth + PlotWorldGenerator.RoadWidth;
			xOffset *= Math.Sign(plotCoordinates.X);
			zOffset *= Math.Sign(plotCoordinates.Z);
			if (xOffset < 0) xOffset -= PlotWorldGenerator.PlotWidth;
			if (zOffset < 0) zOffset -= PlotWorldGenerator.PlotDepth;

			return new BlockCoordinates(xOffset, PlotWorldGenerator.PlotHeight, zOffset);
		}
	}

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
			return $"{nameof(X)}: {X}, {nameof(Z)}: {Z}";
		}
	}

	public class Plot
	{
		public PlotCoordinates Coordinates { get; set; }
		public Guid Owner { get; set; }
	}
}