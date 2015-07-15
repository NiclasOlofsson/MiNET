using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public interface IWorldProvider
	{
		bool IsCaching { get; }

		void Initialize();

		ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates);

		McpeBatch GenerateFullBatch(ChunkCoordinates chunkCoordinates);

		Vector3 GetSpawnPoint();

		long GetTime();

		void SaveChunks();
	}
}