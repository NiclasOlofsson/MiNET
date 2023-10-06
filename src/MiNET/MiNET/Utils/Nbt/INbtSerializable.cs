using fNbt;

namespace MiNET.Utils.Nbt
{
	public interface INbtSerializable
	{
		public NbtCompound ToNbt(string name = null);
	}
}
