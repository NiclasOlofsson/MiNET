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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using fNbt;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Nbt;

namespace MiNET.Blocks
{
	/// <summary>
	///     The block definitions StartGame carries: one entry per block whose class states a
	///     <see cref="BlockDefinition" />, in the palette's block order, serialized once. A block
	///     without a definition is one the client already knows and is not sent.
	/// </summary>
	public static class BlockDefinitions
	{
		private static readonly Lazy<List<ServerBlockProperty>> _serverBlockProperties = new Lazy<List<ServerBlockProperty>>(Build);

		/// <summary>The list StartGame sends. Built on first use, shared by every join.</summary>
		public static List<ServerBlockProperty> ServerBlockProperties()
		{
			return _serverBlockProperties.Value;
		}

		private static List<ServerBlockProperty> Build()
		{
			var result = new List<ServerBlockProperty>();
			var seen = new HashSet<string>(StringComparer.Ordinal);
			BlockPalette palette = BlockFactory.BlockPalette;
			for (int i = 0; i < palette.Count; i++)
			{
				string name = palette[i].Name;
				if (!seen.Add(name)) continue;

				BlockDefinition definition = BlockFactory.GetBlockByName(name)?.Definition;
				if (definition == null) continue;

				NbtCompound root = BlockDefinitionWriter.Write(definition);
				root.Name = "";
				result.Add(new ServerBlockProperty
				{
					blockName = name,
					blockDefinition = new Nbt {NbtFile = new NbtFile(root) {BigEndian = false, UseVarInt = true}}
				});
			}

			return result;
		}
	}
}
