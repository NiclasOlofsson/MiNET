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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET;
using MiNET.Particles;

namespace MiNET.Test
{
	// These enums ARE the wire: the numeric value goes into a packet and the client renders
	// whatever that id means to it. The values are pinned against protocol 1001 as agreed by
	// PMMP BedrockProtocol and Mojang's protocol docs (via ViaBedrock's generated enums);
	// a value change here must come from a protocol bump, never from a refactor.
	[TestClass]
	public class WireEnumTests
	{
		// The old enum dropped ids (2, 7, 9, 18, 24, 26, ...) without leaving gaps, so every
		// member from 7 up was shifted and named the wrong particle on the wire.
		[TestMethod]
		public void ParticleType_PreviouslyShiftedMembers_CarryTheirWireIds()
		{
			// Was correct before the fix; pinned so the shift can never come back.
			Assert.AreEqual(1, (int) ParticleType.Bubble);
			Assert.AreEqual(3, (int) ParticleType.Critical);
			Assert.AreEqual(6, (int) ParticleType.Explode);
			Assert.AreEqual(8, (int) ParticleType.Flame);

			// Every one of these was off by at least one before the fix.
			Assert.AreEqual(10, (int) ParticleType.Lava);
			Assert.AreEqual(11, (int) ParticleType.LargeSmoke);
			Assert.AreEqual(12, (int) ParticleType.Redstone);
			Assert.AreEqual(14, (int) ParticleType.ItemBreak);
			Assert.AreEqual(16, (int) ParticleType.LargeExplode);
			Assert.AreEqual(17, (int) ParticleType.HugeExplode);
			Assert.AreEqual(19, (int) ParticleType.MobFlame);
			Assert.AreEqual(20, (int) ParticleType.Heart);
			Assert.AreEqual(21, (int) ParticleType.Terrain);
			Assert.AreEqual(23, (int) ParticleType.Portal);
			Assert.AreEqual(25, (int) ParticleType.WaterSplash);
			Assert.AreEqual(27, (int) ParticleType.WaterWake);
			Assert.AreEqual(28, (int) ParticleType.DripWater);
			Assert.AreEqual(29, (int) ParticleType.DripLava);
			Assert.AreEqual(30, (int) ParticleType.DripHoney);
			Assert.AreEqual(33, (int) ParticleType.Dust);
			Assert.AreEqual(43, (int) ParticleType.TrackingEmitter);
			Assert.AreEqual(49, (int) ParticleType.DragonsBreath);
			Assert.AreEqual(59, (int) ParticleType.Conduit);
			Assert.AreEqual(62, (int) ParticleType.Sneeze);

			// WhiteSmoke sat at 7, which is Evaporation's id; its real id is 89.
			Assert.AreEqual(89, (int) ParticleType.WhiteSmoke);
		}

		// The ids the old enum dropped, plus everything from 63 up that it never had.
		[TestMethod]
		public void ParticleType_BackfilledMembers_CarryTheirWireIds()
		{
			Assert.AreEqual(0, (int) ParticleType.Undefined);
			Assert.AreEqual(2, (int) ParticleType.BubbleManual);
			Assert.AreEqual(7, (int) ParticleType.Evaporation);
			Assert.AreEqual(9, (int) ParticleType.CandleFlame);
			Assert.AreEqual(18, (int) ParticleType.BreezeWindExplosion);
			Assert.AreEqual(24, (int) ParticleType.MobPortal);
			Assert.AreEqual(26, (int) ParticleType.WaterSplashManual);
			Assert.AreEqual(31, (int) ParticleType.StalactiteDripWater);
			Assert.AreEqual(32, (int) ParticleType.StalactiteDripLava);
			Assert.AreEqual(63, (int) ParticleType.ShulkerBullet);
			Assert.AreEqual(64, (int) ParticleType.Bleach);
			Assert.AreEqual(68, (int) ParticleType.CampfireSmoke);
			Assert.AreEqual(73, (int) ParticleType.Soul);
			Assert.AreEqual(76, (int) ParticleType.Snowflake);
			Assert.AreEqual(77, (int) ParticleType.VibrationSignal);
			Assert.AreEqual(83, (int) ParticleType.Shriek);
			Assert.AreEqual(85, (int) ParticleType.SonicExplosion);
			Assert.AreEqual(87, (int) ParticleType.CherryLeaves);
			Assert.AreEqual(90, (int) ParticleType.VaultConnection);
			Assert.AreEqual(91, (int) ParticleType.WindExplosion);
			Assert.AreEqual(92, (int) ParticleType.WolfArmorCrack);
			Assert.AreEqual(93, (int) ParticleType.OminousItemSpawner);
			Assert.AreEqual(94, (int) ParticleType.CreakingCrumble);
			Assert.AreEqual(95, (int) ParticleType.PaleOakLeaves);
			Assert.AreEqual(98, (int) ParticleType.GreenFlame);
			Assert.AreEqual(101, (int) ParticleType.SulfurCube);
		}

