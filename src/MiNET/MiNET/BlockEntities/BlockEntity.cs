using System.Collections.Generic;
using fNbt;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.BlockEntities
{
	public class BlockEntity
	{
		public string Id { get; private set; }
		public BlockCoordinates Coordinates { get; set; }

		public bool UpdatesOnTick { get; set; }

		public BlockEntity(string id)
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

		public virtual void OnTick(Level level)
		{
		}


		public virtual List<Item> GetDrops()
		{
			return new List<Item>();
		}
	}
}