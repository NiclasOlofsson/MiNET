using System.Numerics;

namespace MiNET.Sounds
{
	public class DoorCloseSound : Sound
	{
		public DoorCloseSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundDoorClose, position, pitch)
		{
		}
	}
}