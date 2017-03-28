namespace MiNET
{
	public enum MessageType : byte
	{
		Raw = 0,
		Chat = 1,
		Translation = 2,
		Popup = 3,
		Tip = 4
	}

	public enum TitleType
	{
		Clear = 0,
		Reset = 1,
		Title = 2,
		SubTitle = 3,
		ActionBar = 4,
		AnimationTimes = 5
	}
}