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
	class BirchTree : Structure
	{
		public override string Name
		{
			get { return "BirchTree"; }
		}

		public override int MaxHeight
		{
			get { return 7; }
		}

		public override Block[] Blocks
		{
			get
			{
				return new Block[]
				{
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 0, 0),
						Metadata = 2
					},
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 1, 0),
						Metadata = 2
					},
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 2, 0),
						Metadata = 2
					},
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 3, 0),
						Metadata = 2
					},
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 4, 0),
						Metadata = 2
					},
					new Block(17)
					{
						Coordinates = new BlockCoordinates(0, 5, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 3, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 3, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 3, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 3, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 3, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 3, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 3, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 3, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 3, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 3, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 3, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 3, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 3, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 3, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 3, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 3, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 3, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 3, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 3, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 3, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 3, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 3, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 4, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 4, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 4, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-2, 4, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 4, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 4, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 4, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 4, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 4, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 4, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 4, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 4, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 4, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 4, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 4, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 4, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 4, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 4, -2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 4, 2),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 4, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 4, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(2, 4, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 5, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 5, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 5, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 5, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 5, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(-1, 6, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 6, 1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 6, 0),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(0, 6, -1),
						Metadata = 2
					},
					new Block(18)
					{
						Coordinates = new BlockCoordinates(1, 6, 0),
						Metadata = 2
					},
				};
			}
		}
	}
}