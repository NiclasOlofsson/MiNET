using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Reflection;
using fNbt;
using fNbt.Serialization;
using Craft.Net.Common;

namespace Craft.Net.Common
{
    public enum Biome
    {
        Ocean = 0,
        Plains = 1,
        Desert = 2,
        ExtremeHills = 3,
        Forest = 4,
        Taiga = 5,
        Swampland = 6,
        River = 7,
        Hell = 8,
        Sky = 9,
        FrozenOcean = 10,
        FrozenRiver = 11,
        IcePlains = 12,
        IceMountains = 13,
        MushroomIsland = 14,
        MushroomIslandShore = 15,
        Beach = 16,
        DesertHills = 17,
        ForestHills = 18,
        TaigaHills = 19,
        ExtremeHillsEdge = 20,
        Jungle = 21,
        JungleHills = 22,
    }
}

namespace Craft.Net.Anvil
{


    public class Chunk : INbtSerializable
    {
        #region Entity Stuff

        private static NbtSerializer Serializer { get; set; }
        private static Dictionary<string, Type> EntityTypes { get; set; }

        public static void RegisterEntityType(string id, Type type)
        {
            if (typeof(IDiskEntity).IsAssignableFrom(type))
                throw new ArgumentException("Specified type does not implement IDiskEntity", "type");
            EntityTypes[id.ToUpper()] = type;
        }

        static Chunk()
        {
            EntityTypes = new Dictionary<string, Type>();
            Serializer = new NbtSerializer(typeof(Chunk));
        }

        #endregion

        public const int Width = 16, Height = 256, Depth = 16;

        [NbtIgnore]
        internal DateTime LastAccessed { get; set; }

        public bool IsModified { get; set; }

        public byte[] Biomes { get; set; }

        public int[] HeightMap { get; set; }

        [NbtIgnore]
        public Section[] Sections { get; set; }

        [TagName("TileEntities")]
        private TileEntity[] _TileEntities
        {
            get { return TileEntities.ToArray(); }
            set { TileEntities = new List<TileEntity>(value); }
        }
        [NbtIgnore]
        public List<TileEntity> TileEntities { get; set; }

        [TagName("Entities")]
        private IDiskEntity[] _Entities
        {
            get { return Entities.ToArray(); }
            set { Entities = new List<IDiskEntity>(value); }
        }
        [NbtIgnore]
        public List<IDiskEntity> Entities { get; set; }

        [TagName("xPos")]
        public int X { get; set; }

        [TagName("zPos")]
        public int Z { get; set; }

        public long LastUpdate { get; set; }

        public bool TerrainPopulated { get; set; }

        [NbtIgnore]
        public Region ParentRegion { get; set; }

        public Chunk()
        {
            TerrainPopulated = true;
            TileEntities = new List<TileEntity>();
            Entities = new List<IDiskEntity>();
            Sections = new Section[16];
            for (int i = 0; i < Sections.Length; i++)
                Sections[i] = new Section((byte)i);
            Biomes = new byte[Width * Depth];
            HeightMap = new int[Width * Depth];
            LastAccessed = DateTime.Now;
        }

        public Chunk(Coordinates2D coordinates) : this()
        {
            X = coordinates.X;
            Z = coordinates.Z;
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetBlockId(coordinates);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            return Sections[section].GetBlockLight(coordinates);
        }

        public void SetBlockId(Coordinates3D coordinates, short value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetBlockId(coordinates, value);
            var oldHeight = GetHeight((byte)coordinates.X, (byte)coordinates.Z);
            if (value == 0) // Air
            {
                if (oldHeight <= coordinates.Y)
                {
                    // Shift height downwards
                    while (coordinates.Y > 0)
                    {
                        coordinates.Y--;
                        if (GetBlockId(coordinates) != 0)
                            SetHeight((byte)coordinates.X, (byte)coordinates.Z, coordinates.Y);
                    }
                }
            }
            else
            {
                if (oldHeight < coordinates.Y)
                    SetHeight((byte)coordinates.X, (byte)coordinates.Z, coordinates.Y);
            }
        }

        public void SetMetadata(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetMetadata(coordinates, value);
        }

        public void SetSkyLight(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetSkyLight(coordinates, value);
        }

        public void SetBlockLight(Coordinates3D coordinates, byte value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            int section = GetSectionNumber(coordinates.Y);
            coordinates.Y = GetPositionInSection(coordinates.Y);
            Sections[section].SetBlockLight(coordinates, value);
        }

