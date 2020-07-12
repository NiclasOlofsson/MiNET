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

    public partial class AcaciaButton // 395 typeof=AcaciaButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_button";
            record.Id = 395;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class AcaciaDoor // 196 typeof=AcaciaDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool DoorHingeBit { get; set; } = true;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpperBlockBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_door";
            record.Id = 196;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class AcaciaFenceGate // 187 typeof=AcaciaFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool InWallBit { get; set; } = false;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_fence_gate";
            record.Id = 187;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class AcaciaPressurePlate // 405 typeof=AcaciaPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 12;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_pressure_plate";
            record.Id = 405;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class AcaciaStairs // 163 typeof=AcaciaStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_stairs";
            record.Id = 163;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class AcaciaStandingSign // 445 typeof=AcaciaStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_standing_sign";
            record.Id = 445;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class AcaciaTrapdoor // 400 typeof=AcaciaTrapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 1;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpsideDownBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_trapdoor";
            record.Id = 400;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class AcaciaWallSign // 446 typeof=AcaciaWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:acacia_wall_sign";
            record.Id = 446;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class ActivatorRail // 126 typeof=ActivatorRail
    {
        [StateBit] public bool RailDataBit { get; set; } = false;
        [StateRange(0, 5)] public int RailDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:activator_rail";
            record.Id = 126;
            record.States.Add(new BlockStateByte {Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit)});
            record.States.Add(new BlockStateInt {Name = "rail_direction", Value = RailDirection});
            return record;
        } // method
    } // class

    public partial class Air // 0 typeof=Air
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:air";
            record.Id = 0;
            return record;
        } // method
    } // class

    public partial class Allow // 210 typeof=Allow
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:allow";
            record.Id = 210;
            return record;
        } // method
    } // class

    public partial class AncientDebris // 526 typeof=AncientDebris
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:ancient_debris";
            record.Id = 526;
            return record;
        } // method
    } // class

    public partial class AndesiteStairs // 426 typeof=AndesiteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:andesite_stairs";
            record.Id = 426;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Anvil // 145 typeof=Anvil
    {
        [StateEnum("very_damaged","slightly_damaged","undamaged","broken")]
        public string Damage { get; set; } = "very_damaged";
        [StateRange(0, 3)] public int Direction { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:anvil";
            record.Id = 145;
            record.States.Add(new BlockStateString {Name = "damage", Value = Damage});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class Bamboo // 418 typeof=Bamboo
    {
        [StateBit] public bool AgeBit { get; set; } = false;
        [StateEnum("large_leaves","no_leaves","small_leaves")]
        public string BambooLeafSize { get; set; } = "large_leaves";
        [StateEnum("thin","thick")]
        public string BambooStalkThickness { get; set; } = "thin";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bamboo";
            record.Id = 418;
            record.States.Add(new BlockStateByte {Name = "age_bit", Value = Convert.ToByte(AgeBit)});
            record.States.Add(new BlockStateString {Name = "bamboo_leaf_size", Value = BambooLeafSize});
            record.States.Add(new BlockStateString {Name = "bamboo_stalk_thickness", Value = BambooStalkThickness});
            return record;
        } // method
    } // class

    public partial class BambooSapling // 419 typeof=BambooSapling
    {
        [StateBit] public bool AgeBit { get; set; } = false;
        [StateEnum("jungle","acacia","dark_oak","birch","oak","spruce")]
        public string SaplingType { get; set; } = "jungle";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bamboo_sapling";
            record.Id = 419;
            record.States.Add(new BlockStateByte {Name = "age_bit", Value = Convert.ToByte(AgeBit)});
            record.States.Add(new BlockStateString {Name = "sapling_type", Value = SaplingType});
            return record;
        } // method
    } // class

    public partial class Barrel // 458 typeof=Barrel
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
        [StateBit] public bool OpenBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:barrel";
            record.Id = 458;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class Barrier // 416 typeof=Barrier
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:barrier";
            record.Id = 416;
            return record;
        } // method
    } // class

    public partial class Basalt // 489 typeof=Basalt
    {
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:basalt";
            record.Id = 489;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class Beacon // 138 typeof=Beacon
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:beacon";
            record.Id = 138;
            return record;
        } // method
    } // class

    public partial class Bed // 26 typeof=Bed
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool HeadPieceBit { get; set; } = true;
        [StateBit] public bool OccupiedBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bed";
            record.Id = 26;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "head_piece_bit", Value = Convert.ToByte(HeadPieceBit)});
            record.States.Add(new BlockStateByte {Name = "occupied_bit", Value = Convert.ToByte(OccupiedBit)});
            return record;
        } // method
    } // class

    public partial class Bedrock // 7 typeof=Bedrock
    {
        [StateBit] public bool InfiniburnBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bedrock";
            record.Id = 7;
            record.States.Add(new BlockStateByte {Name = "infiniburn_bit", Value = Convert.ToByte(InfiniburnBit)});
            return record;
        } // method
    } // class

    public partial class BeeNest // 473 typeof=BeeNest
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
        [StateRange(0, 5)] public int HoneyLevel { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateInt s when s.Name == "facing_direction":
                        FacingDirection = s.Value;
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
            record.Name = "minecraft:bee_nest";
            record.Id = 473;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateInt {Name = "honey_level", Value = HoneyLevel});
            return record;
        } // method
    } // class

    public partial class Beehive // 474 typeof=Beehive
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;
        [StateRange(0, 5)] public int HoneyLevel { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateInt s when s.Name == "facing_direction":
                        FacingDirection = s.Value;
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
            record.Name = "minecraft:beehive";
            record.Id = 474;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateInt {Name = "honey_level", Value = HoneyLevel});
            return record;
        } // method
    } // class

    public partial class Beetroot // 244 typeof=Beetroot
    {
        [StateRange(0, 7)] public override int Growth { get; set; } = 6;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:beetroot";
            record.Id = 244;
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class Bell // 461 typeof=Bell
    {
        [StateEnum("multiple","standing","hanging","side")]
        public string Attachment { get; set; } = "multiple";
        [StateRange(0, 3)] public int Direction { get; set; } = 2;
        [StateBit] public bool ToggleBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bell";
            record.Id = 461;
            record.States.Add(new BlockStateString {Name = "attachment", Value = Attachment});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "toggle_bit", Value = Convert.ToByte(ToggleBit)});
            return record;
        } // method
    } // class

    public partial class BirchButton // 396 typeof=BirchButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = true;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_button";
            record.Id = 396;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class BirchDoor // 194 typeof=BirchDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool DoorHingeBit { get; set; } = false;
        [StateBit] public override bool OpenBit { get; set; } = true;
        [StateBit] public override bool UpperBlockBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_door";
            record.Id = 194;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class BirchFenceGate // 184 typeof=BirchFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool InWallBit { get; set; } = false;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_fence_gate";
            record.Id = 184;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class BirchPressurePlate // 406 typeof=BirchPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_pressure_plate";
            record.Id = 406;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class BirchStairs // 135 typeof=BirchStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_stairs";
            record.Id = 135;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class BirchStandingSign // 441 typeof=BirchStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 14;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_standing_sign";
            record.Id = 441;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class BirchTrapdoor // 401 typeof=BirchTrapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool OpenBit { get; set; } = true;
        [StateBit] public override bool UpsideDownBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_trapdoor";
            record.Id = 401;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class BirchWallSign // 442 typeof=BirchWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:birch_wall_sign";
            record.Id = 442;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class BlackGlazedTerracotta // 235 typeof=BlackGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:black_glazed_terracotta";
            record.Id = 235;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Blackstone // 528 typeof=Blackstone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:blackstone";
            record.Id = 528;
            return record;
        } // method
    } // class

    public partial class BlackstoneDoubleSlab // 538 typeof=BlackstoneDoubleSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blackstone_double_slab";
            record.Id = 538;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class BlackstoneSlab // 537 typeof=BlackstoneSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blackstone_slab";
            record.Id = 537;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class BlackstoneStairs // 531 typeof=BlackstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blackstone_stairs";
            record.Id = 531;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class BlackstoneWall // 532 typeof=BlackstoneWall
    {
        [StateEnum("none","short","tall")]
        public string WallConnectionTypeEast { get; set; } = "none";
        [StateEnum("tall","short","none")]
        public string WallConnectionTypeNorth { get; set; } = "tall";
        [StateEnum("short","none","tall")]
        public string WallConnectionTypeSouth { get; set; } = "short";
        [StateEnum("short","tall","none")]
        public string WallConnectionTypeWest { get; set; } = "short";
        [StateBit] public bool WallPostBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blackstone_wall";
            record.Id = 532;
            record.States.Add(new BlockStateString {Name = "wall_connection_type_east", Value = WallConnectionTypeEast});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_north", Value = WallConnectionTypeNorth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_south", Value = WallConnectionTypeSouth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_west", Value = WallConnectionTypeWest});
            record.States.Add(new BlockStateByte {Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit)});
            return record;
        } // method
    } // class

    public partial class BlastFurnace // 451 typeof=BlastFurnace
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blast_furnace";
            record.Id = 451;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class BlueGlazedTerracotta // 231 typeof=BlueGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:blue_glazed_terracotta";
            record.Id = 231;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class BlueIce // 266 typeof=BlueIce
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:blue_ice";
            record.Id = 266;
            return record;
        } // method
    } // class

    public partial class BoneBlock // 216 typeof=BoneBlock
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 2;
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bone_block";
            record.Id = 216;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class Bookshelf // 47 typeof=Bookshelf
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:bookshelf";
            record.Id = 47;
            return record;
        } // method
    } // class

    public partial class BorderBlock // 212 typeof=BorderBlock
    {
        [StateEnum("short","none","tall")]
        public string WallConnectionTypeEast { get; set; } = "short";
        [StateEnum("none","tall","short")]
        public string WallConnectionTypeNorth { get; set; } = "none";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeSouth { get; set; } = "tall";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeWest { get; set; } = "tall";
        [StateBit] public bool WallPostBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:border_block";
            record.Id = 212;
            record.States.Add(new BlockStateString {Name = "wall_connection_type_east", Value = WallConnectionTypeEast});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_north", Value = WallConnectionTypeNorth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_south", Value = WallConnectionTypeSouth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_west", Value = WallConnectionTypeWest});
            record.States.Add(new BlockStateByte {Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit)});
            return record;
        } // method
    } // class

    public partial class BrewingStand // 117 typeof=BrewingStand
    {
        [StateBit] public bool BrewingStandSlotABit { get; set; } = true;
        [StateBit] public bool BrewingStandSlotBBit { get; set; } = false;
        [StateBit] public bool BrewingStandSlotCBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:brewing_stand";
            record.Id = 117;
            record.States.Add(new BlockStateByte {Name = "brewing_stand_slot_a_bit", Value = Convert.ToByte(BrewingStandSlotABit)});
            record.States.Add(new BlockStateByte {Name = "brewing_stand_slot_b_bit", Value = Convert.ToByte(BrewingStandSlotBBit)});
            record.States.Add(new BlockStateByte {Name = "brewing_stand_slot_c_bit", Value = Convert.ToByte(BrewingStandSlotCBit)});
            return record;
        } // method
    } // class

    public partial class BrickBlock // 45 typeof=BrickBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:brick_block";
            record.Id = 45;
            return record;
        } // method
    } // class

    public partial class BrickStairs // 108 typeof=BrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:brick_stairs";
            record.Id = 108;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class BrownGlazedTerracotta // 232 typeof=BrownGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:brown_glazed_terracotta";
            record.Id = 232;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class BrownMushroom // 39 typeof=BrownMushroom
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:brown_mushroom";
            record.Id = 39;
            return record;
        } // method
    } // class

    public partial class BrownMushroomBlock // 99 typeof=BrownMushroomBlock
    {
        [StateRange(0, 15)] public int HugeMushroomBits { get; set; } = 15;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:brown_mushroom_block";
            record.Id = 99;
            record.States.Add(new BlockStateInt {Name = "huge_mushroom_bits", Value = HugeMushroomBits});
            return record;
        } // method
    } // class

    public partial class BubbleColumn // 415 typeof=BubbleColumn
    {
        [StateBit] public bool DragDown { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:bubble_column";
            record.Id = 415;
            record.States.Add(new BlockStateByte {Name = "drag_down", Value = Convert.ToByte(DragDown)});
            return record;
        } // method
    } // class

    public partial class Cactus // 81 typeof=Cactus
    {
        [StateRange(0, 15)] public int Age { get; set; } = 14;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cactus";
            record.Id = 81;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class Cake // 92 typeof=Cake
    {
        [StateRange(0, 6)] public int BiteCounter { get; set; } = 6;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cake";
            record.Id = 92;
            record.States.Add(new BlockStateInt {Name = "bite_counter", Value = BiteCounter});
            return record;
        } // method
    } // class

    public partial class Camera // 242 typeof=Camera
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:camera";
            record.Id = 242;
            return record;
        } // method
    } // class

    public partial class Campfire // 464 typeof=Campfire
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 2;
        [StateBit] public bool Extinguished { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:campfire";
            record.Id = 464;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "extinguished", Value = Convert.ToByte(Extinguished)});
            return record;
        } // method
    } // class

    public partial class Carpet // 171 typeof=Carpet
    {
        [StateEnum("white","silver","brown","blue","green","yellow","red","black","cyan","pink","lime","purple","light_blue","orange","gray","magenta")]
        public string Color { get; set; } = "white";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:carpet";
            record.Id = 171;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class Carrots // 141 typeof=Carrots
    {
        [StateRange(0, 7)] public override int Growth { get; set; } = 7;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:carrots";
            record.Id = 141;
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class CartographyTable // 455 typeof=CartographyTable
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:cartography_table";
            record.Id = 455;
            return record;
        } // method
    } // class

    public partial class CarvedPumpkin // 410 typeof=CarvedPumpkin
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:carved_pumpkin";
            record.Id = 410;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class Cauldron // 118 typeof=Cauldron
    {
        [StateEnum("water","lava")]
        public string CauldronLiquid { get; set; } = "water";
        [StateRange(0, 6)] public int FillLevel { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cauldron";
            record.Id = 118;
            record.States.Add(new BlockStateString {Name = "cauldron_liquid", Value = CauldronLiquid});
            record.States.Add(new BlockStateInt {Name = "fill_level", Value = FillLevel});
            return record;
        } // method
    } // class

    public partial class Chain // 541 typeof=Chain
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:chain";
            record.Id = 541;
            return record;
        } // method
    } // class

    public partial class ChainCommandBlock // 189 typeof=ChainCommandBlock
    {
        [StateBit] public bool ConditionalBit { get; set; } = false;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:chain_command_block";
            record.Id = 189;
            record.States.Add(new BlockStateByte {Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class ChemicalHeat // 192 typeof=ChemicalHeat
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:chemical_heat";
            record.Id = 192;
            return record;
        } // method
    } // class

    public partial class ChemistryTable // 238 typeof=ChemistryTable
    {
        [StateEnum("material_reducer","element_constructor","compound_creator","lab_table")]
        public string ChemistryTableType { get; set; } = "material_reducer";
        [StateRange(0, 3)] public int Direction { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:chemistry_table";
            record.Id = 238;
            record.States.Add(new BlockStateString {Name = "chemistry_table_type", Value = ChemistryTableType});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class Chest // 54 typeof=Chest
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:chest";
            record.Id = 54;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class ChiseledNetherBricks // 557 typeof=ChiseledNetherBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:chiseled_nether_bricks";
            record.Id = 557;
            return record;
        } // method
    } // class

    public partial class ChiseledPolishedBlackstone // 534 typeof=ChiseledPolishedBlackstone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:chiseled_polished_blackstone";
            record.Id = 534;
            return record;
        } // method
    } // class

    public partial class ChorusFlower // 200 typeof=ChorusFlower
    {
        [StateRange(0, 5)] public int Age { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:chorus_flower";
            record.Id = 200;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class ChorusPlant // 240 typeof=ChorusPlant
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:chorus_plant";
            record.Id = 240;
            return record;
        } // method
    } // class

    public partial class Clay // 82 typeof=Clay
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:clay";
            record.Id = 82;
            return record;
        } // method
    } // class

    public partial class CoalBlock // 173 typeof=CoalBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:coal_block";
            record.Id = 173;
            return record;
        } // method
    } // class

    public partial class CoalOre // 16 typeof=CoalOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:coal_ore";
            record.Id = 16;
            return record;
        } // method
    } // class

    public partial class Cobblestone // 4 typeof=Cobblestone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:cobblestone";
            record.Id = 4;
            return record;
        } // method
    } // class

    public partial class CobblestoneWall // 139 typeof=CobblestoneWall
    {
        [StateEnum("stone_brick","mossy_stone_brick","end_brick","granite","nether_brick","sandstone","brick","red_nether_brick","cobblestone","diorite","red_sandstone","prismarine","mossy_cobblestone","andesite")]
        public string WallBlockType { get; set; } = "stone_brick";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeEast { get; set; } = "tall";
        [StateEnum("tall","short","none")]
        public string WallConnectionTypeNorth { get; set; } = "tall";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeSouth { get; set; } = "tall";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeWest { get; set; } = "tall";
        [StateBit] public bool WallPostBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cobblestone_wall";
            record.Id = 139;
            record.States.Add(new BlockStateString {Name = "wall_block_type", Value = WallBlockType});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_east", Value = WallConnectionTypeEast});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_north", Value = WallConnectionTypeNorth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_south", Value = WallConnectionTypeSouth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_west", Value = WallConnectionTypeWest});
            record.States.Add(new BlockStateByte {Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit)});
            return record;
        } // method
    } // class

    public partial class Cocoa // 127 typeof=Cocoa
    {
        [StateRange(0, 2)] public int Age { get; set; } = 2;
        [StateRange(0, 3)] public int Direction { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cocoa";
            record.Id = 127;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class ColoredTorchBp // 204 typeof=ColoredTorchBp
    {
        [StateBit] public bool ColorBit { get; set; } = true;
        [StateEnum("east","unknown","west","south","top","north")]
        public string TorchFacingDirection { get; set; } = "east";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:colored_torch_bp";
            record.Id = 204;
            record.States.Add(new BlockStateByte {Name = "color_bit", Value = Convert.ToByte(ColorBit)});
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class ColoredTorchRg // 202 typeof=ColoredTorchRg
    {
        [StateBit] public bool ColorBit { get; set; } = false;
        [StateEnum("east","south","north","west","top","unknown")]
        public string TorchFacingDirection { get; set; } = "east";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:colored_torch_rg";
            record.Id = 202;
            record.States.Add(new BlockStateByte {Name = "color_bit", Value = Convert.ToByte(ColorBit)});
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class CommandBlock // 137 typeof=CommandBlock
    {
        [StateBit] public bool ConditionalBit { get; set; } = true;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:command_block";
            record.Id = 137;
            record.States.Add(new BlockStateByte {Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Composter // 468 typeof=Composter
    {
        [StateRange(0, 8)] public int ComposterFillLevel { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:composter";
            record.Id = 468;
            record.States.Add(new BlockStateInt {Name = "composter_fill_level", Value = ComposterFillLevel});
            return record;
        } // method
    } // class

    public partial class Concrete // 236 typeof=Concrete
    {
        [StateEnum("red","purple","green","yellow","pink","magenta","lime","brown","blue","light_blue","white","orange","cyan","silver","black","gray")]
        public string Color { get; set; } = "red";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:concrete";
            record.Id = 236;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class ConcretePowder // 237 typeof=ConcretePowder
    {
        [StateEnum("magenta","yellow","blue","brown","silver","orange","cyan","pink","white","green","black","lime","light_blue","red","purple","gray")]
        public string Color { get; set; } = "magenta";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:concretePowder";
            record.Id = 237;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class Conduit // 412 typeof=Conduit
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:conduit";
            record.Id = 412;
            return record;
        } // method
    } // class

    public partial class Coral // 386 typeof=Coral
    {
        [StateEnum("red","yellow","blue","purple","pink")]
        public string CoralColor { get; set; } = "red";
        [StateBit] public bool DeadBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral";
            record.Id = 386;
            record.States.Add(new BlockStateString {Name = "coral_color", Value = CoralColor});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class CoralBlock // 387 typeof=CoralBlock
    {
        [StateEnum("blue","pink","red","purple","yellow")]
        public string CoralColor { get; set; } = "blue";
        [StateBit] public bool DeadBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_block";
            record.Id = 387;
            record.States.Add(new BlockStateString {Name = "coral_color", Value = CoralColor});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class CoralFan // 388 typeof=CoralFan
    {
        [StateEnum("blue","yellow","purple","pink","red")]
        public string CoralColor { get; set; } = "blue";
        [StateRange(0, 1)] public int CoralFanDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_fan";
            record.Id = 388;
            record.States.Add(new BlockStateString {Name = "coral_color", Value = CoralColor});
            record.States.Add(new BlockStateInt {Name = "coral_fan_direction", Value = CoralFanDirection});
            return record;
        } // method
    } // class

    public partial class CoralFanDead // 389 typeof=CoralFanDead
    {
        [StateEnum("yellow","blue","purple","red","pink")]
        public string CoralColor { get; set; } = "yellow";
        [StateRange(0, 1)] public int CoralFanDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_fan_dead";
            record.Id = 389;
            record.States.Add(new BlockStateString {Name = "coral_color", Value = CoralColor});
            record.States.Add(new BlockStateInt {Name = "coral_fan_direction", Value = CoralFanDirection});
            return record;
        } // method
    } // class

    public partial class CoralFanHang // 390 typeof=CoralFanHang
    {
        [StateRange(0, 3)] public int CoralDirection { get; set; } = 1;
        [StateBit] public bool CoralHangTypeBit { get; set; } = true;
        [StateBit] public bool DeadBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_fan_hang";
            record.Id = 390;
            record.States.Add(new BlockStateInt {Name = "coral_direction", Value = CoralDirection});
            record.States.Add(new BlockStateByte {Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit)});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class CoralFanHang2 // 391 typeof=CoralFanHang2
    {
        [StateRange(0, 3)] public int CoralDirection { get; set; } = 2;
        [StateBit] public bool CoralHangTypeBit { get; set; } = true;
        [StateBit] public bool DeadBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_fan_hang2";
            record.Id = 391;
            record.States.Add(new BlockStateInt {Name = "coral_direction", Value = CoralDirection});
            record.States.Add(new BlockStateByte {Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit)});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class CoralFanHang3 // 392 typeof=CoralFanHang3
    {
        [StateRange(0, 3)] public int CoralDirection { get; set; } = 2;
        [StateBit] public bool CoralHangTypeBit { get; set; } = false;
        [StateBit] public bool DeadBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:coral_fan_hang3";
            record.Id = 392;
            record.States.Add(new BlockStateInt {Name = "coral_direction", Value = CoralDirection});
            record.States.Add(new BlockStateByte {Name = "coral_hang_type_bit", Value = Convert.ToByte(CoralHangTypeBit)});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class CrackedNetherBricks // 558 typeof=CrackedNetherBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:cracked_nether_bricks";
            record.Id = 558;
            return record;
        } // method
    } // class

    public partial class CrackedPolishedBlackstoneBricks // 535 typeof=CrackedPolishedBlackstoneBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:cracked_polished_blackstone_bricks";
            record.Id = 535;
            return record;
        } // method
    } // class

    public partial class CraftingTable // 58 typeof=CraftingTable
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crafting_table";
            record.Id = 58;
            return record;
        } // method
    } // class

    public partial class CrimsonButton // 515 typeof=CrimsonButton
    {
        [StateBit] public bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_button";
            record.Id = 515;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class CrimsonDoor // 499 typeof=CrimsonDoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 2;
        [StateBit] public bool DoorHingeBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = true;
        [StateBit] public bool UpperBlockBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_door";
            record.Id = 499;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class CrimsonDoubleSlab // 521 typeof=CrimsonDoubleSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_double_slab";
            record.Id = 521;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class CrimsonFence // 511 typeof=CrimsonFence
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crimson_fence";
            record.Id = 511;
            return record;
        } // method
    } // class

    public partial class CrimsonFenceGate // 513 typeof=CrimsonFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 3;
        [StateBit] public bool InWallBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_fence_gate";
            record.Id = 513;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class CrimsonFungus // 483 typeof=CrimsonFungus
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crimson_fungus";
            record.Id = 483;
            return record;
        } // method
    } // class

    public partial class CrimsonHyphae // 554 typeof=CrimsonHyphae
    {
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_hyphae";
            record.Id = 554;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class CrimsonNylium // 487 typeof=CrimsonNylium
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crimson_nylium";
            record.Id = 487;
            return record;
        } // method
    } // class

    public partial class CrimsonPlanks // 497 typeof=CrimsonPlanks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crimson_planks";
            record.Id = 497;
            return record;
        } // method
    } // class

    public partial class CrimsonPressurePlate // 517 typeof=CrimsonPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_pressure_plate";
            record.Id = 517;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class CrimsonRoots // 478 typeof=CrimsonRoots
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crimson_roots";
            record.Id = 478;
            return record;
        } // method
    } // class

    public partial class CrimsonSlab // 519 typeof=CrimsonSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_slab";
            record.Id = 519;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class CrimsonStairs // 509 typeof=CrimsonStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_stairs";
            record.Id = 509;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class CrimsonStandingSign // 505 typeof=CrimsonStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_standing_sign";
            record.Id = 505;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class CrimsonStem // 480 typeof=CrimsonStem
    {
        [StateEnum("y","x","z")]
        public string PillarAxis { get; set; } = "y";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_stem";
            record.Id = 480;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class CrimsonTrapdoor // 501 typeof=CrimsonTrapdoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool OpenBit { get; set; } = true;
        [StateBit] public bool UpsideDownBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_trapdoor";
            record.Id = 501;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class CrimsonWallSign // 507 typeof=CrimsonWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:crimson_wall_sign";
            record.Id = 507;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class CryingObsidian // 544 typeof=CryingObsidian
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:crying_obsidian";
            record.Id = 544;
            return record;
        } // method
    } // class

    public partial class CyanGlazedTerracotta // 229 typeof=CyanGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:cyan_glazed_terracotta";
            record.Id = 229;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class DarkOakButton // 397 typeof=DarkOakButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = true;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_button";
            record.Id = 397;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class DarkOakDoor // 197 typeof=DarkOakDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool DoorHingeBit { get; set; } = false;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpperBlockBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_door";
            record.Id = 197;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class DarkOakFenceGate // 186 typeof=DarkOakFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool InWallBit { get; set; } = false;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_fence_gate";
            record.Id = 186;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class DarkOakPressurePlate // 407 typeof=DarkOakPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 9;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_pressure_plate";
            record.Id = 407;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class DarkOakStairs // 164 typeof=DarkOakStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_stairs";
            record.Id = 164;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class DarkOakTrapdoor // 402 typeof=DarkOakTrapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool OpenBit { get; set; } = true;
        [StateBit] public override bool UpsideDownBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_oak_trapdoor";
            record.Id = 402;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class DarkPrismarineStairs // 258 typeof=DarkPrismarineStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dark_prismarine_stairs";
            record.Id = 258;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class DarkoakStandingSign // 447 typeof=DarkoakStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 10;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:darkoak_standing_sign";
            record.Id = 447;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class DarkoakWallSign // 448 typeof=DarkoakWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:darkoak_wall_sign";
            record.Id = 448;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class DaylightDetector // 151 typeof=DaylightDetector
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:daylight_detector";
            record.Id = 151;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class DaylightDetectorInverted // 178 typeof=DaylightDetectorInverted
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 6;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:daylight_detector_inverted";
            record.Id = 178;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class Deadbush // 32 typeof=Deadbush
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:deadbush";
            record.Id = 32;
            return record;
        } // method
    } // class

    public partial class Deny // 211 typeof=Deny
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:deny";
            record.Id = 211;
            return record;
        } // method
    } // class

    public partial class DetectorRail // 28 typeof=DetectorRail
    {
        [StateBit] public bool RailDataBit { get; set; } = false;
        [StateRange(0, 5)] public int RailDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:detector_rail";
            record.Id = 28;
            record.States.Add(new BlockStateByte {Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit)});
            record.States.Add(new BlockStateInt {Name = "rail_direction", Value = RailDirection});
            return record;
        } // method
    } // class

    public partial class DiamondBlock // 57 typeof=DiamondBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:diamond_block";
            record.Id = 57;
            return record;
        } // method
    } // class

    public partial class DiamondOre // 56 typeof=DiamondOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:diamond_ore";
            record.Id = 56;
            return record;
        } // method
    } // class

    public partial class DioriteStairs // 425 typeof=DioriteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:diorite_stairs";
            record.Id = 425;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Dirt // 3 typeof=Dirt
    {
        [StateEnum("normal","coarse")]
        public string DirtType { get; set; } = "normal";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dirt";
            record.Id = 3;
            record.States.Add(new BlockStateString {Name = "dirt_type", Value = DirtType});
            return record;
        } // method
    } // class

    public partial class Dispenser // 23 typeof=Dispenser
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;
        [StateBit] public bool TriggeredBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dispenser";
            record.Id = 23;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit)});
            return record;
        } // method
    } // class

    public partial class DoublePlant // 175 typeof=DoublePlant
    {
        [StateEnum("paeonia","syringa","grass","sunflower","fern","rose")]
        public string DoublePlantType { get; set; } = "paeonia";
        [StateBit] public bool UpperBlockBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_plant";
            record.Id = 175;
            record.States.Add(new BlockStateString {Name = "double_plant_type", Value = DoublePlantType});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class DoubleStoneSlab // 43 typeof=DoubleStoneSlab
    {
        [StateEnum("cobblestone","nether_brick","smooth_stone","brick","stone_brick","sandstone","quartz","wood")]
        public string StoneSlabType { get; set; } = "cobblestone";
        [StateBit] public bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_stone_slab";
            record.Id = 43;
            record.States.Add(new BlockStateString {Name = "stone_slab_type", Value = StoneSlabType});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class DoubleStoneSlab2 // 181 typeof=DoubleStoneSlab2
    {
        [StateEnum("prismarine_rough","red_nether_brick","red_sandstone","purpur","prismarine_dark","prismarine_brick","mossy_cobblestone","smooth_sandstone")]
        public string StoneSlabType2 { get; set; } = "prismarine_rough";
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_stone_slab2";
            record.Id = 181;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_2", Value = StoneSlabType2});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class DoubleStoneSlab3 // 422 typeof=DoubleStoneSlab3
    {
        [StateEnum("granite","smooth_red_sandstone","polished_andesite","polished_diorite","end_stone_brick","diorite","polished_granite","andesite")]
        public string StoneSlabType3 { get; set; } = "granite";
        [StateBit] public bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_stone_slab3";
            record.Id = 422;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_3", Value = StoneSlabType3});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class DoubleStoneSlab4 // 423 typeof=DoubleStoneSlab4
    {
        [StateEnum("smooth_quartz","mossy_stone_brick","stone","cut_red_sandstone","cut_sandstone")]
        public string StoneSlabType4 { get; set; } = "smooth_quartz";
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_stone_slab4";
            record.Id = 423;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_4", Value = StoneSlabType4});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class DoubleWoodenSlab // 157 typeof=DoubleWoodenSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = true;
        [StateEnum("jungle","birch","acacia","oak","dark_oak","spruce")]
        public string WoodType { get; set; } = "jungle";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:double_wooden_slab";
            record.Id = 157;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            record.States.Add(new BlockStateString {Name = "wood_type", Value = WoodType});
            return record;
        } // method
    } // class

    public partial class DragonEgg // 122 typeof=DragonEgg
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:dragon_egg";
            record.Id = 122;
            return record;
        } // method
    } // class

    public partial class DriedKelpBlock // 394 typeof=DriedKelpBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:dried_kelp_block";
            record.Id = 394;
            return record;
        } // method
    } // class

    public partial class Dropper // 125 typeof=Dropper
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 3;
        [StateBit] public bool TriggeredBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:dropper";
            record.Id = 125;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "triggered_bit", Value = Convert.ToByte(TriggeredBit)});
            return record;
        } // method
    } // class

    public partial class Element0 // 36 typeof=Element0
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_0";
            record.Id = 36;
            return record;
        } // method
    } // class

    public partial class Element1 // 267 typeof=Element1
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_1";
            record.Id = 267;
            return record;
        } // method
    } // class

    public partial class Element10 // 276 typeof=Element10
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_10";
            record.Id = 276;
            return record;
        } // method
    } // class

    public partial class Element100 // 366 typeof=Element100
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_100";
            record.Id = 366;
            return record;
        } // method
    } // class

    public partial class Element101 // 367 typeof=Element101
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_101";
            record.Id = 367;
            return record;
        } // method
    } // class

    public partial class Element102 // 368 typeof=Element102
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_102";
            record.Id = 368;
            return record;
        } // method
    } // class

    public partial class Element103 // 369 typeof=Element103
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_103";
            record.Id = 369;
            return record;
        } // method
    } // class

    public partial class Element104 // 370 typeof=Element104
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_104";
            record.Id = 370;
            return record;
        } // method
    } // class

    public partial class Element105 // 371 typeof=Element105
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_105";
            record.Id = 371;
            return record;
        } // method
    } // class

    public partial class Element106 // 372 typeof=Element106
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_106";
            record.Id = 372;
            return record;
        } // method
    } // class

    public partial class Element107 // 373 typeof=Element107
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_107";
            record.Id = 373;
            return record;
        } // method
    } // class

    public partial class Element108 // 374 typeof=Element108
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_108";
            record.Id = 374;
            return record;
        } // method
    } // class

    public partial class Element109 // 375 typeof=Element109
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_109";
            record.Id = 375;
            return record;
        } // method
    } // class

    public partial class Element11 // 277 typeof=Element11
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_11";
            record.Id = 277;
            return record;
        } // method
    } // class

    public partial class Element110 // 376 typeof=Element110
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_110";
            record.Id = 376;
            return record;
        } // method
    } // class

    public partial class Element111 // 377 typeof=Element111
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_111";
            record.Id = 377;
            return record;
        } // method
    } // class

    public partial class Element112 // 378 typeof=Element112
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_112";
            record.Id = 378;
            return record;
        } // method
    } // class

    public partial class Element113 // 379 typeof=Element113
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_113";
            record.Id = 379;
            return record;
        } // method
    } // class

    public partial class Element114 // 380 typeof=Element114
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_114";
            record.Id = 380;
            return record;
        } // method
    } // class

    public partial class Element115 // 381 typeof=Element115
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_115";
            record.Id = 381;
            return record;
        } // method
    } // class

    public partial class Element116 // 382 typeof=Element116
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_116";
            record.Id = 382;
            return record;
        } // method
    } // class

    public partial class Element117 // 383 typeof=Element117
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_117";
            record.Id = 383;
            return record;
        } // method
    } // class

    public partial class Element118 // 384 typeof=Element118
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_118";
            record.Id = 384;
            return record;
        } // method
    } // class

    public partial class Element12 // 278 typeof=Element12
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_12";
            record.Id = 278;
            return record;
        } // method
    } // class

    public partial class Element13 // 279 typeof=Element13
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_13";
            record.Id = 279;
            return record;
        } // method
    } // class

    public partial class Element14 // 280 typeof=Element14
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_14";
            record.Id = 280;
            return record;
        } // method
    } // class

    public partial class Element15 // 281 typeof=Element15
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_15";
            record.Id = 281;
            return record;
        } // method
    } // class

    public partial class Element16 // 282 typeof=Element16
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_16";
            record.Id = 282;
            return record;
        } // method
    } // class

    public partial class Element17 // 283 typeof=Element17
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_17";
            record.Id = 283;
            return record;
        } // method
    } // class

    public partial class Element18 // 284 typeof=Element18
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_18";
            record.Id = 284;
            return record;
        } // method
    } // class

    public partial class Element19 // 285 typeof=Element19
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_19";
            record.Id = 285;
            return record;
        } // method
    } // class

    public partial class Element2 // 268 typeof=Element2
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_2";
            record.Id = 268;
            return record;
        } // method
    } // class

    public partial class Element20 // 286 typeof=Element20
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_20";
            record.Id = 286;
            return record;
        } // method
    } // class

    public partial class Element21 // 287 typeof=Element21
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_21";
            record.Id = 287;
            return record;
        } // method
    } // class

    public partial class Element22 // 288 typeof=Element22
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_22";
            record.Id = 288;
            return record;
        } // method
    } // class

    public partial class Element23 // 289 typeof=Element23
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_23";
            record.Id = 289;
            return record;
        } // method
    } // class

    public partial class Element24 // 290 typeof=Element24
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_24";
            record.Id = 290;
            return record;
        } // method
    } // class

    public partial class Element25 // 291 typeof=Element25
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_25";
            record.Id = 291;
            return record;
        } // method
    } // class

    public partial class Element26 // 292 typeof=Element26
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_26";
            record.Id = 292;
            return record;
        } // method
    } // class

    public partial class Element27 // 293 typeof=Element27
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_27";
            record.Id = 293;
            return record;
        } // method
    } // class

    public partial class Element28 // 294 typeof=Element28
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_28";
            record.Id = 294;
            return record;
        } // method
    } // class

    public partial class Element29 // 295 typeof=Element29
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_29";
            record.Id = 295;
            return record;
        } // method
    } // class

    public partial class Element3 // 269 typeof=Element3
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_3";
            record.Id = 269;
            return record;
        } // method
    } // class

    public partial class Element30 // 296 typeof=Element30
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_30";
            record.Id = 296;
            return record;
        } // method
    } // class

    public partial class Element31 // 297 typeof=Element31
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_31";
            record.Id = 297;
            return record;
        } // method
    } // class

    public partial class Element32 // 298 typeof=Element32
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_32";
            record.Id = 298;
            return record;
        } // method
    } // class

    public partial class Element33 // 299 typeof=Element33
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_33";
            record.Id = 299;
            return record;
        } // method
    } // class

    public partial class Element34 // 300 typeof=Element34
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_34";
            record.Id = 300;
            return record;
        } // method
    } // class

    public partial class Element35 // 301 typeof=Element35
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_35";
            record.Id = 301;
            return record;
        } // method
    } // class

    public partial class Element36 // 302 typeof=Element36
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_36";
            record.Id = 302;
            return record;
        } // method
    } // class

    public partial class Element37 // 303 typeof=Element37
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_37";
            record.Id = 303;
            return record;
        } // method
    } // class

    public partial class Element38 // 304 typeof=Element38
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_38";
            record.Id = 304;
            return record;
        } // method
    } // class

    public partial class Element39 // 305 typeof=Element39
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_39";
            record.Id = 305;
            return record;
        } // method
    } // class

    public partial class Element4 // 270 typeof=Element4
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_4";
            record.Id = 270;
            return record;
        } // method
    } // class

    public partial class Element40 // 306 typeof=Element40
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_40";
            record.Id = 306;
            return record;
        } // method
    } // class

    public partial class Element41 // 307 typeof=Element41
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_41";
            record.Id = 307;
            return record;
        } // method
    } // class

    public partial class Element42 // 308 typeof=Element42
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_42";
            record.Id = 308;
            return record;
        } // method
    } // class

    public partial class Element43 // 309 typeof=Element43
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_43";
            record.Id = 309;
            return record;
        } // method
    } // class

    public partial class Element44 // 310 typeof=Element44
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_44";
            record.Id = 310;
            return record;
        } // method
    } // class

    public partial class Element45 // 311 typeof=Element45
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_45";
            record.Id = 311;
            return record;
        } // method
    } // class

    public partial class Element46 // 312 typeof=Element46
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_46";
            record.Id = 312;
            return record;
        } // method
    } // class

    public partial class Element47 // 313 typeof=Element47
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_47";
            record.Id = 313;
            return record;
        } // method
    } // class

    public partial class Element48 // 314 typeof=Element48
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_48";
            record.Id = 314;
            return record;
        } // method
    } // class

    public partial class Element49 // 315 typeof=Element49
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_49";
            record.Id = 315;
            return record;
        } // method
    } // class

    public partial class Element5 // 271 typeof=Element5
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_5";
            record.Id = 271;
            return record;
        } // method
    } // class

    public partial class Element50 // 316 typeof=Element50
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_50";
            record.Id = 316;
            return record;
        } // method
    } // class

    public partial class Element51 // 317 typeof=Element51
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_51";
            record.Id = 317;
            return record;
        } // method
    } // class

    public partial class Element52 // 318 typeof=Element52
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_52";
            record.Id = 318;
            return record;
        } // method
    } // class

    public partial class Element53 // 319 typeof=Element53
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_53";
            record.Id = 319;
            return record;
        } // method
    } // class

    public partial class Element54 // 320 typeof=Element54
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_54";
            record.Id = 320;
            return record;
        } // method
    } // class

    public partial class Element55 // 321 typeof=Element55
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_55";
            record.Id = 321;
            return record;
        } // method
    } // class

    public partial class Element56 // 322 typeof=Element56
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_56";
            record.Id = 322;
            return record;
        } // method
    } // class

    public partial class Element57 // 323 typeof=Element57
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_57";
            record.Id = 323;
            return record;
        } // method
    } // class

    public partial class Element58 // 324 typeof=Element58
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_58";
            record.Id = 324;
            return record;
        } // method
    } // class

    public partial class Element59 // 325 typeof=Element59
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_59";
            record.Id = 325;
            return record;
        } // method
    } // class

    public partial class Element6 // 272 typeof=Element6
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_6";
            record.Id = 272;
            return record;
        } // method
    } // class

    public partial class Element60 // 326 typeof=Element60
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_60";
            record.Id = 326;
            return record;
        } // method
    } // class

    public partial class Element61 // 327 typeof=Element61
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_61";
            record.Id = 327;
            return record;
        } // method
    } // class

    public partial class Element62 // 328 typeof=Element62
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_62";
            record.Id = 328;
            return record;
        } // method
    } // class

    public partial class Element63 // 329 typeof=Element63
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_63";
            record.Id = 329;
            return record;
        } // method
    } // class

    public partial class Element64 // 330 typeof=Element64
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_64";
            record.Id = 330;
            return record;
        } // method
    } // class

    public partial class Element65 // 331 typeof=Element65
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_65";
            record.Id = 331;
            return record;
        } // method
    } // class

    public partial class Element66 // 332 typeof=Element66
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_66";
            record.Id = 332;
            return record;
        } // method
    } // class

    public partial class Element67 // 333 typeof=Element67
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_67";
            record.Id = 333;
            return record;
        } // method
    } // class

    public partial class Element68 // 334 typeof=Element68
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_68";
            record.Id = 334;
            return record;
        } // method
    } // class

    public partial class Element69 // 335 typeof=Element69
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_69";
            record.Id = 335;
            return record;
        } // method
    } // class

    public partial class Element7 // 273 typeof=Element7
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_7";
            record.Id = 273;
            return record;
        } // method
    } // class

    public partial class Element70 // 336 typeof=Element70
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_70";
            record.Id = 336;
            return record;
        } // method
    } // class

    public partial class Element71 // 337 typeof=Element71
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_71";
            record.Id = 337;
            return record;
        } // method
    } // class

    public partial class Element72 // 338 typeof=Element72
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_72";
            record.Id = 338;
            return record;
        } // method
    } // class

    public partial class Element73 // 339 typeof=Element73
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_73";
            record.Id = 339;
            return record;
        } // method
    } // class

    public partial class Element74 // 340 typeof=Element74
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_74";
            record.Id = 340;
            return record;
        } // method
    } // class

    public partial class Element75 // 341 typeof=Element75
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_75";
            record.Id = 341;
            return record;
        } // method
    } // class

    public partial class Element76 // 342 typeof=Element76
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_76";
            record.Id = 342;
            return record;
        } // method
    } // class

    public partial class Element77 // 343 typeof=Element77
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_77";
            record.Id = 343;
            return record;
        } // method
    } // class

    public partial class Element78 // 344 typeof=Element78
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_78";
            record.Id = 344;
            return record;
        } // method
    } // class

    public partial class Element79 // 345 typeof=Element79
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_79";
            record.Id = 345;
            return record;
        } // method
    } // class

    public partial class Element8 // 274 typeof=Element8
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_8";
            record.Id = 274;
            return record;
        } // method
    } // class

    public partial class Element80 // 346 typeof=Element80
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_80";
            record.Id = 346;
            return record;
        } // method
    } // class

    public partial class Element81 // 347 typeof=Element81
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_81";
            record.Id = 347;
            return record;
        } // method
    } // class

    public partial class Element82 // 348 typeof=Element82
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_82";
            record.Id = 348;
            return record;
        } // method
    } // class

    public partial class Element83 // 349 typeof=Element83
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_83";
            record.Id = 349;
            return record;
        } // method
    } // class

    public partial class Element84 // 350 typeof=Element84
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_84";
            record.Id = 350;
            return record;
        } // method
    } // class

    public partial class Element85 // 351 typeof=Element85
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_85";
            record.Id = 351;
            return record;
        } // method
    } // class

    public partial class Element86 // 352 typeof=Element86
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_86";
            record.Id = 352;
            return record;
        } // method
    } // class

    public partial class Element87 // 353 typeof=Element87
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_87";
            record.Id = 353;
            return record;
        } // method
    } // class

    public partial class Element88 // 354 typeof=Element88
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_88";
            record.Id = 354;
            return record;
        } // method
    } // class

    public partial class Element89 // 355 typeof=Element89
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_89";
            record.Id = 355;
            return record;
        } // method
    } // class

    public partial class Element9 // 275 typeof=Element9
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_9";
            record.Id = 275;
            return record;
        } // method
    } // class

    public partial class Element90 // 356 typeof=Element90
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_90";
            record.Id = 356;
            return record;
        } // method
    } // class

    public partial class Element91 // 357 typeof=Element91
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_91";
            record.Id = 357;
            return record;
        } // method
    } // class

    public partial class Element92 // 358 typeof=Element92
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_92";
            record.Id = 358;
            return record;
        } // method
    } // class

    public partial class Element93 // 359 typeof=Element93
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_93";
            record.Id = 359;
            return record;
        } // method
    } // class

    public partial class Element94 // 360 typeof=Element94
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_94";
            record.Id = 360;
            return record;
        } // method
    } // class

    public partial class Element95 // 361 typeof=Element95
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_95";
            record.Id = 361;
            return record;
        } // method
    } // class

    public partial class Element96 // 362 typeof=Element96
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_96";
            record.Id = 362;
            return record;
        } // method
    } // class

    public partial class Element97 // 363 typeof=Element97
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_97";
            record.Id = 363;
            return record;
        } // method
    } // class

    public partial class Element98 // 364 typeof=Element98
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_98";
            record.Id = 364;
            return record;
        } // method
    } // class

    public partial class Element99 // 365 typeof=Element99
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:element_99";
            record.Id = 365;
            return record;
        } // method
    } // class

    public partial class EmeraldBlock // 133 typeof=EmeraldBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:emerald_block";
            record.Id = 133;
            return record;
        } // method
    } // class

    public partial class EmeraldOre // 129 typeof=EmeraldOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:emerald_ore";
            record.Id = 129;
            return record;
        } // method
    } // class

    public partial class EnchantingTable // 116 typeof=EnchantingTable
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:enchanting_table";
            record.Id = 116;
            return record;
        } // method
    } // class

    public partial class EndBrickStairs // 433 typeof=EndBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:end_brick_stairs";
            record.Id = 433;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class EndBricks // 206 typeof=EndBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:end_bricks";
            record.Id = 206;
            return record;
        } // method
    } // class

    public partial class EndGateway // 209 typeof=EndGateway
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:end_gateway";
            record.Id = 209;
            return record;
        } // method
    } // class

    public partial class EndPortal // 119 typeof=EndPortal
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:end_portal";
            record.Id = 119;
            return record;
        } // method
    } // class

    public partial class EndPortalFrame // 120 typeof=EndPortalFrame
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool EndPortalEyeBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:end_portal_frame";
            record.Id = 120;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "end_portal_eye_bit", Value = Convert.ToByte(EndPortalEyeBit)});
            return record;
        } // method
    } // class

    public partial class EndRod // 208 typeof=EndRod
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:end_rod";
            record.Id = 208;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class EndStone // 121 typeof=EndStone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:end_stone";
            record.Id = 121;
            return record;
        } // method
    } // class

    public partial class EnderChest // 130 typeof=EnderChest
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:ender_chest";
            record.Id = 130;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Farmland // 60 typeof=Farmland
    {
        [StateRange(0, 7)] public int MoisturizedAmount { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:farmland";
            record.Id = 60;
            record.States.Add(new BlockStateInt {Name = "moisturized_amount", Value = MoisturizedAmount});
            return record;
        } // method
    } // class

    public partial class Fence // 85 typeof=Fence
    {
        [StateEnum("birch","dark_oak","acacia","oak","spruce","jungle")]
        public string WoodType { get; set; } = "birch";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:fence";
            record.Id = 85;
            record.States.Add(new BlockStateString {Name = "wood_type", Value = WoodType});
            return record;
        } // method
    } // class

    public partial class FenceGate // 107 typeof=FenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool InWallBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:fence_gate";
            record.Id = 107;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class Fire // 51 typeof=Fire
    {
        [StateRange(0, 15)] public int Age { get; set; } = 9;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:fire";
            record.Id = 51;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class FletchingTable // 456 typeof=FletchingTable
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:fletching_table";
            record.Id = 456;
            return record;
        } // method
    } // class

    public partial class FlowerPot // 140 typeof=FlowerPot
    {
        [StateBit] public bool UpdateBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:flower_pot";
            record.Id = 140;
            record.States.Add(new BlockStateByte {Name = "update_bit", Value = Convert.ToByte(UpdateBit)});
            return record;
        } // method
    } // class

    public partial class FlowingLava // 10 typeof=FlowingLava
    {
        [StateRange(0, 15)] public override int LiquidDepth { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:flowing_lava";
            record.Id = 10;
            record.States.Add(new BlockStateInt {Name = "liquid_depth", Value = LiquidDepth});
            return record;
        } // method
    } // class

    public partial class FlowingWater // 8 typeof=FlowingWater
    {
        [StateRange(0, 15)] public override int LiquidDepth { get; set; } = 8;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:flowing_water";
            record.Id = 8;
            record.States.Add(new BlockStateInt {Name = "liquid_depth", Value = LiquidDepth});
            return record;
        } // method
    } // class

    public partial class Frame // 199 typeof=Frame
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
        [StateBit] public bool ItemFrameMapBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateInt s when s.Name == "facing_direction":
                        FacingDirection = s.Value;
                        break;
                    case BlockStateByte s when s.Name == "item_frame_map_bit":
                        ItemFrameMapBit = Convert.ToBoolean(s.Value);
                        break;
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:frame";
            record.Id = 199;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "item_frame_map_bit", Value = Convert.ToByte(ItemFrameMapBit)});
            return record;
        } // method
    } // class

    public partial class FrostedIce // 207 typeof=FrostedIce
    {
        [StateRange(0, 3)] public int Age { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:frosted_ice";
            record.Id = 207;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class Furnace // 61 typeof=Furnace
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:furnace";
            record.Id = 61;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class GildedBlackstone // 536 typeof=GildedBlackstone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:gilded_blackstone";
            record.Id = 536;
            return record;
        } // method
    } // class

    public partial class Glass // 20 typeof=Glass
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:glass";
            record.Id = 20;
            return record;
        } // method
    } // class

    public partial class GlassPane // 102 typeof=GlassPane
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:glass_pane";
            record.Id = 102;
            return record;
        } // method
    } // class

    public partial class Glowingobsidian // 246 typeof=Glowingobsidian
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:glowingobsidian";
            record.Id = 246;
            return record;
        } // method
    } // class

    public partial class Glowstone // 89 typeof=Glowstone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:glowstone";
            record.Id = 89;
            return record;
        } // method
    } // class

    public partial class GoldBlock // 41 typeof=GoldBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:gold_block";
            record.Id = 41;
            return record;
        } // method
    } // class

    public partial class GoldOre // 14 typeof=GoldOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:gold_ore";
            record.Id = 14;
            return record;
        } // method
    } // class

    public partial class GoldenRail // 27 typeof=GoldenRail
    {
        [StateBit] public bool RailDataBit { get; set; } = false;
        [StateRange(0, 5)] public int RailDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:golden_rail";
            record.Id = 27;
            record.States.Add(new BlockStateByte {Name = "rail_data_bit", Value = Convert.ToByte(RailDataBit)});
            record.States.Add(new BlockStateInt {Name = "rail_direction", Value = RailDirection});
            return record;
        } // method
    } // class

    public partial class GraniteStairs // 424 typeof=GraniteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:granite_stairs";
            record.Id = 424;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Grass // 2 typeof=Grass
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:grass";
            record.Id = 2;
            return record;
        } // method
    } // class

    public partial class GrassPath // 198 typeof=GrassPath
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:grass_path";
            record.Id = 198;
            return record;
        } // method
    } // class

    public partial class Gravel // 13 typeof=Gravel
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:gravel";
            record.Id = 13;
            return record;
        } // method
    } // class

    public partial class GrayGlazedTerracotta // 227 typeof=GrayGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:gray_glazed_terracotta";
            record.Id = 227;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class GreenGlazedTerracotta // 233 typeof=GreenGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:green_glazed_terracotta";
            record.Id = 233;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Grindstone // 450 typeof=Grindstone
    {
        [StateEnum("hanging","multiple","side","standing")]
        public string Attachment { get; set; } = "hanging";
        [StateRange(0, 3)] public int Direction { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:grindstone";
            record.Id = 450;
            record.States.Add(new BlockStateString {Name = "attachment", Value = Attachment});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class HardGlass // 253 typeof=HardGlass
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:hard_glass";
            record.Id = 253;
            return record;
        } // method
    } // class

    public partial class HardGlassPane // 190 typeof=HardGlassPane
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:hard_glass_pane";
            record.Id = 190;
            return record;
        } // method
    } // class

    public partial class HardStainedGlass // 254 typeof=HardStainedGlass
    {
        [StateEnum("pink","light_blue","magenta","black","brown","silver","green","orange","cyan","gray","blue","white","lime","yellow","purple","red")]
        public string Color { get; set; } = "pink";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:hard_stained_glass";
            record.Id = 254;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class HardStainedGlassPane // 191 typeof=HardStainedGlassPane
    {
        [StateEnum("silver","lime","blue","red","gray","purple","black","brown","pink","green","yellow","magenta","orange","white","light_blue","cyan")]
        public string Color { get; set; } = "silver";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:hard_stained_glass_pane";
            record.Id = 191;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class HardenedClay // 172 typeof=HardenedClay
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:hardened_clay";
            record.Id = 172;
            return record;
        } // method
    } // class

    public partial class HayBlock // 170 typeof=HayBlock
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 1;
        [StateEnum("y","z","x")]
        public string PillarAxis { get; set; } = "y";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:hay_block";
            record.Id = 170;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class HeavyWeightedPressurePlate // 148 typeof=HeavyWeightedPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:heavy_weighted_pressure_plate";
            record.Id = 148;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class HoneyBlock // 475 typeof=HoneyBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:honey_block";
            record.Id = 475;
            return record;
        } // method
    } // class

    public partial class HoneycombBlock // 476 typeof=HoneycombBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:honeycomb_block";
            record.Id = 476;
            return record;
        } // method
    } // class

    public partial class Hopper // 154 typeof=Hopper
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
        [StateBit] public bool ToggleBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:hopper";
            record.Id = 154;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "toggle_bit", Value = Convert.ToByte(ToggleBit)});
            return record;
        } // method
    } // class

    public partial class Ice // 79 typeof=Ice
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:ice";
            record.Id = 79;
            return record;
        } // method
    } // class

    public partial class InfoUpdate // 248 typeof=InfoUpdate
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:info_update";
            record.Id = 248;
            return record;
        } // method
    } // class

    public partial class InfoUpdate2 // 249 typeof=InfoUpdate2
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:info_update2";
            record.Id = 249;
            return record;
        } // method
    } // class

    public partial class InvisibleBedrock // 95 typeof=InvisibleBedrock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:invisibleBedrock";
            record.Id = 95;
            return record;
        } // method
    } // class

    public partial class IronBars // 101 typeof=IronBars
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:iron_bars";
            record.Id = 101;
            return record;
        } // method
    } // class

    public partial class IronBlock // 42 typeof=IronBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:iron_block";
            record.Id = 42;
            return record;
        } // method
    } // class

    public partial class IronDoor // 71 typeof=IronDoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 3;
        [StateBit] public bool DoorHingeBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = false;
        [StateBit] public bool UpperBlockBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:iron_door";
            record.Id = 71;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class IronOre // 15 typeof=IronOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:iron_ore";
            record.Id = 15;
            return record;
        } // method
    } // class

    public partial class IronTrapdoor // 167 typeof=IronTrapdoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool OpenBit { get; set; } = false;
        [StateBit] public bool UpsideDownBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:iron_trapdoor";
            record.Id = 167;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class Jigsaw // 466 typeof=Jigsaw
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;
        [StateRange(0, 3)] public int Rotation { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jigsaw";
            record.Id = 466;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateInt {Name = "rotation", Value = Rotation});
            return record;
        } // method
    } // class

    public partial class Jukebox // 84 typeof=Jukebox
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:jukebox";
            record.Id = 84;
            return record;
        } // method
    } // class

    public partial class JungleButton // 398 typeof=JungleButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_button";
            record.Id = 398;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class JungleDoor // 195 typeof=JungleDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 1;
        [StateBit] public override bool DoorHingeBit { get; set; } = false;
        [StateBit] public override bool OpenBit { get; set; } = true;
        [StateBit] public override bool UpperBlockBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_door";
            record.Id = 195;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class JungleFenceGate // 185 typeof=JungleFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool InWallBit { get; set; } = false;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_fence_gate";
            record.Id = 185;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class JunglePressurePlate // 408 typeof=JunglePressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_pressure_plate";
            record.Id = 408;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class JungleStairs // 136 typeof=JungleStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_stairs";
            record.Id = 136;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class JungleStandingSign // 443 typeof=JungleStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_standing_sign";
            record.Id = 443;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class JungleTrapdoor // 403 typeof=JungleTrapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 0;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpsideDownBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_trapdoor";
            record.Id = 403;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class JungleWallSign // 444 typeof=JungleWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:jungle_wall_sign";
            record.Id = 444;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Kelp // 393 typeof=Kelp
    {
        [StateRange(0, 25)] public int KelpAge { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:kelp";
            record.Id = 393;
            record.States.Add(new BlockStateInt {Name = "kelp_age", Value = KelpAge});
            return record;
        } // method
    } // class

    public partial class Ladder // 65 typeof=Ladder
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:ladder";
            record.Id = 65;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Lantern // 463 typeof=Lantern
    {
        [StateBit] public bool Hanging { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lantern";
            record.Id = 463;
            record.States.Add(new BlockStateByte {Name = "hanging", Value = Convert.ToByte(Hanging)});
            return record;
        } // method
    } // class

    public partial class LapisBlock // 22 typeof=LapisBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:lapis_block";
            record.Id = 22;
            return record;
        } // method
    } // class

    public partial class LapisOre // 21 typeof=LapisOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:lapis_ore";
            record.Id = 21;
            return record;
        } // method
    } // class

    public partial class Lava // 11 typeof=Lava
    {
        [StateRange(0, 15)] public override int LiquidDepth { get; set; } = 14;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lava";
            record.Id = 11;
            record.States.Add(new BlockStateInt {Name = "liquid_depth", Value = LiquidDepth});
            return record;
        } // method
    } // class

    public partial class LavaCauldron // 465 typeof=LavaCauldron
    {
        [StateEnum("water","lava")]
        public string CauldronLiquid { get; set; } = "water";
        [StateRange(0, 6)] public int FillLevel { get; set; } = 6;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lava_cauldron";
            record.Id = 465;
            record.States.Add(new BlockStateString {Name = "cauldron_liquid", Value = CauldronLiquid});
            record.States.Add(new BlockStateInt {Name = "fill_level", Value = FillLevel});
            return record;
        } // method
    } // class

    public partial class Leaves // 18 typeof=Leaves
    {
        [StateEnum("birch","jungle","spruce","oak")]
        public string OldLeafType { get; set; } = "birch";
        [StateBit] public bool PersistentBit { get; set; } = true;
        [StateBit] public bool UpdateBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:leaves";
            record.Id = 18;
            record.States.Add(new BlockStateString {Name = "old_leaf_type", Value = OldLeafType});
            record.States.Add(new BlockStateByte {Name = "persistent_bit", Value = Convert.ToByte(PersistentBit)});
            record.States.Add(new BlockStateByte {Name = "update_bit", Value = Convert.ToByte(UpdateBit)});
            return record;
        } // method
    } // class

    public partial class Leaves2 // 161 typeof=Leaves2
    {
        [StateEnum("dark_oak","acacia")]
        public string NewLeafType { get; set; } = "dark_oak";
        [StateBit] public bool PersistentBit { get; set; } = true;
        [StateBit] public bool UpdateBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:leaves2";
            record.Id = 161;
            record.States.Add(new BlockStateString {Name = "new_leaf_type", Value = NewLeafType});
            record.States.Add(new BlockStateByte {Name = "persistent_bit", Value = Convert.ToByte(PersistentBit)});
            record.States.Add(new BlockStateByte {Name = "update_bit", Value = Convert.ToByte(UpdateBit)});
            return record;
        } // method
    } // class

    public partial class Lectern // 449 typeof=Lectern
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 3;
        [StateBit] public bool PoweredBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lectern";
            record.Id = 449;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "powered_bit", Value = Convert.ToByte(PoweredBit)});
            return record;
        } // method
    } // class

    public partial class Lever // 69 typeof=Lever
    {
        [StateEnum("up_east_west","down_east_west","south","down_north_south","east","up_north_south","north","west")]
        public string LeverDirection { get; set; } = "up_east_west";
        [StateBit] public bool OpenBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lever";
            record.Id = 69;
            record.States.Add(new BlockStateString {Name = "lever_direction", Value = LeverDirection});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class LightBlock // 470 typeof=LightBlock
    {
        [StateRange(0, 15)] public int BlockLightLevel { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:light_block";
            record.Id = 470;
            record.States.Add(new BlockStateInt {Name = "block_light_level", Value = BlockLightLevel});
            return record;
        } // method
    } // class

    public partial class LightBlueGlazedTerracotta // 223 typeof=LightBlueGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:light_blue_glazed_terracotta";
            record.Id = 223;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class LightWeightedPressurePlate // 147 typeof=LightWeightedPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:light_weighted_pressure_plate";
            record.Id = 147;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class LimeGlazedTerracotta // 225 typeof=LimeGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lime_glazed_terracotta";
            record.Id = 225;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class LitBlastFurnace // 469 typeof=LitBlastFurnace
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lit_blast_furnace";
            record.Id = 469;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class LitFurnace // 62 typeof=LitFurnace
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lit_furnace";
            record.Id = 62;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class LitPumpkin // 91 typeof=LitPumpkin
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lit_pumpkin";
            record.Id = 91;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class LitRedstoneLamp // 124 typeof=LitRedstoneLamp
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:lit_redstone_lamp";
            record.Id = 124;
            return record;
        } // method
    } // class

    public partial class LitRedstoneOre // 74 typeof=LitRedstoneOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:lit_redstone_ore";
            record.Id = 74;
            return record;
        } // method
    } // class

    public partial class LitSmoker // 454 typeof=LitSmoker
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:lit_smoker";
            record.Id = 454;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Lodestone // 477 typeof=Lodestone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:lodestone";
            record.Id = 477;
            return record;
        } // method
    } // class

    public partial class Log // 17 typeof=Log
    {
        [StateEnum("oak","birch","jungle","spruce")]
        public string OldLogType { get; set; } = "oak";
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateString s when s.Name == "old_log_type":
                        OldLogType = s.Value;
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
            record.Name = "minecraft:log";
            record.Id = 17;
            record.States.Add(new BlockStateString {Name = "old_log_type", Value = OldLogType});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class Log2 // 162 typeof=Log2
    {
        [StateEnum("dark_oak","acacia")]
        public string NewLogType { get; set; } = "dark_oak";
        [StateEnum("y","z","x")]
        public string PillarAxis { get; set; } = "y";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateString s when s.Name == "new_log_type":
                        NewLogType = s.Value;
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
            record.Name = "minecraft:log2";
            record.Id = 162;
            record.States.Add(new BlockStateString {Name = "new_log_type", Value = NewLogType});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class Loom // 459 typeof=Loom
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:loom";
            record.Id = 459;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class MagentaGlazedTerracotta // 222 typeof=MagentaGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:magenta_glazed_terracotta";
            record.Id = 222;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Magma // 213 typeof=Magma
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:magma";
            record.Id = 213;
            return record;
        } // method
    } // class

    public partial class MelonBlock // 103 typeof=MelonBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:melon_block";
            record.Id = 103;
            return record;
        } // method
    } // class

    public partial class MelonStem // 105 typeof=MelonStem
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;
        [StateRange(0, 7)] public int Growth { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:melon_stem";
            record.Id = 105;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class MobSpawner // 52 typeof=MobSpawner
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:mob_spawner";
            record.Id = 52;
            return record;
        } // method
    } // class

    public partial class MonsterEgg // 97 typeof=MonsterEgg
    {
        [StateEnum("stone_brick","cobblestone","chiseled_stone_brick","cracked_stone_brick","mossy_stone_brick","stone")]
        public string MonsterEggStoneType { get; set; } = "stone_brick";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:monster_egg";
            record.Id = 97;
            record.States.Add(new BlockStateString {Name = "monster_egg_stone_type", Value = MonsterEggStoneType});
            return record;
        } // method
    } // class

    public partial class MossyCobblestone // 48 typeof=MossyCobblestone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:mossy_cobblestone";
            record.Id = 48;
            return record;
        } // method
    } // class

    public partial class MossyCobblestoneStairs // 434 typeof=MossyCobblestoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:mossy_cobblestone_stairs";
            record.Id = 434;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class MossyStoneBrickStairs // 430 typeof=MossyStoneBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:mossy_stone_brick_stairs";
            record.Id = 430;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class MovingBlock // 250 typeof=MovingBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:movingBlock";
            record.Id = 250;
            return record;
        } // method
    } // class

    public partial class Mycelium // 110 typeof=Mycelium
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:mycelium";
            record.Id = 110;
            return record;
        } // method
    } // class

    public partial class NetherBrick // 112 typeof=NetherBrick
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:nether_brick";
            record.Id = 112;
            return record;
        } // method
    } // class

    public partial class NetherBrickFence // 113 typeof=NetherBrickFence
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:nether_brick_fence";
            record.Id = 113;
            return record;
        } // method
    } // class

    public partial class NetherBrickStairs // 114 typeof=NetherBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:nether_brick_stairs";
            record.Id = 114;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class NetherGoldOre // 543 typeof=NetherGoldOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:nether_gold_ore";
            record.Id = 543;
            return record;
        } // method
    } // class

    public partial class NetherSprouts // 493 typeof=NetherSprouts
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:nether_sprouts";
            record.Id = 493;
            return record;
        } // method
    } // class

    public partial class NetherWart // 115 typeof=NetherWart
    {
        [StateRange(0, 3)] public int Age { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:nether_wart";
            record.Id = 115;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class NetherWartBlock // 214 typeof=NetherWartBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:nether_wart_block";
            record.Id = 214;
            return record;
        } // method
    } // class

    public partial class NetheriteBlock // 525 typeof=NetheriteBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:netherite_block";
            record.Id = 525;
            return record;
        } // method
    } // class

    public partial class Netherrack // 87 typeof=Netherrack
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:netherrack";
            record.Id = 87;
            return record;
        } // method
    } // class

    public partial class Netherreactor // 247 typeof=Netherreactor
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:netherreactor";
            record.Id = 247;
            return record;
        } // method
    } // class

    public partial class NormalStoneStairs // 435 typeof=NormalStoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:normal_stone_stairs";
            record.Id = 435;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Noteblock // 25 typeof=Noteblock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:noteblock";
            record.Id = 25;
            return record;
        } // method
    } // class

    public partial class OakStairs // 53 typeof=OakStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:oak_stairs";
            record.Id = 53;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Observer // 251 typeof=Observer
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;
        [StateBit] public bool PoweredBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:observer";
            record.Id = 251;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "powered_bit", Value = Convert.ToByte(PoweredBit)});
            return record;
        } // method
    } // class

    public partial class Obsidian // 49 typeof=Obsidian
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:obsidian";
            record.Id = 49;
            return record;
        } // method
    } // class

    public partial class OrangeGlazedTerracotta // 221 typeof=OrangeGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:orange_glazed_terracotta";
            record.Id = 221;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class PackedIce // 174 typeof=PackedIce
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:packed_ice";
            record.Id = 174;
            return record;
        } // method
    } // class

    public partial class PinkGlazedTerracotta // 226 typeof=PinkGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:pink_glazed_terracotta";
            record.Id = 226;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Piston // 33 typeof=Piston
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:piston";
            record.Id = 33;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class PistonArmCollision // 34 typeof=PistonArmCollision
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:pistonArmCollision";
            record.Id = 34;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Planks // 5 typeof=Planks
    {
        [StateEnum("birch","dark_oak","spruce","oak","acacia","jungle")]
        public string WoodType { get; set; } = "birch";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:planks";
            record.Id = 5;
            record.States.Add(new BlockStateString {Name = "wood_type", Value = WoodType});
            return record;
        } // method
    } // class

    public partial class Podzol // 243 typeof=Podzol
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:podzol";
            record.Id = 243;
            return record;
        } // method
    } // class

    public partial class PolishedAndesiteStairs // 429 typeof=PolishedAndesiteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_andesite_stairs";
            record.Id = 429;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class PolishedBasalt // 490 typeof=PolishedBasalt
    {
        [StateEnum("x","y","z")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_basalt";
            record.Id = 490;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstone // 546 typeof=PolishedBlackstone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:polished_blackstone";
            record.Id = 546;
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneBrickDoubleSlab // 540 typeof=PolishedBlackstoneBrickDoubleSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_brick_double_slab";
            record.Id = 540;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneBrickSlab // 539 typeof=PolishedBlackstoneBrickSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_brick_slab";
            record.Id = 539;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneBrickStairs // 530 typeof=PolishedBlackstoneBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_brick_stairs";
            record.Id = 530;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneBrickWall // 533 typeof=PolishedBlackstoneBrickWall
    {
        [StateEnum("short","tall","none")]
        public string WallConnectionTypeEast { get; set; } = "short";
        [StateEnum("tall","short","none")]
        public string WallConnectionTypeNorth { get; set; } = "tall";
        [StateEnum("tall","short","none")]
        public string WallConnectionTypeSouth { get; set; } = "tall";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeWest { get; set; } = "tall";
        [StateBit] public bool WallPostBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_brick_wall";
            record.Id = 533;
            record.States.Add(new BlockStateString {Name = "wall_connection_type_east", Value = WallConnectionTypeEast});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_north", Value = WallConnectionTypeNorth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_south", Value = WallConnectionTypeSouth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_west", Value = WallConnectionTypeWest});
            record.States.Add(new BlockStateByte {Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit)});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneBricks // 529 typeof=PolishedBlackstoneBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:polished_blackstone_bricks";
            record.Id = 529;
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneButton // 551 typeof=PolishedBlackstoneButton
    {
        [StateBit] public bool ButtonPressedBit { get; set; } = true;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_button";
            record.Id = 551;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneDoubleSlab // 549 typeof=PolishedBlackstoneDoubleSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_double_slab";
            record.Id = 549;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstonePressurePlate // 550 typeof=PolishedBlackstonePressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 9;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_pressure_plate";
            record.Id = 550;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneSlab // 548 typeof=PolishedBlackstoneSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_slab";
            record.Id = 548;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneStairs // 547 typeof=PolishedBlackstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_stairs";
            record.Id = 547;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class PolishedBlackstoneWall // 552 typeof=PolishedBlackstoneWall
    {
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeEast { get; set; } = "tall";
        [StateEnum("tall","short","none")]
        public string WallConnectionTypeNorth { get; set; } = "tall";
        [StateEnum("tall","none","short")]
        public string WallConnectionTypeSouth { get; set; } = "tall";
        [StateEnum("none","tall","short")]
        public string WallConnectionTypeWest { get; set; } = "none";
        [StateBit] public bool WallPostBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_blackstone_wall";
            record.Id = 552;
            record.States.Add(new BlockStateString {Name = "wall_connection_type_east", Value = WallConnectionTypeEast});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_north", Value = WallConnectionTypeNorth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_south", Value = WallConnectionTypeSouth});
            record.States.Add(new BlockStateString {Name = "wall_connection_type_west", Value = WallConnectionTypeWest});
            record.States.Add(new BlockStateByte {Name = "wall_post_bit", Value = Convert.ToByte(WallPostBit)});
            return record;
        } // method
    } // class

    public partial class PolishedDioriteStairs // 428 typeof=PolishedDioriteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_diorite_stairs";
            record.Id = 428;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class PolishedGraniteStairs // 427 typeof=PolishedGraniteStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:polished_granite_stairs";
            record.Id = 427;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Portal // 90 typeof=Portal
    {
        [StateEnum("unknown","x","z")]
        public string PortalAxis { get; set; } = "unknown";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:portal";
            record.Id = 90;
            record.States.Add(new BlockStateString {Name = "portal_axis", Value = PortalAxis});
            return record;
        } // method
    } // class

    public partial class Potatoes // 142 typeof=Potatoes
    {
        [StateRange(0, 7)] public override int Growth { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:potatoes";
            record.Id = 142;
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class PoweredComparator // 150 typeof=PoweredComparator
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 3;
        [StateBit] public bool OutputLitBit { get; set; } = true;
        [StateBit] public bool OutputSubtractBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:powered_comparator";
            record.Id = 150;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit)});
            record.States.Add(new BlockStateByte {Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit)});
            return record;
        } // method
    } // class

    public partial class PoweredRepeater // 94 typeof=PoweredRepeater
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateRange(0, 3)] public int RepeaterDelay { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:powered_repeater";
            record.Id = 94;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateInt {Name = "repeater_delay", Value = RepeaterDelay});
            return record;
        } // method
    } // class

    public partial class Prismarine // 168 typeof=Prismarine
    {
        [StateEnum("dark","default","bricks")]
        public string PrismarineBlockType { get; set; } = "dark";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:prismarine";
            record.Id = 168;
            record.States.Add(new BlockStateString {Name = "prismarine_block_type", Value = PrismarineBlockType});
            return record;
        } // method
    } // class

    public partial class PrismarineBricksStairs // 259 typeof=PrismarineBricksStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:prismarine_bricks_stairs";
            record.Id = 259;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class PrismarineStairs // 257 typeof=PrismarineStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:prismarine_stairs";
            record.Id = 257;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Pumpkin // 86 typeof=Pumpkin
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:pumpkin";
            record.Id = 86;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            return record;
        } // method
    } // class

    public partial class PumpkinStem // 104 typeof=PumpkinStem
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;
        [StateRange(0, 7)] public int Growth { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:pumpkin_stem";
            record.Id = 104;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class PurpleGlazedTerracotta // 219 typeof=PurpleGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:purple_glazed_terracotta";
            record.Id = 219;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class PurpurBlock // 201 typeof=PurpurBlock
    {
        [StateEnum("default","chiseled","smooth","lines")]
        public string ChiselType { get; set; } = "default";
        [StateEnum("z","x","y")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:purpur_block";
            record.Id = 201;
            record.States.Add(new BlockStateString {Name = "chisel_type", Value = ChiselType});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class PurpurStairs // 203 typeof=PurpurStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:purpur_stairs";
            record.Id = 203;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class QuartzBlock // 155 typeof=QuartzBlock
    {
        [StateEnum("default","smooth","lines","chiseled")]
        public string ChiselType { get; set; } = "default";
        [StateEnum("x","y","z")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:quartz_block";
            record.Id = 155;
            record.States.Add(new BlockStateString {Name = "chisel_type", Value = ChiselType});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class QuartzBricks // 559 typeof=QuartzBricks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:quartz_bricks";
            record.Id = 559;
            return record;
        } // method
    } // class

    public partial class QuartzOre // 153 typeof=QuartzOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:quartz_ore";
            record.Id = 153;
            return record;
        } // method
    } // class

    public partial class QuartzStairs // 156 typeof=QuartzStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:quartz_stairs";
            record.Id = 156;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Rail // 66 typeof=Rail
    {
        [StateRange(0, 9)] public int RailDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:rail";
            record.Id = 66;
            record.States.Add(new BlockStateInt {Name = "rail_direction", Value = RailDirection});
            return record;
        } // method
    } // class

    public partial class RedFlower // 38 typeof=RedFlower
    {
        [StateEnum("cornflower","houstonia","lily_of_the_valley","allium","tulip_pink","tulip_red","poppy","tulip_white","tulip_orange","oxeye","orchid")]
        public string FlowerType { get; set; } = "cornflower";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_flower";
            record.Id = 38;
            record.States.Add(new BlockStateString {Name = "flower_type", Value = FlowerType});
            return record;
        } // method
    } // class

    public partial class RedGlazedTerracotta // 234 typeof=RedGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_glazed_terracotta";
            record.Id = 234;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class RedMushroom // 40 typeof=RedMushroom
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:red_mushroom";
            record.Id = 40;
            return record;
        } // method
    } // class

    public partial class RedMushroomBlock // 100 typeof=RedMushroomBlock
    {
        [StateRange(0, 15)] public int HugeMushroomBits { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_mushroom_block";
            record.Id = 100;
            record.States.Add(new BlockStateInt {Name = "huge_mushroom_bits", Value = HugeMushroomBits});
            return record;
        } // method
    } // class

    public partial class RedNetherBrick // 215 typeof=RedNetherBrick
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:red_nether_brick";
            record.Id = 215;
            return record;
        } // method
    } // class

    public partial class RedNetherBrickStairs // 439 typeof=RedNetherBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_nether_brick_stairs";
            record.Id = 439;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class RedSandstone // 179 typeof=RedSandstone
    {
        [StateEnum("heiroglyphs","default","cut","smooth")]
        public string SandStoneType { get; set; } = "heiroglyphs";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_sandstone";
            record.Id = 179;
            record.States.Add(new BlockStateString {Name = "sand_stone_type", Value = SandStoneType});
            return record;
        } // method
    } // class

    public partial class RedSandstoneStairs // 180 typeof=RedSandstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:red_sandstone_stairs";
            record.Id = 180;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class RedstoneBlock // 152 typeof=RedstoneBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:redstone_block";
            record.Id = 152;
            return record;
        } // method
    } // class

    public partial class RedstoneLamp // 123 typeof=RedstoneLamp
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:redstone_lamp";
            record.Id = 123;
            return record;
        } // method
    } // class

    public partial class RedstoneOre // 73 typeof=RedstoneOre
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:redstone_ore";
            record.Id = 73;
            return record;
        } // method
    } // class

    public partial class RedstoneTorch // 76 typeof=RedstoneTorch
    {
        [StateEnum("north","east","south","unknown","top","west")]
        public override string TorchFacingDirection { get; set; } = "north";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:redstone_torch";
            record.Id = 76;
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class RedstoneWire // 55 typeof=RedstoneWire
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 7;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:redstone_wire";
            record.Id = 55;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class Reeds // 83 typeof=Reeds
    {
        [StateRange(0, 15)] public int Age { get; set; } = 14;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:reeds";
            record.Id = 83;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class RepeatingCommandBlock // 188 typeof=RepeatingCommandBlock
    {
        [StateBit] public bool ConditionalBit { get; set; } = true;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:repeating_command_block";
            record.Id = 188;
            record.States.Add(new BlockStateByte {Name = "conditional_bit", Value = Convert.ToByte(ConditionalBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Reserved6 // 255 typeof=Reserved6
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:reserved6";
            record.Id = 255;
            return record;
        } // method
    } // class

    public partial class RespawnAnchor // 527 typeof=RespawnAnchor
    {
        [StateRange(0, 4)] public int RespawnAnchorCharge { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:respawn_anchor";
            record.Id = 527;
            record.States.Add(new BlockStateInt {Name = "respawn_anchor_charge", Value = RespawnAnchorCharge});
            return record;
        } // method
    } // class

    public partial class Sand // 12 typeof=Sand
    {
        [StateEnum("normal","red")]
        public string SandType { get; set; } = "normal";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sand";
            record.Id = 12;
            record.States.Add(new BlockStateString {Name = "sand_type", Value = SandType});
            return record;
        } // method
    } // class

    public partial class Sandstone // 24 typeof=Sandstone
    {
        [StateEnum("smooth","default","cut","heiroglyphs")]
        public string SandStoneType { get; set; } = "smooth";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sandstone";
            record.Id = 24;
            record.States.Add(new BlockStateString {Name = "sand_stone_type", Value = SandStoneType});
            return record;
        } // method
    } // class

    public partial class SandstoneStairs // 128 typeof=SandstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sandstone_stairs";
            record.Id = 128;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Sapling // 6 typeof=Sapling
    {
        [StateBit] public bool AgeBit { get; set; } = false;
        [StateEnum("acacia","jungle","birch","oak","spruce","dark_oak")]
        public string SaplingType { get; set; } = "acacia";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sapling";
            record.Id = 6;
            record.States.Add(new BlockStateByte {Name = "age_bit", Value = Convert.ToByte(AgeBit)});
            record.States.Add(new BlockStateString {Name = "sapling_type", Value = SaplingType});
            return record;
        } // method
    } // class

    public partial class Scaffolding // 420 typeof=Scaffolding
    {
        [StateRange(0, 7)] public int Stability { get; set; } = 4;
        [StateBit] public bool StabilityCheck { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:scaffolding";
            record.Id = 420;
            record.States.Add(new BlockStateInt {Name = "stability", Value = Stability});
            record.States.Add(new BlockStateByte {Name = "stability_check", Value = Convert.ToByte(StabilityCheck)});
            return record;
        } // method
    } // class

    public partial class SeaPickle // 411 typeof=SeaPickle
    {
        [StateRange(0, 3)] public int ClusterCount { get; set; } = 0;
        [StateBit] public bool DeadBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sea_pickle";
            record.Id = 411;
            record.States.Add(new BlockStateInt {Name = "cluster_count", Value = ClusterCount});
            record.States.Add(new BlockStateByte {Name = "dead_bit", Value = Convert.ToByte(DeadBit)});
            return record;
        } // method
    } // class

    public partial class Seagrass // 385 typeof=Seagrass
    {
        [StateEnum("default","double_top","double_bot")]
        public string SeaGrassType { get; set; } = "default";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:seagrass";
            record.Id = 385;
            record.States.Add(new BlockStateString {Name = "sea_grass_type", Value = SeaGrassType});
            return record;
        } // method
    } // class

    public partial class SeaLantern // 169 typeof=SeaLantern
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:seaLantern";
            record.Id = 169;
            return record;
        } // method
    } // class

    public partial class Shroomlight // 485 typeof=Shroomlight
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:shroomlight";
            record.Id = 485;
            return record;
        } // method
    } // class

    public partial class ShulkerBox // 218 typeof=ShulkerBox
    {
        [StateEnum("pink","purple","brown","lime","light_blue","cyan","gray","white","blue","orange","black","magenta","green","red","silver","yellow")]
        public string Color { get; set; } = "pink";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:shulker_box";
            record.Id = 218;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class SilverGlazedTerracotta // 228 typeof=SilverGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:silver_glazed_terracotta";
            record.Id = 228;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Skull // 144 typeof=Skull
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 5;
        [StateBit] public bool NoDropBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                    case BlockStateInt s when s.Name == "facing_direction":
                        FacingDirection = s.Value;
                        break;
                    case BlockStateByte s when s.Name == "no_drop_bit":
                        NoDropBit = Convert.ToBoolean(s.Value);
                        break;
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:skull";
            record.Id = 144;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            record.States.Add(new BlockStateByte {Name = "no_drop_bit", Value = Convert.ToByte(NoDropBit)});
            return record;
        } // method
    } // class

    public partial class Slime // 165 typeof=Slime
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:slime";
            record.Id = 165;
            return record;
        } // method
    } // class

    public partial class SmithingTable // 457 typeof=SmithingTable
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:smithing_table";
            record.Id = 457;
            return record;
        } // method
    } // class

    public partial class Smoker // 453 typeof=Smoker
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:smoker";
            record.Id = 453;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class SmoothQuartzStairs // 440 typeof=SmoothQuartzStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:smooth_quartz_stairs";
            record.Id = 440;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class SmoothRedSandstoneStairs // 431 typeof=SmoothRedSandstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:smooth_red_sandstone_stairs";
            record.Id = 431;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class SmoothSandstoneStairs // 432 typeof=SmoothSandstoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:smooth_sandstone_stairs";
            record.Id = 432;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class SmoothStone // 438 typeof=SmoothStone
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:smooth_stone";
            record.Id = 438;
            return record;
        } // method
    } // class

    public partial class Snow // 80 typeof=Snow
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:snow";
            record.Id = 80;
            return record;
        } // method
    } // class

    public partial class SnowLayer // 78 typeof=SnowLayer
    {
        [StateBit] public bool CoveredBit { get; set; } = true;
        [StateRange(0, 7)] public int Height { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:snow_layer";
            record.Id = 78;
            record.States.Add(new BlockStateByte {Name = "covered_bit", Value = Convert.ToByte(CoveredBit)});
            record.States.Add(new BlockStateInt {Name = "height", Value = Height});
            return record;
        } // method
    } // class

    public partial class SoulCampfire // 545 typeof=SoulCampfire
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool Extinguished { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:soul_campfire";
            record.Id = 545;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "extinguished", Value = Convert.ToByte(Extinguished)});
            return record;
        } // method
    } // class

    public partial class SoulFire // 492 typeof=SoulFire
    {
        [StateRange(0, 15)] public int Age { get; set; } = 8;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:soul_fire";
            record.Id = 492;
            record.States.Add(new BlockStateInt {Name = "age", Value = Age});
            return record;
        } // method
    } // class

    public partial class SoulLantern // 524 typeof=SoulLantern
    {
        [StateBit] public bool Hanging { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:soul_lantern";
            record.Id = 524;
            record.States.Add(new BlockStateByte {Name = "hanging", Value = Convert.ToByte(Hanging)});
            return record;
        } // method
    } // class

    public partial class SoulSand // 88 typeof=SoulSand
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:soul_sand";
            record.Id = 88;
            return record;
        } // method
    } // class

    public partial class SoulSoil // 491 typeof=SoulSoil
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:soul_soil";
            record.Id = 491;
            return record;
        } // method
    } // class

    public partial class SoulTorch // 523 typeof=SoulTorch
    {
        [StateEnum("east","top","unknown","west","north","south")]
        public string TorchFacingDirection { get; set; } = "east";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:soul_torch";
            record.Id = 523;
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class Sponge // 19 typeof=Sponge
    {
        [StateEnum("wet","dry")]
        public string SpongeType { get; set; } = "wet";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sponge";
            record.Id = 19;
            record.States.Add(new BlockStateString {Name = "sponge_type", Value = SpongeType});
            return record;
        } // method
    } // class

    public partial class SpruceButton // 399 typeof=SpruceButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_button";
            record.Id = 399;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class SpruceDoor // 193 typeof=SpruceDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 1;
        [StateBit] public override bool DoorHingeBit { get; set; } = true;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpperBlockBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_door";
            record.Id = 193;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class SpruceFenceGate // 183 typeof=SpruceFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 1;
        [StateBit] public bool InWallBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_fence_gate";
            record.Id = 183;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class SprucePressurePlate // 409 typeof=SprucePressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 14;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_pressure_plate";
            record.Id = 409;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class SpruceStairs // 134 typeof=SpruceStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_stairs";
            record.Id = 134;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class SpruceStandingSign // 436 typeof=SpruceStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_standing_sign";
            record.Id = 436;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class SpruceTrapdoor // 404 typeof=SpruceTrapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 1;
        [StateBit] public override bool OpenBit { get; set; } = true;
        [StateBit] public override bool UpsideDownBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_trapdoor";
            record.Id = 404;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class SpruceWallSign // 437 typeof=SpruceWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:spruce_wall_sign";
            record.Id = 437;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class StainedGlass // 241 typeof=StainedGlass
    {
        [StateEnum("cyan","gray","light_blue","magenta","pink","silver","green","black","red","blue","yellow","brown","orange","lime","white","purple")]
        public string Color { get; set; } = "cyan";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stained_glass";
            record.Id = 241;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class StainedGlassPane // 160 typeof=StainedGlassPane
    {
        [StateEnum("lime","purple","magenta","yellow","cyan","gray","green","pink","light_blue","brown","red","orange","black","blue","white","silver")]
        public string Color { get; set; } = "lime";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stained_glass_pane";
            record.Id = 160;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class StainedHardenedClay // 159 typeof=StainedHardenedClay
    {
        [StateEnum("magenta","orange","red","black","yellow","gray","light_blue","cyan","brown","green","pink","silver","purple","lime","white","blue")]
        public string Color { get; set; } = "magenta";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stained_hardened_clay";
            record.Id = 159;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class StandingBanner // 176 typeof=StandingBanner
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:standing_banner";
            record.Id = 176;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class StandingSign // 63 typeof=StandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 8;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:standing_sign";
            record.Id = 63;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class StickyPiston // 29 typeof=StickyPiston
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sticky_piston";
            record.Id = 29;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class StickyPistonArmCollision // 472 typeof=StickyPistonArmCollision
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stickyPistonArmCollision";
            record.Id = 472;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Stone // 1 typeof=Stone
    {
        [StateEnum("stone","granite_smooth","andesite_smooth","diorite","granite","andesite","diorite_smooth")]
        public string StoneType { get; set; } = "stone";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone";
            record.Id = 1;
            record.States.Add(new BlockStateString {Name = "stone_type", Value = StoneType});
            return record;
        } // method
    } // class

    public partial class StoneBrickStairs // 109 typeof=StoneBrickStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_brick_stairs";
            record.Id = 109;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class StoneButton // 77 typeof=StoneButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_button";
            record.Id = 77;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class StonePressurePlate // 70 typeof=StonePressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_pressure_plate";
            record.Id = 70;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class StoneSlab // 44 typeof=StoneSlab
    {
        [StateEnum("nether_brick","smooth_stone","sandstone","quartz","stone_brick","brick","wood","cobblestone")]
        public string StoneSlabType { get; set; } = "nether_brick";
        [StateBit] public override bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_slab";
            record.Id = 44;
            record.States.Add(new BlockStateString {Name = "stone_slab_type", Value = StoneSlabType});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class StoneSlab2 // 182 typeof=StoneSlab2
    {
        [StateEnum("prismarine_brick","prismarine_dark","red_sandstone","red_nether_brick","smooth_sandstone","mossy_cobblestone","prismarine_rough","purpur")]
        public string StoneSlabType2 { get; set; } = "prismarine_brick";
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_slab2";
            record.Id = 182;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_2", Value = StoneSlabType2});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class StoneSlab3 // 417 typeof=StoneSlab3
    {
        [StateEnum("andesite","smooth_red_sandstone","polished_granite","end_stone_brick","polished_diorite","polished_andesite","diorite","granite")]
        public string StoneSlabType3 { get; set; } = "andesite";
        [StateBit] public override bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_slab3";
            record.Id = 417;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_3", Value = StoneSlabType3});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class StoneSlab4 // 421 typeof=StoneSlab4
    {
        [StateEnum("cut_red_sandstone","cut_sandstone","mossy_stone_brick","smooth_quartz","stone")]
        public string StoneSlabType4 { get; set; } = "cut_red_sandstone";
        [StateBit] public override bool TopSlotBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_slab4";
            record.Id = 421;
            record.States.Add(new BlockStateString {Name = "stone_slab_type_4", Value = StoneSlabType4});
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class StoneStairs // 67 typeof=StoneStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = true;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stone_stairs";
            record.Id = 67;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class Stonebrick // 98 typeof=Stonebrick
    {
        [StateEnum("smooth","cracked","mossy","default","chiseled")]
        public string StoneBrickType { get; set; } = "smooth";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stonebrick";
            record.Id = 98;
            record.States.Add(new BlockStateString {Name = "stone_brick_type", Value = StoneBrickType});
            return record;
        } // method
    } // class

    public partial class Stonecutter // 245 typeof=Stonecutter
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:stonecutter";
            record.Id = 245;
            return record;
        } // method
    } // class

    public partial class StonecutterBlock // 452 typeof=StonecutterBlock
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stonecutter_block";
            record.Id = 452;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class StrippedAcaciaLog // 263 typeof=StrippedAcaciaLog
    {
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_acacia_log";
            record.Id = 263;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedBirchLog // 261 typeof=StrippedBirchLog
    {
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_birch_log";
            record.Id = 261;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedCrimsonHyphae // 555 typeof=StrippedCrimsonHyphae
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 2;
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_crimson_hyphae";
            record.Id = 555;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedCrimsonStem // 495 typeof=StrippedCrimsonStem
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 1;
        [StateEnum("y","z","x")]
        public string PillarAxis { get; set; } = "y";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_crimson_stem";
            record.Id = 495;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedDarkOakLog // 264 typeof=StrippedDarkOakLog
    {
        [StateEnum("x","z","y")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_dark_oak_log";
            record.Id = 264;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedJungleLog // 262 typeof=StrippedJungleLog
    {
        [StateEnum("x","y","z")]
        public string PillarAxis { get; set; } = "x";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_jungle_log";
            record.Id = 262;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedOakLog // 265 typeof=StrippedOakLog
    {
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_oak_log";
            record.Id = 265;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedSpruceLog // 260 typeof=StrippedSpruceLog
    {
        [StateEnum("z","x","y")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_spruce_log";
            record.Id = 260;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedWarpedHyphae // 556 typeof=StrippedWarpedHyphae
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 0;
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_warped_hyphae";
            record.Id = 556;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StrippedWarpedStem // 496 typeof=StrippedWarpedStem
    {
        [StateRange(0, 3)] public int Deprecated { get; set; } = 3;
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:stripped_warped_stem";
            record.Id = 496;
            record.States.Add(new BlockStateInt {Name = "deprecated", Value = Deprecated});
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class StructureBlock // 252 typeof=StructureBlock
    {
        [StateEnum("corner","data","save","export","invalid","load")]
        public string StructureBlockType { get; set; } = "corner";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:structure_block";
            record.Id = 252;
            record.States.Add(new BlockStateString {Name = "structure_block_type", Value = StructureBlockType});
            return record;
        } // method
    } // class

    public partial class StructureVoid // 217 typeof=StructureVoid
    {
        [StateEnum("air","void")]
        public string StructureVoidType { get; set; } = "air";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:structure_void";
            record.Id = 217;
            record.States.Add(new BlockStateString {Name = "structure_void_type", Value = StructureVoidType});
            return record;
        } // method
    } // class

    public partial class SweetBerryBush // 462 typeof=SweetBerryBush
    {
        [StateRange(0, 7)] public int Growth { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:sweet_berry_bush";
            record.Id = 462;
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class Tallgrass // 31 typeof=Tallgrass
    {
        [StateEnum("fern","snow","tall","default")]
        public string TallGrassType { get; set; } = "fern";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:tallgrass";
            record.Id = 31;
            record.States.Add(new BlockStateString {Name = "tall_grass_type", Value = TallGrassType});
            return record;
        } // method
    } // class

    public partial class Target // 494 typeof=Target
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:target";
            record.Id = 494;
            return record;
        } // method
    } // class

    public partial class Tnt // 46 typeof=Tnt
    {
        [StateBit] public bool AllowUnderwaterBit { get; set; } = true;
        [StateBit] public bool ExplodeBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:tnt";
            record.Id = 46;
            record.States.Add(new BlockStateByte {Name = "allow_underwater_bit", Value = Convert.ToByte(AllowUnderwaterBit)});
            record.States.Add(new BlockStateByte {Name = "explode_bit", Value = Convert.ToByte(ExplodeBit)});
            return record;
        } // method
    } // class

    public partial class Torch // 50 typeof=Torch
    {
        [StateEnum("west","south","unknown","east","north","top")]
        public string TorchFacingDirection { get; set; } = "west";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:torch";
            record.Id = 50;
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class Trapdoor // 96 typeof=Trapdoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 2;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpsideDownBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:trapdoor";
            record.Id = 96;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class TrappedChest // 146 typeof=TrappedChest
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:trapped_chest";
            record.Id = 146;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class TripWire // 132 typeof=TripWire
    {
        [StateBit] public bool AttachedBit { get; set; } = true;
        [StateBit] public bool DisarmedBit { get; set; } = true;
        [StateBit] public bool PoweredBit { get; set; } = true;
        [StateBit] public bool SuspendedBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:tripWire";
            record.Id = 132;
            record.States.Add(new BlockStateByte {Name = "attached_bit", Value = Convert.ToByte(AttachedBit)});
            record.States.Add(new BlockStateByte {Name = "disarmed_bit", Value = Convert.ToByte(DisarmedBit)});
            record.States.Add(new BlockStateByte {Name = "powered_bit", Value = Convert.ToByte(PoweredBit)});
            record.States.Add(new BlockStateByte {Name = "suspended_bit", Value = Convert.ToByte(SuspendedBit)});
            return record;
        } // method
    } // class

    public partial class TripwireHook // 131 typeof=TripwireHook
    {
        [StateBit] public bool AttachedBit { get; set; } = false;
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool PoweredBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:tripwire_hook";
            record.Id = 131;
            record.States.Add(new BlockStateByte {Name = "attached_bit", Value = Convert.ToByte(AttachedBit)});
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "powered_bit", Value = Convert.ToByte(PoweredBit)});
            return record;
        } // method
    } // class

    public partial class TurtleEgg // 414 typeof=TurtleEgg
    {
        [StateEnum("no_cracks","cracked","max_cracked")]
        public string CrackedState { get; set; } = "no_cracks";
        [StateEnum("one_egg","four_egg","three_egg","two_egg")]
        public string TurtleEggCount { get; set; } = "one_egg";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:turtle_egg";
            record.Id = 414;
            record.States.Add(new BlockStateString {Name = "cracked_state", Value = CrackedState});
            record.States.Add(new BlockStateString {Name = "turtle_egg_count", Value = TurtleEggCount});
            return record;
        } // method
    } // class

    public partial class TwistingVines // 542 typeof=TwistingVines
    {
        [StateRange(0, 25)] public int TwistingVinesAge { get; set; } = 19;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:twisting_vines";
            record.Id = 542;
            record.States.Add(new BlockStateInt {Name = "twisting_vines_age", Value = TwistingVinesAge});
            return record;
        } // method
    } // class

    public partial class UnderwaterTorch // 239 typeof=UnderwaterTorch
    {
        [StateEnum("unknown","east","top","west","north","south")]
        public string TorchFacingDirection { get; set; } = "unknown";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:underwater_torch";
            record.Id = 239;
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class UndyedShulkerBox // 205 typeof=UndyedShulkerBox
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:undyed_shulker_box";
            record.Id = 205;
            return record;
        } // method
    } // class

    public partial class UnlitRedstoneTorch // 75 typeof=UnlitRedstoneTorch
    {
        [StateEnum("west","unknown","east","north","top","south")]
        public override string TorchFacingDirection { get; set; } = "west";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:unlit_redstone_torch";
            record.Id = 75;
            record.States.Add(new BlockStateString {Name = "torch_facing_direction", Value = TorchFacingDirection});
            return record;
        } // method
    } // class

    public partial class UnpoweredComparator // 149 typeof=UnpoweredComparator
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool OutputLitBit { get; set; } = false;
        [StateBit] public bool OutputSubtractBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:unpowered_comparator";
            record.Id = 149;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "output_lit_bit", Value = Convert.ToByte(OutputLitBit)});
            record.States.Add(new BlockStateByte {Name = "output_subtract_bit", Value = Convert.ToByte(OutputSubtractBit)});
            return record;
        } // method
    } // class

    public partial class UnpoweredRepeater // 93 typeof=UnpoweredRepeater
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 2;
        [StateRange(0, 3)] public int RepeaterDelay { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:unpowered_repeater";
            record.Id = 93;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateInt {Name = "repeater_delay", Value = RepeaterDelay});
            return record;
        } // method
    } // class

    public partial class WallBanner // 177 typeof=WallBanner
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wall_banner";
            record.Id = 177;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class WallSign // 68 typeof=WallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wall_sign";
            record.Id = 68;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class WarpedButton // 516 typeof=WarpedButton
    {
        [StateBit] public bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_button";
            record.Id = 516;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class WarpedDoor // 500 typeof=WarpedDoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 3;
        [StateBit] public bool DoorHingeBit { get; set; } = false;
        [StateBit] public bool OpenBit { get; set; } = true;
        [StateBit] public bool UpperBlockBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_door";
            record.Id = 500;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class WarpedDoubleSlab // 522 typeof=WarpedDoubleSlab
    {
        [StateBit] public bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_double_slab";
            record.Id = 522;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class WarpedFence // 512 typeof=WarpedFence
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_fence";
            record.Id = 512;
            return record;
        } // method
    } // class

    public partial class WarpedFenceGate // 514 typeof=WarpedFenceGate
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool InWallBit { get; set; } = true;
        [StateBit] public bool OpenBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_fence_gate";
            record.Id = 514;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "in_wall_bit", Value = Convert.ToByte(InWallBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            return record;
        } // method
    } // class

    public partial class WarpedFungus // 484 typeof=WarpedFungus
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_fungus";
            record.Id = 484;
            return record;
        } // method
    } // class

    public partial class WarpedHyphae // 553 typeof=WarpedHyphae
    {
        [StateEnum("y","x","z")]
        public string PillarAxis { get; set; } = "y";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_hyphae";
            record.Id = 553;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class WarpedNylium // 488 typeof=WarpedNylium
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_nylium";
            record.Id = 488;
            return record;
        } // method
    } // class

    public partial class WarpedPlanks // 498 typeof=WarpedPlanks
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_planks";
            record.Id = 498;
            return record;
        } // method
    } // class

    public partial class WarpedPressurePlate // 518 typeof=WarpedPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 4;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_pressure_plate";
            record.Id = 518;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class WarpedRoots // 479 typeof=WarpedRoots
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_roots";
            record.Id = 479;
            return record;
        } // method
    } // class

    public partial class WarpedSlab // 520 typeof=WarpedSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_slab";
            record.Id = 520;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            return record;
        } // method
    } // class

    public partial class WarpedStairs // 510 typeof=WarpedStairs
    {
        [StateBit] public override bool UpsideDownBit { get; set; } = false;
        [StateRange(0, 3)] public override int WeirdoDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_stairs";
            record.Id = 510;
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            record.States.Add(new BlockStateInt {Name = "weirdo_direction", Value = WeirdoDirection});
            return record;
        } // method
    } // class

    public partial class WarpedStandingSign // 506 typeof=WarpedStandingSign
    {
        [StateRange(0, 15)] public int GroundSignDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_standing_sign";
            record.Id = 506;
            record.States.Add(new BlockStateInt {Name = "ground_sign_direction", Value = GroundSignDirection});
            return record;
        } // method
    } // class

    public partial class WarpedStem // 481 typeof=WarpedStem
    {
        [StateEnum("z","x","y")]
        public string PillarAxis { get; set; } = "z";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_stem";
            record.Id = 481;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            return record;
        } // method
    } // class

    public partial class WarpedTrapdoor // 502 typeof=WarpedTrapdoor
    {
        [StateRange(0, 3)] public int Direction { get; set; } = 0;
        [StateBit] public bool OpenBit { get; set; } = true;
        [StateBit] public bool UpsideDownBit { get; set; } = true;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_trapdoor";
            record.Id = 502;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upside_down_bit", Value = Convert.ToByte(UpsideDownBit)});
            return record;
        } // method
    } // class

    public partial class WarpedWallSign // 508 typeof=WarpedWallSign
    {
        [StateRange(0, 5)] public int FacingDirection { get; set; } = 1;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:warped_wall_sign";
            record.Id = 508;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class WarpedWartBlock // 482 typeof=WarpedWartBlock
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:warped_wart_block";
            record.Id = 482;
            return record;
        } // method
    } // class

    public partial class Water // 9 typeof=Water
    {
        [StateRange(0, 15)] public override int LiquidDepth { get; set; } = 12;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:water";
            record.Id = 9;
            record.States.Add(new BlockStateInt {Name = "liquid_depth", Value = LiquidDepth});
            return record;
        } // method
    } // class

    public partial class Waterlily // 111 typeof=Waterlily
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:waterlily";
            record.Id = 111;
            return record;
        } // method
    } // class

    public partial class Web // 30 typeof=Web
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:web";
            record.Id = 30;
            return record;
        } // method
    } // class

    public partial class WeepingVines // 486 typeof=WeepingVines
    {
        [StateRange(0, 25)] public int WeepingVinesAge { get; set; } = 24;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:weeping_vines";
            record.Id = 486;
            record.States.Add(new BlockStateInt {Name = "weeping_vines_age", Value = WeepingVinesAge});
            return record;
        } // method
    } // class

    public partial class Wheat // 59 typeof=Wheat
    {
        [StateRange(0, 7)] public override int Growth { get; set; } = 6;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wheat";
            record.Id = 59;
            record.States.Add(new BlockStateInt {Name = "growth", Value = Growth});
            return record;
        } // method
    } // class

    public partial class WhiteGlazedTerracotta // 220 typeof=WhiteGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 2;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:white_glazed_terracotta";
            record.Id = 220;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class Vine // 106 typeof=Vine
    {
        [StateRange(0, 15)] public int VineDirectionBits { get; set; } = 5;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:vine";
            record.Id = 106;
            record.States.Add(new BlockStateInt {Name = "vine_direction_bits", Value = VineDirectionBits});
            return record;
        } // method
    } // class

    public partial class WitherRose // 471 typeof=WitherRose
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:wither_rose";
            record.Id = 471;
            return record;
        } // method
    } // class

    public partial class Wood // 467 typeof=Wood
    {
        [StateEnum("z","y","x")]
        public string PillarAxis { get; set; } = "z";
        [StateBit] public bool StrippedBit { get; set; } = false;
        [StateEnum("birch","jungle","oak","acacia","dark_oak","spruce")]
        public string WoodType { get; set; } = "birch";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wood";
            record.Id = 467;
            record.States.Add(new BlockStateString {Name = "pillar_axis", Value = PillarAxis});
            record.States.Add(new BlockStateByte {Name = "stripped_bit", Value = Convert.ToByte(StrippedBit)});
            record.States.Add(new BlockStateString {Name = "wood_type", Value = WoodType});
            return record;
        } // method
    } // class

    public partial class WoodenButton // 143 typeof=WoodenButton
    {
        [StateBit] public override bool ButtonPressedBit { get; set; } = false;
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 3;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wooden_button";
            record.Id = 143;
            record.States.Add(new BlockStateByte {Name = "button_pressed_bit", Value = Convert.ToByte(ButtonPressedBit)});
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class

    public partial class WoodenDoor // 64 typeof=WoodenDoor
    {
        [StateRange(0, 3)] public override int Direction { get; set; } = 2;
        [StateBit] public override bool DoorHingeBit { get; set; } = true;
        [StateBit] public override bool OpenBit { get; set; } = false;
        [StateBit] public override bool UpperBlockBit { get; set; } = false;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wooden_door";
            record.Id = 64;
            record.States.Add(new BlockStateInt {Name = "direction", Value = Direction});
            record.States.Add(new BlockStateByte {Name = "door_hinge_bit", Value = Convert.ToByte(DoorHingeBit)});
            record.States.Add(new BlockStateByte {Name = "open_bit", Value = Convert.ToByte(OpenBit)});
            record.States.Add(new BlockStateByte {Name = "upper_block_bit", Value = Convert.ToByte(UpperBlockBit)});
            return record;
        } // method
    } // class

    public partial class WoodenPressurePlate // 72 typeof=WoodenPressurePlate
    {
        [StateRange(0, 15)] public int RedstoneSignal { get; set; } = 8;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wooden_pressure_plate";
            record.Id = 72;
            record.States.Add(new BlockStateInt {Name = "redstone_signal", Value = RedstoneSignal});
            return record;
        } // method
    } // class

    public partial class WoodenSlab // 158 typeof=WoodenSlab
    {
        [StateBit] public override bool TopSlotBit { get; set; } = false;
        [StateEnum("dark_oak","jungle","acacia","birch","spruce","oak")]
        public string WoodType { get; set; } = "dark_oak";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wooden_slab";
            record.Id = 158;
            record.States.Add(new BlockStateByte {Name = "top_slot_bit", Value = Convert.ToByte(TopSlotBit)});
            record.States.Add(new BlockStateString {Name = "wood_type", Value = WoodType});
            return record;
        } // method
    } // class

    public partial class Wool // 35 typeof=Wool
    {
        [StateEnum("purple","green","yellow","pink","brown","silver","light_blue","orange","cyan","red","gray","lime","blue","black","magenta","white")]
        public string Color { get; set; } = "purple";

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:wool";
            record.Id = 35;
            record.States.Add(new BlockStateString {Name = "color", Value = Color});
            return record;
        } // method
    } // class

    public partial class YellowFlower // 37 typeof=YellowFlower
    {

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
                {
                } // switch
            } // foreach
        } // method

        public override BlockStateContainer GetState()
        {
            var record = new BlockStateContainer();
            record.Name = "minecraft:yellow_flower";
            record.Id = 37;
            return record;
        } // method
    } // class

    public partial class YellowGlazedTerracotta // 224 typeof=YellowGlazedTerracotta
    {
        [StateRange(0, 5)] public override int FacingDirection { get; set; } = 0;

        public override void SetState(List<IBlockState> states)
        {
            foreach (var state in states)
            {
                switch(state)
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
            record.Name = "minecraft:yellow_glazed_terracotta";
            record.Id = 224;
            record.States.Add(new BlockStateInt {Name = "facing_direction", Value = FacingDirection});
            return record;
        } // method
    } // class
   
	public partial class Element0 : Block { public Element0() : base(36) { IsGenerated = true; } }
    public partial class CommandBlock : Block { public CommandBlock() : base(137) { IsGenerated = true; } }
    public partial class RepeatingCommandBlock : Block { public RepeatingCommandBlock() : base(188) { IsGenerated = true; } }
    public partial class ChainCommandBlock : Block { public ChainCommandBlock() : base(189) { IsGenerated = true; } }
    public partial class HardGlassPane : Block { public HardGlassPane() : base(190) { IsGenerated = true; } }
    public partial class HardStainedGlassPane : Block { public HardStainedGlassPane() : base(191) { IsGenerated = true; } }
    public partial class ChemicalHeat : Block { public ChemicalHeat() : base(192) { IsGenerated = true; } }
    public partial class ColoredTorchRg : Block { public ColoredTorchRg() : base(202) { IsGenerated = true; } }
    public partial class ColoredTorchBp : Block { public ColoredTorchBp() : base(204) { IsGenerated = true; } }
    public partial class Allow : Block { public Allow() : base(210) { IsGenerated = true; } }
    public partial class Deny : Block { public Deny() : base(211) { IsGenerated = true; } }
    public partial class BorderBlock : Block { public BorderBlock() : base(212) { IsGenerated = true; } }
    public partial class Magma : Block { public Magma() : base(213) { IsGenerated = true; } }
    public partial class NetherWartBlock : Block { public NetherWartBlock() : base(214) { IsGenerated = true; } }
    public partial class RedNetherBrick : Block { public RedNetherBrick() : base(215) { IsGenerated = true; } }
    public partial class BoneBlock : Block { public BoneBlock() : base(216) { IsGenerated = true; } }
    public partial class StructureVoid : Block { public StructureVoid() : base(217) { IsGenerated = true; } }
    public partial class ChemistryTable : Block { public ChemistryTable() : base(238) { IsGenerated = true; } }
    public partial class UnderwaterTorch : Block { public UnderwaterTorch() : base(239) { IsGenerated = true; } }
    public partial class Camera : Block { public Camera() : base(242) { IsGenerated = true; } }
    public partial class InfoUpdate : Block { public InfoUpdate() : base(248) { IsGenerated = true; } }
    public partial class InfoUpdate2 : Block { public InfoUpdate2() : base(249) { IsGenerated = true; } }
    public partial class MovingBlock : Block { public MovingBlock() : base(250) { IsGenerated = true; } }
    public partial class HardGlass : Block { public HardGlass() : base(253) { IsGenerated = true; } }
    public partial class HardStainedGlass : Block { public HardStainedGlass() : base(254) { IsGenerated = true; } }
    public partial class Reserved6 : Block { public Reserved6() : base(255) { IsGenerated = true; } }
    public partial class StrippedSpruceLog : Block { public StrippedSpruceLog() : base(260) { IsGenerated = true; } }
    public partial class StrippedBirchLog : Block { public StrippedBirchLog() : base(261) { IsGenerated = true; } }
    public partial class StrippedJungleLog : Block { public StrippedJungleLog() : base(262) { IsGenerated = true; } }
    public partial class StrippedAcaciaLog : Block { public StrippedAcaciaLog() : base(263) { IsGenerated = true; } }
    public partial class StrippedDarkOakLog : Block { public StrippedDarkOakLog() : base(264) { IsGenerated = true; } }
    public partial class StrippedOakLog : Block { public StrippedOakLog() : base(265) { IsGenerated = true; } }
    public partial class BlueIce : Block { public BlueIce() : base(266) { IsGenerated = true; } }
    public partial class Element1 : Block { public Element1() : base(267) { IsGenerated = true; } }
    public partial class Element2 : Block { public Element2() : base(268) { IsGenerated = true; } }
    public partial class Element3 : Block { public Element3() : base(269) { IsGenerated = true; } }
    public partial class Element4 : Block { public Element4() : base(270) { IsGenerated = true; } }
    public partial class Element5 : Block { public Element5() : base(271) { IsGenerated = true; } }
    public partial class Element6 : Block { public Element6() : base(272) { IsGenerated = true; } }
    public partial class Element7 : Block { public Element7() : base(273) { IsGenerated = true; } }
    public partial class Element8 : Block { public Element8() : base(274) { IsGenerated = true; } }
    public partial class Element9 : Block { public Element9() : base(275) { IsGenerated = true; } }
    public partial class Element10 : Block { public Element10() : base(276) { IsGenerated = true; } }
    public partial class Element11 : Block { public Element11() : base(277) { IsGenerated = true; } }
    public partial class Element12 : Block { public Element12() : base(278) { IsGenerated = true; } }
    public partial class Element13 : Block { public Element13() : base(279) { IsGenerated = true; } }
    public partial class Element14 : Block { public Element14() : base(280) { IsGenerated = true; } }
    public partial class Element15 : Block { public Element15() : base(281) { IsGenerated = true; } }
    public partial class Element16 : Block { public Element16() : base(282) { IsGenerated = true; } }
    public partial class Element17 : Block { public Element17() : base(283) { IsGenerated = true; } }
    public partial class Element18 : Block { public Element18() : base(284) { IsGenerated = true; } }
    public partial class Element19 : Block { public Element19() : base(285) { IsGenerated = true; } }
    public partial class Element20 : Block { public Element20() : base(286) { IsGenerated = true; } }
    public partial class Element21 : Block { public Element21() : base(287) { IsGenerated = true; } }
    public partial class Element22 : Block { public Element22() : base(288) { IsGenerated = true; } }
    public partial class Element23 : Block { public Element23() : base(289) { IsGenerated = true; } }
    public partial class Element24 : Block { public Element24() : base(290) { IsGenerated = true; } }
    public partial class Element25 : Block { public Element25() : base(291) { IsGenerated = true; } }
    public partial class Element26 : Block { public Element26() : base(292) { IsGenerated = true; } }
    public partial class Element27 : Block { public Element27() : base(293) { IsGenerated = true; } }
    public partial class Element28 : Block { public Element28() : base(294) { IsGenerated = true; } }
    public partial class Element29 : Block { public Element29() : base(295) { IsGenerated = true; } }
    public partial class Element30 : Block { public Element30() : base(296) { IsGenerated = true; } }
    public partial class Element31 : Block { public Element31() : base(297) { IsGenerated = true; } }
    public partial class Element32 : Block { public Element32() : base(298) { IsGenerated = true; } }
    public partial class Element33 : Block { public Element33() : base(299) { IsGenerated = true; } }
    public partial class Element34 : Block { public Element34() : base(300) { IsGenerated = true; } }
    public partial class Element35 : Block { public Element35() : base(301) { IsGenerated = true; } }
    public partial class Element36 : Block { public Element36() : base(302) { IsGenerated = true; } }
    public partial class Element37 : Block { public Element37() : base(303) { IsGenerated = true; } }
    public partial class Element38 : Block { public Element38() : base(304) { IsGenerated = true; } }
    public partial class Element39 : Block { public Element39() : base(305) { IsGenerated = true; } }
    public partial class Element40 : Block { public Element40() : base(306) { IsGenerated = true; } }
    public partial class Element41 : Block { public Element41() : base(307) { IsGenerated = true; } }
    public partial class Element42 : Block { public Element42() : base(308) { IsGenerated = true; } }
    public partial class Element43 : Block { public Element43() : base(309) { IsGenerated = true; } }
    public partial class Element44 : Block { public Element44() : base(310) { IsGenerated = true; } }
    public partial class Element45 : Block { public Element45() : base(311) { IsGenerated = true; } }
    public partial class Element46 : Block { public Element46() : base(312) { IsGenerated = true; } }
    public partial class Element47 : Block { public Element47() : base(313) { IsGenerated = true; } }
    public partial class Element48 : Block { public Element48() : base(314) { IsGenerated = true; } }
    public partial class Element49 : Block { public Element49() : base(315) { IsGenerated = true; } }
    public partial class Element50 : Block { public Element50() : base(316) { IsGenerated = true; } }
    public partial class Element51 : Block { public Element51() : base(317) { IsGenerated = true; } }
    public partial class Element52 : Block { public Element52() : base(318) { IsGenerated = true; } }
    public partial class Element53 : Block { public Element53() : base(319) { IsGenerated = true; } }
    public partial class Element54 : Block { public Element54() : base(320) { IsGenerated = true; } }
    public partial class Element55 : Block { public Element55() : base(321) { IsGenerated = true; } }
    public partial class Element56 : Block { public Element56() : base(322) { IsGenerated = true; } }
    public partial class Element57 : Block { public Element57() : base(323) { IsGenerated = true; } }
    public partial class Element58 : Block { public Element58() : base(324) { IsGenerated = true; } }
    public partial class Element59 : Block { public Element59() : base(325) { IsGenerated = true; } }
    public partial class Element60 : Block { public Element60() : base(326) { IsGenerated = true; } }
    public partial class Element61 : Block { public Element61() : base(327) { IsGenerated = true; } }
    public partial class Element62 : Block { public Element62() : base(328) { IsGenerated = true; } }
    public partial class Element63 : Block { public Element63() : base(329) { IsGenerated = true; } }
    public partial class Element64 : Block { public Element64() : base(330) { IsGenerated = true; } }
    public partial class Element65 : Block { public Element65() : base(331) { IsGenerated = true; } }
    public partial class Element66 : Block { public Element66() : base(332) { IsGenerated = true; } }
    public partial class Element67 : Block { public Element67() : base(333) { IsGenerated = true; } }
    public partial class Element68 : Block { public Element68() : base(334) { IsGenerated = true; } }
    public partial class Element69 : Block { public Element69() : base(335) { IsGenerated = true; } }
    public partial class Element70 : Block { public Element70() : base(336) { IsGenerated = true; } }
    public partial class Element71 : Block { public Element71() : base(337) { IsGenerated = true; } }
    public partial class Element72 : Block { public Element72() : base(338) { IsGenerated = true; } }
    public partial class Element73 : Block { public Element73() : base(339) { IsGenerated = true; } }
    public partial class Element74 : Block { public Element74() : base(340) { IsGenerated = true; } }
    public partial class Element75 : Block { public Element75() : base(341) { IsGenerated = true; } }
    public partial class Element76 : Block { public Element76() : base(342) { IsGenerated = true; } }
    public partial class Element77 : Block { public Element77() : base(343) { IsGenerated = true; } }
    public partial class Element78 : Block { public Element78() : base(344) { IsGenerated = true; } }
    public partial class Element79 : Block { public Element79() : base(345) { IsGenerated = true; } }
    public partial class Element80 : Block { public Element80() : base(346) { IsGenerated = true; } }
    public partial class Element81 : Block { public Element81() : base(347) { IsGenerated = true; } }
    public partial class Element82 : Block { public Element82() : base(348) { IsGenerated = true; } }
    public partial class Element83 : Block { public Element83() : base(349) { IsGenerated = true; } }
    public partial class Element84 : Block { public Element84() : base(350) { IsGenerated = true; } }
    public partial class Element85 : Block { public Element85() : base(351) { IsGenerated = true; } }
    public partial class Element86 : Block { public Element86() : base(352) { IsGenerated = true; } }
    public partial class Element87 : Block { public Element87() : base(353) { IsGenerated = true; } }
    public partial class Element88 : Block { public Element88() : base(354) { IsGenerated = true; } }
    public partial class Element89 : Block { public Element89() : base(355) { IsGenerated = true; } }
    public partial class Element90 : Block { public Element90() : base(356) { IsGenerated = true; } }
    public partial class Element91 : Block { public Element91() : base(357) { IsGenerated = true; } }
    public partial class Element92 : Block { public Element92() : base(358) { IsGenerated = true; } }
    public partial class Element93 : Block { public Element93() : base(359) { IsGenerated = true; } }
    public partial class Element94 : Block { public Element94() : base(360) { IsGenerated = true; } }
    public partial class Element95 : Block { public Element95() : base(361) { IsGenerated = true; } }
    public partial class Element96 : Block { public Element96() : base(362) { IsGenerated = true; } }
    public partial class Element97 : Block { public Element97() : base(363) { IsGenerated = true; } }
    public partial class Element98 : Block { public Element98() : base(364) { IsGenerated = true; } }
    public partial class Element99 : Block { public Element99() : base(365) { IsGenerated = true; } }
    public partial class Element100 : Block { public Element100() : base(366) { IsGenerated = true; } }
    public partial class Element101 : Block { public Element101() : base(367) { IsGenerated = true; } }
    public partial class Element102 : Block { public Element102() : base(368) { IsGenerated = true; } }
    public partial class Element103 : Block { public Element103() : base(369) { IsGenerated = true; } }
    public partial class Element104 : Block { public Element104() : base(370) { IsGenerated = true; } }
    public partial class Element105 : Block { public Element105() : base(371) { IsGenerated = true; } }
    public partial class Element106 : Block { public Element106() : base(372) { IsGenerated = true; } }
    public partial class Element107 : Block { public Element107() : base(373) { IsGenerated = true; } }
    public partial class Element108 : Block { public Element108() : base(374) { IsGenerated = true; } }
    public partial class Element109 : Block { public Element109() : base(375) { IsGenerated = true; } }
    public partial class Element110 : Block { public Element110() : base(376) { IsGenerated = true; } }
    public partial class Element111 : Block { public Element111() : base(377) { IsGenerated = true; } }
    public partial class Element112 : Block { public Element112() : base(378) { IsGenerated = true; } }
    public partial class Element113 : Block { public Element113() : base(379) { IsGenerated = true; } }
    public partial class Element114 : Block { public Element114() : base(380) { IsGenerated = true; } }
    public partial class Element115 : Block { public Element115() : base(381) { IsGenerated = true; } }
    public partial class Element116 : Block { public Element116() : base(382) { IsGenerated = true; } }
    public partial class Element117 : Block { public Element117() : base(383) { IsGenerated = true; } }
    public partial class Element118 : Block { public Element118() : base(384) { IsGenerated = true; } }
    public partial class Seagrass : Block { public Seagrass() : base(385) { IsGenerated = true; } }
    public partial class Coral : Block { public Coral() : base(386) { IsGenerated = true; } }
    public partial class CoralBlock : Block { public CoralBlock() : base(387) { IsGenerated = true; } }
    public partial class CoralFan : Block { public CoralFan() : base(388) { IsGenerated = true; } }
    public partial class CoralFanDead : Block { public CoralFanDead() : base(389) { IsGenerated = true; } }
    public partial class CoralFanHang : Block { public CoralFanHang() : base(390) { IsGenerated = true; } }
    public partial class CoralFanHang2 : Block { public CoralFanHang2() : base(391) { IsGenerated = true; } }
    public partial class CoralFanHang3 : Block { public CoralFanHang3() : base(392) { IsGenerated = true; } }
    public partial class Kelp : Block { public Kelp() : base(393) { IsGenerated = true; } }
    public partial class DriedKelpBlock : Block { public DriedKelpBlock() : base(394) { IsGenerated = true; } }
    public partial class AcaciaPressurePlate : Block { public AcaciaPressurePlate() : base(405) { IsGenerated = true; } }
    public partial class BirchPressurePlate : Block { public BirchPressurePlate() : base(406) { IsGenerated = true; } }
    public partial class DarkOakPressurePlate : Block { public DarkOakPressurePlate() : base(407) { IsGenerated = true; } }
    public partial class JunglePressurePlate : Block { public JunglePressurePlate() : base(408) { IsGenerated = true; } }
    public partial class SprucePressurePlate : Block { public SprucePressurePlate() : base(409) { IsGenerated = true; } }
    public partial class CarvedPumpkin : Block { public CarvedPumpkin() : base(410) { IsGenerated = true; } }
    public partial class SeaPickle : Block { public SeaPickle() : base(411) { IsGenerated = true; } }
    public partial class Conduit : Block { public Conduit() : base(412) { IsGenerated = true; } }
    public partial class TurtleEgg : Block { public TurtleEgg() : base(414) { IsGenerated = true; } }
    public partial class BubbleColumn : Block { public BubbleColumn() : base(415) { IsGenerated = true; } }
    public partial class Barrier : Block { public Barrier() : base(416) { IsGenerated = true; } }
    public partial class Bamboo : Block { public Bamboo() : base(418) { IsGenerated = true; } }
    public partial class BambooSapling : Block { public BambooSapling() : base(419) { IsGenerated = true; } }
    public partial class Scaffolding : Block { public Scaffolding() : base(420) { IsGenerated = true; } }
    public partial class SpruceStandingSign : Block { public SpruceStandingSign() : base(436) { IsGenerated = true; } }
    public partial class SpruceWallSign : Block { public SpruceWallSign() : base(437) { IsGenerated = true; } }
    public partial class SmoothStone : Block { public SmoothStone() : base(438) { IsGenerated = true; } }
    public partial class BirchStandingSign : Block { public BirchStandingSign() : base(441) { IsGenerated = true; } }
    public partial class BirchWallSign : Block { public BirchWallSign() : base(442) { IsGenerated = true; } }
    public partial class JungleStandingSign : Block { public JungleStandingSign() : base(443) { IsGenerated = true; } }
    public partial class JungleWallSign : Block { public JungleWallSign() : base(444) { IsGenerated = true; } }
    public partial class AcaciaStandingSign : Block { public AcaciaStandingSign() : base(445) { IsGenerated = true; } }
    public partial class AcaciaWallSign : Block { public AcaciaWallSign() : base(446) { IsGenerated = true; } }
    public partial class DarkoakStandingSign : Block { public DarkoakStandingSign() : base(447) { IsGenerated = true; } }
    public partial class DarkoakWallSign : Block { public DarkoakWallSign() : base(448) { IsGenerated = true; } }
    public partial class Lectern : Block { public Lectern() : base(449) { IsGenerated = true; } }
    public partial class Grindstone : Block { public Grindstone() : base(450) { IsGenerated = true; } }
    public partial class StonecutterBlock : Block { public StonecutterBlock() : base(452) { IsGenerated = true; } }
    public partial class Smoker : Block { public Smoker() : base(453) { IsGenerated = true; } }
    public partial class LitSmoker : Block { public LitSmoker() : base(454) { IsGenerated = true; } }
    public partial class CartographyTable : Block { public CartographyTable() : base(455) { IsGenerated = true; } }
    public partial class FletchingTable : Block { public FletchingTable() : base(456) { IsGenerated = true; } }
    public partial class SmithingTable : Block { public SmithingTable() : base(457) { IsGenerated = true; } }
    public partial class Barrel : Block { public Barrel() : base(458) { IsGenerated = true; } }
    public partial class Bell : Block { public Bell() : base(461) { IsGenerated = true; } }
    public partial class SweetBerryBush : Block { public SweetBerryBush() : base(462) { IsGenerated = true; } }
    public partial class Lantern : Block { public Lantern() : base(463) { IsGenerated = true; } }
    public partial class Campfire : Block { public Campfire() : base(464) { IsGenerated = true; } }
    public partial class LavaCauldron : Block { public LavaCauldron() : base(465) { IsGenerated = true; } }
    public partial class Jigsaw : Block { public Jigsaw() : base(466) { IsGenerated = true; } }
    public partial class Wood : Block { public Wood() : base(467) { IsGenerated = true; } }
    public partial class Composter : Block { public Composter() : base(468) { IsGenerated = true; } }
    public partial class LightBlock : Block { public LightBlock() : base(470) { IsGenerated = true; } }
    public partial class WitherRose : Block { public WitherRose() : base(471) { IsGenerated = true; } }
    public partial class StickyPistonArmCollision : Block { public StickyPistonArmCollision() : base(472) { IsGenerated = true; } }
    public partial class BeeNest : Block { public BeeNest() : base(473) { IsGenerated = true; } }
    public partial class Beehive : Block { public Beehive() : base(474) { IsGenerated = true; } }
    public partial class HoneyBlock : Block { public HoneyBlock() : base(475) { IsGenerated = true; } }
    public partial class HoneycombBlock : Block { public HoneycombBlock() : base(476) { IsGenerated = true; } }
    public partial class Lodestone : Block { public Lodestone() : base(477) { IsGenerated = true; } }
    public partial class CrimsonRoots : Block { public CrimsonRoots() : base(478) { IsGenerated = true; } }
    public partial class WarpedRoots : Block { public WarpedRoots() : base(479) { IsGenerated = true; } }
    public partial class CrimsonStem : Block { public CrimsonStem() : base(480) { IsGenerated = true; } }
    public partial class WarpedStem : Block { public WarpedStem() : base(481) { IsGenerated = true; } }
    public partial class WarpedWartBlock : Block { public WarpedWartBlock() : base(482) { IsGenerated = true; } }
    public partial class CrimsonFungus : Block { public CrimsonFungus() : base(483) { IsGenerated = true; } }
    public partial class WarpedFungus : Block { public WarpedFungus() : base(484) { IsGenerated = true; } }
    public partial class Shroomlight : Block { public Shroomlight() : base(485) { IsGenerated = true; } }
    public partial class WeepingVines : Block { public WeepingVines() : base(486) { IsGenerated = true; } }
    public partial class CrimsonNylium : Block { public CrimsonNylium() : base(487) { IsGenerated = true; } }
    public partial class WarpedNylium : Block { public WarpedNylium() : base(488) { IsGenerated = true; } }
    public partial class Basalt : Block { public Basalt() : base(489) { IsGenerated = true; } }
    public partial class PolishedBasalt : Block { public PolishedBasalt() : base(490) { IsGenerated = true; } }
    public partial class SoulSoil : Block { public SoulSoil() : base(491) { IsGenerated = true; } }
    public partial class SoulFire : Block { public SoulFire() : base(492) { IsGenerated = true; } }
    public partial class NetherSprouts : Block { public NetherSprouts() : base(493) { IsGenerated = true; } }
    public partial class Target : Block { public Target() : base(494) { IsGenerated = true; } }
    public partial class StrippedCrimsonStem : Block { public StrippedCrimsonStem() : base(495) { IsGenerated = true; } }
    public partial class StrippedWarpedStem : Block { public StrippedWarpedStem() : base(496) { IsGenerated = true; } }
    public partial class CrimsonPlanks : Block { public CrimsonPlanks() : base(497) { IsGenerated = true; } }
    public partial class WarpedPlanks : Block { public WarpedPlanks() : base(498) { IsGenerated = true; } }
    public partial class CrimsonDoor : Block { public CrimsonDoor() : base(499) { IsGenerated = true; } }
    public partial class WarpedDoor : Block { public WarpedDoor() : base(500) { IsGenerated = true; } }
    public partial class CrimsonTrapdoor : Block { public CrimsonTrapdoor() : base(501) { IsGenerated = true; } }
    public partial class WarpedTrapdoor : Block { public WarpedTrapdoor() : base(502) { IsGenerated = true; } }
    public partial class CrimsonStandingSign : Block { public CrimsonStandingSign() : base(505) { IsGenerated = true; } }
    public partial class WarpedStandingSign : Block { public WarpedStandingSign() : base(506) { IsGenerated = true; } }
    public partial class CrimsonWallSign : Block { public CrimsonWallSign() : base(507) { IsGenerated = true; } }
    public partial class WarpedWallSign : Block { public WarpedWallSign() : base(508) { IsGenerated = true; } }
    public partial class CrimsonFence : Block { public CrimsonFence() : base(511) { IsGenerated = true; } }
    public partial class WarpedFence : Block { public WarpedFence() : base(512) { IsGenerated = true; } }
    public partial class CrimsonFenceGate : Block { public CrimsonFenceGate() : base(513) { IsGenerated = true; } }
    public partial class WarpedFenceGate : Block { public WarpedFenceGate() : base(514) { IsGenerated = true; } }
    public partial class CrimsonButton : Block { public CrimsonButton() : base(515) { IsGenerated = true; } }
    public partial class WarpedButton : Block { public WarpedButton() : base(516) { IsGenerated = true; } }
    public partial class CrimsonPressurePlate : Block { public CrimsonPressurePlate() : base(517) { IsGenerated = true; } }
    public partial class WarpedPressurePlate : Block { public WarpedPressurePlate() : base(518) { IsGenerated = true; } }
    public partial class SoulTorch : Block { public SoulTorch() : base(523) { IsGenerated = true; } }
    public partial class SoulLantern : Block { public SoulLantern() : base(524) { IsGenerated = true; } }
    public partial class NetheriteBlock : Block { public NetheriteBlock() : base(525) { IsGenerated = true; } }
    public partial class AncientDebris : Block { public AncientDebris() : base(526) { IsGenerated = true; } }
    public partial class RespawnAnchor : Block { public RespawnAnchor() : base(527) { IsGenerated = true; } }
    public partial class Blackstone : Block { public Blackstone() : base(528) { IsGenerated = true; } }
    public partial class PolishedBlackstoneBricks : Block { public PolishedBlackstoneBricks() : base(529) { IsGenerated = true; } }
    public partial class BlackstoneWall : Block { public BlackstoneWall() : base(532) { IsGenerated = true; } }
    public partial class PolishedBlackstoneBrickWall : Block { public PolishedBlackstoneBrickWall() : base(533) { IsGenerated = true; } }
    public partial class ChiseledPolishedBlackstone : Block { public ChiseledPolishedBlackstone() : base(534) { IsGenerated = true; } }
    public partial class CrackedPolishedBlackstoneBricks : Block { public CrackedPolishedBlackstoneBricks() : base(535) { IsGenerated = true; } }
    public partial class GildedBlackstone : Block { public GildedBlackstone() : base(536) { IsGenerated = true; } }
    public partial class Chain : Block { public Chain() : base(541) { IsGenerated = true; } }
    public partial class TwistingVines : Block { public TwistingVines() : base(542) { IsGenerated = true; } }
    public partial class NetherGoldOre : Block { public NetherGoldOre() : base(543) { IsGenerated = true; } }
    public partial class CryingObsidian : Block { public CryingObsidian() : base(544) { IsGenerated = true; } }
    public partial class SoulCampfire : Block { public SoulCampfire() : base(545) { IsGenerated = true; } }
    public partial class PolishedBlackstone : Block { public PolishedBlackstone() : base(546) { IsGenerated = true; } }
    public partial class PolishedBlackstonePressurePlate : Block { public PolishedBlackstonePressurePlate() : base(550) { IsGenerated = true; } }
    public partial class PolishedBlackstoneButton : Block { public PolishedBlackstoneButton() : base(551) { IsGenerated = true; } }
    public partial class PolishedBlackstoneWall : Block { public PolishedBlackstoneWall() : base(552) { IsGenerated = true; } }
    public partial class WarpedHyphae : Block { public WarpedHyphae() : base(553) { IsGenerated = true; } }
    public partial class CrimsonHyphae : Block { public CrimsonHyphae() : base(554) { IsGenerated = true; } }
    public partial class StrippedCrimsonHyphae : Block { public StrippedCrimsonHyphae() : base(555) { IsGenerated = true; } }
    public partial class StrippedWarpedHyphae : Block { public StrippedWarpedHyphae() : base(556) { IsGenerated = true; } }
    public partial class ChiseledNetherBricks : Block { public ChiseledNetherBricks() : base(557) { IsGenerated = true; } }
    public partial class CrackedNetherBricks : Block { public CrackedNetherBricks() : base(558) { IsGenerated = true; } }
    public partial class QuartzBricks : Block { public QuartzBricks() : base(559) { IsGenerated = true; } }

}