using System;
using System.Linq;

namespace MiNET.Net;

public class HeightMapData
{
	public short[] Heights { get; }

	public HeightMapData(short[] heights)
	{
		if (heights.Length != 256)
			throw new ArgumentException("Expected 256 data entries");

		Heights = heights;
	}

	public int GetHeight(int x, int z)
	{
		return Heights[((z & 0xf) << 4) | (x & 0xf)];
	}

	public bool IsAllTooLow => Heights.Any(x => x > 0);
	public bool IsAllTooHigh => Heights.Any(x => x <= 15);
}

public enum SubChunkPacketHeightMapType : byte
{
	NoData = 0,
	Data = 1,
	AllTooHigh = 2,
	AllTooLow = 3
}

public enum SubChunkRequestResult : byte
{
	Success = 1,
	NoSuchChunk = 2,
	WrongDimension = 3,
	NullPlayer = 4, 
	YIndexOutOfBounds = 5,
	SuccessAllAir = 6
}