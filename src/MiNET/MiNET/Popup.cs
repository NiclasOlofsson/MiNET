namespace MiNET
{
	public class Popup
	{
		public long Id { get; set; }
		internal long CurrentTick { get; set; }
		public MessageType MessageType { get; set; }
		public string Message { get; set; }
		public long Duration { get; set; }
		public long DisplayDelay { get; set; }
		public long TransitionDelay { get; set; }
		public int Priority { get; set; }

		public Popup()
		{
			MessageType = MessageType.Popup;
		}
	}
}