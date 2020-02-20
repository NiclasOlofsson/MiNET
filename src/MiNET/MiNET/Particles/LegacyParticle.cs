#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Numerics;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public enum ParticleType
	{
		Bubble = 1,
		Critical = 3,
		BlockForceField =4,
		Smoke=5,
		Explode=6,
		WhiteSmoke=7,
		Flame=8,
		Lava=9,
		LargeSmoke=10,
		Redstone=11,
		RisingRedDust=12,
		ItemBreak = 13,
		SnowballPoof = 14,
		LargeExplode = 15,
		HugeExplode = 16,
		MobFlame = 17,
		Heart = 18,
		Terrain = 19,
		TownAura = 20,
		Portal = 21,
		WaterSplash = 23,
		WaterWake = 25,
		DripWater = 26,
		DripLava = 27,
		DripHoney= 28,
		Dust,
		MobSpell,
		MobSpellAmbient,
		MobSpellInstantaneous,
		Ink,
		Slime,
		RainSplash,
		VillagerAngry,
		VillagerHappy,
		EnchantmentTable,
		TrackingEmitter,
		Note,
		WitchSpell,
		Carrot,
		Unknown39,
		EndRod,
		DragonsBreath,
		Spit,
		Totem,
		Food,
		FireworksStarter,
		FireworksSpark,
		FireworksOverlay,
		BalloonGas,
		ColoredFlame,
		Sparkler,
		Conduit,
		BubbleColumnUp,
		BubbleColumnDown,
		Sneeze
	}

	public class LegacyParticle : Particle
	{
		public int Id { get; private set; }
		protected int Data { get; set; }

		public LegacyParticle(ParticleType particle, Level level): this((int)particle, level)
		{
		}

		public LegacyParticle(int id, Level level) : base(level)
		{
			Id = id;
			Level = level;
		}

		public override void Spawn(Player[] players)
		{
			var particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(players, particleEvent);
		}
	}
}