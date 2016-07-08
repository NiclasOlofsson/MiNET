using System.Numerics;

namespace MiNET.Sounds
{
	public class ExperienceOrbSound : Sound
	{
		public ExperienceOrbSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundExpOrb, position, pitch)
		{
		}
	}
}