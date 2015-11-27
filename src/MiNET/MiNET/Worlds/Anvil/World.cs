using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using Craft.Net.Common;
using System.Runtime.InteropServices;

namespace Craft.Net.Anvil
{
    public class World : IDisposable
    {
        public const int Height = 256;

        public string Name { get; set; }
        public string BaseDirectory { get; internal set; }
        public Dictionary<Coordinates2D, Region> Regions { get; set; }
        public IWorldGenerator WorldGenerator { get; set; }
        public event EventHandler<BlockChangeEventArgs> BlockChange;
        public event EventHandler<SpawnEntityEventArgs> SpawnEntityRequested;

        public World(string name)
        {
            Name = name;
            Regions = new Dictionary<Coordinates2D, Region>();
        }

        public World(string name, IWorldGenerator worldGenerator) : this(name)
        {
            WorldGenerator = worldGenerator;
        }

        public static World LoadWorld(string baseDirectory)
        {
            if (!Directory.Exists(baseDirectory))
                throw new DirectoryNotFoundException();
            var world = new World(Path.GetFileName(baseDirectory));
            world.BaseDirectory = baseDirectory;
            return world;
        }

        /// <summary>
        /// Finds a chunk that contains the specified block coordinates.
        /// </summary>
        public Chunk FindChunk(Coordinates3D coordinates)
        {
            Chunk chunk;
            FindBlockPosition(coordinates, out chunk);
            return chunk;
        }

        public Chunk GetChunk(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var region = LoadOrGenerateRegion(new Coordinates2D(regionX, regionZ));
            return region.GetChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public void GenerateChunk(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var region = LoadOrGenerateRegion(new Coordinates2D(regionX, regionZ));
            region.GenerateChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public Chunk GetChunkWithoutGeneration(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var regionPosition = new Coordinates2D(regionX, regionZ);
            if (!Regions.ContainsKey(regionPosition)) return null;
            return Regions[regionPosition].GetChunkWithoutGeneration(
                new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public void SetChunk(Coordinates2D coordinates, Chunk chunk)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var region = LoadOrGenerateRegion(new Coordinates2D(regionX, regionZ));
            lock (region)
            {
                chunk.IsModified = true;
                region.SetChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32), chunk);
            }
        }

        public void UnloadRegion(Coordinates2D coordinates)
        {
            lock (Regions)
            {
                Regions[coordinates].Save(Path.Combine(BaseDirectory, Region.GetRegionFileName(coordinates)));
                Regions.Remove(coordinates);
            }
        }

        public void UnloadChunk(Coordinates2D coordinates)
        {
            int regionX = coordinates.X / Region.Width - ((coordinates.X < 0) ? 1 : 0);
            int regionZ = coordinates.Z / Region.Depth - ((coordinates.Z < 0) ? 1 : 0);

            var regionPosition = new Coordinates2D(regionX, regionZ);
            if (!Regions.ContainsKey(regionPosition))
                throw new ArgumentOutOfRangeException("coordinates");
            Regions[regionPosition].UnloadChunk(new Coordinates2D(coordinates.X - regionX * 32, coordinates.Z - regionZ * 32));
        }

        public short GetBlockId(Coordinates3D coordinates)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            return chunk.GetBlockId(coordinates);
        }

        public byte GetMetadata(Coordinates3D coordinates)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            return chunk.GetMetadata(coordinates);
        }

