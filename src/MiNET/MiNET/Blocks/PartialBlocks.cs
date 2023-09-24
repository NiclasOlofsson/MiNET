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
using MiNET.Utils;
// ReSharper disable SuggestVarOrType_SimpleTypes
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable UseObjectOrCollectionInitializer
// ReSharper disable RemoveRedundantBraces
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable PartialMethodWithSinglePart
// ReSharper disable MemberCanBePrivate.Global
#pragma warning disable 1522

namespace MiNET.Blocks
{

	public partial class AcaciaButton  // typeof=AcaciaButton
	{
		public override string Id { get; protected set; } = "minecraft:acacia_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaDoor  // typeof=AcaciaDoor
	{
		public override string Id { get; protected set; } = "minecraft:acacia_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaFence : Block // typeof=AcaciaFence
	{

		public AcaciaFence() : base("minecraft:acacia_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_fence";
			return record;
		} // method
	} // class

	public partial class AcaciaFenceGate  // typeof=AcaciaFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:acacia_fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaHangingSign : Block // typeof=AcaciaHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public AcaciaHangingSign() : base("minecraft:acacia_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class AcaciaLog : LogBase // typeof=AcaciaLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

		public AcaciaLog() : base("minecraft:acacia_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class AcaciaPressurePlate : Block // typeof=AcaciaPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public AcaciaPressurePlate() : base("minecraft:acacia_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class AcaciaStairs  // typeof=AcaciaStairs
	{
		public override string Id { get; protected set; } = "minecraft:acacia_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaStandingSign  // typeof=AcaciaStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:acacia_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaTrapdoor  // typeof=AcaciaTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:acacia_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaWallSign  // typeof=AcaciaWallSign
	{
		public override string Id { get; protected set; } = "minecraft:acacia_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:acacia_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ActivatorRail  // typeof=ActivatorRail
	{
		public override string Id { get; protected set; } = "minecraft:activator_rail";

		[StateBit] public bool RailDataBit { get; set; } = false;
		[StateRange(0, 5)] public int RailDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "rail_data_bit":
						RailDataBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "rail_direction":
						RailDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:activator_rail";
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class Air  // typeof=Air
	{
		public override string Id { get; protected set; } = "minecraft:air";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:air";
			return record;
		} // method
	} // class

	public partial class Allow : Block // typeof=Allow
	{

		public Allow() : base("minecraft:allow")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:allow";
			return record;
		} // method
	} // class

	public partial class AmethystBlock : Block // typeof=AmethystBlock
	{

		public AmethystBlock() : base("minecraft:amethyst_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:amethyst_block";
			return record;
		} // method
	} // class

	public partial class AmethystCluster : Block // typeof=AmethystCluster
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public AmethystCluster() : base("minecraft:amethyst_cluster")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:amethyst_cluster";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class AncientDebris : Block // typeof=AncientDebris
	{

		public AncientDebris() : base("minecraft:ancient_debris")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:ancient_debris";
			return record;
		} // method
	} // class

	public partial class AndesiteStairs  // typeof=AndesiteStairs
	{
		public override string Id { get; protected set; } = "minecraft:andesite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:andesite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Anvil  // typeof=Anvil
	{
		public override string Id { get; protected set; } = "minecraft:anvil";

		[StateEnum("undamaged", "slightly_damaged", "very_damaged", "broken")]
		public string Damage { get; set; } = "undamaged";
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "damage":
						Damage = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:anvil";
			record.States.Add(new BlockStateString { Name = "damage", Value = Damage });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Azalea : Block // typeof=Azalea
	{

		public Azalea() : base("minecraft:azalea")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:azalea";
			return record;
		} // method
	} // class

	public partial class AzaleaLeaves : Block // typeof=AzaleaLeaves
	{
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public AzaleaLeaves() : base("minecraft:azalea_leaves")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:azalea_leaves";
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class AzaleaLeavesFlowered : Block // typeof=AzaleaLeavesFlowered
	{
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public AzaleaLeavesFlowered() : base("minecraft:azalea_leaves_flowered")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:azalea_leaves_flowered";
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Bamboo : Block // typeof=Bamboo
	{
		[StateBit] public bool AgeBit { get; set; } = false;
		[StateEnum("small_leaves", "large_leaves", "no_leaves")]
		public string BambooLeafSize { get; set; } = "";
		[StateEnum("thin", "thick")]
		public string BambooStalkThickness { get; set; } = "";

		public Bamboo() : base("minecraft:bamboo")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "age_bit":
						AgeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "bamboo_leaf_size":
						BambooLeafSize = s.Value;
						break;
					case BlockStateString s when s.Name == "bamboo_stalk_thickness":
						BambooStalkThickness = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo";
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "bamboo_leaf_size", Value = BambooLeafSize });
			record.States.Add(new BlockStateString { Name = "bamboo_stalk_thickness", Value = BambooStalkThickness });
			return record;
		} // method
	} // class

	public partial class BambooBlock : Block // typeof=BambooBlock
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public BambooBlock() : base("minecraft:bamboo_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_block";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class BambooButton : Block // typeof=BambooButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public BambooButton() : base("minecraft:bamboo_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BambooDoor : Block // typeof=BambooDoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public BambooDoor() : base("minecraft:bamboo_door")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class BambooDoubleSlab : Block // typeof=BambooDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public BambooDoubleSlab() : base("minecraft:bamboo_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooFence : Block // typeof=BambooFence
	{

		public BambooFence() : base("minecraft:bamboo_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_fence";
			return record;
		} // method
	} // class

	public partial class BambooFenceGate : Block // typeof=BambooFenceGate
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public BambooFenceGate() : base("minecraft:bamboo_fence_gate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class BambooHangingSign : Block // typeof=BambooHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public BambooHangingSign() : base("minecraft:bamboo_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class BambooMosaic : Block // typeof=BambooMosaic
	{

		public BambooMosaic() : base("minecraft:bamboo_mosaic")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_mosaic";
			return record;
		} // method
	} // class

	public partial class BambooMosaicDoubleSlab : Block // typeof=BambooMosaicDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public BambooMosaicDoubleSlab() : base("minecraft:bamboo_mosaic_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_mosaic_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooMosaicSlab : Block // typeof=BambooMosaicSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public BambooMosaicSlab() : base("minecraft:bamboo_mosaic_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_mosaic_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooMosaicStairs : Block // typeof=BambooMosaicStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public BambooMosaicStairs() : base("minecraft:bamboo_mosaic_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_mosaic_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BambooPlanks : Block // typeof=BambooPlanks
	{

		public BambooPlanks() : base("minecraft:bamboo_planks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_planks";
			return record;
		} // method
	} // class

	public partial class BambooPressurePlate : Block // typeof=BambooPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public BambooPressurePlate() : base("minecraft:bamboo_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class BambooSapling : Block // typeof=BambooSapling
	{
		[StateBit] public bool AgeBit { get; set; } = false;
		[StateEnum("spruce", "birch", "jungle", "acacia", "dark_oak", "oak")]
		public string SaplingType { get; set; } = "";

		public BambooSapling() : base("minecraft:bamboo_sapling")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "age_bit":
						AgeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "sapling_type":
						SaplingType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_sapling";
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "sapling_type", Value = SaplingType });
			return record;
		} // method
	} // class

	public partial class BambooSlab : Block // typeof=BambooSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public BambooSlab() : base("minecraft:bamboo_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooStairs : Block // typeof=BambooStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public BambooStairs() : base("minecraft:bamboo_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BambooStandingSign : Block // typeof=BambooStandingSign
	{
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public BambooStandingSign() : base("minecraft:bamboo_standing_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class BambooTrapdoor : Block // typeof=BambooTrapdoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpsideDownBit { get; set; } = false;

		public BambooTrapdoor() : base("minecraft:bamboo_trapdoor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class BambooWallSign : Block // typeof=BambooWallSign
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public BambooWallSign() : base("minecraft:bamboo_wall_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bamboo_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Barrel : Block // typeof=Barrel
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool OpenBit { get; set; } = false;

		public Barrel() : base("minecraft:barrel")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:barrel";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class Barrier : Block // typeof=Barrier
	{

		public Barrier() : base("minecraft:barrier")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:barrier";
			return record;
		} // method
	} // class

	public partial class Basalt : Block // typeof=Basalt
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public Basalt() : base("minecraft:basalt")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:basalt";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Beacon  // typeof=Beacon
	{
		public override string Id { get; protected set; } = "minecraft:beacon";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:beacon";
			return record;
		} // method
	} // class

	public partial class Bed  // typeof=Bed
	{
		public override string Id { get; protected set; } = "minecraft:bed";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool HeadPieceBit { get; set; } = false;
		[StateBit] public bool OccupiedBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "head_piece_bit":
						HeadPieceBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "occupied_bit":
						OccupiedBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bed";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "head_piece_bit", Value = Convert.ToByte(HeadPieceBit) });
			record.States.Add(new BlockStateByte { Name = "occupied_bit", Value = Convert.ToByte(OccupiedBit) });
			return record;
		} // method
	} // class

	public partial class Bedrock  // typeof=Bedrock
	{
		public override string Id { get; protected set; } = "minecraft:bedrock";

		[StateBit] public bool InfiniburnBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "infiniburn_bit":
						InfiniburnBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bedrock";
			record.States.Add(new BlockStateByte { Name = "infiniburn_bit", Value = Convert.ToByte(InfiniburnBit) });
			return record;
		} // method
	} // class

	public partial class BeeNest : Block // typeof=BeeNest
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateRange(0, 5)] public int HoneyLevel { get; set; } = 0;

