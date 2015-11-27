using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using fNbt;
using fNbt.Serialization;
using Craft.Net.Common;

namespace Craft.Net.Anvil
{
    /// <summary>
    /// Represents a Minecraft level
    /// </summary>
    public class Level : IDisposable
    {
        public const int TickLength = 1000 / 20;
        [NbtIgnore]
        private IWorldGenerator WorldGenerator { get; set; }
        [NbtIgnore]
        private string DatFile { get; set; }
        [NbtIgnore]
        public string BaseDirectory { get; private set; }
        [NbtIgnore]
        public List<World> Worlds { get; set; }
        [NbtIgnore]
        public World DefaultWorld
        {
            get
            {
                if (Worlds.Count == 0)
                    throw new InvalidOperationException("This level is associated with no worlds.");
                return Worlds[0];
            }
        }

        #region NBT Fields
        /// <summary>
        /// Always set to 19133.
        /// </summary>
        [TagName("version")]
        public int Version { get; private set; }
        /// <summary>
        /// True if this level has been initialized with a world.
        /// </summary>
        [TagName("initialized")]
        public bool Initialized { get; private set; }
        /// <summary>
        /// The name of this level.
        /// </summary>
        public string LevelName { get; set; }
        /// <summary>
        /// The name of the world generator to use. Corresponds to IWorldGenerator.GeneratorName.
        /// </summary>
        [TagName("generatorName")]
        public string GeneratorName { get; set; }
        /// <summary>
        /// The version number of the world generator to use.
        /// </summary>
        [TagName("generatorVersion")]
        public int GeneratorVersion { get; set; }
        /// <summary>
        /// The options to pass to the world generator.
        /// </summary>
        [TagName("generatorOptions")]
        public string GeneratorOptions { get; set; }
        /// <summary>
        /// The level's randomly generated seed.
        /// </summary>
        public long RandomSeed { get; set; }
        /// <summary>
        /// If true, structures like villages and strongholds will generate (depending on the IWorldGenerator).
        /// </summary>
        public bool MapFeatures { get; set; }
        /// <summary>
        /// Unix timestamp of last time this level was played.
        /// </summary>
        public long LastPlayed { get; set; }
        /// <summary>
        /// If true, single player users will be allowed to issue commands.
        /// </summary>
        [TagName("allowCommands")]
        public bool AllowCommands { get; set; }
        /// <summary>
        /// If true, the world is to be deleted upon the death of the user (singleplayer only).
        /// </summary>
        [TagName("hardcore")]
        public bool Hardcore { get; set; }
        private int GameType { get; set; }
        /// <summary>
        /// The default game mode for new players.
        /// </summary>
        [NbtIgnore]
        public GameMode GameMode
        {
            get { return (GameMode)GameType; }
            set { GameType = (int)value; }
        }
        /// <summary>
        /// The number of ticks since this level was created.
        /// </summary>
        public long Time { get; set; }
        /// <summary>
        /// The number of ticks in the current day.
        /// </summary>
        public long DayTime
        {
            get { return _DayTime; }
            set { _DayTime = value % 24000; }
        }
        private long _DayTime;
        /// <summary>
        /// This level's spawn point in the overworld.
        /// </summary>
        [NbtIgnore]
        public Vector3 Spawn
        {
            get
            {
                return new Vector3(SpawnX, SpawnY, SpawnZ);
            }
            set
            {
                SpawnX = (int)value.X;
                SpawnY = (int)value.Y;
                SpawnZ = (int)value.Z;
            }
        }
        private int SpawnX { get; set; }
        private int SpawnY { get; set; }
        private int SpawnZ { get; set; }
        /// <summary>
        /// True if the level is currently raining.
        /// </summary>
        [TagName("raining")]
        public bool Raining { get; set; }
        /// <summary>
        /// The number of ticks until the next weather cycle.
        /// </summary>
        [TagName("rainTime")]
        public int RainTime { get; set; }
        /// <summary>
        /// True if it is currently thundering.
        /// </summary>
        [TagName("thundering")]
        public bool Thundering { get; set; }
        /// <summary>
        /// The number of ticks until the next thunder cycle.
        /// </summary>
        [TagName("thunderTime")]
        public int ThunderTime { get; set; }
        /// <summary>
        /// The rules for this level.
        /// </summary>
        public GameRules GameRules { get; set; }
        #endregion

