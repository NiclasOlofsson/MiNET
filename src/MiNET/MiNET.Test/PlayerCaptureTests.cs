#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Net;

namespace MiNET.Test
{
	/// <summary>
	///     Decodes captured AddPlayer and PlayerList frames from a real server and requires each to
	///     consume its frame exactly.
	///     <para>
	///         These are the packets a second player brings, and they carry the persona skin, whose
	///         AnimatedTextureType gained a value at ordinal 0 in protocol 2192 and shifted every
	///         other one. A skin is also the one structure a self round-trip cannot check, since both
	///         ends of MiNET share the codec and agree on a wrong answer; only bytes from another
	///         implementation can tell. Leftover bytes are the signal, so decode alone is the test.
	///     </para>
	/// </summary>
	[TestClass]
	public class PlayerCaptureTests
	{
		/// <summary>
		///     Point MINET_PLAYER_CAPTURES at a BedrockMessageHandlerBase.PacketDumpDir run that had a
		///     second player join. Inconclusive rather than failing when absent, since the captures
		///     live in temp_auto and are not committed.
		/// </summary>
		[TestMethod]
		public void Player_capture_sweep()
		{
			string directory = Environment.GetEnvironmentVariable("MINET_PLAYER_CAPTURES");
			if (directory == null || !Directory.Exists(directory))
			{
				Assert.Inconclusive($"Set MINET_PLAYER_CAPTURES to a packet dump directory to run this. Got: {directory ?? "(unset)"}");
				return;
			}

			var failures = new List<string>();
			int decoded = 0;

			foreach ((string pattern, int id) in new[] {("*id12.bin", 0x0c), ("*id63.bin", 0x3f), ("*id93.bin", 0x5d)})
			{
				foreach (string path in Directory.GetFiles(directory, pattern))
				{
					byte[] frame = File.ReadAllBytes(path);
					string name = Path.GetFileName(path);

					try
					{
						Packet packet = PacketFactory.Create(id, frame.AsMemory(), "mcpe");
						Assert.IsNotNull(packet, $"{name}: no packet type for id {id}");

						decoded++;
						Console.WriteLine($"{name}: {packet.GetType().Name} decoded");
					}
					catch (Exception e)
					{
						failures.Add($"{name}: {e.GetType().Name}: {e.Message}");
					}
				}
			}

			Console.WriteLine($"Decoded {decoded} player captures from {directory}.");

			Assert.AreEqual(0, failures.Count, string.Join(Environment.NewLine, failures));
			Assert.AreNotEqual(0, decoded, "No AddPlayer/PlayerList/PlayerSkin captures in the directory.");
		}
	}
}
