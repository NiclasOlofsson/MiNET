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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds.Structures
{
	class AcaciaTree : Structure
	{
		public override string Name
		{
			get { return "AcaciaTree"; }
		}

		public override int MaxHeight
		{
			get { return 8; }
		}

		public override Block[] Blocks
		{
			get
			{
				return new Block[]
				{
					new Block(162) {Coordinates = new BlockCoordinates(0, 0, 0)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 1, 0)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 2, 0)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 3, 0)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 4, 0)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 5, -1)},
					new Block(162) {Coordinates = new BlockCoordinates(0, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-3, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(-3, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, 1)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 6, -5)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, 1)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 6, -5)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, 1)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 6, -5)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, 1)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 6, -5)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, 1)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 6, -5)},
					new Block(161) {Coordinates = new BlockCoordinates(3, 6, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(3, 6, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(3, 6, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(3, 6, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(3, 6, -4)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 7, 0)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 7, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 7, -1)},
					new Block(161) {Coordinates = new BlockCoordinates(-2, 7, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 7, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 7, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(2, 7, -2)},
					new Block(161) {Coordinates = new BlockCoordinates(-1, 7, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 7, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(1, 7, -3)},
					new Block(161) {Coordinates = new BlockCoordinates(0, 7, -4)},
				};
			}
		}
	}
}