        /// <summary>
        /// An in-memory level, with all defaults set as such.
        /// </summary>
        public Level()
        {
            Version = 19133;
            Initialized = true;
            LevelName = "Level";
            GeneratorVersion = 1;
            GeneratorOptions = string.Empty;
            double seed = MathHelper.Random.NextDouble();
            unsafe { RandomSeed = *((long*)&seed); }
            MapFeatures = true;
            LastPlayed = DateTime.UtcNow.Ticks;
            AllowCommands = true;
            Hardcore = false;
            GameMode = GameMode.Survival;
            Time = 0;
            DayTime = 0;
            Spawn = Vector3.Zero;
            Raining = false;
            RainTime = MathHelper.Random.Next(0, 100000);
            Thundering = false;
            ThunderTime = MathHelper.Random.Next(0, 100000);
            GameRules = new GameRules();
            Worlds = new List<World>();
        }

        /// <summary>
        /// Creates an in-memory world with a given world generator.
        /// </summary>
        public Level(IWorldGenerator generator) : this()
        {
            GeneratorName = generator.GeneratorName;
            generator.Seed = RandomSeed;
            generator.Initialize(this);
            WorldGenerator = generator;
            Spawn = WorldGenerator.SpawnPoint;
        }

        /// <summary>
        /// Creates a named in-memory world.
        /// </summary>
        public Level(string levelName) : this()
        {
            LevelName = levelName;
        }

        /// <summary>
        /// Creates a named in-memory world with a given world generator.
        /// </summary>
        public Level(IWorldGenerator generator, string levelName) : this(generator)
        {
            LevelName = levelName;
        }

        /// <summary>
        /// Adds a world to this level.
        /// </summary>
        public void AddWorld(World world)
        {
            if (Worlds.Any(w => w.Name.ToUpper() == world.Name.ToUpper()))
                throw new InvalidOperationException("A world with the same name already exists in this level.");
            Worlds.Add(world);
        }

        /// <summary>
        /// Creates and adds a world to this level, with the given name.
        /// </summary>
        public void AddWorld(string name, IWorldGenerator worldGenerator = null)
        {
            if (Worlds.Any(w => w.Name.ToUpper() == name.ToUpper()))
                throw new InvalidOperationException("A world with the same name already exists in this level.");
            var world = new World(name);
            if (worldGenerator == null)
                world.WorldGenerator = WorldGenerator;
            else
                world.WorldGenerator = worldGenerator;
            Worlds.Add(world);
        }

        /// <summary>
        /// Saves this level to a directory.
        /// </summary>
        public void SaveTo(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            Save(Path.Combine(directory, "level.dat"));
        }

        /// <summary>
        /// Saves this level to a file, and the worlds to the same directory.
        /// </summary>
        public void Save(string file)
        {
            DatFile = file;
            if (!Path.IsPathRooted(file))
                file = Path.Combine(Directory.GetCurrentDirectory(), file);
            BaseDirectory = Path.GetDirectoryName(file);
            Save();
        }

        /// <summary>
        /// Saves this level. This will not work with an in-memory world.
        /// </summary>
        public void Save()
        {
            if (DatFile == null)
                throw new InvalidOperationException("This level exists only in memory. Use Save(string).");
            LastPlayed = DateTime.UtcNow.Ticks;
            var serializer = new NbtSerializer(typeof(Level));
            var tag = serializer.Serialize(this, "Data") as NbtCompound;
            var file = new NbtFile();
            file.RootTag.Add(tag);
            file.SaveToFile(DatFile, NbtCompression.GZip);
            // Save worlds
            foreach (var world in Worlds)
            {
                if (world.BaseDirectory == null)
                    world.Save(Path.Combine(BaseDirectory, world.Name));
                else
                    world.Save();
            }
        }

