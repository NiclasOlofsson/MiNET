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
					if (propertyInfo.PropertyType == typeof (bool)) propertyInfo.SetValue(target, nbtTag.ByteValue == 1);
					else propertyInfo.SetValue(target, nbtTag.ByteValue);
					break;
				case NbtTagType.Short:
					propertyInfo.SetValue(target, nbtTag.ShortValue);
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof (bool)) propertyInfo.SetValue(target, nbtTag.IntValue == 1);
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

		public T SetPropertyValue<T>(NbtTag tag, Expression<Func<T>> property, bool upperFirst = true)
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
					if (propertyInfo.PropertyType == typeof (bool))
						tag[nbtTag.Name] = new NbtByte((byte) ((bool) propertyInfo.GetValue(target) ? 1 : 0));
					else
						tag[nbtTag.Name] = new NbtByte((byte) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Short:
					tag[nbtTag.Name] = new NbtShort((short) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof (bool))
						tag[nbtTag.Name] = new NbtInt((bool) propertyInfo.GetValue(target) ? 1 : 0);
					else
						tag[nbtTag.Name] = new NbtInt((int) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Long:
					tag[nbtTag.Name] = new NbtLong((long) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Float:
					tag[nbtTag.Name] = new NbtFloat((float) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Double:
					tag[nbtTag.Name] = new NbtDouble((double) propertyInfo.GetValue(target));
					break;
				case NbtTagType.ByteArray:
					tag[nbtTag.Name] = new NbtByteArray((byte[]) propertyInfo.GetValue(target));
					break;
				case NbtTagType.String:
					tag[nbtTag.Name] = new NbtString((string) propertyInfo.GetValue(target));
					break;
				case NbtTagType.List:
					break;
				case NbtTagType.Compound:
					break;
				case NbtTagType.IntArray:
					tag[nbtTag.Name] = new NbtIntArray((int[]) propertyInfo.GetValue(target));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return (T) propertyInfo.GetValue(target);
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