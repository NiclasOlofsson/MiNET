using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using log4net;
using MiNET;
using MiNET.Plugins.Attributes;
using MiNET.Plugins;
using MiNET.Worlds;
using MiNET.Utils;
using MiNET.Blocks;
using System.Threading;

namespace CorePlugins
{
    [Plugin(PluginName = "CoreCommands", Description = "The core.", PluginVersion = "1.0", Author = "Artem Valko")]
    public class CoreCommands : Plugin
    {
        protected override void OnEnable()
        {
            foreach (var level in Context.LevelManager.Levels)
            {
                level.BlockBreak += LevelOnBlockBreak;
                level.BlockPlace += LevelOnBlockPlace;
            }

            _timerOnBlock = new Timer(TimerOnBlock, null, 30000, 5000);
        }


        #region Block Change
        static readonly object _lockOnBlockAdd = new object();
        private List<Block> _onBlockCoordinates = new List<Block>();
        private Timer _timerOnBlock;

        private void LevelOnBlockBreak(object sender, BlockBreakEventArgs e)
        {             
            lock (_lockOnBlockAdd)
            {
                //Console.WriteLine("break lock");
                Block temp = new Block(0) { Coordinates = e.Block.Coordinates };

                int i = _onBlockCoordinates.FindLastIndex((x) => x.Coordinates.X == temp.Coordinates.X && x.Coordinates.Z == temp.Coordinates.Z && x.Coordinates.Y == temp.Coordinates.Y);
                if (i == -1)
                    _onBlockCoordinates.Add(temp);
                else
                {
                    _onBlockCoordinates[i] = temp;
                }
                Console.WriteLine("break unlocked block = {0}; x={1};y={2};z={3}", temp.Id, temp.Coordinates.X, temp.Coordinates.Y, temp.Coordinates.Z);
            }
        }

        private void LevelOnBlockPlace(object sender, BlockPlaceEventArgs e)
        {
            lock (_lockOnBlockAdd)
            {
                Block temp = new Block((byte)e.Next.Id) { Coordinates = e.Next.GetNewCoordinatesFromFace(e.Block.Coordinates, e.Face) };
                //Console.WriteLine("place lock");
                int i = _onBlockCoordinates.FindLastIndex((x) => x.Coordinates.X == temp.Coordinates.X && x.Coordinates.Z == temp.Coordinates.Z && x.Coordinates.Y == temp.Coordinates.Y);
                if (i == -1)
                    _onBlockCoordinates.Add(temp);
                else
                {
                    _onBlockCoordinates[i] = temp;
                }

                Console.WriteLine("place unlocked block = {0}; x={1};y={2};z={3}", temp.Id, temp.Coordinates.X, temp.Coordinates.Y, temp.Coordinates.Z);
            }
        }

        private void TimerOnBlock(object state)
        {
            //_timerOnBlock.Change(Timeout.Infinite, Timeout.Infinite);
            List<Block> copy;
            lock (_lockOnBlockAdd)
            {
                copy = new List<Block>(_onBlockCoordinates);
                _onBlockCoordinates.Clear();
            }

            foreach (Block block in copy)
            {
                int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                var anvil = new AnvilWorldProvider("world");
                anvil.Initialize();
                ChunkCoordinates coordinates = new ChunkCoordinates(x >> 4, z >> 4);
                ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);
                chunk.SetBlock(x % 16, y, z % 16, block.Id);
                AnvilWorldProvider.SaveChunk(chunk, "world", 0);
            }

            //Console.WriteLine("Save... {0}", copy.Count);
        }


       /* private struct _tempItemOnBlock
        {
            public ChunkColumn column;
            public List<Block> list;
        }
        
        private void TimerOnBlock(object state)
        {
           // _timerOnBlock.Change(Timeout.Infinite, Timeout.Infinite);
            List<Block> copy;
            lock (_lockOnBlockAdd)
            {
                copy = new List<Block>(_onBlockCoordinates);
                _onBlockCoordinates.Clear();
            }
            //Console.WriteLine("save...");
            List<_tempItemOnBlock> dict = new List<_tempItemOnBlock>();



            foreach(Block block in copy)
            {
                int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                var anvil = new AnvilWorldProvider("world");
                anvil.Initialize();
                ChunkCoordinates coordinates = new ChunkCoordinates(x >> 4, z >> 4);
                ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);

                int i = dict.FindLastIndex(item => item.column.z == chunk.z && item.column.x == chunk.x);
                if (i == -1)
                {
                    dict.Add(new _tempItemOnBlock { column = chunk, list = new List<Block> { block } });
                }
                else
                {
                    dict[i].list.Add(block);
                }
            }

            foreach (var item in dict)
            {
                foreach (var block in item.list)
                {
                    int x = block.Coordinates.X, z = block.Coordinates.Z, y = block.Coordinates.Y;
                    item.column.SetBlock(x % 16, y, z % 16, block.Id);
                }
                AnvilWorldProvider.SaveChunk(item.column, "world", 0);
            }

            Console.WriteLine("save {0} (1: {1})", dict.Count, dict[1].list.Count);
        }*/
        
        #endregion
    }
}