		// Every id and name here is a two-of-three ruling among direct reads of Cloudburst,
		// PMMP, and Mojang's docs: the fireball pair is ghast 1008 / blaze 1009, the item
		// frame breaks on 1041 and places on 1042, and 2008/2009 are deny-block and
		// generic-spawn (Cloudburst + Mojang; PMMP alone reads them as force-field and
		// projectile-hit). 1050/4000/9800 stand on PMMP alone, the other sources silent.
		[TestMethod]
		public void LevelEventType_DisputedMembers_CarryTheirWireIds()
		{
			Assert.AreEqual(1008, (int) LevelEventType.SoundGhastFireball);
			Assert.AreEqual(1009, (int) LevelEventType.SoundBlazeFireball);
			Assert.AreEqual(1041, (int) LevelEventType.SoundItemFrameBreak);
			Assert.AreEqual(1042, (int) LevelEventType.SoundItemFramePlaced);
			Assert.AreEqual(2008, (int) LevelEventType.ParticleDenyBlock);
			Assert.AreEqual(2009, (int) LevelEventType.ParticleGenericSpawn);
			Assert.AreEqual(4000, (int) LevelEventType.SetData);
			Assert.AreEqual(9800, (int) LevelEventType.PlayersSleeping);

			// Consensus-worded renames of members whose old names carried no source.
			Assert.AreEqual(1002, (int) LevelEventType.SoundLaunch);
			Assert.AreEqual(1005, (int) LevelEventType.SoundFuse);
			Assert.AreEqual(1007, (int) LevelEventType.SoundGhastWarning);
			Assert.AreEqual(1010, (int) LevelEventType.SoundZombieDoorBump);
			Assert.AreEqual(1012, (int) LevelEventType.SoundZombieDoorCrash);
			Assert.AreEqual(1017, (int) LevelEventType.SoundZombieConverted);
			Assert.AreEqual(1022, (int) LevelEventType.SoundAnvilLand);
			Assert.AreEqual(1050, (int) LevelEventType.SoundCamera);
			Assert.AreEqual(1051, (int) LevelEventType.SoundExperienceOrbPickup);
			Assert.AreEqual(2001, (int) LevelEventType.ParticlesDestroyBlock);
			Assert.AreEqual(2002, (int) LevelEventType.ParticlesPotionSplash);
			Assert.AreEqual(2003, (int) LevelEventType.ParticlesEyeOfEnderDeath);
			Assert.AreEqual(2004, (int) LevelEventType.ParticlesMobBlockSpawn);
			Assert.AreEqual(2013, (int) LevelEventType.ParticlesTeleport);
			Assert.AreEqual(2014, (int) LevelEventType.ParticlesCrackBlock);
			Assert.AreEqual(3001, (int) LevelEventType.StartRaining);
			Assert.AreEqual(3002, (int) LevelEventType.StartThunderstorm);
			Assert.AreEqual(3003, (int) LevelEventType.StopRaining);
			Assert.AreEqual(3004, (int) LevelEventType.StopThunderstorm);

			// Names ruled by two-of-three direct reads (Cloudburst + Mojang + PMMP): 1003 is
			// the door OPEN sound, 1030 the infinity-arrow pickup, 3500 activate-block. Ids
			// 1015 and 1031 exist in no source at this protocol and must stay absent.
			Assert.AreEqual(1003, (int) LevelEventType.SoundOpenDoor);
			Assert.AreEqual(1030, (int) LevelEventType.SoundInfinityArrowPickup);
			Assert.AreEqual(2005, (int) LevelEventType.ParticleCropGrowth);
			Assert.AreEqual(2006, (int) LevelEventType.ParticleSoundGuardianGhost);
			Assert.AreEqual(3500, (int) LevelEventType.ActivateBlock);
		}

