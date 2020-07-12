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

using fNbt;

namespace MiNET.BlockEntities
{
	public class SignBlockEntity : BlockEntity
	{
		public string Text { get; set; }
		public string Text1 { get; set; }
		public string Text2 { get; set; }
		public string Text3 { get; set; }
		public string Text4 { get; set; }

		public SignBlockEntity() : base("Sign")
		{
			Text = string.Empty;
			Text1 = string.Empty;
			Text2 = string.Empty;
			Text3 = string.Empty;
			Text4 = string.Empty;
		}

		public override NbtCompound GetCompound()
		{
			var compound = new NbtCompound(string.Empty)
			{
				new NbtString("id", Id),
				new NbtString("Text", Text ?? string.Empty),
				new NbtString("Text1", Text1 ?? string.Empty),
				new NbtString("Text2", Text2 ?? string.Empty),
				new NbtString("Text3", Text3 ?? string.Empty),
				new NbtString("Text4", Text4 ?? string.Empty),
				new NbtInt("x", Coordinates.X),
				new NbtInt("y", Coordinates.Y),
				new NbtInt("z", Coordinates.Z)
			};

			return compound;
		}

		public override void SetCompound(NbtCompound compound)
		{
			Text = GetTextValue(compound, "Text");
			Text1 = GetTextValue(compound, "Text1");
			Text2 = GetTextValue(compound, "Text2");
			Text3 = GetTextValue(compound, "Text3");
			Text4 = GetTextValue(compound, "Text4");
		}

		private string GetTextValue(NbtCompound compound, string key)
		{
			compound.TryGet(key, out NbtString text);
			return text != null ? (text.StringValue ?? string.Empty) : string.Empty;
		}
	}
}