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

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds.Tests
{
	[TestClass()]
	public class LevelDbProviderTests
	{
		[TestMethod()]
		public void RoundtripTest()
		{
			var provider = new LevelDbProvider();
			var flatGenerator = new SuperflatGenerator(Dimension.Overworld);
			flatGenerator.Initialize(null);
			SubChunk chunk = flatGenerator.GenerateChunkColumn(new ChunkCoordinates())[0];

			// -4 is the bottom section of a 1.18+ world, and the case the old writer got wrong:
			// it wrote version 8 with an unsigned index, which Bedrock treats as an outdated
			// chunk and puts through its upgrade path, rewriting the block states we stored.
			const int sectionY = -4;

			using var stream = new MemoryStream();
			provider.Write(chunk, stream, sectionY);
			byte[] output = stream.ToArray();

			Assert.AreEqual(9, output[0], "subchunk record must declare version 9");
			Assert.AreEqual(unchecked((byte) (sbyte) sectionY), output[2], "version 9 stores its own signed section index");

			var parsedChunk = new SubChunk();
			provider.ParseSection(parsedChunk, output);

			// Assert
			CollectionAssert.AreEqual(chunk.Blocks.ToArray(), parsedChunk.Blocks.ToArray());
			CollectionAssert.AreEqual(chunk.LoggedBlocks.ToArray(), parsedChunk.LoggedBlocks.ToArray());
			CollectionAssert.AreEqual(chunk.RuntimeIds, parsedChunk.RuntimeIds);
		}
	}
}