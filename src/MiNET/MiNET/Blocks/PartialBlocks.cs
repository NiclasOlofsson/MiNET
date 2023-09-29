using System;
using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Blocks
{

	public partial class AcaciaButton
	{
		public override string Id { get; protected set; } = "minecraft:acacia_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaDoor
	{
		public override string Id { get; protected set; } = "minecraft:acacia_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:acacia_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class AcaciaFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:acacia_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:acacia_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class AcaciaLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:acacia_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class AcaciaPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:acacia_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class AcaciaStairs
	{
		public override string Id { get; protected set; } = "minecraft:acacia_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:acacia_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class AcaciaTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:acacia_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class AcaciaWallSign
	{
		public override string Id { get; protected set; } = "minecraft:acacia_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ActivatorRail : Block
	{
		public override string Id { get; protected set; } = "minecraft:activator_rail";

		[StateBit]
		public bool RailDataBit { get; set; } = false;

		[StateRange(0, 5)]
		public int RailDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class Air : Block
	{
		public override string Id { get; protected set; } = "minecraft:air";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Allow : Block
	{
		public override string Id { get; protected set; } = "minecraft:allow";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class AmethystBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:amethyst_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class AmethystCluster : Block
	{
		public override string Id { get; protected set; } = "minecraft:amethyst_cluster";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class AncientDebris : Block
	{
		public override string Id { get; protected set; } = "minecraft:ancient_debris";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class AndesiteStairs
	{
		public override string Id { get; protected set; } = "minecraft:andesite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Anvil : Block
	{
		public override string Id { get; protected set; } = "minecraft:anvil";

		[StateEnum("undamaged", "slightly_damaged", "very_damaged", "broken")]
		public string Damage { get; set; } = "undamaged";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "damage", Value = Damage });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Azalea : Block
	{
		public override string Id { get; protected set; } = "minecraft:azalea";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class AzaleaLeaves : Block
	{
		public override string Id { get; protected set; } = "minecraft:azalea_leaves";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class AzaleaLeavesFlowered : Block
	{
		public override string Id { get; protected set; } = "minecraft:azalea_leaves_flowered";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Bamboo : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo";

		[StateBit]
		public bool AgeBit { get; set; } = false;

		[StateEnum("small_leaves", "large_leaves", "no_leaves")]
		public string BambooLeafSize { get; set; } = "";

		[StateEnum("thin", "thick")]
		public string BambooStalkThickness { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "bamboo_leaf_size", Value = BambooLeafSize });
			record.States.Add(new BlockStateString { Name = "bamboo_stalk_thickness", Value = BambooStalkThickness });
			return record;
		} // method
	} // class

	public partial class BambooBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_block";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class BambooButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BambooDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class BambooDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BambooFenceGate : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class BambooHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class BambooMosaic : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_mosaic";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BambooMosaicDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_mosaic_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooMosaicSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_mosaic_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooMosaicStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_mosaic_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BambooPlanks : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_planks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BambooPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class BambooSapling : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_sapling";

		[StateBit]
		public bool AgeBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "sapling_type", Value = SaplingType });
			return record;
		} // method
	} // class

	public partial class BambooSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BambooStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BambooStandingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class BambooTrapdoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_trapdoor";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class BambooWallSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:bamboo_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Barrel : Block
	{
		public override string Id { get; protected set; } = "minecraft:barrel";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class Barrier : Block
	{
		public override string Id { get; protected set; } = "minecraft:barrier";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Basalt : Block
	{
		public override string Id { get; protected set; } = "minecraft:basalt";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Beacon : Block
	{
		public override string Id { get; protected set; } = "minecraft:beacon";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Bed : Block
	{
		public override string Id { get; protected set; } = "minecraft:bed";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool HeadPieceBit { get; set; } = false;

		[StateBit]
		public bool OccupiedBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "head_piece_bit", Value = Convert.ToByte(HeadPieceBit) });
			record.States.Add(new BlockStateByte { Name = "occupied_bit", Value = Convert.ToByte(OccupiedBit) });
			return record;
		} // method
	} // class

	public partial class Bedrock : Block
	{
		public override string Id { get; protected set; } = "minecraft:bedrock";

		[StateBit]
		public bool InfiniburnBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "infiniburn_bit", Value = Convert.ToByte(InfiniburnBit) });
			return record;
		} // method
	} // class

	public partial class BeeNest : Block
	{
		public override string Id { get; protected set; } = "minecraft:bee_nest";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateRange(0, 5)]
		public int HoneyLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "honey_level", Value = HoneyLevel });
			return record;
		} // method
	} // class

	public partial class Beehive : Block
	{
		public override string Id { get; protected set; } = "minecraft:beehive";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateRange(0, 5)]
		public int HoneyLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "honey_level", Value = HoneyLevel });
			return record;
		} // method
	} // class

	public partial class Beetroot
	{
		public override string Id { get; protected set; } = "minecraft:beetroot";

		[StateRange(0, 7)]
		public override int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Bell : Block
	{
		public override string Id { get; protected set; } = "minecraft:bell";

		[StateEnum("standing", "hanging", "side", "multiple")]
		public string Attachment { get; set; } = "standing";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool ToggleBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "attachment", Value = Attachment });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "toggle_bit", Value = Convert.ToByte(ToggleBit) });
			return record;
		} // method
	} // class

	public partial class BigDripleaf : Block
	{
		public override string Id { get; protected set; } = "minecraft:big_dripleaf";

		[StateBit]
		public bool BigDripleafHead { get; set; } = false;

		[StateEnum("none", "unstable", "partial_tilt", "full_tilt")]
		public string BigDripleafTilt { get; set; } = "none";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "big_dripleaf_head", Value = Convert.ToByte(BigDripleafHead) });
			record.States.Add(new BlockStateString { Name = "big_dripleaf_tilt", Value = BigDripleafTilt });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class BirchButton
	{
		public override string Id { get; protected set; } = "minecraft:birch_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BirchDoor
	{
		public override string Id { get; protected set; } = "minecraft:birch_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class BirchFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:birch_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BirchFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:birch_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class BirchHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:birch_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class BirchLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:birch_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class BirchPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:birch_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class BirchStairs
	{
		public override string Id { get; protected set; } = "minecraft:birch_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BirchStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:birch_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class BirchTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:birch_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class BirchWallSign
	{
		public override string Id { get; protected set; } = "minecraft:birch_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlackCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:black_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlackCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:black_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlackGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:black_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlackWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:black_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Blackstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:blackstone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BlackstoneDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BlackstoneSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class BlackstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BlackstoneWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:blackstone_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class BlastFurnace
	{
		public override string Id { get; protected set; } = "minecraft:blast_furnace";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlueCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:blue_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlueCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:blue_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BlueGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:blue_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BlueIce : Block
	{
		public override string Id { get; protected set; } = "minecraft:blue_ice";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BlueWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:blue_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BoneBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:bone_block";

		[StateRange(0, 3)]
		public int Deprecated { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "deprecated", Value = Deprecated });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Bookshelf : Block
	{
		public override string Id { get; protected set; } = "minecraft:bookshelf";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BorderBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:border_block";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class BrewingStand : Block
	{
		public override string Id { get; protected set; } = "minecraft:brewing_stand";

		[StateBit]
		public bool BrewingStandSlotABit { get; set; } = false;

		[StateBit]
		public bool BrewingStandSlotBBit { get; set; } = false;

		[StateBit]
		public bool BrewingStandSlotCBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_a_bit", Value = Convert.ToByte(BrewingStandSlotABit) });
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_b_bit", Value = Convert.ToByte(BrewingStandSlotBBit) });
			record.States.Add(new BlockStateByte { Name = "brewing_stand_slot_c_bit", Value = Convert.ToByte(BrewingStandSlotCBit) });
			return record;
		} // method
	} // class

	public partial class BrickBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:brick_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class BrownCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:brown_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BrownCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:brown_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class BrownGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:brown_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class BrownMushroom : Block
	{
		public override string Id { get; protected set; } = "minecraft:brown_mushroom";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BrownMushroomBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:brown_mushroom_block";

		[StateRange(0, 15)]
		public int HugeMushroomBits { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "huge_mushroom_bits", Value = HugeMushroomBits });
			return record;
		} // method
	} // class

	public partial class BrownWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:brown_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class BubbleColumn : Block
	{
		public override string Id { get; protected set; } = "minecraft:bubble_column";

		[StateBit]
		public bool DragDown { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "drag_down", Value = Convert.ToByte(DragDown) });
			return record;
		} // method
	} // class

	public partial class BuddingAmethyst : Block
	{
		public override string Id { get; protected set; } = "minecraft:budding_amethyst";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Cactus : Block
	{
		public override string Id { get; protected set; } = "minecraft:cactus";

		[StateRange(0, 15)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class Cake : Block
	{
		public override string Id { get; protected set; } = "minecraft:cake";

		[StateRange(0, 6)]
		public int BiteCounter { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "bite_counter", Value = BiteCounter });
			return record;
		} // method
	} // class

	public partial class Calcite : Block
	{
		public override string Id { get; protected set; } = "minecraft:calcite";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CalibratedSculkSensor : Block
	{
		public override string Id { get; protected set; } = "minecraft:calibrated_sculk_sensor";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool PoweredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Camera : Block
	{
		public override string Id { get; protected set; } = "minecraft:camera";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Campfire : Block
	{
		public override string Id { get; protected set; } = "minecraft:campfire";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool Extinguished { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "extinguished", Value = Convert.ToByte(Extinguished) });
			return record;
		} // method
	} // class

	public partial class Candle : Block
	{
		public override string Id { get; protected set; } = "minecraft:candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class Carpet : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class Carrots
	{
		public override string Id { get; protected set; } = "minecraft:carrots";

		[StateRange(0, 7)]
		public override int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class CartographyTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:cartography_table";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CarvedPumpkin : Block
	{
		public override string Id { get; protected set; } = "minecraft:carved_pumpkin";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Cauldron : Block
	{
		public override string Id { get; protected set; } = "minecraft:cauldron";

		[StateEnum("water", "powder_snow", "lava")]
		public string CauldronLiquid { get; set; } = "water";

		[StateRange(0, 6)]
		public int FillLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "cauldron_liquid", Value = CauldronLiquid });
			record.States.Add(new BlockStateInt { Name = "fill_level", Value = FillLevel });
			return record;
		} // method
	} // class

	public partial class CaveVines : Block
	{
		public override string Id { get; protected set; } = "minecraft:cave_vines";

		[StateRange(0, 25)]
		public int GrowingPlantAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class CaveVinesBodyWithBerries : Block
	{
		public override string Id { get; protected set; } = "minecraft:cave_vines_body_with_berries";

		[StateRange(0, 25)]
		public int GrowingPlantAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class CaveVinesHeadWithBerries : Block
	{
		public override string Id { get; protected set; } = "minecraft:cave_vines_head_with_berries";

		[StateRange(0, 25)]
		public int GrowingPlantAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growing_plant_age", Value = GrowingPlantAge });
			return record;
		} // method
	} // class

	public partial class Chain : Block
	{
		public override string Id { get; protected set; } = "minecraft:chain";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class ChainCommandBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:chain_command_block";

		[StateBit]
		public bool ConditionalBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ChemicalHeat : Block
	{
		public override string Id { get; protected set; } = "minecraft:chemical_heat";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ChemistryTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:chemistry_table";

		[StateEnum("compound_creator", "material_reducer", "element_constructor", "lab_table")]
		public string ChemistryTableType { get; set; } = "compound_creator";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "chemistry_table_type", Value = ChemistryTableType });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class CherryButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CherryDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class CherryDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CherryFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CherryFenceGate : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class CherryHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class CherryLeaves : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_leaves";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class CherryLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:cherry_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CherryPlanks : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_planks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CherryPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class CherrySapling : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_sapling";

		[StateBit]
		public bool AgeBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			return record;
		} // method
	} // class

	public partial class CherrySlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:cherry_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CherryStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CherryStandingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class CherryTrapdoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_trapdoor";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class CherryWallSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:cherry_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CherryWood : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:cherry_wood";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		[StateBit]
		public bool StrippedBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			return record;
		} // method
	} // class

	public partial class Chest
	{
		public override string Id { get; protected set; } = "minecraft:chest";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ChiseledBookshelf : Block
	{
		public override string Id { get; protected set; } = "minecraft:chiseled_bookshelf";

		[StateRange(0, 63)]
		public int BooksStored { get; set; } = 0;

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "books_stored", Value = BooksStored });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class ChiseledDeepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:chiseled_deepslate";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ChiseledNetherBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:chiseled_nether_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ChiseledPolishedBlackstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:chiseled_polished_blackstone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ChorusFlower : Block
	{
		public override string Id { get; protected set; } = "minecraft:chorus_flower";

		[StateRange(0, 5)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class ChorusPlant : Block
	{
		public override string Id { get; protected set; } = "minecraft:chorus_plant";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Clay : Block
	{
		public override string Id { get; protected set; } = "minecraft:clay";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ClientRequestPlaceholderBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:client_request_placeholder_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CoalBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:coal_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CoalOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:coal_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CobbledDeepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:cobbled_deepslate";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:cobbled_deepslate_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:cobbled_deepslate_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:cobbled_deepslate_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CobbledDeepslateWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:cobbled_deepslate_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class Cobblestone : Block
	{
		public override string Id { get; protected set; } = "minecraft:cobblestone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CobblestoneWall : Block
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

		[StateBit]
		public bool WallPostBit { get; set; } = true;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_block_type", Value = WallBlockType });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class Cocoa : Block
	{
		public override string Id { get; protected set; } = "minecraft:cocoa";

		[StateRange(0, 2)]
		public int Age { get; set; } = 0;

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class ColoredTorchBp : Block
	{
		public override string Id { get; protected set; } = "minecraft:colored_torch_bp";

		[StateBit]
		public bool ColorBit { get; set; } = false;

		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "color_bit", Value = Convert.ToByte(ColorBit) });
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class ColoredTorchRg : Block
	{
		public override string Id { get; protected set; } = "minecraft:colored_torch_rg";

		[StateBit]
		public bool ColorBit { get; set; } = false;

		[StateEnum("west", "east", "north", "south", "top", "unknown")]
		public string TorchFacingDirection { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "color_bit", Value = Convert.ToByte(ColorBit) });
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class CommandBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:command_block";

		[StateBit]
		public bool ConditionalBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Composter : Block
	{
		public override string Id { get; protected set; } = "minecraft:composter";

		[StateRange(0, 8)]
		public int ComposterFillLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "composter_fill_level", Value = ComposterFillLevel });
			return record;
		} // method
	} // class

	public partial class Concrete : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class ConcretePowder : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class Conduit : Block
	{
		public override string Id { get; protected set; } = "minecraft:conduit";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CopperBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:copper_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CopperOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:copper_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Coral : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral";

		[StateEnum("blue", "pink", "purple", "red", "yellow")]
		public string CoralColor { get; set; } = "blue";

		[StateBit]
		public bool DeadBit { get; set; } = true;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_block";

		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";

		[StateBit]
		public bool DeadBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFan : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_fan";

		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";

		[StateRange(0, 1)]
		public int CoralFanDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateInt { Name = "coral_fan_direction", Value = CoralFanDirection });
			return record;
		} // method
	} // class

	public partial class CoralFanDead : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_fan_dead";

		[StateEnum("pink", "purple", "red", "yellow", "blue")]
		public string CoralColor { get; set; } = "";

		[StateRange(0, 1)]
		public int CoralFanDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "coral_color", Value = CoralColor });
			record.States.Add(new BlockStateInt { Name = "coral_fan_direction", Value = CoralFanDirection });
			return record;
		} // method
	} // class

	public partial class CoralFanHang : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_fan_hang";

		[StateRange(0, 3)]
		public int CoralDirection { get; set; } = 0;

		[StateBit]
		public bool CoralHangTypeBit { get; set; } = false;

		[StateBit]
		public bool DeadBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFanHang2 : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_fan_hang2";

		[StateRange(0, 3)]
		public int CoralDirection { get; set; } = 0;

		[StateBit]
		public bool CoralHangTypeBit { get; set; } = false;

		[StateBit]
		public bool DeadBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CoralFanHang3 : Block
	{
		public override string Id { get; protected set; } = "minecraft:coral_fan_hang3";

		[StateRange(0, 3)]
		public int CoralDirection { get; set; } = 0;

		[StateBit]
		public bool CoralHangTypeBit { get; set; } = false;

		[StateBit]
		public bool DeadBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "coral_direction", Value = CoralDirection });
			record.States.Add(new BlockStateByte { Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit) });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class CrackedDeepslateBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:cracked_deepslate_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrackedDeepslateTiles : Block
	{
		public override string Id { get; protected set; } = "minecraft:cracked_deepslate_tiles";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrackedNetherBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:cracked_nether_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrackedPolishedBlackstoneBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:cracked_polished_blackstone_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CraftingTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:crafting_table";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonFenceGate : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonFungus : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_fungus";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class CrimsonHyphae : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:crimson_hyphae";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CrimsonNylium : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_nylium";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonPlanks : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_planks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class CrimsonRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:crimson_roots";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CrimsonSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:crimson_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonStairs
	{
		public override string Id { get; protected set; } = "minecraft:crimson_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:crimson_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class CrimsonStem : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:crimson_stem";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class CrimsonTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:crimson_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class CrimsonWallSign
	{
		public override string Id { get; protected set; } = "minecraft:crimson_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CryingObsidian : Block
	{
		public override string Id { get; protected set; } = "minecraft:crying_obsidian";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class CutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class CutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class CyanCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:cyan_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CyanCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:cyan_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class CyanGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:cyan_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class CyanWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:cyan_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DarkOakButton
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class DarkOakDoor
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class DarkOakFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DarkOakFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class DarkOakHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class DarkOakLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class DarkOakPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class DarkOakStairs
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DarkOakTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:dark_oak_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class DarkPrismarineStairs
	{
		public override string Id { get; protected set; } = "minecraft:dark_prismarine_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DarkoakStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:darkoak_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class DarkoakWallSign
	{
		public override string Id { get; protected set; } = "minecraft:darkoak_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class DaylightDetector : Block
	{
		public override string Id { get; protected set; } = "minecraft:daylight_detector";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class DaylightDetectorInverted : Block
	{
		public override string Id { get; protected set; } = "minecraft:daylight_detector_inverted";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class Deadbush : Block
	{
		public override string Id { get; protected set; } = "minecraft:deadbush";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DecoratedPot : Block
	{
		public override string Id { get; protected set; } = "minecraft:decorated_pot";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class Deepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_brick_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_brick_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_brick_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DeepslateBrickWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_brick_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateCoalOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_coal_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateCopperOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_copper_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateDiamondOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_diamond_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateEmeraldOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_emerald_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateGoldOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_gold_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateIronOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_iron_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateLapisOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_lapis_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateRedstoneOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_redstone_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DeepslateTileDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_tile_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTileSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_tile_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTileStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_tile_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class DeepslateTileWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_tile_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class DeepslateTiles : Block
	{
		public override string Id { get; protected set; } = "minecraft:deepslate_tiles";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Deny : Block
	{
		public override string Id { get; protected set; } = "minecraft:deny";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DetectorRail : Block
	{
		public override string Id { get; protected set; } = "minecraft:detector_rail";

		[StateBit]
		public bool RailDataBit { get; set; } = false;

		[StateRange(0, 5)]
		public int RailDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class DiamondBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:diamond_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DiamondOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:diamond_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DioriteStairs
	{
		public override string Id { get; protected set; } = "minecraft:diorite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Dirt : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "dirt_type", Value = DirtType });
			return record;
		} // method
	} // class

	public partial class DirtWithRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:dirt_with_roots";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Dispenser : Block
	{
		public override string Id { get; protected set; } = "minecraft:dispenser";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool TriggeredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit) });
			return record;
		} // method
	} // class

	public partial class DoubleCutCopperSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:double_cut_copper_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoublePlant : Block
	{
		public override string Id { get; protected set; } = "minecraft:double_plant";

		[StateEnum("syringa", "grass", "fern", "rose", "paeonia", "sunflower")]
		public string DoublePlantType { get; set; } = "";

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "double_plant_type", Value = DoublePlantType });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:double_stone_block_slab";

		[StateEnum("smooth_stone", "sandstone", "wood", "cobblestone", "brick", "stone_brick", "quartz", "nether_brick")]
		public string StoneSlabType { get; set; } = "smooth_stone";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type", Value = StoneSlabType });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab2 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:double_stone_block_slab2";

		[StateEnum("red_sandstone", "purpur", "prismarine_rough", "prismarine_dark", "prismarine_brick", "mossy_cobblestone", "smooth_sandstone", "red_nether_brick")]
		public string StoneSlabType2 { get; set; } = "red_sandstone";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_2", Value = StoneSlabType2 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab3 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:double_stone_block_slab3";

		[StateEnum("end_stone_brick", "smooth_red_sandstone", "polished_andesite", "andesite", "diorite", "polished_diorite", "granite", "polished_granite")]
		public string StoneSlabType3 { get; set; } = "end_stone_brick";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_3", Value = StoneSlabType3 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleStoneBlockSlab4 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:double_stone_block_slab4";

		[StateEnum("smooth_quartz", "stone", "cut_sandstone", "cut_red_sandstone", "mossy_stone_brick")]
		public string StoneSlabType4 { get; set; } = "";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_4", Value = StoneSlabType4 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class DoubleWoodenSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:double_wooden_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class DragonEgg : Block
	{
		public override string Id { get; protected set; } = "minecraft:dragon_egg";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DriedKelpBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:dried_kelp_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class DripstoneBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:dripstone_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Dropper : Block
	{
		public override string Id { get; protected set; } = "minecraft:dropper";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool TriggeredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit) });
			return record;
		} // method
	} // class

	public partial class Element0 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_0";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element1 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_1";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element10 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_10";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element100 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_100";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element101 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_101";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element102 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_102";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element103 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_103";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element104 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_104";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element105 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_105";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element106 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_106";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element107 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_107";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element108 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_108";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element109 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_109";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element11 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_11";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element110 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_110";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element111 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_111";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element112 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_112";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element113 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_113";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element114 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_114";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element115 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_115";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element116 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_116";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element117 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_117";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element118 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_118";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element12 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_12";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element13 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_13";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element14 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_14";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element15 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_15";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element16 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_16";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element17 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_17";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element18 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_18";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element19 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_19";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element2 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_2";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element20 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_20";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element21 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_21";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element22 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_22";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element23 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_23";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element24 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_24";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element25 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_25";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element26 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_26";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element27 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_27";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element28 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_28";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element29 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_29";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element3 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_3";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element30 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_30";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element31 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_31";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element32 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_32";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element33 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_33";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element34 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_34";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element35 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_35";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element36 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_36";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element37 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_37";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element38 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_38";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element39 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_39";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element4 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_4";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element40 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_40";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element41 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_41";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element42 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_42";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element43 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_43";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element44 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_44";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element45 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_45";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element46 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_46";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element47 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_47";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element48 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_48";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element49 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_49";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element5 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_5";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element50 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_50";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element51 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_51";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element52 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_52";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element53 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_53";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element54 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_54";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element55 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_55";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element56 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_56";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element57 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_57";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element58 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_58";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element59 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_59";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element6 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_6";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element60 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_60";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element61 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_61";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element62 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_62";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element63 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_63";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element64 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_64";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element65 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_65";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element66 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_66";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element67 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_67";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element68 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_68";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element69 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_69";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element7 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_7";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element70 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_70";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element71 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_71";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element72 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_72";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element73 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_73";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element74 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_74";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element75 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_75";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element76 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_76";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element77 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_77";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element78 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_78";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element79 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_79";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element8 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_8";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element80 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_80";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element81 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_81";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element82 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_82";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element83 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_83";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element84 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_84";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element85 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_85";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element86 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_86";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element87 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_87";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element88 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_88";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element89 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_89";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element9 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_9";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element90 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_90";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element91 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_91";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element92 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_92";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element93 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_93";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element94 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_94";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element95 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_95";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element96 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_96";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element97 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_97";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element98 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_98";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Element99 : Block
	{
		public override string Id { get; protected set; } = "minecraft:element_99";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EmeraldBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:emerald_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EmeraldOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:emerald_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EnchantingTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:enchanting_table";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EndBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:end_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class EndBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EndGateway : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_gateway";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EndPortal : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_portal";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EndPortalFrame : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_portal_frame";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool EndPortalEyeBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "end_portal_eye_bit", Value = Convert.ToByte(EndPortalEyeBit) });
			return record;
		} // method
	} // class

	public partial class EndRod : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_rod";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class EndStone : Block
	{
		public override string Id { get; protected set; } = "minecraft:end_stone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class EnderChest
	{
		public override string Id { get; protected set; } = "minecraft:ender_chest";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class ExposedCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:exposed_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ExposedCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:exposed_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ExposedCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:exposed_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class ExposedCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:exposed_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class ExposedDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:exposed_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class Farmland : Block
	{
		public override string Id { get; protected set; } = "minecraft:farmland";

		[StateRange(0, 7)]
		public int MoisturizedAmount { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "moisturized_amount", Value = MoisturizedAmount });
			return record;
		} // method
	} // class

	public partial class FenceGate
	{
		public override string Id { get; protected set; } = "minecraft:fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class Fire : Block
	{
		public override string Id { get; protected set; } = "minecraft:fire";

		[StateRange(0, 15)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class FletchingTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:fletching_table";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class FlowerPot : Block
	{
		public override string Id { get; protected set; } = "minecraft:flower_pot";

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class FloweringAzalea : Block
	{
		public override string Id { get; protected set; } = "minecraft:flowering_azalea";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class FlowingLava
	{
		public override string Id { get; protected set; } = "minecraft:flowing_lava";

		[StateRange(0, 15)]
		public override int LiquidDepth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class FlowingWater
	{
		public override string Id { get; protected set; } = "minecraft:flowing_water";

		[StateRange(0, 15)]
		public override int LiquidDepth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class Frame : Block
	{
		public override string Id { get; protected set; } = "minecraft:frame";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool ItemFrameMapBit { get; set; } = false;

		[StateBit]
		public bool ItemFramePhotoBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "item_frame_map_bit", Value = Convert.ToByte(ItemFrameMapBit) });
			record.States.Add(new BlockStateByte { Name = "item_frame_photo_bit", Value = Convert.ToByte(ItemFramePhotoBit) });
			return record;
		} // method
	} // class

	public partial class FrogSpawn : Block
	{
		public override string Id { get; protected set; } = "minecraft:frog_spawn";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class FrostedIce : Block
	{
		public override string Id { get; protected set; } = "minecraft:frosted_ice";

		[StateRange(0, 3)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class Furnace
	{
		public override string Id { get; protected set; } = "minecraft:furnace";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GildedBlackstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:gilded_blackstone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Glass : Block
	{
		public override string Id { get; protected set; } = "minecraft:glass";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GlassPane : Block
	{
		public override string Id { get; protected set; } = "minecraft:glass_pane";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GlowFrame : Block
	{
		public override string Id { get; protected set; } = "minecraft:glow_frame";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool ItemFrameMapBit { get; set; } = false;

		[StateBit]
		public bool ItemFramePhotoBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "item_frame_map_bit", Value = Convert.ToByte(ItemFrameMapBit) });
			record.States.Add(new BlockStateByte { Name = "item_frame_photo_bit", Value = Convert.ToByte(ItemFramePhotoBit) });
			return record;
		} // method
	} // class

	public partial class GlowLichen : Block
	{
		public override string Id { get; protected set; } = "minecraft:glow_lichen";

		[StateRange(0, 63)]
		public int MultiFaceDirectionBits { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "multi_face_direction_bits", Value = MultiFaceDirectionBits });
			return record;
		} // method
	} // class

	public partial class Glowingobsidian : Block
	{
		public override string Id { get; protected set; } = "minecraft:glowingobsidian";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Glowstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:glowstone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GoldBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:gold_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GoldOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:gold_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GoldenRail : Block
	{
		public override string Id { get; protected set; } = "minecraft:golden_rail";

		[StateBit]
		public bool RailDataBit { get; set; } = false;

		[StateRange(0, 5)]
		public int RailDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit) });
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class GraniteStairs
	{
		public override string Id { get; protected set; } = "minecraft:granite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Grass : Block
	{
		public override string Id { get; protected set; } = "minecraft:grass";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GrassPath : Block
	{
		public override string Id { get; protected set; } = "minecraft:grass_path";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Gravel : Block
	{
		public override string Id { get; protected set; } = "minecraft:gravel";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GrayCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:gray_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GrayCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:gray_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GrayGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:gray_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GrayWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:gray_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class GreenCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:green_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GreenCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:green_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class GreenGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:green_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class GreenWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:green_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Grindstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:grindstone";

		[StateEnum("standing", "hanging", "side", "multiple")]
		public string Attachment { get; set; } = "standing";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "attachment", Value = Attachment });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class HangingRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:hanging_roots";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class HardGlass : Block
	{
		public override string Id { get; protected set; } = "minecraft:hard_glass";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class HardGlassPane : Block
	{
		public override string Id { get; protected set; } = "minecraft:hard_glass_pane";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class HardStainedGlass : Block
	{
		public override string Id { get; protected set; } = "minecraft:hard_stained_glass";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class HardStainedGlassPane : Block
	{
		public override string Id { get; protected set; } = "minecraft:hard_stained_glass_pane";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class HardenedClay : Block
	{
		public override string Id { get; protected set; } = "minecraft:hardened_clay";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class HayBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:hay_block";

		[StateRange(0, 3)]
		public int Deprecated { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "deprecated", Value = Deprecated });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class HeavyWeightedPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:heavy_weighted_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class HoneyBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:honey_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class HoneycombBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:honeycomb_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Hopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:hopper";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool ToggleBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "toggle_bit", Value = Convert.ToByte(ToggleBit) });
			return record;
		} // method
	} // class

	public partial class Ice : Block
	{
		public override string Id { get; protected set; } = "minecraft:ice";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class InfestedDeepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:infested_deepslate";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class InfoUpdate : Block
	{
		public override string Id { get; protected set; } = "minecraft:info_update";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class InfoUpdate2 : Block
	{
		public override string Id { get; protected set; } = "minecraft:info_update2";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class InvisibleBedrock : Block
	{
		public override string Id { get; protected set; } = "minecraft:invisible_bedrock";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class IronBars : Block
	{
		public override string Id { get; protected set; } = "minecraft:iron_bars";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class IronBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:iron_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class IronDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:iron_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 1;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class IronOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:iron_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class IronTrapdoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:iron_trapdoor";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class Jigsaw : Block
	{
		public override string Id { get; protected set; } = "minecraft:jigsaw";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 3)]
		public int Rotation { get; set; } = 1;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "rotation", Value = Rotation });
			return record;
		} // method
	} // class

	public partial class Jukebox : Block
	{
		public override string Id { get; protected set; } = "minecraft:jukebox";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class JungleButton
	{
		public override string Id { get; protected set; } = "minecraft:jungle_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class JungleDoor
	{
		public override string Id { get; protected set; } = "minecraft:jungle_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class JungleFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:jungle_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class JungleFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:jungle_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class JungleHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:jungle_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class JungleLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:jungle_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class JunglePressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:jungle_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class JungleStairs
	{
		public override string Id { get; protected set; } = "minecraft:jungle_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class JungleStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:jungle_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class JungleTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:jungle_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class JungleWallSign
	{
		public override string Id { get; protected set; } = "minecraft:jungle_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Kelp : Block
	{
		public override string Id { get; protected set; } = "minecraft:kelp";

		[StateRange(0, 25)]
		public int KelpAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "kelp_age", Value = KelpAge });
			return record;
		} // method
	} // class

	public partial class Ladder : Block
	{
		public override string Id { get; protected set; } = "minecraft:ladder";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lantern : Block
	{
		public override string Id { get; protected set; } = "minecraft:lantern";

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class LapisBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:lapis_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LapisOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:lapis_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LargeAmethystBud : Block
	{
		public override string Id { get; protected set; } = "minecraft:large_amethyst_bud";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lava
	{
		public override string Id { get; protected set; } = "minecraft:lava";

		[StateRange(0, 15)]
		public override int LiquidDepth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class LavaCauldron : Block
	{
		public override string Id { get; protected set; } = "minecraft:lava_cauldron";

		[StateEnum("water", "powder_snow", "lava")]
		public string CauldronLiquid { get; set; } = "water";

		[StateRange(0, 6)]
		public int FillLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "cauldron_liquid", Value = CauldronLiquid });
			record.States.Add(new BlockStateInt { Name = "fill_level", Value = FillLevel });
			return record;
		} // method
	} // class

	public partial class Leaves : Block
	{
		public override string Id { get; protected set; } = "minecraft:leaves";

		[StateEnum("oak", "spruce", "birch", "jungle")]
		public string OldLeafType { get; set; } = "oak";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "old_leaf_type", Value = OldLeafType });
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Leaves2 : Block
	{
		public override string Id { get; protected set; } = "minecraft:leaves2";

		[StateEnum("dark_oak", "acacia")]
		public string NewLeafType { get; set; } = "";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "new_leaf_type", Value = NewLeafType });
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class Lectern : Block
	{
		public override string Id { get; protected set; } = "minecraft:lectern";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool PoweredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Lever : Block
	{
		public override string Id { get; protected set; } = "minecraft:lever";

		[StateEnum("down_east_west", "east", "west", "south", "north", "up_north_south", "up_east_west", "down_north_south")]
		public string LeverDirection { get; set; } = "down_east_west";

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "lever_direction", Value = LeverDirection });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class LightBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_block";

		[StateRange(0, 15)]
		public int BlockLightLevel { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "block_light_level", Value = BlockLightLevel });
			return record;
		} // method
	} // class

	public partial class LightBlueCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_blue_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightBlueCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_blue_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightBlueGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:light_blue_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LightBlueWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_blue_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LightGrayCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_gray_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightGrayCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_gray_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LightGrayWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_gray_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LightWeightedPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:light_weighted_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class LightningRod : Block
	{
		public override string Id { get; protected set; } = "minecraft:lightning_rod";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LimeCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:lime_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LimeCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:lime_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class LimeGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:lime_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LimeWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:lime_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LitBlastFurnace
	{
		public override string Id { get; protected set; } = "minecraft:lit_blast_furnace";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LitDeepslateRedstoneOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:lit_deepslate_redstone_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LitFurnace
	{
		public override string Id { get; protected set; } = "minecraft:lit_furnace";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class LitPumpkin : Block
	{
		public override string Id { get; protected set; } = "minecraft:lit_pumpkin";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class LitRedstoneLamp
	{
		public override string Id { get; protected set; } = "minecraft:lit_redstone_lamp";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LitRedstoneOre
	{
		public override string Id { get; protected set; } = "minecraft:lit_redstone_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class LitSmoker : Block
	{
		public override string Id { get; protected set; } = "minecraft:lit_smoker";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Lodestone : Block
	{
		public override string Id { get; protected set; } = "minecraft:lodestone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Loom : Block
	{
		public override string Id { get; protected set; } = "minecraft:loom";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class MagentaCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:magenta_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class MagentaCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:magenta_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class MagentaGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:magenta_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MagentaWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:magenta_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Magma : Block
	{
		public override string Id { get; protected set; } = "minecraft:magma";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MangroveButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MangroveDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class MangroveDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MangroveFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MangroveFenceGate : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class MangroveHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class MangroveLeaves : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_leaves";

		[StateBit]
		public bool PersistentBit { get; set; } = false;

		[StateBit]
		public bool UpdateBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "persistent_bit", Value = Convert.ToByte(PersistentBit) });
			record.States.Add(new BlockStateByte { Name = "update_bit", Value = Convert.ToByte(UpdateBit) });
			return record;
		} // method
	} // class

	public partial class MangroveLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class MangrovePlanks : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_planks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MangrovePressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class MangrovePropagule : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_propagule";

		[StateBit]
		public bool Hanging { get; set; } = false;

		[StateRange(0, 4)]
		public int PropaguleStage { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			record.States.Add(new BlockStateInt { Name = "propagule_stage", Value = PropaguleStage });
			return record;
		} // method
	} // class

	public partial class MangroveRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_roots";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MangroveSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MangroveStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MangroveStandingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class MangroveTrapdoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_trapdoor";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class MangroveWallSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MangroveWood : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:mangrove_wood";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

		[StateBit]
		public bool StrippedBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			return record;
		} // method
	} // class

	public partial class MediumAmethystBud : Block
	{
		public override string Id { get; protected set; } = "minecraft:medium_amethyst_bud";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class MelonBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:melon_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MelonStem : Block
	{
		public override string Id { get; protected set; } = "minecraft:melon_stem";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 7)]
		public int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class MobSpawner : Block
	{
		public override string Id { get; protected set; } = "minecraft:mob_spawner";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MonsterEgg : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "monster_egg_stone_type", Value = MonsterEggStoneType });
			return record;
		} // method
	} // class

	public partial class MossBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:moss_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MossCarpet : Block
	{
		public override string Id { get; protected set; } = "minecraft:moss_carpet";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MossyCobblestone : Block
	{
		public override string Id { get; protected set; } = "minecraft:mossy_cobblestone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MossyCobblestoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:mossy_cobblestone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MossyStoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:mossy_stone_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MovingBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:moving_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Mud : Block
	{
		public override string Id { get; protected set; } = "minecraft:mud";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MudBrickDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:mud_brick_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MudBrickSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:mud_brick_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class MudBrickStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:mud_brick_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class MudBrickWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:mud_brick_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class MudBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:mud_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class MuddyMangroveRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:muddy_mangrove_roots";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Mycelium : Block
	{
		public override string Id { get; protected set; } = "minecraft:mycelium";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetherBrick : Block
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetherBrickFence
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetherBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:nether_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class NetherGoldOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:nether_gold_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetherSprouts : Block
	{
		public override string Id { get; protected set; } = "minecraft:nether_sprouts";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetherWart : Block
	{
		public override string Id { get; protected set; } = "minecraft:nether_wart";

		[StateRange(0, 3)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class NetherWartBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:nether_wart_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NetheriteBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:netherite_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Netherrack : Block
	{
		public override string Id { get; protected set; } = "minecraft:netherrack";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Netherreactor : Block
	{
		public override string Id { get; protected set; } = "minecraft:netherreactor";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class NormalStoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:normal_stone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Noteblock : Block
	{
		public override string Id { get; protected set; } = "minecraft:noteblock";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OakFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:oak_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OakHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:oak_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class OakLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:oak_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class OakStairs
	{
		public override string Id { get; protected set; } = "minecraft:oak_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Observer : Block
	{
		public override string Id { get; protected set; } = "minecraft:observer";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateBit]
		public bool PoweredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Obsidian : Block
	{
		public override string Id { get; protected set; } = "minecraft:obsidian";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OchreFroglight : Block
	{
		public override string Id { get; protected set; } = "minecraft:ochre_froglight";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class OrangeCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:orange_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class OrangeCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:orange_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class OrangeGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:orange_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class OrangeWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:orange_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OxidizedCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:oxidized_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:oxidized_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:oxidized_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class OxidizedCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:oxidized_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class OxidizedDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:oxidized_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PackedIce : Block
	{
		public override string Id { get; protected set; } = "minecraft:packed_ice";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PackedMud : Block
	{
		public override string Id { get; protected set; } = "minecraft:packed_mud";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PearlescentFroglight : Block
	{
		public override string Id { get; protected set; } = "minecraft:pearlescent_froglight";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PinkCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:pink_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PinkCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:pink_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PinkGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:pink_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PinkPetals : Block
	{
		public override string Id { get; protected set; } = "minecraft:pink_petals";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateRange(0, 7)]
		public int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PinkWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:pink_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Piston : Block
	{
		public override string Id { get; protected set; } = "minecraft:piston";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PistonArmCollision : Block
	{
		public override string Id { get; protected set; } = "minecraft:piston_arm_collision";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Planks : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class Podzol : Block
	{
		public override string Id { get; protected set; } = "minecraft:podzol";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PointedDripstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:pointed_dripstone";

		[StateEnum("tip", "frustum", "middle", "base", "merge")]
		public string DripstoneThickness { get; set; } = "tip";

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "dripstone_thickness", Value = DripstoneThickness });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class PolishedAndesiteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_andesite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBasalt : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_basalt";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstone : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBrickWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_brick_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstonePressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedBlackstoneWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_blackstone_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_deepslate";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_deepslate_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:polished_deepslate_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_deepslate_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedDeepslateWall : Block
	{
		public override string Id { get; protected set; } = "minecraft:polished_deepslate_wall";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeEast { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeNorth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeSouth { get; set; } = "none";

		[StateEnum("none", "short", "tall")]
		public string WallConnectionTypeWest { get; set; } = "none";

		[StateBit]
		public bool WallPostBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "wall_connection_type_east", Value = WallConnectionTypeEast });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_north", Value = WallConnectionTypeNorth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_south", Value = WallConnectionTypeSouth });
			record.States.Add(new BlockStateString { Name = "wall_connection_type_west", Value = WallConnectionTypeWest });
			record.States.Add(new BlockStateByte { Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit) });
			return record;
		} // method
	} // class

	public partial class PolishedDioriteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_diorite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PolishedGraniteStairs
	{
		public override string Id { get; protected set; } = "minecraft:polished_granite_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Portal : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "portal_axis", Value = PortalAxis });
			return record;
		} // method
	} // class

	public partial class Potatoes
	{
		public override string Id { get; protected set; } = "minecraft:potatoes";

		[StateRange(0, 7)]
		public override int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PowderSnow : Block
	{
		public override string Id { get; protected set; } = "minecraft:powder_snow";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PoweredComparator
	{
		public override string Id { get; protected set; } = "minecraft:powered_comparator";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OutputLitBit { get; set; } = false;

		[StateBit]
		public bool OutputSubtractBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit) });
			record.States.Add(new BlockStateByte { Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit) });
			return record;
		} // method
	} // class

	public partial class PoweredRepeater
	{
		public override string Id { get; protected set; } = "minecraft:powered_repeater";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateRange(0, 3)]
		public int RepeaterDelay { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "repeater_delay", Value = RepeaterDelay });
			return record;
		} // method
	} // class

	public partial class Prismarine : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "prismarine_block_type", Value = PrismarineBlockType });
			return record;
		} // method
	} // class

	public partial class PrismarineBricksStairs
	{
		public override string Id { get; protected set; } = "minecraft:prismarine_bricks_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class PrismarineStairs
	{
		public override string Id { get; protected set; } = "minecraft:prismarine_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Pumpkin : Block
	{
		public override string Id { get; protected set; } = "minecraft:pumpkin";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			return record;
		} // method
	} // class

	public partial class PumpkinStem : Block
	{
		public override string Id { get; protected set; } = "minecraft:pumpkin_stem";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 7)]
		public int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class PurpleCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:purple_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PurpleCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:purple_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class PurpleGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:purple_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class PurpleWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:purple_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class PurpurBlock : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "chisel_type", Value = ChiselType });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class PurpurStairs
	{
		public override string Id { get; protected set; } = "minecraft:purpur_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class QuartzBlock : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "chisel_type", Value = ChiselType });
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class QuartzBricks : Block
	{
		public override string Id { get; protected set; } = "minecraft:quartz_bricks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class QuartzOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:quartz_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class QuartzStairs
	{
		public override string Id { get; protected set; } = "minecraft:quartz_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Rail : Block
	{
		public override string Id { get; protected set; } = "minecraft:rail";

		[StateRange(0, 9)]
		public int RailDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "rail_direction", Value = RailDirection });
			return record;
		} // method
	} // class

	public partial class RawCopperBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:raw_copper_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RawGoldBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:raw_gold_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RawIronBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:raw_iron_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class RedCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class RedFlower : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "flower_type", Value = FlowerType });
			return record;
		} // method
	} // class

	public partial class RedGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:red_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class RedMushroom : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_mushroom";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedMushroomBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_mushroom_block";

		[StateRange(0, 15)]
		public int HugeMushroomBits { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "huge_mushroom_bits", Value = HugeMushroomBits });
			return record;
		} // method
	} // class

	public partial class RedNetherBrick : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_nether_brick";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedNetherBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:red_nether_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class RedSandstone : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "sand_stone_type", Value = SandStoneType });
			return record;
		} // method
	} // class

	public partial class RedSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:red_sandstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class RedWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:red_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedstoneBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:redstone_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedstoneLamp : Block
	{
		public override string Id { get; protected set; } = "minecraft:redstone_lamp";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedstoneOre : Block
	{
		public override string Id { get; protected set; } = "minecraft:redstone_ore";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RedstoneTorch
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class RedstoneWire : Block
	{
		public override string Id { get; protected set; } = "minecraft:redstone_wire";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class Reeds : Block
	{
		public override string Id { get; protected set; } = "minecraft:reeds";

		[StateRange(0, 15)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class ReinforcedDeepslate : Block
	{
		public override string Id { get; protected set; } = "minecraft:reinforced_deepslate";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RepeatingCommandBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:repeating_command_block";

		[StateBit]
		public bool ConditionalBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Reserved6 : Block
	{
		public override string Id { get; protected set; } = "minecraft:reserved6";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class RespawnAnchor : Block
	{
		public override string Id { get; protected set; } = "minecraft:respawn_anchor";

		[StateRange(0, 4)]
		public int RespawnAnchorCharge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "respawn_anchor_charge", Value = RespawnAnchorCharge });
			return record;
		} // method
	} // class

	public partial class Sand : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "sand_type", Value = SandType });
			return record;
		} // method
	} // class

	public partial class Sandstone : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "sand_stone_type", Value = SandStoneType });
			return record;
		} // method
	} // class

	public partial class SandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:sandstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Sapling : Block
	{
		public override string Id { get; protected set; } = "minecraft:sapling";

		[StateBit]
		public bool AgeBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "age_bit", Value = Convert.ToByte(AgeBit) });
			record.States.Add(new BlockStateString { Name = "sapling_type", Value = SaplingType });
			return record;
		} // method
	} // class

	public partial class Scaffolding : Block
	{
		public override string Id { get; protected set; } = "minecraft:scaffolding";

		[StateRange(0, 7)]
		public int Stability { get; set; } = 0;

		[StateBit]
		public bool StabilityCheck { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "stability", Value = Stability });
			record.States.Add(new BlockStateByte { Name = "stability_check", Value = Convert.ToByte(StabilityCheck) });
			return record;
		} // method
	} // class

	public partial class Sculk : Block
	{
		public override string Id { get; protected set; } = "minecraft:sculk";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SculkCatalyst : Block
	{
		public override string Id { get; protected set; } = "minecraft:sculk_catalyst";

		[StateBit]
		public bool Bloom { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "bloom", Value = Convert.ToByte(Bloom) });
			return record;
		} // method
	} // class

	public partial class SculkSensor : Block
	{
		public override string Id { get; protected set; } = "minecraft:sculk_sensor";

		[StateBit]
		public bool PoweredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class SculkShrieker : Block
	{
		public override string Id { get; protected set; } = "minecraft:sculk_shrieker";

		[StateBit]
		public bool Active { get; set; } = false;

		[StateBit]
		public bool CanSummon { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "active", Value = Convert.ToByte(Active) });
			record.States.Add(new BlockStateByte { Name = "can_summon", Value = Convert.ToByte(CanSummon) });
			return record;
		} // method
	} // class

	public partial class SculkVein : Block
	{
		public override string Id { get; protected set; } = "minecraft:sculk_vein";

		[StateRange(0, 63)]
		public int MultiFaceDirectionBits { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "multi_face_direction_bits", Value = MultiFaceDirectionBits });
			return record;
		} // method
	} // class

	public partial class SeaLantern : Block
	{
		public override string Id { get; protected set; } = "minecraft:sea_lantern";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SeaPickle : Block
	{
		public override string Id { get; protected set; } = "minecraft:sea_pickle";

		[StateRange(0, 3)]
		public int ClusterCount { get; set; } = 0;

		[StateBit]
		public bool DeadBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "cluster_count", Value = ClusterCount });
			record.States.Add(new BlockStateByte { Name = "dead_bit", Value = Convert.ToByte(DeadBit) });
			return record;
		} // method
	} // class

	public partial class Seagrass : Block
	{
		public override string Id { get; protected set; } = "minecraft:seagrass";

		[StateEnum("double_top", "double_bot", "default")]
		public string SeaGrassType { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "sea_grass_type", Value = SeaGrassType });
			return record;
		} // method
	} // class

	public partial class Shroomlight : Block
	{
		public override string Id { get; protected set; } = "minecraft:shroomlight";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class ShulkerBox
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class SilverGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:silver_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Skull : Block
	{
		public override string Id { get; protected set; } = "minecraft:skull";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Slime : Block
	{
		public override string Id { get; protected set; } = "minecraft:slime";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SmallAmethystBud : Block
	{
		public override string Id { get; protected set; } = "minecraft:small_amethyst_bud";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SmallDripleafBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:small_dripleaf_block";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class SmithingTable : Block
	{
		public override string Id { get; protected set; } = "minecraft:smithing_table";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Smoker : Block
	{
		public override string Id { get; protected set; } = "minecraft:smoker";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SmoothBasalt : Block
	{
		public override string Id { get; protected set; } = "minecraft:smooth_basalt";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SmoothQuartzStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_quartz_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothRedSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_red_sandstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothSandstoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:smooth_sandstone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SmoothStone : Block
	{
		public override string Id { get; protected set; } = "minecraft:smooth_stone";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Snow : Block
	{
		public override string Id { get; protected set; } = "minecraft:snow";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SnowLayer : Block
	{
		public override string Id { get; protected set; } = "minecraft:snow_layer";

		[StateBit]
		public bool CoveredBit { get; set; } = false;

		[StateRange(0, 7)]
		public int Height { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "covered_bit", Value = Convert.ToByte(CoveredBit) });
			record.States.Add(new BlockStateInt { Name = "height", Value = Height });
			return record;
		} // method
	} // class

	public partial class SoulCampfire : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_campfire";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool Extinguished { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "extinguished", Value = Convert.ToByte(Extinguished) });
			return record;
		} // method
	} // class

	public partial class SoulFire : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_fire";

		[StateRange(0, 15)]
		public int Age { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "age", Value = Age });
			return record;
		} // method
	} // class

	public partial class SoulLantern : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_lantern";

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SoulSand : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_sand";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SoulSoil : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_soil";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SoulTorch : Block
	{
		public override string Id { get; protected set; } = "minecraft:soul_torch";

		[StateEnum("unknown", "west", "east", "north", "south", "top")]
		public string TorchFacingDirection { get; set; } = "unknown";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class Sponge : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "sponge_type", Value = SpongeType });
			return record;
		} // method
	} // class

	public partial class SporeBlossom : Block
	{
		public override string Id { get; protected set; } = "minecraft:spore_blossom";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SpruceButton
	{
		public override string Id { get; protected set; } = "minecraft:spruce_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class SpruceDoor
	{
		public override string Id { get; protected set; } = "minecraft:spruce_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class SpruceFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:spruce_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class SpruceFenceGate
	{
		public override string Id { get; protected set; } = "minecraft:spruce_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class SpruceHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:spruce_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SpruceLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:spruce_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class SprucePressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:spruce_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class SpruceStairs
	{
		public override string Id { get; protected set; } = "minecraft:spruce_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class SpruceStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:spruce_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class SpruceTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:spruce_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class SpruceWallSign
	{
		public override string Id { get; protected set; } = "minecraft:spruce_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StainedGlass
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StainedGlassPane : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StainedHardenedClay : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "color", Value = Color });
			return record;
		} // method
	} // class

	public partial class StandingBanner : Block
	{
		public override string Id { get; protected set; } = "minecraft:standing_banner";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class StandingSign
	{
		public override string Id { get; protected set; } = "minecraft:standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class StickyPiston : Block
	{
		public override string Id { get; protected set; } = "minecraft:sticky_piston";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StickyPistonArmCollision : Block
	{
		public override string Id { get; protected set; } = "minecraft:sticky_piston_arm_collision";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class Stone : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_type", Value = StoneType });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:stone_block_slab";

		[StateEnum("smooth_stone", "sandstone", "wood", "cobblestone", "brick", "stone_brick", "quartz", "nether_brick")]
		public string StoneSlabType { get; set; } = "smooth_stone";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type", Value = StoneSlabType });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab2 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:stone_block_slab2";

		[StateEnum("red_sandstone", "purpur", "prismarine_rough", "prismarine_dark", "prismarine_brick", "mossy_cobblestone", "smooth_sandstone", "red_nether_brick")]
		public string StoneSlabType2 { get; set; } = "red_sandstone";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_2", Value = StoneSlabType2 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab3 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:stone_block_slab3";

		[StateEnum("end_stone_brick", "smooth_red_sandstone", "polished_andesite", "andesite", "diorite", "polished_diorite", "granite", "polished_granite")]
		public string StoneSlabType3 { get; set; } = "end_stone_brick";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_3", Value = StoneSlabType3 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBlockSlab4 : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:stone_block_slab4";

		[StateEnum("smooth_quartz", "stone", "cut_sandstone", "cut_red_sandstone", "mossy_stone_brick")]
		public string StoneSlabType4 { get; set; } = "";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_slab_type_4", Value = StoneSlabType4 });
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class StoneBrickStairs
	{
		public override string Id { get; protected set; } = "minecraft:stone_brick_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class StoneButton
	{
		public override string Id { get; protected set; } = "minecraft:stone_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StonePressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:stone_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class StoneStairs
	{
		public override string Id { get; protected set; } = "minecraft:stone_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class Stonebrick : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "stone_brick_type", Value = StoneBrickType });
			return record;
		} // method
	} // class

	public partial class Stonecutter : Block
	{
		public override string Id { get; protected set; } = "minecraft:stonecutter";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class StonecutterBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:stonecutter_block";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class StrippedAcaciaLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_acacia_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedBambooBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:stripped_bamboo_block";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedBirchLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_birch_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCherryLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_cherry_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCherryWood : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_cherry_wood";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCrimsonHyphae : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_crimson_hyphae";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedCrimsonStem : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_crimson_stem";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedDarkOakLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_dark_oak_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedJungleLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_jungle_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedMangroveLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_mangrove_log";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedMangroveWood : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_mangrove_wood";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedOakLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_oak_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedSpruceLog : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_spruce_log";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedWarpedHyphae : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_warped_hyphae";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StrippedWarpedStem : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:stripped_warped_stem";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class StructureBlock : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "structure_block_type", Value = StructureBlockType });
			return record;
		} // method
	} // class

	public partial class StructureVoid : Block
	{
		public override string Id { get; protected set; } = "minecraft:structure_void";

		[StateEnum("void", "air")]
		public string StructureVoidType { get; set; } = "void";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "structure_void_type", Value = StructureVoidType });
			return record;
		} // method
	} // class

	public partial class SuspiciousGravel : Block
	{
		public override string Id { get; protected set; } = "minecraft:suspicious_gravel";

		[StateRange(0, 3)]
		public int BrushedProgress { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "brushed_progress", Value = BrushedProgress });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SuspiciousSand : Block
	{
		public override string Id { get; protected set; } = "minecraft:suspicious_sand";

		[StateRange(0, 3)]
		public int BrushedProgress { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "brushed_progress", Value = BrushedProgress });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class SweetBerryBush : Block
	{
		public override string Id { get; protected set; } = "minecraft:sweet_berry_bush";

		[StateRange(0, 7)]
		public int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Tallgrass : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "tall_grass_type", Value = TallGrassType });
			return record;
		} // method
	} // class

	public partial class Target : Block
	{
		public override string Id { get; protected set; } = "minecraft:target";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class TintedGlass : Block
	{
		public override string Id { get; protected set; } = "minecraft:tinted_glass";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Tnt : Block
	{
		public override string Id { get; protected set; } = "minecraft:tnt";

		[StateBit]
		public bool AllowUnderwaterBit { get; set; } = false;

		[StateBit]
		public bool ExplodeBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "allow_underwater_bit", Value = Convert.ToByte(AllowUnderwaterBit) });
			record.States.Add(new BlockStateByte { Name = "explode_bit", Value = Convert.ToByte(ExplodeBit) });
			return record;
		} // method
	} // class

	public partial class Torch : Block
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class Torchflower : Block
	{
		public override string Id { get; protected set; } = "minecraft:torchflower";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class TorchflowerCrop : Block
	{
		public override string Id { get; protected set; } = "minecraft:torchflower_crop";

		[StateRange(0, 7)]
		public int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class Trapdoor
	{
		public override string Id { get; protected set; } = "minecraft:trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class TrappedChest
	{
		public override string Id { get; protected set; } = "minecraft:trapped_chest";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class TripWire : Block
	{
		public override string Id { get; protected set; } = "minecraft:trip_wire";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateBit]
		public bool DisarmedBit { get; set; } = false;

		[StateBit]
		public bool PoweredBit { get; set; } = false;

		[StateBit]
		public bool SuspendedBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateByte { Name = "disarmed_bit", Value = Convert.ToByte(DisarmedBit) });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			record.States.Add(new BlockStateByte { Name = "suspended_bit", Value = Convert.ToByte(SuspendedBit) });
			return record;
		} // method
	} // class

	public partial class TripwireHook : Block
	{
		public override string Id { get; protected set; } = "minecraft:tripwire_hook";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool PoweredBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "powered_bit", Value = Convert.ToByte(PoweredBit) });
			return record;
		} // method
	} // class

	public partial class Tuff : Block
	{
		public override string Id { get; protected set; } = "minecraft:tuff";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class TurtleEgg : Block
	{
		public override string Id { get; protected set; } = "minecraft:turtle_egg";

		[StateEnum("cracked", "max_cracked", "no_cracks")]
		public string CrackedState { get; set; } = "";

		[StateEnum("one_egg", "two_egg", "three_egg", "four_egg")]
		public string TurtleEggCount { get; set; } = "";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "cracked_state", Value = CrackedState });
			record.States.Add(new BlockStateString { Name = "turtle_egg_count", Value = TurtleEggCount });
			return record;
		} // method
	} // class

	public partial class TwistingVines : Block
	{
		public override string Id { get; protected set; } = "minecraft:twisting_vines";

		[StateRange(0, 25)]
		public int TwistingVinesAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "twisting_vines_age", Value = TwistingVinesAge });
			return record;
		} // method
	} // class

	public partial class UnderwaterTorch : Block
	{
		public override string Id { get; protected set; } = "minecraft:underwater_torch";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class UndyedShulkerBox : Block
	{
		public override string Id { get; protected set; } = "minecraft:undyed_shulker_box";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Unknown : Block
	{
		public override string Id { get; protected set; } = "minecraft:unknown";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class UnlitRedstoneTorch
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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "torch_facing_direction", Value = TorchFacingDirection });
			return record;
		} // method
	} // class

	public partial class UnpoweredComparator
	{
		public override string Id { get; protected set; } = "minecraft:unpowered_comparator";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool OutputLitBit { get; set; } = false;

		[StateBit]
		public bool OutputSubtractBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit) });
			record.States.Add(new BlockStateByte { Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit) });
			return record;
		} // method
	} // class

	public partial class UnpoweredRepeater
	{
		public override string Id { get; protected set; } = "minecraft:unpowered_repeater";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateRange(0, 3)]
		public int RepeaterDelay { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateInt { Name = "repeater_delay", Value = RepeaterDelay });
			return record;
		} // method
	} // class

	public partial class VerdantFroglight : Block
	{
		public override string Id { get; protected set; } = "minecraft:verdant_froglight";

		[StateEnum("y", "x", "z")]
		public string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class Vine : Block
	{
		public override string Id { get; protected set; } = "minecraft:vine";

		[StateRange(0, 15)]
		public int VineDirectionBits { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "vine_direction_bits", Value = VineDirectionBits });
			return record;
		} // method
	} // class

	public partial class WallBanner : Block
	{
		public override string Id { get; protected set; } = "minecraft:wall_banner";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WallSign
	{
		public override string Id { get; protected set; } = "minecraft:wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedButton : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_button";

		[StateBit]
		public bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedDoor : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_door";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

		[StateBit]
		public bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class WarpedDoubleSlab : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_double_slab";

		[StateBit]
		public bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WarpedFence : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_fence";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WarpedFenceGate : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_fence_gate";

		[StateRange(0, 3)]
		public int Direction { get; set; } = 0;

		[StateBit]
		public bool InWallBit { get; set; } = false;

		[StateBit]
		public bool OpenBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "in_wall_bit", Value = Convert.ToByte(InWallBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			return record;
		} // method
	} // class

	public partial class WarpedFungus : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_fungus";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WarpedHangingSign : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_hanging_sign";

		[StateBit]
		public bool AttachedBit { get; set; } = false;

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

		[StateBit]
		public bool Hanging { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "attached_bit", Value = Convert.ToByte(AttachedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			record.States.Add(new BlockStateByte { Name = "hanging", Value = Convert.ToByte(Hanging) });
			return record;
		} // method
	} // class

	public partial class WarpedHyphae : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:warped_hyphae";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class WarpedNylium : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_nylium";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WarpedPlanks : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_planks";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WarpedPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class WarpedRoots : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_roots";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WarpedSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:warped_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WarpedStairs
	{
		public override string Id { get; protected set; } = "minecraft:warped_stairs";

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public override int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WarpedStandingSign
	{
		public override string Id { get; protected set; } = "minecraft:warped_standing_sign";

		[StateRange(0, 15)]
		public int GroundSignDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "ground_sign_direction", Value = GroundSignDirection });
			return record;
		} // method
	} // class

	public partial class WarpedStem : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:warped_stem";

		[StateEnum("y", "x", "z")]
		public override string PillarAxis { get; set; } = "y";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			return record;
		} // method
	} // class

	public partial class WarpedTrapdoor
	{
		public override string Id { get; protected set; } = "minecraft:warped_trapdoor";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 0;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpsideDownBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			return record;
		} // method
	} // class

	public partial class WarpedWallSign
	{
		public override string Id { get; protected set; } = "minecraft:warped_wall_sign";

		[StateRange(0, 5)]
		public int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WarpedWartBlock : Block
	{
		public override string Id { get; protected set; } = "minecraft:warped_wart_block";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Water
	{
		public override string Id { get; protected set; } = "minecraft:water";

		[StateRange(0, 15)]
		public override int LiquidDepth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "liquid_depth", Value = LiquidDepth });
			return record;
		} // method
	} // class

	public partial class Waterlily : Block
	{
		public override string Id { get; protected set; } = "minecraft:waterlily";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedExposedCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_exposed_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_exposed_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_exposed_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedExposedCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_exposed_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedExposedDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_exposed_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_oxidized_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_oxidized_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_oxidized_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_oxidized_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedOxidizedDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_oxidized_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_weathered_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_weathered_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_weathered_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:waxed_weathered_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WaxedWeatheredDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:waxed_weathered_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WeatheredCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:weathered_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopper : Block
	{
		public override string Id { get; protected set; } = "minecraft:weathered_cut_copper";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:weathered_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class WeatheredCutCopperStairs : Block
	{
		public override string Id { get; protected set; } = "minecraft:weathered_cut_copper_stairs";

		[StateBit]
		public bool UpsideDownBit { get; set; } = false;

		[StateRange(0, 3)]
		public int WeirdoDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit) });
			record.States.Add(new BlockStateInt { Name = "weirdo_direction", Value = WeirdoDirection });
			return record;
		} // method
	} // class

	public partial class WeatheredDoubleCutCopperSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:weathered_double_cut_copper_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			return record;
		} // method
	} // class

	public partial class Web : Block
	{
		public override string Id { get; protected set; } = "minecraft:web";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WeepingVines : Block
	{
		public override string Id { get; protected set; } = "minecraft:weeping_vines";

		[StateRange(0, 25)]
		public int WeepingVinesAge { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "weeping_vines_age", Value = WeepingVinesAge });
			return record;
		} // method
	} // class

	public partial class Wheat
	{
		public override string Id { get; protected set; } = "minecraft:wheat";

		[StateRange(0, 7)]
		public override int Growth { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "growth", Value = Growth });
			return record;
		} // method
	} // class

	public partial class WhiteCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:white_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class WhiteCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:white_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class WhiteGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:white_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WhiteWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:white_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class WitherRose : Block
	{
		public override string Id { get; protected set; } = "minecraft:wither_rose";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class Wood : LogBase
	{
		public override string Id { get; protected set; } = "minecraft:wood";

		[StateEnum("x", "z", "y")]
		public override string PillarAxis { get; set; } = "x";

		[StateBit]
		public bool StrippedBit { get; set; } = false;

		[StateEnum("oak", "spruce", "birch", "jungle", "acacia", "dark_oak")]
		public string WoodType { get; set; } = "oak";

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
			record.Id = Id;
			record.States.Add(new BlockStateString { Name = "pillar_axis", Value = PillarAxis });
			record.States.Add(new BlockStateByte { Name = "stripped_bit", Value = Convert.ToByte(StrippedBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class WoodenButton
	{
		public override string Id { get; protected set; } = "minecraft:wooden_button";

		[StateBit]
		public override bool ButtonPressedBit { get; set; } = false;

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit) });
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class WoodenDoor
	{
		public override string Id { get; protected set; } = "minecraft:wooden_door";

		[StateRange(0, 3)]
		public override int Direction { get; set; } = 1;

		[StateBit]
		public override bool DoorHingeBit { get; set; } = false;

		[StateBit]
		public override bool OpenBit { get; set; } = false;

		[StateBit]
		public override bool UpperBlockBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "direction", Value = Direction });
			record.States.Add(new BlockStateByte { Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit) });
			record.States.Add(new BlockStateByte { Name = "open_bit", Value = Convert.ToByte(OpenBit) });
			record.States.Add(new BlockStateByte { Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit) });
			return record;
		} // method
	} // class

	public partial class WoodenPressurePlate : Block
	{
		public override string Id { get; protected set; } = "minecraft:wooden_pressure_plate";

		[StateRange(0, 15)]
		public int RedstoneSignal { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "redstone_signal", Value = RedstoneSignal });
			return record;
		} // method
	} // class

	public partial class WoodenSlab : SlabBase
	{
		public override string Id { get; protected set; } = "minecraft:wooden_slab";

		[StateBit]
		public override bool TopSlotBit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit) });
			record.States.Add(new BlockStateString { Name = "wood_type", Value = WoodType });
			return record;
		} // method
	} // class

	public partial class YellowCandle : Block
	{
		public override string Id { get; protected set; } = "minecraft:yellow_candle";

		[StateRange(0, 3)]
		public int Candles { get; set; } = 0;

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "candles", Value = Candles });
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class YellowCandleCake : Block
	{
		public override string Id { get; protected set; } = "minecraft:yellow_candle_cake";

		[StateBit]
		public bool Lit { get; set; } = false;

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
			record.Id = Id;
			record.States.Add(new BlockStateByte { Name = "lit", Value = Convert.ToByte(Lit) });
			return record;
		} // method
	} // class

	public partial class YellowFlower : Block
	{
		public override string Id { get; protected set; } = "minecraft:yellow_flower";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class

	public partial class YellowGlazedTerracotta
	{
		public override string Id { get; protected set; } = "minecraft:yellow_glazed_terracotta";

		[StateRange(0, 5)]
		public override int FacingDirection { get; set; } = 0;

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
			record.Id = Id;
			record.States.Add(new BlockStateInt { Name = "facing_direction", Value = FacingDirection });
			return record;
		} // method
	} // class

	public partial class YellowWool : Block
	{
		public override string Id { get; protected set; } = "minecraft:yellow_wool";

		public override BlockStateContainer GetState()
		{
			var record = new BlockStateContainer();
			record.Id = Id;
			return record;
		} // method
	} // class
}
