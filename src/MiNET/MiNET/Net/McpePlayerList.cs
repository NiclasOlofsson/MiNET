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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MiNET.Net
{
	public partial class McpePlayerList
	{
		/// <summary>
		///     Serializes one Add record as three wire fragments: the fields before the skin
		///     (led by the record's variant tag so a roster is pure concatenation), the serialized
		///     skin alone (shared between players via the content-addressed store), and the fields
		///     after it. Concatenated they are byte-identical to what <see cref="EncodePacket" />
		///     writes for the same entry, which is what the round-trip test pins.
		/// </summary>
		public static (byte[] prefix, byte[] skin, byte[] suffix) EncodeAddRecordSlices(PlayerListAddEntry entry)
		{
			var writer = new McpePlayerList();
			using var scratch = new MemoryStream();
			writer.BeginFragmentEncode(scratch);
			try
			{
				writer.WriteUnsignedVarInt(1); // variant tag: PlayerListAddEntry
				writer.Write((byte) entry.action);
				writer.Write(entry.uuid);
				writer.WriteSignedVarLong(entry.actorUniqueId);
				writer.Write(entry.playerName);
				writer.Write(entry.xblXuid);
				writer.Write(entry.playfabId);
				writer.Write(entry.platformOnlineId);
				writer.Write((int) entry.buildPlatform);
				byte[] prefix = scratch.ToArray();

				scratch.SetLength(0);
				writer.Write(entry.serializedSkin);
				byte[] skin = scratch.ToArray();

				scratch.SetLength(0);
				writer.Write(entry.isTeacher);
				writer.Write(entry.isHost);
				writer.Write(entry.isSubclient);
				writer.Write(entry.playerColor);
				byte[] suffix = scratch.ToArray();

				return (prefix, skin, suffix);
			}
			finally
			{
				writer.EndFragmentEncode();
			}
		}

		/// <summary>The roster entry that announces a player, built from the player itself.</summary>
		public static PlayerListAddEntry AddEntry(Player player)
		{
			return new PlayerListAddEntry
			{
				action = PlayerListAddEntry.Action.Add,
				uuid = player.ClientUuid,
				actorUniqueId = player.EntityId,
				playerName = player.DisplayName ?? player.Username,
				xblXuid = player.PlayerInfo.CertificateData?.ExtraData?.Xuid ?? string.Empty,
				playfabId = player.Skin?.PlayFabId ?? string.Empty,
				platformOnlineId = player.PlayerInfo.PlatformChatId,
				buildPlatform = (PlayerListAddEntry.BuildPlatform) player.PlayerInfo.DeviceOS,
				serializedSkin = player.Skin,
				isTeacher = false,
				isHost = false,
				isSubclient = false,
				playerColor = player.PlayerListColor
			};
		}

		/// <summary>The roster entry that takes a player off the list. Nothing but the uuid is read.</summary>
		public static PlayerListRemoveEntry RemoveEntry(Player player)
		{
			return new PlayerListRemoveEntry
			{
				action = PlayerListRemoveEntry.Action.Remove,
				uuid = player.ClientUuid
			};
		}

		public static List<PlayerListBase> Added(params Player[] players)
		{
			return players.Select(player => (PlayerListBase) AddEntry(player)).ToList();
		}

		public static List<PlayerListBase> Added(IEnumerable<Player> players)
		{
			return players.Select(player => (PlayerListBase) AddEntry(player)).ToList();
		}

		public static List<PlayerListBase> Removed(params Player[] players)
		{
			return players.Select(player => (PlayerListBase) RemoveEntry(player)).ToList();
		}

		public static List<PlayerListBase> Removed(IEnumerable<Player> players)
		{
			return players.Select(player => (PlayerListBase) RemoveEntry(player)).ToList();
		}
	}
}
