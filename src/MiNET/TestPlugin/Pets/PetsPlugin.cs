using System;
using MiNET;
using MiNET.Entities.Passive;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using TestPlugin.Annotations;

namespace TestPlugin.Pets
{
	[Plugin(PluginName = "Pets", Description = "", PluginVersion = "1.0", Author = "MiNET Team"), UsedImplicitly]
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
				petType = (PetTypes) Enum.Parse(typeof (PetTypes), type, true);
			}
			catch (ArgumentException e)
			{
				return;
			}

			if (!Enum.IsDefined(typeof (PetTypes), petType))
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

			Pet newPet = new Pet(player, player.Level, (int) petType)
			{
				NameTag = petName, KnownPosition = (PlayerLocation) player.KnownPosition.Clone(),
				Height = height,
			};
			newPet.SpawnEntity();
		}
	}
}