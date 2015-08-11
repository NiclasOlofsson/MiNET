using System;
using MiNET;
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

			//ThreadPool.QueueUserWorkItem(delegate(object state)
			//{
			Pet newPet = new Pet(player, player.Level, (int) petType)
			{
				NameTag = petName,
				KnownPosition = (PlayerLocation) player.KnownPosition.Clone()
			};
			newPet.SpawnEntity();
			//});
		}
	}
}