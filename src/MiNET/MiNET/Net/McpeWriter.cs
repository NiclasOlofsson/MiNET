using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Utils;

namespace MiNET.Net
{
	public class McpeWriter
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (McpeWriter));

		private BinaryWriter _writer;

		public McpeWriter(Stream stream)
		{
			_writer = new BinaryWriter(stream);
		}


		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public void Write(byte[] value)
		{
			if (value == null) return;

			_writer.Write(value);
		}

		public void Write(short value)
		{
			_writer.Write(Endian.SwapInt16(value));
		}

		public void Write(ushort value)
		{
			_writer.Write(Endian.SwapUInt16(value));
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public void Write(int value)
		{
			_writer.Write(Endian.SwapInt32(value));
		}

		public void Write(BinaryWriter writer, int value)
		{
			writer.Write(Endian.SwapInt32(value));
		}

		public void Write(uint value)
		{
			_writer.Write(Endian.SwapUInt32(value));
		}

		public void Write(long value)
		{
			_writer.Write(Endian.SwapInt64(value));
		}

		public void Write(ulong value)
		{
			_writer.Write(Endian.SwapUInt64(value));
		}

		public void Write(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			_writer.Write(bytes[3]);
			_writer.Write(bytes[2]);
			_writer.Write(bytes[1]);
			_writer.Write(bytes[0]);
		}

		public void Write(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Write((short)0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short)bytes.Length);
			Write(bytes);
		}

		public void Write(UUID uuid)
		{
			if (uuid == null) throw new Exception("Expected UUID, required");
			Write(uuid.GetBytes());
		}

		public void Write(Nbt nbt)
		{
			var file = nbt.NbtFile;
			file.BigEndian = false;

			Write(file.SaveToBuffer(NbtCompression.None));
		}

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				Write((short)0);
				return;
			}

			Write((short)metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					Write(slot.Value);
				}
			}
		}

		public void Write(MetadataSlots metadata)
		{
			if (metadata == null)
			{
				Write((short)0);
				return;
			}

			Write((short)metadata.Count);

			for (int i = 0; i < metadata.Count; i++)
			{
				//if (!metadata.Contains(i)) continue;

				MetadataSlot slot = metadata[i] as MetadataSlot;
				if (slot != null)
				{
					if (slot.Value.Id == 0)
					{
						Write((short)0);
						continue;
					}

					Write(slot.Value.Id);
					Write(slot.Value.Count);
					Write(slot.Value.Metadata);
					var extraData = slot.Value.ExtraData;
					extraData = SignNbt(extraData, true);

					if (extraData != null)
					{
						var bytes = GetNbtData(extraData);
						Write((short)bytes.Length);
						Write(bytes);
					}
					else
					{
						Write((short)0);
					}
				}
			}
		}

		public void Write(MetadataSlot slot)
		{
			if (slot == null || slot.Value.Id <= 0)
			{
				Write((short)0);
				return;
			}

			Write(slot.Value.Id);
			Write(slot.Value.Count);
			Write(slot.Value.Metadata);
			var extraData = slot.Value.ExtraData;
			extraData = SignNbt(extraData, true);

			if (extraData != null)
			{
				var bytes = GetNbtData(extraData);
				Write((short)bytes.Length);
				Write(bytes);
			}
			else
			{
				Write((short)0);
			}
		}

		public static NbtCompound SignNbt(NbtCompound extraData, bool crafting = false)
		{
			if (extraData == null)
			{
				extraData = new NbtCompound("");
			}

			lock (extraData)
			{
				//extraData = (NbtCompound)extraData.Clone();
				if (!extraData.Contains("Item"))
					extraData["Item"] =
						new NbtCompound("Item")
						{
							new NbtShort("_hash", 1234),
							new NbtByte("Crafting", (byte) (crafting?1:0))
						};
			}
			return extraData;
		}

		public static bool VerifyItemStack(Player player, ItemStack itemStack)
		{
			if (itemStack.Id == 0 && itemStack.Count == 0 && itemStack.Metadata == 0) return true;

			if (itemStack.ExtraData == null)
			{
				Log.Error($"{player.Username} Missing ExtraData on item with ID: {itemStack.Id}, Meta: {itemStack.Metadata}, Count: {itemStack.Count}");
				return false;
			}

			NbtCompound tag = itemStack.ExtraData["Item"] as NbtCompound;
			if (tag == null)
			{
				Log.Error($"{player.Username} Missing hash for ExtraData on item with ID: {itemStack.Id}, Meta: {itemStack.Metadata}, Count: {itemStack.Count}");
				return false;
			}

			NbtShort name = tag["_hash"] as NbtShort;
			if (name == null)
			{
				Log.Error($"{player.Username} Invalid hash for ExtraData on item with ID: {itemStack.Id}, Meta: {itemStack.Metadata}, Count: {itemStack.Count}");
				return false;
			}

			return true;
		}

		private byte[] GetNbtData(NbtCompound nbtCompound)
		{
			nbtCompound.Name = string.Empty;
			var file = new NbtFile(nbtCompound);
			file.BigEndian = false;

			return file.SaveToBuffer(NbtCompression.None);
		}

		public void Write(MetadataDictionary metadata)
		{
			if (metadata != null)
			{
				metadata.WriteTo(_writer);
			}
		}

		public void Write(PlayerAttributes attributes)
		{
			Write((short)attributes.Count);
			foreach (PlayerAttribute attribute in attributes.Values)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.Name);
			}
		}
	}
}