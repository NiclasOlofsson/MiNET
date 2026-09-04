using System.Collections.Generic;

namespace MiNET.Net
{
	/// <summary>
	///     The experiment toggles a 1.26.50 vanilla server declares in StartGame. Server
	///     configuration, not extracted data: the client turns features on by these names, and
	///     <see cref="DataDrivenVanillaBlocksAndItems" /> is the one that makes it accept the block
	///     definitions StartGame carries.
	/// </summary>
	public static class KnownExperiments
	{
		public const string UpcomingCreatorFeatures = "upcoming_creator_features";
		public const string GameTest = "gametest";
		public const string ExperimentalCreatorCameras = "experimental_creator_cameras";
		public const string VillagerTradesRebalance = "villager_trades_rebalance";
		public const string VoxelShapes = "voxel_shapes";
		public const string DataDrivenVanillaBlocksAndItems = "data_driven_vanilla_blocks_and_items";

		/// <summary>The six, in the order vanilla sends them, all enabled.</summary>
		public static readonly string[] Vanilla =
		[
			UpcomingCreatorFeatures, GameTest, ExperimentalCreatorCameras, VillagerTradesRebalance, VoxelShapes, DataDrivenVanillaBlocksAndItems
		];
	}

	public class Experiments : List<Experiments.Experiment>
	{
		/// <summary>Whether any experiments have ever been toggled in this world. Part of the wire type (trailing bool after the toggle list).</summary>
		public bool ExperimentsEverToggled { get; set; }

		/// <summary>What a vanilla 1.26.50 server sends: the six known experiments on, and the toggled bit set.</summary>
		public static Experiments Vanilla()
		{
			var experiments = new Experiments {ExperimentsEverToggled = true};
			foreach (string name in KnownExperiments.Vanilla) experiments.Add(new Experiment(name, true));
			return experiments;
		}

		public class Experiment
		{
			public string Name { get; }
			public bool Enabled { get; }

			public Experiment(string name, bool enabled)
			{
				Name = name;
				Enabled = enabled;
			}
		}
	}
}