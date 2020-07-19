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

using System;
using System.IO;
using fNbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiNET.Worlds.Tests
{
	[TestClass
	, Ignore("Manual tests")
	]
	public class LevelInfoBedrockTests
	{
		[TestMethod]
		public void ToNbtTest()
		{
			var levelInfo = new LevelInfoBedrock();
			levelInfo.GameType = 1;
			levelInfo.Generator = 1;
			levelInfo.LastPlayed = 1594911609;
			levelInfo.LevelName = "BedrockGeneratedLevel";
			levelInfo.Platform = 2;
			levelInfo.RandomSeed = 3429004588;
			levelInfo.SpawnX = 44;
			levelInfo.SpawnY = 32767;
			levelInfo.SpawnZ = 4;
			levelInfo.Time = 269000;
			levelInfo.SpawnMobs = 1;

			NbtTag nbt = levelInfo.Serialize();
			Console.WriteLine(nbt);
			Assert.IsNotNull(nbt);
			Assert.IsInstanceOfType(nbt, typeof(NbtCompound));
			Assert.AreEqual(levelInfo.GameType, nbt["GameType"].IntValue);
			Assert.AreEqual(levelInfo.Generator, nbt["Generator"].IntValue);
			Assert.AreEqual(levelInfo.LevelName, nbt["LevelName"].StringValue);

			var file = new NbtFile
			{
				BigEndian = false,
				UseVarInt = false,
			};
			file.RootTag = nbt;
			var bytes = file.SaveToBuffer(NbtCompression.None);

			using FileStream stream = File.Create(@"C:\Temp\TrashBedrockWorld\level_test_generated.dat");
			stream.Write(new ReadOnlySpan<byte>(new byte[] {0x08, 0, 0, 0}));
			stream.Write(BitConverter.GetBytes(bytes.Length));
			stream.Write(bytes);
			stream.Flush();
		}

		[TestMethod]
		public void FromNbtTest()
		{
			var file = new NbtFile
			{
				BigEndian = false,
				UseVarInt = false
			};
			//var levelStream = File.OpenRead(@"C:\Temp\TrashBedrockWorld\level.dat");
			using FileStream stream = File.OpenRead(@"C:\Temp\TrashBedrockWorld\level_test_generated.dat");
			var header = new Span<byte>(new byte[4]);
			stream.Read(header);
			Console.WriteLine($"File version:{BitConverter.ToInt32(header)}");
			stream.Seek(4, SeekOrigin.Current);
			file.LoadFromStream(stream, NbtCompression.None);

			var levelInfo = LevelInfoBedrock.FromNbt(file.RootTag);
			Assert.IsNotNull(levelInfo);

			//public int GameType { get; set; }
			Assert.AreEqual(1, levelInfo.GameType);
			//public int Generator { get; set; }
			Assert.AreEqual(1, levelInfo.Generator);
			//public long LastPlayed { get; set; }
			Assert.AreEqual(1594911609, levelInfo.LastPlayed);
			//public string LevelName { get; set; }
			Assert.AreEqual("BedrockGeneratedLevel", levelInfo.LevelName);
			//public int Platform { get; set; }
			Assert.AreEqual(2, levelInfo.Platform);
			//public long RandomSeed { get; set; }
			Assert.AreEqual(3429004588, levelInfo.RandomSeed);
			//public int SpawnX { get; set; }
			Assert.AreEqual(44, levelInfo.SpawnX);
			//public int SpawnY { get; set; }
			Assert.AreEqual(32767, levelInfo.SpawnY);
			////public int SpawnZ { get; set; }
			Assert.AreEqual(4, levelInfo.SpawnZ);
			////public long Time { get; set; }
			Assert.AreEqual(269000, levelInfo.Time);
			////public long SpawnMobs { get; set; }
			Assert.AreEqual(1, levelInfo.SpawnMobs);
		}

		[TestMethod]
		public void GenerateLevelInfoPropertiesTest()
		{
			var file = new NbtFile
			{
				BigEndian = false,
				UseVarInt = false
			};
			using FileStream stream = File.OpenRead(@"C:\Temp\TrashBedrockWorld\level.dat");
			stream.Seek(8, SeekOrigin.Begin);
			file.LoadFromStream(stream, NbtCompression.None);

			foreach (NbtTag tag in (NbtCompound) file.RootTag)
			{
				NbtTagType tagTagType = tag.TagType;
				string type = tagTagType switch
				{
					NbtTagType.Int => "int",
					NbtTagType.Byte => "byte",
					NbtTagType.Short => "short",
					NbtTagType.Long => "long",
					NbtTagType.Float => "float",
					NbtTagType.Double => "double",
					NbtTagType.ByteArray => "byte[]",
					NbtTagType.String => "string",
					//NbtTagType.List => throw new NotImplementedException(),
					//NbtTagType.Compound => throw new NotImplementedException(),
					NbtTagType.IntArray => "int[]",
					NbtTagType.LongArray => "long[]",
					_ => null
				};


				string comment = "";
				if (type == null) comment = "//";

				string attribute = "";

				string name = UppercaseFirst(tag.Name);
				if (name != tag.Name) attribute = $"[JsonPropertyName(\"{tag.Name}\")] ";

				Console.WriteLine($"\t\t{comment}{attribute}public {type} {name} {{ get; set; }}");
			}
		}

		private static string UppercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToUpper(s[0]) + s.Substring(1);
		}
	}
}