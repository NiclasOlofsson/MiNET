using System.Numerics;

namespace MiNET.Sounds
{
	public class GhastSound : Sound
	{
		public GhastSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundGhast, position, pitch)
		{
		}
	}
}
