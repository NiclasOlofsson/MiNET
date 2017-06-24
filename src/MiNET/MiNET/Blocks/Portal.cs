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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Portal : Block
	{
		public Portal() : base(90)
		{
			IsTransparent = true;
			IsSolid = false;
			LightLevel = 11;
			Hardness = 60000;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			bool shouldKeep = true;
			shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.Up));
			shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.Down));

			if (Metadata < 2)
			{
				shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.West));
				shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.East));
			}
			else
			{
				shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.South));
				shouldKeep &= IsValid(level.GetBlock(Coordinates + BlockCoordinates.North));
			}

			if (!shouldKeep)
			{
				level.BreakBlock(this);
			}
		}

		private bool IsValid(Block block)
		{
			return block is Obsidian || block is Portal;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}