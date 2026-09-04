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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Test.Worlds
{
	[TestClass]
	public class ChunkSentSetTests
	{
		// A player's sent-set is membership in their disc and nothing else. Block writes reach the
		// client as UpdateBlock, so a column the client holds is never pushed a second time for
		// having changed: re-pushing made every chunk-boundary crossing near a spreading liquid
		// hand the client its own column again, whole, to rebuild.

		[TestMethod]
		public void HeldColumn_IsNotPushedAgainWhenItsContentChanges()
		{
			var provider = new AnvilWorldProvider {MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld)};
			var level = new Level(null, "sent-set", provider, new EntityManager(), GameMode.Creative, viewDistance: 1) {EnableLevelTicking = false};
			level.Initialize();

			var centre = new ChunkCoordinates(0, 0);
			var held = new Dictionary<ChunkCoordinates, long>();

			var first = level.GenerateChunks(centre, held, 0).ToList();
			Assert.AreEqual(1, first.Count);
			Assert.IsNotNull(first[0].Chunk, "a column the player does not hold is pushed");
			first[0].Chunk.PutPool();

			long sentVersion = level.GetChunk(centre).Version;
			level.SetBlock(new Stone {Coordinates = new BlockCoordinates(3, 10, 3)});
			Assert.AreNotEqual(sentVersion, level.GetChunk(centre).Version, "the write moved the column's version");

			var second = level.GenerateChunks(centre, held, 0).ToList();
			Assert.AreEqual(0, second.Count, "a held column is not pushed again for having changed");
			Assert.IsTrue(held.ContainsKey(centre), "and it stays held");
		}
	}
}