		// Representatives of every backfilled range (1060s armor stand, 1900s music, 2007-2040
		// particles, 3005-3007 sim, 3503-3515 cauldron, 3600s block cracking, 9810+).
		[TestMethod]
		public void LevelEventType_BackfilledMembers_CarryTheirWireIds()
		{
			Assert.AreEqual(0, (int) LevelEventType.Undefined);
			Assert.AreEqual(1052, (int) LevelEventType.SoundTotemUsed);
			Assert.AreEqual(1064, (int) LevelEventType.SoundPointedDripstoneLand);
			Assert.AreEqual(1901, (int) LevelEventType.PlayCustomMusic);
			Assert.AreEqual(2007, (int) LevelEventType.ParticleDeathSmoke);
			Assert.AreEqual(2012, (int) LevelEventType.ParticlesCrit);
			Assert.AreEqual(2030, (int) LevelEventType.WaxOn);
			Assert.AreEqual(2039, (int) LevelEventType.SonicExplosion);
			Assert.AreEqual(2040, (int) LevelEventType.DustPlume);
			Assert.AreEqual(3005, (int) LevelEventType.GlobalPause);
			Assert.AreEqual(3505, (int) LevelEventType.CauldronTakePotion);
			Assert.AreEqual(3510, (int) LevelEventType.CauldronFlush);
			Assert.AreEqual(3600, (int) LevelEventType.StartBlockCracking);
			Assert.AreEqual(3611, (int) LevelEventType.ParticlesTrialSpawnerDetection);
			Assert.AreEqual(3617, (int) LevelEventType.AllPlayersSleeping);
			Assert.AreEqual(9810, (int) LevelEventType.JumpPrevented);
			Assert.AreEqual(9816, (int) LevelEventType.ParticleCreakingHeartTrail);
			Assert.AreEqual(0x4000, (int) LevelEventType.ParticleLegacyEvent);
		}

		// Bit indexes into the ActorFlags bitset. 0..52 were already correct (CanDash=46 is the
		// historically load-bearing one); 53..129 are the backfill. Note bits 64+ live in the
		// second flags long (metadata index 92), which GetFlags() does not emit yet.
		[TestMethod]
		public void EntityDataFlags_CarryTheirBitIndexes()
		{
			Assert.AreEqual(46, (int) Entities.Entity.DataFlags.CanDash);
			Assert.AreEqual(52, (int) Entities.Entity.DataFlags.Enchanted);
			Assert.AreEqual(53, (int) Entities.Entity.DataFlags.ReturnTrident);
			Assert.AreEqual(57, (int) Entities.Entity.DataFlags.Swimming);
			Assert.AreEqual(76, (int) Entities.Entity.DataFlags.Sleeping);
			Assert.AreEqual(93, (int) Entities.Entity.DataFlags.Celebrating);
			Assert.AreEqual(104, (int) Entities.Entity.DataFlags.Emerging);
			Assert.AreEqual(114, (int) Entities.Entity.DataFlags.Crawling);
			Assert.AreEqual(121, (int) Entities.Entity.DataFlags.Collidable);
			Assert.AreEqual(129, (int) Entities.Entity.DataFlags.NameplateDepthTested);
		}