        public byte GetSkyLight(Coordinates3D coordinates)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            return chunk.GetSkyLight(coordinates);
        }

        public byte GetBlockLight(Coordinates3D coordinates)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            return chunk.GetBlockLight(coordinates);
        }

        public void SetBlockId(Coordinates3D coordinates, short value)
        {
            Chunk chunk;
            var adjustedCoordinates = FindBlockPosition(coordinates, out chunk);
            chunk.SetBlockId(adjustedCoordinates, value);
            OnBlockChange(coordinates);
        }

        public void SetMetadata(Coordinates3D coordinates, byte value)
        {
            Chunk chunk;
            var adjustedCoordinates = FindBlockPosition(coordinates, out chunk);
            chunk.SetMetadata(adjustedCoordinates, value);
            OnBlockChange(coordinates);
        }

        public void SetSkyLight(Coordinates3D coordinates, byte value)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            chunk.SetSkyLight(coordinates, value);
        }

        public void SetBlockLight(Coordinates3D coordinates, byte value)
        {
            Chunk chunk;
            coordinates = FindBlockPosition(coordinates, out chunk);
            chunk.SetBlockLight(coordinates, value);
        }

        public void Save()
        {
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save(Path.Combine(BaseDirectory, Region.GetRegionFileName(region.Key)));
            }
        }

        public void Save(string path)
        {
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            BaseDirectory = path;
            lock (Regions)
            {
                foreach (var region in Regions)
                    region.Value.Save(Path.Combine(BaseDirectory, Region.GetRegionFileName(region.Key)));
            }
        }

        public Coordinates3D FindBlockPosition(Coordinates3D coordinates, out Chunk chunk)
        {
            if (coordinates.Y < 0 || coordinates.Y >= Chunk.Height)
                throw new ArgumentOutOfRangeException("coordinates", "Coordinates are out of range");

            var chunkX = (int)Math.Floor((double)coordinates.X / Chunk.Width);
            var chunkZ = (int)Math.Floor((double)coordinates.Z / Chunk.Depth);

            chunk = GetChunk(new Coordinates2D(chunkX, chunkZ));
            return new Coordinates3D(
                (coordinates.X - chunkX * Chunk.Width) % Chunk.Width,
                coordinates.Y,
                (coordinates.Z - chunkZ * Chunk.Depth) % Chunk.Depth);
        }

        public static bool IsValidPosition(Coordinates3D position)
        {
            return position.Y >= 0 && position.Y <= 255;
        }

        private Region LoadOrGenerateRegion(Coordinates2D coordinates)
        {
            if (Regions.ContainsKey(coordinates))
                return Regions[coordinates];
            Region region;
            if (BaseDirectory != null)
            {
                var file = Path.Combine(BaseDirectory, Region.GetRegionFileName(coordinates));
                if (File.Exists(file))
                    region = new Region(coordinates, this, file);
                else
                    region = new Region(coordinates, this);
            }
            else
                region = new Region(coordinates, this);
            lock (Regions)
                Regions[coordinates] = region;
            return region;
        }

        public void Dispose()
        {
            foreach (var region in Regions)
                region.Value.Dispose();
        }

        public void OnSpawnEntityRequested(object entity)
        {
            if (SpawnEntityRequested != null) SpawnEntityRequested(this, new SpawnEntityEventArgs(entity));
        }

        protected internal virtual void OnBlockChange(Coordinates3D coordinates)
        {
            if (BlockChange != null) BlockChange(this, new BlockChangeEventArgs(coordinates));
        }
    }
}


namespace Craft.Net.Common
{
    public struct Coordinates2D : IEquatable<Coordinates2D>
    {
        public int X, Z;

        public Coordinates2D(int value)
        {
            X = Z = value;
        }

        public Coordinates2D(int x, int z)
        {
            X = x;
            Z = z;
        }

        public Coordinates2D(Coordinates2D v)
        {
            X = v.X;
            Z = v.Z;
        }

        /// <summary>
        /// Converts this Coordinates2D to a string in the format &lt;x, z&gt;.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<{0},{1}>", X, Z);
        }

        #region Math

        /// <summary>
        /// Calculates the distance between two Coordinates2D objects.
        /// </summary>
        public double DistanceTo(Coordinates2D other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private int Square(int num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this Coordinates2D from Coordinates2D.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Coordinates2D Min(Coordinates2D value1, Coordinates2D value2)
        {
            return new Coordinates2D(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Coordinates2D Max(Coordinates2D value1, Coordinates2D value2)
        {
            return new Coordinates2D(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Coordinates2D a, Coordinates2D b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Coordinates2D a, Coordinates2D b)
        {
            return a.Equals(b);
        }

        public static Coordinates2D operator +(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X + b.X, a.Z + b.Z);
        }

        public static Coordinates2D operator -(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X - b.X, a.Z - b.Z);
        }

        public static Coordinates2D operator -(Coordinates2D a)
        {
            return new Coordinates2D(
                -a.X,
                -a.Z);
        }

        public static Coordinates2D operator *(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X * b.X, a.Z * b.Z);
        }

        public static Coordinates2D operator /(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X / b.X, a.Z / b.Z);
        }

        public static Coordinates2D operator %(Coordinates2D a, Coordinates2D b)
        {
            return new Coordinates2D(a.X % b.X, a.Z % b.Z);
        }

        public static Coordinates2D operator +(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X + b, a.Z + b);
        }

