using System;

namespace MiNET.Net;

[Flags]
public enum AuthInputFlags : long
{
	/// <summary>
	/// Pressing the "fly up" key when using touch.
	/// </summary>
	Ascend = 1L <<0,
	/// <summary>
	///  Pressing the "fly down" key when using touch. 
	/// </summary>
	Descend = 1L <<1,
	/// <summary>
	/// Pressing (and optionally holding) the jump key (while not flying).
	/// </summary>
	NorthJump = 1L <<2,
	/// <summary>
	///  Pressing (and optionally holding) the jump key (including while flying).
	/// </summary>
	JumpDown = 1L <<3,
	/// <summary>
	/// Pressing (and optionally holding) the sprint key (typically the CTRL key). Does not include double-pressing the forward key. 
	/// </summary>
	SprintDown = 1L <<4,
	/// <summary>
	///  Pressing (and optionally holding) the fly button ONCE when in flight mode when using touch. This has no obvious use.
	/// </summary>
	ChangeHeight = 1L <<5,
	/// <summary>
	/// Pressing (and optionally holding) the jump key (including while flying), and also auto-jumping. 
	/// </summary>
	Jumping = 1L <<6,
	/// <summary>
	/// Auto-swimming upwards while pressing forwards with auto-jump enabled.
	/// </summary>
	AutoJumpingInWater = 1L <<7,
	/// <summary>
	/// Sneaking, and pressing the "fly down" key or "sneak" key (including while flying).
	/// </summary>
	Sneaking = 1L <<8,
	/// <summary>
	///  Pressing (and optionally holding) the sneak key (including while flying). This includes when the sneak button is toggled ON with touch controls. 
	/// </summary>
	SneakDown = 1L <<9,
	
	/// <summary>
	/// Pressing the forward key (typically W on keyboard).
	/// </summary>
	WalkForwards = 1L <<10,
	/// <summary>
	/// Pressing the backward key (typically S on keyboard).
	/// </summary>
	WalkBackwards = 1L <<11,
	/// <summary>
	/// Pressing the left key (typically A on keyboard).
	/// </summary>
	StrafeLeft = 1L <<12,
	/// <summary>
	///  Pressing the right key (typically D on keyboard). 
	/// </summary>
	StrafeRight = 1L <<13,
	
	UpLeft = 1L <<14,
	UpRight = 1L <<15,
	
	/// <summary>
	/// Client wants to go upwards. Sent when Ascend or Jump is pressed, irrespective of whether flight is enabled
	/// </summary>
	WantUp = 1L <<16,
	
	/// <summary>
	/// Client wants to go downwards. Sent when Descend or Sneak is pressed, irrespective of whether flight is enabled
	/// </summary>
	WantDown = 1L <<17,
	WantDownSlow = 1L <<18,
	WantUpSlow = 1L <<19,
	
	Sprinting = 1L <<20,
	AscendBlock = 1L <<21,
	DescendBlock = 1L <<22,
	
	/// <summary>
	///  Toggling the sneak button on touch when the button enters the "enabled" state. 
	/// </summary>
	SneakToggleDown = 1L <<23,
	PersistSneak = 1L <<24,
	
	/// <summary>
	///		 Pressing the sprint toggle while NOT sprinting.
	/// </summary>
	StartSprinting = 1L <<25,
	
	/// <summary>
	///		 Pressing the sprint toggle while sprinting.
	/// </summary>
	StopSprinting = 1L <<26,
	
	/// <summary>
	///		 Pressing the sneak toggle while NOT sneaking.
	/// </summary>
	StartSneaking = 1L <<27,
	
	/// <summary>
	///		 Pressing the sneak toggle while sneaking.
	/// </summary>
	StopSneaking = 1L <<28,
	
	StartSwimming = 1L <<29,
	StopSwimming = 1L <<30,
	
	/// <summary>
	///  Initiating a new jump. Sent every time the client leaves the ground due to jumping, including auto jumps.
	/// </summary>
	StartJumping = 1L <<31,
	StartGliding = 1L <<32,
	StopGliding = 1L <<33,
	
	PerformItemInteraction = 1L <<34,
	PerformBlockActions = 1L <<35,
	PerformItemStackRequest = 1L <<36
}