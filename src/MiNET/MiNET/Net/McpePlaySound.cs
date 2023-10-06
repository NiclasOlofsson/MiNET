using System.Numerics;
using MiNET.Utils.Vectors;

namespace MiNET.Net
{
	public partial class McpePlaySound
	{
		public Vector3 position; // = null;
		public float volume; // = null;
		public float pitch; // = null;

		partial void AfterEncode()
		{
			var pos = (BlockCoordinates) position * 8;
			WriteSignedVarInt(pos.X);
			WriteSignedVarInt(pos.Y >> 1);
			WriteSignedVarInt(pos.Z);

			Write(volume);
			Write(pitch);
		}

		partial void AfterDecode()
		{
			position = new Vector3(ReadSignedVarInt() / 8f, ReadSignedVarInt() / 4f, ReadSignedVarInt() / 8f);
			volume = ReadFloat();
			pitch = ReadFloat();
		}

		public override void Reset()
		{
			position = default(BlockCoordinates);
			volume = default(float);
			pitch = default(float);

			base.Reset();
		}
	}
}