        public TileEntity GetTileEntity(Coordinates3D coordinates)
        {
            LastAccessed = DateTime.Now;
            for (int i = 0; i < TileEntities.Count; i++)
                if (TileEntities[i].Coordinates == coordinates)
                    return TileEntities[i];
            return null;
        }

        public void SetTileEntity(Coordinates3D coordinates, TileEntity value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            for (int i = 0; i < TileEntities.Count; i++)
            {
                if (TileEntities[i].Coordinates == coordinates)
                {
                    TileEntities[i] = value;
                    return;
                }
            }
            TileEntities.Add(value);
        }

        private static int GetSectionNumber(int yPos)
        {
             return yPos / 16;
        }

        private static int GetPositionInSection(int yPos)
        {
            return yPos % 16;
        }

        /// <summary>
        /// Gets the biome at the given column.
        /// </summary>
        public Biome GetBiome(byte x, byte z)
        {
            LastAccessed = DateTime.Now;
            return (Biome)Biomes[(byte)(z * Depth) + x];
        }

        /// <summary>
        /// Sets the value of the biome at the given column.
        /// </summary>
        public void SetBiome(byte x, byte z, Biome value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            Biomes[(byte)(z * Depth) + x] = (byte)value;
            IsModified = true;
        }

        /// <summary>
        /// Gets the height of the specified column.
        /// </summary>
        public int GetHeight(byte x, byte z)
        {
            LastAccessed = DateTime.Now;
            return HeightMap[(byte)(z * Depth) + x];
        }

        private void SetHeight(byte x, byte z, int value)
        {
            LastAccessed = DateTime.Now;
            IsModified = true;
            HeightMap[(byte)(z * Depth) + x] = value;
        }

        public NbtFile ToNbt()
        {
            LastAccessed = DateTime.Now;
            var serializer = new NbtSerializer(typeof(Chunk));
            var compound = serializer.Serialize(this, "Level") as NbtCompound;
            var file = new NbtFile();
            file.RootTag.Add(compound);
            return file;
        }

        public static Chunk FromNbt(NbtFile nbt)
        {
            var serializer = new NbtSerializer(typeof(Chunk));
            var chunk = (Chunk)serializer.Deserialize(nbt.RootTag["Level"]);
            return chunk;
        }

        public NbtTag Serialize(string tagName)
        {
            var chunk = (NbtCompound)Serializer.Serialize(this, tagName, true);
            var entities = new NbtList("Entities", NbtTagType.Compound);
            for (int i = 0; i < Entities.Count; i++)
                entities.Add(Entities[i].Serialize(string.Empty));
            chunk.Add(entities);
            var sections = new NbtList("Sections", NbtTagType.Compound);
            var serializer = new NbtSerializer(typeof(Section));
            for (int i = 0; i < Sections.Length; i++)
            {
                if (!Sections[i].IsAir)
                    sections.Add(serializer.Serialize(Sections[i]));
            }
            chunk.Add(sections);
            return chunk;
        }

        public void Deserialize(NbtTag value)
        {
            IsModified = true;
            var compound = value as NbtCompound;
            var chunk = (Chunk)Serializer.Deserialize(value, true);

            this._TileEntities = chunk._TileEntities;
            this.Biomes = chunk.Biomes;
            this.HeightMap = chunk.HeightMap;
            this.LastUpdate = chunk.LastUpdate;
            this.Sections = chunk.Sections;
            this.TerrainPopulated = chunk.TerrainPopulated;
            this.X = chunk.X;
            this.Z = chunk.Z;

            // Entities
            var entities = compound["Entities"] as NbtList;
            Entities = new List<IDiskEntity>();
            for (int i = 0; i < entities.Count; i++)
            {
                var id = entities[i]["id"].StringValue;
                IDiskEntity entity;
                if (EntityTypes.ContainsKey(id.ToUpper()))
                    entity = (IDiskEntity)Activator.CreateInstance(EntityTypes[id]);
                else
                    entity = new UnrecognizedEntity(id);
                entity.Deserialize(entities[i]);
                Entities.Add(entity);
            }
            var serializer = new NbtSerializer(typeof(Section));
            foreach (var section in compound["Sections"] as NbtList)
            {
                int index = section["Y"].IntValue;
                Sections[index] = (Section)serializer.Deserialize(section);
                Sections[index].ProcessSection();
            }
        }
    }
}

namespace fNbt.Serialization
{
    public class NbtSerializer
    {
        public Type Type { get; set; }

