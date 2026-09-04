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
using System.Numerics;
using MiNET.Net;

namespace MiNET.Camera
{
	/// <summary>
	///     The camera presets vanilla ships, and their names. A preset is a named camera
	///     configuration the client holds; these six carry almost no values because the client
	///     already implements them, so naming <see cref="FirstPerson" /> is the whole instruction.
	///     Presets with real values in them go through <see cref="CameraManager.AddPreset" />.
	///
	///     <see cref="Vanilla" /> hands out fresh instances because each player owns their own copy.
	///     The values match behavior_packs/*/cameras/presets/*.json, which is what BDS reads.
	/// </summary>
	public static class CameraPresets
	{
		public const string FirstPerson = "minecraft:first_person";
		public const string FixedBoom = "minecraft:fixed_boom";
		public const string FollowOrbit = "minecraft:follow_orbit";
		public const string Free = "minecraft:free";
		public const string ThirdPerson = "minecraft:third_person";
		public const string ThirdPersonFront = "minecraft:third_person_front";

		public static IEnumerable<CameraPreset> Vanilla
		{
			get
			{
				yield return new CameraPreset {name = FirstPerson, inheritFrom = ""};

				// The boom presets pin an offset even though it is the origin: the client
				// distinguishes "no offset given" from "an offset of zero". The -0 on X is what BDS
				// puts on the wire (its own preset file says 0.0), and it is a different float, so
				// it stays.
				yield return new CameraPreset
				{
					name = FixedBoom,
					inheritFrom = "",
					viewOffset = Vector2.Zero,
					entityOffset = new Vector3(-0f, 0f, 0f)
				};
				yield return new CameraPreset
				{
					name = FollowOrbit,
					inheritFrom = "",
					viewOffset = Vector2.Zero,
					entityOffset = new Vector3(-0f, 0f, 0f),
					radius = 10
				};

				// The free camera is the only one placed absolutely, so it opens at the origin and
				// stays there until an instruction moves it.
				yield return new CameraPreset
				{
					name = Free,
					inheritFrom = "",
					posX = 0,
					posY = 0,
					posZ = 0,
					rotX = 0,
					rotY = 0
				};

				yield return new CameraPreset {name = ThirdPerson, inheritFrom = ""};
				yield return new CameraPreset {name = ThirdPersonFront, inheritFrom = ""};
			}
		}
	}
}
