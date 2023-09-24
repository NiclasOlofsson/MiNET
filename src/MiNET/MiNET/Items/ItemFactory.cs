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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MiNET.Blocks;
using MiNET.Net.Items;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(short id, short metadata, int count);
	}

	public interface ICustomBlockItemFactory
	{
		ItemBlock GetBlockItem(Block block, short metadata, int count);
	}

	public class ItemFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFactory));

		public static ICustomItemFactory CustomItemFactory { get; set; }
		public static ICustomBlockItemFactory CustomBlockItemFactory { get; set; }

		public static Dictionary<int, string> RuntimeIdToId { get; private set; }
		public static Dictionary<string, Type> IdToType { get; private set; } = new Dictionary<string, Type>();
		public static Dictionary<Type, string> TypeToId { get; private set; } = new Dictionary<Type, string>();

		public static Itemstates Itemstates { get; internal set; } = new Itemstates();

		static ItemFactory()
		{
			Itemstates = ResourceUtil.ReadResource<Itemstates>("required_item_list.json", typeof(Item), "Data");

			RuntimeIdToId = BuildRuntimeIdToId();
			(IdToType, TypeToId) = BuildIdTypeMapPair();
		}

		[Obsolete]
		public static int GetItemIdByName(string itemName)
		{
			return 0;
		}

		public static string GetIdByType<T>()
		{
			return GetIdByType(typeof(T));
		}

		public static string GetIdByType(Type type)
		{
			return TypeToId.GetValueOrDefault(type);
		}

		public static int GetRuntimeIdById(string id)
		{
			return Itemstates.GetValueOrDefault(id)?.RuntimeId ?? 0;
		}

		public static string GetIdByRuntimeId(int id)
		{
			return RuntimeIdToId.GetValueOrDefault(id);
		}

		public static ItemBlock GetItem<T>(short metadata = 0, int count = 1) where T : Block
		{
			return GetItem<ItemBlock>(BlockFactory.GetIdByType<T>(), metadata, count);
		}

		public static Item GetItem(int runtimeId, short metadata = 0, int count = 1)
		{
			return GetItem<Item>(runtimeId, metadata, count);
		}

		public static T GetItem<T>(int runtimeId, short metadata = 0, int count = 1) where T : Item
		{
			return GetItem<T>(GetIdByRuntimeId(runtimeId), metadata, count);
		}

		public static Item GetItem(string id, short metadata = 0, int count = 1)
		{
			return GetItem<Item>(id, metadata, count);
		}

		public static T GetItem<T>(string id, short metadata = 0, int count = 1) where T : Item
		{
			T item;

			var block = BlockFactory.GetBlockById(id, metadata);

			if (block != null)
			{
				item = (T) (object) new ItemBlock(block, metadata) { Count = (byte) count };
			}
			else
			{
				item = GetItemInstance<T>(id);
				item.Metadata = metadata;
				item.Count = (byte) count;
			}

			return item;
		}

		private static T GetItemInstance<T>(string id) where T : Item
		{
			if (string.IsNullOrEmpty(id)) return null;

			var type = IdToType.GetValueOrDefault(id);

			if (type == null) return null;

			return (T) Activator.CreateInstance(type);
		}

		private static (Dictionary<string, Type>, Dictionary<Type, string>) BuildIdTypeMapPair()
		{
			var idToType = new Dictionary<string, Type>();
			var typeToId = new Dictionary<Type, string>();

			var itemTypes = typeof(ItemFactory).Assembly.GetTypes().Where(type => type.IsAssignableTo(typeof(Item)) && !type.IsAbstract);

			foreach (var type in itemTypes)
			{
				if (type == typeof(Item)) continue;

				try
				{
					var item = (Item) Activator.CreateInstance(type);

					if (string.IsNullOrEmpty(item.Id))
					{
						Log.Error($"Missing id for item [{type}]");
						continue;
					}

					idToType[item.Id] = type;
					typeToId[type] = item.Id;
				}
				catch
				{
					Log.Warn($"Unhandled item on mapping [{type}]");
				}
			}

			return (idToType, typeToId);
		}

		private static Dictionary<int, string> BuildRuntimeIdToId()
		{
			var runtimeIdToId = new Dictionary<int, string>();

			foreach (var state in Itemstates)
			{
				runtimeIdToId.Add(state.Value.RuntimeId, state.Key);
			}

			return runtimeIdToId;
		}
	}
}