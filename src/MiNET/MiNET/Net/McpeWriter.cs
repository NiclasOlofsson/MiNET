using System;
using System.IO;
using System.Text;
using fNbt;
using log4net;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Net
{
    public class McpeWriter
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(McpeWriter));

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

        public void Write(Item stack, bool signItem = true)
        {
            if (stack == null || stack.Id <= 0)
            {
                Write((short)0);
                return;
            }

            Write(stack.Id);
            Write(stack.Count);
            Write(stack.Metadata);

            //NbtCompound extraData = stack.ExtraData;
            if (signItem)
            {
                stack = ItemSigner.DefaultItemSigner?.SignNbt(stack);
            }

            if (stack.ExtraData != null)
            {
                byte[] bytes = GetNbtData(stack.ExtraData);
                Write((byte)bytes.Length);
                Write((byte)0);
                Write(bytes);
            }
            else
            {
                Write((short)0);
            }
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