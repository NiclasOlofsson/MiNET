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

		public int ReliableMessageNumber = 0;
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

		public void Write(bool value)
		{
			Write((byte) (value ? 1 : 0));
		}

		public bool ReadBool()
		{
			return _reader.ReadByte() != 0;
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
			if (readBytes.Length != count) throw new ArgumentOutOfRangeException($"Expected {count} bytes, only read {readBytes.Length}.");
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
			if (string.IsNullOrEmpty(value))
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
			Write(coord.X);
			Write(coord.Y);
			Write(coord.Z);
		}

		public BlockCoordinates ReadBlockCoordinates()
		{
			return new BlockCoordinates(ReadInt(), ReadInt(), ReadInt());
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
					Write(record.DisplayName ?? record.Username);
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
			// This should never be used in production. It is primarily for 
			// the client to work.
			byte recordType = ReadByte();
			int count = ReadInt();
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
							player.EntityId = ReadLong();
							player.NameTag = ReadString();
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
				block.Metadata = (byte) (metadata & 0xf);
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

		public IPEndPoint[] ReadIPEndPoints(int count)
		{
			var endPoints = new IPEndPoint[count];
			for (int i = 0; i < endPoints.Length; i++)
			{
				endPoints[i] = ReadIPEndPoint();
			}

			return endPoints;
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

		public void Write(ItemStacks metadata)
		{
			McpeContainerSetContent msg = this as McpeContainerSetContent;
			bool signItems = msg == null || msg.windowId != 0x79;

			if (metadata == null)
			{
				if (this is McpeCraftingEvent)
				{
					Write((int) 0);
				}
				else
				{
					Write((short) 0);
				}
				return;
			}

			if (this is McpeCraftingEvent)
			{
				Write((int) metadata.Count);
			}
			else
			{
				Write((short) metadata.Count);
			}


			for (int i = 0; i < metadata.Count; i++)
			{
				Write(metadata[i], signItems);
			}
		}

		public ItemStacks ReadItemStacks()
		{
			int count;
			if (this is McpeCraftingEvent)
			{
				// Misaligned array counters for some packets :-(
				count = ReadInt();
			}
			else
			{
				count = ReadShort();
			}

			ItemStacks metadata = new ItemStacks();

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
				Write((short) 0);
				return;
			}

			Write(stack.Id);
			Write(stack.Count);
			Write(stack.Metadata);

			if (signItem)
			{
				stack = ItemSigner.DefaultItemSigner?.SignItem(stack);
			}

			if (stack.ExtraData != null)
			{
				byte[] bytes = GetNbtData(stack.ExtraData);
				Write((byte) bytes.Length);
				Write((byte) 0);
				Write(bytes);
			}
			else
			{
				Write((short) 0);
			}
		}

		public Item ReadItem()
		{
			short id = ReadShort();
			if (id <= 0)
			{
				return new ItemAir();
			}

			byte count = ReadByte();
			short metadata = ReadShort();
			Item stack = ItemFactory.GetItem(id, metadata, count);

			int nbtLen = ReadShort(); // NbtLen
			if (nbtLen > 0)
			{
				stack.ExtraData = ReadNbt().NbtFile.RootTag;
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

			try
			{
				skin.SkinType = ReadString();
				if (string.IsNullOrEmpty(skin.SkinType)) skin.SkinType = null;

				if (skin.SkinType != null)
				{
					var length = ReadShort();
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

		public void Write(Skin skin)
		{
			if (skin.Texture != null)
			{
				var skinType = skin.SkinType;
				if (string.IsNullOrEmpty(skinType)) skinType = "Standard_Custom";
				Write(skinType);
				Write((short) skin.Texture.Length);
				Write(skin.Texture);
			}
		}

		public Recipes ReadRecipes()
		{
			Recipes recipes = new Recipes();

			int count = ReadInt();

			for (int i = 0; i < count; i++)
			{
				int recipeType = ReadInt();
				int len = ReadInt();
				if (recipeType < 0 || len == 0)
				{
					Log.Error("Read void recipe");
					continue;
				}

				if (recipeType == 0)
				{
					//const ENTRY_SHAPELESS = 0;
					ShapelessRecipe recipe = new ShapelessRecipe();
					int ingrediensCount = ReadInt(); // 
					for (int j = 0; j < ingrediensCount; j++)
					{
						recipe.Input.Add(ReadItem());
					}
					ReadInt(); // 1?
					recipe.Result = ReadItem();
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
				}
				else if (recipeType == 1)
				{
					//const ENTRY_SHAPED = 1;
					int width = ReadInt(); // Width
					int height = ReadInt(); // Height
					ShapedRecipe recipe = new ShapedRecipe(width, height);
					if (width > 3 || height > 3) throw new Exception("Wrong number of ingredience. Width=" + width + ", height=" + height);
					for (int w = 0; w < width; w++)
					{
						for (int h = 0; h < height; h++)
						{
							recipe.Input[(h*width) + w] = ReadItem();
						}
					}

					int resultCount = ReadInt(); // 1?
					for (int j = 0; j < resultCount; j++)
					{
						recipe.Result = ReadItem();
					}
					recipe.Id = ReadUUID(); // Id
					recipes.Add(recipe);
				}
				else if (recipeType == 2)
				{
					//const ENTRY_FURNACE = 2;
					SmeltingRecipe recipe = new SmeltingRecipe();
					short meta = ReadShort(); // input (with metadata) 
					short id = ReadShort(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, meta);
					recipe.Result = result;
					recipes.Add(recipe);
				}
				else if (recipeType == 3)
				{
					//const ENTRY_FURNACE_DATA = 3;
					SmeltingRecipe recipe = new SmeltingRecipe();
					short id = ReadShort(); // input (with metadata) 
					short meta = ReadShort(); // input (with metadata) 
					Item result = ReadItem(); // Result
					recipe.Input = ItemFactory.GetItem(id, meta);
					recipe.Result = result;
					recipes.Add(recipe);
				}
				else if (recipeType == 4)
				{
					Log.Error("Reading ENCHANT_LIST");
					//const ENTRY_ENCHANT_LIST = 4;
					int enchantmentListCount = ReadByte(); // count
					for (int j = 0; j < enchantmentListCount; j++)
					{
						ReadInt(); // Cost
						byte enchantmentCount = ReadByte(); // EnchantCount
						for (int k = 0; k < enchantmentCount; k++)
						{
							ReadInt(); // Id
							ReadInt(); // Level(strenght)
						}
						string name = ReadString(); // Name
						Log.Error("Enchant: " + name);
					}
				}
				else
				{
					Log.Error($"Read unknown recipe type: {recipeType}, lenght: {len}");
					ReadBytes(len);
				}
			}

			ReadByte(); // Clean (1) or update (0)

			return recipes;
		}

		public void Write(Recipes recipes)
		{
			Write(recipes.Count);

			foreach (Recipe recipe in recipes)
			{
				if (recipe is ShapelessRecipe)
				{
					var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					McpeWriter writer = new McpeWriter(memoryStream);

					ShapelessRecipe rec = (ShapelessRecipe) recipe;
					writer.Write(rec.Input.Count);
					foreach (Item stack in rec.Input)
					{
						writer.Write(stack);
					}
					writer.Write(1);
					writer.Write(rec.Result);
					writer.Write(new UUID(Guid.NewGuid()));

					Write(0); // Type
					var bytes = memoryStream.ToArray();
					Write(bytes.Length);
					Write(bytes);
				}
				else if (recipe is ShapedRecipe)
				{
					var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					McpeWriter writer = new McpeWriter(memoryStream);

					ShapedRecipe rec = (ShapedRecipe) recipe;
					writer.Write(rec.Width);
					writer.Write(rec.Height);

					for (int w = 0; w < rec.Width; w++)
					{
						for (int h = 0; h < rec.Height; h++)
						{
							writer.Write(rec.Input[(h*rec.Width) + w]);
						}
					}
					writer.Write(1);
					writer.Write(rec.Result);
					writer.Write(new UUID(Guid.NewGuid()));

					Write(1); // Type
					var bytes = memoryStream.ToArray();
					Write(bytes.Length);
					Write(bytes);
				}
				else if (recipe is SmeltingRecipe)
				{
					////const ENTRY_FURNACE = 2;
					//SmeltingRecipe recipe = new SmeltingRecipe();
					//short meta = ReadShort(); // input (with metadata) 
					//short id = ReadShort(); // input (with metadata) 
					//Item result = ReadItem(); // Result
					//recipe.Input = ItemFactory.GetItem(id, meta);
					//recipe.Result = result;
					//recipes.Add(recipe);
					var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					McpeWriter writer = new McpeWriter(memoryStream);

					SmeltingRecipe rec = (SmeltingRecipe) recipe;
					writer.Write(rec.Input.Metadata);
					writer.Write(rec.Input.Id);
					writer.Write(rec.Result);

					Write(rec.Input.Metadata == 0 ? 2 : 3); // Type
					var bytes = memoryStream.ToArray();
					Write(bytes.Length);
					Write(bytes);
				}
				else if (recipe is EnchantingRecipe)
				{
					var memoryStream = MiNetServer.MemoryStreamManager.GetStream();
					McpeWriter writer = new McpeWriter(memoryStream);

					writer.Write((byte) 3); // Count
					{
						writer.Write((int) 1); // Cost
						writer.Write((byte) 1); // Count
						writer.Write((int) 9); // Id
						writer.Write((int) 1); // Level
						writer.Write("Test1"); // Level
					}

					{
						writer.Write((int) 2); // Cost
						writer.Write((byte) 1); // Count
						writer.Write((int) 10); // Id
						writer.Write((int) 2); // Level
						writer.Write("Test2"); // Level
					}
					{
						writer.Write((int) 3); // Cost
						writer.Write((byte) 1); // Count
						writer.Write((int) 12); // Id
						writer.Write((int) 3); // Level
						writer.Write("Test3"); // Level
					}

					Write(4); // Type
					var bytes = memoryStream.ToArray();
					Write(bytes.Length);
					Write(bytes);
				}
			}

			Write((byte) 1);
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

		private bool _prependByte = false;

		public bool CanRead()
		{
			return _reader.BaseStream.Position < _reader.BaseStream.Length;
		}

		protected virtual void EncodePackage()
		{
			_buffer.Position = 0;
			if (_prependByte) Write((byte) 0x8e);
			Write(Id);
		}

		public virtual void Reset()
		{
			DatagramSequenceNumber = 0;
			OrderingIndex = 0;
			NoBatch = false;
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
			return Encode(false);
		}

		public virtual byte[] Encode(bool prependByte)
		{
			lock (_encodeSync)
			{
				if (_isEncoded && _prependByte != prependByte)
				{
					if (Log.IsDebugEnabled) Log.Debug("Doing uncessary encoding :-(");
				}

				if (_isEncoded && _prependByte == prependByte) return _encodedMessage;

				_isEncoded = false;
				_prependByte = prependByte;

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
			if (Id == 0x8e)
			{
				Id = ReadByte();
			}
		}

		public virtual void Decode(byte[] buffer)
		{
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

		//static Package()
		//{
		//	for (int i = 0; i < 100; i++)
		//	{
		//		Pool.PutObject(Pool.GetObject());
		//	}
		//}


		//~Package()
		//{
		//	//if(IsPooled)
		//	{
		//		Log.Warn($"Unexpected dispose 0x{Id:x2} {GetType().Name}, IsPooled={IsPooled}, Refs={_referenceCounter}");
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