using System.Collections.Generic;
using fNbt;
using MiNET.Utils;

namespace MiNET.BlockEntities
{
	public class BlockEntity
	{
		public string Id { get; private set; }
		public BlockCoordinates Coordinates { get; set; }

		protected BlockEntity(string id)
		{
			Id = id;
		}

		public virtual NbtCompound GetCompound()
		{
			return new NbtCompound();
		}

		public virtual void SetCompound(NbtCompound compound)
		{
		}

		public virtual List<ItemStack> GetDrops()
		{
			return new List<ItemStack>();
		}
	}
}