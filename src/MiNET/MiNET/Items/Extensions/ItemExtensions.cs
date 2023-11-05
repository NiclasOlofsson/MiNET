using System;
using MiNET.Blocks;

namespace MiNET.Items.Extensions
{
	public static class ItemExtensions
	{
		public static bool IsItemBlockOf<T>(this Item item) where T : Block
		{
			return IsItemBlockOf(item, typeof(T));
		}

		public static bool IsItemBlockOf(this Item item, Type blockType)
		{
			if (item is not ItemBlock) return false;
			if (item.BlockRuntimeId == 0) return blockType == typeof(Air);

			return BlockFactory.GetIdByRuntimeId(item.BlockRuntimeId)
				.Equals(BlockFactory.GetIdByType(blockType));
		}
	}
}
