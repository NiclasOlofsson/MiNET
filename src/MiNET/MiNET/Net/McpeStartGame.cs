using System;
using System.Numerics;
using fNbt;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Nbt;

namespace MiNET.Net
{
	public class SpawnSettings
	{
		public short BiomeType { get; set; }
		public string BiomeName { get; set; }
		public int Dimension { get; set; }
		
		public void Read(Packet packet)
		{
			BiomeType = packet.ReadShort();
			BiomeName = packet.ReadString();
			Dimension = packet.ReadVarInt();
		}

		public void Write(Packet packet)
		{
			packet.Write(BiomeType);
			packet.Write(BiomeName);
			packet.WriteVarInt(Dimension);
		}
	}

	public class LevelSettings
	{
		public long seed; // = null;
		public SpawnSettings spawnSettings;

		public int generator; // = null;
		public int gamemode; // = null;
		public int difficulty; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool hasAchievementsDisabled; // = null;
		public bool editorWorld;
		public int time; // = null;
		public int eduOffer; // = null;
		public bool hasEduFeaturesEnabled; // = null;
		public string eduProductUuid; // = null;
		public float rainLevel; // = null;
		public float lightningLevel; // = null;
		public bool hasConfirmedPlatformLockedContent; // = null;
		public bool isMultiplayer; // = null;
		public bool broadcastToLan; // = null;
		public int xboxLiveBroadcastMode; // = null;
		public int platformBroadcastMode; // = null;
		public bool enableCommands; // = null;
		public bool isTexturepacksRequired; // = null;
		public GameRules gamerules; // = null;
		public Experiments experiments;
		public bool bonusChest; // = null;
		public bool mapEnabled; // = null;
		public byte permissionLevel; // = null;
		public int serverChunkTickRange; // = null;
		public bool hasLockedBehaviorPack; // = null;
		public bool hasLockedResourcePack; // = null;
		public bool isFromLockedWorldTemplate; // = null;
		public bool useMsaGamertagsOnly; // = null;
		public bool isFromWorldTemplate; // = null;
		public bool isWorldTemplateOptionLocked; // = null;
		public bool onlySpawnV1Villagers; // = null;
		public string gameVersion; // = null;
		public int limitedWorldWidth; // = null;
		public int limitedWorldLength; // = null;
		public bool isNewNether; // = null;
		public EducationUriResource eduSharedUriResource = null;
		public bool experimentalGameplayOverride; // = null;

		public void Write(Packet packet)
		{
			packet.Write(seed);

			var s = spawnSettings ?? new SpawnSettings();
			s.Write(packet);

			packet.WriteSignedVarInt(generator);
			packet.WriteSignedVarInt(gamemode);
			packet.WriteSignedVarInt(difficulty);

			packet.WriteSignedVarInt(x);
			packet.WriteVarInt(y);
			packet.WriteSignedVarInt(z);

			packet.Write(hasAchievementsDisabled);
			packet.Write(editorWorld);
			packet.WriteSignedVarInt(time);
			packet.WriteSignedVarInt(eduOffer);
			packet.Write(hasEduFeaturesEnabled);
			packet.Write(eduProductUuid);
			packet.Write(rainLevel);
			packet.Write(lightningLevel);
			packet.Write(hasConfirmedPlatformLockedContent);
			packet.Write(isMultiplayer);
			packet.Write(broadcastToLan);
			packet.WriteVarInt(xboxLiveBroadcastMode);
			packet.WriteVarInt(platformBroadcastMode);
			packet.Write(enableCommands);
			packet.Write(isTexturepacksRequired);
			packet.Write(gamerules);
			packet.Write(experiments);
			packet.Write(false); //ExperimentsPreviouslyToggled
			packet.Write(bonusChest);
			packet.Write(mapEnabled);
			packet.Write(permissionLevel);
			packet.Write(serverChunkTickRange);
			packet.Write(hasLockedBehaviorPack);
			packet.Write(hasLockedResourcePack);
			packet.Write(isFromLockedWorldTemplate);
			packet.Write(useMsaGamertagsOnly);
			packet.Write(isFromWorldTemplate);
			packet.Write(isWorldTemplateOptionLocked);
			packet.Write(onlySpawnV1Villagers);
			packet.Write(gameVersion);
			packet.Write(limitedWorldWidth);
			packet.Write(limitedWorldLength);
			packet.Write(isNewNether);
			packet.Write(eduSharedUriResource ?? new EducationUriResource("", ""));
			packet.Write(false);
		}

