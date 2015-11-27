using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using Craft.Net.Common;
using Craft.Net.Classic.Common;
using System.Text;

namespace Craft.Net.Anvil
{
    /// <summary>
    /// Represents a 32x32 area of <see cref="Chunk"/> objects.
    /// Not all of these chunks are represented at any given time, and
    /// will be loaded from disk or generated when the need arises.
    /// </summary>
    public class Region : IDisposable
    {
        // In chunks
        public const int Width = 32, Depth = 32;

        /// <summary>
        /// The currently loaded chunk list.
        /// </summary>
        public Dictionary<Coordinates2D, Chunk> Chunks { get; set; }
        /// <summary>
        /// The location of this region in the overworld.
        /// </summary>
        public Coordinates2D Position { get; set; }

        public World World { get; set; }

        private Stream regionFile { get; set; }

        /// <summary>
        /// Creates a new Region for server-side use at the given position using
        /// the provided terrain generator.
        /// </summary>
        public Region(Coordinates2D position, World world)
        {
            Chunks = new Dictionary<Coordinates2D, Chunk>();
            Position = position;
            World = world;
        }

        /// <summary>
        /// Creates a region from the given region file.
        /// </summary>
        public Region(Coordinates2D position, World world, string file) : this(position, world)
        {
            if (File.Exists(file))
                regionFile = File.Open(file, FileMode.OpenOrCreate);
            else
            {
                regionFile = File.Open(file, FileMode.OpenOrCreate);
                CreateRegionHeader();
            }
        }

        /// <summary>
        /// Retrieves the requested chunk from the region, or
        /// generates it if a world generator is provided.
        /// </summary>
        /// <param name="position">The position of the requested local chunk coordinates.</param>
        public Chunk GetChunk(Coordinates2D position)
        {
            // TODO: This could use some refactoring
            lock (Chunks)
            {
                if (!Chunks.ContainsKey(position))
                {
                    if (regionFile != null)
                    {
                        // Search the stream for that region
                        lock (regionFile)
                        {
                            var chunkData = GetChunkFromTable(position);
                            if (chunkData == null)
                            {
                                if (World.WorldGenerator == null)
                                    throw new ArgumentException("The requested chunk is not loaded.", "position");
                                GenerateChunk(position);
                                return Chunks[position];
                            }
                            regionFile.Seek(chunkData.Item1, SeekOrigin.Begin);
                            /*int length = */new MinecraftStream(regionFile).ReadInt32(); // TODO: Avoid making new objects here, and in the WriteInt32
                            int compressionMode = regionFile.ReadByte();
                            switch (compressionMode)
                            {
                                case 1: // gzip
                                    break;
                                case 2: // zlib
                                    var nbt = new NbtFile();
                                    nbt.LoadFromStream(regionFile, NbtCompression.ZLib, null);
                                    var chunk = Chunk.FromNbt(nbt);
                                    Chunks.Add(position, chunk);
                                    break;
                                default:
                                    throw new InvalidDataException("Invalid compression scheme provided by region file.");
                            }
                        }
                    }
                    else if (World.WorldGenerator == null)
                        throw new ArgumentException("The requested chunk is not loaded.", "position");
                    else
                        GenerateChunk(position);
                }
                return Chunks[position];
            }
        }

        /// <summary>
        /// Retrieves the requested chunk from the region, without using the
        /// world generator if it does not exist.
        /// </summary>
        /// <param name="position">The position of the requested local chunk coordinates.</param>
        public Chunk GetChunkWithoutGeneration(Coordinates2D position)
        {
            // TODO: This could use some refactoring
            lock (Chunks)
            {
                if (!Chunks.ContainsKey(position))
                {
                    if (regionFile != null)
                    {
                        // Search the stream for that region
                        lock (regionFile)
                        {
                            var chunkData = GetChunkFromTable(position);
                            if (chunkData == null)
                                return null;
                            regionFile.Seek(chunkData.Item1, SeekOrigin.Begin);
                            /*int length = */new MinecraftStream(regionFile).ReadInt32(); // TODO: Avoid making new objects here, and in the WriteInt32
                            int compressionMode = regionFile.ReadByte();
                            switch (compressionMode)
                            {
                                case 1: // gzip
                                    break;
                                case 2: // zlib
                                    var nbt = new NbtFile();
                                    nbt.LoadFromStream(regionFile, NbtCompression.ZLib, null);
                                    var chunk = Chunk.FromNbt(nbt);
                                    Chunks.Add(position, chunk);
                                    break;
                                default:
                                    throw new InvalidDataException("Invalid compression scheme provided by region file.");
                            }
                        }
                    }
                    else if (World.WorldGenerator == null)
                        throw new ArgumentException("The requested chunk is not loaded.", "position");
                    else
                        GenerateChunk(position);
                }
                return Chunks[position];
            }
        }

