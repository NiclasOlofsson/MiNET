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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Entities;
using MiNET.Entities.Behaviors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Path = System.IO.Path;

namespace MiNET.Test
{
	[TestClass, Ignore("Manual code generation")]
	public class GenerateMobsTests
	{
		Dictionary<string, CodeTypeDeclaration> _declaredTypes = new Dictionary<string, CodeTypeDeclaration>();

		[TestMethod]
		public void ReadMobDataFromJson()
		{
			var unit = new CodeCompileUnit();

			unit.Namespaces.Add(new CodeNamespace {Imports = {new CodeNamespaceImport("MiNET.Entities.Behaviors")}});

			var ns = new CodeNamespace("MiNET.Generated");
			ns.Comments.Add(new CodeCommentStatement("Types generated from bedrock component JSON"));
			unit.Namespaces.Add(ns);

			var files = Directory.EnumerateFileSystemEntries(@"D:\Downloads\bedrock-server-1.11.4.2\behavior_packs\vanilla\entities\", "*.json");
			foreach (var file in files)
			{
				CreateEntity(ns, file);
			}

			CodeTypeDeclarationCollection types = ns.Types;
			CodeTypeDeclaration[] t = new CodeTypeDeclaration[types.Count];
			types.CopyTo(t, 0);
			ns.Types.Clear();

			var g = t.GroupBy(ctd => ctd.Name);

			foreach (var gg in g)
			{
				ns.Types.Add(gg.First());
			}

			GenerateCSharpCode(unit);
		}

		private void CreateEntity(CodeNamespace ns, string fileName)
		{
			using (Stream stream = File.OpenRead(fileName))
			using (var reader = new JsonTextReader(new StreamReader(stream)))
			{
				dynamic input = JObject.Load(reader);
				var mob = input["minecraft:entity"];

				string description = mob.description.identifier;

				// Generate class for entity
				string typeName = CodeTypeName(description.Replace("minecraft:", "").Replace(".", "_"));
				var csClass = new CodeTypeDeclaration(typeName);
				csClass.Comments.Add(new CodeCommentStatement(description, true));

				csClass.BaseTypes.Add(typeof(Entity));
				ns.Types.Add(csClass);

				JObject identifier = mob.description;
				foreach (var jToken in identifier.Properties())
				{
					Console.WriteLine(jToken);
					AutoPropertyFromJToken(jToken, csClass);
				}

				JObject components = mob.components;
				foreach (var pair in components)
				{
					string componentName = pair.Key;

					var comment = new CodeCommentStatement(componentName);
					csClass.Members[0].Comments.Add(comment);

					if (componentName.StartsWith("minecraft:behavior."))
					{
						string name = CodeTypeName(componentName.Replace("minecraft:behavior.", "").Replace(".", "_")) + "Behavior";

						if (!_declaredTypes.TryGetValue(name, out CodeTypeDeclaration csComponentClass))
						{
							csComponentClass = new CodeTypeDeclaration(name);
							csComponentClass.BaseTypes.Add(typeof(BehaviorBase));
							ns.Types.Add(csComponentClass);
							_declaredTypes.Add(name, csComponentClass);
						}

						foreach (JToken jToken in pair.Value)
						{
							AutoPropertyFromJToken(jToken, csComponentClass);
						}
					}
					else
					{
						string name = CodeTypeName(componentName.Replace("minecraft:", "").Replace(".", "_")) + "Component";

						if (!_declaredTypes.TryGetValue(name, out CodeTypeDeclaration csComponentClass))
						{
							csComponentClass = new CodeTypeDeclaration(name);
							csComponentClass.BaseTypes.Add("ComponentBase");
							ns.Types.Add(csComponentClass);
							_declaredTypes.Add(name, csComponentClass);
						}

						foreach (JToken jToken in pair.Value)
						{
							AutoPropertyFromJToken(jToken, csComponentClass);
						}
					}
				}
			}
		}

		private void AutoPropertyFromJToken(JToken jToken, CodeTypeDeclaration csClass)
		{
			if (jToken.Type == JTokenType.Property)
			{
				var prop = jToken.ToObject<JProperty>();

				var autoProp = new CodeSnippetTypeMember();
				//string autoPropText = "\t\tpublic {0} " + CodeName(prop.Name, true) + " {{ get; set; }} = {1};\n";
				string autoPropText = "\t\tpublic {0} " + CodeName(prop.Name, true) + " {{ get; set; }}\n";

				switch (prop.Value.Type)
				{
					case JTokenType.Integer:
						autoProp.Text = string.Format(autoPropText, "int", prop.Value);
						break;
					case JTokenType.Float:
						autoProp.Text = string.Format(autoPropText, "float", prop.Value);
						break;
					case JTokenType.String:
						autoProp.Text = string.Format(autoPropText, "string", "\"" + prop.Value + "\"");
						break;
					case JTokenType.Boolean:
						autoProp.Text = string.Format(autoPropText, "bool", ((string) prop.Value).ToLower());
						break;
					case JTokenType.None:
						Console.WriteLine($"{prop.Name} has no type");
						break;
					case JTokenType.Null:
						Console.WriteLine($"{prop.Name} type is null");
						break;
					case JTokenType.Undefined:
						Console.WriteLine($"{prop.Name} type is undefined");
						break;
					case JTokenType.Property:
					case JTokenType.Constructor:
					case JTokenType.Date:
					case JTokenType.Raw:
					case JTokenType.Bytes:
					case JTokenType.Guid:
					case JTokenType.Uri:
					case JTokenType.TimeSpan:
					case JTokenType.Object:
					case JTokenType.Array:
					case JTokenType.Comment:
						Console.WriteLine($"{prop.Name} type (is {prop.Value.Type}) can't be handled right now");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}


				foreach (CodeTypeMember codeTypeMember in csClass.Members)
				{
					if (codeTypeMember is CodeSnippetTypeMember member)
					{
						if (member.Text == autoProp.Text)
							return;
					}
				}

				csClass.Members.Add(autoProp);
			}
		}

		private void AddCodeMemberProperty(CodeTypeMember theClass)
		{
		}

		private string UpperInitial(string name)
		{
			return name[0].ToString().ToUpperInvariant() + name.Substring(1).Replace(@"[]", "s");
		}

		private string CodeTypeName(string name)
		{
			if (name.StartsWith("ID_"))
			{
				name = name.Substring(3);
			}
			return CodeName(name, true);
		}

		private string CodeFieldName(string name, bool isPrivate = false)
		{
			return (isPrivate ? "_" : "") + CodeName(name, false);
		}

		private string CodeName(string name, bool firstUpper = false)
		{
			name = name.ToLowerInvariant();

			string result = name;
			bool upperCase = firstUpper;

			result = string.Empty;
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

		[TestMethod]
		public void ReadAndGenerateCodeForMobs()
		{
			var unit = new CodeCompileUnit();

			var ns = new CodeNamespace("MiNET.Generated");
			unit.Namespaces.Add(ns);
			ns.Comments.Add(new CodeCommentStatement("Test"));


			var aClass = new CodeTypeDeclaration("MyClass");
			aClass.Members.Add(new CodeConstructor());

			var method = new CodeMemberMethod() {Name = "Test"};
			aClass.Members.Add(method);

			var start = new CodeEntryPointMethod();
			var cs1 = new CodeMethodInvokeExpression(
				new CodeTypeReferenceExpression("System.Console"),
				"WriteLine", new CodePrimitiveExpression("Hello World!"));
			start.Statements.Add(cs1);
			aClass.Members.Add(start);

			ns.Types.Add(aClass);

			GenerateCSharpCode(unit);
		}


		private static void GenerateCSharpCode(CodeCompileUnit compileUnit)
		{
// Generate the code with the C# code provider.
			var provider = new CSharpCodeProvider();

			// Build the output file name.
			string sourceFile;
			if (provider.FileExtension[0] == '.')
			{
				sourceFile = "HelloWorld" + provider.FileExtension;
			}
			else
			{
				sourceFile = "HelloWorld." + provider.FileExtension;
			}

			// Create a TextWriter to a StreamWriter to the output file.
			using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetTempPath(), sourceFile), false))
			{
				var tw = new IndentedTextWriter(sw, "\t");

				// Generate source code using the code provider.
				var generatorOptions = new CodeGeneratorOptions();
				generatorOptions.IndentString = "\t";
				generatorOptions.BlankLinesBetweenMembers = false;

				provider.GenerateCodeFromCompileUnit(compileUnit, tw, generatorOptions);

				// Close the output file.
				tw.Close();
			}
		}
	}
}