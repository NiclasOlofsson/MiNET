using System.Numerics;

namespace MiNET.Sounds
{
	public class ClickFailSound : Sound
	{
		public ClickFailSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundClickFail, position, pitch)
		{
		}
	}
}