        public void GenerateChunk(Coordinates2D position)
        {
            var globalPosition = (Position * new Coordinates2D(Width, Depth)) + position;
            var chunk = World.WorldGenerator.GenerateChunk(globalPosition);
            chunk.IsModified = true;
            chunk.X = globalPosition.X;
            chunk.Z = globalPosition.Z;
            Chunks.Add(position, chunk);
        }

        /// <summary>
        /// Sets the chunk at the specified local position to the given value.
        /// </summary>
        public void SetChunk(Coordinates2D position, Chunk chunk)
        {
            if (!Chunks.ContainsKey(position))
                Chunks.Add(position, chunk);
            chunk.IsModified = true;
            chunk.X = position.X;
            chunk.Z = position.Z;
            chunk.LastAccessed = DateTime.Now;
            Chunks[position] = chunk;
        }

        /// <summary>
        /// Saves this region to the specified file.
        /// </summary>
        public void Save(string file)
        {
            if(File.Exists(file))
                regionFile = regionFile ?? File.Open(file, FileMode.OpenOrCreate);
            else
            {
                regionFile = regionFile ?? File.Open(file, FileMode.OpenOrCreate);
                CreateRegionHeader();
            }
            Save();
        }

        /// <summary>
        /// Saves this region to the open region file.
        /// </summary>
        public void Save()
        {
            lock (Chunks)
            {
                lock (regionFile)
                {
                    var toRemove = new List<Coordinates2D>();
                    foreach (var kvp in Chunks)
                    {
                        var chunk = kvp.Value;
                        if (chunk.IsModified)
                        {
                            var data = chunk.ToNbt();
                            byte[] raw = data.SaveToBuffer(NbtCompression.ZLib);

                            var header = GetChunkFromTable(kvp.Key);
                            if (header == null || header.Item2 > raw.Length)
                                header = AllocateNewChunks(kvp.Key, raw.Length);

                            regionFile.Seek(header.Item1, SeekOrigin.Begin);
                            new MinecraftStream(regionFile).WriteInt32(raw.Length);
                            regionFile.WriteByte(2); // Compressed with zlib
                            regionFile.Write(raw, 0, raw.Length);

                            chunk.IsModified = false;
                        }
                        if ((DateTime.Now - chunk.LastAccessed).TotalMinutes > 5)
                            toRemove.Add(kvp.Key);
                    }
                    regionFile.Flush();
                    // Unload idle chunks
                    foreach (var chunk in toRemove)
                        Chunks.Remove(chunk);
                }
            }
        }

        #region Stream Helpers

        private const int ChunkSizeMultiplier = 4096;
        private Tuple<int, int> GetChunkFromTable(Coordinates2D position) // <offset, length>
        {
            int tableOffset = ((position.X % Width) + (position.Z % Depth) * Width) * 4;
            regionFile.Seek(tableOffset, SeekOrigin.Begin);
            byte[] offsetBuffer = new byte[4];
            regionFile.Read(offsetBuffer, 0, 3);
            Array.Reverse(offsetBuffer);
            int length = regionFile.ReadByte();
            int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
            if (offset == 0 || length == 0)
                return null;
            return new Tuple<int, int>(offset,
                length * ChunkSizeMultiplier);
        }

        private void CreateRegionHeader()
        {
            regionFile.Write(new byte[8192], 0, 8192);
            regionFile.Flush();
        }

