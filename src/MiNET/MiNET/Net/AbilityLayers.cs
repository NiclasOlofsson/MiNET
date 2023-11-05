﻿#region LICENSE
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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2022 Niclas Olofsson.
// All Rights Reserved.
#endregion

using System;
using System.Collections.Generic;

namespace MiNET.Net;

public class AbilityLayers : List<AbilityLayer>
{
	
}

public class AbilityLayer
{
	public AbilityLayerType Type;
	public PlayerAbility Abilities;
	public uint Values;
	public float FlySpeed;
	public float WalkSpeed;
}

public enum AbilityLayerType
{
	CustomCache = 0,
	Base = 1,
	Spectator = 2,
	Commands = 3,
	Editor = 4
}

[Flags]
public enum PlayerAbility : uint
{
	Build = 1 << 0,
	Mine = 1 << 1,
	DoorsAndSwitches = 1 << 2,
	OpenContainers = 1 << 3,
	AttackPlayers = 1 << 4,
	AttackMobs = 1 << 5,
	OperatorCommands = 1 << 6,
	Teleport = 1 << 7,
	Invulnerable = 1 << 8,
	Flying = 1 << 9,
	MayFly = 1 << 10,
	InstantBuild = 1 << 11,
	Lightning = 1 << 12,
	FlySpeed = 1 << 13,
	WalkSpeed = 1 << 14,
	Muted = 1 << 15,
	WorldBuilder = 1 << 16,
	NoClip = 1 << 17,
	PrivilegedBuilder= 1 << 18
}