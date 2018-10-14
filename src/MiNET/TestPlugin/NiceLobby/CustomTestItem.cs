using MiNET.Items;

namespace TestPlugin.NiceLobby
{
	public class CustomTestItem : ItemApple
	{
		public uint SomeVariable { get; set; }

		public CustomTestItem(uint someVariable)
		{
			SomeVariable = someVariable;
			ExtraData = new fNbt.NbtCompound();
			ExtraData.Add(new fNbt.NbtInt("HashCode", GetHashCode()));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ SomeVariable.GetHashCode();
			}
		}
	}
}
