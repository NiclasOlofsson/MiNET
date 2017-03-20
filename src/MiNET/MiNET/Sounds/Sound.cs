using System.Numerics;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Sounds
{
	public class Sound
	{
		public short Id { get; private set; }
		public int Pitch { get; set; }
		public Vector3 Position { get; set; }

		public Sound(short id, Vector3 position, int pitch = 0)
		{
			Id = id;
			Position = position;
			Pitch = pitch;
		}


		public virtual void Spawn(Level level)
		{
			McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			levelEvent.eventId = Id;
			levelEvent.data = Pitch;
			levelEvent.position = Position;

			level.RelayBroadcast(levelEvent);
		}
	}
}