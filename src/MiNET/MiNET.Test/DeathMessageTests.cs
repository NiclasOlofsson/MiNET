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

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Projectiles;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Test
{
	/// <summary>
	///     A death message goes out as BDS sends it: a death.attack.* translation key with the victim
	///     and the killer as parameters, a mob named by its entity name key. The client words and
	///     localizes it, so every mob reads with its real name in the player's own language.
	/// </summary>
	[TestClass]
	public class DeathMessageTests
	{
		// BDS 1.26.70.21 telling gurunx they were killed by a zombie (Text, id 9).
		private const string BdsZombieKill = "090102021064656174682e61747461636b2e6d6f620206677572756e781325656e746974792e7a6f6d6269652e6e616d65000000";

		private class CapturingLevel : Level
		{
			public readonly List<McpeText> Messages = new List<McpeText>();

			public CapturingLevel() : base(null, "death", new AnvilWorldProvider {MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld)}, new EntityManager())
			{
			}

			public override void BroadcastMessage(string text, MessageType type = MessageType.Chat, Player sender = null, Player[] sendList = null, bool needsTranslation = false, string[] parameters = null)
			{
				var message = McpeText.CreateObject();
				message.type = (byte) type;
				message.message = text;
				message.needsTranslation = needsTranslation;
				message.parameters = parameters;
				Messages.Add(message);
			}
		}

		private static McpeText DeathMessage(Entity source, DamageCause cause, string victim = "Steve")
		{
			var level = new CapturingLevel();
			var player = new Player(null, null) {Level = level, Username = victim};
			player.BroadcastDeathMessage(source, cause);
			return level.Messages.Single();
		}

		private static void AssertTranslation(McpeText message, string key, params string[] parameters)
		{
			Assert.AreEqual((byte) MessageType.Translation, message.type);
			Assert.IsTrue(message.needsTranslation);
			Assert.AreEqual(key, message.message);
			CollectionAssert.AreEqual(parameters, message.parameters);
		}

		[TestMethod]
		public void KilledByMob_IsByteIdenticalToBds()
		{
			byte[] bds = Convert.FromHexString(BdsZombieKill);

			McpeText ours = DeathMessage(new Zombie(null), DamageCause.EntityAttack, victim: "gurunx");

			CollectionAssert.AreEqual(bds, ours.Encode());
		}

		[TestMethod]
		public void KilledByNamedMob_UsesTheNameTag()
		{
			AssertTranslation(DeathMessage(new Zombie(null) {NameTag = "Bob"}, DamageCause.EntityAttack), "death.attack.mob", "Steve", "Bob");
		}

		[TestMethod]
		public void KilledByPlayer_NamesThePlayer()
		{
			var killer = new Player(null, null) {Username = "Alex", NameTag = "§cAlex"};
			AssertTranslation(DeathMessage(killer, DamageCause.EntityAttack), "death.attack.player", "Steve", "Alex");
		}

		// The killing arrow is the damage source, but the message names whoever shot it.
		[TestMethod]
		public void ShotByPlayer_NamesTheShooter()
		{
			var shooter = new Player(null, null) {Username = "Alex"};
			AssertTranslation(DeathMessage(new Arrow(shooter, null), DamageCause.Projectile), "death.attack.arrow", "Steve", "Alex");
		}

		[TestMethod]
		public void ShotWithoutShooter_NamesTheArrow()
		{
			AssertTranslation(DeathMessage(new Arrow(null, null), DamageCause.Projectile), "death.attack.arrow", "Steve", "%entity.arrow.name");
		}

		[TestMethod]
		public void DiedWithoutKiller_NamesOnlyTheVictim()
		{
			AssertTranslation(DeathMessage(null, DamageCause.Fall), "death.attack.fall", "Steve");
		}
	}
}