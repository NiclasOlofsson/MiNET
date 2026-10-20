
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

Read more about packets and this specification on the [Protocol Wiki](https://github.com/NiclasOlofsson/MiNET/wiki//ref-protocol)

## ALL PACKETS

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Disconnect | 0x05 | 5 |   
| Resource Pack Stack | 0x07 | 7 |   
| Text | 0x09 | 9 |   
| Move Entity | 0x12 | 18 |   
| Camera Instruction | 0x12c | 18 |   
| Trim Data | 0x12e | 18 |   
| Set Player Inventory Options | 0x133 | 19 |   
| Container Registry Cleanup | 0x13d | 19 |   
| Movement Effect | 0x13e | 19 |   
| Camera Aim Assist Presets | 0x140 | 20 |   
| Client Camera Aim Assist | 0x141 | 20 |   
| Client Movement Prediction Sync | 0x142 | 20 |   
| Player Video Capture | 0x144 | 20 |   
| Primitive Shapes | 0x148 | 20 |   
| Clientbound Data Store | 0x14a | 20 |   
| Graphics Override Parameter | 0x14b | 20 |   
| Serverbound Data Store | 0x14c | 20 |   
| Voxel Shapes | 0x151 | 21 |   
| Camera Spline | 0x152 | 21 |   
| Camera Aim Assist Actor Priority | 0x153 | 21 |   
| Locator Bar | 0x155 | 21 |   
| Party Changed | 0x156 | 21 |   
| Serverbound Data Driven Screen Closed | 0x157 | 21 |   
| Clientbound Attribute Layer Sync | 0x159 | 21 |   
| Server Store Info | 0x15a | 21 |   
| Server Presence Info | 0x15b | 21 |   
| Send Party Destination Cookie | 0x15d | 21 |   
| Block Pick Request | 0x22 | 34 |   
| Entity Pick Request | 0x23 | 35 |   
| Set Entity Link | 0x29 | 41 |   
| Respawn | 0x2d | 45 |   
| Crafting Data | 0x34 | 52 |   
| Telemetry Event | 0x41 | 65 |   
| Game Rules Changed | 0x48 | 72 |   
| Available Commands | 0x4c | 76 |   
| Command Request | 0x4d | 77 |   
| Command Output | 0x4f | 79 |   
| Resource Pack Chunk Request | 0x54 | 84 |   
| Purchase Receipt | 0x5c | 92 |   
| Set Last Hurt By | 0x60 | 96 |   
| Book Edit | 0x61 | 97 |   
| Lab Table | 0x6d | 109 |   
| Update Soft Enum | 0x72 | 114 |   
| Network Stack Latency | 0x73 | 115 |   
| Biome Definition List | 0x7a | 122 |   
| Level Sound Event | 0x7b | 123 |   
| Structure Template Data Export Response | 0x85 | 133 |   
| Client Cache Blob Status | 0x87 | 135 |   
| Client Cache Miss Response | 0x88 | 136 |   
| Education Settings | 0x89 | 137 |   
| Player Enchant Options | 0x92 | 146 |   
| Player Armor Damage | 0x95 | 149 |   
| Player Fog | 0xa0 | 160 |   
| Item Component | 0xa2 | 162 |   
| Clientbound Debug Renderer | 0xa4 | 164 |   
| Npc Dialogue | 0xa9 | 169 |   
| Edu Uri Resource | 0xaa | 170 |   
| Create Photo | 0xab | 171 |   
| Update Sub Chunk Blocks Packet | 0xac | 172 |   
| Sub Chunk Request Packet | 0xaf | 175 |   
| Update Abilities | 0xbb | 187 |   
| Update Adventure Settings | 0xbc | 188 |   
| Death Info | 0xbd | 189 |   
| Editor Network | 0xbe | 190 |   
| Feature Registry | 0xbf | 191 |   
| Unlocked Recipes | 0xc7 | 199 |   


## Data types

| Data type | 
|:--- |
| BlockCoordinates [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-BlockCoordinates) |
| bool [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-bool) |
| byte [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte) |
| byte[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte[]) |
| CommandOriginData [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-CommandOriginData) |
| EnchantOptions [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-EnchantOptions) |
| Experiments [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Experiments) |
| FixedString [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-FixedString) |
| float [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-float) |
| ItemComponentList [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ItemComponentList) |
| long [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-long) |
| MaterialReducerRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-MaterialReducerRecipe[]) |
| NbtBody [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-NbtBody) |
| OFFLINE_MESSAGE_DATA_ID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-OFFLINE_MESSAGE_DATA_ID) |
| PlayerLocation [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerLocation) |
| PotionContainerChangeRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PotionContainerChangeRecipe[]) |
| PotionTypeRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PotionTypeRecipe[]) |
| Recipes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Recipes) |
| ResourcePackIdVersions [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ResourcePackIdVersions) |
| SignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarInt) |
| SignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarLong) |
| Skin [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Skin) |
| string [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-string) |
| uint [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-uint) |
| ulong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ulong) |
| UnsignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarInt) |
| UnsignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarLong) |
| UpdateSubChunkBlocksPacketEntry[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UpdateSubChunkBlocksPacketEntry[]) |
| UUID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UUID) |
| VarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-VarInt) |
| Vector3 [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Vector3) |

