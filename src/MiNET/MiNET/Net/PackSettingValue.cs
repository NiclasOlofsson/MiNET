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

namespace MiNET.Net
{
	/// <summary>
	///     The tagged value of a resource pack setting change: a varint type tag, then the payload
	///     the tag selects. Tags 0-2 (float, bool, string) are the 2168 wire unchanged; 2192 adds
	///     tag 3, a varint-counted list of strings.
	/// </summary>
	public class PackSettingValue
	{
		public uint TypeId { get; set; }
		public float FloatValue { get; set; }
		public bool BoolValue { get; set; }
		public string StringValue { get; set; }
		public List<string> StringListValue { get; set; }
	}
}
