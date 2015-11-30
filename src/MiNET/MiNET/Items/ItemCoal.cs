namespace MiNET.Items
{
	public class ItemCoal : Item
	{
		public ItemCoal(short metadata) : base(263, metadata)
		{
			MaxStackSize = 1;
			FuelEfficiency = 80;
		}
	}
}