## Constants
	OFFLINE_MESSAGE_DATA_ID
	byte[]
	{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }

## Packets

### Disconnect (0x05)
Wiki: [Disconnect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Disconnect)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Reason | SignedVarInt |  |
|Hide disconnect reason | bool |  |
-----------------------------------------------------------------------
### Resource Pack Stack (0x07)
Wiki: [Resource Pack Stack](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackStack)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Must accept | bool |  |
|ResourcePackIdVersions | ResourcePackIdVersions |  |
|Game Version | string |  |
|Experiments | Experiments |  |
|Has editor packs | bool |  |
-----------------------------------------------------------------------
### Text (0x09)
Wiki: [Text](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Text)

**Sent from server:** true  
**Sent from client:** true



#### Chat Types constants

| Name | Value |
|:-----|:-----|
|Raw | 0 |
|Chat | 1 |
|Translation | 2 |
|Popup | 3 |
|Jukeboxpopup | 4 |
|Tip | 5 |
|System | 6 |
|Whisper | 7 |
|Announcement | 8 |
|Jsonwhisper | 9 |
|Json | 10 |
|Jsonannouncement | 11 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Move Entity (0x12)
Wiki: [Move Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MoveEntity)

**Sent from server:** true  
**Sent from client:** true


TODO: Links
count short
loop
link[0] long
link[1] long
link[2] byte
TODO: Modifiers
count int
name string
val1 float
val2 float



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Flags | byte |  |
|Position | PlayerLocation |  |
-----------------------------------------------------------------------
### Block Pick Request (0x22)
Wiki: [Block Pick Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockPickRequest)

**Sent from server:** false  
**Sent from client:** true

 0x18 LevelSoundEventV1 removed from the protocol at 785 (Feb 2025); use 0x7b LevelSoundEvent 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | SignedVarInt |  |
