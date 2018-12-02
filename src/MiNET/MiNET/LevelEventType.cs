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

namespace MiNET
{
	public enum LevelEventType : short
	{
		SoundClick = 1000,
		SoundClickFail = 1001, // try also with data 1200 for Dispenser sound
		SoundShoot = 1002,
		SoundDoorClose = 1003, // spooky sound with data 20
		SoundFizz = 1004,
		SoundTNTFuse = 1005,

		SoundGhast = 1007,
		SoundBlazeFireball = 1008, // same as the one below
		SoundGhastFireball = 1009,
		SoundZombieDoorHit = 1010,
		SoundZombieBreakDoor = 1012,

		SoundBatFly = 1015,
		SoundZombieInfect = 1016,
		SoundZombieHeal = 1017,
		SoundEndermanTeleport = 1018,

		SoundAnvilBreak = 1020,
		SoundAnvilUse = 1021,
		SoundAnvilFall = 1022,

		SoundItemDrop = 1030,
		SoundItemThrown = 1031,

		SoundItemFrameItemAdded = 1040,
		SoundItemFramePlaced = 1041,
		SoundItemFrameRemoved = 1042,
		SoundItemFrameItemRemoved = 1043,
		SoundItemFrameItemRotated = 1044, // Not sure if the names are correct

		SoundCameraTakePicture = 1050,
		SoundExpOrb = 1051,

		SoundButtonClick = 3500,

		ParticleShoot = 2000,
		ParticleDestroy = 2001,
		ParticleSplash = 2002,
		ParticleEyeDespawn = 2003,
		ParticleSpawn = 2004,
		ParticleGreenThingy = 2005,

		StartRain = 3001,
		StartThunder = 3002,
		StopRain = 3003,
		StopThunder = 3004,

		DankMemes = 3500,
		CauldronExplode = 3501, // no idea what it is or why it is named like that
		CauldronDyeArmor = 3502,
		CauldronFillPotion = 3504,
		CauldronFillWater = 3506,

		SetData = 4000,

		PlayersSleeping = 9800,
	}
}