        private Tuple<int, int> AllocateNewChunks(Coordinates2D position, int length)
        {
            // Expand region file
            regionFile.Seek(0, SeekOrigin.End);
            int dataOffset = (int)regionFile.Position;

            length /= ChunkSizeMultiplier;
            length++;
            regionFile.Write(new byte[length * ChunkSizeMultiplier], 0, length * ChunkSizeMultiplier);

            // Write table entry
            int tableOffset = ((position.X % Width) + (position.Z % Depth) * Width) * 4;
            regionFile.Seek(tableOffset, SeekOrigin.Begin);

            byte[] entry = BitConverter.GetBytes(dataOffset >> 4);
            entry[0] = (byte)length;
            Array.Reverse(entry);
            regionFile.Write(entry, 0, entry.Length);

            return new Tuple<int, int>(dataOffset, length * ChunkSizeMultiplier);
        }

        #endregion

        public static string GetRegionFileName(Coordinates2D position)
        {
            return string.Format("r.{0}.{1}.mca", position.X, position.Z);
        }

        public void UnloadChunk(Coordinates2D position)
        {
            Chunks.Remove(position);
        }

        public void Dispose()
        {
            if (regionFile == null)
                return;
            lock (regionFile)
            {
                regionFile.Flush();
                regionFile.Close();
            }
        }
    }
}

namespace Craft.Net.Classic.Common
{
    public partial class MinecraftStream : Stream
    {
        public Stream BaseStream { get; set; }
        public Encoding StringEncoding { get; set; }

        public MinecraftStream(Stream baseStream)
        {
            BaseStream = baseStream;
            StringEncoding = Encoding.BigEndianUnicode;
        }

        public override bool CanRead { get { return BaseStream.CanRead; } }

        public override bool CanSeek { get { return BaseStream.CanSeek; } }

        public override bool CanWrite { get { return BaseStream.CanWrite; } }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override long Length
        {
            get { return BaseStream.Length; }
        }