		public BeeNest() : base("minecraft:bee_nest")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateInt s when s.Name == "honey_level":
						HoneyLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bee_nest";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "honey_level", Value = HoneyLevel });
			return record;
		} // method
	} // class

	public partial class Beehive : Block // typeof=Beehive
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateRange(0, 5)] public int HoneyLevel { get; set; } = 0;

		public Beehive() : base("minecraft:beehive")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateInt s when s.Name == "honey_level":
						HoneyLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:beehive";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "honey_level", Value = HoneyLevel });
			return record;
		} // method
	} // class

	public partial class Beetroot  // typeof=Beetroot
	{
		public override string Id { get; protected set; } = "minecraft:beetroot";

		[StateRange(0, 7)] public override int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:beetroot";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Bell : Block // typeof=Bell
	{
		[StateEnum("standing", "hanging", "side", "multiple")]
		public string Attachment { get; set; } = "standing";
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool ToggleBit { get; set; } = false;

		public Bell() : base("minecraft:bell")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "attachment":
						Attachment = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "toggle_bit":
						ToggleBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bell";
			record.States.Add(new BlockStateString { Name = "attachment", Value = Attachment });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "toggle_bit", Value = Convert.ToByte(ToggleBit) });
			return record;
		} // method
	} // class

	public partial class BigDripleaf : Block // typeof=BigDripleaf
	{
		[StateBit] public bool BigDripleafHead { get; set; } = false;
		[StateEnum("none", "unstable", "partial_tilt", "full_tilt")]
		public string BigDripleafTilt { get; set; } = "none";
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public BigDripleaf() : base("minecraft:big_dripleaf")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "big_dripleaf_head":
						BigDripleafHead = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "big_dripleaf_tilt":
						BigDripleafTilt = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:big_dripleaf";
			record.States.Add(new BlockStateByte { Name = "big_dripleaf_head", Value = Convert.ToByte(BigDripleafHead) });
			record.States.Add(new BlockStateString { Name = "big_dripleaf_tilt", Value = BigDripleafTilt });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class BirchButton  // typeof=BirchButton
	{
		public override string Id { get; protected set; } = "minecraft:birch_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BirchDoor  // typeof=BirchDoor
	{
		public override string Id { get; protected set; } = "minecraft:birch_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class BirchFence : Block // typeof=BirchFence
	{

		public BirchFence() : base("minecraft:birch_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_fence";
			return record;
		} // method
	} // class

	public partial class BirchFenceGate  // typeof=BirchFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:birch_fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class BirchHangingSign : Block // typeof=BirchHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public BirchHangingSign() : base("minecraft:birch_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class BirchLog : LogBase // typeof=BirchLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

		public BirchLog() : base("minecraft:birch_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class BirchPressurePlate : Block // typeof=BirchPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public BirchPressurePlate() : base("minecraft:birch_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class BirchStairs  // typeof=BirchStairs
	{
		public override string Id { get; protected set; } = "minecraft:birch_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BirchStandingSign  // typeof=BirchStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:birch_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class BirchTrapdoor  // typeof=BirchTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:birch_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class BirchWallSign  // typeof=BirchWallSign
	{
		public override string Id { get; protected set; } = "minecraft:birch_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:birch_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlackCandle : Block // typeof=BlackCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public BlackCandle() : base("minecraft:black_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:black_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlackCandleCake : Block // typeof=BlackCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public BlackCandleCake() : base("minecraft:black_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:black_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlackGlazedTerracotta  // typeof=BlackGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:black_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:black_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlackWool : Block // typeof=BlackWool
	{

		public BlackWool() : base("minecraft:black_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:black_wool";
			return record;
		} // method
	} // class

	public partial class Blackstone : Block // typeof=Blackstone
	{

		public Blackstone() : base("minecraft:blackstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blackstone";
			return record;
		} // method
	} // class

	public partial class BlackstoneDoubleSlab  // typeof=Block
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_double_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blackstone_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BlackstoneSlab  // typeof=BlackstoneSlab
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blackstone_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BlackstoneStairs  // typeof=BlackstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blackstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BlackstoneWall : Block // typeof=BlackstoneWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public BlackstoneWall() : base("minecraft:blackstone_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blackstone_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class BlastFurnace  // typeof=BlastFurnace
	{
		public override string Id { get; protected set; } = "minecraft:blast_furnace";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blast_furnace";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlueCandle : Block // typeof=BlueCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public BlueCandle() : base("minecraft:blue_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blue_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlueCandleCake : Block // typeof=BlueCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public BlueCandleCake() : base("minecraft:blue_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blue_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlueGlazedTerracotta  // typeof=BlueGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:blue_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blue_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlueIce : Block // typeof=BlueIce
	{

		public BlueIce() : base("minecraft:blue_ice")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blue_ice";
			return record;
		} // method
	} // class

	public partial class BlueWool : Block // typeof=BlueWool
	{

		public BlueWool() : base("minecraft:blue_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:blue_wool";
			return record;
		} // method
	} // class

	public partial class BoneBlock : Block // typeof=BoneBlock
	{
		[StateRange(0, 3)] public int Deprecated { get; set; } = 0;
		[StateEnum("x", "z", "y")]
		public string PillarAxis { get; set; } = "";

		public BoneBlock() : base("minecraft:bone_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "deprecated":
						Deprecated = s.Value;
						break;
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bone_block";
			record.States.Add(new BlockStateInt { Name = "deprecated", Value = Deprecated });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Bookshelf  // typeof=Bookshelf
	{
		public override string Id { get; protected set; } = "minecraft:bookshelf";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bookshelf";
			return record;
		} // method
	} // class

	public partial class BorderBlock : Block // typeof=BorderBlock
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public BorderBlock() : base("minecraft:border_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:border_block";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class BrewingStand  // typeof=BrewingStand
	{
		public override string Id { get; protected set; } = "minecraft:brewing_stand";

		[StateBit] public bool BrewingStandSlotABit { get; set; } = false;
		[StateBit] public bool BrewingStandSlotBBit { get; set; } = false;
		[StateBit] public bool BrewingStandSlotCBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "brewing_stand_slot_a_bit":
						BrewingStandSlotABit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "brewing_stand_slot_b_bit":
						BrewingStandSlotBBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "brewing_stand_slot_c_bit":
						BrewingStandSlotCBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brewing_stand";
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_a_bit", Value = Convert.ToByte(BrewingStandSlotABit) });
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_b_bit", Value = Convert.ToByte(BrewingStandSlotBBit) });
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_c_bit", Value = Convert.ToByte(BrewingStandSlotCBit) });
			return record;
		} // method
	} // class

	public partial class BrickBlock  // typeof=BrickBlock
	{
		public override string Id { get; protected set; } = "minecraft:brick_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brick_block";
			return record;
		} // method
	} // class

	public partial class BrickStairs  // typeof=BrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BrownCandle : Block // typeof=BrownCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public BrownCandle() : base("minecraft:brown_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BrownCandleCake : Block // typeof=BrownCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public BrownCandleCake() : base("minecraft:brown_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BrownGlazedTerracotta  // typeof=BrownGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:brown_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BrownMushroom  // typeof=BrownMushroom
	{
		public override string Id { get; protected set; } = "minecraft:brown_mushroom";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_mushroom";
			return record;
		} // method
	} // class

	public partial class BrownMushroomBlock  // typeof=BrownMushroomBlock
	{
		public override string Id { get; protected set; } = "minecraft:brown_mushroom_block";

		[StateRange(0, 15)] public int HugeMushroomBits { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "huge_mushroom_bits":
						HugeMushroomBits = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_mushroom_block";
			record.States.Add(new BlockStateInt { Name = "huge_mushroom_bits", Value = HugeMushroomBits });
			return record;
		} // method
	} // class

	public partial class BrownWool : Block // typeof=BrownWool
	{

		public BrownWool() : base("minecraft:brown_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:brown_wool";
			return record;
		} // method
	} // class

	public partial class BubbleColumn : Block // typeof=BubbleColumn
	{
		[StateBit] public bool DragDown { get; set; } = false;

		public BubbleColumn() : base("minecraft:bubble_column")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "drag_down":
						DragDown = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:bubble_column";
			record.States.Add(new BlockStateByte { Name = "drag_down", Value = Convert.ToByte(DragDown) });
			return record;
		} // method
	} // class

	public partial class BuddingAmethyst : Block // typeof=BuddingAmethyst
	{

		public BuddingAmethyst() : base("minecraft:budding_amethyst")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:budding_amethyst";
			return record;
		} // method
	} // class

	public partial class Cactus  // typeof=Cactus
	{
		public override string Id { get; protected set; } = "minecraft:cactus";

		[StateRange(0, 15)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cactus";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class Cake  // typeof=Cake
	{
		public override string Id { get; protected set; } = "minecraft:cake";

		[StateRange(0, 6)] public int BiteCounter { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "bite_counter":
						BiteCounter = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cake";
			record.States.Add(new BlockStateInt { Name = "bite_counter", Value = BiteCounter });
			return record;
		} // method
	} // class

	public partial class Calcite : Block // typeof=Calcite
	{

		public Calcite() : base("minecraft:calcite")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:calcite";
			return record;
		} // method
	} // class

	public partial class CalibratedSculkSensor : Block // typeof=CalibratedSculkSensor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool PoweredBit { get; set; } = false;

		public CalibratedSculkSensor() : base("minecraft:calibrated_sculk_sensor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:calibrated_sculk_sensor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Camera : Block // typeof=Camera
	{

		public Camera() : base("minecraft:camera")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:camera";
			return record;
		} // method
	} // class

	public partial class Campfire : Block // typeof=Campfire
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool Extinguished { get; set; } = false;

		public Campfire() : base("minecraft:campfire")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "extinguished":
						Extinguished = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:campfire";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "extinguished", Value = Convert.ToByte(Extinguished) });
			return record;
		} // method
	} // class

	public partial class Candle : Block // typeof=Candle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public Candle() : base("minecraft:candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CandleCake : Block // typeof=CandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public CandleCake() : base("minecraft:candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class Carpet  // typeof=Carpet
	{
		public override string Id { get; protected set; } = "minecraft:carpet";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:carpet";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class Carrots  // typeof=Carrots
	{
		public override string Id { get; protected set; } = "minecraft:carrots";

		[StateRange(0, 7)] public override int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:carrots";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class CartographyTable : Block // typeof=CartographyTable
	{

		public CartographyTable() : base("minecraft:cartography_table")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cartography_table";
			return record;
		} // method
	} // class

	public partial class CarvedPumpkin : Block // typeof=CarvedPumpkin
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public CarvedPumpkin() : base("minecraft:carved_pumpkin")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:carved_pumpkin";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Cauldron  // typeof=Cauldron
	{
		public override string Id { get; protected set; } = "minecraft:cauldron";

		[StateEnum("water", "powder_snow", "lava")]
		public string CauldronLiquid { get; set; } = "water";
		[StateRange(0, 6)] public int FillLevel { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "cauldron_liquid":
						CauldronLiquid = s.Value;
						break;
					case BlockStateInt s when s.Name == "fill_level":
						FillLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cauldron";
			record.States.Add(new BlockStateString { Name = "cauldron_liquid", Value = CauldronLiquid });
			record.States.Add(new BlockStateInt { Name = "fill_level", Value = FillLevel });
			return record;
		} // method
	} // class

	public partial class CaveVines : Block // typeof=CaveVines
	{
		[StateRange(0, 25)] public int GrowingPlantAge { get; set; } = 0;

		public CaveVines() : base("minecraft:cave_vines")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growing_plant_age":
						GrowingPlantAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cave_vines";
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class CaveVinesBodyWithBerries : Block // typeof=CaveVinesBodyWithBerries
	{
		[StateRange(0, 25)] public int GrowingPlantAge { get; set; } = 0;

		public CaveVinesBodyWithBerries() : base("minecraft:cave_vines_body_with_berries")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growing_plant_age":
						GrowingPlantAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cave_vines_body_with_berries";
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class CaveVinesHeadWithBerries : Block // typeof=CaveVinesHeadWithBerries
	{
		[StateRange(0, 25)] public int GrowingPlantAge { get; set; } = 0;

		public CaveVinesHeadWithBerries() : base("minecraft:cave_vines_head_with_berries")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growing_plant_age":
						GrowingPlantAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cave_vines_head_with_berries";
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class Chain : Block // typeof=Chain
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public Chain() : base("minecraft:chain")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chain";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class ChainCommandBlock : Block // typeof=ChainCommandBlock
	{
		[StateBit] public bool ConditionalBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public ChainCommandBlock() : base("minecraft:chain_command_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "conditional_bit":
						ConditionalBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chain_command_block";
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ChemicalHeat : Block // typeof=ChemicalHeat
	{

		public ChemicalHeat() : base("minecraft:chemical_heat")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chemical_heat";
			return record;
		} // method
	} // class

	public partial class ChemistryTable : Block // typeof=ChemistryTable
	{
		[StateEnum("compound_creator", "material_reducer", "element_constructor", "lab_table")]
		public string ChemistryTableType { get; set; } = "compound_creator";
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public ChemistryTable() : base("minecraft:chemistry_table")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "chemistry_table_type":
						ChemistryTableType = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chemistry_table";
			record.States.Add(new BlockStateString { Name = "chemistry_table_type", Value = ChemistryTableType });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class CherryButton : Block // typeof=CherryButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public CherryButton() : base("minecraft:cherry_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CherryDoor : Block // typeof=CherryDoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public CherryDoor() : base("minecraft:cherry_door")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class CherryDoubleSlab : Block // typeof=CherryDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public CherryDoubleSlab() : base("minecraft:cherry_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CherryFence : Block // typeof=CherryFence
	{

		public CherryFence() : base("minecraft:cherry_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_fence";
			return record;
		} // method
	} // class

	public partial class CherryFenceGate : Block // typeof=CherryFenceGate
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public CherryFenceGate() : base("minecraft:cherry_fence_gate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class CherryHangingSign : Block // typeof=CherryHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public CherryHangingSign() : base("minecraft:cherry_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class CherryLeaves : Block // typeof=CherryLeaves
	{
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public CherryLeaves() : base("minecraft:cherry_leaves")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_leaves";
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class CherryLog : LogBase // typeof=CherryLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		public CherryLog() : base("minecraft:cherry_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CherryPlanks : Block // typeof=CherryPlanks
	{

		public CherryPlanks() : base("minecraft:cherry_planks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_planks";
			return record;
		} // method
	} // class

	public partial class CherryPressurePlate : Block // typeof=CherryPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public CherryPressurePlate() : base("minecraft:cherry_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class CherrySapling : Block // typeof=CherrySapling
	{
		[StateBit] public bool AgeBit { get; set; } = false;

		public CherrySapling() : base("minecraft:cherry_sapling")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "age_bit":
						AgeBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_sapling";
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			return record;
		} // method
	} // class

	public partial class CherrySlab : Block // typeof=CherrySlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public CherrySlab() : base("minecraft:cherry_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CherryStairs : Block // typeof=CherryStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public CherryStairs() : base("minecraft:cherry_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CherryStandingSign : Block // typeof=CherryStandingSign
	{
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public CherryStandingSign() : base("minecraft:cherry_standing_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class CherryTrapdoor : Block // typeof=CherryTrapdoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpsideDownBit { get; set; } = false;

		public CherryTrapdoor() : base("minecraft:cherry_trapdoor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class CherryWallSign : Block // typeof=CherryWallSign
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public CherryWallSign() : base("minecraft:cherry_wall_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CherryWood : Block // typeof=CherryWood
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";
		[StateBit] public bool StrippedBit { get; set; } = false;

		public CherryWood() : base("minecraft:cherry_wood")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
					case BlockStateByte s when s.Name == "stripped_bit":
						StrippedBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cherry_wood";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			return record;
		} // method
	} // class

	public partial class Chest  // typeof=Chest
	{
		public override string Id { get; protected set; } = "minecraft:chest";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chest";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ChiseledBookshelf : Block // typeof=ChiseledBookshelf
	{
		[StateRange(0, 63)] public int BooksStored { get; set; } = 0;
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public ChiseledBookshelf() : base("minecraft:chiseled_bookshelf")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "books_stored":
						BooksStored = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chiseled_bookshelf";
			record.States.Add(new BlockStateInt { Name = "books_stored", Value = BooksStored });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class ChiseledDeepslate : Block // typeof=ChiseledDeepslate
	{

		public ChiseledDeepslate() : base("minecraft:chiseled_deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chiseled_deepslate";
			return record;
		} // method
	} // class

	public partial class ChiseledNetherBricks : Block // typeof=ChiseledNetherBricks
	{

		public ChiseledNetherBricks() : base("minecraft:chiseled_nether_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chiseled_nether_bricks";
			return record;
		} // method
	} // class

	public partial class ChiseledPolishedBlackstone : Block // typeof=ChiseledPolishedBlackstone
	{

		public ChiseledPolishedBlackstone() : base("minecraft:chiseled_polished_blackstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chiseled_polished_blackstone";
			return record;
		} // method
	} // class

	public partial class ChorusFlower  // typeof=ChorusFlower
	{
		public override string Id { get; protected set; } = "minecraft:chorus_flower";

		[StateRange(0, 5)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chorus_flower";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class ChorusPlant  // typeof=ChorusPlant
	{
		public override string Id { get; protected set; } = "minecraft:chorus_plant";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:chorus_plant";
			return record;
		} // method
	} // class

	public partial class Clay  // typeof=Clay
	{
		public override string Id { get; protected set; } = "minecraft:clay";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:clay";
			return record;
		} // method
	} // class

	public partial class ClientRequestPlaceholderBlock : Block // typeof=ClientRequestPlaceholderBlock
	{

		public ClientRequestPlaceholderBlock() : base("minecraft:client_request_placeholder_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:client_request_placeholder_block";
			return record;
		} // method
	} // class

	public partial class CoalBlock  // typeof=CoalBlock
	{
		public override string Id { get; protected set; } = "minecraft:coal_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coal_block";
			return record;
		} // method
	} // class

	public partial class CoalOre  // typeof=CoalOre
	{
		public override string Id { get; protected set; } = "minecraft:coal_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coal_ore";
			return record;
		} // method
	} // class

	public partial class CobbledDeepslate : Block // typeof=CobbledDeepslate
	{

		public CobbledDeepslate() : base("minecraft:cobbled_deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobbled_deepslate";
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateDoubleSlab : Block // typeof=CobbledDeepslateDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public CobbledDeepslateDoubleSlab() : base("minecraft:cobbled_deepslate_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobbled_deepslate_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateSlab : Block // typeof=CobbledDeepslateSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public CobbledDeepslateSlab() : base("minecraft:cobbled_deepslate_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobbled_deepslate_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateStairs : Block // typeof=CobbledDeepslateStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public CobbledDeepslateStairs() : base("minecraft:cobbled_deepslate_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobbled_deepslate_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateWall : Block // typeof=CobbledDeepslateWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public CobbledDeepslateWall() : base("minecraft:cobbled_deepslate_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobbled_deepslate_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class Cobblestone  // typeof=Cobblestone
	{
		public override string Id { get; protected set; } = "minecraft:cobblestone";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobblestone";
			return record;
		} // method
	} // class

	public partial class CobblestoneWall  // typeof=CobblestoneWall
	{
		public override string Id { get; protected set; } = "minecraft:cobblestone_wall";

		[StateEnum("cobblestone", "mossy_cobblestone", "granite", "diorite", "andesite", "sandstone", "brick", "stone_brick", "mossy_stone_brick", "nether_brick", "end_brick", "prismarine", "red_sandstone", "red_nether_brick")]
		public string WallBlockType { get; set; } = "cobblestone";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = true;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_block_type":
						WallBlockType = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cobblestone_wall";
			record.States.Add(new BlockStateString { Name = "wall_block_type", Value = WallBlockType });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class Cocoa  // typeof=Cocoa
	{
		public override string Id { get; protected set; } = "minecraft:cocoa";

		[StateRange(0, 2)] public int Age { get; set; } = 0;
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cocoa";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class ColoredTorchBp : Block // typeof=ColoredTorchBp
	{
		[StateBit] public bool ColorBit { get; set; } = false;
		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

		public ColoredTorchBp() : base("minecraft:colored_torch_bp")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "color_bit":
						ColorBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:colored_torch_bp";
			record.States.Add(new BlockStateByte { Name = "color_bit", Value = Convert.ToByte(ColorBit) });
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class ColoredTorchRg : Block // typeof=ColoredTorchRg
	{
		[StateBit] public bool ColorBit { get; set; } = false;
		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

		public ColoredTorchRg() : base("minecraft:colored_torch_rg")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "color_bit":
						ColorBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:colored_torch_rg";
			record.States.Add(new BlockStateByte { Name = "color_bit", Value = Convert.ToByte(ColorBit) });
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class CommandBlock : Block // typeof=CommandBlock
	{
		[StateBit] public bool ConditionalBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public CommandBlock() : base("minecraft:command_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "conditional_bit":
						ConditionalBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:command_block";
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Composter : Block // typeof=Composter
	{
		[StateRange(0, 8)] public int ComposterFillLevel { get; set; } = 0;

		public Composter() : base("minecraft:composter")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "composter_fill_level":
						ComposterFillLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:composter";
			record.States.Add(new BlockStateInt { Name = "composter_fill_level", Value = ComposterFillLevel });
			return record;
		} // method
	} // class

	public partial class Concrete  // typeof=Concrete
	{
		public override string Id { get; protected set; } = "minecraft:concrete";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:concrete";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class ConcretePowder  // typeof=ConcretePowder
	{
		public override string Id { get; protected set; } = "minecraft:concrete_powder";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:concrete_powder";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class Conduit : Block // typeof=Conduit
	{

		public Conduit() : base("minecraft:conduit")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:conduit";
			return record;
		} // method
	} // class

	public partial class CopperBlock : Block // typeof=CopperBlock
	{

		public CopperBlock() : base("minecraft:copper_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:copper_block";
			return record;
		} // method
	} // class

	public partial class CopperOre : Block // typeof=CopperOre
	{

		public CopperOre() : base("minecraft:copper_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:copper_ore";
			return record;
		} // method
	} // class

	public partial class Coral : Block // typeof=Coral
	{
		[StateEnum("blue", "pink", "purple", "red", "yellow")]
		public string CoralColor { get; set; } = "blue";
		[StateBit] public bool DeadBit { get; set; } = true;

		public Coral() : base("minecraft:coral")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "coral_color":
						CoralColor = s.Value;
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral";
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralBlock : Block // typeof=CoralBlock
	{
		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";
		[StateBit] public bool DeadBit { get; set; } = false;

		public CoralBlock() : base("minecraft:coral_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "coral_color":
						CoralColor = s.Value;
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_block";
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFan : Block // typeof=CoralFan
	{
		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";
		[StateRange(0, 1)] public int CoralFanDirection { get; set; } = 0;

		public CoralFan() : base("minecraft:coral_fan")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "coral_color":
						CoralColor = s.Value;
						break;
					case BlockStateInt s when s.Name == "coral_fan_direction":
						CoralFanDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_fan";
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateInt { Name = "coral_fan_direction", Value = CoralFanDirection });
			return record;
		} // method
	} // class

	public partial class CoralFanDead : Block // typeof=CoralFanDead
	{
		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";
		[StateRange(0, 1)] public int CoralFanDirection { get; set; } = 0;

		public CoralFanDead() : base("minecraft:coral_fan_dead")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "coral_color":
						CoralColor = s.Value;
						break;
					case BlockStateInt s when s.Name == "coral_fan_direction":
						CoralFanDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_fan_dead";
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateInt { Name = "coral_fan_direction", Value = CoralFanDirection });
			return record;
		} // method
	} // class

	public partial class CoralFanHang : Block // typeof=CoralFanHang
	{
		[StateRange(0, 3)] public int CoralDirection { get; set; } = 0;
		[StateBit] public bool CoralHangTypeBit { get; set; } = false;
		[StateBit] public bool DeadBit { get; set; } = false;

		public CoralFanHang() : base("minecraft:coral_fan_hang")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "coral_direction":
						CoralDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "coral_hang_type_bit":
						CoralHangTypeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_fan_hang";
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFanHang2 : Block // typeof=CoralFanHang2
	{
		[StateRange(0, 3)] public int CoralDirection { get; set; } = 0;
		[StateBit] public bool CoralHangTypeBit { get; set; } = false;
		[StateBit] public bool DeadBit { get; set; } = false;

		public CoralFanHang2() : base("minecraft:coral_fan_hang2")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "coral_direction":
						CoralDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "coral_hang_type_bit":
						CoralHangTypeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_fan_hang2";
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFanHang3 : Block // typeof=CoralFanHang3
	{
		[StateRange(0, 3)] public int CoralDirection { get; set; } = 0;
		[StateBit] public bool CoralHangTypeBit { get; set; } = false;
		[StateBit] public bool DeadBit { get; set; } = false;

		public CoralFanHang3() : base("minecraft:coral_fan_hang3")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "coral_direction":
						CoralDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "coral_hang_type_bit":
						CoralHangTypeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:coral_fan_hang3";
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CrackedDeepslateBricks : Block // typeof=CrackedDeepslateBricks
	{

		public CrackedDeepslateBricks() : base("minecraft:cracked_deepslate_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cracked_deepslate_bricks";
			return record;
		} // method
	} // class

	public partial class CrackedDeepslateTiles : Block // typeof=CrackedDeepslateTiles
	{

		public CrackedDeepslateTiles() : base("minecraft:cracked_deepslate_tiles")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cracked_deepslate_tiles";
			return record;
		} // method
	} // class

	public partial class CrackedNetherBricks : Block // typeof=CrackedNetherBricks
	{

		public CrackedNetherBricks() : base("minecraft:cracked_nether_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cracked_nether_bricks";
			return record;
		} // method
	} // class

	public partial class CrackedPolishedBlackstoneBricks : Block // typeof=CrackedPolishedBlackstoneBricks
	{

		public CrackedPolishedBlackstoneBricks() : base("minecraft:cracked_polished_blackstone_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cracked_polished_blackstone_bricks";
			return record;
		} // method
	} // class

	public partial class CraftingTable  // typeof=CraftingTable
	{
		public override string Id { get; protected set; } = "minecraft:crafting_table";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crafting_table";
			return record;
		} // method
	} // class

	public partial class CrimsonButton : Block // typeof=CrimsonButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public CrimsonButton() : base("minecraft:crimson_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonDoor : Block // typeof=CrimsonDoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public CrimsonDoor() : base("minecraft:crimson_door")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonDoubleSlab  // typeof=CrimsonDoubleSlab
	{
		public override string Id { get; protected set; } = "minecraft:crimson_double_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonFence : Block // typeof=CrimsonFence
	{

		public CrimsonFence() : base("minecraft:crimson_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_fence";
			return record;
		} // method
	} // class

	public partial class CrimsonFenceGate : Block // typeof=CrimsonFenceGate
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public CrimsonFenceGate() : base("minecraft:crimson_fence_gate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonFungus : Block // typeof=CrimsonFungus
	{

		public CrimsonFungus() : base("minecraft:crimson_fungus")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_fungus";
			return record;
		} // method
	} // class

	public partial class CrimsonHangingSign : Block // typeof=CrimsonHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public CrimsonHangingSign() : base("minecraft:crimson_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class CrimsonHyphae : Block // typeof=CrimsonHyphae
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public CrimsonHyphae() : base("minecraft:crimson_hyphae")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_hyphae";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CrimsonNylium : Block // typeof=CrimsonNylium
	{

		public CrimsonNylium() : base("minecraft:crimson_nylium")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_nylium";
			return record;
		} // method
	} // class

	public partial class CrimsonPlanks : Block // typeof=CrimsonPlanks
	{

		public CrimsonPlanks() : base("minecraft:crimson_planks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_planks";
			return record;
		} // method
	} // class

	public partial class CrimsonPressurePlate : Block // typeof=CrimsonPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public CrimsonPressurePlate() : base("minecraft:crimson_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class CrimsonRoots : Block // typeof=CrimsonRoots
	{

		public CrimsonRoots() : base("minecraft:crimson_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_roots";
			return record;
		} // method
	} // class

	public partial class CrimsonSlab  // typeof=CrimsonSlab
	{
		public override string Id { get; protected set; } = "minecraft:crimson_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonStairs  // typeof=CrimsonStairs
	{
		public override string Id { get; protected set; } = "minecraft:crimson_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonStandingSign  // typeof=CrimsonStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:crimson_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonStem : Block // typeof=CrimsonStem
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public CrimsonStem() : base("minecraft:crimson_stem")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_stem";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CrimsonTrapdoor  // typeof=CrimsonTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:crimson_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonWallSign  // typeof=CrimsonWallSign
	{
		public override string Id { get; protected set; } = "minecraft:crimson_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crimson_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CryingObsidian : Block // typeof=CryingObsidian
	{

		public CryingObsidian() : base("minecraft:crying_obsidian")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:crying_obsidian";
			return record;
		} // method
	} // class

	public partial class CutCopper : Block // typeof=CutCopper
	{

		public CutCopper() : base("minecraft:cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cut_copper";
			return record;
		} // method
	} // class

	public partial class CutCopperSlab : Block // typeof=CutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public CutCopperSlab() : base("minecraft:cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CutCopperStairs : Block // typeof=CutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public CutCopperStairs() : base("minecraft:cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CyanCandle : Block // typeof=CyanCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public CyanCandle() : base("minecraft:cyan_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cyan_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CyanCandleCake : Block // typeof=CyanCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public CyanCandleCake() : base("minecraft:cyan_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cyan_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CyanGlazedTerracotta  // typeof=CyanGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:cyan_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cyan_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CyanWool : Block // typeof=CyanWool
	{

		public CyanWool() : base("minecraft:cyan_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:cyan_wool";
			return record;
		} // method
	} // class

	public partial class DarkOakButton  // typeof=DarkOakButton
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class DarkOakDoor  // typeof=DarkOakDoor
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class DarkOakFence : Block // typeof=DarkOakFence
	{

		public DarkOakFence() : base("minecraft:dark_oak_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_fence";
			return record;
		} // method
	} // class

	public partial class DarkOakFenceGate  // typeof=DarkOakFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class DarkOakHangingSign : Block // typeof=DarkOakHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public DarkOakHangingSign() : base("minecraft:dark_oak_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class DarkOakLog : LogBase // typeof=DarkOakLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

		public DarkOakLog() : base("minecraft:dark_oak_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class DarkOakPressurePlate : Block // typeof=DarkOakPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public DarkOakPressurePlate() : base("minecraft:dark_oak_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class DarkOakStairs  // typeof=DarkOakStairs
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DarkOakTrapdoor  // typeof=DarkOakTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_oak_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class DarkPrismarineStairs  // typeof=DarkPrismarineStairs
	{
		public override string Id { get; protected set; } = "minecraft:dark_prismarine_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dark_prismarine_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DarkoakStandingSign  // typeof=DarkoakStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:darkoak_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:darkoak_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class DarkoakWallSign  // typeof=DarkoakWallSign
	{
		public override string Id { get; protected set; } = "minecraft:darkoak_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:darkoak_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class DaylightDetector  // typeof=DaylightDetector
	{
		public override string Id { get; protected set; } = "minecraft:daylight_detector";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:daylight_detector";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class DaylightDetectorInverted  // typeof=DaylightDetectorInverted
	{
		public override string Id { get; protected set; } = "minecraft:daylight_detector_inverted";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:daylight_detector_inverted";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class Deadbush  // typeof=Deadbush
	{
		public override string Id { get; protected set; } = "minecraft:deadbush";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deadbush";
			return record;
		} // method
	} // class

	public partial class DecoratedPot : Block // typeof=DecoratedPot
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public DecoratedPot() : base("minecraft:decorated_pot")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:decorated_pot";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Deepslate : Block // typeof=Deepslate
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public Deepslate() : base("minecraft:deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickDoubleSlab : Block // typeof=DeepslateBrickDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DeepslateBrickDoubleSlab() : base("minecraft:deepslate_brick_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_brick_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickSlab : Block // typeof=DeepslateBrickSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DeepslateBrickSlab() : base("minecraft:deepslate_brick_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_brick_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickStairs : Block // typeof=DeepslateBrickStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public DeepslateBrickStairs() : base("minecraft:deepslate_brick_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickWall : Block // typeof=DeepslateBrickWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public DeepslateBrickWall() : base("minecraft:deepslate_brick_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_brick_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBricks : Block // typeof=DeepslateBricks
	{

		public DeepslateBricks() : base("minecraft:deepslate_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_bricks";
			return record;
		} // method
	} // class

	public partial class DeepslateCoalOre : Block // typeof=DeepslateCoalOre
	{

		public DeepslateCoalOre() : base("minecraft:deepslate_coal_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_coal_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateCopperOre : Block // typeof=DeepslateCopperOre
	{

		public DeepslateCopperOre() : base("minecraft:deepslate_copper_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_copper_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateDiamondOre : Block // typeof=DeepslateDiamondOre
	{

		public DeepslateDiamondOre() : base("minecraft:deepslate_diamond_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_diamond_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateEmeraldOre : Block // typeof=DeepslateEmeraldOre
	{

		public DeepslateEmeraldOre() : base("minecraft:deepslate_emerald_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_emerald_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateGoldOre : Block // typeof=DeepslateGoldOre
	{

		public DeepslateGoldOre() : base("minecraft:deepslate_gold_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_gold_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateIronOre : Block // typeof=DeepslateIronOre
	{

		public DeepslateIronOre() : base("minecraft:deepslate_iron_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_iron_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateLapisOre : Block // typeof=DeepslateLapisOre
	{

		public DeepslateLapisOre() : base("minecraft:deepslate_lapis_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_lapis_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateRedstoneOre : Block // typeof=DeepslateRedstoneOre
	{

		public DeepslateRedstoneOre() : base("minecraft:deepslate_redstone_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_redstone_ore";
			return record;
		} // method
	} // class

	public partial class DeepslateTileDoubleSlab : Block // typeof=DeepslateTileDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DeepslateTileDoubleSlab() : base("minecraft:deepslate_tile_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_tile_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTileSlab : Block // typeof=DeepslateTileSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DeepslateTileSlab() : base("minecraft:deepslate_tile_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_tile_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTileStairs : Block // typeof=DeepslateTileStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public DeepslateTileStairs() : base("minecraft:deepslate_tile_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_tile_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DeepslateTileWall : Block // typeof=DeepslateTileWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public DeepslateTileWall() : base("minecraft:deepslate_tile_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_tile_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTiles : Block // typeof=DeepslateTiles
	{

		public DeepslateTiles() : base("minecraft:deepslate_tiles")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deepslate_tiles";
			return record;
		} // method
	} // class

	public partial class Deny : Block // typeof=Deny
	{

		public Deny() : base("minecraft:deny")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:deny";
			return record;
		} // method
	} // class

	public partial class DetectorRail  // typeof=DetectorRail
	{
		public override string Id { get; protected set; } = "minecraft:detector_rail";

		[StateBit] public bool RailDataBit { get; set; } = false;
		[StateRange(0, 5)] public int RailDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "rail_data_bit":
						RailDataBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "rail_direction":
						RailDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:detector_rail";
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class DiamondBlock  // typeof=DiamondBlock
	{
		public override string Id { get; protected set; } = "minecraft:diamond_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:diamond_block";
			return record;
		} // method
	} // class

	public partial class DiamondOre  // typeof=DiamondOre
	{
		public override string Id { get; protected set; } = "minecraft:diamond_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:diamond_ore";
			return record;
		} // method
	} // class

	public partial class DioriteStairs  // typeof=DioriteStairs
	{
		public override string Id { get; protected set; } = "minecraft:diorite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:diorite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Dirt  // typeof=Dirt
	{
		public override string Id { get; protected set; } = "minecraft:dirt";

		[StateEnum("normal", "coarse")]
		public string DirtType { get; set; } = "normal";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "dirt_type":
						DirtType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dirt";
			record.States.Add(new BlockStateString { Name = "dirt_type", Value = DirtType });
			return record;
		} // method
	} // class

	public partial class DirtWithRoots : Block // typeof=DirtWithRoots
	{

		public DirtWithRoots() : base("minecraft:dirt_with_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dirt_with_roots";
			return record;
		} // method
	} // class

	public partial class Dispenser  // typeof=Dispenser
	{
		public override string Id { get; protected set; } = "minecraft:dispenser";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool TriggeredBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "triggered_bit":
						TriggeredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dispenser";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit) });
			return record;
		} // method
	} // class

	public partial class DoubleCutCopperSlab : Block // typeof=DoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DoubleCutCopperSlab() : base("minecraft:double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoublePlant  // typeof=DoublePlant
	{
		public override string Id { get; protected set; } = "minecraft:double_plant";

		[StateEnum("syringa", "grass", "fern", "rose", "paeonia", "sunflower")]
		public string DoublePlantType { get; set; } = "";
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "double_plant_type":
						DoublePlantType = s.Value;
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_plant";
			record.States.Add(new BlockStateString { Name = "double_plant_type", Value = DoublePlantType });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab : Block // typeof=DoubleStoneBlockSlab
	{
		[StateEnum("smooth_stone", "sandstone", "wood", "cobblestone", "brick", "stone_brick", "quartz", "nether_brick")]
		public string StoneSlabType { get; set; } = "smooth_stone";
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DoubleStoneBlockSlab() : base("minecraft:double_stone_block_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type":
						StoneSlabType = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_stone_block_slab";
			record.States.Add(new BlockStateString { Name = "stone_slab_type", Value = StoneSlabType });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab2 : Block // typeof=DoubleStoneBlockSlab2
	{
		[StateEnum("red_sandstone", "purpur", "prismarine_rough", "prismarine_dark", "prismarine_brick", "mossy_cobblestone", "smooth_sandstone", "red_nether_brick")]
		public string StoneSlabType2 { get; set; } = "red_sandstone";
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DoubleStoneBlockSlab2() : base("minecraft:double_stone_block_slab2")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_2":
						StoneSlabType2 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_stone_block_slab2";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_2", Value = StoneSlabType2 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab3 : Block // typeof=DoubleStoneBlockSlab3
	{
		[StateEnum("end_stone_brick", "smooth_red_sandstone", "polished_andesite", "andesite", "diorite", "polished_diorite", "granite", "polished_granite")]
		public string StoneSlabType3 { get; set; } = "end_stone_brick";
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DoubleStoneBlockSlab3() : base("minecraft:double_stone_block_slab3")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_3":
						StoneSlabType3 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_stone_block_slab3";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_3", Value = StoneSlabType3 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab4 : Block // typeof=DoubleStoneBlockSlab4
	{
		[StateEnum("smooth_quartz", "stone", "cut_sandstone", "cut_red_sandstone", "mossy_stone_brick")]
		public string StoneSlabType4 { get; set; } = "";
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public DoubleStoneBlockSlab4() : base("minecraft:double_stone_block_slab4")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_4":
						StoneSlabType4 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_stone_block_slab4";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_4", Value = StoneSlabType4 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleWoodenSlab  // typeof=DoubleWoodenSlab
	{
		public override string Id { get; protected set; } = "minecraft:double_wooden_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;
		[StateEnum("spruce", "birch", "jungle", "acacia", "dark_oak", "oak")]
		public string WoodType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "wood_type":
						WoodType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:double_wooden_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class DragonEgg  // typeof=DragonEgg
	{
		public override string Id { get; protected set; } = "minecraft:dragon_egg";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dragon_egg";
			return record;
		} // method
	} // class

	public partial class DriedKelpBlock : Block // typeof=DriedKelpBlock
	{

		public DriedKelpBlock() : base("minecraft:dried_kelp_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dried_kelp_block";
			return record;
		} // method
	} // class

	public partial class DripstoneBlock : Block // typeof=DripstoneBlock
	{

		public DripstoneBlock() : base("minecraft:dripstone_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dripstone_block";
			return record;
		} // method
	} // class

	public partial class Dropper  // typeof=Dropper
	{
		public override string Id { get; protected set; } = "minecraft:dropper";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool TriggeredBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "triggered_bit":
						TriggeredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:dropper";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit) });
			return record;
		} // method
	} // class

	public partial class Element0 : Block // typeof=Element0
	{

		public Element0() : base("minecraft:element_0")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_0";
			return record;
		} // method
	} // class

	public partial class Element1 : Block // typeof=Element1
	{

		public Element1() : base("minecraft:element_1")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_1";
			return record;
		} // method
	} // class

	public partial class Element10 : Block // typeof=Element10
	{

		public Element10() : base("minecraft:element_10")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_10";
			return record;
		} // method
	} // class

	public partial class Element100 : Block // typeof=Element100
	{

		public Element100() : base("minecraft:element_100")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_100";
			return record;
		} // method
	} // class

	public partial class Element101 : Block // typeof=Element101
	{

		public Element101() : base("minecraft:element_101")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_101";
			return record;
		} // method
	} // class

	public partial class Element102 : Block // typeof=Element102
	{

		public Element102() : base("minecraft:element_102")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_102";
			return record;
		} // method
	} // class

	public partial class Element103 : Block // typeof=Element103
	{

		public Element103() : base("minecraft:element_103")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_103";
			return record;
		} // method
	} // class

	public partial class Element104 : Block // typeof=Element104
	{

		public Element104() : base("minecraft:element_104")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_104";
			return record;
		} // method
	} // class

	public partial class Element105 : Block // typeof=Element105
	{

		public Element105() : base("minecraft:element_105")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_105";
			return record;
		} // method
	} // class

	public partial class Element106 : Block // typeof=Element106
	{

		public Element106() : base("minecraft:element_106")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_106";
			return record;
		} // method
	} // class

	public partial class Element107 : Block // typeof=Element107
	{

		public Element107() : base("minecraft:element_107")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_107";
			return record;
		} // method
	} // class

	public partial class Element108 : Block // typeof=Element108
	{

		public Element108() : base("minecraft:element_108")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_108";
			return record;
		} // method
	} // class

	public partial class Element109 : Block // typeof=Element109
	{

		public Element109() : base("minecraft:element_109")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_109";
			return record;
		} // method
	} // class

	public partial class Element11 : Block // typeof=Element11
	{

		public Element11() : base("minecraft:element_11")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_11";
			return record;
		} // method
	} // class

	public partial class Element110 : Block // typeof=Element110
	{

		public Element110() : base("minecraft:element_110")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_110";
			return record;
		} // method
	} // class

	public partial class Element111 : Block // typeof=Element111
	{

		public Element111() : base("minecraft:element_111")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_111";
			return record;
		} // method
	} // class

	public partial class Element112 : Block // typeof=Element112
	{

		public Element112() : base("minecraft:element_112")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_112";
			return record;
		} // method
	} // class

	public partial class Element113 : Block // typeof=Element113
	{

		public Element113() : base("minecraft:element_113")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_113";
			return record;
		} // method
	} // class

	public partial class Element114 : Block // typeof=Element114
	{

		public Element114() : base("minecraft:element_114")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_114";
			return record;
		} // method
	} // class

	public partial class Element115 : Block // typeof=Element115
	{

		public Element115() : base("minecraft:element_115")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_115";
			return record;
		} // method
	} // class

	public partial class Element116 : Block // typeof=Element116
	{

		public Element116() : base("minecraft:element_116")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_116";
			return record;
		} // method
	} // class

	public partial class Element117 : Block // typeof=Element117
	{

		public Element117() : base("minecraft:element_117")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_117";
			return record;
		} // method
	} // class

	public partial class Element118 : Block // typeof=Element118
	{

		public Element118() : base("minecraft:element_118")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_118";
			return record;
		} // method
	} // class

	public partial class Element12 : Block // typeof=Element12
	{

		public Element12() : base("minecraft:element_12")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_12";
			return record;
		} // method
	} // class

	public partial class Element13 : Block // typeof=Element13
	{

		public Element13() : base("minecraft:element_13")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_13";
			return record;
		} // method
	} // class

	public partial class Element14 : Block // typeof=Element14
	{

		public Element14() : base("minecraft:element_14")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_14";
			return record;
		} // method
	} // class

	public partial class Element15 : Block // typeof=Element15
	{

		public Element15() : base("minecraft:element_15")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_15";
			return record;
		} // method
	} // class

	public partial class Element16 : Block // typeof=Element16
	{

		public Element16() : base("minecraft:element_16")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_16";
			return record;
		} // method
	} // class

	public partial class Element17 : Block // typeof=Element17
	{

		public Element17() : base("minecraft:element_17")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_17";
			return record;
		} // method
	} // class

	public partial class Element18 : Block // typeof=Element18
	{

		public Element18() : base("minecraft:element_18")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_18";
			return record;
		} // method
	} // class

	public partial class Element19 : Block // typeof=Element19
	{

		public Element19() : base("minecraft:element_19")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_19";
			return record;
		} // method
	} // class

	public partial class Element2 : Block // typeof=Element2
	{

		public Element2() : base("minecraft:element_2")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_2";
			return record;
		} // method
	} // class

	public partial class Element20 : Block // typeof=Element20
	{

		public Element20() : base("minecraft:element_20")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_20";
			return record;
		} // method
	} // class

	public partial class Element21 : Block // typeof=Element21
	{

		public Element21() : base("minecraft:element_21")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_21";
			return record;
		} // method
	} // class

	public partial class Element22 : Block // typeof=Element22
	{

		public Element22() : base("minecraft:element_22")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_22";
			return record;
		} // method
	} // class

	public partial class Element23 : Block // typeof=Element23
	{

		public Element23() : base("minecraft:element_23")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_23";
			return record;
		} // method
	} // class

	public partial class Element24 : Block // typeof=Element24
	{

		public Element24() : base("minecraft:element_24")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_24";
			return record;
		} // method
	} // class

	public partial class Element25 : Block // typeof=Element25
	{

		public Element25() : base("minecraft:element_25")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_25";
			return record;
		} // method
	} // class

	public partial class Element26 : Block // typeof=Element26
	{

		public Element26() : base("minecraft:element_26")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_26";
			return record;
		} // method
	} // class

	public partial class Element27 : Block // typeof=Element27
	{

		public Element27() : base("minecraft:element_27")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_27";
			return record;
		} // method
	} // class

	public partial class Element28 : Block // typeof=Element28
	{

		public Element28() : base("minecraft:element_28")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_28";
			return record;
		} // method
	} // class

	public partial class Element29 : Block // typeof=Element29
	{

		public Element29() : base("minecraft:element_29")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_29";
			return record;
		} // method
	} // class

	public partial class Element3 : Block // typeof=Element3
	{

		public Element3() : base("minecraft:element_3")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_3";
			return record;
		} // method
	} // class

	public partial class Element30 : Block // typeof=Element30
	{

		public Element30() : base("minecraft:element_30")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_30";
			return record;
		} // method
	} // class

	public partial class Element31 : Block // typeof=Element31
	{

		public Element31() : base("minecraft:element_31")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_31";
			return record;
		} // method
	} // class

	public partial class Element32 : Block // typeof=Element32
	{

		public Element32() : base("minecraft:element_32")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_32";
			return record;
		} // method
	} // class

	public partial class Element33 : Block // typeof=Element33
	{

		public Element33() : base("minecraft:element_33")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_33";
			return record;
		} // method
	} // class

	public partial class Element34 : Block // typeof=Element34
	{

		public Element34() : base("minecraft:element_34")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_34";
			return record;
		} // method
	} // class

	public partial class Element35 : Block // typeof=Element35
	{

		public Element35() : base("minecraft:element_35")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_35";
			return record;
		} // method
	} // class

	public partial class Element36 : Block // typeof=Element36
	{

		public Element36() : base("minecraft:element_36")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_36";
			return record;
		} // method
	} // class

	public partial class Element37 : Block // typeof=Element37
	{

		public Element37() : base("minecraft:element_37")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_37";
			return record;
		} // method
	} // class

	public partial class Element38 : Block // typeof=Element38
	{

		public Element38() : base("minecraft:element_38")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_38";
			return record;
		} // method
	} // class

	public partial class Element39 : Block // typeof=Element39
	{

		public Element39() : base("minecraft:element_39")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_39";
			return record;
		} // method
	} // class

	public partial class Element4 : Block // typeof=Element4
	{

		public Element4() : base("minecraft:element_4")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_4";
			return record;
		} // method
	} // class

	public partial class Element40 : Block // typeof=Element40
	{

		public Element40() : base("minecraft:element_40")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_40";
			return record;
		} // method
	} // class

	public partial class Element41 : Block // typeof=Element41
	{

		public Element41() : base("minecraft:element_41")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_41";
			return record;
		} // method
	} // class

	public partial class Element42 : Block // typeof=Element42
	{

		public Element42() : base("minecraft:element_42")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_42";
			return record;
		} // method
	} // class

	public partial class Element43 : Block // typeof=Element43
	{

		public Element43() : base("minecraft:element_43")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_43";
			return record;
		} // method
	} // class

	public partial class Element44 : Block // typeof=Element44
	{

		public Element44() : base("minecraft:element_44")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_44";
			return record;
		} // method
	} // class

	public partial class Element45 : Block // typeof=Element45
	{

		public Element45() : base("minecraft:element_45")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_45";
			return record;
		} // method
	} // class

	public partial class Element46 : Block // typeof=Element46
	{

		public Element46() : base("minecraft:element_46")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_46";
			return record;
		} // method
	} // class

	public partial class Element47 : Block // typeof=Element47
	{

		public Element47() : base("minecraft:element_47")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_47";
			return record;
		} // method
	} // class

	public partial class Element48 : Block // typeof=Element48
	{

		public Element48() : base("minecraft:element_48")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_48";
			return record;
		} // method
	} // class

	public partial class Element49 : Block // typeof=Element49
	{

		public Element49() : base("minecraft:element_49")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_49";
			return record;
		} // method
	} // class

	public partial class Element5 : Block // typeof=Element5
	{

		public Element5() : base("minecraft:element_5")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_5";
			return record;
		} // method
	} // class

	public partial class Element50 : Block // typeof=Element50
	{

		public Element50() : base("minecraft:element_50")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_50";
			return record;
		} // method
	} // class

	public partial class Element51 : Block // typeof=Element51
	{

		public Element51() : base("minecraft:element_51")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_51";
			return record;
		} // method
	} // class

	public partial class Element52 : Block // typeof=Element52
	{

		public Element52() : base("minecraft:element_52")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_52";
			return record;
		} // method
	} // class

	public partial class Element53 : Block // typeof=Element53
	{

		public Element53() : base("minecraft:element_53")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_53";
			return record;
		} // method
	} // class

	public partial class Element54 : Block // typeof=Element54
	{

		public Element54() : base("minecraft:element_54")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_54";
			return record;
		} // method
	} // class

	public partial class Element55 : Block // typeof=Element55
	{

		public Element55() : base("minecraft:element_55")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_55";
			return record;
		} // method
	} // class

	public partial class Element56 : Block // typeof=Element56
	{

		public Element56() : base("minecraft:element_56")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_56";
			return record;
		} // method
	} // class

	public partial class Element57 : Block // typeof=Element57
	{

		public Element57() : base("minecraft:element_57")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_57";
			return record;
		} // method
	} // class

	public partial class Element58 : Block // typeof=Element58
	{

		public Element58() : base("minecraft:element_58")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_58";
			return record;
		} // method
	} // class

	public partial class Element59 : Block // typeof=Element59
	{

		public Element59() : base("minecraft:element_59")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_59";
			return record;
		} // method
	} // class

	public partial class Element6 : Block // typeof=Element6
	{

		public Element6() : base("minecraft:element_6")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_6";
			return record;
		} // method
	} // class

	public partial class Element60 : Block // typeof=Element60
	{

		public Element60() : base("minecraft:element_60")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_60";
			return record;
		} // method
	} // class

	public partial class Element61 : Block // typeof=Element61
	{

		public Element61() : base("minecraft:element_61")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_61";
			return record;
		} // method
	} // class

	public partial class Element62 : Block // typeof=Element62
	{

		public Element62() : base("minecraft:element_62")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_62";
			return record;
		} // method
	} // class

	public partial class Element63 : Block // typeof=Element63
	{

		public Element63() : base("minecraft:element_63")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_63";
			return record;
		} // method
	} // class

	public partial class Element64 : Block // typeof=Element64
	{

		public Element64() : base("minecraft:element_64")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_64";
			return record;
		} // method
	} // class

	public partial class Element65 : Block // typeof=Element65
	{

		public Element65() : base("minecraft:element_65")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_65";
			return record;
		} // method
	} // class

	public partial class Element66 : Block // typeof=Element66
	{

		public Element66() : base("minecraft:element_66")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_66";
			return record;
		} // method
	} // class

	public partial class Element67 : Block // typeof=Element67
	{

		public Element67() : base("minecraft:element_67")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_67";
			return record;
		} // method
	} // class

	public partial class Element68 : Block // typeof=Element68
	{

		public Element68() : base("minecraft:element_68")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_68";
			return record;
		} // method
	} // class

	public partial class Element69 : Block // typeof=Element69
	{

		public Element69() : base("minecraft:element_69")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_69";
			return record;
		} // method
	} // class

	public partial class Element7 : Block // typeof=Element7
	{

		public Element7() : base("minecraft:element_7")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_7";
			return record;
		} // method
	} // class

	public partial class Element70 : Block // typeof=Element70
	{

		public Element70() : base("minecraft:element_70")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_70";
			return record;
		} // method
	} // class

	public partial class Element71 : Block // typeof=Element71
	{

		public Element71() : base("minecraft:element_71")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_71";
			return record;
		} // method
	} // class

	public partial class Element72 : Block // typeof=Element72
	{

		public Element72() : base("minecraft:element_72")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_72";
			return record;
		} // method
	} // class

	public partial class Element73 : Block // typeof=Element73
	{

		public Element73() : base("minecraft:element_73")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_73";
			return record;
		} // method
	} // class

	public partial class Element74 : Block // typeof=Element74
	{

		public Element74() : base("minecraft:element_74")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_74";
			return record;
		} // method
	} // class

	public partial class Element75 : Block // typeof=Element75
	{

		public Element75() : base("minecraft:element_75")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_75";
			return record;
		} // method
	} // class

	public partial class Element76 : Block // typeof=Element76
	{

		public Element76() : base("minecraft:element_76")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_76";
			return record;
		} // method
	} // class

	public partial class Element77 : Block // typeof=Element77
	{

		public Element77() : base("minecraft:element_77")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_77";
			return record;
		} // method
	} // class

	public partial class Element78 : Block // typeof=Element78
	{

		public Element78() : base("minecraft:element_78")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_78";
			return record;
		} // method
	} // class

	public partial class Element79 : Block // typeof=Element79
	{

		public Element79() : base("minecraft:element_79")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_79";
			return record;
		} // method
	} // class

	public partial class Element8 : Block // typeof=Element8
	{

		public Element8() : base("minecraft:element_8")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_8";
			return record;
		} // method
	} // class

	public partial class Element80 : Block // typeof=Element80
	{

		public Element80() : base("minecraft:element_80")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_80";
			return record;
		} // method
	} // class

	public partial class Element81 : Block // typeof=Element81
	{

		public Element81() : base("minecraft:element_81")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_81";
			return record;
		} // method
	} // class

	public partial class Element82 : Block // typeof=Element82
	{

		public Element82() : base("minecraft:element_82")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_82";
			return record;
		} // method
	} // class

	public partial class Element83 : Block // typeof=Element83
	{

		public Element83() : base("minecraft:element_83")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_83";
			return record;
		} // method
	} // class

	public partial class Element84 : Block // typeof=Element84
	{

		public Element84() : base("minecraft:element_84")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_84";
			return record;
		} // method
	} // class

	public partial class Element85 : Block // typeof=Element85
	{

		public Element85() : base("minecraft:element_85")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_85";
			return record;
		} // method
	} // class

	public partial class Element86 : Block // typeof=Element86
	{

		public Element86() : base("minecraft:element_86")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_86";
			return record;
		} // method
	} // class

	public partial class Element87 : Block // typeof=Element87
	{

		public Element87() : base("minecraft:element_87")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_87";
			return record;
		} // method
	} // class

	public partial class Element88 : Block // typeof=Element88
	{

		public Element88() : base("minecraft:element_88")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_88";
			return record;
		} // method
	} // class

	public partial class Element89 : Block // typeof=Element89
	{

		public Element89() : base("minecraft:element_89")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_89";
			return record;
		} // method
	} // class

	public partial class Element9 : Block // typeof=Element9
	{

		public Element9() : base("minecraft:element_9")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_9";
			return record;
		} // method
	} // class

	public partial class Element90 : Block // typeof=Element90
	{

		public Element90() : base("minecraft:element_90")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_90";
			return record;
		} // method
	} // class

	public partial class Element91 : Block // typeof=Element91
	{

		public Element91() : base("minecraft:element_91")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_91";
			return record;
		} // method
	} // class

	public partial class Element92 : Block // typeof=Element92
	{

		public Element92() : base("minecraft:element_92")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_92";
			return record;
		} // method
	} // class

	public partial class Element93 : Block // typeof=Element93
	{

		public Element93() : base("minecraft:element_93")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_93";
			return record;
		} // method
	} // class

	public partial class Element94 : Block // typeof=Element94
	{

		public Element94() : base("minecraft:element_94")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_94";
			return record;
		} // method
	} // class

	public partial class Element95 : Block // typeof=Element95
	{

		public Element95() : base("minecraft:element_95")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_95";
			return record;
		} // method
	} // class

	public partial class Element96 : Block // typeof=Element96
	{

		public Element96() : base("minecraft:element_96")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_96";
			return record;
		} // method
	} // class

	public partial class Element97 : Block // typeof=Element97
	{

		public Element97() : base("minecraft:element_97")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_97";
			return record;
		} // method
	} // class

	public partial class Element98 : Block // typeof=Element98
	{

		public Element98() : base("minecraft:element_98")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_98";
			return record;
		} // method
	} // class

	public partial class Element99 : Block // typeof=Element99
	{

		public Element99() : base("minecraft:element_99")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:element_99";
			return record;
		} // method
	} // class

	public partial class EmeraldBlock  // typeof=EmeraldBlock
	{
		public override string Id { get; protected set; } = "minecraft:emerald_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:emerald_block";
			return record;
		} // method
	} // class

	public partial class EmeraldOre  // typeof=EmeraldOre
	{
		public override string Id { get; protected set; } = "minecraft:emerald_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:emerald_ore";
			return record;
		} // method
	} // class

	public partial class EnchantingTable  // typeof=EnchantingTable
	{
		public override string Id { get; protected set; } = "minecraft:enchanting_table";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:enchanting_table";
			return record;
		} // method
	} // class

	public partial class EndBrickStairs  // typeof=EndBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:end_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class EndBricks  // typeof=EndBricks
	{
		public override string Id { get; protected set; } = "minecraft:end_bricks";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_bricks";
			return record;
		} // method
	} // class

	public partial class EndGateway  // typeof=EndGateway
	{
		public override string Id { get; protected set; } = "minecraft:end_gateway";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_gateway";
			return record;
		} // method
	} // class

	public partial class EndPortal  // typeof=EndPortal
	{
		public override string Id { get; protected set; } = "minecraft:end_portal";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_portal";
			return record;
		} // method
	} // class

	public partial class EndPortalFrame  // typeof=EndPortalFrame
	{
		public override string Id { get; protected set; } = "minecraft:end_portal_frame";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool EndPortalEyeBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "end_portal_eye_bit":
						EndPortalEyeBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_portal_frame";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "end_portal_eye_bit", Value = Convert.ToByte(EndPortalEyeBit) });
			return record;
		} // method
	} // class

	public partial class EndRod  // typeof=EndRod
	{
		public override string Id { get; protected set; } = "minecraft:end_rod";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_rod";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class EndStone  // typeof=EndStone
	{
		public override string Id { get; protected set; } = "minecraft:end_stone";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:end_stone";
			return record;
		} // method
	} // class

	public partial class EnderChest  // typeof=EnderChest
	{
		public override string Id { get; protected set; } = "minecraft:ender_chest";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:ender_chest";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ExposedCopper : Block // typeof=ExposedCopper
	{

		public ExposedCopper() : base("minecraft:exposed_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:exposed_copper";
			return record;
		} // method
	} // class

	public partial class ExposedCutCopper : Block // typeof=ExposedCutCopper
	{

		public ExposedCutCopper() : base("minecraft:exposed_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:exposed_cut_copper";
			return record;
		} // method
	} // class

	public partial class ExposedCutCopperSlab : Block // typeof=ExposedCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public ExposedCutCopperSlab() : base("minecraft:exposed_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:exposed_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class ExposedCutCopperStairs : Block // typeof=ExposedCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public ExposedCutCopperStairs() : base("minecraft:exposed_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:exposed_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class ExposedDoubleCutCopperSlab : Block // typeof=ExposedDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public ExposedDoubleCutCopperSlab() : base("minecraft:exposed_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:exposed_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class Farmland  // typeof=Farmland
	{
		public override string Id { get; protected set; } = "minecraft:farmland";

		[StateRange(0, 7)] public int MoisturizedAmount { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "moisturized_amount":
						MoisturizedAmount = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:farmland";
			record.States.Add(new BlockStateInt { Name = "moisturized_amount", Value = MoisturizedAmount });
			return record;
		} // method
	} // class

	public partial class FenceGate  // typeof=FenceGate
	{
		public override string Id { get; protected set; } = "minecraft:fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class Fire  // typeof=Fire
	{
		public override string Id { get; protected set; } = "minecraft:fire";

		[StateRange(0, 15)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:fire";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class FletchingTable : Block // typeof=FletchingTable
	{

		public FletchingTable() : base("minecraft:fletching_table")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:fletching_table";
			return record;
		} // method
	} // class

	public partial class FlowerPot  // typeof=FlowerPot
	{
		public override string Id { get; protected set; } = "minecraft:flower_pot";

		[StateBit] public bool UpdateBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:flower_pot";
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class FloweringAzalea : Block // typeof=FloweringAzalea
	{

		public FloweringAzalea() : base("minecraft:flowering_azalea")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:flowering_azalea";
			return record;
		} // method
	} // class

	public partial class FlowingLava  // typeof=FlowingLava
	{
		public override string Id { get; protected set; } = "minecraft:flowing_lava";

		[StateRange(0, 15)] public override int LiquidDepth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "liquid_depth":
						LiquidDepth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:flowing_lava";
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class FlowingWater  // typeof=FlowingWater
	{
		public override string Id { get; protected set; } = "minecraft:flowing_water";

		[StateRange(0, 15)] public override int LiquidDepth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "liquid_depth":
						LiquidDepth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:flowing_water";
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class Frame  // typeof=Frame
	{
		public override string Id { get; protected set; } = "minecraft:frame";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool ItemFrameMapBit { get; set; } = false;
		[StateBit] public bool ItemFramePhotoBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "item_frame_map_bit":
						ItemFrameMapBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "item_frame_photo_bit":
						ItemFramePhotoBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:frame";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "item_frame_map_bit", Value = Convert.ToByte(ItemFrameMapBit) });
			record.States.Add(new BlockStateByte { Name = "item_frame_photo_bit", Value = Convert.ToByte(ItemFramePhotoBit) });
			return record;
		} // method
	} // class

	public partial class FrogSpawn : Block // typeof=FrogSpawn
	{

		public FrogSpawn() : base("minecraft:frog_spawn")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:frog_spawn";
			return record;
		} // method
	} // class

	public partial class FrostedIce  // typeof=FrostedIce
	{
		public override string Id { get; protected set; } = "minecraft:frosted_ice";

		[StateRange(0, 3)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:frosted_ice";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class Furnace  // typeof=Furnace
	{
		public override string Id { get; protected set; } = "minecraft:furnace";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:furnace";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GildedBlackstone : Block // typeof=GildedBlackstone
	{

		public GildedBlackstone() : base("minecraft:gilded_blackstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gilded_blackstone";
			return record;
		} // method
	} // class

	public partial class Glass  // typeof=Glass
	{
		public override string Id { get; protected set; } = "minecraft:glass";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glass";
			return record;
		} // method
	} // class

	public partial class GlassPane  // typeof=GlassPane
	{
		public override string Id { get; protected set; } = "minecraft:glass_pane";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glass_pane";
			return record;
		} // method
	} // class

	public partial class GlowFrame : Block // typeof=GlowFrame
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool ItemFrameMapBit { get; set; } = false;
		[StateBit] public bool ItemFramePhotoBit { get; set; } = false;

		public GlowFrame() : base("minecraft:glow_frame")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "item_frame_map_bit":
						ItemFrameMapBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "item_frame_photo_bit":
						ItemFramePhotoBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glow_frame";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "item_frame_map_bit", Value = Convert.ToByte(ItemFrameMapBit) });
			record.States.Add(new BlockStateByte { Name = "item_frame_photo_bit", Value = Convert.ToByte(ItemFramePhotoBit) });
			return record;
		} // method
	} // class

	public partial class GlowLichen : Block // typeof=GlowLichen
	{
		[StateRange(0, 63)] public int MultiFaceDirectionBits { get; set; } = 0;

		public GlowLichen() : base("minecraft:glow_lichen")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "multi_face_direction_bits":
						MultiFaceDirectionBits = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glow_lichen";
			record.States.Add(new BlockStateInt { Name = "multi_face_direction_bits", Value = MultiFaceDirectionBits });
			return record;
		} // method
	} // class

	public partial class Glowingobsidian  // typeof=Glowingobsidian
	{
		public override string Id { get; protected set; } = "minecraft:glowingobsidian";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glowingobsidian";
			return record;
		} // method
	} // class

	public partial class Glowstone  // typeof=Glowstone
	{
		public override string Id { get; protected set; } = "minecraft:glowstone";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:glowstone";
			return record;
		} // method
	} // class

	public partial class GoldBlock  // typeof=GoldBlock
	{
		public override string Id { get; protected set; } = "minecraft:gold_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gold_block";
			return record;
		} // method
	} // class

	public partial class GoldOre  // typeof=GoldOre
	{
		public override string Id { get; protected set; } = "minecraft:gold_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gold_ore";
			return record;
		} // method
	} // class

	public partial class GoldenRail  // typeof=GoldenRail
	{
		public override string Id { get; protected set; } = "minecraft:golden_rail";

		[StateBit] public bool RailDataBit { get; set; } = false;
		[StateRange(0, 5)] public int RailDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "rail_data_bit":
						RailDataBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "rail_direction":
						RailDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:golden_rail";
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class GraniteStairs  // typeof=GraniteStairs
	{
		public override string Id { get; protected set; } = "minecraft:granite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:granite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Grass  // typeof=Grass
	{
		public override string Id { get; protected set; } = "minecraft:grass";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:grass";
			return record;
		} // method
	} // class

	public partial class GrassPath  // typeof=GrassPath
	{
		public override string Id { get; protected set; } = "minecraft:grass_path";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:grass_path";
			return record;
		} // method
	} // class

	public partial class Gravel  // typeof=Gravel
	{
		public override string Id { get; protected set; } = "minecraft:gravel";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gravel";
			return record;
		} // method
	} // class

	public partial class GrayCandle : Block // typeof=GrayCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public GrayCandle() : base("minecraft:gray_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gray_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GrayCandleCake : Block // typeof=GrayCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public GrayCandleCake() : base("minecraft:gray_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gray_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GrayGlazedTerracotta  // typeof=GrayGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:gray_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gray_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GrayWool : Block // typeof=GrayWool
	{

		public GrayWool() : base("minecraft:gray_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:gray_wool";
			return record;
		} // method
	} // class

	public partial class GreenCandle : Block // typeof=GreenCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public GreenCandle() : base("minecraft:green_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:green_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GreenCandleCake : Block // typeof=GreenCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public GreenCandleCake() : base("minecraft:green_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:green_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GreenGlazedTerracotta  // typeof=GreenGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:green_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:green_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GreenWool : Block // typeof=GreenWool
	{

		public GreenWool() : base("minecraft:green_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:green_wool";
			return record;
		} // method
	} // class

	public partial class Grindstone : Block // typeof=Grindstone
	{
		[StateEnum("standing", "hanging", "side", "multiple")]
		public string Attachment { get; set; } = "standing";
		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public Grindstone() : base("minecraft:grindstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "attachment":
						Attachment = s.Value;
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:grindstone";
			record.States.Add(new BlockStateString { Name = "attachment", Value = Attachment });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class HangingRoots : Block // typeof=HangingRoots
	{

		public HangingRoots() : base("minecraft:hanging_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hanging_roots";
			return record;
		} // method
	} // class

	public partial class HardGlass : Block // typeof=HardGlass
	{

		public HardGlass() : base("minecraft:hard_glass")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hard_glass";
			return record;
		} // method
	} // class

	public partial class HardGlassPane : Block // typeof=HardGlassPane
	{

		public HardGlassPane() : base("minecraft:hard_glass_pane")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hard_glass_pane";
			return record;
		} // method
	} // class

	public partial class HardStainedGlass : Block // typeof=HardStainedGlass
	{
		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public HardStainedGlass() : base("minecraft:hard_stained_glass")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hard_stained_glass";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class HardStainedGlassPane : Block // typeof=HardStainedGlassPane
	{
		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public HardStainedGlassPane() : base("minecraft:hard_stained_glass_pane")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hard_stained_glass_pane";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class HardenedClay  // typeof=HardenedClay
	{
		public override string Id { get; protected set; } = "minecraft:hardened_clay";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hardened_clay";
			return record;
		} // method
	} // class

	public partial class HayBlock  // typeof=HayBlock
	{
		public override string Id { get; protected set; } = "minecraft:hay_block";

		[StateRange(0, 3)] public int Deprecated { get; set; } = 0;
		[StateEnum("x", "z", "y")]
		public string PillarAxis { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "deprecated":
						Deprecated = s.Value;
						break;
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hay_block";
			record.States.Add(new BlockStateInt { Name = "deprecated", Value = Deprecated });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class HeavyWeightedPressurePlate  // typeof=HeavyWeightedPressurePlate
	{
		public override string Id { get; protected set; } = "minecraft:heavy_weighted_pressure_plate";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:heavy_weighted_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class HoneyBlock : Block // typeof=HoneyBlock
	{

		public HoneyBlock() : base("minecraft:honey_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:honey_block";
			return record;
		} // method
	} // class

	public partial class HoneycombBlock : Block // typeof=HoneycombBlock
	{

		public HoneycombBlock() : base("minecraft:honeycomb_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:honeycomb_block";
			return record;
		} // method
	} // class

	public partial class Hopper  // typeof=Hopper
	{
		public override string Id { get; protected set; } = "minecraft:hopper";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool ToggleBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "toggle_bit":
						ToggleBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:hopper";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "toggle_bit", Value = Convert.ToByte(ToggleBit) });
			return record;
		} // method
	} // class

	public partial class Ice  // typeof=Ice
	{
		public override string Id { get; protected set; } = "minecraft:ice";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:ice";
			return record;
		} // method
	} // class

	public partial class InfestedDeepslate : Block // typeof=InfestedDeepslate
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public InfestedDeepslate() : base("minecraft:infested_deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:infested_deepslate";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class InfoUpdate : Block // typeof=InfoUpdate
	{

		public InfoUpdate() : base("minecraft:info_update")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:info_update";
			return record;
		} // method
	} // class

	public partial class InfoUpdate2 : Block // typeof=InfoUpdate2
	{

		public InfoUpdate2() : base("minecraft:info_update2")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:info_update2";
			return record;
		} // method
	} // class

	public partial class InvisibleBedrock  // typeof=InvisibleBedrock
	{
		public override string Id { get; protected set; } = "minecraft:invisible_bedrock";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:invisible_bedrock";
			return record;
		} // method
	} // class

	public partial class IronBars  // typeof=IronBars
	{
		public override string Id { get; protected set; } = "minecraft:iron_bars";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:iron_bars";
			return record;
		} // method
	} // class

	public partial class IronBlock  // typeof=IronBlock
	{
		public override string Id { get; protected set; } = "minecraft:iron_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:iron_block";
			return record;
		} // method
	} // class

	public partial class IronDoor  // typeof=IronDoor
	{
		public override string Id { get; protected set; } = "minecraft:iron_door";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:iron_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class IronOre  // typeof=IronOre
	{
		public override string Id { get; protected set; } = "minecraft:iron_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:iron_ore";
			return record;
		} // method
	} // class

	public partial class IronTrapdoor  // typeof=IronTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:iron_trapdoor";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:iron_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class Jigsaw : Block // typeof=Jigsaw
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 3)] public int Rotation { get; set; } = 1;

		public Jigsaw() : base("minecraft:jigsaw")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "rotation":
						Rotation = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jigsaw";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "rotation", Value = Rotation });
			return record;
		} // method
	} // class

	public partial class Jukebox  // typeof=Jukebox
	{
		public override string Id { get; protected set; } = "minecraft:jukebox";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jukebox";
			return record;
		} // method
	} // class

	public partial class JungleButton  // typeof=JungleButton
	{
		public override string Id { get; protected set; } = "minecraft:jungle_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class JungleDoor  // typeof=JungleDoor
	{
		public override string Id { get; protected set; } = "minecraft:jungle_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class JungleFence : Block // typeof=JungleFence
	{

		public JungleFence() : base("minecraft:jungle_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_fence";
			return record;
		} // method
	} // class

	public partial class JungleFenceGate  // typeof=JungleFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:jungle_fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class JungleHangingSign : Block // typeof=JungleHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public JungleHangingSign() : base("minecraft:jungle_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class JungleLog : LogBase // typeof=JungleLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

		public JungleLog() : base("minecraft:jungle_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class JunglePressurePlate : Block // typeof=JunglePressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public JunglePressurePlate() : base("minecraft:jungle_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class JungleStairs  // typeof=JungleStairs
	{
		public override string Id { get; protected set; } = "minecraft:jungle_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class JungleStandingSign  // typeof=JungleStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:jungle_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class JungleTrapdoor  // typeof=JungleTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:jungle_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class JungleWallSign  // typeof=JungleWallSign
	{
		public override string Id { get; protected set; } = "minecraft:jungle_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:jungle_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Kelp : Block // typeof=Kelp
	{
		[StateRange(0, 25)] public int KelpAge { get; set; } = 0;

		public Kelp() : base("minecraft:kelp")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "kelp_age":
						KelpAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:kelp";
			record.States.Add(new BlockStateInt { Name = "kelp_age", Value = KelpAge });
			return record;
		} // method
	} // class

	public partial class Ladder  // typeof=Ladder
	{
		public override string Id { get; protected set; } = "minecraft:ladder";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:ladder";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lantern : Block // typeof=Lantern
	{
		[StateBit] public bool Hanging { get; set; } = false;

		public Lantern() : base("minecraft:lantern")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lantern";
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class LapisBlock  // typeof=LapisBlock
	{
		public override string Id { get; protected set; } = "minecraft:lapis_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lapis_block";
			return record;
		} // method
	} // class

	public partial class LapisOre  // typeof=LapisOre
	{
		public override string Id { get; protected set; } = "minecraft:lapis_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lapis_ore";
			return record;
		} // method
	} // class

	public partial class LargeAmethystBud : Block // typeof=LargeAmethystBud
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public LargeAmethystBud() : base("minecraft:large_amethyst_bud")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:large_amethyst_bud";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lava  // typeof=Lava
	{
		public override string Id { get; protected set; } = "minecraft:lava";

		[StateRange(0, 15)] public override int LiquidDepth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "liquid_depth":
						LiquidDepth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lava";
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class LavaCauldron : Block // typeof=LavaCauldron
	{
		[StateEnum("water", "powder_snow", "lava")]
		public string CauldronLiquid { get; set; } = "water";
		[StateRange(0, 6)] public int FillLevel { get; set; } = 0;

		public LavaCauldron() : base("minecraft:lava_cauldron")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "cauldron_liquid":
						CauldronLiquid = s.Value;
						break;
					case BlockStateInt s when s.Name == "fill_level":
						FillLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lava_cauldron";
			record.States.Add(new BlockStateString { Name = "cauldron_liquid", Value = CauldronLiquid });
			record.States.Add(new BlockStateInt { Name = "fill_level", Value = FillLevel });
			return record;
		} // method
	} // class

	public partial class Leaves  // typeof=Leaves
	{
		public override string Id { get; protected set; } = "minecraft:leaves";

		[StateEnum("oak", "spruce", "birch", "jungle")]
		public string OldLeafType { get; set; } = "oak";
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "old_leaf_type":
						OldLeafType = s.Value;
						break;
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:leaves";
			record.States.Add(new BlockStateString { Name = "old_leaf_type", Value = OldLeafType });
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Leaves2  // typeof=Leaves2
	{
		public override string Id { get; protected set; } = "minecraft:leaves2";

		[StateEnum("dark_oak", "acacia")]
		public string NewLeafType { get; set; } = "";
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "new_leaf_type":
						NewLeafType = s.Value;
						break;
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:leaves2";
			record.States.Add(new BlockStateString { Name = "new_leaf_type", Value = NewLeafType });
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Lectern : Block // typeof=Lectern
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool PoweredBit { get; set; } = false;

		public Lectern() : base("minecraft:lectern")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lectern";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Lever  // typeof=Lever
	{
		public override string Id { get; protected set; } = "minecraft:lever";

		[StateEnum("down_east_west", "east", "west", "south", "north", "up_north_south", "up_east_west", "down_north_south")]
		public string LeverDirection { get; set; } = "down_east_west";
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "lever_direction":
						LeverDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lever";
			record.States.Add(new BlockStateString { Name = "lever_direction", Value = LeverDirection });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class LightBlock : Block // typeof=LightBlock
	{
		[StateRange(0, 15)] public int BlockLightLevel { get; set; } = 0;

		public LightBlock() : base("minecraft:light_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "block_light_level":
						BlockLightLevel = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_block";
			record.States.Add(new BlockStateInt { Name = "block_light_level", Value = BlockLightLevel });
			return record;
		} // method
	} // class

	public partial class LightBlueCandle : Block // typeof=LightBlueCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public LightBlueCandle() : base("minecraft:light_blue_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_blue_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightBlueCandleCake : Block // typeof=LightBlueCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public LightBlueCandleCake() : base("minecraft:light_blue_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_blue_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightBlueGlazedTerracotta  // typeof=LightBlueGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:light_blue_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_blue_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LightBlueWool : Block // typeof=LightBlueWool
	{

		public LightBlueWool() : base("minecraft:light_blue_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_blue_wool";
			return record;
		} // method
	} // class

	public partial class LightGrayCandle : Block // typeof=LightGrayCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public LightGrayCandle() : base("minecraft:light_gray_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_gray_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightGrayCandleCake : Block // typeof=LightGrayCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public LightGrayCandleCake() : base("minecraft:light_gray_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_gray_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightGrayWool : Block // typeof=LightGrayWool
	{

		public LightGrayWool() : base("minecraft:light_gray_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_gray_wool";
			return record;
		} // method
	} // class

	public partial class LightWeightedPressurePlate  // typeof=LightWeightedPressurePlate
	{
		public override string Id { get; protected set; } = "minecraft:light_weighted_pressure_plate";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:light_weighted_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class LightningRod : Block // typeof=LightningRod
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public LightningRod() : base("minecraft:lightning_rod")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lightning_rod";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LimeCandle : Block // typeof=LimeCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public LimeCandle() : base("minecraft:lime_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lime_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LimeCandleCake : Block // typeof=LimeCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public LimeCandleCake() : base("minecraft:lime_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lime_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LimeGlazedTerracotta  // typeof=LimeGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:lime_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lime_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LimeWool : Block // typeof=LimeWool
	{

		public LimeWool() : base("minecraft:lime_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lime_wool";
			return record;
		} // method
	} // class

	public partial class LitBlastFurnace  // typeof=LitBlastFurnace
	{
		public override string Id { get; protected set; } = "minecraft:lit_blast_furnace";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_blast_furnace";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LitDeepslateRedstoneOre : Block // typeof=LitDeepslateRedstoneOre
	{

		public LitDeepslateRedstoneOre() : base("minecraft:lit_deepslate_redstone_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_deepslate_redstone_ore";
			return record;
		} // method
	} // class

	public partial class LitFurnace  // typeof=LitFurnace
	{
		public override string Id { get; protected set; } = "minecraft:lit_furnace";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_furnace";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LitPumpkin  // typeof=LitPumpkin
	{
		public override string Id { get; protected set; } = "minecraft:lit_pumpkin";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_pumpkin";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class LitRedstoneLamp  // typeof=LitRedstoneLamp
	{
		public override string Id { get; protected set; } = "minecraft:lit_redstone_lamp";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_redstone_lamp";
			return record;
		} // method
	} // class

	public partial class LitRedstoneOre  // typeof=LitRedstoneOre
	{
		public override string Id { get; protected set; } = "minecraft:lit_redstone_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_redstone_ore";
			return record;
		} // method
	} // class

	public partial class LitSmoker : Block // typeof=LitSmoker
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public LitSmoker() : base("minecraft:lit_smoker")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lit_smoker";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lodestone : Block // typeof=Lodestone
	{

		public Lodestone() : base("minecraft:lodestone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:lodestone";
			return record;
		} // method
	} // class

	public partial class Loom  // typeof=Loom
	{
		public override string Id { get; protected set; } = "minecraft:loom";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:loom";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class MagentaCandle : Block // typeof=MagentaCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public MagentaCandle() : base("minecraft:magenta_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:magenta_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class MagentaCandleCake : Block // typeof=MagentaCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public MagentaCandleCake() : base("minecraft:magenta_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:magenta_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class MagentaGlazedTerracotta  // typeof=MagentaGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:magenta_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:magenta_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MagentaWool : Block // typeof=MagentaWool
	{

		public MagentaWool() : base("minecraft:magenta_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:magenta_wool";
			return record;
		} // method
	} // class

	public partial class Magma : Block // typeof=Magma
	{

		public Magma() : base("minecraft:magma")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:magma";
			return record;
		} // method
	} // class

	public partial class MangroveButton : Block // typeof=MangroveButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public MangroveButton() : base("minecraft:mangrove_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MangroveDoor : Block // typeof=MangroveDoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public MangroveDoor() : base("minecraft:mangrove_door")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class MangroveDoubleSlab : Block // typeof=MangroveDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public MangroveDoubleSlab() : base("minecraft:mangrove_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MangroveFence : Block // typeof=MangroveFence
	{

		public MangroveFence() : base("minecraft:mangrove_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_fence";
			return record;
		} // method
	} // class

	public partial class MangroveFenceGate : Block // typeof=MangroveFenceGate
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public MangroveFenceGate() : base("minecraft:mangrove_fence_gate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class MangroveHangingSign : Block // typeof=MangroveHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public MangroveHangingSign() : base("minecraft:mangrove_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class MangroveLeaves : Block // typeof=MangroveLeaves
	{
		[StateBit] public bool PersistentBit { get; set; } = false;
		[StateBit] public bool UpdateBit { get; set; } = false;

		public MangroveLeaves() : base("minecraft:mangrove_leaves")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "persistent_bit":
						PersistentBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "update_bit":
						UpdateBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_leaves";
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class MangroveLog : LogBase // typeof=MangroveLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		public MangroveLog() : base("minecraft:mangrove_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class MangrovePlanks : Block // typeof=MangrovePlanks
	{

		public MangrovePlanks() : base("minecraft:mangrove_planks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_planks";
			return record;
		} // method
	} // class

	public partial class MangrovePressurePlate : Block // typeof=MangrovePressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public MangrovePressurePlate() : base("minecraft:mangrove_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class MangrovePropagule : Block // typeof=MangrovePropagule
	{
		[StateBit] public bool Hanging { get; set; } = false;
		[StateRange(0, 4)] public int PropaguleStage { get; set; } = 0;

		public MangrovePropagule() : base("minecraft:mangrove_propagule")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "propagule_stage":
						PropaguleStage = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_propagule";
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			record.States.Add(new BlockStateInt { Name = "propagule_stage", Value = PropaguleStage });
			return record;
		} // method
	} // class

	public partial class MangroveRoots : Block // typeof=MangroveRoots
	{

		public MangroveRoots() : base("minecraft:mangrove_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_roots";
			return record;
		} // method
	} // class

	public partial class MangroveSlab : Block // typeof=MangroveSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public MangroveSlab() : base("minecraft:mangrove_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MangroveStairs : Block // typeof=MangroveStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public MangroveStairs() : base("minecraft:mangrove_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MangroveStandingSign : Block // typeof=MangroveStandingSign
	{
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public MangroveStandingSign() : base("minecraft:mangrove_standing_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class MangroveTrapdoor : Block // typeof=MangroveTrapdoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpsideDownBit { get; set; } = false;

		public MangroveTrapdoor() : base("minecraft:mangrove_trapdoor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class MangroveWallSign : Block // typeof=MangroveWallSign
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public MangroveWallSign() : base("minecraft:mangrove_wall_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MangroveWood : Block // typeof=MangroveWood
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";
		[StateBit] public bool StrippedBit { get; set; } = false;

		public MangroveWood() : base("minecraft:mangrove_wood")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
					case BlockStateByte s when s.Name == "stripped_bit":
						StrippedBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mangrove_wood";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			return record;
		} // method
	} // class

	public partial class MediumAmethystBud : Block // typeof=MediumAmethystBud
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public MediumAmethystBud() : base("minecraft:medium_amethyst_bud")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:medium_amethyst_bud";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MelonBlock  // typeof=MelonBlock
	{
		public override string Id { get; protected set; } = "minecraft:melon_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:melon_block";
			return record;
		} // method
	} // class

	public partial class MelonStem  // typeof=MelonStem
	{
		public override string Id { get; protected set; } = "minecraft:melon_stem";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 7)] public int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:melon_stem";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class MobSpawner  // typeof=MobSpawner
	{
		public override string Id { get; protected set; } = "minecraft:mob_spawner";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mob_spawner";
			return record;
		} // method
	} // class

	public partial class MonsterEgg  // typeof=MonsterEgg
	{
		public override string Id { get; protected set; } = "minecraft:monster_egg";

		[StateEnum("cobblestone", "stone_brick", "mossy_stone_brick", "cracked_stone_brick", "chiseled_stone_brick", "stone")]
		public string MonsterEggStoneType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "monster_egg_stone_type":
						MonsterEggStoneType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:monster_egg";
			record.States.Add(new BlockStateString { Name = "monster_egg_stone_type", Value = MonsterEggStoneType });
			return record;
		} // method
	} // class

	public partial class MossBlock : Block // typeof=MossBlock
	{

		public MossBlock() : base("minecraft:moss_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:moss_block";
			return record;
		} // method
	} // class

	public partial class MossCarpet : Block // typeof=MossCarpet
	{

		public MossCarpet() : base("minecraft:moss_carpet")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:moss_carpet";
			return record;
		} // method
	} // class

	public partial class MossyCobblestone  // typeof=MossyCobblestone
	{
		public override string Id { get; protected set; } = "minecraft:mossy_cobblestone";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mossy_cobblestone";
			return record;
		} // method
	} // class

	public partial class MossyCobblestoneStairs  // typeof=MossyCobblestoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:mossy_cobblestone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mossy_cobblestone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MossyStoneBrickStairs  // typeof=MossyStoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:mossy_stone_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mossy_stone_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MovingBlock : Block // typeof=MovingBlock
	{

		public MovingBlock() : base("minecraft:moving_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:moving_block";
			return record;
		} // method
	} // class

	public partial class Mud : Block // typeof=Mud
	{

		public Mud() : base("minecraft:mud")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud";
			return record;
		} // method
	} // class

	public partial class MudBrickDoubleSlab : Block // typeof=MudBrickDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public MudBrickDoubleSlab() : base("minecraft:mud_brick_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud_brick_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MudBrickSlab : Block // typeof=MudBrickSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public MudBrickSlab() : base("minecraft:mud_brick_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud_brick_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MudBrickStairs : Block // typeof=MudBrickStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public MudBrickStairs() : base("minecraft:mud_brick_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MudBrickWall : Block // typeof=MudBrickWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public MudBrickWall() : base("minecraft:mud_brick_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud_brick_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class MudBricks : Block // typeof=MudBricks
	{

		public MudBricks() : base("minecraft:mud_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mud_bricks";
			return record;
		} // method
	} // class

	public partial class MuddyMangroveRoots : Block // typeof=MuddyMangroveRoots
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public MuddyMangroveRoots() : base("minecraft:muddy_mangrove_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:muddy_mangrove_roots";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Mycelium  // typeof=Mycelium
	{
		public override string Id { get; protected set; } = "minecraft:mycelium";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:mycelium";
			return record;
		} // method
	} // class

	public partial class NetherBrick  // typeof=NetherBrick
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_brick";
			return record;
		} // method
	} // class

	public partial class NetherBrickFence  // typeof=NetherBrickFence
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick_fence";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_brick_fence";
			return record;
		} // method
	} // class

	public partial class NetherBrickStairs  // typeof=NetherBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class NetherGoldOre : Block // typeof=NetherGoldOre
	{

		public NetherGoldOre() : base("minecraft:nether_gold_ore")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_gold_ore";
			return record;
		} // method
	} // class

	public partial class NetherSprouts : Block // typeof=NetherSprouts
	{

		public NetherSprouts() : base("minecraft:nether_sprouts")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_sprouts";
			return record;
		} // method
	} // class

	public partial class NetherWart  // typeof=NetherWart
	{
		public override string Id { get; protected set; } = "minecraft:nether_wart";

		[StateRange(0, 3)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_wart";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class NetherWartBlock : Block // typeof=NetherWartBlock
	{

		public NetherWartBlock() : base("minecraft:nether_wart_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:nether_wart_block";
			return record;
		} // method
	} // class

	public partial class NetheriteBlock : Block // typeof=NetheriteBlock
	{

		public NetheriteBlock() : base("minecraft:netherite_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:netherite_block";
			return record;
		} // method
	} // class

	public partial class Netherrack  // typeof=Netherrack
	{
		public override string Id { get; protected set; } = "minecraft:netherrack";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:netherrack";
			return record;
		} // method
	} // class

	public partial class Netherreactor  // typeof=Netherreactor
	{
		public override string Id { get; protected set; } = "minecraft:netherreactor";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:netherreactor";
			return record;
		} // method
	} // class

	public partial class NormalStoneStairs  // typeof=NormalStoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:normal_stone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:normal_stone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Noteblock  // typeof=Noteblock
	{
		public override string Id { get; protected set; } = "minecraft:noteblock";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:noteblock";
			return record;
		} // method
	} // class

	public partial class OakFence : Block // typeof=OakFence
	{

		public OakFence() : base("minecraft:oak_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oak_fence";
			return record;
		} // method
	} // class

	public partial class OakHangingSign : Block // typeof=OakHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public OakHangingSign() : base("minecraft:oak_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oak_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class OakLog : LogBase // typeof=OakLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		public OakLog() : base("minecraft:oak_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oak_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class OakStairs  // typeof=OakStairs
	{
		public override string Id { get; protected set; } = "minecraft:oak_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oak_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Observer  // typeof=Observer
	{
		public override string Id { get; protected set; } = "minecraft:observer";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateBit] public bool PoweredBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:observer";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Obsidian  // typeof=Obsidian
	{
		public override string Id { get; protected set; } = "minecraft:obsidian";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:obsidian";
			return record;
		} // method
	} // class

	public partial class OchreFroglight : Block // typeof=OchreFroglight
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public OchreFroglight() : base("minecraft:ochre_froglight")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:ochre_froglight";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class OrangeCandle : Block // typeof=OrangeCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public OrangeCandle() : base("minecraft:orange_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:orange_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class OrangeCandleCake : Block // typeof=OrangeCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public OrangeCandleCake() : base("minecraft:orange_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:orange_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class OrangeGlazedTerracotta  // typeof=OrangeGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:orange_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:orange_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class OrangeWool : Block // typeof=OrangeWool
	{

		public OrangeWool() : base("minecraft:orange_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:orange_wool";
			return record;
		} // method
	} // class

	public partial class OxidizedCopper : Block // typeof=OxidizedCopper
	{

		public OxidizedCopper() : base("minecraft:oxidized_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oxidized_copper";
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopper : Block // typeof=OxidizedCutCopper
	{

		public OxidizedCutCopper() : base("minecraft:oxidized_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oxidized_cut_copper";
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopperSlab : Block // typeof=OxidizedCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public OxidizedCutCopperSlab() : base("minecraft:oxidized_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oxidized_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopperStairs : Block // typeof=OxidizedCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public OxidizedCutCopperStairs() : base("minecraft:oxidized_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oxidized_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class OxidizedDoubleCutCopperSlab : Block // typeof=OxidizedDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public OxidizedDoubleCutCopperSlab() : base("minecraft:oxidized_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:oxidized_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PackedIce  // typeof=PackedIce
	{
		public override string Id { get; protected set; } = "minecraft:packed_ice";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:packed_ice";
			return record;
		} // method
	} // class

	public partial class PackedMud : Block // typeof=PackedMud
	{

		public PackedMud() : base("minecraft:packed_mud")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:packed_mud";
			return record;
		} // method
	} // class

	public partial class PearlescentFroglight : Block // typeof=PearlescentFroglight
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public PearlescentFroglight() : base("minecraft:pearlescent_froglight")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pearlescent_froglight";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PinkCandle : Block // typeof=PinkCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public PinkCandle() : base("minecraft:pink_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pink_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PinkCandleCake : Block // typeof=PinkCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public PinkCandleCake() : base("minecraft:pink_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pink_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PinkGlazedTerracotta  // typeof=PinkGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:pink_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pink_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PinkPetals : Block // typeof=PinkPetals
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateRange(0, 7)] public int Growth { get; set; } = 0;

		public PinkPetals() : base("minecraft:pink_petals")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pink_petals";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PinkWool : Block // typeof=PinkWool
	{

		public PinkWool() : base("minecraft:pink_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pink_wool";
			return record;
		} // method
	} // class

	public partial class Piston  // typeof=Piston
	{
		public override string Id { get; protected set; } = "minecraft:piston";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:piston";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PistonArmCollision  // typeof=PistonArmCollision
	{
		public override string Id { get; protected set; } = "minecraft:piston_arm_collision";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:piston_arm_collision";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Planks  // typeof=Planks
	{
		public override string Id { get; protected set; } = "minecraft:planks";

		[StateEnum("spruce", "birch", "jungle", "acacia", "dark_oak", "oak")]
		public string WoodType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wood_type":
						WoodType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:planks";
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class Podzol  // typeof=Podzol
	{
		public override string Id { get; protected set; } = "minecraft:podzol";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:podzol";
			return record;
		} // method
	} // class

	public partial class PointedDripstone : Block // typeof=PointedDripstone
	{
		[StateEnum("tip", "frustum", "middle", "base", "merge")]
		public string DripstoneThickness { get; set; } = "tip";
		[StateBit] public bool Hanging { get; set; } = false;

		public PointedDripstone() : base("minecraft:pointed_dripstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "dripstone_thickness":
						DripstoneThickness = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pointed_dripstone";
			record.States.Add(new BlockStateString { Name = "dripstone_thickness", Value = DripstoneThickness });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class PolishedAndesiteStairs  // typeof=PolishedAndesiteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_andesite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_andesite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBasalt : Block // typeof=PolishedBasalt
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public PolishedBasalt() : base("minecraft:polished_basalt")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_basalt";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstone : Block // typeof=PolishedBlackstone
	{

		public PolishedBlackstone() : base("minecraft:polished_blackstone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone";
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickDoubleSlab  // typeof=PolishedBlackstoneBrickDoubleSlab
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_double_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_brick_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickSlab  // typeof=PolishedBlackstoneBrickSlab
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_brick_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickStairs  // typeof=PolishedBlackstoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickWall : Block // typeof=PolishedBlackstoneBrickWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public PolishedBlackstoneBrickWall() : base("minecraft:polished_blackstone_brick_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_brick_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBricks : Block // typeof=PolishedBlackstoneBricks
	{

		public PolishedBlackstoneBricks() : base("minecraft:polished_blackstone_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_bricks";
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneButton : Block // typeof=PolishedBlackstoneButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public PolishedBlackstoneButton() : base("minecraft:polished_blackstone_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneDoubleSlab  // typeof=PolishedBlackstoneDoubleSlab
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_double_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstonePressurePlate : Block // typeof=PolishedBlackstonePressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public PolishedBlackstonePressurePlate() : base("minecraft:polished_blackstone_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneSlab  // typeof=PolishedBlackstoneSlab
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneStairs  // typeof=PolishedBlackstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneWall : Block // typeof=PolishedBlackstoneWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public PolishedBlackstoneWall() : base("minecraft:polished_blackstone_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_blackstone_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslate : Block // typeof=PolishedDeepslate
	{

		public PolishedDeepslate() : base("minecraft:polished_deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_deepslate";
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateDoubleSlab : Block // typeof=PolishedDeepslateDoubleSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public PolishedDeepslateDoubleSlab() : base("minecraft:polished_deepslate_double_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_deepslate_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateSlab : Block // typeof=PolishedDeepslateSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public PolishedDeepslateSlab() : base("minecraft:polished_deepslate_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_deepslate_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateStairs : Block // typeof=PolishedDeepslateStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public PolishedDeepslateStairs() : base("minecraft:polished_deepslate_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_deepslate_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateWall : Block // typeof=PolishedDeepslateWall
	{
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";
		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";
		[StateBit] public bool WallPostBit { get; set; } = false;

		public PolishedDeepslateWall() : base("minecraft:polished_deepslate_wall")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "wall_connection_type_east":
						WallConnectionTypeEast = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_north":
						WallConnectionTypeNorth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_south":
						WallConnectionTypeSouth = s.Value;
						break;
					case BlockStateString s when s.Name == "wall_connection_type_west":
						WallConnectionTypeWest = s.Value;
						break;
					case BlockStateByte s when s.Name == "wall_post_bit":
						WallPostBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_deepslate_wall";
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDioriteStairs  // typeof=PolishedDioriteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_diorite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_diorite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedGraniteStairs  // typeof=PolishedGraniteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_granite_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:polished_granite_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Portal  // typeof=Portal
	{
		public override string Id { get; protected set; } = "minecraft:portal";

		[StateEnum("x", "z", "unknown")]
		public string PortalAxis { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "portal_axis":
						PortalAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:portal";
			record.States.Add(new BlockStateString { Name = "portal_axis", Value = PortalAxis });
			return record;
		} // method
	} // class

	public partial class Potatoes  // typeof=Potatoes
	{
		public override string Id { get; protected set; } = "minecraft:potatoes";

		[StateRange(0, 7)] public override int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:potatoes";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PowderSnow : Block // typeof=PowderSnow
	{

		public PowderSnow() : base("minecraft:powder_snow")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:powder_snow";
			return record;
		} // method
	} // class

	public partial class PoweredComparator  // typeof=PoweredComparator
	{
		public override string Id { get; protected set; } = "minecraft:powered_comparator";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OutputLitBit { get; set; } = false;
		[StateBit] public bool OutputSubtractBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "output_lit_bit":
						OutputLitBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "output_subtract_bit":
						OutputSubtractBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:powered_comparator";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit) });
			record.States.Add(new BlockStateByte { Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit) });
			return record;
		} // method
	} // class

	public partial class PoweredRepeater  // typeof=PoweredRepeater
	{
		public override string Id { get; protected set; } = "minecraft:powered_repeater";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateRange(0, 3)] public int RepeaterDelay { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateInt s when s.Name == "repeater_delay":
						RepeaterDelay = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:powered_repeater";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "repeater_delay", Value = RepeaterDelay });
			return record;
		} // method
	} // class

	public partial class Prismarine  // typeof=Prismarine
	{
		public override string Id { get; protected set; } = "minecraft:prismarine";

		[StateEnum("dark", "bricks", "default")]
		public string PrismarineBlockType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "prismarine_block_type":
						PrismarineBlockType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:prismarine";
			record.States.Add(new BlockStateString { Name = "prismarine_block_type", Value = PrismarineBlockType });
			return record;
		} // method
	} // class

	public partial class PrismarineBricksStairs  // typeof=PrismarineBricksStairs
	{
		public override string Id { get; protected set; } = "minecraft:prismarine_bricks_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:prismarine_bricks_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PrismarineStairs  // typeof=PrismarineStairs
	{
		public override string Id { get; protected set; } = "minecraft:prismarine_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:prismarine_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Pumpkin  // typeof=Pumpkin
	{
		public override string Id { get; protected set; } = "minecraft:pumpkin";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pumpkin";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class PumpkinStem  // typeof=PumpkinStem
	{
		public override string Id { get; protected set; } = "minecraft:pumpkin_stem";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 7)] public int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:pumpkin_stem";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PurpleCandle : Block // typeof=PurpleCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public PurpleCandle() : base("minecraft:purple_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purple_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PurpleCandleCake : Block // typeof=PurpleCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public PurpleCandleCake() : base("minecraft:purple_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purple_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PurpleGlazedTerracotta  // typeof=PurpleGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:purple_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purple_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PurpleWool : Block // typeof=PurpleWool
	{

		public PurpleWool() : base("minecraft:purple_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purple_wool";
			return record;
		} // method
	} // class

	public partial class PurpurBlock  // typeof=PurpurBlock
	{
		public override string Id { get; protected set; } = "minecraft:purpur_block";

		[StateEnum("default", "chiseled", "lines", "smooth")]
		public string ChiselType { get; set; } = "";
		[StateEnum("x", "z", "y")]
		public string PillarAxis { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "chisel_type":
						ChiselType = s.Value;
						break;
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purpur_block";
			record.States.Add(new BlockStateString { Name = "chisel_type", Value = ChiselType });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PurpurStairs  // typeof=PurpurStairs
	{
		public override string Id { get; protected set; } = "minecraft:purpur_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:purpur_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class QuartzBlock  // typeof=QuartzBlock
	{
		public override string Id { get; protected set; } = "minecraft:quartz_block";

		[StateEnum("default", "chiseled", "lines", "smooth")]
		public string ChiselType { get; set; } = "";
		[StateEnum("x", "z", "y")]
		public string PillarAxis { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "chisel_type":
						ChiselType = s.Value;
						break;
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:quartz_block";
			record.States.Add(new BlockStateString { Name = "chisel_type", Value = ChiselType });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class QuartzBricks : Block // typeof=QuartzBricks
	{

		public QuartzBricks() : base("minecraft:quartz_bricks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:quartz_bricks";
			return record;
		} // method
	} // class

	public partial class QuartzOre  // typeof=QuartzOre
	{
		public override string Id { get; protected set; } = "minecraft:quartz_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:quartz_ore";
			return record;
		} // method
	} // class

	public partial class QuartzStairs  // typeof=QuartzStairs
	{
		public override string Id { get; protected set; } = "minecraft:quartz_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:quartz_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Rail  // typeof=Rail
	{
		public override string Id { get; protected set; } = "minecraft:rail";

		[StateRange(0, 9)] public int RailDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "rail_direction":
						RailDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:rail";
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class RawCopperBlock : Block // typeof=RawCopperBlock
	{

		public RawCopperBlock() : base("minecraft:raw_copper_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:raw_copper_block";
			return record;
		} // method
	} // class

	public partial class RawGoldBlock : Block // typeof=RawGoldBlock
	{

		public RawGoldBlock() : base("minecraft:raw_gold_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:raw_gold_block";
			return record;
		} // method
	} // class

	public partial class RawIronBlock : Block // typeof=RawIronBlock
	{

		public RawIronBlock() : base("minecraft:raw_iron_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:raw_iron_block";
			return record;
		} // method
	} // class

	public partial class RedCandle : Block // typeof=RedCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public RedCandle() : base("minecraft:red_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class RedCandleCake : Block // typeof=RedCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public RedCandleCake() : base("minecraft:red_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class RedFlower  // typeof=RedFlower
	{
		public override string Id { get; protected set; } = "minecraft:red_flower";

		[StateEnum("orchid", "allium", "houstonia", "tulip_red", "tulip_orange", "tulip_white", "tulip_pink", "oxeye", "cornflower", "lily_of_the_valley", "poppy")]
		public string FlowerType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "flower_type":
						FlowerType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_flower";
			record.States.Add(new BlockStateString { Name = "flower_type", Value = FlowerType });
			return record;
		} // method
	} // class

	public partial class RedGlazedTerracotta  // typeof=RedGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:red_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class RedMushroom  // typeof=RedMushroom
	{
		public override string Id { get; protected set; } = "minecraft:red_mushroom";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_mushroom";
			return record;
		} // method
	} // class

	public partial class RedMushroomBlock  // typeof=RedMushroomBlock
	{
		public override string Id { get; protected set; } = "minecraft:red_mushroom_block";

		[StateRange(0, 15)] public int HugeMushroomBits { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "huge_mushroom_bits":
						HugeMushroomBits = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_mushroom_block";
			record.States.Add(new BlockStateInt { Name = "huge_mushroom_bits", Value = HugeMushroomBits });
			return record;
		} // method
	} // class

	public partial class RedNetherBrick : Block // typeof=RedNetherBrick
	{

		public RedNetherBrick() : base("minecraft:red_nether_brick")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_nether_brick";
			return record;
		} // method
	} // class

	public partial class RedNetherBrickStairs  // typeof=RedNetherBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:red_nether_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_nether_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class RedSandstone  // typeof=RedSandstone
	{
		public override string Id { get; protected set; } = "minecraft:red_sandstone";

		[StateEnum("default", "heiroglyphs", "cut", "smooth")]
		public string SandStoneType { get; set; } = "default";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "sand_stone_type":
						SandStoneType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_sandstone";
			record.States.Add(new BlockStateString { Name = "sand_stone_type", Value = SandStoneType });
			return record;
		} // method
	} // class

	public partial class RedSandstoneStairs  // typeof=RedSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:red_sandstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_sandstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class RedWool : Block // typeof=RedWool
	{

		public RedWool() : base("minecraft:red_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:red_wool";
			return record;
		} // method
	} // class

	public partial class RedstoneBlock  // typeof=RedstoneBlock
	{
		public override string Id { get; protected set; } = "minecraft:redstone_block";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:redstone_block";
			return record;
		} // method
	} // class

	public partial class RedstoneLamp  // typeof=RedstoneLamp
	{
		public override string Id { get; protected set; } = "minecraft:redstone_lamp";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:redstone_lamp";
			return record;
		} // method
	} // class

	public partial class RedstoneOre  // typeof=RedstoneOre
	{
		public override string Id { get; protected set; } = "minecraft:redstone_ore";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:redstone_ore";
			return record;
		} // method
	} // class

	public partial class RedstoneTorch  // typeof=RedstoneTorch
	{
		public override string Id { get; protected set; } = "minecraft:redstone_torch";

		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public override string TorchFacingDirection { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:redstone_torch";
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class RedstoneWire  // typeof=RedstoneWire
	{
		public override string Id { get; protected set; } = "minecraft:redstone_wire";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:redstone_wire";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class Reeds  // typeof=Reeds
	{
		public override string Id { get; protected set; } = "minecraft:reeds";

		[StateRange(0, 15)] public int Age { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:reeds";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class ReinforcedDeepslate : Block // typeof=ReinforcedDeepslate
	{

		public ReinforcedDeepslate() : base("minecraft:reinforced_deepslate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:reinforced_deepslate";
			return record;
		} // method
	} // class

	public partial class RepeatingCommandBlock : Block // typeof=RepeatingCommandBlock
	{
		[StateBit] public bool ConditionalBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public RepeatingCommandBlock() : base("minecraft:repeating_command_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "conditional_bit":
						ConditionalBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:repeating_command_block";
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Reserved6 : Block // typeof=Reserved6
	{

		public Reserved6() : base("minecraft:reserved6")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:reserved6";
			return record;
		} // method
	} // class

	public partial class RespawnAnchor : Block // typeof=RespawnAnchor
	{
		[StateRange(0, 4)] public int RespawnAnchorCharge { get; set; } = 0;

		public RespawnAnchor() : base("minecraft:respawn_anchor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "respawn_anchor_charge":
						RespawnAnchorCharge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:respawn_anchor";
			record.States.Add(new BlockStateInt { Name = "respawn_anchor_charge", Value = RespawnAnchorCharge });
			return record;
		} // method
	} // class

	public partial class Sand  // typeof=Sand
	{
		public override string Id { get; protected set; } = "minecraft:sand";

		[StateEnum("normal", "red")]
		public string SandType { get; set; } = "normal";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "sand_type":
						SandType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sand";
			record.States.Add(new BlockStateString { Name = "sand_type", Value = SandType });
			return record;
		} // method
	} // class

	public partial class Sandstone  // typeof=Sandstone
	{
		public override string Id { get; protected set; } = "minecraft:sandstone";

		[StateEnum("default", "heiroglyphs", "cut", "smooth")]
		public string SandStoneType { get; set; } = "default";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "sand_stone_type":
						SandStoneType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sandstone";
			record.States.Add(new BlockStateString { Name = "sand_stone_type", Value = SandStoneType });
			return record;
		} // method
	} // class

	public partial class SandstoneStairs  // typeof=SandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:sandstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sandstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Sapling  // typeof=Sapling
	{
		public override string Id { get; protected set; } = "minecraft:sapling";

		[StateBit] public bool AgeBit { get; set; } = false;
		[StateEnum("spruce", "birch", "jungle", "acacia", "dark_oak", "oak")]
		public string SaplingType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "age_bit":
						AgeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "sapling_type":
						SaplingType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sapling";
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "sapling_type", Value = SaplingType });
			return record;
		} // method
	} // class

	public partial class Scaffolding : Block // typeof=Scaffolding
	{
		[StateRange(0, 7)] public int Stability { get; set; } = 0;
		[StateBit] public bool StabilityCheck { get; set; } = false;

		public Scaffolding() : base("minecraft:scaffolding")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "stability":
						Stability = s.Value;
						break;
					case BlockStateByte s when s.Name == "stability_check":
						StabilityCheck = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:scaffolding";
			record.States.Add(new BlockStateInt { Name = "stability", Value = Stability });
			record.States.Add(new BlockStateByte { Name = "stability_check", Value = Convert.ToByte(StabilityCheck) });
			return record;
		} // method
	} // class

	public partial class Sculk : Block // typeof=Sculk
	{

		public Sculk() : base("minecraft:sculk")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sculk";
			return record;
		} // method
	} // class

	public partial class SculkCatalyst : Block // typeof=SculkCatalyst
	{
		[StateBit] public bool Bloom { get; set; } = false;

		public SculkCatalyst() : base("minecraft:sculk_catalyst")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "bloom":
						Bloom = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sculk_catalyst";
			record.States.Add(new BlockStateByte { Name = "bloom", Value = Convert.ToByte(Bloom) });
			return record;
		} // method
	} // class

	public partial class SculkSensor : Block // typeof=SculkSensor
	{
		[StateBit] public bool PoweredBit { get; set; } = false;

		public SculkSensor() : base("minecraft:sculk_sensor")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sculk_sensor";
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class SculkShrieker : Block // typeof=SculkShrieker
	{
		[StateBit] public bool Active { get; set; } = false;
		[StateBit] public bool CanSummon { get; set; } = false;

		public SculkShrieker() : base("minecraft:sculk_shrieker")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "active":
						Active = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "can_summon":
						CanSummon = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sculk_shrieker";
			record.States.Add(new BlockStateByte { Name = "active", Value = Convert.ToByte(Active) });
			record.States.Add(new BlockStateByte { Name = "can_summon", Value = Convert.ToByte(CanSummon) });
			return record;
		} // method
	} // class

	public partial class SculkVein : Block // typeof=SculkVein
	{
		[StateRange(0, 63)] public int MultiFaceDirectionBits { get; set; } = 0;

		public SculkVein() : base("minecraft:sculk_vein")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "multi_face_direction_bits":
						MultiFaceDirectionBits = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sculk_vein";
			record.States.Add(new BlockStateInt { Name = "multi_face_direction_bits", Value = MultiFaceDirectionBits });
			return record;
		} // method
	} // class

	public partial class SeaLantern  // typeof=SeaLantern
	{
		public override string Id { get; protected set; } = "minecraft:sea_lantern";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sea_lantern";
			return record;
		} // method
	} // class

	public partial class SeaPickle : Block // typeof=SeaPickle
	{
		[StateRange(0, 3)] public int ClusterCount { get; set; } = 0;
		[StateBit] public bool DeadBit { get; set; } = false;

		public SeaPickle() : base("minecraft:sea_pickle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "cluster_count":
						ClusterCount = s.Value;
						break;
					case BlockStateByte s when s.Name == "dead_bit":
						DeadBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sea_pickle";
			record.States.Add(new BlockStateInt { Name = "cluster_count", Value = ClusterCount });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class Seagrass : Block // typeof=Seagrass
	{
		[StateEnum("double_top", "double_bot", "default")]
		public string SeaGrassType { get; set; } = "";

		public Seagrass() : base("minecraft:seagrass")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "sea_grass_type":
						SeaGrassType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:seagrass";
			record.States.Add(new BlockStateString { Name = "sea_grass_type", Value = SeaGrassType });
			return record;
		} // method
	} // class

	public partial class Shroomlight : Block // typeof=Shroomlight
	{

		public Shroomlight() : base("minecraft:shroomlight")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:shroomlight";
			return record;
		} // method
	} // class

	public partial class ShulkerBox  // typeof=ShulkerBox
	{
		public override string Id { get; protected set; } = "minecraft:shulker_box";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:shulker_box";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class SilverGlazedTerracotta  // typeof=SilverGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:silver_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:silver_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Skull  // typeof=Skull
	{
		public override string Id { get; protected set; } = "minecraft:skull";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:skull";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Slime  // typeof=Slime
	{
		public override string Id { get; protected set; } = "minecraft:slime";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:slime";
			return record;
		} // method
	} // class

	public partial class SmallAmethystBud : Block // typeof=SmallAmethystBud
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public SmallAmethystBud() : base("minecraft:small_amethyst_bud")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:small_amethyst_bud";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SmallDripleafBlock : Block // typeof=SmallDripleafBlock
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public SmallDripleafBlock() : base("minecraft:small_dripleaf_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:small_dripleaf_block";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class SmithingTable : Block // typeof=SmithingTable
	{

		public SmithingTable() : base("minecraft:smithing_table")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smithing_table";
			return record;
		} // method
	} // class

	public partial class Smoker : Block // typeof=Smoker
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public Smoker() : base("minecraft:smoker")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smoker";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SmoothBasalt : Block // typeof=SmoothBasalt
	{

		public SmoothBasalt() : base("minecraft:smooth_basalt")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smooth_basalt";
			return record;
		} // method
	} // class

	public partial class SmoothQuartzStairs  // typeof=SmoothQuartzStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_quartz_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smooth_quartz_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothRedSandstoneStairs  // typeof=SmoothRedSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_red_sandstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smooth_red_sandstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothSandstoneStairs  // typeof=SmoothSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_sandstone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smooth_sandstone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothStone : Block // typeof=SmoothStone
	{

		public SmoothStone() : base("minecraft:smooth_stone")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:smooth_stone";
			return record;
		} // method
	} // class

	public partial class Snow  // typeof=Snow
	{
		public override string Id { get; protected set; } = "minecraft:snow";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:snow";
			return record;
		} // method
	} // class

	public partial class SnowLayer  // typeof=SnowLayer
	{
		public override string Id { get; protected set; } = "minecraft:snow_layer";

		[StateBit] public bool CoveredBit { get; set; } = false;
		[StateRange(0, 7)] public int Height { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "covered_bit":
						CoveredBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "height":
						Height = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:snow_layer";
			record.States.Add(new BlockStateByte { Name = "covered_bit", Value = Convert.ToByte(CoveredBit) });
			record.States.Add(new BlockStateInt { Name = "height", Value = Height });
			return record;
		} // method
	} // class

	public partial class SoulCampfire : Block // typeof=SoulCampfire
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool Extinguished { get; set; } = false;

		public SoulCampfire() : base("minecraft:soul_campfire")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "extinguished":
						Extinguished = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_campfire";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "extinguished", Value = Convert.ToByte(Extinguished) });
			return record;
		} // method
	} // class

	public partial class SoulFire : Block // typeof=SoulFire
	{
		[StateRange(0, 15)] public int Age { get; set; } = 0;

		public SoulFire() : base("minecraft:soul_fire")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "age":
						Age = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_fire";
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class SoulLantern : Block // typeof=SoulLantern
	{
		[StateBit] public bool Hanging { get; set; } = false;

		public SoulLantern() : base("minecraft:soul_lantern")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_lantern";
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SoulSand  // typeof=SoulSand
	{
		public override string Id { get; protected set; } = "minecraft:soul_sand";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_sand";
			return record;
		} // method
	} // class

	public partial class SoulSoil : Block // typeof=SoulSoil
	{

		public SoulSoil() : base("minecraft:soul_soil")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_soil";
			return record;
		} // method
	} // class

	public partial class SoulTorch : Block // typeof=SoulTorch
	{
		[StateEnum("unknown", "west", "east", "north", "south", "top")]
		public string TorchFacingDirection { get; set; } = "unknown";

		public SoulTorch() : base("minecraft:soul_torch")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:soul_torch";
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class Sponge  // typeof=Sponge
	{
		public override string Id { get; protected set; } = "minecraft:sponge";

		[StateEnum("dry", "wet")]
		public string SpongeType { get; set; } = "dry";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "sponge_type":
						SpongeType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sponge";
			record.States.Add(new BlockStateString { Name = "sponge_type", Value = SpongeType });
			return record;
		} // method
	} // class

	public partial class SporeBlossom : Block // typeof=SporeBlossom
	{

		public SporeBlossom() : base("minecraft:spore_blossom")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spore_blossom";
			return record;
		} // method
	} // class

	public partial class SpruceButton  // typeof=SpruceButton
	{
		public override string Id { get; protected set; } = "minecraft:spruce_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SpruceDoor  // typeof=SpruceDoor
	{
		public override string Id { get; protected set; } = "minecraft:spruce_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class SpruceFence : Block // typeof=SpruceFence
	{

		public SpruceFence() : base("minecraft:spruce_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_fence";
			return record;
		} // method
	} // class

	public partial class SpruceFenceGate  // typeof=SpruceFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:spruce_fence_gate";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class SpruceHangingSign : Block // typeof=SpruceHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public SpruceHangingSign() : base("minecraft:spruce_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SpruceLog : LogBase // typeof=SpruceLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

		public SpruceLog() : base("minecraft:spruce_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class SprucePressurePlate : Block // typeof=SprucePressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public SprucePressurePlate() : base("minecraft:spruce_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class SpruceStairs  // typeof=SpruceStairs
	{
		public override string Id { get; protected set; } = "minecraft:spruce_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SpruceStandingSign  // typeof=SpruceStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:spruce_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class SpruceTrapdoor  // typeof=SpruceTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:spruce_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class SpruceWallSign  // typeof=SpruceWallSign
	{
		public override string Id { get; protected set; } = "minecraft:spruce_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:spruce_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StainedGlass  // typeof=StainedGlass
	{
		public override string Id { get; protected set; } = "minecraft:stained_glass";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stained_glass";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StainedGlassPane  // typeof=StainedGlassPane
	{
		public override string Id { get; protected set; } = "minecraft:stained_glass_pane";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stained_glass_pane";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StainedHardenedClay  // typeof=StainedHardenedClay
	{
		public override string Id { get; protected set; } = "minecraft:stained_hardened_clay";

		[StateEnum("white", "orange", "magenta", "light_blue", "yellow", "lime", "pink", "gray", "silver", "cyan", "purple", "blue", "brown", "green", "red", "black")]
		public string Color { get; set; } = "white";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "color":
						Color = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stained_hardened_clay";
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StandingBanner  // typeof=StandingBanner
	{
		public override string Id { get; protected set; } = "minecraft:standing_banner";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:standing_banner";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class StandingSign  // typeof=StandingSign
	{
		public override string Id { get; protected set; } = "minecraft:standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class StickyPiston  // typeof=StickyPiston
	{
		public override string Id { get; protected set; } = "minecraft:sticky_piston";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sticky_piston";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StickyPistonArmCollision : Block // typeof=StickyPistonArmCollision
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public StickyPistonArmCollision() : base("minecraft:sticky_piston_arm_collision")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sticky_piston_arm_collision";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Stone  // typeof=Stone
	{
		public override string Id { get; protected set; } = "minecraft:stone";

		[StateEnum("granite", "granite_smooth", "diorite", "diorite_smooth", "andesite", "andesite_smooth", "stone")]
		public string StoneType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_type":
						StoneType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone";
			record.States.Add(new BlockStateString { Name = "stone_type", Value = StoneType });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab : SlabBase // typeof=StoneBlockSlab
	{
		[StateEnum("smooth_stone", "sandstone", "wood", "cobblestone", "brick", "stone_brick", "quartz", "nether_brick")]
		public string StoneSlabType { get; set; } = "smooth_stone";
		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public StoneBlockSlab() : base("minecraft:stone_block_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type":
						StoneSlabType = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_block_slab";
			record.States.Add(new BlockStateString { Name = "stone_slab_type", Value = StoneSlabType });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab2 : SlabBase // typeof=StoneBlockSlab2
	{
		[StateEnum("red_sandstone", "purpur", "prismarine_rough", "prismarine_dark", "prismarine_brick", "mossy_cobblestone", "smooth_sandstone", "red_nether_brick")]
		public string StoneSlabType2 { get; set; } = "red_sandstone";
		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public StoneBlockSlab2() : base("minecraft:stone_block_slab2")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_2":
						StoneSlabType2 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_block_slab2";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_2", Value = StoneSlabType2 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab3 : SlabBase // typeof=StoneBlockSlab3
	{
		[StateEnum("end_stone_brick", "smooth_red_sandstone", "polished_andesite", "andesite", "diorite", "polished_diorite", "granite", "polished_granite")]
		public string StoneSlabType3 { get; set; } = "end_stone_brick";
		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public StoneBlockSlab3() : base("minecraft:stone_block_slab3")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_3":
						StoneSlabType3 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_block_slab3";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_3", Value = StoneSlabType3 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab4 : SlabBase // typeof=StoneBlockSlab4
	{
		[StateEnum("smooth_quartz", "stone", "cut_sandstone", "cut_red_sandstone", "mossy_stone_brick")]
		public string StoneSlabType4 { get; set; } = "";
		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public StoneBlockSlab4() : base("minecraft:stone_block_slab4")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_slab_type_4":
						StoneSlabType4 = s.Value;
						break;
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_block_slab4";
			record.States.Add(new BlockStateString { Name = "stone_slab_type_4", Value = StoneSlabType4 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBrickStairs  // typeof=StoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:stone_brick_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_brick_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class StoneButton  // typeof=StoneButton
	{
		public override string Id { get; protected set; } = "minecraft:stone_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StonePressurePlate  // typeof=StonePressurePlate
	{
		public override string Id { get; protected set; } = "minecraft:stone_pressure_plate";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class StoneStairs  // typeof=StoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:stone_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stone_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Stonebrick  // typeof=Stonebrick
	{
		public override string Id { get; protected set; } = "minecraft:stonebrick";

		[StateEnum("mossy", "cracked", "chiseled", "smooth", "default")]
		public string StoneBrickType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "stone_brick_type":
						StoneBrickType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stonebrick";
			record.States.Add(new BlockStateString { Name = "stone_brick_type", Value = StoneBrickType });
			return record;
		} // method
	} // class

	public partial class Stonecutter  // typeof=Stonecutter
	{
		public override string Id { get; protected set; } = "minecraft:stonecutter";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stonecutter";
			return record;
		} // method
	} // class

	public partial class StonecutterBlock : Block // typeof=StonecutterBlock
	{
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public StonecutterBlock() : base("minecraft:stonecutter_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stonecutter_block";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StrippedAcaciaLog : LogBase // typeof=StrippedAcaciaLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedAcaciaLog() : base("minecraft:stripped_acacia_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_acacia_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedBambooBlock : Block // typeof=StrippedBambooBlock
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedBambooBlock() : base("minecraft:stripped_bamboo_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_bamboo_block";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedBirchLog : LogBase // typeof=StrippedBirchLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedBirchLog() : base("minecraft:stripped_birch_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_birch_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCherryLog : LogBase // typeof=StrippedCherryLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		public StrippedCherryLog() : base("minecraft:stripped_cherry_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_cherry_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCherryWood : Block // typeof=StrippedCherryWood
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedCherryWood() : base("minecraft:stripped_cherry_wood")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_cherry_wood";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCrimsonHyphae : Block // typeof=StrippedCrimsonHyphae
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedCrimsonHyphae() : base("minecraft:stripped_crimson_hyphae")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_crimson_hyphae";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCrimsonStem : Block // typeof=StrippedCrimsonStem
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedCrimsonStem() : base("minecraft:stripped_crimson_stem")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_crimson_stem";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedDarkOakLog : LogBase // typeof=StrippedDarkOakLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedDarkOakLog() : base("minecraft:stripped_dark_oak_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_dark_oak_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedJungleLog : LogBase // typeof=StrippedJungleLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedJungleLog() : base("minecraft:stripped_jungle_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_jungle_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedMangroveLog : LogBase // typeof=StrippedMangroveLog
	{
		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		public StrippedMangroveLog() : base("minecraft:stripped_mangrove_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_mangrove_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedMangroveWood : Block // typeof=StrippedMangroveWood
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedMangroveWood() : base("minecraft:stripped_mangrove_wood")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_mangrove_wood";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedOakLog : LogBase // typeof=StrippedOakLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedOakLog() : base("minecraft:stripped_oak_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_oak_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedSpruceLog : LogBase // typeof=StrippedSpruceLog
	{
		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

		public StrippedSpruceLog() : base("minecraft:stripped_spruce_log")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_spruce_log";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedWarpedHyphae : Block // typeof=StrippedWarpedHyphae
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedWarpedHyphae() : base("minecraft:stripped_warped_hyphae")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_warped_hyphae";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedWarpedStem : Block // typeof=StrippedWarpedStem
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public StrippedWarpedStem() : base("minecraft:stripped_warped_stem")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:stripped_warped_stem";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StructureBlock  // typeof=StructureBlock
	{
		public override string Id { get; protected set; } = "minecraft:structure_block";

		[StateEnum("save", "load", "corner", "invalid", "export", "data")]
		public string StructureBlockType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "structure_block_type":
						StructureBlockType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:structure_block";
			record.States.Add(new BlockStateString { Name = "structure_block_type", Value = StructureBlockType });
			return record;
		} // method
	} // class

	public partial class StructureVoid : Block // typeof=StructureVoid
	{
		[StateEnum("void", "air")]
		public string StructureVoidType { get; set; } = "void";

		public StructureVoid() : base("minecraft:structure_void")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "structure_void_type":
						StructureVoidType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:structure_void";
			record.States.Add(new BlockStateString { Name = "structure_void_type", Value = StructureVoidType });
			return record;
		} // method
	} // class

	public partial class SuspiciousGravel : Block // typeof=SuspiciousGravel
	{
		[StateRange(0, 3)] public int BrushedProgress { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public SuspiciousGravel() : base("minecraft:suspicious_gravel")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "brushed_progress":
						BrushedProgress = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:suspicious_gravel";
			record.States.Add(new BlockStateInt { Name = "brushed_progress", Value = BrushedProgress });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SuspiciousSand : Block // typeof=SuspiciousSand
	{
		[StateRange(0, 3)] public int BrushedProgress { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public SuspiciousSand() : base("minecraft:suspicious_sand")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "brushed_progress":
						BrushedProgress = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:suspicious_sand";
			record.States.Add(new BlockStateInt { Name = "brushed_progress", Value = BrushedProgress });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SweetBerryBush : Block // typeof=SweetBerryBush
	{
		[StateRange(0, 7)] public int Growth { get; set; } = 0;

		public SweetBerryBush() : base("minecraft:sweet_berry_bush")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:sweet_berry_bush";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Tallgrass  // typeof=Tallgrass
	{
		public override string Id { get; protected set; } = "minecraft:tallgrass";

		[StateEnum("default", "tall", "fern", "snow")]
		public string TallGrassType { get; set; } = "default";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "tall_grass_type":
						TallGrassType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:tallgrass";
			record.States.Add(new BlockStateString { Name = "tall_grass_type", Value = TallGrassType });
			return record;
		} // method
	} // class

	public partial class Target : Block // typeof=Target
	{

		public Target() : base("minecraft:target")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:target";
			return record;
		} // method
	} // class

	public partial class TintedGlass : Block // typeof=TintedGlass
	{

		public TintedGlass() : base("minecraft:tinted_glass")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:tinted_glass";
			return record;
		} // method
	} // class

	public partial class Tnt  // typeof=Tnt
	{
		public override string Id { get; protected set; } = "minecraft:tnt";

		[StateBit] public bool AllowUnderwaterBit { get; set; } = false;
		[StateBit] public bool ExplodeBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "allow_underwater_bit":
						AllowUnderwaterBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "explode_bit":
						ExplodeBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:tnt";
			record.States.Add(new BlockStateByte { Name = "allow_underwater_bit", Value = Convert.ToByte(AllowUnderwaterBit) });
			record.States.Add(new BlockStateByte { Name = "explode_bit", Value = Convert.ToByte(ExplodeBit) });
			return record;
		} // method
	} // class

	public partial class Torch  // typeof=Torch
	{
		public override string Id { get; protected set; } = "minecraft:torch";

		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:torch";
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class Torchflower : Block // typeof=Torchflower
	{

		public Torchflower() : base("minecraft:torchflower")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:torchflower";
			return record;
		} // method
	} // class

	public partial class TorchflowerCrop : Block // typeof=TorchflowerCrop
	{
		[StateRange(0, 7)] public int Growth { get; set; } = 0;

		public TorchflowerCrop() : base("minecraft:torchflower_crop")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:torchflower_crop";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Trapdoor  // typeof=Trapdoor
	{
		public override string Id { get; protected set; } = "minecraft:trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class TrappedChest  // typeof=TrappedChest
	{
		public override string Id { get; protected set; } = "minecraft:trapped_chest";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:trapped_chest";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class TripWire  // typeof=TripWire
	{
		public override string Id { get; protected set; } = "minecraft:trip_wire";

		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateBit] public bool DisarmedBit { get; set; } = false;
		[StateBit] public bool PoweredBit { get; set; } = false;
		[StateBit] public bool SuspendedBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "disarmed_bit":
						DisarmedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "suspended_bit":
						SuspendedBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:trip_wire";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateByte { Name = "disarmed_bit", Value = Convert.ToByte(DisarmedBit) });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			record.States.Add(new BlockStateByte { Name = "suspended_bit", Value = Convert.ToByte(SuspendedBit) });
			return record;
		} // method
	} // class

	public partial class TripwireHook  // typeof=TripwireHook
	{
		public override string Id { get; protected set; } = "minecraft:tripwire_hook";

		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool PoweredBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "powered_bit":
						PoweredBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:tripwire_hook";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Tuff : Block // typeof=Tuff
	{

		public Tuff() : base("minecraft:tuff")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:tuff";
			return record;
		} // method
	} // class

	public partial class TurtleEgg : Block // typeof=TurtleEgg
	{
		[StateEnum("cracked", "max_cracked", "no_cracks")]
		public string CrackedState { get; set; } = "";
		[StateEnum("one_egg", "two_egg", "three_egg", "four_egg")]
		public string TurtleEggCount { get; set; } = "";

		public TurtleEgg() : base("minecraft:turtle_egg")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "cracked_state":
						CrackedState = s.Value;
						break;
					case BlockStateString s when s.Name == "turtle_egg_count":
						TurtleEggCount = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:turtle_egg";
			record.States.Add(new BlockStateString { Name = "cracked_state", Value = CrackedState });
			record.States.Add(new BlockStateString { Name = "turtle_egg_count", Value = TurtleEggCount });
			return record;
		} // method
	} // class

	public partial class TwistingVines : Block // typeof=TwistingVines
	{
		[StateRange(0, 25)] public int TwistingVinesAge { get; set; } = 0;

		public TwistingVines() : base("minecraft:twisting_vines")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "twisting_vines_age":
						TwistingVinesAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:twisting_vines";
			record.States.Add(new BlockStateInt { Name = "twisting_vines_age", Value = TwistingVinesAge });
			return record;
		} // method
	} // class

	public partial class UnderwaterTorch : Block // typeof=UnderwaterTorch
	{
		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

		public UnderwaterTorch() : base("minecraft:underwater_torch")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:underwater_torch";
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class UndyedShulkerBox  // typeof=UndyedShulkerBox
	{
		public override string Id { get; protected set; } = "minecraft:undyed_shulker_box";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:undyed_shulker_box";
			return record;
		} // method
	} // class

	public partial class Unknown : Block // typeof=Unknown
	{

		public Unknown() : base("minecraft:unknown")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:unknown";
			return record;
		} // method
	} // class

	public partial class UnlitRedstoneTorch  // typeof=UnlitRedstoneTorch
	{
		public override string Id { get; protected set; } = "minecraft:unlit_redstone_torch";

		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public override string TorchFacingDirection { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "torch_facing_direction":
						TorchFacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:unlit_redstone_torch";
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class UnpoweredComparator  // typeof=UnpoweredComparator
	{
		public override string Id { get; protected set; } = "minecraft:unpowered_comparator";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool OutputLitBit { get; set; } = false;
		[StateBit] public bool OutputSubtractBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "output_lit_bit":
						OutputLitBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "output_subtract_bit":
						OutputSubtractBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:unpowered_comparator";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit) });
			record.States.Add(new BlockStateByte { Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit) });
			return record;
		} // method
	} // class

	public partial class UnpoweredRepeater  // typeof=UnpoweredRepeater
	{
		public override string Id { get; protected set; } = "minecraft:unpowered_repeater";

		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateRange(0, 3)] public int RepeaterDelay { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateInt s when s.Name == "repeater_delay":
						RepeaterDelay = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:unpowered_repeater";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "repeater_delay", Value = RepeaterDelay });
			return record;
		} // method
	} // class

	public partial class VerdantFroglight : Block // typeof=VerdantFroglight
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public VerdantFroglight() : base("minecraft:verdant_froglight")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:verdant_froglight";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Vine  // typeof=Vine
	{
		public override string Id { get; protected set; } = "minecraft:vine";

		[StateRange(0, 15)] public int VineDirectionBits { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "vine_direction_bits":
						VineDirectionBits = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:vine";
			record.States.Add(new BlockStateInt { Name = "vine_direction_bits", Value = VineDirectionBits });
			return record;
		} // method
	} // class

	public partial class WallBanner  // typeof=WallBanner
	{
		public override string Id { get; protected set; } = "minecraft:wall_banner";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wall_banner";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WallSign  // typeof=WallSign
	{
		public override string Id { get; protected set; } = "minecraft:wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedButton : Block // typeof=WarpedButton
	{
		[StateBit] public bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public WarpedButton() : base("minecraft:warped_button")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedDoor : Block // typeof=WarpedDoor
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool DoorHingeBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;
		[StateBit] public bool UpperBlockBit { get; set; } = false;

		public WarpedDoor() : base("minecraft:warped_door")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class WarpedDoubleSlab  // typeof=WarpedDoubleSlab
	{
		public override string Id { get; protected set; } = "minecraft:warped_double_slab";

		[StateBit] public bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_double_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WarpedFence : Block // typeof=WarpedFence
	{

		public WarpedFence() : base("minecraft:warped_fence")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_fence";
			return record;
		} // method
	} // class

	public partial class WarpedFenceGate : Block // typeof=WarpedFenceGate
	{
		[StateRange(0, 3)] public int Direction { get; set; } = 0;
		[StateBit] public bool InWallBit { get; set; } = false;
		[StateBit] public bool OpenBit { get; set; } = false;

		public WarpedFenceGate() : base("minecraft:warped_fence_gate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "in_wall_bit":
						InWallBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_fence_gate";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class WarpedFungus : Block // typeof=WarpedFungus
	{

		public WarpedFungus() : base("minecraft:warped_fungus")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_fungus";
			return record;
		} // method
	} // class

	public partial class WarpedHangingSign : Block // typeof=WarpedHangingSign
	{
		[StateBit] public bool AttachedBit { get; set; } = false;
		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;
		[StateBit] public bool Hanging { get; set; } = false;

		public WarpedHangingSign() : base("minecraft:warped_hanging_sign")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "attached_bit":
						AttachedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
					case BlockStateByte s when s.Name == "hanging":
						Hanging = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_hanging_sign";
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class WarpedHyphae : Block // typeof=WarpedHyphae
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public WarpedHyphae() : base("minecraft:warped_hyphae")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_hyphae";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class WarpedNylium : Block // typeof=WarpedNylium
	{

		public WarpedNylium() : base("minecraft:warped_nylium")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_nylium";
			return record;
		} // method
	} // class

	public partial class WarpedPlanks : Block // typeof=WarpedPlanks
	{

		public WarpedPlanks() : base("minecraft:warped_planks")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_planks";
			return record;
		} // method
	} // class

	public partial class WarpedPressurePlate : Block // typeof=WarpedPressurePlate
	{
		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public WarpedPressurePlate() : base("minecraft:warped_pressure_plate")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class WarpedRoots : Block // typeof=WarpedRoots
	{

		public WarpedRoots() : base("minecraft:warped_roots")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_roots";
			return record;
		} // method
	} // class

	public partial class WarpedSlab  // typeof=WarpedSlab
	{
		public override string Id { get; protected set; } = "minecraft:warped_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WarpedStairs  // typeof=WarpedStairs
	{
		public override string Id { get; protected set; } = "minecraft:warped_stairs";

		[StateBit] public override bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WarpedStandingSign  // typeof=WarpedStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:warped_standing_sign";

		[StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "ground_sign_direction":
						GroundSignDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_standing_sign";
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class WarpedStem : Block // typeof=WarpedStem
	{
		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

		public WarpedStem() : base("minecraft:warped_stem")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_stem";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class WarpedTrapdoor  // typeof=WarpedTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:warped_trapdoor";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpsideDownBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_trapdoor";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class WarpedWallSign  // typeof=WarpedWallSign
	{
		public override string Id { get; protected set; } = "minecraft:warped_wall_sign";

		[StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_wall_sign";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedWartBlock : Block // typeof=WarpedWartBlock
	{

		public WarpedWartBlock() : base("minecraft:warped_wart_block")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:warped_wart_block";
			return record;
		} // method
	} // class

	public partial class Water  // typeof=Water
	{
		public override string Id { get; protected set; } = "minecraft:water";

		[StateRange(0, 15)] public override int LiquidDepth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "liquid_depth":
						LiquidDepth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:water";
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class Waterlily  // typeof=Waterlily
	{
		public override string Id { get; protected set; } = "minecraft:waterlily";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waterlily";
			return record;
		} // method
	} // class

	public partial class WaxedCopper : Block // typeof=WaxedCopper
	{

		public WaxedCopper() : base("minecraft:waxed_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_copper";
			return record;
		} // method
	} // class

	public partial class WaxedCutCopper : Block // typeof=WaxedCutCopper
	{

		public WaxedCutCopper() : base("minecraft:waxed_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_cut_copper";
			return record;
		} // method
	} // class

	public partial class WaxedCutCopperSlab : Block // typeof=WaxedCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedCutCopperSlab() : base("minecraft:waxed_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedCutCopperStairs : Block // typeof=WaxedCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public WaxedCutCopperStairs() : base("minecraft:waxed_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedDoubleCutCopperSlab : Block // typeof=WaxedDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedDoubleCutCopperSlab() : base("minecraft:waxed_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedExposedCopper : Block // typeof=WaxedExposedCopper
	{

		public WaxedExposedCopper() : base("minecraft:waxed_exposed_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_exposed_copper";
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopper : Block // typeof=WaxedExposedCutCopper
	{

		public WaxedExposedCutCopper() : base("minecraft:waxed_exposed_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_exposed_cut_copper";
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopperSlab : Block // typeof=WaxedExposedCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedExposedCutCopperSlab() : base("minecraft:waxed_exposed_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_exposed_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopperStairs : Block // typeof=WaxedExposedCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public WaxedExposedCutCopperStairs() : base("minecraft:waxed_exposed_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_exposed_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedExposedDoubleCutCopperSlab : Block // typeof=WaxedExposedDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedExposedDoubleCutCopperSlab() : base("minecraft:waxed_exposed_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_exposed_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCopper : Block // typeof=WaxedOxidizedCopper
	{

		public WaxedOxidizedCopper() : base("minecraft:waxed_oxidized_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_oxidized_copper";
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopper : Block // typeof=WaxedOxidizedCutCopper
	{

		public WaxedOxidizedCutCopper() : base("minecraft:waxed_oxidized_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_oxidized_cut_copper";
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopperSlab : Block // typeof=WaxedOxidizedCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedOxidizedCutCopperSlab() : base("minecraft:waxed_oxidized_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_oxidized_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopperStairs : Block // typeof=WaxedOxidizedCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public WaxedOxidizedCutCopperStairs() : base("minecraft:waxed_oxidized_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_oxidized_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedDoubleCutCopperSlab : Block // typeof=WaxedOxidizedDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedOxidizedDoubleCutCopperSlab() : base("minecraft:waxed_oxidized_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_oxidized_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCopper : Block // typeof=WaxedWeatheredCopper
	{

		public WaxedWeatheredCopper() : base("minecraft:waxed_weathered_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_weathered_copper";
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopper : Block // typeof=WaxedWeatheredCutCopper
	{

		public WaxedWeatheredCutCopper() : base("minecraft:waxed_weathered_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_weathered_cut_copper";
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopperSlab : Block // typeof=WaxedWeatheredCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedWeatheredCutCopperSlab() : base("minecraft:waxed_weathered_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_weathered_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopperStairs : Block // typeof=WaxedWeatheredCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public WaxedWeatheredCutCopperStairs() : base("minecraft:waxed_weathered_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_weathered_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredDoubleCutCopperSlab : Block // typeof=WaxedWeatheredDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WaxedWeatheredDoubleCutCopperSlab() : base("minecraft:waxed_weathered_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:waxed_weathered_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WeatheredCopper : Block // typeof=WeatheredCopper
	{

		public WeatheredCopper() : base("minecraft:weathered_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weathered_copper";
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopper : Block // typeof=WeatheredCutCopper
	{

		public WeatheredCutCopper() : base("minecraft:weathered_cut_copper")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weathered_cut_copper";
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopperSlab : Block // typeof=WeatheredCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WeatheredCutCopperSlab() : base("minecraft:weathered_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weathered_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopperStairs : Block // typeof=WeatheredCutCopperStairs
	{
		[StateBit] public bool UpsideDownBit { get; set; } = false;
		[StateRange(0, 3)] public int WeirdoDirection { get; set; } = 0;

		public WeatheredCutCopperStairs() : base("minecraft:weathered_cut_copper_stairs")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "upside_down_bit":
						UpsideDownBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "weirdo_direction":
						WeirdoDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weathered_cut_copper_stairs";
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WeatheredDoubleCutCopperSlab : Block // typeof=WeatheredDoubleCutCopperSlab
	{
		[StateBit] public bool TopSlotBit { get; set; } = false;

		public WeatheredDoubleCutCopperSlab() : base("minecraft:weathered_double_cut_copper_slab")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weathered_double_cut_copper_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class Web  // typeof=Web
	{
		public override string Id { get; protected set; } = "minecraft:web";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:web";
			return record;
		} // method
	} // class

	public partial class WeepingVines : Block // typeof=WeepingVines
	{
		[StateRange(0, 25)] public int WeepingVinesAge { get; set; } = 0;

		public WeepingVines() : base("minecraft:weeping_vines")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "weeping_vines_age":
						WeepingVinesAge = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:weeping_vines";
			record.States.Add(new BlockStateInt { Name = "weeping_vines_age", Value = WeepingVinesAge });
			return record;
		} // method
	} // class

	public partial class Wheat  // typeof=Wheat
	{
		public override string Id { get; protected set; } = "minecraft:wheat";

		[StateRange(0, 7)] public override int Growth { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "growth":
						Growth = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wheat";
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class WhiteCandle : Block // typeof=WhiteCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public WhiteCandle() : base("minecraft:white_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:white_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class WhiteCandleCake : Block // typeof=WhiteCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public WhiteCandleCake() : base("minecraft:white_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:white_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class WhiteGlazedTerracotta  // typeof=WhiteGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:white_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:white_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WhiteWool : Block // typeof=WhiteWool
	{

		public WhiteWool() : base("minecraft:white_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:white_wool";
			return record;
		} // method
	} // class

	public partial class WitherRose : Block // typeof=WitherRose
	{

		public WitherRose() : base("minecraft:wither_rose")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wither_rose";
			return record;
		} // method
	} // class

	public partial class Wood : Block // typeof=Wood
	{
		[StateEnum("x", "z", "y")]
		public string PillarAxis { get; set; } = "x";
		[StateBit] public bool StrippedBit { get; set; } = false;
		[StateEnum("oak", "spruce", "birch", "jungle", "acacia", "dark_oak")]
		public string WoodType { get; set; } = "oak";

		public Wood() : base("minecraft:wood")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateString s when s.Name == "pillar_axis":
						PillarAxis = s.Value;
						break;
					case BlockStateByte s when s.Name == "stripped_bit":
						StrippedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "wood_type":
						WoodType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wood";
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class WoodenButton  // typeof=WoodenButton
	{
		public override string Id { get; protected set; } = "minecraft:wooden_button";

		[StateBit] public override bool ButtonPressedBit { get; set; } = false;
		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "button_pressed_bit":
						ButtonPressedBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wooden_button";
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WoodenDoor  // typeof=WoodenDoor
	{
		public override string Id { get; protected set; } = "minecraft:wooden_door";

		[StateRange(0, 3)] public override int Direction { get; set; } = 0;
		[StateBit] public override bool DoorHingeBit { get; set; } = false;
		[StateBit] public override bool OpenBit { get; set; } = false;
		[StateBit] public override bool UpperBlockBit { get; set; } = false;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "direction":
						Direction = s.Value;
						break;
					case BlockStateByte s when s.Name == "door_hinge_bit":
						DoorHingeBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "open_bit":
						OpenBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateByte s when s.Name == "upper_block_bit":
						UpperBlockBit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wooden_door";
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class WoodenPressurePlate  // typeof=WoodenPressurePlate
	{
		public override string Id { get; protected set; } = "minecraft:wooden_pressure_plate";

		[StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "redstone_signal":
						RedstoneSignal = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wooden_pressure_plate";
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class WoodenSlab  // typeof=WoodenSlab
	{
		public override string Id { get; protected set; } = "minecraft:wooden_slab";

		[StateBit] public override bool TopSlotBit { get; set; } = false;
		[StateEnum("spruce", "birch", "jungle", "acacia", "dark_oak", "oak")]
		public string WoodType { get; set; } = "";

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "top_slot_bit":
						TopSlotBit = Convert.ToBoolean(s.Value);
						break;
					case BlockStateString s when s.Name == "wood_type":
						WoodType = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:wooden_slab";
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class YellowCandle : Block // typeof=YellowCandle
	{
		[StateRange(0, 3)] public int Candles { get; set; } = 0;
		[StateBit] public bool Lit { get; set; } = false;

		public YellowCandle() : base("minecraft:yellow_candle")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "candles":
						Candles = s.Value;
						break;
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:yellow_candle";
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class YellowCandleCake : Block // typeof=YellowCandleCake
	{
		[StateBit] public bool Lit { get; set; } = false;

		public YellowCandleCake() : base("minecraft:yellow_candle_cake")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateByte s when s.Name == "lit":
						Lit = Convert.ToBoolean(s.Value);
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:yellow_candle_cake";
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class YellowFlower  // typeof=YellowFlower
	{
		public override string Id { get; protected set; } = "minecraft:yellow_flower";


		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:yellow_flower";
			return record;
		} // method
	} // class

	public partial class YellowGlazedTerracotta  // typeof=YellowGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:yellow_glazed_terracotta";

		[StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
					case BlockStateInt s when s.Name == "facing_direction":
						FacingDirection = s.Value;
						break;
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:yellow_glazed_terracotta";
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class YellowWool : Block // typeof=YellowWool
	{

		public YellowWool() : base("minecraft:yellow_wool")
		{
			IsGenerated = true;
		}

		public override void SetState(List<IBlockState> states)
		{
			foreach (var state in states)
			{
				switch (state)
				{
				} // switch
			} // foreach
		} // method

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = "minecraft:yellow_wool";
			return record;
		} // method
	} // class
}
