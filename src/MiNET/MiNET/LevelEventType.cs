namespace MiNET
{
	public enum LevelEventType : short
	{
		SoundClick = 1000,
		SoundClickFail = 1001,
		SoundShoot = 1002,
		SoundDoor = 1003,
		SoundFizz = 1004,

		SoundGhast = 1007,
		SoundGhastShoot = 1008,
		SoundBlazeShoot = 1009,

		SoundDoorBump = 1010,
		SoundDoorCrash = 1012,

		SoundBatFly = 1015,
		SoundZombieInfect = 1016,
		SoundZombieHeal = 1017,
		SoundEndermanTeleport = 1018,

		SoundAnvilBreak = 1020,
		SoundAnvilUse = 1021,
		SoundAnvilFall = 1022,

		SoundButtonClick = 3500,

		ParticleShoot = 2000,
		ParticleDestroy = 2001,
		ParticleSplash = 2002,
		ParticleEyeDespawn = 2003,
		ParticleSpawn = 2004,

		StartRain = 3001,
		StartThunder = 3002,
		StopRain = 3003,
		StopThunder = 3004,

		SetData = 4000,

		PlayersSleeping = 9800,
	}
}