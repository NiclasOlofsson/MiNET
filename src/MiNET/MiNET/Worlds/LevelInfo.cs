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
using System.Linq.Expressions;
using System.Reflection;
using fNbt;

namespace MiNET.Worlds
{
	public class LevelInfo : ICloneable
	{
		public LevelInfo()
		{
		}

		public LevelInfo(NbtTag dataTag)
		{
			LoadFromNbt(dataTag);
		}

		public int DataVersion { get; set; }
		public int Version { get; set; }
		public bool Initialized { get; set; }
		public string LevelName { get; set; }
		public string GeneratorName { get; set; }
		public int GeneratorVersion { get; set; }
		public string GeneratorOptions { get; set; }
		public long RandomSeed { get; set; }
		public bool MapFeatures { get; set; }
		public long LastPlayed { get; set; }
		public bool AllowCommands { get; set; }
		public bool Hardcore { get; set; }
		public int GameType { get; set; }
		public long Time { get; set; }
		public long DayTime { get; set; }
		public int SpawnX { get; set; }
		public int SpawnY { get; set; }
		public int SpawnZ { get; set; }
		public bool Raining { get; set; }
		public int RainTime { get; set; }
		public bool Thundering { get; set; }
		public int ThunderTime { get; set; }

		public T GetPropertyValue<T>(NbtTag tag, Expression<Func<T>> property)
		{
			var propertyInfo = ((MemberExpression) property.Body).Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}

			NbtTag nbtTag = tag[propertyInfo.Name];
			if (nbtTag == null)
			{
				nbtTag = tag[LowercaseFirst(propertyInfo.Name)];
			}

			if (nbtTag == null) return default(T);

			var mex = property.Body as MemberExpression;
			var target = Expression.Lambda(mex.Expression).Compile().DynamicInvoke();

			switch (nbtTag.TagType)
			{
				case NbtTagType.Unknown:
					break;
				case NbtTagType.End:
					break;
				case NbtTagType.Byte:
					if (propertyInfo.PropertyType == typeof(bool)) propertyInfo.SetValue(target, nbtTag.ByteValue == 1);
					else propertyInfo.SetValue(target, nbtTag.ByteValue);
					break;
				case NbtTagType.Short:
					propertyInfo.SetValue(target, nbtTag.ShortValue);
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof(bool)) propertyInfo.SetValue(target, nbtTag.IntValue == 1);
					else propertyInfo.SetValue(target, nbtTag.IntValue);
					break;
				case NbtTagType.Long:
					propertyInfo.SetValue(target, nbtTag.LongValue);
					break;
				case NbtTagType.Float:
					propertyInfo.SetValue(target, nbtTag.FloatValue);
					break;
				case NbtTagType.Double:
					propertyInfo.SetValue(target, nbtTag.DoubleValue);
					break;
				case NbtTagType.ByteArray:
					propertyInfo.SetValue(target, nbtTag.ByteArrayValue);
					break;
				case NbtTagType.String:
					propertyInfo.SetValue(target, nbtTag.StringValue);
					break;
				case NbtTagType.List:
					break;
				case NbtTagType.Compound:
					break;
				case NbtTagType.IntArray:
					propertyInfo.SetValue(target, nbtTag.IntArrayValue);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return (T) propertyInfo.GetValue(target);
		}

		public void SetPropertyValue<T>(NbtTag tag, Expression<Func<T>> property, bool upperFirst = true)
		{
			var propertyInfo = ((MemberExpression) property.Body).Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}

			NbtTag nbtTag = tag[propertyInfo.Name];
			if (nbtTag == null)
			{
				nbtTag = tag[LowercaseFirst(propertyInfo.Name)];
			}

