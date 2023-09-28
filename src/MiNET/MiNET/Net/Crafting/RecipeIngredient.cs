using System.Linq;
using MiNET.Items;

namespace MiNET.Net.Crafting
{
	public abstract class RecipeIngredient : IPacketDataObject
	{
		public abstract RecipeIngredientType Type { get; }

		public int Count { get; set; }

		public void Write(Packet packet)
		{
			packet.Write((byte) Type);

			WriteData(packet);

			packet.WriteSignedVarInt(Count);
		}

		protected virtual void WriteData(Packet packet) { }

		public static RecipeIngredient Read(Packet packet)
		{
			var type = (RecipeIngredientType) packet.ReadByte();

			RecipeIngredient ingredient = type switch
			{
				RecipeIngredientType.StringIdMeta => RecipeItemIngredient.ReadData(packet),
				RecipeIngredientType.Tag => RecipeTagIngredient.ReadData(packet),
				_ => new RecipeAirIngredient()
			};

			ingredient.Count = packet.ReadSignedVarInt();

			return ingredient;
		}

		public abstract bool ValidateItem(Item item);
	}

	public class RecipeAirIngredient : RecipeIngredient
	{
		public override RecipeIngredientType Type => RecipeIngredientType.Air;

		public override bool ValidateItem(Item item)
		{
			return true;
		}

		public override string ToString()
		{
			return Count > 0 ? $"Air(count: {Count})" : "Air()";
		}
	}

	public class RecipeItemIngredient : RecipeIngredient
	{
		public override RecipeIngredientType Type => RecipeIngredientType.StringIdMeta;

		public string Id { get; set; }

		public short Metadata { get; set; }

		public RecipeItemIngredient(string id, short metadata, int count = 1)
		{
			Id = id;
			Metadata = metadata;
			Count = count;
		}

		protected RecipeItemIngredient()
		{ 
		
		}

		protected override void WriteData(Packet packet)
		{
			packet.Write(Id);
			packet.Write(Metadata);
		}

		internal static RecipeIngredient ReadData(Packet packet)
		{
			return new RecipeItemIngredient()
			{
				Id = packet.ReadString(),
				Metadata = packet.ReadShort()
			};
		}

		public override bool ValidateItem(Item item)
		{
			return item.Id == Id
				&& (item.Metadata == Metadata || Metadata == short.MaxValue)
				&& item.Count >= Count;
		}

		public override string ToString()
		{
			return $"Item(id: {Id}, metadata: {Metadata}, count: {Count})";
		}
	}

	public class RecipeTagIngredient : RecipeIngredient
	{
		public override RecipeIngredientType Type => RecipeIngredientType.Tag;

		public string Tag { get; set; }

		public RecipeTagIngredient(string tag, int count = 1)
		{
			Tag = tag;
			Count = count;
		}

		protected RecipeTagIngredient()
		{

		}

		protected override void WriteData(Packet packet)
		{
			packet.Write(Tag);
		}

		internal static RecipeIngredient ReadData(Packet packet)
		{
			return new RecipeTagIngredient()
			{
				Tag = packet.ReadString()
			};
		}

		public override bool ValidateItem(Item item)
		{
			return item.Count >= Count
				&& ItemFactory.ItemTags[Tag].Contains(item.Id);
		}

		public override string ToString()
		{
			return $"Tag(tag: {Tag} count: {Count})";
		}
	}
}
