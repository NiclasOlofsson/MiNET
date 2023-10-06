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
using System.Collections.Generic;
using MiNET.Crafting;

namespace MiNET.Net
{
	public partial class McpeTrimData : Packet<McpeTrimData>
	{
		public List<TrimPattern> Patterns;
		public List<TrimMaterial> Materials;

		partial void AfterEncode()
		{
			WriteUnsignedVarInt((uint) Patterns.Count);
			foreach (var pattern in Patterns)
			{
				Write(pattern.ItemId);
				Write(pattern.PatternId);
			}

			WriteUnsignedVarInt((uint) Materials.Count);
			foreach (var material in Materials)
			{
				Write(material.MaterialId);
				Write(material.Color);
				Write(material.ItemId);
			}
		}

		partial void AfterDecode()
		{
			Patterns = new List<TrimPattern>();
			int countPattern = (int) ReadUnsignedVarInt();
			for (int i = 0; i < countPattern; i++)
			{
				TrimPattern pattern = new TrimPattern();
				pattern.ItemId = ReadString();
				pattern.PatternId = ReadString();
				Patterns.Add(pattern);
			}

			Materials = new List<TrimMaterial>();
			int countMaterial = (int) ReadUnsignedVarInt();
			for (int i = 0; i < countMaterial; i++)
			{
				TrimMaterial material = new TrimMaterial();
				material.MaterialId = ReadString();
				material.Color = ReadString();
				material.ItemId = ReadString();
				Materials.Add(material);
			}
		}
	}
}