using MiNET.Utils.Vectors;

namespace MiNET.Net;

public class UpdateSubChunkBlocksPacketEntry
{
	public BlockCoordinates Coordinates { get; set; }
	public uint BlockRuntimeId { get; set; }
	public uint Flags { get; set; }
	public long SyncedUpdatedEntityUniqueId { get; set; }
	public uint SyncedUpdateType { get; set; }
}