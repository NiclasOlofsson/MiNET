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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using fNbt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;

namespace MiNET.Test.Net
{
	/// <summary>
	///     Locks the bytes every packet class encodes to. Each packet's declared fields are filled with
	///     deterministic values in declaration order, which is wire order for both the T4 and the
	///     ProtocolGen classes, then encoded and decoded again; the hex and the decoded values are
	///     compared with Data/packets/golden-bytes.txt. A generator change that shifts the bytes of an
	///     already generated packet fails here instead of on a real client. Moving a packet from the XML
	///     to the roster keeps its bytes, so its golden entry stays identical across the move even though
	///     the member names change. Set MINET_GOLDEN_UPDATE=1 to rewrite the file deliberately.
	/// </summary>
	[TestClass]
	public class GeneratedPacketBytesTests
	{
		private const string GoldenFile = "golden-bytes.txt";

		[TestMethod]
		public void Generated_packet_bytes_match_golden()
		{
			string actual = Render();

			if (Environment.GetEnvironmentVariable("MINET_GOLDEN_UPDATE") == "1")
			{
				string source = SourceGoldenPath();
				File.WriteAllText(source, actual);
				Assert.Inconclusive($"Golden file rewritten: {source}");
			}

			string goldenPath = Path.Combine(AppContext.BaseDirectory, "Data", "packets", GoldenFile);
			Assert.IsTrue(File.Exists(goldenPath), $"No golden file at {goldenPath}. Run once with MINET_GOLDEN_UPDATE=1.");
			// Git may check the file out with CRLF line endings (text=auto); the content is what counts.
			string expected = File.ReadAllText(goldenPath).Replace("\r\n", "\n");

			if (expected == actual) return;

			string[] e = expected.Split('\n'), a = actual.Split('\n');
			int line = Enumerable.Range(0, Math.Min(e.Length, a.Length)).FirstOrDefault(i => e[i] != a[i]);
			string packet = e.Take(line + 1).LastOrDefault(l => l.StartsWith("== ")) ?? "(before the first packet)";
			Assert.Fail($"Packet bytes differ from the golden file at line {line + 1}, in {packet}\nexpected: {Get(e, line)}\nactual:   {Get(a, line)}");
		}

		private static string Get(string[] lines, int i) => i < lines.Length ? lines[i] : "(end of file)";

		private static string Render()
		{
			var packetTypes = typeof(Packet).Assembly.GetTypes()
				.Where(t => t.Namespace == "MiNET.Net" && t.Name.StartsWith("Mcpe") && !t.IsAbstract && t != typeof(McpeWrapper))
				.Where(t => t.BaseType is {IsGenericType: true} b && b.GetGenericTypeDefinition() == typeof(Packet<>))
				.OrderBy(t => t.Name, StringComparer.Ordinal);

			var sb = new StringBuilder();
			foreach (Type type in packetTypes)
			{
				try
				{
					RenderPacket(sb, type);
				}
				catch (Exception ex)
				{
					Exception inner = Unwrap(ex);
					sb.Append($"== {type.Name}\n  SKIP {inner.GetType().Name} in {inner.TargetSite?.DeclaringType?.Name}.{inner.TargetSite?.Name}: {FirstLine(inner.Message)}\n");
				}
			}
			return sb.ToString();
		}

		private static void RenderPacket(StringBuilder sb, Type type)
		{
			var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			// Hand-written partials sometimes keep their codec state in properties; fill those too,
			// after the fields, so the generated fields keep their positions.
			var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
				.Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0 && p.SetMethod.IsPublic).ToArray();
			var packet = (Packet) Activator.CreateInstance(type);
			for (int i = 0; i < fields.Length; i++) fields[i].SetValue(packet, Sample(fields[i].FieldType, i + 1, 0));
			for (int i = 0; i < properties.Length; i++) properties[i].SetValue(packet, Sample(properties[i].PropertyType, fields.Length + i + 1, 0));

			byte[] bytes = packet.Encode();
			var text = new StringBuilder();
			text.Append($"== {type.Name} id=0x{packet.Id:x}\n  hex {Convert.ToHexString(bytes)}\n");

