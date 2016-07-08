using System.Numerics;

namespace MiNET.Sounds
{
	public class FizzSound : Sound
	{
		public FizzSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundFizz, position, pitch)
		{
		}
	}
}