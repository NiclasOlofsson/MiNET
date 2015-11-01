using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Utils;

namespace MiNET.Net
{
	public abstract partial class Package
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Package));

		protected object _bufferSync = new object();
		private bool _isEncoded = false;
		private byte[] _encodedMessage;

		public int DatagramSequenceNumber = 0;

		public int OrderingChannel = 0;
		public int OrderingIndex = 0;

		public byte Id;

		protected MemoryStream _buffer;
		private BinaryWriter _writer;
		private BinaryReader _reader;
		private Stopwatch _timer = new Stopwatch();

		public Package()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		public Stopwatch Timer
		{
			get { return _timer; }
		}

		public void Write(byte value)
		{
			_writer.Write(value);
		}

		public byte ReadByte()
		{
			return _reader.ReadByte();
		}

		public void Write(sbyte value)
		{
			_writer.Write(value);
		}

		public sbyte ReadSByte()
		{
			return _reader.ReadSByte();
		}

		public void Write(byte[] value)
		{
			if (value == null) return;

			_writer.Write(value);
		}

		public byte[] ReadBytes(int count)
		{
			if (count == 0)
			{
				count = (int) (_reader.BaseStream.Length - _reader.BaseStream.Position);
			}

			var readBytes = _reader.ReadBytes(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException();
			return readBytes;
		}

		public void Write(short value)
		{
			_writer.Write(Endian.SwapInt16(value));
		}

		public short ReadShort()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return 0;

			return Endian.SwapInt16(_reader.ReadInt16());
		}

		public void Write(ushort value)
		{
			_writer.Write(Endian.SwapUInt16(value));
		}

		public ushort ReadUShort()
		{
			return Endian.SwapUInt16(_reader.ReadUInt16());
		}

		public void Write(Int24 value)
		{
			_writer.Write(value.GetBytes());
		}

		public Int24 ReadLittle()
		{
			return new Int24(_reader.ReadBytes(3));
		}

		public void Write(int value)
		{
			_writer.Write(Endian.SwapInt32(value));
		}

		public int ReadInt()
		{
			return Endian.SwapInt32(_reader.ReadInt32());
		}

		public void Write(uint value)
		{
			_writer.Write(Endian.SwapUInt32(value));
		}

		public uint ReadUInt()
		{
			return Endian.SwapUInt32(_reader.ReadUInt32());
		}

		public void Write(long value)
		{
			_writer.Write(Endian.SwapInt64(value));
		}

		public long ReadLong()
		{
			return Endian.SwapInt64(_reader.ReadInt64());
		}

		public void Write(ulong value)
		{
			_writer.Write(Endian.SwapUInt64(value));
		}

		public ulong ReadULong()
		{
			return Endian.SwapUInt64(_reader.ReadUInt64());
		}

		public void Write(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			_writer.Write(bytes[3]);
			_writer.Write(bytes[2]);
			_writer.Write(bytes[1]);
			_writer.Write(bytes[0]);
		}

		public float ReadFloat()
		{
			byte[] buffer = _reader.ReadBytes(4);
			return BitConverter.ToSingle(new[] {buffer[3], buffer[2], buffer[1], buffer[0]}, 0);
		}

		public void Write(string value)
		{
			if (value == null)
			{
				Write((short) 0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short) bytes.Length);
			Write(bytes);
		}

		public string ReadString()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return "";
			short len = ReadShort();
			if (len == 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public void Write(PlayerRecords records)
		{
			if (records is PlayerAddRecords)
			{
				Write((byte) 0);
				Write(records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
					Write(record.EntityId);
					Write(record.Username);
					Write(record.Skin);
				}
			}
			else if (records is PlayerRemoveRecords)
			{
				Write((byte) 1);
				Write(records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
				}
			}
		}

		public PlayerRecords ReadPlayerRecords()
		{
			return null;
		}

		public void Write(Records records)
		{
			Write(records.Count);
			foreach (BlockCoordinates coord in records)
			{
				Write((byte) coord.X);
				Write((byte) coord.Y);
				Write((byte) coord.Z);
			}
		}

		public Records ReadRecords()
		{
			return new Records();
		}

		public void Write(BlockRecords records)
		{
			Write((int) records.Count);
			foreach (Block block in records)
			{
				Write(block.Coordinates.X);
				Write(block.Coordinates.Z);
				Write((byte) block.Coordinates.Y);
				Write((byte) block.Id);
				Write((byte) block.Metadata); //TODO: Shift in the block flags
			}
		}

		public BlockRecords ReadBlockRecords()
		{
			int count = ReadInt();
			var blockRecords = new BlockRecords();
			for (int i = 0; i < count; i++)
			{
				var x = ReadInt();
				var z = ReadInt();
				var y = ReadByte();
				var id = ReadByte();
				var metadata = ReadByte();

				Block block = BlockFactory.GetBlockById(id);
				block.Metadata = metadata;
				block.Coordinates = new BlockCoordinates(x, y, z);
				blockRecords.Add(block);
			}

			return blockRecords;
		}

		public void Write(EntityLocations locations)
		{
			Write(locations.Count);
			foreach (var location in locations)
			{
				Write(location.Key); // Entity ID
				Write(location.Value.X);
				Write(location.Value.Y);
				Write(location.Value.Z);
				Write(location.Value.Yaw);
				Write(location.Value.HeadYaw);
				Write(location.Value.Pitch);
			}
		}

		public EntityLocations ReadEntityLocations()
		{
			return new EntityLocations();
		}

		public void Write(EntityHeadRotations locations)
		{
			Write(locations.Count);
			foreach (var location in locations)
			{
				Write(location.Key); // Entity ID
				Write((byte) (location.Value.HeadYaw*0.71)); // 256/360
			}
		}

		public EntityHeadRotations ReadEntityHeadRotations()
		{
			return new EntityHeadRotations();
		}

		public void Write(EntityMotions motions)
		{
			Write(motions.Count);
			foreach (var motion in motions)
			{
				Write((long) motion.Key); // Entity ID
				Write((float) (motion.Value.X));
				Write((float) (motion.Value.Y));
				Write((float) (motion.Value.Z));
			}
		}

		public IPEndPoint[] ReadIPEndPoints()
		{
			return new IPEndPoint[0];
		}


		public void Write(IPEndPoint[] endpoints)
		{
			foreach (var endpoint in endpoints)
			{
				Write(endpoint);
			}
		}

		public UUID ReadUUID()
		{
			UUID uuid = new UUID(ReadBytes(16));
			return uuid;
		}

		public void Write(UUID uuid)
		{
			if (uuid == null) throw new Exception("Expected UUID, required");
			Write(uuid.GetBytes());
		}

		public void Write(IPEndPoint endpoint)
		{
			if (endpoint.AddressFamily == AddressFamily.InterNetwork)
			{
				Write((byte) 4);
				var parts = endpoint.Address.ToString().Split('.');
				foreach (var part in parts)
				{
					Write((byte) ~byte.Parse(part));
				}
				Write((short) endpoint.Port);
			}
		}

		public IPEndPoint ReadIPEndPoint()
		{
			return new IPEndPoint(IPAddress.Loopback, 19132);
		}

		public EntityMotions ReadEntityMotions()
		{
			return new EntityMotions();
		}

		public void Write(Nbt nbt)
		{
			var file = nbt.NbtFile;
			file.BigEndian = false;

			Write(file.SaveToBuffer(NbtCompression.None));
		}

		public Nbt ReadNbt()
		{
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			nbt.NbtFile = file;
			file.LoadFromStream(_reader.BaseStream, NbtCompression.None);

			return nbt;
		}

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				Write((short) 0);
				return;
			}

			Write((short) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					Write(slot.Value);
				}
			}
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			short count = ReadShort();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt(ReadInt());
			}

			return metadata;
		}

		public void Write(MetadataSlots metadata)
		{
			if (metadata == null)
			{
				Write((short) 0);
				return;
			}

			Write((short) metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				if (!metadata.Contains(i)) continue;

				MetadataSlot slot = metadata[i] as MetadataSlot;
				if (slot != null)
				{
					if (slot.Value.Id == 0)
					{
						Write((short) 0);
						continue;
					}

					Write(slot.Value.Id);
					Write(slot.Value.Count);
					Write(slot.Value.Metadata);
					Write((short) 0); // NBT Len
				}
			}
		}

		public MetadataSlots ReadMetadataSlots()
		{
			short count = ReadShort();

			MetadataSlots metadata = new MetadataSlots();

			for (int i = 0; i < count; i++)
			{
				short id = ReadShort();
				if (id <= 0)
				{
					metadata[i] = new MetadataSlot(new ItemStack());
					continue;
				}

				metadata[i] = new MetadataSlot(new ItemStack(id, ReadByte(), ReadShort()));
				int nbtLen = ReadShort(); // NbtLen
				if (nbtLen > 0)
				{
					ReadBytes(nbtLen); // Slurp
				}
			}

			return metadata;
		}

		public void Write(MetadataSlot slot)
		{
			if (slot == null || slot.Value.Id == 0)
			{
				Write((short) 0);
				return;
			}

			Write(slot.Value.Id);
			Write(slot.Value.Count);
			Write(slot.Value.Metadata);
			Write((short) 0);
		}

		public MetadataSlot ReadMetadataSlot()
		{
			short id = ReadShort();
			if (id == 0) return new MetadataSlot(new ItemStack());

			MetadataSlot metadataSlot = new MetadataSlot(new ItemStack(id, ReadByte(), ReadShort()));
			ReadShort(); // Nbt len
			return metadataSlot;
		}


		public MetadataDictionary ReadMetadataDictionary()
		{
			return MetadataDictionary.FromStream(_reader);
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
			Write((short) attributes.Count);
			foreach (PlayerAttribute attribute in attributes.Values)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.Name);
			}
		}

		public PlayerAttributes ReadPlayerAttributes()
		{
			var attributes = new PlayerAttributes();
			short count = ReadShort();
			for (int i = 0; i < count; i++)
			{
				PlayerAttribute attribute = new PlayerAttribute
				{
					MinValue = ReadFloat(),
					MaxValue = ReadFloat(),
					Value = ReadFloat(),
					Name = ReadString()
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();
			skin.Slim = ReadByte() == 0x01;
			try
			{
				skin.Texture = ReadBytes(ReadShort());
			}
			catch (Exception e)
			{
			}

			return skin;
		}

		public void Write(Skin skin)
		{
			Write((byte) (skin.Slim ? 0x01 : 0x00));
			if (skin.Texture != null)
			{
				Write((short) skin.Texture.Length);
				Write(skin.Texture);
			}
		}

		public Recipes ReadRecipes()
		{
			Recipes recipes = new Recipes();

			int count = ReadInt();
			Log.InfoFormat("Recipes Count: {0}", count);

			for (int i = 0; i < count; i++)
			{
				int recipeType = ReadInt();
				int len = ReadInt();
				if (recipeType < 0 || len == 0)
				{
					Log.Warn("Read void recipe");
					return recipes;
				}

				if (recipeType == 0)
				{
					//const ENTRY_SHAPELESS = 0;
					ShapelessRecipe recipe = new ShapelessRecipe();
					int ingrediensCount = ReadInt(); // 
					for (int j = 0; j < ingrediensCount; j++)
					{
						recipe.Input.Add(ReadMetadataSlot().Value);
					}
					ReadInt(); // 1?
					recipe.Result = ReadMetadataSlot().Value;
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
				}
				else if (recipeType == 1)
				{
					//const ENTRY_SHAPED = 1;
					int width = ReadInt(); // Width
					int height = ReadInt(); // Height
					ShapedRecipe recipe = new ShapedRecipe(width, height);
					//if(width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
					for (int w = 0; w < width; w++)
					{
						for (int h = 0; h < height; h++)
						{
							recipe.Input[(h*width) + w] = ReadMetadataSlot().Value.Item;
						}
					}

					ReadInt(); // 1?
					recipe.Result = ReadMetadataSlot().Value;
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
				}
				else if (recipeType == 2)
				{
					//const ENTRY_FURNACE = 2;
					ReadInt(); // type
					ReadInt(); // input (with metadata) 
					ReadMetadataSlot(); // Result
				}
				else if (recipeType == 3)
				{
					//const ENTRY_FURNACE_DATA = 3;
					ReadInt(); // input 
					ReadMetadataSlot(); // Result
				}
				else if (recipeType == 4)
				{
					//const ENTRY_ENCHANT_LIST = 4;
					int enchantmentListCount = ReadByte(); // count
					for (int j = 0; j < enchantmentListCount; j++)
					{
						ReadMetadataSlot();
						ReadInt(); // Cost
						byte enchantmentCount = ReadByte(); // EnchantCount
						for (int k = 0; k < enchantmentCount; k++)
						{
							ReadInt(); // Id
							ReadInt(); // Level(strenght)
						}
						ReadString(); // Name
					}
				}
			}

			return recipes;
		}

		public void Write(Recipes recipes)
		{
		}

		public bool CanRead()
		{
			return _reader.BaseStream.Position < _reader.BaseStream.Length;
		}

		protected virtual void EncodePackage()
		{
			_buffer.Position = 0;
			Write(Id);
		}

		public virtual void Reset()
		{
			DatagramSequenceNumber = 0;
			OrderingIndex = 0;
			_encodedMessage = null;
			_writer.Flush();
			_buffer.SetLength(0);
			_buffer.Position = 0;
			_timer.Reset();
			_isEncoded = false;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
			_isEncoded = true;
		}

		public virtual byte[] Encode()
		{
			if (_isEncoded) return _encodedMessage;

			EncodePackage();
			_writer.Flush();
			_buffer.Position = 0;
			_encodedMessage = _buffer.ToArray();
			_isEncoded = true;
			return _encodedMessage;
		}

		protected virtual void DecodePackage()
		{
			_buffer.Position = 0;
			Id = ReadByte();
		}

		public virtual void Decode(byte[] buffer)
		{
			_buffer.Position = 0;
			_buffer.SetLength(buffer.Length);
			_buffer.Write(buffer, 0, buffer.Length);
			_buffer.Position = 0;
			DecodePackage();
		}

		public void CloneReset()
		{
			_buffer = new MemoryStream();
			_reader = new BinaryReader(_buffer);
			_writer = new BinaryWriter(_buffer);
			Timer.Start();
		}

		public virtual object Clone()
		{
			Package clone = (Package) MemberwiseClone();
			clone.CloneReset();
			return clone;
		}

		public virtual T Clone<T>() where T : Package
		{
			return (T) Clone();
		}

		public abstract void PutPool();

		public static string HexDump(byte[] bytes, int bytesPerLine = 16)
		{
			StringBuilder sb = new StringBuilder();
			for (int line = 0; line < bytes.Length; line += bytesPerLine)
			{
				byte[] lineBytes = bytes.Skip(line).Take(bytesPerLine).ToArray();
				sb.AppendFormat("{0:x8} ", line);
				sb.Append(string.Join(" ", lineBytes.Select(b => b.ToString("x2"))
					.ToArray()).PadRight(bytesPerLine*3));
				sb.Append(" ");
				sb.Append(new string(lineBytes.Select(b => b < 32 ? '.' : (char) b)
					.ToArray()));
				sb.AppendLine();
			}
			return sb.ToString();
		}
	}

	/// Base package class
	public abstract partial class Package<T> : Package, ICloneable where T : Package<T>, new()
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Package));

		private static readonly ObjectPool<T> Pool = new ObjectPool<T>(() => new T());

		private bool _isPermanent;
		private bool _isPooled;
		private long _referenceCounter;

		public bool IsPooled
		{
			get { return _isPooled; }
		}

		public long ReferenceCounter
		{
			get { return _referenceCounter; }
			set { _referenceCounter = value; }
		}


		public T MarkPermanent(bool permanent = true)
		{
			if (!_isPooled) throw new Exception("Tried to make non pooled item permanent");
			_isPermanent = permanent;

			return (T) this;
		}

		public T AddReferences(long numberOfReferences)
		{
			if (_isPermanent) return (T) this;

			if (!_isPooled) throw new Exception("Tried to referenc count a non pooled item");
			Interlocked.Add(ref _referenceCounter, numberOfReferences);

			return (T) this;
		}

		public T AddReference(Package<T> item)
		{
			if (_isPermanent) return (T) this;

			if (!item.IsPooled) throw new Exception("Item template needs to come from a pool");

			Interlocked.Increment(ref item._referenceCounter);
			return (T) item;
		}

		public T MakePoolable(long numberOfReferences = 1)
		{
			_isPooled = true;
			_referenceCounter = numberOfReferences;
			return (T) this;
		}


		public static T CreateObject(long numberOfReferences = 1)
		{
			var item = Pool.GetObject();
			item._isPooled = true;
			item._referenceCounter = numberOfReferences;
			item.Timer.Restart();
			return item;
		}

		static Package()
		{
			for (int i = 0; i < 100; i++)
			{
				Pool.PutObject(Pool.GetObject());
			}
		}


		//~Package()
		//{
		//	//if(IsPooled)
		//	{
		//		Log.Warn(string.Format("Unexpected dispose 0x{0:x2} IsPooled={1}, Refs={2}, Source={3}", Id, IsPooled, _referenceCounter, Source));
		//	}
		//}

		public override void PutPool()
		{
			if (_isPermanent) return;
			if (!IsPooled) return;

			if (Interlocked.Decrement(ref _referenceCounter) > 0) return;

			Reset();
			Pool.PutObject((T) this);
		}
	}
}