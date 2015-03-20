namespace MiNET.Net
{
	public enum Reliability
	{
		Unreliable = 0,
		UnreliableSequenced = 1,
		Reliable = 2,
		ReliableOrdered = 3,
		ReliableSequenced = 4,
		UnreliableWithAckReceipt = 5,
		ReliableWithAckReceipt = 6,
		ReliableOrderedWithAckReceipt = 7
	}
}