        /// <summary>
        /// Loads a level by directory name from the .minecraft saves folder.
        /// </summary>
        public static Level LoadSavedLevel(string world)
        {
            return LoadFrom(Path.Combine(GetDotMinecraftPath(), "saves", world));
        }

        private static string GetDotMinecraftPath()
        {
            if (RuntimeInfo.IsLinux)
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".minecraft");
            if (RuntimeInfo.IsMacOSX)
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library", "Application Support", ".minecraft");
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".minecraft");
        }

        /// <summary>
        /// Loads a level from the given directory.
        /// </summary>
        public static Level LoadFrom(string directory)
        {
            return Load(Path.Combine(directory, "level.dat"));
        }

        /// <summary>
        /// Loads a level from the given level.dat file.
        /// </summary>
        public static Level Load(string file)
        {
            if (!Path.IsPathRooted(file))
                file = Path.Combine(Directory.GetCurrentDirectory(), file);
            var serializer = new NbtSerializer(typeof(Level));
            var nbtFile = new NbtFile(file);
            var level = (Level)serializer.Deserialize(nbtFile.RootTag["Data"]);
            level.DatFile = file;
            level.BaseDirectory = Path.GetDirectoryName(file);
            level.WorldGenerator = GetGenerator(level.GeneratorName);
            level.WorldGenerator.Initialize(level);
            var worlds = Directory.GetDirectories(level.BaseDirectory).Where(
                d => Directory.GetFiles(d).Any(f => f.EndsWith(".mca") || f.EndsWith(".mcr")));
            foreach (var world in worlds)
            {
                var w = World.LoadWorld(world);
                w.WorldGenerator = level.WorldGenerator;
                level.AddWorld(w);
            }
            return level;
        }

        /// <summary>
        /// Gets a world generator for the given world generator name.
        /// </summary>
        public static IWorldGenerator GetGenerator(string generatorName)
        {
            IWorldGenerator worldGenerator = null;
            Type generatorType;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t =>
                    !t.IsAbstract && t.IsClass && typeof(IWorldGenerator).IsAssignableFrom(t) &&
                    t.GetConstructors().Any(c => c.IsPublic && !c.GetParameters().Any())).ToArray();
                generatorType = types.FirstOrDefault(t =>
                    (worldGenerator = (IWorldGenerator)Activator.CreateInstance(t)).GeneratorName == generatorName);
                if (generatorType != null)
                    break;
            }
            return worldGenerator;
        }

        // Internally, we use network slots everywhere, but on disk, we need to use data slots
        // Maybe someday we can use the Window classes to remap these around or something
        // Or better yet, Mojang can stop making terrible design decisions
        public static int DataSlotToNetworkSlot(int index)
        {
            if (index <= 8)
                index += 36;
            else if (index == 100)
                index = 8;
            else if (index == 101)
                index = 7;
            else if (index == 102)
                index = 6;
            else if (index == 103)
                index = 5;
            else if (index >= 80 && index <= 83)
                index -= 79;
            return index;
        }

        public static int NetworkSlotToDataSlot(int index)
        {
            if (index >= 36 && index <= 44)
                index -= 36;
            else if (index == 8)
                index = 100;
            else if (index == 7)
                index = 101;
            else if (index == 6)
                index = 102;
            else if (index == 5)
                index = 103;
            else if (index >= 1 && index <= 4)
                index += 79;
            return index;
        }

        public void Dispose()
        {
            foreach (var world in Worlds)
                world.Dispose();
        }
    }
}


namespace Craft.Net.Common
{

    public enum GameMode
    {
        /// <summary>
        /// Players fight against the enviornment, mobs, and players
        /// with limited resources.
        /// </summary>
        Survival = 0,
        /// <summary>
        /// Players are given unlimited resources, flying, and
        /// invulnerability.
        /// </summary>
        Creative = 1,
        /// <summary>
        /// Similar to survival, with the exception that players may
        /// not place or remove blocks.
        /// </summary>
        Adventure = 2
    }
    public class GameRules
    {
        public GameRules()
        {
            CommandBlockOutput = true;
            DoFireTick = true;
            DoMobLoot = true;
            DoMobSpawning = true;
            DoTileDrops = true;
            KeepInventory = false;
            MobGriefing = true;
        }

