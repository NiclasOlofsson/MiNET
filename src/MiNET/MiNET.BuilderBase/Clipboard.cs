using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using MiNET.Blocks;
using MiNET.BuilderBase.Masks;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.BuilderBase
{
	public class Clipboard
	{
		private readonly Level _level;
		private List<Block> _buffer;

		public Mask SourceMask { get; set; }
		public BlockCoordinates Origin { get; set; }
		public Func<BlockCoordinates, bool> SourceFuncion { get; set; }
		public BlockCoordinates OriginPosition1 { get; set; }
		public BlockCoordinates OriginPosition2 { get; set; }
		public Matrix4x4 Transform { get; set; } = Matrix4x4.Identity;

		public Clipboard(Level level, List<Block> buffer = null)
		{
			_level = level;
			_buffer = buffer;
		}

		public void Fill(BlockCoordinates[] selected)
		{
			_buffer = new List<Block>();

			foreach (BlockCoordinates coordinate in selected)
			{
				if (SourceMask != null)
				{
					if (!SourceMask.Test(coordinate)) continue;
				}

				_buffer.Add(_level.GetBlock(coordinate));
				SourceFuncion?.Invoke(coordinate);
			}
		}

		public Block[] GetBuffer()
		{
			List<Block> copy = new List<Block>();
			foreach (var block in _buffer)
			{
				Block clone = (Block) block.Clone();
				copy.Add(clone);
			}

			return copy.ToArray();
		}

		public BlockCoordinates GetMin()
		{
			return new BlockCoordinates(Math.Min(OriginPosition1.X, OriginPosition2.X), Math.Min(OriginPosition1.Y, OriginPosition2.Y), Math.Min(OriginPosition1.Z, OriginPosition2.Z));
		}

		public BlockCoordinates GetMax()
		{
			return new BlockCoordinates(Math.Max(OriginPosition1.X, OriginPosition2.X), Math.Max(OriginPosition1.Y, OriginPosition2.Y), Math.Max(OriginPosition1.Z, OriginPosition2.Z));
		}
	}
}