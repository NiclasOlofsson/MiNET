using System.Numerics;

namespace MiNET.Sounds
{
	public class AnvilBreakSound : Sound
	{
		public AnvilBreakSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundAnvilBreak, position, pitch)
		{
		}
	}
}