        private string commandBlockOutput;
        /// <summary>
        /// Determines if command blocks will have their results output to chat.
        /// </summary>
        [NbtIgnore]
        public bool CommandBlockOutput
        {
            get { return bool.Parse(commandBlockOutput); }
            set { commandBlockOutput = value.ToString(); }
        }

        private string doFireTick;
        /// <summary>
        /// Determines if fire may spread.
        /// </summary>
        [NbtIgnore]
        public bool DoFireTick
        {
            get { return bool.Parse(doFireTick); }
            set { doFireTick = value.ToString(); }
        }

        private string doMobLoot;
        /// <summary>
        /// Determines if killing mods drops items.
        /// </summary>
        [NbtIgnore]
        public bool DoMobLoot
        {
            get { return bool.Parse(doMobLoot); }
            set { doMobLoot = value.ToString(); }
        }

        private string doMobSpawning;
        /// <summary>
        /// Determines if mobs will be allowed to spawn.
        /// </summary>
        [NbtIgnore]
        public bool DoMobSpawning
        {
            get { return bool.Parse(doMobSpawning); }
            set { doMobSpawning = value.ToString(); }
        }

        private string doTileDrops;
        /// <summary>
        /// Determines if breaking blocks with tile entities will drop items within.
        /// </summary>
        [NbtIgnore]
        public bool DoTileDrops
        {
            get { return bool.Parse(doTileDrops); }
            set { doTileDrops = value.ToString(); }
        }

        private string keepInventory;
        /// <summary>
        /// True if a player's death does not remove their inventory.
        /// </summary>
        [NbtIgnore]
        public bool KeepInventory
        {
            get { return bool.Parse(keepInventory); }
            set { keepInventory = value.ToString(); }
        }

        private string mobGriefing;
        /// <summary>
        /// True to allow mob effects to modify terrain.
        /// </summary>
        [NbtIgnore]
        public bool MobGriefing
        {
            get { return bool.Parse(mobGriefing); }
            set { mobGriefing = value.ToString(); }
        }
    }

    public class MathHelper
    {
        /// <summary>
        /// A global <see cref="System.Random"/> instance.
        /// </summary>
        public static Random Random = new Random();

        /// <summary>
        /// Maps a float from 0...360 to 0...255
        /// </summary>
        /// <param name="value"></param>
        public static byte CreateRotationByte(float value)
        {
            return (byte)(((value % 360) / 360) * 256);
        }

        public static int CreateAbsoluteInt(double value)
        {
            return (int)(value * 32);
        }

       /* public static Coordinates3D BlockFaceToCoordinates(BlockFace face)
        {
            switch (face)
            {
                case BlockFace.NegativeY:
                    return Coordinates3D.Down;
                case BlockFace.PositiveY:
                    return Coordinates3D.Up;
                case BlockFace.NegativeZ:
                    return Coordinates3D.Backwards;
                case BlockFace.PositiveZ:
                    return Coordinates3D.Forwards;
                case BlockFace.NegativeX:
                    return Coordinates3D.Left;
                default:
                    return Coordinates3D.Right;
            }
        }*/

        public static double Distance2D(double a1, double a2, double b1, double b2)
        {
            return Math.Sqrt(Math.Pow(b1 - a1, 2) + Math.Pow(b2 - a2, 2));
        }

        public static Direction DirectionByRotationFlat(float yaw, bool invert = false)
        {
            byte direction = (byte)((int)Math.Floor((yaw * 4F) / 360F + 0.5D) & 3);
            if (invert)
                switch (direction)
                {
                    case 0: return Direction.North;
                    case 1: return Direction.East;
                    case 2: return Direction.South;
                    case 3: return Direction.West;
                }
            else
                switch (direction)
                {
                    case 0: return Direction.South;
                    case 1: return Direction.West;
                    case 2: return Direction.North;
                    case 3: return Direction.East;
                }
            return 0;
        }

