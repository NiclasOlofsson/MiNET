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

using System;
using MiNET.Items;

namespace MiNET.Blocks
{
	public partial class Wheat : Crops
	{
		public Wheat() : base(59)
		{
		}

		public override Item[] GetDrops(Item tool)
		{
			if (Growth == 7)
			{
				// Can also return 0-3 seeds at random.
				var rnd = new Random();
				var count = rnd.Next(4);
				if (count > 0)
				{
					return new[] {ItemFactory.GetItem(296, 0, 1), ItemFactory.GetItem(295, 0, (byte) count)};
				}
				return new[] {ItemFactory.GetItem(296, 0, 1)};
			}

			return new[] {ItemFactory.GetItem(295, 0, 1)};
		}
	}
}