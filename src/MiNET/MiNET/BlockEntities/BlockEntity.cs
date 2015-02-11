﻿using System.Collections.Generic;
using Craft.Net.Common;
using fNbt;
using ItemStack = MiNET.Utils.ItemStack;

namespace MiNET.BlockEntities
{
	public class BlockEntity
	{
		public string Id { get; private set; }
		public Coordinates3D Coordinates { get; set; }

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