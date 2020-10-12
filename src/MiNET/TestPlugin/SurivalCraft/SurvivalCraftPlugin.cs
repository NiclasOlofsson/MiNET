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

using MiNET;
using MiNET.Events;
using MiNET.Events.Block;
using MiNET.Events.Level;
using MiNET.Events.Player;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.SurivalCraft
{
	[Plugin(PluginName = "SurvivalCraft", Description = "", PluginVersion = "1.0", Author = "MiNET Team")]
	public class SurvivalCraftPlugin : Plugin, IEventHandler
	{
		protected override void OnEnable()
		{
			var server = Context.Server;
			Context.EventDispatcher.RegisterEvents(this);
		}

		[EventHandler]
		private void LevelOnBlockBreak(BlockBreakEvent e)
		{
			if (e.Source is Player player)
			{
				if (e.Block.Coordinates.DistanceTo((BlockCoordinates) player.SpawnPosition) < 16 * 4)
				{
					e.SetCancelled(player.GameMode != GameMode.Creative);
				}
			}
		}
		
		[EventHandler]
		private void LevelOnBlockPlace(BlockPlaceEvent e)
		{
			if (e.Block.Coordinates.DistanceTo((BlockCoordinates) e.Player.SpawnPosition) < 16 * 4)
			{
				e.SetCancelled(e.Player.GameMode != GameMode.Creative);
			}
		}
	}
}