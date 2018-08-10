namespace MiNET
{
	public enum MessageType : byte
	{
		Raw = 0,
		Chat = 1,
		Translation = 2,
		Popup = 3,
		JukeboxPopup = 4,
		Tip = 5,
		System = 6,
		Whisper = 7,
		Announcement = 8
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