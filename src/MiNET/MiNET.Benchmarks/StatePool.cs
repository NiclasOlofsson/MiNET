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

using System.Collections.Generic;
using MiNET.Utils;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     State instances handed out per entry and reused on the next one. A palette entry's states
	///     live only long enough to hash and compare against the stored palette, so nothing here ever
	///     escapes: the lookup returns the stored container and the scratch is refilled.
	/// </summary>
	public sealed class StatePool
	{
		private readonly List<BlockStateByte> _bytes = new();
		private readonly List<BlockStateInt> _ints = new();
		private readonly List<BlockStateString> _strings = new();

		private int _byteCursor;
		private int _intCursor;
		private int _stringCursor;

		public void Reset()
		{
			_byteCursor = 0;
			_intCursor = 0;
			_stringCursor = 0;
		}

		public BlockStateByte Byte(string name, byte value)
		{
			if (_byteCursor == _bytes.Count) _bytes.Add(new BlockStateByte());

			BlockStateByte state = _bytes[_byteCursor++];
			state.Name = name;
			state.Value = value;

			return state;
		}

		public BlockStateInt Int(string name, int value)
		{
			if (_intCursor == _ints.Count) _ints.Add(new BlockStateInt());

			BlockStateInt state = _ints[_intCursor++];
			state.Name = name;
			state.Value = value;

			return state;
		}

		public BlockStateString String(string name, string value)
		{
			if (_stringCursor == _strings.Count) _strings.Add(new BlockStateString());

			BlockStateString state = _strings[_stringCursor++];
			state.Name = name;
			state.Value = value;

			return state;
		}
	}
}
