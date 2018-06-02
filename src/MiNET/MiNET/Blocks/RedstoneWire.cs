using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
    public class RedstoneWire : Block
    {
        public RedstoneWire() : base(55)
        {
            IsTransparent = true;
            IsConductive = true;
        }

        public override Item[] GetDrops(Item tool)
        {
            return new[] { ItemFactory.GetItem(331) };
        }

        public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
        {
            level.BlockWithTicks.TryAdd(Coordinates, 0);
        }

        public override void DoPhysics(Level level)
        {
            level.BlockWithTicks.TryAdd(Coordinates, 0);
        }

        public override bool PlaceBlock(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
        {
            Metadata = 0;
            world.SetBlock(this, applyPhysics: false);
            world.BlockWithTicks.TryAdd(Coordinates, 0);
            return true;
        }

        //public override void BreakBlock(Level world, bool silent = false)
        //{
        //    base.BreakBlock(world, silent);
        //}

        public override void OnTick(Level level, bool isRandom)
        {
            if (isRandom) return;
            if (level.GetBlock(Coordinates).Id == Id)
                CalculatePower(level);
        }

        public void CalculatePower(Level level)
        {
            Block blockWithMaxPower = GetPoweredBlock(level, new BlockCoordinates[] { Coordinates }, PoweredPriorityMode.Redstone);

            if (blockWithMaxPower.Power == 0 || blockWithMaxPower.Coordinates == Coordinates)
            {
                //Console.WriteLine("update block");
                if (Power != 0)
                    OnPower(level, Coordinates, 0, PowerAction.UnPower);
            }
            else
            {
                if (blockWithMaxPower.Power > Power || (blockWithMaxPower.Power != Power && blockWithMaxPower is RedstoneWire))
                    OnPower(level, blockWithMaxPower.Coordinates, blockWithMaxPower.Power, PowerAction.Power);
            }
        }

        protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
        {
            return true;
        }

        public override void OnPower(Level level, BlockCoordinates sourceBlockCoordinates, byte power, PowerAction powerAction = PowerAction.Power)
        {
            List<BlockCoordinates> cachedBlocks = new List<BlockCoordinates>();
            if (powerAction == PowerAction.UnPower)
            {
                List<BlockCoordinates> ignoredBlocks = new List<BlockCoordinates>();
                List<BlockCoordinates> unPowerCachedBlocks = new List<BlockCoordinates>();
                unPowerCachedBlocks = RedstoneUnPower(level, ignoredBlocks, cachedBlocks);
                //Console.WriteLine("upcache " + unPowerCachedBlocks.Count);
                while (unPowerCachedBlocks.Count > 0)
                {
                    List<BlockCoordinates> tmpBlocks = new List<BlockCoordinates>();
                    foreach (var coords in unPowerCachedBlocks)
                    {
                        var block = level.GetBlock(coords);
                        if (block is RedstoneWire)
                            foreach (var toUnPowerBlock in ((RedstoneWire)block).RedstoneUnPower(level, ignoredBlocks, cachedBlocks))
                                if (!tmpBlocks.Contains(toUnPowerBlock))
                                    tmpBlocks.Add(toUnPowerBlock);
                    }
                    unPowerCachedBlocks = tmpBlocks;
                }
                if (cachedBlocks.Count == 0)
                {
                    UpdateBlockData(level);
                    return;
                }
            }

            if (cachedBlocks.Count == 0)
                cachedBlocks = RedstoneOnPower(level);
            
            while(cachedBlocks.Count > 0)
            {
                List<BlockCoordinates> tmpBlocks = new List<BlockCoordinates>();
                foreach (var coords in cachedBlocks)
                {
                    var block = level.GetBlock(coords);
                    if (block is RedstoneWire)
                        foreach (var toPowerBlock in ((RedstoneWire)block).RedstoneOnPower(level))
                            if (!tmpBlocks.Contains(toPowerBlock))
                                tmpBlocks.Add(toPowerBlock);
                }
                cachedBlocks = tmpBlocks;
            }
        }

        public List<BlockCoordinates> RedstoneUnPower(Level level, List<BlockCoordinates> ignoredBlocks, List<BlockCoordinates> cachedBlocks)
        {
            List<BlockCoordinates> toUnPowerBlocks = new List<BlockCoordinates>();
            //Console.WriteLine("unpower  " + Coordinates);
            if (ignoredBlocks.Contains(Coordinates)) return toUnPowerBlocks;
            Block blockWithMaxPower = GetPoweredBlock(level, ignoredBlocks.ToArray(), PoweredPriorityMode.Redstone);
            Block _blockWithMaxPower = blockWithMaxPower.GetPoweredBlock(level, new BlockCoordinates[] { Coordinates }, PoweredPriorityMode.Redstone);

            ignoredBlocks.Add(Coordinates);
            Power = 0;
            level.SetBlockPower(Coordinates, 0);
            if (!(blockWithMaxPower.Coordinates == Coordinates ||
                _blockWithMaxPower.Coordinates == Coordinates ||
                (blockWithMaxPower is RedstoneWire && blockWithMaxPower.Power <= Power)) && level.GetBlock(Coordinates).Id == Id)
            {
                cachedBlocks.Add(Coordinates);
                return toUnPowerBlocks;
            }
            //if (oldPower == Power) return;
            for (var y = -1; y <= 1; y++)
                for (var i = 0; i < 6; i++)
                {
                    if (y != 0 && i < 2) continue;
                    var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
                    coords.Y += y;
                    var block = level.GetBlock(coords);
                    if (ignoredBlocks.Contains(coords)) continue;
                    if (block.IsConductive && CanPower(level, block) && block.Power > 0)
                        toUnPowerBlocks.Add(coords);
                }
            return toUnPowerBlocks;
        }

        public List<BlockCoordinates> RedstoneOnPower(Level level)
        {
            List<BlockCoordinates> toPowerBlocks = new List<BlockCoordinates>();

            Block blockWithMaxPower = GetPoweredBlock(level, new BlockCoordinates[] { Coordinates }, PoweredPriorityMode.Redstone);
            //Console.WriteLine("power  " + Coordinates + "   " + blockWithMaxPower.Power);

            bool isRedstone = blockWithMaxPower is RedstoneWire;
            var oldPower = Power;

            var newSourcePower = blockWithMaxPower is RedstoneWire ? blockWithMaxPower.Power - 1 : blockWithMaxPower.Power;

            if(newSourcePower == 0)
            {
                UpdateBlockData(level);
                return toPowerBlocks;
            }
            if (Power >= newSourcePower) return toPowerBlocks;

            SetPower(level, blockWithMaxPower.Power, isRedstone);
            if (Power == 0 || Power == oldPower) return toPowerBlocks;

            for (var y = -1; y <= 1; y++)
                for (var i = 0; i < 6; i++)
                {
                    if (y != 0 && i < 2) continue;
                    var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
                    coords.Y += y;
                    if (coords == blockWithMaxPower.Coordinates) continue;
                    var block = level.GetBlock(coords);
                    if (block.IsConductive && CanPower(level, block))
                        toPowerBlocks.Add(coords);
                    //block.OnPower(level, Coordinates, Power, PowerAction.Power);
                }
            return toPowerBlocks;
        }

        //public override void OnPower(Level level, BlockCoordinates sourceBlockCoordinates, byte power, PowerAction powerAction = PowerAction.Power)
        //{
        //    {
        //        Console.WriteLine("power  " + power + "   " + powerAction);
        //        var sourceBlock = level.GetBlock(sourceBlockCoordinates);
        //        Block blockWithMaxPower = GetPoweredBlock(level, sourceBlockCoordinates, PoweredPriorityMode.Redstone);
        //        Block _blockWithMaxPower = blockWithMaxPower.GetPoweredBlock(level, Coordinates, PoweredPriorityMode.Redstone);

        //        if (sourceBlockCoordinates == Coordinates)
        //            sourceBlock = blockWithMaxPower;

        //        bool isRedstone = sourceBlock is RedstoneWire;
        //        var oldPower = Power;

        //        if (powerAction == PowerAction.UnPower)
        //        {
        //            if (blockWithMaxPower.Coordinates == Coordinates ||
        //                _blockWithMaxPower.Coordinates == Coordinates ||
        //                blockWithMaxPower is RedstoneWire && blockWithMaxPower.Power <= Power)
        //                SetPower(level, 0);
        //            else
        //                SetPower(level, blockWithMaxPower.Power, blockWithMaxPower is RedstoneWire);

        //            //if (oldPower == Power) return;

        //            var _powerAction = Power == 0 ? PowerAction.UnPower : PowerAction.Power;

        //            for (var i = 1; i < 6; i++)
        //            {
        //                var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
        //                var block = level.GetBlock(coords);
        //                if (_powerAction == PowerAction.UnPower && coords == sourceBlockCoordinates) continue;
        //                if (block.IsConductive && CanPower(block) && (block.Power > 0 || _powerAction == PowerAction.Power))
        //                    block.OnPower(level, Coordinates, Power, _powerAction);
        //            }
        //        }
        //        else
        //        {
        //            var newSourcePower = sourceBlock is RedstoneWire ? power - 1 : power;
        //            //var newMaxPower = blockWithMaxPower is RedstoneWire ? blockWithMaxPower.Power - 1 : blockWithMaxPower.Power;

        //            if (Power >= newSourcePower) return;

        //            SetPower(level, sourceBlock.Power, isRedstone);
        //            if (Power == 0 || Power == oldPower) return;

        //            for (var i = 1; i < 6; i++)
        //            {
        //                var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
        //                if (coords == sourceBlock.Coordinates) continue;
        //                var block = level.GetBlock(coords);
        //                if (block.IsConductive && CanPower(this))
        //                    block.OnPower(level, Coordinates, Power, PowerAction.Power);
        //            }
        //        }
        //    }
        //}
        
        public override bool CanPower(Level level, Block targetBlock)
        {
            var yoffset = targetBlock.Coordinates.Y - Coordinates.Y;
            var canPower = yoffset == 0 && (targetBlock is UnpoweredRepeater ||
                targetBlock is Dispenser || targetBlock is GoldenRail || targetBlock is Piston ||
                targetBlock is StickyPiston || targetBlock is Tnt || targetBlock is IronDoor || targetBlock is WoodenDoor ||
                targetBlock is Trapdoor || targetBlock is IronTrapdoor || targetBlock is RedstoneLamp ||
                targetBlock is Dropper || targetBlock is Hopper || targetBlock is ActivatorRail || targetBlock is Rail ||
                targetBlock is UnpoweredComparator);

            if (!canPower)
                if (targetBlock is RedstoneWire)
                {
                    if (yoffset == 0)
                        canPower = true;
                    else
                    {
                        Block up_block = yoffset == 1 ? level.GetBlock(Coordinates + new BlockCoordinates(0, 1, 0)) : level.GetBlock(targetBlock.Coordinates + new BlockCoordinates(0, 1, 0));
                        Block down_block = level.GetBlock(Coordinates + new BlockCoordinates(0, -1, 0));
                        //if (!_block.IsSolid || _block is StoneSlab || _block is WoodenSlab || _block is StoneSlab2 || _block is BlockStairs ||
                        //    _block is Farmland || _block is Fence || _block is CobblestoneWall || _block is FenceGate || _block is Leaves ||
                        //    _block is Leaves2 || _block is Glowstone || _block is Chest || _block is EnderChest || _block is )
                        if (!(up_block.IsSolid && up_block.IsConductive) && (yoffset != -1 || down_block.IsConductive))
                            canPower = true;
                    }
                }
                else
                {

                }
            return canPower;
        }

        public void UpdateBlockData(Level level)
        {
            //Console.WriteLine("upd  " + Metadata + "   " + Power);
            if (Metadata == Power) return;
            Metadata = Power;
            if (level.GetBlock(Coordinates).Id == Id)
                level.SetBlock(this, applyPhysics: false);
                for (var y = -1; y <= 1; y++)
                    for (var i = 0; i < 6; i++)
                    {
                        if (y != 0 && i < 2) continue;
                        var coords = GetNewCoordinatesFromFace(Coordinates, (BlockFace)i);
                        coords.Y += y;
                        var block = level.GetBlock(coords);
                        if (block is RedstoneWire)
                            ((RedstoneWire)block).UpdateBlockData(level);
                    }
        }

        public void SetPower(Level level, byte power, bool redstone = false)
        {
            var oldPower = Power;
            Power = power;
            if (redstone && Power > 0)
                Power--;
            if (Power == oldPower) return;
            Metadata = Power;
            //Console.WriteLine(Power + "   " + Metadata);
            if (level.GetBlock(Coordinates).Id == Id)
                level.SetBlock(this, applyPhysics: false);
        }
    }
}
