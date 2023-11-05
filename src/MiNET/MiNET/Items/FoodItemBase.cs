﻿#region LICENSE

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

using log4net;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public abstract class FoodItemBase : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(FoodItemBase));

		public int FoodPoints { get; set; }
		public double SaturationRestore { get; set; }

		public FoodItemBase(int foodPoints, double saturationRestore) : this(0, foodPoints, saturationRestore)
		{

		}

		public FoodItemBase(short metadata, int foodPoints, double saturationRestore) : base()
		{
			Metadata = metadata;
			FoodPoints = foodPoints;
			SaturationRestore = saturationRestore;
		}

		public FoodItemBase()
		{
			Log.Warn($"Does not implemented food partial for [{Id}]");
		}

		private bool _isUsing;

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			if (player.GameMode == GameMode.Survival || player.GameMode == GameMode.Adventure)
			{
				if (_isUsing)
				{
					Consume(player);

					Count--;
					player.Inventory.SetInventorySlot(player.Inventory.InHandSlot, this);
					_isUsing = false;
					return;
				}

				if (player.HungerManager.CanEat()) _isUsing = true;
			}
		}

		public override void Release(Level world, Player player, BlockCoordinates blockCoordinates)
		{
			_isUsing = false;
		}

		public virtual void Consume(Player player)
		{
			player.HungerManager.IncreaseFoodAndSaturation(this, FoodPoints, SaturationRestore);
		}
	}
}