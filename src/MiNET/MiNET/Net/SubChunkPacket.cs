namespace MiNET.Net;

partial class McpeSubChunkPacket
{
	public SubChunkEntryCommon[] entries;
	partial void AfterEncode()
	{
		Write(entries != null ? entries.Length : 0);

		foreach (var entry in entries)
		{
			entry.Write(this, cacheEnabled);
		}
	}

	partial void AfterDecode()
	{
		var count = ReadInt();

		entries = new SubChunkEntryCommon[count];

		for (int i = 0; i < entries.Length; i++)
		{
			if (cacheEnabled)
			{
				entries[i] = new SubChunkEntryWithCache();
			}
			else
			{
				entries[i] = new SubChunkEntryWithoutCache();
			}
			
			entries[i].Read(this, cacheEnabled);
		}
	}
}

public abstract class SubChunkEntryCommon
{
	public SubChunkPositionOffset Offset { get; set; }
	public SubChunkRequestResult RequestResult { get; set; }
	public HeightMapData HeightMapData { get; set; }
	public byte[] Data { get; set; }
	
	public void Read(Packet packet, bool cacheEnabled)
	{
		Offset = packet.ReadSubChunkPositionOffset();
		RequestResult = (SubChunkRequestResult)packet.ReadByte();

		byte[] data = null;
		if (!cacheEnabled || RequestResult != SubChunkRequestResult.SuccessAllAir)
		{
			data = packet.ReadByteArray();
		}

		Data = data;
		
		HeightMapData = packet.ReadHeightMapData();
		
		OnRead(packet);
	}

	public void Write(Packet packet, bool cacheEnabled)
	{
		packet.Write(Offset);
		packet.Write((byte)RequestResult);

		if (!cacheEnabled || RequestResult != SubChunkRequestResult.SuccessAllAir)
		{
			packet.WriteByteArray(Data);
		}
		
		packet.Write(HeightMapData);
		
		OnWrite(packet);
	}

	protected abstract void OnRead(Packet packet);
	protected abstract void OnWrite(Packet packet);
}

public class SubChunkPositionOffset
{
	public sbyte XOffset { get; set; }
	public sbyte YOffset { get; set; }
	public sbyte ZOffset { get; set; }
}

public class SubChunkEntryWithCache : SubChunkEntryCommon
{
	public long usedBlobHash;

	/// <inheritdoc />
	protected override void OnRead(Packet packet)
	{
		usedBlobHash = packet.ReadLong();
	}

	/// <inheritdoc />
	protected override void OnWrite(Packet packet)
	{
		packet.Write(usedBlobHash);
	}
}

public class SubChunkEntryWithoutCache : SubChunkEntryCommon
{
	/// <inheritdoc />
	protected override void OnRead(Packet packet)
	{
		
	}

	/// <inheritdoc />
	protected override void OnWrite(Packet packet)
	{
		
	}
}