namespace MiNET.Net;

public partial class McpeRequestAbility
{
	public object Value = false;
	
	partial void AfterEncode()
	{
		switch (Value)
		{
			case bool boolean:
			{
				Write((byte)1);
				Write(boolean);
				Write(0f);
				break;
			}
			
			case float floatingPoint:
			{
				Write((byte)2);
				Write(false);
				Write(floatingPoint);
				break;
			}
		}	
	}

	partial void AfterDecode()
	{
		var type = ReadByte();
		var boolValue = ReadBool();
		var floatValue = ReadFloat();
		
		switch (type)
		{
			case 1:
				Value = boolValue;
				break;
			case 2:
				Value = floatValue;
				break;
		}
	}

	/// <inheritdoc />
	public override void Reset()
	{
		Value = false;
		base.Reset();
	}
}