			if (nbtTag == null)
			{
				if (propertyInfo.PropertyType == typeof(bool))
				{
					nbtTag = new NbtByte(propertyInfo.Name);
				}
				else if (propertyInfo.PropertyType == typeof(byte))
				{
					nbtTag = new NbtByte(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(short))
				{
					nbtTag = new NbtShort(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(int))
				{
					nbtTag = new NbtInt(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(long))
				{
					nbtTag = new NbtLong(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(float))
				{
					nbtTag = new NbtFloat(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(double))
				{
					nbtTag = new NbtDouble(LowercaseFirst(propertyInfo.Name));
				}
				else if (propertyInfo.PropertyType == typeof(string))
				{
					nbtTag = new NbtString(LowercaseFirst(propertyInfo.Name), "");
				}
				else
				{
					return;
				}
			}

			var mex = property.Body as MemberExpression;
			var target = Expression.Lambda(mex.Expression).Compile().DynamicInvoke();

			switch (nbtTag.TagType)
			{
				case NbtTagType.Unknown:
					break;
				case NbtTagType.End:
					break;
				case NbtTagType.Byte:
					if (propertyInfo.PropertyType == typeof(bool))
						tag[nbtTag.Name] = new NbtByte(nbtTag.Name, (byte) ((bool) propertyInfo.GetValue(target) ? 1 : 0));
					else
						tag[nbtTag.Name] = new NbtByte(nbtTag.Name, (byte) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Short:
					tag[nbtTag.Name] = new NbtShort(nbtTag.Name, (short) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof(bool))
						tag[nbtTag.Name] = new NbtInt(nbtTag.Name, (bool) propertyInfo.GetValue(target) ? 1 : 0);
					else
						tag[nbtTag.Name] = new NbtInt(nbtTag.Name, (int) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Long:
					tag[nbtTag.Name] = new NbtLong(nbtTag.Name, (long) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Float:
					tag[nbtTag.Name] = new NbtFloat(nbtTag.Name, (float) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Double:
					tag[nbtTag.Name] = new NbtDouble(nbtTag.Name, (double) propertyInfo.GetValue(target));
					break;
				case NbtTagType.ByteArray:
					tag[nbtTag.Name] = new NbtByteArray(nbtTag.Name, (byte[]) propertyInfo.GetValue(target));
					break;
				case NbtTagType.String:
					tag[nbtTag.Name] = new NbtString(nbtTag.Name, (string) propertyInfo.GetValue(target) ?? "");
					break;
				case NbtTagType.List:
					break;
				case NbtTagType.Compound:
					break;
				case NbtTagType.IntArray:
					tag[nbtTag.Name] = new NbtIntArray(nbtTag.Name, (int[]) propertyInfo.GetValue(target));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			//return (T) propertyInfo.GetValue(target);
		}


		private static string LowercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToLower(s[0]) + s.Substring(1);
		}

		public void LoadFromNbt(NbtTag dataTag)
		{
			GetPropertyValue(dataTag, () => DataVersion);
			GetPropertyValue(dataTag, () => Version);
			GetPropertyValue(dataTag, () => Initialized);
			GetPropertyValue(dataTag, () => LevelName);
			GetPropertyValue(dataTag, () => GeneratorName);
			GetPropertyValue(dataTag, () => GeneratorVersion);
			GetPropertyValue(dataTag, () => GeneratorOptions);
			GetPropertyValue(dataTag, () => RandomSeed);
			GetPropertyValue(dataTag, () => MapFeatures);
			GetPropertyValue(dataTag, () => LastPlayed);
			GetPropertyValue(dataTag, () => AllowCommands);
			GetPropertyValue(dataTag, () => Hardcore);
			GetPropertyValue(dataTag, () => GameType);
			GetPropertyValue(dataTag, () => Time);
			GetPropertyValue(dataTag, () => DayTime);
			GetPropertyValue(dataTag, () => SpawnX);
			GetPropertyValue(dataTag, () => SpawnY);
			GetPropertyValue(dataTag, () => SpawnZ);
			GetPropertyValue(dataTag, () => Raining);
			GetPropertyValue(dataTag, () => RainTime);
			GetPropertyValue(dataTag, () => Thundering);
			GetPropertyValue(dataTag, () => ThunderTime);
		}

		public void SaveToNbt(NbtTag dataTag)
		{
			SetPropertyValue(dataTag, () => Version);
			SetPropertyValue(dataTag, () => Initialized);
			SetPropertyValue(dataTag, () => LevelName);
			SetPropertyValue(dataTag, () => GeneratorName);
			SetPropertyValue(dataTag, () => GeneratorVersion);
			SetPropertyValue(dataTag, () => GeneratorOptions);
			SetPropertyValue(dataTag, () => RandomSeed);
			SetPropertyValue(dataTag, () => MapFeatures);
			SetPropertyValue(dataTag, () => LastPlayed);
			SetPropertyValue(dataTag, () => AllowCommands);
			SetPropertyValue(dataTag, () => Hardcore);
			SetPropertyValue(dataTag, () => GameType);
			SetPropertyValue(dataTag, () => Time);
			SetPropertyValue(dataTag, () => DayTime);
			SetPropertyValue(dataTag, () => SpawnX);
			SetPropertyValue(dataTag, () => SpawnY);
			SetPropertyValue(dataTag, () => SpawnZ);
			SetPropertyValue(dataTag, () => Raining);
			SetPropertyValue(dataTag, () => RainTime);
			SetPropertyValue(dataTag, () => Thundering);
			SetPropertyValue(dataTag, () => ThunderTime);
		}

		public object Clone()
		{
			return MemberwiseClone();
		}
	}
}