        /// <summary>
        /// Decorates the given property or field with the specified
        /// NBT tag name.
        /// </summary>
        public NbtSerializer(Type type)
        {
            Type = type;
        }

        public NbtTag Serialize(object value, bool skipInterfaceCheck = false)
        {
            return Serialize(value, "", skipInterfaceCheck);
        }

        public NbtTag Serialize(object value, string tagName, bool skipInterfaceCheck = false)
        {
            if (!skipInterfaceCheck && value is INbtSerializable)
                return ((INbtSerializable)value).Serialize(tagName);
            else if (value is NbtTag)
                return (NbtTag)value;
            else if (value is byte)
                return new NbtByte(tagName, (byte)value);
            else if (value is sbyte)
                return new NbtByte(tagName, (byte)(sbyte)value);
            else if (value is bool)
                return new NbtByte(tagName, (byte)((bool)value ? 1 : 0));
            else if (value is byte[])
                return new NbtByteArray(tagName, (byte[])value);
            else if (value is double)
                return new NbtDouble(tagName, (double)value);
            else if (value is float)
                return new NbtFloat(tagName, (float)value);
            else if (value is int)
                return new NbtInt(tagName, (int)value);
            else if (value is uint)
                return new NbtInt(tagName, (int)(uint)value);
            else if (value is int[])
                return new NbtIntArray(tagName, (int[])value);
            else if (value is long)
                return new NbtLong(tagName, (long)value);
            else if (value is ulong)
                return new NbtLong(tagName, (long)(ulong)value);
            else if (value is short)
                return new NbtShort(tagName, (short)value);
            else if (value is ushort)
                return new NbtShort(tagName, (short)(ushort)value);
            else if (value is string)
                return new NbtString(tagName, (string)value);
            else if (value.GetType().IsArray)
            {
                var elementType = value.GetType().GetElementType();
                var array = value as Array;
                var listType = NbtTagType.Compound;
                if (elementType == typeof(byte) || elementType == typeof(sbyte))
                    listType = NbtTagType.Byte;
                else if (elementType == typeof(bool))
                    listType = NbtTagType.Byte;
                else if (elementType == typeof(byte[]))
                    listType = NbtTagType.ByteArray;
                else if (elementType == typeof(double))
                    listType = NbtTagType.Double;
                else if (elementType == typeof(float))
                    listType = NbtTagType.Float;
                else if (elementType == typeof(int) || elementType == typeof(uint))
                    listType = NbtTagType.Int;
                else if (elementType == typeof(int[]))
                    listType = NbtTagType.IntArray;
                else if (elementType == typeof(long) || elementType == typeof(ulong))
                    listType = NbtTagType.Long;
                else if (elementType == typeof(short) || elementType == typeof(ushort))
                    listType = NbtTagType.Short;
                else if (elementType == typeof(string))
                    listType = NbtTagType.String;
                var list = new NbtList(tagName, listType);
                var innerSerializer = new NbtSerializer(elementType);
                for (int i = 0; i < array.Length; i++)
                    list.Add(innerSerializer.Serialize(array.GetValue(i)));
                return list;
            }
            else if (value is NbtFile)
                return ((NbtFile)value).RootTag;
            else
            {
                var compound = new NbtCompound(tagName);

                if (value == null) return compound;
                var nameAttributes = Attribute.GetCustomAttributes(value.GetType(), typeof(TagNameAttribute));

                if (nameAttributes.Length > 0)
                    compound = new NbtCompound(((TagNameAttribute)nameAttributes[0]).Name);

                var properties = Type.GetProperties().Where(p => !Attribute.GetCustomAttributes(p,
                    typeof(NbtIgnoreAttribute)).Any());

                foreach (var property in properties)
                {
                    if (!property.CanRead)
                        continue;

                    NbtTag tag = null;

                    string name = property.Name;
                    nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));
                    var ignoreOnNullAttribute = Attribute.GetCustomAttribute(property, typeof(IgnoreOnNullAttribute));
                    if (nameAttributes.Length != 0)
                        name = ((TagNameAttribute)nameAttributes[0]).Name;

                    var innerSerializer = new NbtSerializer(property.PropertyType);
                    var propValue = property.GetValue(value, null);

                    if (propValue == null)
                    {
                        if (ignoreOnNullAttribute != null) continue;
                        if (property.PropertyType.IsValueType)
                        {
                            propValue = Activator.CreateInstance(property.PropertyType);
                        }
                        else if (property.PropertyType == typeof(string))
                            propValue = "";
                    }

                    tag = innerSerializer.Serialize(propValue, name);
                    compound.Add(tag);
                }

