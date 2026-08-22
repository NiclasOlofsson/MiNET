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

namespace MiNET.BdsExtract;

using System.Globalization;
using System.Text.Json;

/// <summary>
///     How a value is stated, for the two things every reader of this tool needs to agree on: how
///     many decimals a float is written to, and how much of a population a measured position has to
///     hold before it counts.
/// </summary>
public static class Sentinels
{
	/// <summary>
	///     How much of the population a position must hold before it counts as holding a member.
	///     Niclas set it at 95%. The measurements have room on both sides: a value the game changed
	///     between builds moves at most 18 blocks of 1,429, and the best position that is NOT the
	///     member scores 79% where a bit that is always false scores 65%.
	/// </summary>
	public const double Floor = 0.95;

	/// <summary>
	///     The decimals the game states a field to, so a float is written as the value Minecraft has
	///     rather than the binary that carries it. Three for friction, because blue ice is 0.989; two
	///     everywhere else, terracotta hardness 1.25 and barrier's blast resistance 3600000.75 being
	///     the most precise values those fields take.
	/// </summary>
	/// <remarks>
	///     A member inside a container carries the path it sits under, so the field is the last name
	///     in it: directData.friction is friction wherever it is declared.
	/// </remarks>
	private static int Decimals(string field) => field[(field.LastIndexOf('.') + 1)..] == "friction" ? 3 : 2;

	/// <summary>A float as the value the game states, rounded to that field's decimals.</summary>
	public static string Number(string field, float value)
	{
		// Rounded as a double, because a float's own formatting caps at seven significant digits
		// and turns barrier's 3600000.75 into 3600001.
		return float.IsFinite(value)
			? Math.Round((double) value, Decimals(field)).ToString("R", CultureInfo.InvariantCulture)
			: "null";
	}
}
