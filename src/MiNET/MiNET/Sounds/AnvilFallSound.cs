using System.Numerics;

namespace MiNET.Sounds
{
	public class AnvilFallSound : Sound
	{
		public AnvilFallSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundAnvilFall, position, pitch)
		{
		}
	}
}