using System.Numerics;

namespace MiNET.Sounds
{
	public class AnvilUseSound : Sound
	{
		public AnvilUseSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundAnvilUse, position, pitch)
		{
		}
	}
}