        public static Direction DirectionByRotation(Vector3 source, float yaw, Vector3 position, bool invert = false)
        {
            // TODO: Figure out some algorithm based on player's look yaw
            double d = Math.Asin((position.Y - position.Y) / position.DistanceTo(source));
            if (d > (Math.PI / 4)) return invert ? (Direction)1 : (Direction)0;
            if (d < -(Math.PI / 4)) return invert ? (Direction)0 : (Direction)1;
            return DirectionByRotationFlat(yaw, invert);
        }

        /// <summary>
        /// Gets a byte representing block direction based on the rotation
        /// of the entity that placed it.
        /// </summary>
        public static Vector3 FowardVector(float yaw, bool invert = false)
        {
            Direction value = (Direction)DirectionByRotationFlat(yaw, invert);
            switch (value)
            {
                case Direction.East:
                    return Vector3.East;
                case Direction.West:
                    return Vector3.West;
                case Direction.North:
                    return Vector3.North;
                case Direction.South:
                    return Vector3.South;
                default:
                    return Vector3.Zero;
            }
        }

        public static Vector3 GetVectorTowards(Vector3 a, Vector3 b)
        {
            double angle = Math.Asin((a.X - b.X) / Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Z - b.Z, 2)));
            if (a.Z > b.Z) angle += Math.PI;
            return RotateY(Vector3.Forwards, angle);
        }

        public static Vector3 RotateX(Vector3 vector, double rotation) // TODO: Matrix
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.X,
                vector.Y * Math.Cos(rotation) - vector.Z * Math.Sin(rotation),
                vector.Y * Math.Sin(rotation) + vector.Z * Math.Cos(rotation));
        }

        public static Vector3 RotateY(Vector3 vector, double rotation)
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.Z * Math.Sin(rotation) + vector.X * Math.Cos(rotation),
                vector.Y,
                vector.Z * Math.Cos(rotation) - vector.X * Math.Sin(rotation));
        }

        public static Vector3 RotateZ(Vector3 vector, double rotation)
        {
            rotation = -rotation; // the algorithms I found were left-handed
            return new Vector3(
                vector.X * Math.Cos(rotation) - vector.Y * Math.Sin(rotation),
                vector.X * Math.Sin(rotation) + vector.Y * Math.Cos(rotation),
                vector.Z);
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static double ToNotchianYaw(double yaw)
        {
            return RadiansToDegrees(Math.PI - yaw);
        }

        public static double ToNotchianPitch(double pitch)
        {
            return RadiansToDegrees(-pitch);
        }

        /// <summary>
        /// Returns a value indicating the most extreme value of the
        /// provided Vector.
        /// </summary>
       /* public static unsafe CollisionPoint GetCollisionPoint(Vector3 velocity)
        {
            // NOTE: Does this really need to be so unsafe?
            int index = 0;
            void* vPtr = &velocity;
            double* ptr = (double*)vPtr;
            double max = 0;
            for (int i = 0; i < 3; i++)
            {
                double value = *(ptr + i);
                if (max < Math.Abs(value))
                {
                    index = i;
                    max = Math.Abs(value);
                }
            }
            switch (index)
            {
                case 0:
                    if (velocity.X < 0)
                        return CollisionPoint.NegativeX;
                    return CollisionPoint.PositiveX;
                case 1:
                    if (velocity.Y < 0)
                        return CollisionPoint.NegativeY;
                    return CollisionPoint.PositiveY;
                default:
                    if (velocity.Z < 0)
                        return CollisionPoint.NegativeZ;
                    return CollisionPoint.PositiveZ;
            }
        }*/
    }

    public enum Direction
    {
        Bottom = 0,
        Top = 1,
        North = 2,
        South = 3,
        West = 4,
        East = 5
    }

    public enum CollisionPoint
    {
        PositiveX,
        NegativeX,
        PositiveY,
        NegativeY,
        PositiveZ,
        NegativeZ
    }
}