using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Threading;
using fNbt;
using log4net;
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Net
{
	public abstract partial class Package
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Package));

		private bool _isEncoded = false;
		private byte[] _encodedMessage;

		public int DatagramSequenceNumber = 0;

		public bool NoBatch { get; set; }

		public DateTime? ValidUntil { get; set; }

		public Reliability Reliability = Reliability.Unreliable;
		public int ReliableMessageNumber = 0;
		public byte OrderingChannel = 0;
		public int OrderingIndex = 0;

		public bool ForceClear = false;

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

		public void Write(bool value)
		{
			Write((byte) (value ? 1 : 0));
		}

		public bool ReadBool()
		{
			return _reader.ReadByte() != 0;
		}

		public void Write(byte[] value)
		{
			if (value == null)
			{
				Log.Warn("Trying to write null byte[]");
				return;
			}

			_writer.Write(value);
		}

		public byte[] ReadBytes(int count)
		{
			if (count == 0)
			{
				count = (int) (_reader.BaseStream.Length - _reader.BaseStream.Position);
			}

			var readBytes = _reader.ReadBytes(count);
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
			return readBytes;
		}

		public void WriteByteArray(byte[] value)
		{
			if (value == null)
			{
				Log.Warn("Trying to write null PrefixedArray");
				return;
			}

			WriteLenght(value.Length);
			_writer.Write(value, 0, value.Length);
		}

		public byte[] ReadByteArray()
		{
			var len = ReadLenght();
			var bytes = ReadBytes(len);
			return bytes;
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

		public void WriteVarInt(int value)
		{
			VarInt.WriteInt32(_buffer, value);
		}

		public int ReadVarInt()
		{
			return VarInt.ReadInt32(_buffer);
		}

		public void WriteSignedVarInt(int value)
		{
			VarInt.WriteSInt32(_buffer, value);
		}

		public int ReadSignedVarInt()
		{
			return VarInt.ReadSInt32(_buffer);
		}

		public void WriteUnsignedVarInt(uint value)
		{
			VarInt.WriteUInt32(_buffer, value);
		}

		public uint ReadUnsignedVarInt()
		{
			return VarInt.ReadUInt32(_buffer);
		}

		public int ReadLenght()
		{
			return (int) VarInt.ReadUInt32(_buffer);
		}

		public void WriteLenght(int value)
		{
			VarInt.WriteUInt32(_buffer, (uint) value);
		}

		public void WriteVarLong(long value)
		{
			VarInt.WriteInt64(_buffer, value);
		}

		public long ReadVarLong()
		{
			return VarInt.ReadInt64(_buffer);
		}

		public void Write(long value)
		{
			_writer.Write(Endian.SwapInt64(value));
		}

		public long ReadLong()
		{
			return Endian.SwapInt64(_reader.ReadInt64());
		}

		public void Write(float value)
		{
			byte[] bytes = BitConverter.GetBytes(value);

			_writer.Write(value);
			//_writer.Write(bytes[3]);
			//_writer.Write(bytes[2]);
			//_writer.Write(bytes[1]);
			//_writer.Write(bytes[0]);
		}

		public float ReadFloat()
		{
			//byte[] buffer = _reader.ReadBytes(4);
			//return BitConverter.ToSingle(new[] {buffer[3], buffer[2], buffer[1], buffer[0]}, 0);
			return _reader.ReadSingle();
		}

		public void Write(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				WriteLenght(0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			WriteLenght(bytes.Length);
			Write(bytes);
		}

		public string ReadString()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return string.Empty;
			int len = ReadLenght();
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public void WriteFixedString(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				Write((short) 0);
				return;
			}

			byte[] bytes = Encoding.UTF8.GetBytes(value);

			Write((short) bytes.Length);
			Write(bytes);
		}

		public string ReadFixedString()
		{
			if (_reader.BaseStream.Position == _reader.BaseStream.Length) return string.Empty;
			short len = ReadShort();
			if (len <= 0) return string.Empty;
			return Encoding.UTF8.GetString(ReadBytes(len));
		}

		public void Write(Vector3 vec)
		{
			Write((float) vec.X);
			Write((float) vec.Y);
			Write((float) vec.Z);
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
		}

		public void Write(BlockCoordinates coord)
		{
			WriteSignedVarInt(coord.X);
			WriteUnsignedVarInt((uint) coord.Y);
			WriteSignedVarInt(coord.Z);
		}

		public BlockCoordinates ReadBlockCoordinates()
		{
			return new BlockCoordinates(ReadSignedVarInt(), (int) ReadUnsignedVarInt(), ReadSignedVarInt());
		}

		public void Write(PlayerRecords records)
		{
			if (records is PlayerAddRecords)
			{
				Write((byte) 0);
				WriteUnsignedVarInt((uint) records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
					WriteVarLong(record.EntityId);
					Write(record.DisplayName ?? record.Username);
					Write(record.Skin);
				}
			}
			else if (records is PlayerRemoveRecords)
			{
				Write((byte) 1);
				WriteUnsignedVarInt((uint) records.Count);
				foreach (var record in records)
				{
					Write(record.ClientUuid);
				}
			}
		}

		public PlayerRecords ReadPlayerRecords()
		{
			// This should never be used in production. It is primarily for 
			// the client to work.
			byte recordType = ReadByte();
			uint count = ReadUnsignedVarInt();
			PlayerRecords records = null;
			switch (recordType)
			{
				case 0:
					records = new PlayerAddRecords();
					for (int i = 0; i < count; i++)
					{
						var player = new Player(null, null);
						try
						{
							player.ClientUuid = ReadUUID();
							player.EntityId = ReadVarLong();
							player.DisplayName = ReadString();
							player.Skin = ReadSkin();
							records.Add(player);
						}
						catch (Exception e)
						{
							Log.Error("Player List", e);
						}
					}
					break;
				case 1:
					records = new PlayerRemoveRecords();
					for (int i = 0; i < count; i++)
					{
						var player = new Player(null, null);
						player.ClientUuid = ReadUUID();
						records.Add(player);
					}
					break;
			}

			return records;
		}

		public void Write(Records records)
		{
			WriteUnsignedVarInt((uint) records.Count);
			foreach (BlockCoordinates coord in records)
			{
				WriteVarInt(coord.X);
				WriteVarInt(coord.Y);
				WriteVarInt(coord.Z);
			}
		}

		public Records ReadRecords()
		{
			return new Records();
		}

		public void Write(PlayerLocation location)
		{
			Write(location.X);
			Write(location.Y);
			Write(location.Z);
			Write((byte) (location.Pitch*0.71)); // 256/360
			Write((byte) (location.HeadYaw*0.71)); // 256/360
			Write((byte) (location.Yaw*0.71)); // 256/360
			Write((byte) 0); // Unknown
		}

		public PlayerLocation ReadPlayerLocation()
		{
			PlayerLocation location = new PlayerLocation();
			location.X = ReadFloat();
			location.Y = ReadFloat();
			location.Z = ReadFloat();
			location.Pitch = ReadByte()*1f/0.71f;
			location.HeadYaw = ReadByte()*1f/0.71f;
			location.Yaw = ReadByte()*1f/0.71f;
			ReadByte(); // Unknown

			return location;
		}

		public void Write(IPEndPoint endpoint)
		{
			if (endpoint.AddressFamily == AddressFamily.InterNetwork)
			{
				Write((byte) 4);
				var parts = endpoint.Address.ToString().Split('.');
				foreach (var part in parts)
				{
					Write((byte) byte.Parse(part));
				}
				Write((short) endpoint.Port);
			}
		}

		public IPEndPoint ReadIPEndPoint()
		{
			byte ipVersion = ReadByte();
			string ipAddress = $"{ReadByte()}.{ReadByte()}.{ReadByte()}.{ReadByte()}";
			int port = ReadShort();

			return new IPEndPoint(IPAddress.Parse(ipAddress), 19132);
		}

		public void Write(IPEndPoint[] endpoints)
		{
			foreach (var endpoint in endpoints)
			{
				Write(endpoint);
			}
		}

		public IPEndPoint[] ReadIPEndPoints(int count)
		{
			var endPoints = new IPEndPoint[count];
			for (int i = 0; i < endPoints.Length; i++)
			{
				endPoints[i] = ReadIPEndPoint();
			}

			return endPoints;
		}

		public void Write(UUID uuid)
		{
			if (uuid == null) throw new Exception("Expected UUID, required");
			Write(uuid.GetBytes());
		}

		public UUID ReadUUID()
		{
			UUID uuid = new UUID(ReadBytes(16));
			return uuid;
		}

		public void Write(Nbt nbt)
		{
			NbtFile file = nbt.NbtFile;
			file.BigEndian = false;
		    file.UseVarInt = true;

		    NbtCompound cm;
			Write(file.SaveToBuffer(NbtCompression.None));
		}

		public Nbt ReadNbt()
		{
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
            file.UseVarInt = true;
            nbt.NbtFile = file;
			file.LoadFromStream(_reader.BaseStream, NbtCompression.None);

			return nbt;
		}

		public void Write(MetadataInts metadata)
		{
			if (metadata == null)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(metadata.Count);

			for (byte i = 0; i < metadata.Count; i++)
			{
				MetadataInt slot = metadata[i] as MetadataInt;
				if (slot != null)
				{
					WriteSignedVarInt(slot.Value);
				}
			}
		}

		public MetadataInts ReadMetadataInts()
		{
			MetadataInts metadata = new MetadataInts();
			int count = ReadVarInt();

			for (byte i = 0; i < count; i++)
			{
				metadata[i] = new MetadataInt(ReadSignedVarInt());
			}

			return metadata;
		}

		public void Write(ItemStacks metadata)
		{
			McpeContainerSetContent msg = this as McpeContainerSetContent;
			bool signItems = msg == null || msg.windowId != 0x79;

			if (metadata == null)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(metadata.Count);

			for (int i = 0; i < metadata.Count; i++)
			{
				Write(metadata[i], signItems);
			}
		}

		public ItemStacks ReadItemStacks()
		{
			ItemStacks metadata = new ItemStacks();

			var count = ReadVarInt();

			for (int i = 0; i < count; i++)
			{
				metadata.Add(ReadItem());
			}

			return metadata;
		}

		public void Write(Item stack, bool signItem = true)
		{
			if (stack == null || stack.Id <= 0)
			{
				WriteSignedVarInt(0);
				return;
			}

			WriteSignedVarInt(stack.Id);
			WriteSignedVarInt((stack.Metadata << 8) + (stack.Count & 0xff));

			if (signItem)
			{
				stack = ItemSigner.DefaultItemSigner?.SignItem(stack);
			}

			if (stack.ExtraData != null)
			{
				byte[] bytes = GetNbtData(stack.ExtraData);
				_writer.Write((short) bytes.Length);
				Write(bytes);
			}
			else
			{
				Write((short) 0);
			}
		}

		public Item ReadItem()
		{
			int id = (int) ReadSignedVarInt();
			if (id <= 0)
			{
				return new ItemAir();
			}

			int tmp = (int) ReadSignedVarInt();
			short metadata = (short) (tmp >> 8);
			byte count = (byte) (tmp & 0xff);
			Item stack = ItemFactory.GetItem((short) id, metadata, count);

			//Log.Error($"Read Item={id}, Meta={metadata}, Count={count}, TMP={tmp}");

			int nbtLen = (int) _reader.ReadInt16(); // NbtLen
			//int nbtLen = (int) _reader.ReadSingle(); // NbtLen
			if (nbtLen > 0)
			{
				//Log.Error($"Read NBT lenght={nbtLen}");
				//_reader.ReadBytes(nbtLen);

				stack.ExtraData = ReadNbt().NbtFile.RootTag;
				//Log.Debug($"Read Item wiht NBT: {stack.ToString()}");
			}

			return stack;
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

		public MetadataDictionary ReadMetadataDictionary()
		{
			return MetadataDictionary.FromStream(_reader);
		}

		public void Write(PlayerAttributes attributes)
		{
			WriteUnsignedVarInt((uint) attributes.Count);
			foreach (PlayerAttribute attribute in attributes.Values)
			{
				Write(attribute.MinValue);
				Write(attribute.MaxValue);
				Write(attribute.Value);
				Write(attribute.Unknown); // unknown
				Write(attribute.Name);
			}
		}

		public PlayerAttributes ReadPlayerAttributes()
		{
			var attributes = new PlayerAttributes();
			uint count = ReadUnsignedVarInt();
			for (int i = 0; i < count; i++)
			{
				PlayerAttribute attribute = new PlayerAttribute
				{
					MinValue = ReadFloat(),
					MaxValue = ReadFloat(),
					Value = ReadFloat(),
					Unknown = ReadFloat(),
					Name = ReadString(),
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(EntityAttributes attributes)
		{
			if (attributes == null)
			{
				WriteVarInt(0);
				return;
			}

			WriteVarInt(attributes.Count);
			foreach (EntityAttribute attribute in attributes.Values)
			{
				Write(attribute.Name);
				Write(attribute.MinValue);
				Write(attribute.Value);
				Write(attribute.MaxValue);
			}
		}

		public EntityAttributes ReadEntityAttributes()
		{
			var attributes = new EntityAttributes();
			int count = ReadVarInt();
			for (int i = 0; i < count; i++)
			{
				EntityAttribute attribute = new EntityAttribute
				{
					Name = ReadString(),
					MinValue = ReadFloat(),
					Value = ReadFloat(),
					MaxValue = ReadFloat(),
				};

				attributes[attribute.Name] = attribute;
			}

			return attributes;
		}

		public void Write(Skin skin)
		{
			if (skin.Texture != null)
			{
				var skinType = skin.SkinType;
				if (string.IsNullOrEmpty(skinType)) skinType = "Standard_Custom";
				Write(skinType);
				WriteUnsignedVarInt((uint) skin.Texture.Length);
				Write(skin.Texture);
			}
		}

		public Skin ReadSkin()
		{
			Skin skin = new Skin();

			try
			{
				skin.SkinType = ReadString();
				if (string.IsNullOrEmpty(skin.SkinType)) skin.SkinType = null;

				if (skin.SkinType != null)
				{
					int length = (int) ReadUnsignedVarInt();
					if (length == 64*32*4 || length == 64*64*4)
					{
						skin.Texture = ReadBytes(length);
					}
					else
					{
						skin.SkinType = null;
					}
				}
			}
			catch (Exception e)
			{
				skin.SkinType = null;
				skin.Texture = null;
			}

			return skin;
		}

		public void Write(Recipes recipes)
		{
			WriteUnsignedVarInt((uint) recipes.Count);

			foreach (Recipe recipe in recipes)
			{
				if (recipe is ShapelessRecipe)
				{
					WriteSignedVarInt(0); // Type

					ShapelessRecipe rec = (ShapelessRecipe) recipe;
					WriteVarInt(rec.Input.Count);
					foreach (Item stack in rec.Input)
					{
						Write(stack);
					}
					WriteVarInt(1);
					Write(rec.Result);
					Write(new UUID(Guid.NewGuid()));
				}
				else if (recipe is ShapedRecipe)
				{
					WriteSignedVarInt(1); // Type

					ShapedRecipe rec = (ShapedRecipe) recipe;
					WriteSignedVarInt(rec.Width);
					WriteSignedVarInt(rec.Height);

					for (int w = 0; w < rec.Width; w++)
					{
						for (int h = 0; h < rec.Height; h++)
						{
							Write(rec.Input[(h*rec.Width) + w]);
						}
					}
					WriteVarInt(1);
					Write(rec.Result);
					Write(new UUID(Guid.NewGuid()));
				}
				else if (recipe is SmeltingRecipe)
				{
					SmeltingRecipe rec = (SmeltingRecipe) recipe;
					WriteSignedVarInt(rec.Input.Metadata == 0 ? 2 : 3); // Type
					WriteSignedVarInt(rec.Input.Id);
					if (rec.Input.Metadata != 0) WriteSignedVarInt(rec.Input.Metadata);
					Write(rec.Result);
				}
				else if (recipe is EnchantingRecipe)
				{
					//var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					//McpeWriter writer = new McpeWriter(memoryStream);

					//writer.Write((byte) 3); // Count
					//{
					//	writer.Write((int) 1); // Cost
					//	writer.Write((byte) 1); // Count
					//	writer.Write((int) 9); // Id
					//	writer.Write((int) 1); // Level
					//	writer.Write("Test1"); // Level
					//}

					//Write(4); // Type
					//var bytes = memoryStream.ToArray();
					//Write(bytes.Length);
					//Write(bytes);
				}
			}

			Write((byte) 1);
		}

		public Recipes ReadRecipes()
		{
			Recipes recipes = new Recipes();

			int count = (int) ReadUnsignedVarInt();

			Log.Error($"Reading {count} recipes");

			for (int i = 0; i < count; i++)
			{
				int recipeType = (int) ReadSignedVarInt();

				//Log.Error($"Read recipe no={i} type={recipeType}");

				if (recipeType < 0 /*|| len == 0*/)
				{
					Log.Error("Read void recipe");
					break;
				}

				if (recipeType == 0)
				{
					//const ENTRY_SHAPELESS = 0;
					ShapelessRecipe recipe = new ShapelessRecipe();
					int ingrediensCount = ReadVarInt(); // 
					for (int j = 0; j < ingrediensCount; j++)
					{
						recipe.Input.Add(ReadItem());
					}
					ReadVarInt(); // 1?
					recipe.Result = ReadItem();
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
					//Log.Error("Read shapeless recipe");
				}
				else if (recipeType == 1)
				{
					//const ENTRY_SHAPED = 1;
					int width = ReadSignedVarInt(); // Width
					int height = ReadSignedVarInt(); // Height
					ShapedRecipe recipe = new ShapedRecipe(width, height);
					if (width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
					for (int w = 0; w < width; w++)
					{
						for (int h = 0; h < height; h++)
						{
							recipe.Input[(h*width) + w] = ReadItem();
						}
					}

					int resultCount = ReadVarInt(); // 1?
					for (int j = 0; j < resultCount; j++)
					{
						recipe.Result = ReadItem();
					}
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
					//Log.Error("Read shaped recipe");
				}
				else if (recipeType == 2)
				{
					//const ENTRY_FURNACE = 2;
					SmeltingRecipe recipe = new SmeltingRecipe();
					//short meta = (short) ReadVarInt(); // input (with metadata) 
					short id = (short) ReadSignedVarInt(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, 0);
					recipe.Result = result;
					recipes.Add(recipe);
					//Log.Error("Read furnace recipe");
					//Log.Error($"Input={id}, meta={""} Item={result.Id}, Meta={result.Metadata}");
				}
				else if (recipeType == 3)
				{
					//const ENTRY_FURNACE_DATA = 3;
					SmeltingRecipe recipe = new SmeltingRecipe();
					short id = (short) ReadSignedVarInt(); // input (with metadata) 
					short meta = (short) ReadSignedVarInt(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, meta);
					recipe.Result = result;
					recipes.Add(recipe);
					//Log.Error("Read smelting recipe");
					//Log.Error($"Input={id}, meta={meta} Item={result.Id}, Meta={result.Metadata}");
				}
				else if (recipeType == 4)
				{
					//const ENTRY_ENCHANT_LIST = 4;
					Log.Error("Reading ENCHANT_LIST");

					ReadUUID();
				}
				else
				{
					Log.Error($"Read unknown recipe type: {recipeType}");
					//ReadBytes(len);
				}
			}

			ReadByte(); // Clean (1) or update (0)

			return recipes;
		}

		public void Write(MapInfo map)
		{
			Write(map.MapId);
			Write(new byte[3]);
			Write(map.UpdateType);
			Write(new byte[4]);
			Write((byte) 1);
			Write((byte) 0);
			Write(map.Direction);
			Write(map.X);
			Write(map.Z);
			if (map.UpdateType == 0x06)
			{
				// Full map
				Write(map.Col);
				Write(map.Row);
				Write(map.XOffset);
				Write(map.ZOffset);
				Write(map.Data);
			}
			else if (map.UpdateType == 0x04)
			{
				// Map update
			}
			else
			{
				Log.Warn($"Tried to send unknown map-type 0x{map.UpdateType:X2}");
			}
		}

		public MapInfo ReadMapInfo()
		{
			MapInfo map = new MapInfo();

			map.MapId = ReadLong();
			var readBytes = ReadBytes(3);
			//Log.Warn($"{HexDump(readBytes)}");
			map.UpdateType = ReadByte(); //
			var bytes = ReadBytes(6);
			//Log.Warn($"{HexDump(bytes)}");

			map.Direction = ReadByte(); //
			map.X = ReadByte(); //
			map.Z = ReadByte(); //

			if (map.UpdateType == 0x06)
			{
				// Full map
				try
				{
					if (bytes[4] == 1)
					{
						map.Col = ReadInt();
						map.Row = ReadInt(); //

						map.XOffset = ReadInt(); //
						map.ZOffset = ReadInt(); //

						map.Data = ReadBytes(map.Col*map.Row*4);
					}
				}
				catch (Exception e)
				{
					Log.Error($"Errror while reading map data for map={map}", e);
				}
			}
			else if (map.UpdateType == 0x04)
			{
				// Map update
			}
			else
			{
				Log.Warn($"Unknown map-type 0x{map.UpdateType:X2}");
			}

			return map;
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
			DatagramSequenceNumber = -1;

			Reliability = Reliability.Unreliable;
			ReliableMessageNumber = -1;
			OrderingChannel = 0;
			OrderingIndex = -1;

			NoBatch = false;
			ForceClear = false;

			ValidUntil = null;

			_encodedMessage = null;
			_writer.Flush();
			_buffer.SetLength(0);
			_buffer.Position = 0;
			_timer.Restart();
			_isEncoded = false;
		}

		public void SetEncodedMessage(byte[] encodedMessage)
		{
			_encodedMessage = encodedMessage;
			_isEncoded = true;
		}

		private object _encodeSync = new object();

		public virtual byte[] Encode()
		{
			lock (_encodeSync)
			{
				if (_isEncoded) return _encodedMessage;

				_isEncoded = false;

				EncodePackage();

				_writer.Flush();
				_buffer.Position = 0;
				_encodedMessage = _buffer.ToArray();
				_isEncoded = true;
				return _encodedMessage;
			}
		}

		protected virtual void DecodePackage()
		{
			_buffer.Position = 0;
			Id = ReadByte();
		}

		public virtual void Decode(byte[] buffer)
		{
			Bytes = buffer;
			_buffer.Position = 0;
			_buffer.SetLength(buffer.Length);
			_buffer.Write(buffer, 0, buffer.Length);
			_buffer.Position = 0;
			DecodePackage();
			if (Log.IsDebugEnabled && _buffer.Position != (buffer.Length))
			{
				Log.Warn($"{GetType().Name}: Still have {buffer.Length - _buffer.Position} bytes to read!!\n{HexDump(buffer)}");
			}
		}

		public byte[] Bytes { get; private set; }

		public void CloneReset()
		{
			_buffer = MiNetServer.MemoryStreamManager.GetStream();
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

		public static string HexDump(byte[] bytes, int bytesPerLine = 16, bool printLineCount = false)
		{
			StringBuilder sb = new StringBuilder();
			for (int line = 0; line < bytes.Length; line += bytesPerLine)
			{
				byte[] lineBytes = bytes.Skip(line).Take(bytesPerLine).ToArray();
				if (printLineCount) sb.AppendFormat("{0:x8} ", line);
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

		//static Package()
		//{
		//	for (int i = 0; i < 100; i++)
		//	{
		//		Pool.PutObject(Pool.GetObject());
		//	}
		//}


		~Package()
		{
			if (_isPooled)
			{
				//Log.Error($"Unexpected dispose 0x{Id:x2} {GetType().Name}, IsPooled={_isPooled}, IsPermanent={_isPermanent}, Refs={_referenceCounter}");
			}
		}

		public override void PutPool()
		{
			if (_isPermanent) return;
			if (!IsPooled) return;

			if (Interlocked.Decrement(ref _referenceCounter) > 0) return;

			if (_referenceCounter < 0)
			{
				Log.Error($"Pooling error. Added pooled object too many times. 0x{Id:x2} {GetType().Name}, IsPooled={IsPooled}, IsPooled={_isPermanent}, Refs={_referenceCounter}");
				return;
			}

			Reset();

			_isPooled = false;

			Pool.PutObject((T) this);
		}
	}
}