        public static Coordinates2D operator -(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X - b, a.Z - b);
        }

        public static Coordinates2D operator *(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X * b, a.Z * b);
        }

        public static Coordinates2D operator /(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X / b, a.Z / b);
        }

        public static Coordinates2D operator %(Coordinates2D a, int b)
        {
            return new Coordinates2D(a.X % b, a.Z % b);
        }

        public static Coordinates2D operator +(int a, Coordinates2D b)
        {
            return new Coordinates2D(a + b.X, a + b.Z);
        }

        public static Coordinates2D operator -(int a, Coordinates2D b)
        {
            return new Coordinates2D(a - b.X, a - b.Z);
        }

        public static Coordinates2D operator *(int a, Coordinates2D b)
        {
            return new Coordinates2D(a * b.X, a * b.Z);
        }

        public static Coordinates2D operator /(int a, Coordinates2D b)
        {
            return new Coordinates2D(a / b.X, a / b.Z);
        }

        public static Coordinates2D operator %(int a, Coordinates2D b)
        {
            return new Coordinates2D(a % b.X, a % b.Z);
        }

        public static explicit operator Coordinates2D(Coordinates3D a)
        {
            return new Coordinates2D(a.X, a.Z);
        }

        #endregion

        #region Constants

        public static readonly Coordinates2D Zero = new Coordinates2D(0);
        public static readonly Coordinates2D One = new Coordinates2D(1);

        public static readonly Coordinates2D Forward = new Coordinates2D(0, 1);
        public static readonly Coordinates2D Backward = new Coordinates2D(0, -1);
        public static readonly Coordinates2D Left = new Coordinates2D(-1, 0);
        public static readonly Coordinates2D Right = new Coordinates2D(1, 0);

        #endregion

        public bool Equals(Coordinates2D other)
        {
            return other.X.Equals(X) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Coordinates2D)) return false;
            return Equals((Coordinates2D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }

    public struct Coordinates3D : IEquatable<Coordinates3D>
    {
        public int X, Y, Z;

        public Coordinates3D(int value)
        {
            X = Y = Z = value;
        }

        public Coordinates3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Coordinates3D(Coordinates3D v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Converts this Coordinates3D to a string in the format &lt;x, y, z&gt;.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<{0},{1},{2}>", X, Y, Z);
        }

        #region Math

        /// <summary>
        /// Calculates the distance between two Coordinates3D objects.
        /// </summary>
        public double DistanceTo(Coordinates3D other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private int Square(int num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this Coordinate3D from Coordinates3D.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Coordinates3D Min(Coordinates3D value1, Coordinates3D value2)
        {
            return new Coordinates3D(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Y, value2.Y),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Coordinates3D Max(Coordinates3D value1, Coordinates3D value2)
        {
            return new Coordinates3D(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Y, value2.Y),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Coordinates3D a, Coordinates3D b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Coordinates3D a, Coordinates3D b)
        {
            return a.Equals(b);
        }

        public static Coordinates3D operator +(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinates3D operator -(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Coordinates3D operator -(Coordinates3D a)
        {
            return new Coordinates3D(-a.X, -a.Y, -a.Z);
        }

        public static Coordinates3D operator *(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X * b.X, a.Y * b.Y, a.Z * b.Z);
        }

        public static Coordinates3D operator /(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X / b.X, a.Y / b.Y, a.Z / b.Z);
        }

        public static Coordinates3D operator %(Coordinates3D a, Coordinates3D b)
        {
            return new Coordinates3D(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static Coordinates3D operator +(Coordinates3D a, int b)
        {
            return new Coordinates3D(a.X + b, a.Y + b, a.Z + b);
        }

        public static Coordinates3D operator -(Coordinates3D a, int b)
        {
            return new Coordinates3D(a.X - b, a.Y - b, a.Z - b);
        }

        public static Coordinates3D operator *(Coordinates3D a, int b)
        {
            return new Coordinates3D(a.X * b, a.Y * b, a.Z * b);
        }

        public static Coordinates3D operator /(Coordinates3D a, int b)
        {
            return new Coordinates3D(a.X / b, a.Y / b, a.Z / b);
        }

        public static Coordinates3D operator %(Coordinates3D a, int b)
        {
            return new Coordinates3D(a.X % b, a.Y % b, a.Z % b);
        }

        public static Coordinates3D operator +(int a, Coordinates3D b)
        {
            return new Coordinates3D(a + b.X, a + b.Y, a + b.Z);
        }

        public static Coordinates3D operator -(int a, Coordinates3D b)
        {
            return new Coordinates3D(a - b.X, a - b.Y, a - b.Z);
        }

        public static Coordinates3D operator *(int a, Coordinates3D b)
        {
            return new Coordinates3D(a * b.X, a * b.Y, a * b.Z);
        }

        public static Coordinates3D operator /(int a, Coordinates3D b)
        {
            return new Coordinates3D(a / b.X, a / b.Y, a / b.Z);
        }

        public static Coordinates3D operator %(int a, Coordinates3D b)
        {
            return new Coordinates3D(a % b.X, a % b.Y, a % b.Z);
        }

        public static explicit operator Coordinates3D(Coordinates2D a)
        {
            return new Coordinates3D(a.X, 0, a.Z);
        }

        public static implicit operator Coordinates3D(Vector3 a)
        {
            return new Coordinates3D((int)a.X, (int)a.Y, (int)a.Z);
        }

        public static implicit operator Vector3(Coordinates3D a)
        {
            return new Vector3(a.X, a.Y, a.Z);
        }

        #endregion

        #region Constants

        public static readonly Coordinates3D Zero = new Coordinates3D(0);
        public static readonly Coordinates3D One = new Coordinates3D(1);

        public static readonly Coordinates3D Up = new Coordinates3D(0, 1, 0);
        public static readonly Coordinates3D Down = new Coordinates3D(0, -1, 0);
        public static readonly Coordinates3D Left = new Coordinates3D(-1, 0, 0);
        public static readonly Coordinates3D Right = new Coordinates3D(1, 0, 0);
        public static readonly Coordinates3D Backwards = new Coordinates3D(0, 0, -1);
        public static readonly Coordinates3D Forwards = new Coordinates3D(0, 0, 1);

        public static readonly Coordinates3D East = new Coordinates3D(1, 0, 0);
        public static readonly Coordinates3D West = new Coordinates3D(-1, 0, 0);
        public static readonly Coordinates3D North = new Coordinates3D(0, 0, -1);
        public static readonly Coordinates3D South = new Coordinates3D(0, 0, 1);

        #endregion

        public bool Equals(Coordinates3D other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Coordinates3D)) return false;
            return Equals((Coordinates3D)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }
    /// <summary>
    /// Represents the location of an object in 3D space.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Vector3 : IEquatable<Vector3>
    {
        [FieldOffset(0)]
        public double X;
        [FieldOffset(8)]
        public double Y;
        [FieldOffset(16)]
        public double Z;

        public Vector3(double value)
        {
            X = Y = Z = value;
        }

        public Vector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector3 v)
        {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        /// <summary>
        /// Converts this Vector3 to a string in the format &lt;x, y, z&gt;.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("<{0},{1},{2}>", X, Y, Z);
        }

        #region Math

        /// <summary>
        /// Truncates the decimal component of each part of this Vector3.
        /// </summary>
        public Vector3 Floor()
        {
            return new Vector3(Math.Floor(X), Math.Floor(Y), Math.Floor(Z));
        }

        /// <summary>
        /// Calculates the distance between two Vector3 objects.
        /// </summary>
        public double DistanceTo(Vector3 other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
                             Square(other.Z - Z));
        }

        /// <summary>
        /// Calculates the square of a num.
        /// </summary>
        private double Square(double num)
        {
            return num * num;
        }

        /// <summary>
        /// Finds the distance of this vector from Vector3.Zero
        /// </summary>
        public double Distance
        {
            get
            {
                return DistanceTo(Zero);
            }
        }

        public static Vector3 Min(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                Math.Min(value1.X, value2.X),
                Math.Min(value1.Y, value2.Y),
                Math.Min(value1.Z, value2.Z)
                );
        }

        public static Vector3 Max(Vector3 value1, Vector3 value2)
        {
            return new Vector3(
                Math.Max(value1.X, value2.X),
                Math.Max(value1.Y, value2.Y),
                Math.Max(value1.Z, value2.Z)
                );
        }

        #endregion

        #region Operators

        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !a.Equals(b);
        }

        public static bool operator ==(Vector3 a, Vector3 b)
        {
            return a.Equals(b);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X + b.X,
                a.Y + b.Y,
                a.Z + b.Z);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X - b.X,
                a.Y - b.Y,
                a.Z - b.Z);
        }

        public static Vector3 operator +(Vector3 a, Size b)
        {
            return new Vector3(
                a.X + b.Width,
                a.Y + b.Height,
                a.Z + b.Depth);
        }

        public static Vector3 operator -(Vector3 a, Size b)
        {
            return new Vector3(
                a.X - b.Width,
                a.Y - b.Height,
                a.Z - b.Depth);
        }

        public static Vector3 operator -(Vector3 a)
        {
            return new Vector3(
                -a.X,
                -a.Y,
                -a.Z);
        }

        public static Vector3 operator *(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X * b.X,
                a.Y * b.Y,
                a.Z * b.Z);
        }

        public static Vector3 operator /(Vector3 a, Vector3 b)
        {
            return new Vector3(
                a.X / b.X,
                a.Y / b.Y,
                a.Z / b.Z);
        }

        public static Vector3 operator %(Vector3 a, Vector3 b)
        {
            return new Vector3(a.X % b.X, a.Y % b.Y, a.Z % b.Z);
        }

        public static Vector3 operator +(Vector3 a, double b)
        {
            return new Vector3(
                a.X + b,
                a.Y + b,
                a.Z + b);
        }

        public static Vector3 operator -(Vector3 a, double b)
        {
            return new Vector3(
                a.X - b,
                a.Y - b,
                a.Z - b);
        }

        public static Vector3 operator *(Vector3 a, double b)
        {
            return new Vector3(
                a.X * b,
                a.Y * b,
                a.Z * b);
        }

        public static Vector3 operator /(Vector3 a, double b)
        {
            return new Vector3(
                a.X / b,
                a.Y / b,
                a.Z / b);
        }

        public static Vector3 operator %(Vector3 a, double b)
        {
            return new Vector3(a.X % b, a.Y % b, a.Y % b);
        }

        public static Vector3 operator +(double a, Vector3 b)
        {
            return new Vector3(
                a + b.X,
                a + b.Y,
                a + b.Z);
        }

        public static Vector3 operator -(double a, Vector3 b)
        {
            return new Vector3(
                a - b.X,
                a - b.Y,
                a - b.Z);
        }

        public static Vector3 operator *(double a, Vector3 b)
        {
            return new Vector3(
                a * b.X,
                a * b.Y,
                a * b.Z);
        }

        public static Vector3 operator /(double a, Vector3 b)
        {
            return new Vector3(
                a / b.X,
                a / b.Y,
                a / b.Z);
        }

        public static Vector3 operator %(double a, Vector3 b)
        {
            return new Vector3(a % b.X, a % b.Y, a % b.Y);
        }

        #endregion

        #region Constants

        public static readonly Vector3 Zero = new Vector3(0);
        public static readonly Vector3 One = new Vector3(1);

        public static readonly Vector3 Up = new Vector3(0, 1, 0);
        public static readonly Vector3 Down = new Vector3(0, -1, 0);
        public static readonly Vector3 Left = new Vector3(-1, 0, 0);
        public static readonly Vector3 Right = new Vector3(1, 0, 0);
        public static readonly Vector3 Backwards = new Vector3(0, 0, -1);
        public static readonly Vector3 Forwards = new Vector3(0, 0, 1);

        public static readonly Vector3 East = new Vector3(1, 0, 0);
        public static readonly Vector3 West = new Vector3(-1, 0, 0);
        public static readonly Vector3 North = new Vector3(0, 0, -1);
        public static readonly Vector3 South = new Vector3(0, 0, 1);

        #endregion

        public bool Equals(Vector3 other)
        {
            return other.X.Equals(X) && other.Y.Equals(Y) && other.Z.Equals(Z);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vector3)) return false;
            return Equals((Vector3)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = X.GetHashCode();
                result = (result * 397) ^ Y.GetHashCode();
                result = (result * 397) ^ Z.GetHashCode();
                return result;
            }
        }
    }

    public struct Size
    {
        public double Depth;
        public double Height;
        public double Width;

        public Size(double width, double height, double depth)
        {
            this.Width = width;
            this.Height = height;
            this.Depth = depth;
        }

        public Size(Size s)
        {
            this.Width = s.Width;
            this.Height = s.Height;
            this.Depth = s.Depth;
        }

        // TODO: More operators
        public static Size operator /(Size a, double b)
        {
            return new Size(a.Width / b, a.Height / b, a.Depth / b);
        }
    }
}
