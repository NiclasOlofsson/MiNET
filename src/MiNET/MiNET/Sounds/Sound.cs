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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

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

		public virtual void SpawnToPlayers(Player[] players)
		{
			if (players == null) return;
			if (players.Length == 0) return;

			McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
			levelEvent.eventId = Id;
			levelEvent.data = Pitch;
			levelEvent.position = Position;
			levelEvent.AddReferences(players.Length - 1);

			foreach (var player in players)
			{
				player.SendPackage(levelEvent);
			}
		}
	}
}