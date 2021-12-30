using System.Numerics;

namespace MiNET.Net;

public partial class McpePlayerAuthInput : Packet<McpePlayerAuthInput>
{
	public float Pitch, Yaw;
	public Vector3 Position;
	public float MoveVecX, MoveVecZ;
	public float HeadYaw;
	public AuthInputFlags InputFlags;
	public uint InputMode;
	public PlayerPlayMode PlayMode;
	public Vector3 VirtualRealityGazeDirection;
	public long Tick;
	public Vector3 Delta;

	partial void AfterDecode()
	{
		Pitch = ReadFloat();
		Yaw = ReadFloat();
		Position = ReadVector3();
		MoveVecX = ReadFloat();
		MoveVecZ = ReadFloat();
		HeadYaw = ReadFloat();
		InputFlags = (AuthInputFlags)ReadUnsignedVarLong();
		InputMode = ReadUnsignedVarInt();
		PlayMode = (PlayerPlayMode)ReadUnsignedVarInt();
		//IF VR.
		if (PlayMode == PlayerPlayMode.VR)
		{
			VirtualRealityGazeDirection = ReadVector3();
		}

		Tick = ReadUnsignedVarLong();
		Delta = ReadVector3();

		if ((InputFlags & AuthInputFlags.PerformItemInteraction) != 0)
		{
			
		}
	}

	partial void AfterEncode()
	{
		Write(Pitch);
		Write(Yaw);
		Write(Position);
		Write(MoveVecX);
		Write(MoveVecZ);
		Write(HeadYaw);
		WriteUnsignedVarLong((long)InputFlags);
		WriteUnsignedVarInt(InputMode);
		WriteUnsignedVarInt((uint) PlayMode);

		if (PlayMode == PlayerPlayMode.VR)
		{
			Write(VirtualRealityGazeDirection);
		}
		
		WriteUnsignedVarLong(Tick);
		Write(Delta);
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Pitch = Yaw = MoveVecX = MoveVecZ = HeadYaw = 0f;
		Position = Vector3.Zero;
		InputFlags = 0;
		InputMode = 0;
		PlayMode = PlayerPlayMode.Normal;
		Tick = 0;
		Delta = Vector3.Zero;
	}

	public enum PlayerPlayMode
	{
		Normal = 0,
		Teaser = 1,
		Screen = 2,
		Viewer = 3,
		VR = 4,
		Placement = 5,
		LivingRoom = 6,
		ExitLevel = 7,
		ExitLevelLivingRoom = 8
	}
}