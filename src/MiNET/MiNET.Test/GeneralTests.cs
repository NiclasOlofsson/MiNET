using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using Newtonsoft.Json.Linq;

namespace MiNET.Test
{
	[TestClass]
	public class GeneralTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralTests));

		[TestMethod]
		public void GenerateClassesForBlocks()
		{

			Dictionary<int, Blockstate> blockstates = new Dictionary<int, Blockstate>();

			var assembly = Assembly.GetAssembly(typeof(Block));
			using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				dynamic jsonBlockstates = JArray.Parse(reader.ReadToEnd());

				int runtimeId = 0;
				foreach (var obj in jsonBlockstates)
				{
					try
					{
						blockstates.Add(runtimeId, new Blockstate() {Id = (int) obj.id, Data = (short) obj.data, Name = (string) obj.name, RuntimeId = runtimeId});
						runtimeId++;
					}
					catch (Exception e)
					{
						Console.WriteLine($"{obj}");
						throw;
					}
				}

			}


			List<(int, string)> blocks = new List<(int, string)>();

			string fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using (FileStream file = File.OpenWrite(fileName))
			{
				Log.Warn($"Writing new blocks to filename:\n{fileName}");

				IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(file));

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;


				foreach (IGrouping<string, KeyValuePair<int, Blockstate>> blockstate in blockstates.OrderBy(kvp => kvp.Value.Name).ThenBy(kvp => kvp.Value.Data).GroupBy(kvp => kvp.Value.Name))
				{
					var enumerator = blockstate.GetEnumerator();
					enumerator.MoveNext();
					var value = enumerator.Current.Value;
					if (value == null) continue;
					Log.Debug($"{value.RuntimeId}, {value.Name}, {value.Data}");
					int id = BlockFactory.GetBlockIdByName(value.Name.Replace("minecraft:", ""));

					if (id == 0 && !value.Name.Contains("air"))
					{
						string blockName = CodeName(value.Name.Replace("minecraft:", ""), true);

						blocks.Add((value.Id, blockName));

						writer.WriteLine($"public class {blockName}: Block");
						writer.WriteLine($"{{");
						writer.Indent++;

						writer.WriteLine($"");
						writer.WriteLine($"public string Name {{ get; set; }} = \"{value.Name}\";");
						writer.WriteLine($"");

						writer.WriteLine($"public {blockName}() : base({value.Id})");
						writer.WriteLine($"{{");
						writer.Indent++;

						do
						{
							writer.WriteLine($"// runtime id: {enumerator.Current.Value.RuntimeId} 0x{enumerator.Current.Value.RuntimeId:X}, data: {enumerator.Current.Value.Data}");
						} while (enumerator.MoveNext());

						writer.Indent--;
						writer.WriteLine($"}}");

						writer.Indent--;
						writer.WriteLine($"}}");
					}
				}
				writer.Indent--;
				writer.WriteLine($"}}");

				foreach (var block in blocks.OrderBy(tuple => tuple.Item1))
				{
					writer.WriteLine($"else if (blockId == {block.Item1}) block = new {block.Item2}();");
				}

				writer.Flush();
			}


		}


		private string CodeName(string name, bool firstUpper = false)
		{
			name = name.ToLowerInvariant();

			bool upperCase = firstUpper;

			var result = string.Empty;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == ' ' || name[i] == '_')
				{
					upperCase = true;
				}
				else
				{
					if ((i == 0 && firstUpper) || upperCase)
					{
						result += name[i].ToString().ToUpperInvariant();
						upperCase = false;
					}
					else
					{
						result += name[i];
					}
				}
			}

			result = result.Replace(@"[]", "s");
			return result;
		}

	}
}