using System.Numerics;

namespace MiNET.Sounds
{
	public class CameraTakePictureSound : Sound
	{
		public CameraTakePictureSound(Vector3 position, int pitch = 0) : base((short) LevelEventType.SoundCameraTakePicture, position, pitch)
		{
		}
	}
}