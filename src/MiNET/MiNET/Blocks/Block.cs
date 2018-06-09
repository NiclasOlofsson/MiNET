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
using System.Numerics;
using System.Linq;
using log4net;
using MiNET.Items;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	/// <summary>
	///     Blocks are the basic units of structure in Minecraft. Together, they build up the in-game environment and can be
	///     mined and utilized in various fashions.
	/// </summary>
	public class Block : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Block));

		public BlockCoordinates Coordinates { get; set; }
		public byte Id { get; }
		public byte Metadata { get; set; }

		public float Hardness { get; protected set; } = 0;
		public float BlastResistance { get; protected set; } = 0;
		public short FuelEfficiency { get; protected set; } = 0;
		public float FrictionFactor { get; protected set; } = 0.6f;
		public int LightLevel { get; set; } = 0;

        public byte Power { get; set; } = 0;
        public bool IsConductive { get; set; } = false;

		public bool IsReplacible { get; protected set; } = false;
		public bool IsSolid { get; protected set; } = true;
		public bool IsBuildable { get; protected set; } = true;
		public bool IsTransparent { get; protected set; } = false;
		public bool IsFlammable { get; protected set; } = false;
		public bool IsBlockingSkylight { get; protected set; } = true;

		public byte BlockLight { get; set; }
		public byte SkyLight { get; set; }

		public byte BiomeId { get; set; }

		public Block(byte id)
		{
			Id = id;
		}

		public uint GetRuntimeId() => BlockFactory.GetRuntimeId(Id, Metadata);

		public bool CanPlace(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return CanPlace(world, player, Coordinates, targetCoordinates, face);
		}

		protected virtual bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			var playerBbox = (player.GetBoundingBox() - 0.01f);
			var blockBbox = GetBoundingBox();
			if (playerBbox.Intersects(blockBbox))
			{
				Log.Debug($"Player bbox={playerBbox}, block bbox={blockBbox}, intersects={playerBbox.Intersects(blockBbox)}");
				Log.Debug($"Can't build where you are standing");
				return false;
			}

			return world.GetBlock(blockCoordinates).IsReplacible;
		}

		public virtual void BreakBlock(Level world, bool silent = false)
		{
			world.SetAir(Coordinates);
            if (Power > 0)
                OnPower(world, Coordinates, Power, PowerAction.UnPower);
            Console.WriteLine(Id + "  " + Metadata);

            if (!silent)
			{
				DestroyBlockParticle particle = new DestroyBlockParticle(world, this);
				particle.Spawn();
			}

			UpdateBlocks(world);
		}

		protected void UpdateBlocks(Level world)
		{
			world.GetBlock(Coordinates + BlockCoordinates.Up).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.Down).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.West).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.East).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.South).BlockUpdate(world, Coordinates);
			world.GetBlock(Coordinates + BlockCoordinates.North).BlockUpdate(world, Coordinates);
		}

		public virtual bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			// No default placement. Return unhandled.
			return false;
		}

		public virtual void BlockAdded(Level level)
		{
		}

		public virtual bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			// No default interaction. Return unhandled.
			return false;
		}

		public virtual void OnTick(Level level, bool isRandom)
		{
		}

		public virtual void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
		}

		public float GetHardness()
		{
			return Hardness/5.0F;
		}

		//public double GetMineTime(Item miningTool)
		//{
		//	int multiplier = (int) miningTool.ItemMaterial;
		//	return Hardness*(1.5*multiplier);
		//}

		protected BlockCoordinates GetNewCoordinatesFromFace(BlockCoordinates target, BlockFace face)
		{
			switch (face)
			{
				case BlockFace.Down:
					return target + Level.Down;
				case BlockFace.Up:
					return target + Level.Up;
				case BlockFace.East:
					return target + Level.East;
				case BlockFace.West:
					return target + Level.West;
				case BlockFace.North:
					return target + Level.North;
				case BlockFace.South:
					return target + Level.South;
				default:
					return target;
			}
		}

		public virtual Item[] GetDrops(Item tool)
		{
			return new Item[] {new ItemBlock(this, Metadata) {Count = 1}};
		}

		public virtual Item GetSmelt()
		{
			return null;
		}

		public virtual float GetExperiencePoints()
		{
			return 0;
		}

		public virtual void DoPhysics(Level level)
		{
		}

        public enum PowerAction
        {
            Power = 0,
            PowerBlock,
            UnPower
        }

        public virtual void OnPower(Level level, BlockCoordinates sourceBlockCoordinates, byte power, PowerAction powerAction = PowerAction.Power)
        {
            //if (powerAction == PowerAction.Power) return;

            //Block blockWithMaxPower = GetPoweredBlock(level, new BlockCoordinates[] { sourceBlockCoordinates });
            //Block _blockWithMaxPower = blockWithMaxPower.GetPoweredBlock(level, new BlockCoordinates[] { Coordinates });

            //if (powerAction == PowerAction.UnPower)
            //{
            //    if (Power == 0) return;
            //    var oldPower = Power;

            //    if (blockWithMaxPower is RedstoneWire || _blockWithMaxPower.Coordinates == Coordinates)
            //        Power = 0;
            //    else
            //        Power = blockWithMaxPower.Power;
                
            //    for (var i = 0; i < 6; i++)
            //    {
            //        var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
            //        if (coords == sourceBlockCoordinates || blockWithMaxPower.Coordinates == coords) continue;
            //        var block = level.GetBlock(coords);
            //        if (block.IsConductive && CanPower(level, block))
            //        {
            //            if (blockWithMaxPower.Coordinates == Coordinates)
            //            {
            //                if (block.Power > 0)
            //                    block.OnPower(level, Coordinates, oldPower, PowerAction.UnPower);
            //            }
            //            else if (blockWithMaxPower.Coordinates != coords && block.Power != Power)
            //                block.OnPower(level, Coordinates, Power, PowerAction.Power);
            //        }
            //    }
            //    return;
            //}

            //if (power <= Power) return;

            //Power = power;
            //level.SetBlockPower(Coordinates, power);

            //for (var i = 0; i < 6; i++)
            //{
            //    var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
            //    if (coords == sourceBlockCoordinates) continue;
            //    var block = level.GetBlock(coords);
            //    if (block.IsConductive && CanPower(level, block))
            //        block.OnPower(level, Coordinates, power, PowerAction.Power);
            //}
        }

        //public virtual void OnPower(Level level, BlockCoordinates sourceBlockCoordinates, byte power, PowerAction powerAction = PowerAction.Power)
        //{
        //    if (powerAction == PowerAction.Power) return;

        //    Block blockWithMaxPower = GetPoweredBlock(level, sourceBlockCoordinates);
        //    Block _blockWithMaxPower = blockWithMaxPower.GetPoweredBlock(level, Coordinates);

        //    if (powerAction == PowerAction.UnPower)
        //    {
        //        if (Power == 0) return;
        //        var oldPower = Power;

        //        if (blockWithMaxPower is RedstoneWire || _blockWithMaxPower.Coordinates == Coordinates)
        //            Power = 0;
        //        else
        //            Power = blockWithMaxPower.Power;
                

        //        for (var i = 0; i < 6; i++)
        //        {
        //            var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
        //            if (coords == sourceBlockCoordinates || blockWithMaxPower.Coordinates == coords) continue;
        //            var block = level.GetBlock(coords);
        //            if (block.IsConductive && CanPower(block))
        //            {
        //                if (blockWithMaxPower.Coordinates == Coordinates)
        //                {
        //                    if (block.Power > 0)
        //                        block.OnPower(level, Coordinates, oldPower, PowerAction.UnPower);
        //                }
        //                else if (blockWithMaxPower.Coordinates != coords && block.Power != Power)
        //                    block.OnPower(level, Coordinates, Power, PowerAction.Power);
        //            }
        //        }
        //        return;
        //    }

        //    if (power <= Power) return;

        //    Power = power;
        //    level.SetBlockPower(Coordinates, power);

        //    for (var i = 0; i < 6; i++)
        //    {
        //        var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
        //        if (coords == sourceBlockCoordinates) continue;
        //        var block = level.GetBlock(coords);
        //        if (block.IsConductive && CanPower(block))
        //            block.OnPower(level, Coordinates, power, PowerAction.Power);
        //    }
        //}

        public enum PoweredPriorityMode
        {
            All = 0,
            Blocks,
            Redstone
        }

        public Block GetPoweredBlock(Level level, BlockCoordinates[] sourceBlocksCoordinates, PoweredPriorityMode priority = PoweredPriorityMode.All)
        {
            Block blockWithMaxPower = new Air() { Coordinates = Coordinates };

            for (var y = -1; y <= 1; y++)
                for (var i = 0; i < 6; i++)
                {
                    if (y != 0 && i < 2) continue;
                    var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
                    coords.Y += y;
                    if (sourceBlocksCoordinates.Contains(coords)) continue;
                    var block = level.GetBlock(coords);
                    if (!block.CanPower(level, this) || !((block is RedstoneWire && this is RedstoneWire) || y == 0)) continue;
                    if (priority != PoweredPriorityMode.All && block.Power < Power) continue;
                    if (priority == PoweredPriorityMode.Redstone && block is RedstoneWire && block.Power == Power) continue;
                    if (block.Power - (this is RedstoneWire && block is RedstoneWire ? 1 : 0) > blockWithMaxPower.Power - (this is RedstoneWire && blockWithMaxPower is RedstoneWire ? 1 : 0))
                        blockWithMaxPower = block;
                }
            return blockWithMaxPower;
        }

        public virtual bool CanPower(Level level, Block targetBlock)
        {
            return true;
        }

		public virtual BoundingBox GetBoundingBox()
		{
			return new BoundingBox(Coordinates, Coordinates + 1);
		}


		public object Clone()
		{
			return MemberwiseClone();
		}

		public override string ToString()
		{
			return $"Id: {Id}, Metadata: {Metadata}, Coordinates: {Coordinates}";
		}
	}
}