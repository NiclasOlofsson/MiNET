using System.Numerics;

namespace MiNET.Net;

public partial class McpePlayerAuthInput : Packet<McpePlayerAuthInput>
{
	/// <summary>
	///		Pitch and Yaw hold the rotation that the player reports it has.
	/// </summary>
	public float Pitch;
	
	/// <summary>
	/// Pitch and Yaw hold the rotation that the player reports it has.
	/// </summary>
	public float Yaw;
	
	/// <summary>
	///		Pitch and Yaw hold the rotation that the player reports it has.
	/// </summary>
	public float HeadYaw;
	
	/// <summary>
	///		 Position holds the position that the player reports it has.
	/// </summary>
	public Vector3 Position;
	
	/// <summary>
	///		MoveVector is a Vec2 that specifies the direction in which the player moved, as a combination of X/Z
	///		values which are created using the WASD/controller stick state.
	/// </summary>
	public Vector2 MoveVector;
	
	/// <summary>
	///		InputData is a combination of bit flags that together specify the way the player moved last tick.
	/// </summary>
	public AuthInputFlags InputFlags;
	
	/// <summary>
	///  InputMode specifies the way that the client inputs data to the screen.
	/// </summary>
	public PlayerInputMode InputMode;
	
	/// <summary>
	/// PlayMode specifies the way that the player is playing. 
	/// </summary>
	public PlayerPlayMode PlayMode;

	/// <summary>
	///		InteractionModel is a constant representing the interaction model the player is using. 
	/// </summary>
	public PlayerInteractionModel InteractionModel;
	
	/// <summary>
	///		GazeDirection is the direction in which the player is gazing, when the PlayMode is PlayModeReality: In other words, when the player is playing in virtual reality.
	/// </summary>
	public Vector3 GazeDirection;
	
	/// <summary>
	///		Tick is the server tick at which the packet was sent. It is used in relation to CorrectPlayerMovePrediction.
	/// </summary>
	public long Tick;
	
	/// <summary>
	///		Delta was the delta between the old and the new position.
	/// </summary>
	public Vector3 Delta;

	partial void AfterDecode()
	{
		Pitch = ReadFloat();
		Yaw = ReadFloat();
		Position = ReadVector3();
		MoveVector = ReadVector2();
		HeadYaw = ReadFloat();
		InputFlags = (AuthInputFlags)ReadUnsignedVarLong();
		InputMode = (PlayerInputMode)ReadUnsignedVarInt();
		PlayMode = (PlayerPlayMode)ReadUnsignedVarInt();
		InteractionModel = (PlayerInteractionModel) ReadUnsignedVarInt();
		//IF VR.
		if (PlayMode == PlayerPlayMode.VR)
		{
			GazeDirection = ReadVector3();
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
		Write(MoveVector);
		Write(HeadYaw);
		WriteUnsignedVarLong((long)InputFlags);
		WriteUnsignedVarInt((uint)InputMode);
		WriteUnsignedVarInt((uint) PlayMode);
		WriteUnsignedVarInt((uint)InteractionModel);

		if (PlayMode == PlayerPlayMode.VR)
		{
			Write(GazeDirection);
		}
		
		WriteUnsignedVarLong(Tick);
		Write(Delta);
	}

	/// <inheritdoc />
	public override void Reset()
	{
		base.Reset();
		Pitch = Yaw = HeadYaw = 0f;
		MoveVector = Vector2.Zero;
		Position = Vector3.Zero;
		InputFlags = 0;
		InputMode = PlayerInputMode.Mouse;
		PlayMode = PlayerPlayMode.Normal;
		InteractionModel = PlayerInteractionModel.Touch;
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
	
	public enum PlayerInputMode
	{
		Mouse = 1,
		Touch = 2,
		GamePad = 3,
		MotionController = 4
	}

	public enum PlayerInteractionModel
	{
		Touch = 0,
		Crosshair = 1,
		Classic = 2
	}
}