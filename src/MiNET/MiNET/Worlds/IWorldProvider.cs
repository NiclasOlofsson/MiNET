using System.Numerics;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public interface IWorldProvider
	{
		bool IsCaching { get; }

		void Initialize();

		ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates);

		Vector3 GetSpawnPoint();
		string GetName();

		long GetTime();

		int SaveChunks();
	}
}