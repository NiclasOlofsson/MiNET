using System;
using System.Numerics;
using log4net;
using MiNET.Utils;

namespace MiNET.Net
{
	public partial class McpeStartGame : Packet<McpeStartGame>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(McpeStartGame));
		
		public long entityIdSelf; // = null;
		public long runtimeEntityId; // = null;
		public int playerGamemode; // = null;
		public Vector3 spawn; // = null;
		public Vector2 rotation; // = null;
		public int seed; // = null;
		public short biomeType; // = null;
		public string biomeName; // = null;
		public int dimension; // = null;
		public int generator; // = null;
		public int gamemode; // = null;
		public int difficulty; // = null;
		public int x; // = null;
		public int y; // = null;
		public int z; // = null;
		public bool hasAchievementsDisabled; // = null;
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
		public int permissionLevel; // = null;
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
		public bool experimentalGameplayOverride; // = null;
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
		public Itemstates itemstates; // = null;
		public string multiplayerCorrelationId; // = null;
		public bool enableNewInventorySystem; // = null;
		public string serverVersion; // = null;
		
		partial void AfterEncode()
		{
			WriteSignedVarLong(entityIdSelf);
			WriteUnsignedVarLong(runtimeEntityId);
			WriteSignedVarInt(playerGamemode);
			Write(spawn);
			Write(rotation);
			
			//Level settings
			WriteSignedVarInt(seed);
			Write(biomeType);
			Write(biomeName);
			WriteSignedVarInt(dimension);
			WriteSignedVarInt(generator);
			WriteSignedVarInt(gamemode);
			WriteSignedVarInt(difficulty);
			
			WriteSignedVarInt(x);
			WriteVarInt(y);
			WriteSignedVarInt(z);
			
			Write(hasAchievementsDisabled);
			WriteSignedVarInt(time);
			WriteSignedVarInt(eduOffer);
			Write(hasEduFeaturesEnabled);
			Write(eduProductUuid);
			Write(rainLevel);
			Write(lightningLevel);
			Write(hasConfirmedPlatformLockedContent);
			Write(isMultiplayer);
			Write(broadcastToLan);
			WriteVarInt(xboxLiveBroadcastMode);
			WriteVarInt(platformBroadcastMode);
			Write(enableCommands);
			Write(isTexturepacksRequired);
			Write(gamerules);
			Write(experiments);
			Write(false);//ExperimentsPreviouslyToggled
			Write(bonusChest);
			Write(mapEnabled);
			WriteSignedVarInt(permissionLevel);
			Write(serverChunkTickRange);
			Write(hasLockedBehaviorPack);
			Write(hasLockedResourcePack);
			Write(isFromLockedWorldTemplate);
			Write(useMsaGamertagsOnly);
			Write(isFromWorldTemplate);
			Write(isWorldTemplateOptionLocked);
			Write(onlySpawnV1Villagers);
			Write(gameVersion);
			Write(limitedWorldWidth);
			Write(limitedWorldLength);
			Write(isNewNether);
			Write(false);
		//	Write(experimentalGameplayOverride);
			//End of level settings
			
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
		}
		
		partial void AfterDecode()
		{
			entityIdSelf = ReadSignedVarLong();
			runtimeEntityId = ReadUnsignedVarLong();
			playerGamemode = ReadSignedVarInt();
			spawn = ReadVector3();
			rotation = ReadVector2();

			//Level Settings
			seed = ReadSignedVarInt();
			biomeType = ReadShort();
			biomeName = ReadString();
			dimension = ReadSignedVarInt();
			generator = ReadSignedVarInt();
			gamemode = ReadSignedVarInt();
			difficulty = ReadSignedVarInt();
			
			x = ReadSignedVarInt();
			y = ReadVarInt();
			z = ReadSignedVarInt();
			
			hasAchievementsDisabled = ReadBool();
			time = ReadSignedVarInt();
			eduOffer = ReadSignedVarInt();
			hasEduFeaturesEnabled = ReadBool();
			eduProductUuid = ReadString();
			rainLevel = ReadFloat();
			lightningLevel = ReadFloat();
			hasConfirmedPlatformLockedContent = ReadBool();
			isMultiplayer = ReadBool();
			broadcastToLan = ReadBool();
			xboxLiveBroadcastMode = ReadVarInt();
			platformBroadcastMode = ReadVarInt();
			enableCommands = ReadBool();
			isTexturepacksRequired = ReadBool();
			gamerules = ReadGameRules();
			experiments = ReadExperiments();
			ReadBool();
			bonusChest = ReadBool();
			mapEnabled = ReadBool();
			permissionLevel = ReadSignedVarInt();
			serverChunkTickRange = ReadInt();
			hasLockedBehaviorPack = ReadBool();
			hasLockedResourcePack = ReadBool();
			isFromLockedWorldTemplate = ReadBool();
			useMsaGamertagsOnly = ReadBool();
			isFromWorldTemplate = ReadBool();
			isWorldTemplateOptionLocked = ReadBool();
			onlySpawnV1Villagers = ReadBool();
			gameVersion = ReadString();
			limitedWorldWidth = ReadInt();
			limitedWorldLength = ReadInt();
			isNewNether = ReadBool();
			if (ReadBool())
			{
				experimentalGameplayOverride = ReadBool();
			}
			else
			{
				experimentalGameplayOverride = false;
			}
			//End of level settings.
			
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
		}

		/// <inheritdoc />
		public override void Reset()
		{
			entityIdSelf=default(long);
			runtimeEntityId=default(long);
			playerGamemode=default(int);
			spawn=default(Vector3);
			rotation=default(Vector2);
			seed=default(int);
			biomeType=default(short);
			biomeName=default(string);
			dimension=default(int);
			generator=default(int);
			gamemode=default(int);
			difficulty=default(int);
			x=default(int);
			y=default(int);
			z=default(int);
			hasAchievementsDisabled=default(bool);
			time=default(int);
			eduOffer=default(int);
			hasEduFeaturesEnabled=default(bool);
			eduProductUuid=default(string);
			rainLevel=default(float);
			lightningLevel=default(float);
			hasConfirmedPlatformLockedContent=default(bool);
			isMultiplayer=default(bool);
			broadcastToLan=default(bool);
			xboxLiveBroadcastMode=default(int);
			platformBroadcastMode=default(int);
			enableCommands=default(bool);
			isTexturepacksRequired=default(bool);
			gamerules=default(GameRules);
			experiments=default(Experiments);
			bonusChest=default(bool);
			mapEnabled=default(bool);
			permissionLevel=default(int);
			serverChunkTickRange=default(int);
			hasLockedBehaviorPack=default(bool);
			hasLockedResourcePack=default(bool);
			isFromLockedWorldTemplate=default(bool);
			useMsaGamertagsOnly=default(bool);
			isFromWorldTemplate=default(bool);
			isWorldTemplateOptionLocked=default(bool);
			onlySpawnV1Villagers=default(bool);
			gameVersion=default(string);
			limitedWorldWidth=default(int);
			limitedWorldLength=default(int);
			isNewNether=default(bool);
			experimentalGameplayOverride=default(bool);
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
			
			base.Reset();
		}
	}
}