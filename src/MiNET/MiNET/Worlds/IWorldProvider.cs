using Craft.Net.Common;

namespace MiNET.Worlds
{
	public interface IWorldProvider
	{
		bool IsCaching { get; }
		void Initialize();
		ChunkColumn GenerateChunkColumn(Coordinates2D chunkCoordinates);
		Coordinates3D GetSpawnPoint();
	}
}