			var back = (Packet) Activator.CreateInstance(type);
			try
			{
				back.Decode(bytes);
				for (int i = 0; i < fields.Length; i++) text.Append($"  back[{i}] {Format(fields[i].GetValue(back), 0)}\n");
				for (int i = 0; i < properties.Length; i++) text.Append($"  back[{fields.Length + i}] {Format(properties[i].GetValue(back), 0)}\n");
			}
			catch (Exception ex)
			{
				text.Append($"  DECODE {Unwrap(ex).GetType().Name}: {FirstLine(Unwrap(ex).Message)}\n");
			}
			sb.Append(text);
		}

		// One value rule for every integral type and enum, small enough for all of them: a field that
		// changes from int to an enum (or byte to int) keeps its sample, so its bytes keep comparing.
		private static long Small(int i) => (i * 37 + 5) % 120 + 1;

		private static object Sample(Type type, int i, int depth)
		{
			Type nullable = Nullable.GetUnderlyingType(type);
			if (nullable != null) return Sample(nullable, i, depth);

			if (type.IsEnum) return Enum.ToObject(type, Small(i));
			if (type == typeof(bool)) return i % 2 == 1;
			if (type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
				|| type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong))
				return Convert.ChangeType(Small(i), type);
			if (type == typeof(float)) return 1.25f * i + 0.5f;
			if (type == typeof(double)) return 2.5 * i + 0.25;
			if (type == typeof(string)) return $"s{i}";
			if (type == typeof(byte[])) return new byte[] {0x00, 0x7f, 0x80, 0xff, (byte) i};
			if (type == typeof(Vector3)) return new Vector3(i, -i * 2.5f, i * 3.25f);
			if (type == typeof(Vector2)) return new Vector2(i * 1.5f, -i);
			if (type == typeof(BlockCoordinates)) return new BlockCoordinates(i * 17, -i * 3, i * 101);
			if (type == typeof(UUID)) return new UUID(Guid.Parse($"00000000-0000-0000-0000-{i:D12}").ToByteArray());
			if (type == typeof(NbtCompound)) return new NbtCompound("") {new NbtInt("v", i), new NbtString("s", $"x{i}")};
			// Network NBT as every real caller builds it: varint, little-endian. ReadNbt always reads it that way.
			if (type == typeof(Nbt)) return new Nbt {NbtFile = new NbtFile(new NbtCompound("") {new NbtInt("v", i), new NbtString("s", $"x{i}")}) {BigEndian = false, UseVarInt = true}};
			// UniqueId defaults to Environment.TickCount and travels as the stack network id.
			if (type == typeof(Item)) return new ItemStick {Count = (byte) (i % 63 + 1), UniqueId = (int) Small(i)};
			if (type == typeof(CommandOriginData)) return new CommandOriginData((CommandOriginType) 0, (UUID) Sample(typeof(UUID), i, depth), $"r{i}", Small(i));
			if (type == typeof(SubChunkHeightmapData))
			{
				// Heightmaps are fixed 16x16 grids on the wire.
				return new SubChunkHeightmapData
				{
					heightMapType = (SubChunkHeightmapData.HeightMapType) Small(i), heights = Enumerable.Range(0, 256).Select(b => (byte) (b + i)).ToArray(),
					renderHeightMapType = (SubChunkHeightmapData.RenderHeightMapType) Small(i + 1), renderHeights = Enumerable.Range(0, 256).Select(b => (byte) (255 - b)).ToArray()
				};
			}
			if (depth > 6) return type.IsValueType ? Activator.CreateInstance(type) : null;

			if (type.IsArray)
			{
				Array array = Array.CreateInstance(type.GetElementType(), 2);
				for (int j = 0; j < 2; j++) array.SetValue(Sample(type.GetElementType(), i * 10 + j + 1, depth + 1), j);
				return array;
			}

			if (type.IsGenericType && typeof(IList).IsAssignableFrom(type) && type.GetGenericArguments().Length == 1)
			{
				var list = (IList) Activator.CreateInstance(type);
				for (int j = 0; j < 2; j++) list.Add(Sample(type.GetGenericArguments()[0], i * 10 + j + 1, depth + 1));
				return list;
			}

