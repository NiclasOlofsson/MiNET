using fNbt;
using log4net;
using MiNET.Items;

namespace MiNET.Net
{
	public class ItemSigner
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemSigner));

		public static ItemSigner DefaultItemSigner { get; set; }

		static ItemSigner()
		{
			DefaultItemSigner = new ItemSigner();
			//DefaultItemSigner = new HashedItemSigner();
		}

		public virtual Item SignItem(Item item)
		{
			return item;
		}

		public virtual bool VerifyItemStack(Player player, Item itemStack)
		{
			return true;
		}
	}
}