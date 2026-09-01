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
using fNbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Test
{
	/// <summary>
	///     The chunk reader takes palette entries off the bytes without fNbt, so the two have to
	///     agree on every entry the game has. They disagreeing means a world reads back as different
	///     blocks than it was saved with, which nothing downstream would notice.
	/// </summary>
	[TestClass]
	public class PaletteEntryReaderTests
	{
		[TestMethod]
		public void EveryPaletteEntryReadsBackToItself()
		{
			var scratch = new BlockStateContainer();
			int checkedEntries = 0;

			foreach (BlockStateContainer expected in BlockFactory.BlockPalette)
			{
				byte[] bytes = WriteStoredEntry(expected);

				int position = 0;
				PaletteEntryReader.Read(bytes, ref position, scratch);

				Assert.AreEqual(bytes.Length, position, $"{expected.Name}: reader stopped at {position} of {bytes.Length} bytes");
				Assert.IsTrue(BlockFactory.BlockStates.TryGetValue(scratch, out BlockStateContainer match), $"{expected.Name}: read back to something the palette does not hold");
				Assert.AreEqual(expected.RuntimeId, match.RuntimeId, $"{expected.Name}: resolved to the wrong block");

				checkedEntries++;
			}

			Assert.AreEqual(BlockFactory.BlockPalette.Count, checkedEntries);
		}

		/// <summary>
		///     States in the reverse order to the palette's own, which is what a world written by
		///     another version can hold. Identity is the set, not the sequence.
		/// </summary>
		[TestMethod]
		public void StateOrderDoesNotChangeWhatAnEntryResolvesTo()
		{
			var scratch = new BlockStateContainer();
			int checkedEntries = 0;

			foreach (BlockStateContainer expected in BlockFactory.BlockPalette)
			{
				if (expected.States.Count < 2) continue;

				var reversed = new BlockStateContainer {Name = expected.Name, States = new List<IBlockState>(expected.States)};
				reversed.States.Reverse();

				int position = 0;
				PaletteEntryReader.Read(WriteStoredEntry(reversed), ref position, scratch);

				Assert.IsTrue(BlockFactory.BlockStates.TryGetValue(scratch, out BlockStateContainer match), $"{expected.Name}: states in another order stopped resolving");
				Assert.AreEqual(expected.RuntimeId, match.RuntimeId, $"{expected.Name}: states in another order resolved elsewhere");

				checkedEntries++;
			}

			Assert.IsTrue(checkedEntries > 0, "the palette holds no multi-state block, so this proved nothing");
		}

		/// <summary>
		///     A tag type the palette never uses has to be refused rather than stepped over: reading
		///     on would drop a state and resolve the entry to a block that merely looks similar.
		/// </summary>
		[TestMethod]
		public void AnUnknownStateTypeIsRefusedRatherThanSkipped()
		{
			var tag = new NbtCompound("")
			{
				new NbtString("name", "minecraft:stone"),
				new NbtCompound("states") {new NbtFloat("weirdness", 1.5f)},
				new NbtInt("version", 0)
			};

			byte[] bytes = ToBytes(tag);

			var scratch = new BlockStateContainer();
			int position = 0;

			Assert.ThrowsExactly<FormatException>(() => PaletteEntryReader.Read(bytes, ref position, scratch));
		}

		/// <summary>The stored form: little endian, fixed-width lengths, no varints.</summary>
		private static byte[] WriteStoredEntry(BlockStateContainer container)
		{
			var states = new NbtCompound("states");
			foreach (IBlockState state in container.States)
			{
				switch (state)
				{
					case BlockStateByte stateByte:
						states.Add(new NbtByte(stateByte.Name, stateByte.Value));
						break;
					case BlockStateInt stateInt:
						states.Add(new NbtInt(stateInt.Name, stateInt.Value));
						break;
					case BlockStateString stateString:
						states.Add(new NbtString(stateString.Name, stateString.Value));
						break;
					default:
						throw new InvalidOperationException($"Unhandled state type {state.GetType().Name}");
				}
			}

			return ToBytes(new NbtCompound("")
			{
				new NbtString("name", container.Name),
				states,
				new NbtInt("version", BlockPaletteData.BlockStateVersion)
			});
		}

		private static byte[] ToBytes(NbtCompound tag)
		{
			var file = new NbtFile(tag)
			{
				BigEndian = false,
				UseVarInt = false
			};

			using var stream = new MemoryStream();
			file.SaveToStream(stream, NbtCompression.None);

			return stream.ToArray();
		}
	}
}
