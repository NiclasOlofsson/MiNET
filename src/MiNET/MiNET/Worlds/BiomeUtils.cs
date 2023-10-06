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

using System.Collections.Generic;
using System.Reflection;
using fNbt;
using MiNET.Net;
using MiNET.Utils;
using SixLabors.ImageSharp.PixelFormats;

namespace MiNET.Worlds
{
	public class Biome
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Rain { get; set; }
		public float Depth { get; set; }
		public float Downfall { get; set; }
		public float Height { get; set; }
		public float Temperature { get; set; }
		public float BlueSpores { get; set; }
		public float RedSpores { get; set; }
		public float Ash { get; set; }
		public float WhiteAsh { get; set; }
		public float WaterTransparency { get; set; }
		public RgbaVector WaterColor { get; set; }
	}

	public class BiomeUtils
	{
		static BiomeUtils()
		{
			var assembly = Assembly.GetAssembly(typeof(Biome));

			var biomeIdMap = ResourceUtil.ReadResource<Dictionary<string, int>>("biome_id_map.json", typeof(Biome), "Data");

			using (var stream = assembly.GetManifestResourceStream(typeof(Biome).Namespace + ".Data.biome_definitions.nbt"))
			{
				BiomesCache = Packet.ReadNbtCompound(stream, true);

				foreach (NbtCompound biomeTag in BiomesCache)
				{
					var biome = new Biome()
					{
						Name = biomeTag["name_hash"].StringValue,
						Rain = biomeTag["rain"].ByteValue == 1,
						Depth = biomeTag["depth"].FloatValue,
						Downfall = biomeTag["downfall"].FloatValue,
						Height = biomeTag["height"].FloatValue,
						Temperature = biomeTag["temperature"].FloatValue,
						BlueSpores = biomeTag["blue_spores"].FloatValue,
						RedSpores = biomeTag["red_spores"].FloatValue,
						Ash = biomeTag["ash"].FloatValue,
						WhiteAsh = biomeTag["white_ash"].FloatValue,
						WaterTransparency = biomeTag["waterTransparency"].FloatValue,
						WaterColor = new RgbaVector(
							biomeTag["waterColorR"].FloatValue, 
							biomeTag["waterColorG"].FloatValue,
							biomeTag["waterColorB"].FloatValue, 
							biomeTag["waterColorA"].FloatValue),
					};

					biome.Id = biomeIdMap[biome.Name];

					IdBiomeMap.Add(biome.Id, biome);
					NameBiomeMap.Add(biome.Name, biome);
				}
			}
		}

		public static Dictionary<int, Biome> IdBiomeMap { get; } = new Dictionary<int, Biome>();
		public static Dictionary<string, Biome> NameBiomeMap { get; } = new Dictionary<string, Biome>();
		public static NbtCompound BiomesCache { get; set; }

		private struct BiomeCorner
		{
			public int Red;
			public int Green;
			public int Blue;
		}

		//$c = self::interpolateColor(256, $x, $z, [0x47, 0xd0, 0x33], [0x6c, 0xb4, 0x93], [0xbf, 0xb6, 0x55], [0x80, 0xb4, 0x97]);

		//private static BiomeCorner[] PEgrassCorners = new BiomeCorner[3]
		//{
		//	new BiomeCorner {red = 0xbf, green = 0xb6, blue = 0x55}, // lower left, temperature starts at 1.0 on left
		//	new BiomeCorner {red = 0x80, green = 0xb4, blue = 0x97}, // lower right
		//	new BiomeCorner {red = 0x47, green = 0xd0, blue = 0x33} // upper left
		//};

		private static BiomeCorner[] grassCorners = new BiomeCorner[3]
		{
			new BiomeCorner
			{
				Red = 191,
				Green = 183,
				Blue = 85
			}, // lower left, temperature starts at 1.0 on left
			new BiomeCorner
			{
				Red = 128,
				Green = 180,
				Blue = 151
			}, // lower right
			new BiomeCorner
			{
				Red = 71,
				Green = 205,
				Blue = 51
			} // upper left
		};

		private static BiomeCorner[] foliageCorners = new BiomeCorner[3]
		{
			new BiomeCorner
			{
				Red = 174,
				Green = 164,
				Blue = 42
			}, // lower left, temperature starts at 1.0 on left
			new BiomeCorner
			{
				Red = 96,
				Green = 161,
				Blue = 123
			}, // lower right
			new BiomeCorner
			{
				Red = 26,
				Green = 191,
				Blue = 0
			} // upper left
		};

		public static float Clamp(float value, float min, float max)
		{
			return (value < min) ? min : (value > max) ? max : value;
		}

		// NOTE: elevation is number of meters above a height of 64. If elevation is < 64, pass in 0.
		private int BiomeColor(float temperature, float rainfall, int elevation, BiomeCorner[] corners)
		{
			// get UVs
			temperature = Clamp(temperature - elevation * 0.00166667f, 0.0f, 1.0f);
			// crank it up: temperature = clamp(temperature - (float)elevation*0.166667f,0.0f,1.0f);
			rainfall = Clamp(rainfall, 0.0f, 1.0f);
			rainfall *= temperature;

			// UV is essentially temperature, rainfall

			// lambda values for barycentric coordinates
			float[] lambda = new float[3];
			lambda[0] = temperature - rainfall;
			lambda[1] = 1.0f - temperature;
			lambda[2] = rainfall;

			float red = 0.0f, green = 0.0f, blue = 0.0f;
			for (int i = 0; i < 3; i++)
			{
				red += lambda[i] * corners[i].Red;
				green += lambda[i] * corners[i].Green;
				blue += lambda[i] * corners[i].Blue;
			}

			int r = (int) Clamp(red, 0.0f, 255.0f);
			int g = (int) Clamp(green, 0.0f, 255.0f);
			int b = (int) Clamp(blue, 0.0f, 255.0f);

			return (r << 16) | (g << 8) | b;
		}

		private int BiomeGrassColor(float temperature, float rainfall, int elevation)
		{
			return BiomeColor(temperature, rainfall, elevation, grassCorners);
		}

		private int BiomeFoliageColor(float temperature, float rainfall, int elevation)
		{
			return BiomeColor(temperature, rainfall, elevation, foliageCorners);
		}

		public void PrecomputeBiomeColors()
		{
			//for (int biome = 0; biome < IdBiomeMap.Count; biome++)
			//{
			//	IdBiomeMap[biome].Grass = ComputeBiomeColor(biome, 0, true);
			//	IdBiomeMap[biome].Foliage = ComputeBiomeColor(biome, 0, false);
			//}

			//var mesaGrass = GetBiome(37).grass;
			//var desertGrass = GetBiome(2).grass;
			//if(mesaGrass != desertGrass) throw new Exception("Mesa: " + mesaGrass + " Desert: " + desertGrass);
		}

		private const int FOREST_BIOME = 4;
		private const int SWAMPLAND_BIOME = 6;
		private const int FOREST_HILLS_BIOME = 18;
		private const int BIRCH_FOREST_BIOME = 27;
		private const int BIRCH_FOREST_HILLS_BIOME = 28;
		private const int ROOFED_FOREST_BIOME = 29;

		private const int MESA_BIOME = 37;
		private const int MESA_PLATEAU_F_BIOME = 38;
		private const int MESA_PLATEAU_BIOME = 39;


		// elevation == 0 means for precomputed colors and for elevation off
		// or 64 high or below. 
		public int ComputeBiomeColor(int biome, int elevation, bool isGrass)
		{
			int color;

			switch (biome)
			{
				case SWAMPLAND_BIOME:
					// the fefefe makes it so that carries are copied to the low bit,
					// then their magic "go to green" color offset is added in, then
					// divide by two gives a carry that will nicely go away.
					// old method:
					//color = BiomeGrassColor( gBiomes[biome].temperature, gBiomes[biome].rainfall );
					//gBiomes[biome].grass = ((color & 0xfefefe) + 0x4e0e4e) / 2;
					//color = BiomeFoliageColor( gBiomes[biome].temperature, gBiomes[biome].rainfall );
					//gBiomes[biome].foliage = ((color & 0xfefefe) + 0x4e0e4e) / 2;

					// new method:
					// yes, it's hard-wired in. It actually varies with temperature:
					//         return temperature < -0.1D ? 0x4c763c : 0x6a7039;
					// where temperature is varied by PerlinNoise, but I haven't recreated the
					// PerlinNoise function yet. Rich green vs. sickly swamp brown. I'm going with brown.
					return 0x6a7039;

				// These are actually perfectly normal. Only sub-type 3, roofed forest, is different.
				//case FOREST_BIOME:	// forestType 0
				//case FOREST_HILLS_BIOME:	// forestType 0
				//case BIRCH_FOREST_BIOME:	// forestType 2
				//case BIRCH_FOREST_HILLS_BIOME:	// forestType 2
				//	break;

				case ROOFED_FOREST_BIOME: // forestType 3
					if (isGrass)
					{
						color = BiomeGrassColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);
						// the fefefe makes it so that carries are copied to the low bit,
						// then their magic "go to green" color offset is added in, then
						// divide by two gives a carry that will nicely go away.
						return ((color & 0xfefefe) + 0x28340a) / 2;
					}
					else
					{
						return BiomeFoliageColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);
					}

				case MESA_BIOME:
				case MESA_PLATEAU_F_BIOME:
				case MESA_PLATEAU_BIOME:
					// yes, it's hard-wired
					return isGrass ? 0x90814d : 0x9e814d;

				default:
					return isGrass ? BiomeGrassColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation) : BiomeFoliageColor(GetBiome(biome).Temperature, GetBiome(biome).Downfall, elevation);
			}
		}

		public static Biome GetBiome(int biomeId)
		{
			return IdBiomeMap.GetValueOrDefault(biomeId) ?? new Biome
			{
				Id = biomeId,
				Name = "" + biomeId
			};
		}

		public static Biome GetBiome(string name)
		{
			return NameBiomeMap.GetValueOrDefault(name);
		}

		public int BiomeSwampRiverColor(int color)
		{
			int r = (int) ((color >> 16) & 0xff);
			int g = (int) ((color >> 8) & 0xff);
			int b = (int) color & 0xff;

			// swamp color modifier is 0xE0FFAE
			r = (r * 0xE0) / 255;
			// does nothing: g=(g*0xFF)/255;
			b = (b * 0xAE) / 255;
			color = (r << 16) | (g << 8) | b;

			return color;
		}
	}
}