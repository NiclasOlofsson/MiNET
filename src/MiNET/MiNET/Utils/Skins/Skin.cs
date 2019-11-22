﻿#region LICENSE

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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace MiNET.Utils.Skins
{
	public class Skin : ICloneable
	{
		public bool Slim { get; set; }
		public bool IsPersonaSkin { get; set; }
		public bool IsPremiumSkin { get; set; }

		public Cape Cape { get; set; }
		public string SkinId { get; set; }
		public string ResourcePatch { get; set; }  // contains GeometryName
		public int Height { get; set; }
		public int Width { get; set; }
		public byte[] Data { get; set; }
		public string GeometryName { get; set; }
		public string GeometryData { get; set; }
		public string AnimationData { get; set; }
		public List<Animation> Animations { get; set; }

		public Skin()
		{
			Cape = new Cape();
			Animations = new List<Animation>();
		}

		public static byte[] GetTextureFromFile(string filename)
		{
			Bitmap bitmap = new Bitmap(filename);

			var size = bitmap.Height * bitmap.Width * 4;

			if (size != 0x2000 && size != 0x4000 && size != 0x10000) return null;

			byte[] bytes = new byte[size];

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					Color color = bitmap.GetPixel(x, y);
					bytes[i++] = color.R;
					bytes[i++] = color.G;
					bytes[i++] = color.B;
					bytes[i++] = color.A;
				}
			}

			return bytes;
		}

		public static void SaveTextureToFile(string filename, byte[] bytes)
		{
			var size = bytes.Length;

			int width = size == 0x10000 ? 128 : 64;
			var height = size == 0x2000 ? 32 : (size == 0x4000 ? 64 : 128);

			Bitmap bitmap = new Bitmap(width, height);

			int i = 0;
			for (int y = 0; y < bitmap.Height; y++)
			{
				for (int x = 0; x < bitmap.Width; x++)
				{
					byte r = bytes[i++];
					byte g = bytes[i++];
					byte b = bytes[i++];
					byte a = bytes[i++];

					Color color = Color.FromArgb(a, r, g, b);
					bitmap.SetPixel(x, y, color);
				}
			}

			bitmap.Save(filename, ImageFormat.Png);
		}

		public static GeometryModel ParseGeometry(string path)
		{
			string json = File.ReadAllText(path);
			return Parse(json);
		}

		public static GeometryModel Parse(string json)
		{
			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			return JsonConvert.DeserializeObject<GeometryModel>(json, settings);
		}

		public static string ToJson(GeometryModel geometryModel)
		{
			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			//settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			settings.Converters.Add(new StringEnumConverter {CamelCaseText = true});

			return JsonConvert.SerializeObject(geometryModel, settings);
		}

		public object Clone()
		{
			byte[] clonedSkinData = null;

			if (Data != null)
			{
				clonedSkinData = new byte[Data.Length];
				Data.CopyTo(clonedSkinData, 0);
			}

			var clonedSkin = new Skin
			{
				SkinId = SkinId,
				GeometryData = GeometryData,
				Slim = Slim,
				Data = clonedSkinData,
				Cape = Cape == null ? null : (Cape)Cape.Clone(),
				IsPersonaSkin = IsPersonaSkin,
				IsPremiumSkin = IsPremiumSkin,
				ResourcePatch = ResourcePatch,
				Height = Height,
				Width = Width,
				GeometryName = GeometryName,
				AnimationData = AnimationData,
			};

			foreach (Animation animation in Animations)
			{
				clonedSkin.Animations.Add((Animation)animation.Clone());
			}

			return clonedSkin;
		}
	}
}