		public void Read(Packet packet)
		{
			seed = packet.ReadLong();

			spawnSettings = new SpawnSettings();
			spawnSettings.Read(packet);

			generator = packet.ReadSignedVarInt();
			gamemode = packet.ReadSignedVarInt();
			difficulty = packet.ReadSignedVarInt();

			x = packet.ReadSignedVarInt();
			y = packet.ReadVarInt();
			z = packet.ReadSignedVarInt();

			hasAchievementsDisabled = packet.ReadBool();
			editorWorld = packet.ReadBool();
			time = packet.ReadSignedVarInt();
			eduOffer = packet.ReadSignedVarInt();
			hasEduFeaturesEnabled = packet.ReadBool();
			eduProductUuid = packet.ReadString();
			rainLevel = packet.ReadFloat();
			lightningLevel = packet.ReadFloat();
			hasConfirmedPlatformLockedContent = packet.ReadBool();
			isMultiplayer = packet.ReadBool();
			broadcastToLan = packet.ReadBool();
			xboxLiveBroadcastMode = packet.ReadVarInt();
			platformBroadcastMode = packet.ReadVarInt();
			enableCommands = packet.ReadBool();
			isTexturepacksRequired = packet.ReadBool();
			gamerules = packet.ReadGameRules();
			experiments = packet.ReadExperiments();
			packet.ReadBool();
			bonusChest = packet.ReadBool();
			mapEnabled = packet.ReadBool();
			permissionLevel = packet.ReadByte();
			serverChunkTickRange = packet.ReadInt();
			hasLockedBehaviorPack = packet.ReadBool();
			hasLockedResourcePack = packet.ReadBool();
			isFromLockedWorldTemplate = packet.ReadBool();
			useMsaGamertagsOnly = packet.ReadBool();
			isFromWorldTemplate = packet.ReadBool();
			isWorldTemplateOptionLocked = packet.ReadBool();
			onlySpawnV1Villagers = packet.ReadBool();
			gameVersion = packet.ReadString();

			limitedWorldWidth = packet.ReadInt();
			limitedWorldLength = packet.ReadInt();
			isNewNether = packet.ReadBool();
			eduSharedUriResource = packet.ReadEducationUriResource();

			if (packet.ReadBool())
			{
				experimentalGameplayOverride = packet.ReadBool();
			}
			else
			{
				experimentalGameplayOverride = false;
			}
		}
	}

	public partial class McpeStartGame : Packet<McpeStartGame>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeStartGame));
		
		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public int playerGamemode; // = null;
		public Vector3 spawn; // = null;
		public Vector2 rotation; // = null;
		
		public string levelId; // = null;
		public string worldName; // = null;
		public string premiumWorldTemplateId; // = null;
		public bool isTrial; // = null;
		public int movementType; // = null;
		public int movementRewindHistorySize; // = null;
		public bool enableNewBlockBreakSystem; // = null;
		public long currentTick; // = null;
		public int enchantmentSeed; // = null;
		public BlockPalette blockPalette; // = null;
		public ulong blockPaletteChecksum;
		public Itemstates itemstates; // = null;
		public string multiplayerCorrelationId; // = null;
		public bool enableNewInventorySystem; // = null;
		public string serverVersion; // = null;
		public Nbt propertyData;
		public UUID worldTemplateId;

		public LevelSettings levelSettings = new LevelSettings();
		
		partial void AfterEncode()
		{
			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(playerGamemode);
			Write(spawn);
			Write(rotation);
			
			LevelSettings s = levelSettings ?? new LevelSettings();
			s.Write(this);
			
			Write(levelId);
			Write(worldName);
			Write(premiumWorldTemplateId);
			Write(isTrial);
			
			//Player movement settings
			WriteSignedVarInt(movementType);
			WriteSignedVarInt(movementRewindHistorySize);
			Write(enableNewBlockBreakSystem);
			
			Write(currentTick);
			WriteSignedVarInt(enchantmentSeed);
			
			Write(blockPalette);

			Write(itemstates);
			
			Write(multiplayerCorrelationId);
			Write(enableNewInventorySystem);
			Write(serverVersion);
			Write(propertyData);
			Write(blockPaletteChecksum);
			Write(worldTemplateId);
		}
		
		partial void AfterDecode()
		{
			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			playerGamemode = ReadSignedVarInt();
			spawn = ReadVector3();
			rotation = ReadVector2();

			levelSettings = new LevelSettings();
			levelSettings.Read(this);
			
			levelId = ReadString();
			worldName = ReadString();
			premiumWorldTemplateId = ReadString();
			isTrial = ReadBool();
			
			//Player movement settings
			movementType = ReadSignedVarInt();
			movementRewindHistorySize = ReadSignedVarInt();
			enableNewBlockBreakSystem = ReadBool();
			
			currentTick = ReadLong();
			enchantmentSeed = ReadSignedVarInt();

			try
			{
				blockPalette = ReadBlockPalette();
			}
			catch (Exception ex)
			{
				Log.Warn($"Failed to read complete blockpallete", ex);
				return;
			}
			
			itemstates = ReadItemstates();
			
			multiplayerCorrelationId = ReadString();
			enableNewInventorySystem = ReadBool();
			serverVersion = ReadString();
			propertyData = ReadNbt();
			blockPaletteChecksum = ReadUlong();
			worldTemplateId = ReadUUID();
		}

		/// <inheritdoc />
		public override void Reset()
		{
			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			playerGamemode=default(int);
			spawn=default(Vector3);
			rotation=default(Vector2);
			levelSettings = default;
			levelId=default(string);
			worldName=default(string);
			premiumWorldTemplateId=default(string);
			isTrial=default(bool);
			movementType=default(int);
			movementRewindHistorySize=default(int);
			enableNewBlockBreakSystem=default(bool);
			currentTick=default(long);
			enchantmentSeed=default(int);
			blockPalette=default(BlockPalette);
			itemstates=default(Itemstates);
			multiplayerCorrelationId=default(string);
			enableNewInventorySystem=default(bool);
			serverVersion=default(string);
			propertyData = default;
			worldTemplateId = default;
			base.Reset();
		}
	}
}