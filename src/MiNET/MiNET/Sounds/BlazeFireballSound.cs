using System.Numerics;

namespace MiNET.Sounds
{
	public class BlazeFireballSound : Sound
	{
		public BlazeFireballSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundBlazeFireball, position, pitch)
		{
		}
	}
}