                return compound;
            }
        }

        public object Deserialize(NbtTag value, bool skipInterfaceCheck = false)
        {
            if (!skipInterfaceCheck && typeof(INbtSerializable).IsAssignableFrom(Type))
            {
                var instance = (INbtSerializable)Activator.CreateInstance(Type);
                instance.Deserialize(value);
                return instance;
            }
            if (value is NbtByte)
                return ((NbtByte)value).Value;
            else if (value is NbtByteArray)
                return ((NbtByteArray)value).Value;
            else if (value is NbtDouble)
                return ((NbtDouble)value).Value;
            else if (value is NbtFloat)
                return ((NbtFloat)value).Value;
            else if (value is NbtInt)
                return ((NbtInt)value).Value;
            else if (value is NbtIntArray)
                return ((NbtIntArray)value).Value;
            else if (value is NbtLong)
                return ((NbtLong)value).Value;
            else if (value is NbtShort)
                return ((NbtShort)value).Value;
            else if (value is NbtString)
                return ((NbtString)value).Value;
            else if (value is NbtList)
            {
                var list = (NbtList)value;
                var type = typeof(object);
                if (list.ListType == NbtTagType.Byte)
                    type = typeof(byte);
                else if (list.ListType == NbtTagType.ByteArray)
                    type = typeof(byte[]);
                else if (list.ListType == NbtTagType.Compound)
                {
                    if (Type.IsArray)
                        type = Type.GetElementType();
                    else
                        type = typeof(object);
                }
                else if (list.ListType == NbtTagType.Double)
                    type = typeof(double);
                else if (list.ListType == NbtTagType.Float)
                    type = typeof(float);
                else if (list.ListType == NbtTagType.Int)
                    type = typeof(int);
                else if (list.ListType == NbtTagType.IntArray)
                    type = typeof(int[]);
                else if (list.ListType == NbtTagType.Long)
                    type = typeof(long);
                else if (list.ListType == NbtTagType.Short)
                    type = typeof(short);
                else if (list.ListType == NbtTagType.String)
                    type = typeof(string);
                else
                    throw new NotSupportedException("The NBT list type '" + list.TagType + "' is not supported.");
                var array = Array.CreateInstance(type, list.Count);
                var innerSerializer = new NbtSerializer(type);
                for (int i = 0; i < array.Length; i++)
                    array.SetValue(innerSerializer.Deserialize(list[i]), i);
                return array;
            }
            else if (value is NbtCompound)
            {
                var compound = value as NbtCompound;
                var properties = Type.GetProperties().Where(p =>
                    !Attribute.GetCustomAttributes(p, typeof(NbtIgnoreAttribute)).Any());
                var resultObject = Activator.CreateInstance(Type);
                foreach (var property in properties)
                {
                    if (!property.CanWrite)
                        continue;
                    string name = property.Name;
                    var nameAttributes = Attribute.GetCustomAttributes(property, typeof(TagNameAttribute));

                    if (nameAttributes.Length != 0)
                        name = ((TagNameAttribute)nameAttributes[0]).Name;
                    var node = compound.Tags.SingleOrDefault(a => a.Name == name);
                    if (node == null) continue;
                    object data;
                    if (typeof(INbtSerializable).IsAssignableFrom(property.PropertyType))
                    {
                        data = Activator.CreateInstance(property.PropertyType);
                        ((INbtSerializable)data).Deserialize(node);
                    }
                    else
                        data = new NbtSerializer(property.PropertyType).Deserialize(node);

                    // Some manual casting for edge cases
                    if (property.PropertyType == typeof(bool)
                        && data is byte)
                        data = (byte)data == 1;
                    if (property.PropertyType == typeof(sbyte) && data is byte)
                        data = (sbyte)(byte)data;

                    property.SetValue(resultObject, data, null);
                }

                return resultObject;
            }

            throw new NotSupportedException("The node type '" + value.GetType() + "' is not supported.");
        }
    }

    public interface INbtSerializable
    {
        NbtTag Serialize(string tagName);
        void Deserialize(NbtTag value);
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Class)]
    public class TagNameAttribute : Attribute
    {
        public string Name { get; set; }

        /// <summary>
        /// Decorates the given property or field with the specified
        /// NBT tag name.
        /// </summary>
        public TagNameAttribute(string name)
        {
            Name = name;
        }
    }
    public class NbtIgnoreAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class IgnoreOnNullAttribute : Attribute { }
}