        public override long Position
        {
            get { return BaseStream.Position; }
            set { BaseStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return BaseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return BaseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            BaseStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            BaseStream.Write(buffer, offset, count);
        }

        public byte ReadUInt8()
        {
            int value = BaseStream.ReadByte();
            if (value == -1)
                throw new EndOfStreamException();
            return (byte)value;
        }

        public void WriteUInt8(byte value)
        {
            WriteByte(value);
        }

        public sbyte ReadInt8()
        {
            return (sbyte)ReadUInt8();
        }

        public void WriteInt8(sbyte value)
        {
            WriteUInt8((byte)value);
        }

        public ushort ReadUInt16()
        {
            return (ushort)(
                (ReadUInt8() << 8) |
                ReadUInt8());
        }

        public void WriteUInt16(ushort value)
        {
            Write(new[]
                {
                    (byte)((value & 0xFF00) >> 8),
                    (byte)(value & 0xFF)
                }, 0, 2);
        }

        public short ReadInt16()
        {
            return (short)ReadUInt16();
        }

        public void WriteInt16(short value)
        {
            WriteUInt16((ushort)value);
        }

        public uint ReadUInt32()
        {
            return (uint)(
                (ReadUInt8() << 24) |
                (ReadUInt8() << 16) |
                (ReadUInt8() << 8) |
                ReadUInt8());
        }

        public void WriteUInt32(uint value)
        {
            Write(new[]
                {
                    (byte)((value & 0xFF000000) >> 24),
                    (byte)((value & 0xFF0000) >> 16),
                    (byte)((value & 0xFF00) >> 8),
                    (byte)(value & 0xFF)
                }, 0, 4);
        }

        public int ReadInt32()
        {
            return (int)ReadUInt32();
        }

        public void WriteInt32(int value)
        {
            WriteUInt32((uint)value);
        }

        public ulong ReadUInt64()
        {
            return unchecked(
                ((ulong)ReadUInt8() << 56) |
                ((ulong)ReadUInt8() << 48) |
                ((ulong)ReadUInt8() << 40) |
                ((ulong)ReadUInt8() << 32) |
                ((ulong)ReadUInt8() << 24) |
                ((ulong)ReadUInt8() << 16) |
                ((ulong)ReadUInt8() << 8) |
                (ulong)ReadUInt8());
        }

        public void WriteUInt64(ulong value)
        {
            Write(new[]
                {
                    (byte)((value & 0xFF00000000000000) >> 56),
                    (byte)((value & 0xFF000000000000) >> 48),
                    (byte)((value & 0xFF0000000000) >> 40),
                    (byte)((value & 0xFF00000000) >> 32),
                    (byte)((value & 0xFF000000) >> 24),
                    (byte)((value & 0xFF0000) >> 16),
                    (byte)((value & 0xFF00) >> 8),
                    (byte)(value & 0xFF)
                }, 0, 8);
        }

        public long ReadInt64()
        {
            return (long)ReadUInt64();
        }

        public void WriteInt64(long value)
        {
            WriteUInt64((ulong)value);
        }

        public byte[] ReadUInt8Array(int length)
        {
            var result = new byte[length];
            if (length == 0) return result;
            int n = length;
            while (true)
            {
                n -= Read(result, length - n, n);
                if (n == 0)
                    break;
                System.Threading.Thread.Sleep(1);
            }
            return result;
        }

        public void WriteUInt8Array(byte[] value)
        {
            Write(value, 0, value.Length);
        }

        public void WriteUInt8Array(byte[] value, int offset, int count)
        {
            Write(value, offset, count);
        }

        public sbyte[] ReadInt8Array(int length)
        {
            return (sbyte[])(Array)ReadUInt8Array(length);
        }

        public void WriteInt8Array(sbyte[] value)
        {
            Write((byte[])(Array)value, 0, value.Length);
        }

        public ushort[] ReadUInt16Array(int length)
        {
            var result = new ushort[length];
            if (length == 0) return result;
            for (int i = 0; i < length; i++)
                result[i] = ReadUInt16();
            return result;
        }

        public void WriteUInt16Array(ushort[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt16(value[i]);
        }

        public short[] ReadInt16Array(int length)
        {
            return (short[])(Array)ReadUInt16Array(length);
        }

        public void WriteInt16Array(short[] value)
        {
            WriteUInt16Array((ushort[])(Array)value);
        }

        public uint[] ReadUInt32Array(int length)
        {
            var result = new uint[length];
            if (length == 0) return result;
            for (int i = 0; i < length; i++)
                result[i] = ReadUInt32();
            return result;
        }

        public void WriteUInt32Array(uint[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt32(value[i]);
        }

        public int[] ReadInt32Array(int length)
        {
            return (int[])(Array)ReadUInt32Array(length);
        }

        public void WriteInt32Array(int[] value)
        {
            WriteUInt32Array((uint[])(Array)value);
        }

        public ulong[] ReadUInt64Array(int length)
        {
            var result = new ulong[length];
            if (length == 0) return result;
            for (int i = 0; i < length; i++)
                result[i] = ReadUInt64();
            return result;
        }

        public void WriteUInt64Array(ulong[] value)
        {
            for (int i = 0; i < value.Length; i++)
                WriteUInt64(value[i]);
        }

        public long[] ReadInt64Array(int length)
        {
            return (long[])(Array)ReadUInt64Array(length);
        }

        public void WriteInt64Array(long[] value)
        {
            WriteUInt64Array((ulong[])(Array)value);
        }

        public bool ReadBoolean()
        {
            return ReadUInt8() != 0;
        }

        public void WriteBoolean(bool value)
        {
            WriteUInt8(value ? (byte)1 : (byte)0);
        }

        public string ReadString()
        {
            short length = ReadInt16();
            if (length == 0) return string.Empty;
            var data = ReadUInt8Array(length * 2);
            return StringEncoding.GetString(data);
        }

        public void WriteString(string value)
        {
            WriteInt16((short)value.Length);
            if (value.Length > 0)
                WriteUInt8Array(StringEncoding.GetBytes(value));
        }

        // TODO: string8 is modified UTF-8, which has a special representation of the NULL character
        public string ReadString8()
        {
            short length = ReadInt16();
            if (length == 0) return string.Empty;
            var data = ReadUInt8Array((int)length);
            return Encoding.UTF8.GetString(data);
        }

        public void WriteString8(string value)
        {
            WriteInt16((short)value.Length);
            if (value.Length > 0)
                WriteUInt8Array(Encoding.UTF8.GetBytes(value));
        }
    }
}