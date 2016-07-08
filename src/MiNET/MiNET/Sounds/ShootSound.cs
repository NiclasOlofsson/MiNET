using System.Numerics;

namespace MiNET.Sounds
{
	public class ShootSound : Sound
	{
		public ShootSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundShoot, position, pitch)
		{
		}
	}
}