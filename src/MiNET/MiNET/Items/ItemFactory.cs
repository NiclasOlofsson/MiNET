using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(string id, short metadata, int count);
	}

	public class ItemFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFactory));

		public static ICustomItemFactory CustomItemFactory { get; set; }

		public static Dictionary<int, string> RuntimeIdToId { get; private set; }
		public static Dictionary<string, Type> IdToType { get; private set; } = new Dictionary<string, Type>();
		public static Dictionary<Type, string> TypeToId { get; private set; } = new Dictionary<Type, string>();
		public static Dictionary<string, Func<Item>> IdToFactory { get; private set; } = new Dictionary<string, Func<Item>>();
		public static Dictionary<string, string[]> ItemTags { get; private set; } = new Dictionary<string, string[]>();

		public static Itemstates Itemstates { get; internal set; } = new Itemstates();

		static ItemFactory()
		{
			ItemTags = ResourceUtil.ReadResource<Dictionary<string, string[]>>("item_tags.json", typeof(ItemFactory), "Data");
			Itemstates = ResourceUtil.ReadResource<Itemstates>("required_item_list.json", typeof(ItemFactory), "Data");

			var maxRuntimeId = Itemstates.Max(state => state.Value.RuntimeId);
			foreach (var blockId in BlockFactory.ItemToBlock.Values)
			{
				Itemstates.TryAdd(blockId, new Itemstate() { RuntimeId = ++maxRuntimeId });
			}

			RuntimeIdToId = BuildRuntimeIdToId();
			(IdToType, TypeToId) = BuildIdTypeMapPair();
			IdToFactory = BuildIdToFactory();
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

		public static Item FromNbt<T>(NbtTag tag) where T : Item
		{
			return (T) FromNbt(tag);
		}

		public static Item FromNbt(NbtTag tag)
		{
			// TODO - rework on serialization
			var id = tag["Name"].StringValue;
			var metadata = tag["Damage"].ShortValue;
			var count = tag["Count"].ByteValue;
			var extraData = tag["tag"] as NbtCompound;

			var item = GetItem(id, metadata, count);

			if (item == null)
			{
				//var blockTag = tag["Block"];

				//if (blockTag != null)
				//{
				//	var block = BlockFactory.FromNbt(blockTag);
				//	item = GetItem(block.Id, metadata, count);
				//}

				if (item == null) return null;
			}

			item.ExtraData = extraData;

			return item;
		}

		public static ItemBlock GetItem(Block block, int count = 1)
		{
			return GetItem<ItemBlock>(block.Id, block.GetGlobalState().Data, count) ?? GetItem<Air>();
		}

		public static ItemBlock GetItem<T>(short metadata = 0, int count = 1) where T : Block
		{
			return GetItem<ItemBlock>(BlockFactory.GetIdByType<T>(), metadata, count) ?? GetItem<Air>();
		}

		public static T GetItem<T>(int runtimeId, short metadata = 0, int count = 1) where T : Item
		{
			return (T) GetItem(runtimeId, metadata, count);
		}

		public static Item GetItem(int runtimeId, short metadata = 0, int count = 1)
		{
			return GetItem(GetIdByRuntimeId(runtimeId), metadata, count);
		}

		public static T GetItem<T>(string id, short metadata = 0, int count = 1) where T : Item
		{
			return (T) GetItem(id, metadata, count);
		}

		public static Item GetItem(string id, short metadata = 0, int count = 1)
		{
			if (CustomItemFactory != null)
			{
				var customItem = CustomItemFactory.GetItem(id, metadata, count);
				if (customItem != null) return customItem;
			}

			var item = GetItemInstance(id);

			if (item != null)
			{
				item.Metadata = metadata;
				item.Count = (byte) count;
			}
			else
			{
				var block = BlockFactory.GetBlockById(BlockFactory.GetBlockIdFromItemId(id), metadata)
					?? BlockFactory.GetBlockById(id, metadata);

				if (block != null)
				{
					item = new ItemBlock(block, metadata) { Count = (byte) count };
				}
			}

			return item ?? new ItemAir();
		}

		private static Item GetItemInstance(string id)
		{
			if (string.IsNullOrEmpty(id)) return null;

			return IdToFactory.GetValueOrDefault(id)?.Invoke();
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

		private static Dictionary<string, Func<Item>> BuildIdToFactory()
		{
			var idToFactory = new Dictionary<string, Func<Item>>();

			foreach (var pair in IdToType)
			{
				// faster then Activator.CreateInstance
				var constructorExpression = Expression.New(pair.Value);
				var lambdaExpression = Expression.Lambda<Func<Item>>(constructorExpression);
				var createFunc = lambdaExpression.Compile();

				idToFactory.Add(pair.Key, createFunc);
			}

			return idToFactory;
		}
	}
}