			if (type.IsGenericType && typeof(IDictionary).IsAssignableFrom(type) && type.GetGenericArguments()[0] == typeof(string))
			{
				var map = (IDictionary) Activator.CreateInstance(type);
				map.Add($"k{i}", Sample(type.GetGenericArguments()[1], i * 10 + 1, depth + 1));
				return map;
			}

			if (type.IsAbstract)
			{
				// A tagged variant's base: the first concrete option by name, so the choice is stable.
				Type option = type.Assembly.GetTypes().Where(t => !t.IsAbstract && type.IsAssignableFrom(t) && t.GetConstructor(Type.EmptyTypes) != null)
					.OrderBy(t => t.Name, StringComparer.Ordinal).FirstOrDefault();
				return option == null ? null : Sample(option, i, depth);
			}

			// A struct has no parameterless constructor to find, but it always has a default instance to
			// fill; leaving it at zero would hide its element shape.
			if (type.GetConstructor(Type.EmptyTypes) == null && !type.IsValueType) return null;

			object o = Activator.CreateInstance(type);
			int k = i * 10;
			foreach (FieldInfo f in type.GetFields(BindingFlags.Public | BindingFlags.Instance)) f.SetValue(o, Sample(f.FieldType, ++k, depth + 1));
			foreach (PropertyInfo p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0 && p.SetMethod.IsPublic))
				p.SetValue(o, Sample(p.PropertyType, ++k, depth + 1));
			return o;
		}

		private static string Format(object value, int depth)
		{
			switch (value)
			{
				case null: return "null";
				case Enum e: return Convert.ToInt64(e).ToString();
				case string s: return s;
				case byte[] b: return Convert.ToHexString(b);
				case float f: return f.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
				case double d: return d.ToString("R", System.Globalization.CultureInfo.InvariantCulture);
				case Vector3 v: return $"({Format(v.X, 0)},{Format(v.Y, 0)},{Format(v.Z, 0)})";
				case Vector2 v: return $"({Format(v.X, 0)},{Format(v.Y, 0)})";
				case BlockCoordinates c: return $"({c.X},{c.Y},{c.Z})";
				case Nbt n: return n.NbtFile?.RootTag?.ToString().Replace("\n", " ").Replace("\r", "") ?? "nbt-null";
				case NbtTag t: return t.ToString().Replace("\n", " ").Replace("\r", "");
				case Item item: return $"item({item.Name},{item.Count},{item.Metadata})";
				case IConvertible c when value.GetType().IsPrimitive: return Convert.ToString(c, System.Globalization.CultureInfo.InvariantCulture);
				case IEnumerable seq when depth < 4: return "[" + string.Join(",", seq.Cast<object>().Select(x => Format(x, depth + 1))) + "]";
			}

			Type type = value.GetType();
			if (depth >= 4 || type.Namespace?.StartsWith("MiNET") != true) return value.ToString();
			var parts = type.GetFields(BindingFlags.Public | BindingFlags.Instance).Select(f => Format(f.GetValue(value), depth + 1))
				.Concat(type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0).Select(p => Format(p.GetValue(value), depth + 1)));
			return "{" + string.Join(",", parts) + "}";
		}

		private static Exception Unwrap(Exception ex) => ex is TargetInvocationException {InnerException: not null} t ? t.InnerException : ex;

		private static string FirstLine(string s) => s?.Split('\n')[0].Trim() ?? "";

		private static string SourceGoldenPath()
		{
			// The test project is either above the output directory or, for a build redirected
			// elsewhere in the repository, at its usual place under the repository root.
			string project = null;
			for (var dir = new DirectoryInfo(AppContext.BaseDirectory); dir != null && project == null; dir = dir.Parent)
			{
				if (File.Exists(Path.Combine(dir.FullName, "MiNETTests.csproj"))) project = dir.FullName;
				else if (File.Exists(Path.Combine(dir.FullName, "src", "MiNET", "MiNET.Test", "MiNETTests.csproj"))) project = Path.Combine(dir.FullName, "src", "MiNET", "MiNET.Test");
			}
			if (project == null) throw new DirectoryNotFoundException("MiNETTests.csproj not found from the test output directory");
			string path = Path.Combine(project, "Data", "packets", GoldenFile);
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			return path;
		}
	}
}
