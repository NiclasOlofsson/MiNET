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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections;
using System.IO;
using System.Reflection;

namespace MiNET.Client
{
	public class ObjectDumper
	{
		public static void Write(object element)
		{
			Write(element, 0);
		}

		public static void Write(object element, int depth)
		{
			Write(element, depth, Console.Out);
		}

		public static void Write(object element, int depth, TextWriter log)
		{
			ObjectDumper dumper = new ObjectDumper(depth);
			dumper.writer = log;
			dumper.WriteObject(null, element);
		}

		private TextWriter writer;
		private int pos;
		private int level;
		private int depth;

		private ObjectDumper(int depth)
		{
			this.depth = depth;
		}

		private void Write(string s)
		{
			if (s != null)
			{
				writer.Write(s);
				pos += s.Length;
			}
		}

		private void WriteIndent()
		{
			for (int i = 0; i < level; i++) writer.Write("  ");
		}

		private void WriteLine()
		{
			writer.WriteLine();
			pos = 0;
		}

		private void WriteTab()
		{
			Write("\t");
			while (pos % 8 != 0) Write(" ");
		}

		private void WriteObject(string prefix, object element)
		{
			if (element == null || element is ValueType || element is string)
			{
				WriteIndent();
				Write(prefix);
				WriteValue(element);
				WriteLine();
			}
			else
			{
				IEnumerable enumerableElement = element as IEnumerable;
				if (enumerableElement != null)
				{
					foreach (object item in enumerableElement)
					{
						if (item is IEnumerable && !(item is string))
						{
							WriteIndent();
							Write(prefix);
							Write("...");
							WriteLine();
							if (level < depth)
							{
								level++;
								WriteObject(prefix, item);
								level--;
							}
						}
						else
						{
							WriteObject(prefix, item);
						}
					}
				}
				else
				{
					MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
					WriteIndent();
					Write(prefix);
					bool propWritten = false;
					foreach (MemberInfo m in members)
					{
						FieldInfo f = m as FieldInfo;
						PropertyInfo p = m as PropertyInfo;
						if (f != null || p != null)
						{
							if (propWritten)
							{
								WriteTab();
							}
							else
							{
								propWritten = true;
							}
							Write(m.Name);
							Write("=");
							Type t = f != null ? f.FieldType : p.PropertyType;
							if (t.IsValueType || t == typeof(string))
							{
								WriteValue(f != null ? f.GetValue(element) : p.GetValue(element, null));
							}
							else
							{
								if (typeof(IEnumerable).IsAssignableFrom(t))
								{
									Write("...");
								}
								else
								{
									Write("{ }");
								}
							}
						}
						if (propWritten) WriteLine();
					}
					if (propWritten) WriteLine();
					if (level < depth)
					{
						foreach (MemberInfo m in members)
						{
							FieldInfo f = m as FieldInfo;
							PropertyInfo p = m as PropertyInfo;
							if (f != null || p != null)
							{
								Type t = f != null ? f.FieldType : p.PropertyType;
								if (!(t.IsValueType || t == typeof(string)))
								{
									object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
									if (value != null)
									{
										level++;
										WriteObject(m.Name + ": ", value);
										level--;
									}
								}
							}
						}
					}
				}
			}
		}

		private void WriteValue(object o)
		{
			if (o == null)
			{
				Write("null");
			}
			else if (o is DateTime)
			{
				Write(((DateTime) o).ToShortDateString());
			}
			else if (o is ValueType || o is string)
			{
				Write(o.ToString());
			}
			else if (o is IEnumerable)
			{
				Write("...");
			}
			else
			{
				Write("{ }");
			}
		}
	}
}