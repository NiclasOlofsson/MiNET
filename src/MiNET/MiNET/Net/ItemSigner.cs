using fNbt;
using log4net;
using MiNET.Items;

namespace MiNET.Net
{
	public class ItemSigner
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemSigner));

		public static ItemSigner DefualtItemSigner { get; set; }

		static ItemSigner()
		{
			DefualtItemSigner = new ItemSigner();
		}

		public virtual NbtCompound SignNbt(NbtCompound extraData, bool crafting = false)
		{
			return extraData;
		}

		public virtual bool VerifyItemStack(Player player, Item itemStack)
		{
			return true;
		}
	}
}