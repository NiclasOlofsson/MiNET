using Craft.Net.Common;

namespace Craft.Net.Anvil
{
    public interface IWorldGenerator
    {
        string LevelType { get; }
        string GeneratorName { get; }
        string GeneratorOptions { get; set; }
        long Seed { get; set; }
        Vector3 SpawnPoint { get; }

        Chunk GenerateChunk(Coordinates2D position);

        /// <summary>
        /// Called after the world generator is created and
        /// all values are set.
        /// </summary>
        void Initialize(Level level);
    }
}