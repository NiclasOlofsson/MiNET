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

using System;
using MiNET;
using MiNET.Entities.Passive;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace TestPlugin.Pets
{
	[Plugin(PluginName = "Pets", Description = "", PluginVersion = "1.0", Author = "MiNET Team")]
	public class PetsPlugin : Plugin
	{
		[Command]
		//public void Pet(Player player, string type)
		public void Pet(Player player, string type, params string[] name)
		{
			//TODO: Fix space in pets name, too difficult damn..

			PetTypes petType;
			try
			{
				petType = (PetTypes) Enum.Parse(typeof(PetTypes), type, true);
			}
			catch (ArgumentException e)
			{
				return;
			}

			if (!Enum.IsDefined(typeof(PetTypes), petType))
			{
				player.SendMessage("No pet found");
				return;
			}


			double height = 0.5;
			switch (petType)
			{
				case PetTypes.Chicken:
					height = new Chicken(null).Height;
					break;
				case PetTypes.Cow:
					height = new Cow(null).Height;
					break;
				case PetTypes.Pig:
					height = new Pig(null).Height;
					break;
				case PetTypes.Sheep:
					height = new Sheep(null).Height;
					break;
				case PetTypes.Wolf:
					height = new Wolf(null).Height;
					break;
				case PetTypes.Npc:
					break;
				case PetTypes.Mooshroom:
					break;
				case PetTypes.Squid:
					break;
				case PetTypes.Rabbit:
					break;
				case PetTypes.Bat:
					break;
				case PetTypes.IronGolem:
					break;
				case PetTypes.SnowGolem:
					break;
				case PetTypes.Ocelot:
					break;
				case PetTypes.Zombie:
					break;
				case PetTypes.Creeper:
					break;
				case PetTypes.ZombiePigman:
					break;
				case PetTypes.Enderman:
					break;
				case PetTypes.Blaze:
					break;
				case PetTypes.ZombieVillager:
					break;
				case PetTypes.Witch:
					break;
			}

			string petName = null;
			if (name.Length > 0)
			{
				petName = string.Join(" ", name);
			}

			var entities = player.Level.GetEntites();
			foreach (var entity in entities)
			{
				Pet pet = entity as Pet;
				if (pet != null && pet.Owner == player)
				{
					pet.HealthManager.Kill();
					break;
				}
			}

			Pet newPet = new Pet(player, player.Level)
			{
				NameTag = petName,
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
				Height = height,
			};
			newPet.SpawnEntity();
		}
	}
}