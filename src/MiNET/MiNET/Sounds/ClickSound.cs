using System.Numerics;

namespace MiNET.Sounds
{
	public class ClickSound : Sound
	{
		public ClickSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundClick, position, pitch)
		{
		}
	}
}