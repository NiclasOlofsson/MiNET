﻿using MiNET.Blocks;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public class DestroyBlockParticle : Particle
	{
		public DestroyBlockParticle(Level level, Block block) : base(0, level)
		{
			Data = (int)block.GetRuntimeId();
			Position = block.Coordinates;
		}

		public override void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = 2001;
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}
}