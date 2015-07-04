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

	public class Popup
	{
		public long Id { get; set; }
		public MessageType MessageType { get; set; }
		public string Message { get; set; }
		public long Duration { get; set; }
		public long DisplayDelay { get; set; }
		public long CurrentTick { get; set; }
		public long TransitionDelay { get; set; }
		public int Priority { get; set; }

		public Popup()
		{
			MessageType = MessageType.Popup;
		}
	}
}