|Y | SignedVarInt |  |
|Z | SignedVarInt |  |
|Add User Data | bool |  |
|Selected Slot | byte |  |
-----------------------------------------------------------------------
### Entity Pick Request (0x23)
Wiki: [Entity Pick Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityPickRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | ulong |  |
|Selected Slot | byte |  |
|Add User Data | bool |  |
-----------------------------------------------------------------------
### Set Entity Link (0x29)
Wiki: [Set Entity Link](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityLink)

**Sent from server:** true  
**Sent from client:** false



#### Link Actions constants

| Name | Value |
|:-----|:-----|
|Remove | 0 |
|Ride | 1 |
|Passenger | 2 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Ridden ID | SignedVarLong |  |
|Rider ID | SignedVarLong |  |
|Link Type | byte |  |
|Immediate | bool |  |
|Rider Initiated | bool |  |
|Vehicle Angular Velocity | float |  |
-----------------------------------------------------------------------
### Respawn (0x2d)
Wiki: [Respawn](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Respawn)

**Sent from server:** true  
**Sent from client:** true

 0x2c Animate is Cereal: generated from AnimatePacket.json, see MiNET.ProtocolGen/roster.json 

#### Respawn State constants

| Name | Value |
|:-----|:-----|
|Search | 0 |
|Ready | 1 |
|Client Ready | 2 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
|State | byte |  |
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Crafting Data (0x34)
Wiki: [Crafting Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CraftingData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Recipes | Recipes |  |
|Potion type recipes | PotionTypeRecipe[] |  |
|potion container recipes | PotionContainerChangeRecipe[] |  |
|Material reducer recipes | MaterialReducerRecipe[] |  |
|Is Clean | bool |  |
-----------------------------------------------------------------------
### Telemetry Event (0x41)
Wiki: [Telemetry Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TelemetryEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Event data | SignedVarInt |  |
|Event type | byte |  |
|Aux Data | byte[] | 0, true |
-----------------------------------------------------------------------
### Game Rules Changed (0x48)
Wiki: [Game Rules Changed](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GameRulesChanged)

**Sent from server:** true  
**Sent from client:** false

 0x47 ItemFrameDropItem removed from the protocol at 662 (Feb 2024); frame drops arrive via the block-attack path 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Available Commands (0x4c)
Wiki: [Available Commands](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AvailableCommands)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Command Request (0x4d)
Wiki: [Command Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Command | string |  |
|Origin | CommandOriginData |  |
|Is Internal | bool |  |
|Version | string |  |
-----------------------------------------------------------------------
### Command Output (0x4f)
Wiki: [Command Output](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandOutput)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Resource Pack Chunk Request (0x54)
Wiki: [Resource Pack Chunk Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackChunkRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Chunk Index | uint |  |
-----------------------------------------------------------------------
### Purchase Receipt (0x5c)
Wiki: [Purchase Receipt](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PurchaseReceipt)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Set Last Hurt By (0x60)
Wiki: [Set Last Hurt By](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetLastHurtBy)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown | VarInt |  |
-----------------------------------------------------------------------
### Book Edit (0x61)
Wiki: [Book Edit](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BookEdit)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Inventory Slot | SignedVarInt |  |
|Type | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Lab Table (0x6d)
Wiki: [Lab Table](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LabTable)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Useless Byte | byte |  |
|Lab Table X | SignedVarInt |  |
|Lab Table Y | SignedVarInt |  |
|Lab Table Z | SignedVarInt |  |
|Reaction Type | byte |  |
-----------------------------------------------------------------------
### Update Soft Enum (0x72)
Wiki: [Update Soft Enum](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateSoftEnum)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Network Stack Latency (0x73)
Wiki: [Network Stack Latency](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NetworkStackLatency)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Timestamp | ulong |  |
|Unknown Flag | byte |  |
-----------------------------------------------------------------------
### Biome Definition List (0x7a)
Wiki: [Biome Definition List](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BiomeDefinitionList)

**Sent from server:** true  
**Sent from client:** false

 0x78 LevelSoundEventV2 removed from the protocol at 785 (Feb 2025); use 0x7b LevelSoundEvent 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Level Sound Event (0x7b)
Wiki: [Level Sound Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelSoundEvent)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Sound ID | string |  |
|Position | Vector3 |  |
|Block Id | SignedVarInt |  |
|Entity Type | string |  |
|Is baby mob | bool |  |
|Is global | bool |  |
-----------------------------------------------------------------------
### Structure Template Data Export Response (0x85)
Wiki: [Structure Template Data Export Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StructureTemplateDataExportResponse)

**Sent from server:** true  
**Sent from client:** false

 0x7e VideoStreamConnect gone since 407 (Jun 2020); the websocket packet is AutomationClientConnect at 0x5f 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
-----------------------------------------------------------------------
### Client Cache Blob Status (0x87)
Wiki: [Client Cache Blob Status](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientCacheBlobStatus)

**Sent from server:** false  
**Sent from client:** true

 The client sends this to report which blobs it already holds, so the handler belongs on
     the server. It was declared the other way round, which put the handler on the client and
     left the server with no way to receive it at all. 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Client Cache Miss Response (0x88)
Wiki: [Client Cache Miss Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientCacheMissResponse)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Education Settings (0x89)
Wiki: [Education Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EducationSettings)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Code Builder Default Uri | string |  |
|Code Builder Title | string |  |
|Can Resize Code Builder | bool |  |
|Disable Legacy Title Bar | bool |  |
|Post Process Filter | string |  |
|Screenshot Border Resource Path | string |  |
-----------------------------------------------------------------------
### Player Enchant Options (0x92)
Wiki: [Player Enchant Options](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerEnchantOptions)

**Sent from server:** true  
**Sent from client:** false


public const PLAYER_AUTH_INPUT_PACKET = 0x90;
public const PLAYER_ARMOR_DAMAGE_PACKET = 0x95;
public const CODE_BUILDER_PACKET = 0x96;
public const UPDATE_PLAYER_GAME_TYPE_PACKET = 0x97;
public const EMOTE_LIST_PACKET = 0x98;
public const POSITION_TRACKING_D_B_SERVER_BROADCAST_PACKET = 0x99;
public const POSITION_TRACKING_D_B_CLIENT_REQUEST_PACKET = 0x9a;
public const DEBUG_INFO_PACKET = 0x9b;



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Enchant options | EnchantOptions |  |
-----------------------------------------------------------------------
### Player Armor Damage (0x95)
Wiki: [Player Armor Damage](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerArmorDamage)

**Sent from server:** true  
**Sent from client:** false

 Whole payload is a length-prefixed array of (slot, damage) pairs, which the generator has
     no type for, so it lives entirely in the partial (Net/McpePlayerArmorDamage.cs). 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Player Fog (0xa0)
Wiki: [Player Fog](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerFog)

**Sent from server:** true  
**Sent from client:** false

 Trailing runtime entity id list is a VarInt-count-prefixed array of UnsignedVarLong,
     which the generator has no type for, so it lives in the partial (Net/McpeAnimateEntity.cs). 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Item Component (0xa2)
Wiki: [Item Component](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ItemComponent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entries | ItemComponentList |  |
-----------------------------------------------------------------------
### Clientbound Debug Renderer (0xa4)
Wiki: [Clientbound Debug Renderer](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundDebugRenderer)

**Sent from server:** true  
**Sent from client:** false

 0xa3 FilterText deprecated at 671 (Mar 2024); profanity filtering no longer round-trips to the server 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | string |  |
-----------------------------------------------------------------------
### Npc Dialogue (0xa9)
Wiki: [Npc Dialogue](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NpcDialogue)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Npc Unique ID | long |  |
|Action Type | SignedVarInt |  |
|Dialogue | string |  |
|Scene Name | string |  |
|Npc Name | string |  |
|Action JSON | string |  |
-----------------------------------------------------------------------
### Edu Uri Resource (0xaa)
Wiki: [Edu Uri Resource](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EduUriResource)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Button Name | string |  |
|Link Uri | string |  |
-----------------------------------------------------------------------
### Create Photo (0xab)
Wiki: [Create Photo](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CreatePhoto)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Unique ID | long |  |
|Photo Name | string |  |
|Photo Item Name | string |  |
-----------------------------------------------------------------------
### Update Sub Chunk Blocks Packet (0xac)
Wiki: [Update Sub Chunk Blocks Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateSubChunkBlocksPacket)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Subchunk coordinates | BlockCoordinates |  |
|Layer zero updates | UpdateSubChunkBlocksPacketEntry[] |  |
|Layer one updates | UpdateSubChunkBlocksPacketEntry[] |  |
-----------------------------------------------------------------------
### Sub Chunk Request Packet (0xaf)
Wiki: [Sub Chunk Request Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SubChunkRequestPacket)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Update Abilities (0xbb)
Wiki: [Update Abilities](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateAbilities)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Unique ID | long |  |
|Permission Level | byte |  |
|Command Permission | byte |  |
-----------------------------------------------------------------------
### Update Adventure Settings (0xbc)
Wiki: [Update Adventure Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateAdventureSettings)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|No PvM | bool |  |
|No MvP | bool |  |
|Immutable World | bool |  |
|Show Name Tags | bool |  |
|Auto Jump | bool |  |
-----------------------------------------------------------------------
### Death Info (0xbd)
Wiki: [Death Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-DeathInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Cause | string |  |
-----------------------------------------------------------------------
### Editor Network (0xbe)
Wiki: [Editor Network](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EditorNetwork)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Route To Manager | bool |  |
|Payload | NbtBody |  |
-----------------------------------------------------------------------
### Feature Registry (0xbf)
Wiki: [Feature Registry](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-FeatureRegistry)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Unlocked Recipes (0xc7)
Wiki: [Unlocked Recipes](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UnlockedRecipes)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | uint |  |
-----------------------------------------------------------------------
### Trim Data (0x12e)
Wiki: [Trim Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TrimData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Set Player Inventory Options (0x133)
Wiki: [Set Player Inventory Options](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetPlayerInventoryOptions)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Left Tab | SignedVarInt |  |
|Right Tab | SignedVarInt |  |
|Filtering | bool |  |
|Layout | SignedVarInt |  |
|Crafting Layout | SignedVarInt |  |
-----------------------------------------------------------------------
### Container Registry Cleanup (0x13d)
Wiki: [Container Registry Cleanup](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerRegistryCleanup)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Movement Effect (0x13e)
Wiki: [Movement Effect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MovementEffect)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Effect Type | UnsignedVarInt |  |
|Duration | UnsignedVarInt |  |
|Tick | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Client Camera Aim Assist (0x141)
Wiki: [Client Camera Aim Assist](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientCameraAimAssist)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Preset ID | string |  |
|Action | byte |  |
|Allow Aim Assist | bool |  |
-----------------------------------------------------------------------
### Camera Aim Assist Presets (0x140)
Wiki: [Camera Aim Assist Presets](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraAimAssistPresets)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Client Movement Prediction Sync (0x142)
Wiki: [Client Movement Prediction Sync](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientMovementPredictionSync)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Scale | float |  |
|Width | float |  |
|Height | float |  |
|Movement Speed | float |  |
|Underwater Movement Speed | float |  |
|Lava Movement Speed | float |  |
|Jump Strength | float |  |
|Health | float |  |
|Hunger | float |  |
|Friction Modifier | float |  |
|Bounciness | float |  |
|Air Drag Modifier | float |  |
|Actor Unique ID | SignedVarLong |  |
|Actor Flying State | bool |  |
-----------------------------------------------------------------------
### Player Video Capture (0x144)
Wiki: [Player Video Capture](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerVideoCapture)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Recording | bool |  |
-----------------------------------------------------------------------
### Primitive Shapes (0x148)
Wiki: [Primitive Shapes](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PrimitiveShapes)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Clientbound Data Store (0x14a)
Wiki: [Clientbound Data Store](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundDataStore)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Graphics Override Parameter (0x14b)
Wiki: [Graphics Override Parameter](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GraphicsOverrideParameter)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Serverbound Data Store (0x14c)
Wiki: [Serverbound Data Store](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerboundDataStore)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Property | string |  |
|Path | string |  |
-----------------------------------------------------------------------
### Voxel Shapes (0x151)
Wiki: [Voxel Shapes](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-VoxelShapes)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Camera Spline (0x152)
Wiki: [Camera Spline](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraSpline)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Camera Aim Assist Actor Priority (0x153)
Wiki: [Camera Aim Assist Actor Priority](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraAimAssistActorPriority)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Camera Instruction (0x12c)
Wiki: [Camera Instruction](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraInstruction)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Locator Bar (0x155)
Wiki: [Locator Bar](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LocatorBar)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Party Changed (0x156)
Wiki: [Party Changed](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PartyChanged)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Party Id | string |  |
|Party Leader | bool |  |
-----------------------------------------------------------------------
### Serverbound Data Driven Screen Closed (0x157)
Wiki: [Serverbound Data Driven Screen Closed](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerboundDataDrivenScreenClosed)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Form Id | uint |  |
|Close Reason | string |  |
-----------------------------------------------------------------------
### Clientbound Attribute Layer Sync (0x159)
Wiki: [Clientbound Attribute Layer Sync](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundAttributeLayerSync)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload Type | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Server Store Info (0x15a)
Wiki: [Server Store Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerStoreInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Server Presence Info (0x15b)
Wiki: [Server Presence Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerPresenceInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Send Party Destination Cookie (0x15d)
Wiki: [Send Party Destination Cookie](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SendPartyDestinationCookie)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Cookie | string |  |
|Intent | string |  |
|Destination Name | string |  |
-----------------------------------------------------------------------


