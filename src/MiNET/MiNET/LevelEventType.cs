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