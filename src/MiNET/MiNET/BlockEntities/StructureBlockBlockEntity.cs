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

using fNbt;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.BlockEntities
{
	public class StructureBlockBlockEntity : BlockEntity
	{
		private NbtCompound Compound { get; set; }

		public BlockCoordinates Offset { get; set; } = new BlockCoordinates(0, -1, 0);
		public BlockCoordinates Size { get; set; } = new BlockCoordinates(5, 5, 5);
		public bool ShowBoundingBox { get; set; } = true;

		public StructureBlockBlockEntity() : base("StructureBlock")
		{
			Compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z),
				new NbtInt("xStructureOffset", Offset.X),
				new NbtInt("yStructureOffset", Offset.Y),
				new NbtInt("zStructureOffset", Offset.Z),
				new NbtInt("xStructureSize", Size.X),
				new NbtInt("yStructureSize", Size.Y),
				new NbtInt("zStructureSize", Size.Z),
				new NbtByte("showBoundingBox", (byte) (ShowBoundingBox ? 1 : 0)),
			};
		}


		public override NbtCompound GetCompound()
		{
			Compound["x"] = new NbtInt("x", Coordinates.X);
			Compound["y"] = new NbtInt("y", Coordinates.Y);
			Compound["z"] = new NbtInt("z", Coordinates.Z);
			Compound["xStructureOffset"] = new NbtInt("xStructureOffset", Offset.X);
			Compound["yStructureOffset"] = new NbtInt("yStructureOffset", Offset.Y);
			Compound["zStructureOffset"] = new NbtInt("zStructureOffset", Offset.Z);
			Compound["xStructureSize"] = new NbtInt("xStructureSize", Size.X);
			Compound["yStructureSize"] = new NbtInt("yStructureSize", Size.Y);
			Compound["zStructureSize"] = new NbtInt("zStructureSize", Size.Z);
			Compound["showBoundingBox"] = new NbtByte("showBoundingBox", (byte) (ShowBoundingBox ? 1 : 0));

			return Compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Offset = new BlockCoordinates(compound["xStructureOffset"].IntValue, compound["yStructureOffset"].IntValue, compound["zStructureOffset"].IntValue);
			Size = new BlockCoordinates(compound["xStructureSize"].IntValue, compound["yStructureSize"].IntValue, compound["zStructureSize"].IntValue);
			ShowBoundingBox = compound["showBoundingBox"].ByteValue == 1;

			Compound = compound;
		}
	}
}