using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net.RakNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiNET.Net.RakNet.Tests
{
	[TestClass()]
	public class ConnectedPacketTests
	{
		[TestMethod()]
		public void ConnectedPacket_encode_basic_ok()
		{

			ConnectedPacket packet = new ConnectedPacket();
			//packet.Decode();

		}
	}
}