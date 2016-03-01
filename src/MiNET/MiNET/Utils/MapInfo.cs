using System;

namespace MiNET.Utils
{
	public class MapInfo: ICloneable
	{
		public long MapId;
		public byte UpdateType;
		public byte Direction;
		public byte X;
		public byte Z;
		public int Col;
		public int Row;
		public int XOffset;
		public int ZOffset;
		public byte[] Data;




		public override string ToString()
		{
			return $"MapId: {MapId}, UpdateType: {UpdateType}, Direction: {Direction}, X: {X}, Z: {Z}, Col: {Col}, Row: {Row}, X-offset: {XOffset}, Z-offset: {ZOffset}, Data: {Data?.Length}";
		}

		public object Clone()
		{
			MapInfo clone = new MapInfo();
			clone.MapId = MapId;
			clone.UpdateType = UpdateType;
			clone.Direction = Direction;
			clone.X = X;
			clone.Z = Z;
			clone.Col = Col;
			clone.Row = Row;
			clone.XOffset = XOffset;
			clone.ZOffset = ZOffset;
			clone.Data = (byte[]) Data?.Clone();

			return clone;
		}
	}
}