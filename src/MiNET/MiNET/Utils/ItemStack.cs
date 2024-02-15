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

using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Net;

namespace MiNET.Utils
{
	public class ItemStacks : List<Item>, IPacketDataObject
	{
		public virtual void Write(Packet packet)
		{
			packet.WriteUnsignedVarInt((uint) Count);

			for (int i = 0; i < Count; i++)
			{
				packet.Write(this[i]);
			}
		}

		public static ItemStacks Read(Packet packet)
		{
			var itemStacks = new ItemStacks();

			var count = packet.ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				itemStacks.Add(packet.ReadItem());
			}

			return itemStacks;
		}
	}

	public class CreativeItemStacks : ItemStacks
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(CreativeItemStacks));

		public override void Write(Packet packet)
		{
			packet.WriteUnsignedVarInt((uint) Count);

			foreach (var item in this)
			{
				packet.WriteUnsignedVarInt((uint) item.UniqueId);
				packet.Write(item, false);
			}
		}

		public static new CreativeItemStacks Read(Packet packet)
		{
			var metadata = new CreativeItemStacks();

			var count = packet.ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				var networkId = packet.ReadUnsignedVarInt();
				var item = packet.ReadItem(false);
				item.UniqueId = (int) networkId;
				metadata.Add(item);
				Log.Debug(item);
			}

			return metadata;
		}
	}
}