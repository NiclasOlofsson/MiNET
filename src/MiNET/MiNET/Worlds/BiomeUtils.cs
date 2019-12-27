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

using System.Linq;
using fNbt;

namespace MiNET.Worlds
{
	public class Biome
	{
		public int Id;
		public string Name;
		public string DefinitionName;
		public float Temperature;
		public float Downfall;
		public int Grass; // r,g,b, NOT multiplied by alpha
		public int Foliage; // r,g,b, NOT multiplied by alpha
	}

	public class BiomeUtils
	{
		public static Biome[] Biomes =
		{
			new Biome
			{
				Id = 0,
				Name = "Ocean",
				DefinitionName = "ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			}, // default values of temp and rain
			new Biome
			{
				Id = 1,
				Name = "Plains",
				DefinitionName = "plains",
				Temperature = 0.8f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 2,
				Name = "Desert",
				DefinitionName = "desert",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 3,
				Name = "Extreme Hills",
				DefinitionName = "extreme_hills",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 4,
				Name = "Forest",
				DefinitionName = "forest",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 5,
				Name = "Taiga",
				DefinitionName = "taiga",
				Temperature = 0.25f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 6,
				Name = "Swampland",
				DefinitionName = "swampland",
				Temperature = 0.8f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 7,
				Name = "River",
				DefinitionName = "river",
				Temperature = 0.5f,
				Downfall = 0.5f
			}, // default values of temp and rain
			new Biome
			{
				Id = 8,
				Name = "Nether",
				DefinitionName = "hell",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 9,
				Name = "End",
				DefinitionName = "the_end",
				Temperature = 0.5f,
				Downfall = 0.5f
			}, // default values of temp and rain
			new Biome
			{
				Id = 10,
				Name = "Frozen Ocean",
				DefinitionName = "frozen_ocean",
				Temperature = 0.0f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 11,
				Name = "Frozen River",
				DefinitionName = "frozen_river",
				Temperature = 0.0f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 12,
				Name = "Ice Plains",
				DefinitionName = "ice_plains",
				Temperature = 0.0f,
				Downfall = 1.0f
			},
			new Biome
			{
				Id = 13,
				Name = "Ice Mountains",
				DefinitionName = "ice_mountains",
				Temperature = 0.0f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 14,
				Name = "Mushroom Island",
				DefinitionName = "mushroom_island",
				Temperature = 0.9f,
				Downfall = 1.0f
			},
			new Biome
			{
				Id = 15,
				Name = "Mushroom Island Shore",
				DefinitionName = "mushroom_island_shore",
				Temperature = 0.9f,
				Downfall = 1.0f
			},
			new Biome
			{
				Id = 16,
				Name = "Beach",
				DefinitionName = "beach",
				Temperature = 0.8f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 17,
				Name = "Desert Hills",
				DefinitionName = "desert_hills",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 18,
				Name = "Forest Hills",
				DefinitionName = "forest_hills",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 19,
				Name = "Taiga Hills",
				DefinitionName = "taiga_hills",
				Temperature = 0.25f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 20,
				Name = "Extreme Hills Edge",
				DefinitionName = "extreme_hills_edge",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 21,
				Name = "Jungle",
				DefinitionName = "jungle",
				Temperature = 0.95f,
				Downfall = 0.9f
			},
			new Biome
			{
				Id = 22,
				Name = "Jungle Hills",
				DefinitionName = "jungle_hills",
				Temperature = 0.95f,
				Downfall = 0.9f
			},
			new Biome
			{
				Id = 23,
				Name = "Jungle Edge",
				DefinitionName = "jungle_edge",
				Temperature = 0.95f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 24,
				Name = "Deep Ocean",
				DefinitionName = "deep_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 25,
				Name = "Stone Beach",
				DefinitionName = "stone_beach",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 26,
				Name = "Cold ",
				DefinitionName = "cold_beach",
				Temperature = 0.05f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 27,
				Name = "Birch Forest",
				DefinitionName = "birch_forest",
				Temperature = 0.6f,
				Downfall = 0.6f
			},
			new Biome
			{
				Id = 28,
				Name = "Birch Forest Hills",
				DefinitionName = "birch_forest_hills",
				Temperature = 0.6f,
				Downfall = 0.6f
			},
			new Biome
			{
				Id = 29,
				Name = "Roofed Forest",
				DefinitionName = "roofed_forest",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 30,
				Name = "Cold Taiga",
				DefinitionName = "cold_taiga",
				Temperature = -0.5f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 31,
				Name = "Cold Taiga Hills",
				DefinitionName = "cold_taiga_hills",
				Temperature = -0.5f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 32,
				Name = "Mega Taiga",
				DefinitionName = "mega_taiga",
				Temperature = 0.3f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 33,
				Name = "Mega Taiga Hills",
				DefinitionName = "mega_taiga_hills",
				Temperature = 0.3f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 34,
				Name = "Extreme Hills+",
				DefinitionName = "extreme_hills_plus_trees",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 35,
				Name = "Savanna",
				DefinitionName = "savanna",
				Temperature = 1.2f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 36,
				Name = "Savanna Plateau",
				DefinitionName = "savanna_plateau",
				Temperature = 1.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 37,
				Name = "Mesa",
				DefinitionName = "mesa",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 38,
				Name = "Mesa Plateau F",
				DefinitionName = "mesa_plateau_stone",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 39,
				Name = "Mesa Plateau",
				DefinitionName = "mesa_plateau",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 44,
				Name = "Warm Ocean",
				DefinitionName = "warm_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 45,
				Name = "Lukewarm Ocean",
				DefinitionName = "lukewarm_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 46,
				Name = "Cold Ocean",
				DefinitionName = "cold_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 47,
				Name = "Deep Warm Ocean",
				DefinitionName = "deep_warm_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 48,
				Name = "Deep Lukewarm Ocean",
				DefinitionName = "deep_lukewarm_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 49,
				Name = "Deep Cold Ocean",
				DefinitionName = "deep_cold_ocean",
				Temperature = 0.5f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 50,
				Name = "Deep Frozen Ocean",
				DefinitionName = "deep_frozen_ocean",
				Temperature = 0.0f,
				Downfall = 0.5f
			},
			new Biome // doesn't exists in BE
			{
				Id = 127,
				Name = "The Void",
				DefinitionName = "",
				Temperature = 0.8f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 129,
				Name = "Sunflower Plains",
				DefinitionName = "sunflower_plains",
				Temperature = 0.8f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 130,
				Name = "Desert M",
				DefinitionName = "desert_mutated",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 131,
				Name = "Extreme Hills M",
				DefinitionName = "extreme_hills_mutated",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 132,
				Name = "Flower Forest",
				DefinitionName = "flower_forest",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 133,
				Name = "Taiga M",
				DefinitionName = "taiga_mutated",
				Temperature = 0.25f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 134,
				Name = "Swampland M",
				DefinitionName = "swampland_mutated",
				Temperature = 0.8f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 140,
				Name = "Ice Plains Spikes",
				DefinitionName = "ice_plains_spikes",
				Temperature = 0.0f,
				Downfall = 1.0f
			},
			new Biome
			{
				Id = 149,
				Name = "Jungle M",
				DefinitionName = "jungle_mutated",
				Temperature = 0.95f,
				Downfall = 0.9f
			},
			new Biome
			{
				Id = 151,
				Name = "JungleEdge M",
				DefinitionName = "jungle_edge_mutated",
				Temperature = 0.95f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 155,
				Name = "Birch Forest M",
				DefinitionName = "birch_forest_mutated",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 156,
				Name = "Birch Forest Hills M",
				DefinitionName = "birch_forest_hills_mutated",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 157,
				Name = "Roofed Forest M",
				DefinitionName = "roofed_forest_mutated",
				Temperature = 0.7f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 158,
				Name = "Cold Taiga M",
				DefinitionName = "cold_taiga_mutated",
				Temperature = -0.5f,
				Downfall = 0.4f
			},
			new Biome
			{
				Id = 160,
				Name = "Mega Spruce Taiga",
				DefinitionName = "redwood_taiga_mutated",
				Temperature = 0.25f,
				Downfall = 0.8f
			}, // special exception, temperature not 0.3
			new Biome
			{
				Id = 161,
				Name = "Mega Spruce Taiga Hills",
				DefinitionName = "redwood_taiga_hills_mutated",
				Temperature = 0.3f,
				Downfall = 0.8f
			},
			new Biome
			{
				Id = 162,
				Name = "Extreme Hills+ M",
				DefinitionName = "extreme_hills_plus_trees_mutated",
				Temperature = 0.2f,
				Downfall = 0.3f
			},
			new Biome
			{
				Id = 163,
				Name = "Savanna M",
				DefinitionName = "savanna_mutated",
				Temperature = 1.1f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 164,
				Name = "Savanna Plateau M",
				DefinitionName = "savanna_plateau_mutated",
				Temperature = 1.0f,
				Downfall = 0.5f
			},
			new Biome
			{
				Id = 165,
				Name = "Mesa (Bryce)",
				DefinitionName = "mesa_bryce",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 166,
				Name = "Mesa Plateau F M",
				DefinitionName = "mesa_plateau_stone_mutated",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 167,
				Name = "Mesa Plateau M",
				DefinitionName = "mesa_plateau_mutated",
				Temperature = 2.0f,
				Downfall = 0.0f
			},
			new Biome
			{
				Id = 168,
				Name = " Bamboo Jungle",
				DefinitionName = "bamboo_jungle",
				Temperature = 0.95f,
				Downfall = 0.9f
			},
			new Biome
			{
				Id = 169,
				Name = " Bamboo Jungle Hills",
				DefinitionName = "bamboo_jungle_hills",
				Temperature = 0.95f,
				Downfall = 0.9f
			},
		};

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
			for (int biome = 0; biome < Biomes.Length; biome++)
			{
				Biomes[biome].Grass = ComputeBiomeColor(biome, 0, true);
				Biomes[biome].Foliage = ComputeBiomeColor(biome, 0, false);
			}

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
			return Biomes.FirstOrDefault(biome => biome.Id == biomeId) ?? new Biome
			{
				Id = biomeId,
				Name = "" + biomeId
			};
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

		public static NbtCompound GenerateDefinitionList()
		{
			NbtCompound list = new NbtCompound("");
			foreach (Biome biome in Biomes)
			{
				if (string.IsNullOrEmpty(biome.DefinitionName))
					continue;
				list.Add(
					new NbtCompound(biome.DefinitionName)
					{
						new NbtFloat("downfall", biome.Downfall),
						new NbtFloat("temperature", biome.Temperature),
					}
				);
			}
			return list;
		}
	}
}