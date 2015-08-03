using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class Particle
	{
		public int Id { get; private set; }
		protected Level Level { get; set; }
		public Vector3 Position { get; set; }
		protected int Data { get; set; }

		protected Particle(int id, Level level)
		{
			Id = id;
			Level = level;
		}

		public virtual void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.x = (float) Position.X;
			particleEvent.y = (float) Position.Y;
			particleEvent.z = (float) Position.Z;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}

}