		// McpeEntityEvent's event ids, previously bare integer literals at every call site.
		// The pinned ones are the ids those call sites were already sending.
		[TestMethod]
		public void ActorEventType_CarriesTheWireIds_TheCallSitesWereSending()
		{
			Assert.AreEqual(2, (int) Entities.ActorEventType.Hurt);
			Assert.AreEqual(3, (int) Entities.ActorEventType.Death);
			Assert.AreEqual(6, (int) Entities.ActorEventType.TamingFailed);
			Assert.AreEqual(7, (int) Entities.ActorEventType.TamingSucceeded);
			Assert.AreEqual(10, (int) Entities.ActorEventType.EatGrass);
			Assert.AreEqual(25, (int) Entities.ActorEventType.FireworksExplode);
			Assert.AreEqual(39, (int) Entities.ActorEventType.Shake);
			Assert.AreEqual(57, (int) Entities.ActorEventType.Feed);
			Assert.AreEqual(81, (int) Entities.ActorEventType.HurtWithoutReceivingDamage);
		}

		// LevelSoundEventType, settled by majority among Cloudburst (chain-order replay),
		// minecraft-data 1.26.30, and Mojang's docs. 113/155/222 are the spec-hiding cases:
		// both implementations carry them while Mojang's docs drop or rename them. 274 is a
		// real event, not a sentinel. The tail from 611 is BDS 1.26.60.30's own LevelSoundEvent
		// schema (explicit binary values), where Undefined is 615.
		[TestMethod]
		public void LevelSoundEventType_CarriesTheRuledWireIds()
		{
			Assert.AreEqual(0, (int) LevelSoundEventType.ItemUseOn);
			Assert.AreEqual(113, (int) LevelSoundEventType.StopRecord);
			Assert.AreEqual(155, (int) LevelSoundEventType.ImitateIllusionIllager);
			Assert.AreEqual(222, (int) LevelSoundEventType.SpawnBaby);
			Assert.AreEqual(256, (int) LevelSoundEventType.LecternBookPlace);
			Assert.AreEqual(258, (int) LevelSoundEventType.Bell);
			Assert.AreEqual(264, (int) LevelSoundEventType.CartographyTableUse);
			Assert.AreEqual(274, (int) LevelSoundEventType.AmbientInRaid);
			Assert.AreEqual(275, (int) LevelSoundEventType.UiCartographyTableUse);
			Assert.AreEqual(314, (int) LevelSoundEventType.RecordPigstep);
			Assert.AreEqual(383, (int) LevelSoundEventType.GoatCall0);
			Assert.AreEqual(483, (int) LevelSoundEventType.CrafterDisableSlot);
			Assert.AreEqual(530, (int) LevelSoundEventType.VaultRejectRewardedPlayer);
			Assert.AreEqual(610, (int) LevelSoundEventType.GeyserContinuousEruptionActive);
			Assert.AreEqual(611, (int) LevelSoundEventType.Mount);
			Assert.AreEqual(613, (int) LevelSoundEventType.StrawBedBreakLeave);
			Assert.AreEqual(614, (int) LevelSoundEventType.IceBallBreak);
			Assert.AreEqual(615, (int) LevelSoundEventType.Undefined);
		}

		// DamageCause is server-internal today, but it mirrors ActorDamageCause so a future
		// wire use cannot ship the old misalignment (Starving sat on Wither's 15, Custom on
		// Starve's 16). Custom is MiNET's own and lives above the reference range.
		[TestMethod]
		public void DamageCause_AlignsWithActorDamageCause()
		{
			Assert.AreEqual(14, (int) DamageCause.Magic);
			Assert.AreEqual(15, (int) DamageCause.Wither);
			Assert.AreEqual(16, (int) DamageCause.Starving);
			Assert.AreEqual(17, (int) DamageCause.Anvil);
			Assert.AreEqual(24, (int) DamageCause.Lightning);
			Assert.AreEqual(27, (int) DamageCause.Freezing);
			Assert.AreEqual(31, (int) DamageCause.SonicBoom);
			Assert.AreEqual(33, (int) DamageCause.SoulCampfire);
			Assert.AreEqual(100, (int) DamageCause.Custom);
		}
	}
}
