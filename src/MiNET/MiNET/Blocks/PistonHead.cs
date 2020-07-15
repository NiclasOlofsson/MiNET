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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using MiNET.Items;

namespace MiNET.Blocks
{
	public partial class PistonArmCollision : Block
	{
		public PistonArmCollision() : base(34)
		{
			BlastResistance = 2.5f;
			IsTransparent = true;
			// runtime id: 1580 0x62C, data: 0
			// runtime id: 2117 0x845, data: 1
			// runtime id: 71 0x47, data: 2
			// runtime id: 1499 0x5DB, data: 3
			// runtime id: 2605 0xA2D, data: 4
			// runtime id: 124 0x7C, data: 5
			// runtime id: 580 0x244, data: 6
			// runtime id: 175 0xAF, data: 7
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}