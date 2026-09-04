
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

Read more about packets and this specification on the [Protocol Wiki](https://github.com/NiclasOlofsson/MiNET/wiki//ref-protocol)

## ALL PACKETS

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Login | 0x01 | 1 |   
| Play Status | 0x02 | 2 |   
| Client To Server Handshake | 0x04 | 4 |   
| Disconnect | 0x05 | 5 |   
| Resource Pack Stack | 0x07 | 7 |   
| Text | 0x09 | 9 |   
| Set Time | 0x0a | 10 |   
| Remove Entity | 0x0e | 14 |   
| Server Player Post Move Position | 0x10 | 16 |   
| Take Item Entity | 0x11 | 17 |   
| Move Entity | 0x12 | 18 |   
| Camera Instruction | 0x12c | 18 |   
| Trim Data | 0x12e | 18 |   
| Open Sign | 0x12f | 18 |   
| Agent Animation | 0x130 | 19 |   
| Refresh Entitlements | 0x131 | 19 |   
| Player Toggle Crafter Slot Request | 0x132 | 19 |   
| Set Player Inventory Options | 0x133 | 19 |   
| Set Hud | 0x134 | 19 |   
| Award Achievement | 0x135 | 19 |   
| Clientbound Close Form | 0x136 | 19 |   
| Server Bound Loading Screen | 0x138 | 19 |   
| Jigsaw Structure Data | 0x139 | 19 |   
| Current Structure Feature | 0x13a | 19 |   
| Camera Aim Assist | 0x13c | 19 |   
| Container Registry Cleanup | 0x13d | 19 |   
| Movement Effect | 0x13e | 19 |   
| Camera Aim Assist Presets | 0x140 | 20 |   
| Client Camera Aim Assist | 0x141 | 20 |   
| Client Movement Prediction Sync | 0x142 | 20 |   
| Update Client Options | 0x143 | 20 |   
| Player Video Capture | 0x144 | 20 |   
| Clientbound Control Scheme Set | 0x147 | 20 |   
| Primitive Shapes | 0x148 | 20 |   
| Clientbound Data Store | 0x14a | 20 |   
| Graphics Override Parameter | 0x14b | 20 |   
| Serverbound Data Store | 0x14c | 20 |   
| Clientbound Data Driven Ui Show Screen | 0x14d | 20 |   
| Clientbound Data Driven Ui Close Screen | 0x14e | 20 |   
| Clientbound Data Driven Ui Reload | 0x14f | 20 |   
| Update Block | 0x15 | 21 |   
| Clientbound Texture Shift | 0x150 | 21 |   
| Voxel Shapes | 0x151 | 21 |   
| Camera Spline | 0x152 | 21 |   
| Camera Aim Assist Actor Priority | 0x153 | 21 |   
| Resource Packs Ready For Validation | 0x154 | 21 |   
| Locator Bar | 0x155 | 21 |   
| Party Changed | 0x156 | 21 |   
| Serverbound Data Driven Screen Closed | 0x157 | 21 |   
| Sync World Clocks | 0x158 | 21 |   
| Clientbound Attribute Layer Sync | 0x159 | 21 |   
| Server Store Info | 0x15a | 21 |   
| Server Presence Info | 0x15b | 21 |   
| Send Party Destination Cookie | 0x15d | 21 |   
| Party Destination Cookie Response | 0x15e | 21 |   
| Add Painting | 0x16 | 22 |   
| Level Event | 0x19 | 25 |   
| Block Event | 0x1a | 26 |   
| Entity Event | 0x1b | 27 |   
| Mob Effect | 0x1c | 28 |   
| Update Attributes | 0x1d | 29 |   
| Interact | 0x21 | 33 |   
| Block Pick Request | 0x22 | 34 |   
| Entity Pick Request | 0x23 | 35 |   
| Player Action | 0x24 | 36 |   
| Hurt Armor | 0x26 | 38 |   
| Set Entity Motion | 0x28 | 40 |   
| Set Entity Link | 0x29 | 41 |   
| Set Health | 0x2a | 42 |   
| Set Spawn Position | 0x2b | 43 |   
| Animate | 0x2c | 44 |   
| Respawn | 0x2d | 45 |   
| Container Open | 0x2e | 46 |   
| Container Close | 0x2f | 47 |   
| Player Hotbar | 0x30 | 48 |   
| Container Set Data | 0x33 | 51 |   
| Crafting Data | 0x34 | 52 |   
| Gui Data Pick Item | 0x36 | 54 |   
| Block Entity Data | 0x38 | 56 |   
| Set Commands Enabled | 0x3b | 59 |   
| Set Difficulty | 0x3c | 60 |   
| Change Dimension | 0x3d | 61 |   
| Set Player Game Type | 0x3e | 62 |   
| Simple Event | 0x40 | 64 |   
| Telemetry Event | 0x41 | 65 |   
| Spawn Experience Orb | 0x42 | 66 |   
| Map Info Request | 0x44 | 68 |   
| Request Chunk Radius | 0x45 | 69 |   
| Chunk Radius Update | 0x46 | 70 |   
| Game Rules Changed | 0x48 | 72 |   
| Camera | 0x49 | 73 |   
| Show Credits | 0x4b | 75 |   
| Available Commands | 0x4c | 76 |   
| Command Request | 0x4d | 77 |   
| Command Block Update | 0x4e | 78 |   
| Command Output | 0x4f | 79 |   
| Update Trade | 0x50 | 80 |   
| Update Equipment | 0x51 | 81 |   
| Resource Pack Data Info | 0x52 | 82 |   
| Resource Pack Chunk Data | 0x53 | 83 |   
| Resource Pack Chunk Request | 0x54 | 84 |   
| Stop Sound | 0x57 | 87 |   
| Set Title | 0x58 | 88 |   
| Add Behavior Tree | 0x59 | 89 |   
| Show Store Offer | 0x5b | 91 |   
| Purchase Receipt | 0x5c | 92 |   
| Sub Client Login | 0x5e | 94 |   
| Initiate Web Socket Connection | 0x5f | 95 |   
| Set Last Hurt By | 0x60 | 96 |   
| Book Edit | 0x61 | 97 |   
| Npc Request | 0x62 | 98 |   
| Modal Form Request | 0x64 | 100 |   
| Modal Form Response | 0x65 | 101 |   
| Server Settings Request | 0x66 | 102 |   
| Server Settings Response | 0x67 | 103 |   
| Show Profile | 0x68 | 104 |   
| Set Default Game Type | 0x69 | 105 |   
| Set Display Objective | 0x6b | 107 |   
| Lab Table | 0x6d | 109 |   
| Update Block Synced | 0x6e | 110 |   
| Set Local Player As Initialized | 0x71 | 113 |   
| Update Soft Enum | 0x72 | 114 |   
| Network Stack Latency | 0x73 | 115 |   
| Spawn Particle Effect | 0x76 | 118 |   
| Available Entity Identifiers | 0x77 | 119 |   
| Network Chunk Publisher Update | 0x79 | 121 |   
| Biome Definition List | 0x7a | 122 |   
| Level Sound Event | 0x7b | 123 |   
| Level Event Generic | 0x7c | 124 |   
| Lectern Update | 0x7d | 125 |   
| Client Cache Status | 0x81 | 129 |   
| On Screen Texture Animation | 0x82 | 130 |   
| Map Create Locked Copy | 0x83 | 131 |   
| Structure Template Data Export Request | 0x84 | 132 |   
| Structure Template Data Export Response | 0x85 | 133 |   
| Client Cache Blob Status | 0x87 | 135 |   
| Client Cache Miss Response | 0x88 | 136 |   
| Education Settings | 0x89 | 137 |   
| Emote | 0x8a | 138 |   
| Multiplayer Settings | 0x8b | 139 |   
| Settings Command | 0x8c | 140 |   
| Completed Using Item | 0x8e | 142 |   
| Network Settings | 0x8f | 143 |   
| Player Enchant Options | 0x92 | 146 |   
| Player Armor Damage | 0x95 | 149 |   
| Code Builder | 0x96 | 150 |   
| Update Player Game Type | 0x97 | 151 |   
| Emote List | 0x98 | 152 |   
| Position Tracking Db Server Broadcast | 0x99 | 153 |   
| Position Tracking Db Client Request | 0x9a | 154 |   
| Debug Info | 0x9b | 155 |   
| Packet Violation Warning | 0x9c | 156 |   
| Motion Prediction Hints | 0x9d | 157 |   
| Animate Entity | 0x9e | 158 |   
| Camera Shake | 0x9f | 159 |   
| Player Fog | 0xa0 | 160 |   
| Correct Player Move Prediction | 0xa1 | 161 |   
| Item Component | 0xa2 | 162 |   
| Clientbound Debug Renderer | 0xa4 | 164 |   
| Sync Entity Property | 0xa5 | 165 |   
| Add Volume Entity | 0xa6 | 166 |   
| Remove Volume Entity | 0xa7 | 167 |   
| Simulation Type | 0xa8 | 168 |   
| Npc Dialogue | 0xa9 | 169 |   
| Edu Uri Resource | 0xaa | 170 |   
| Create Photo | 0xab | 171 |   
| Update Sub Chunk Blocks Packet | 0xac | 172 |   
| Sub Chunk Request Packet | 0xaf | 175 |   
| Player Start Item Cooldown | 0xb0 | 176 |   
| Script Message | 0xb1 | 177 |   
| Code Builder Source | 0xb2 | 178 |   
| Ticking Areas Load Status | 0xb3 | 179 |   
| Agent Action Event | 0xb5 | 181 |   
| Change Mob Property | 0xb6 | 182 |   
| Lesson Progress | 0xb7 | 183 |   
| Request Permissions | 0xb9 | 185 |   
| Toast Request | 0xba | 186 |   
| Update Abilities | 0xbb | 187 |   
| Update Adventure Settings | 0xbc | 188 |   
| Death Info | 0xbd | 189 |   
| Editor Network | 0xbe | 190 |   
| Feature Registry | 0xbf | 191 |   
| Server Stats | 0xc0 | 192 |   
| Request Network Settings | 0xc1 | 193 |   
| Game Test Request | 0xc2 | 194 |   
| Game Test Results | 0xc3 | 195 |   
| Update Client Input Locks | 0xc4 | 196 |   
| Unlocked Recipes | 0xc7 | 199 |   


## Data types

| Data type | 
|:--- |
| BlockCoordinates [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-BlockCoordinates) |
| bool [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-bool) |
| byte [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte) |
| byte[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte[]) |
| ByteArray [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ByteArray) |
| CommandOriginData [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-CommandOriginData) |
| EnchantOptions [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-EnchantOptions) |
| Experiments [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Experiments) |
| FixedString [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-FixedString) |
| float [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-float) |
| int [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-int) |
| ItemComponentList [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ItemComponentList) |
| long [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-long) |
| MaterialReducerRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-MaterialReducerRecipe[]) |
| Nbt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Nbt) |
| NbtBody [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-NbtBody) |
| OFFLINE_MESSAGE_DATA_ID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-OFFLINE_MESSAGE_DATA_ID) |
| PlayerAttributes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerAttributes) |
| PlayerLocation [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerLocation) |
| PotionContainerChangeRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PotionContainerChangeRecipe[]) |
| PotionTypeRecipe[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PotionTypeRecipe[]) |
| Recipes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Recipes) |
| ResourcePackIdVersions [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ResourcePackIdVersions) |
| short [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-short) |
| SignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarInt) |
| SignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarLong) |
| Skin [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Skin) |
| string [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-string) |
| StructureSettings [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-StructureSettings) |
| uint [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-uint) |
| ulong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ulong) |
| UnsignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarInt) |
| UnsignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarLong) |
| UpdateSubChunkBlocksPacketEntry[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UpdateSubChunkBlocksPacketEntry[]) |
| ushort [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ushort) |
| UUID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UUID) |
| VarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-VarInt) |
| Vector2 [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Vector2) |
| Vector3 [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Vector3) |

## Constants
	OFFLINE_MESSAGE_DATA_ID
	byte[]
	{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }

## Packets

### Login (0x01)
Wiki: [Login](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Login)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Protocol Version | int |  |
|Payload | ByteArray |  |
-----------------------------------------------------------------------
### Play Status (0x02)
Wiki: [Play Status](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayStatus)

**Sent from server:** true  
**Sent from client:** false



#### Play Status constants

| Name | Value |
|:-----|:-----|
|Login Success | 0 |
|Login Failed Client | 1 |
|Login Failed Server | 2 |
|Player Spawn | 3 |
|Login Failed Invalid Tenant | 4 |
|Login Failed Vanilla Edu | 5 |
|Login Failed Edu Vanilla | 6 |
|Login Failed Server Full | 7 |
|Login Failed Editor Vanilla Mismatch | 8 |
|Login Failed Vanilla Editor Mismatch | 9 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Status | int |  |
-----------------------------------------------------------------------
### Client To Server Handshake (0x04)
Wiki: [Client To Server Handshake](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientToServerHandshake)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
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
### Set Time (0x0a)
Wiki: [Set Time](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetTime)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | SignedVarInt |  |
-----------------------------------------------------------------------
### Remove Entity (0x0e)
Wiki: [Remove Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RemoveEntity)

**Sent from server:** true  
**Sent from client:** false


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
|Entity ID Self | SignedVarLong |  |
-----------------------------------------------------------------------
### Server Player Post Move Position (0x10)
Wiki: [Server Player Post Move Position](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerPlayerPostMovePosition)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
-----------------------------------------------------------------------
### Take Item Entity (0x11)
Wiki: [Take Item Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TakeItemEntity)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Target | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Move Entity (0x12)
Wiki: [Move Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MoveEntity)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Flags | byte |  |
|Position | PlayerLocation |  |
-----------------------------------------------------------------------
### Update Block (0x15)
Wiki: [Update Block](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateBlock)

**Sent from server:** true  
**Sent from client:** false

 0x14 RiderJump became PassengerJump at 471 and was removed from the protocol at 800 (Apr 2025) 

#### Flags constants

| Name | Value |
|:-----|:-----|
|None | 0 |
|Neighbors | 1 |
|Network | 2 |
|Nographic | 4 |
|Priority | 8 |
|All | (Neighbors | Network) |
|All Priority | (All | Priority) |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Block Runtime ID | UnsignedVarInt |  |
|Block Priority | UnsignedVarInt |  |
|Storage | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Add Painting (0x16)
Wiki: [Add Painting](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddPainting)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Runtime Entity ID | UnsignedVarLong |  |
|Coordinates | Vector3 |  |
|Direction | SignedVarInt |  |
|Title | string |  |
-----------------------------------------------------------------------
### Level Event (0x19)
Wiki: [Level Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelEvent)

**Sent from server:** true  
**Sent from client:** false

 0x18 LevelSoundEventV1 removed from the protocol at 785 (Feb 2025); use 0x7b LevelSoundEvent 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event ID | SignedVarInt |  |
|Position | Vector3 |  |
|Data | SignedVarInt |  |
-----------------------------------------------------------------------
### Block Event (0x1a)
Wiki: [Block Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Case 1 | SignedVarInt |  |
|Case 2 | SignedVarInt |  |
-----------------------------------------------------------------------
### Entity Event (0x1b)
Wiki: [Entity Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityEvent)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Event ID | byte |  |
|Data | SignedVarInt |  |
-----------------------------------------------------------------------
### Mob Effect (0x1c)
Wiki: [Mob Effect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobEffect)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Event ID | byte |  |
|Effect ID | SignedVarInt |  |
|Amplifier | SignedVarInt |  |
|Particles | bool |  |
|Duration | SignedVarInt |  |
|Tick | UnsignedVarLong |  |
|Ambient | bool |  |
-----------------------------------------------------------------------
### Update Attributes (0x1d)
Wiki: [Update Attributes](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateAttributes)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Attributes | PlayerAttributes |  |
|Tick | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Interact (0x21)
Wiki: [Interact](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Interact)

**Sent from server:** true  
**Sent from client:** true



#### Actions constants

| Name | Value |
|:-----|:-----|
|Right Click | 1 |
|Left Click | 2 |
|Leave Vehicle | 3 |
|Mouse Over | 4 |
|Open Npc | 5 |
|Open Inventory | 6 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Block Pick Request (0x22)
Wiki: [Block Pick Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockPickRequest)

**Sent from server:** false  
**Sent from client:** true




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
### Player Action (0x24)
Wiki: [Player Action](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerAction)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Action ID | SignedVarInt |  |
|Coordinates | BlockCoordinates |  |
|Result Coordinates | BlockCoordinates |  |
|Face | SignedVarInt |  |
-----------------------------------------------------------------------
### Hurt Armor (0x26)
Wiki: [Hurt Armor](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-HurtArmor)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Cause | SignedVarInt |  |
|Health | SignedVarInt |  |
|Armor slot flags | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Set Entity Motion (0x28)
Wiki: [Set Entity Motion](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityMotion)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Velocity | Vector3 |  |
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
### Set Health (0x2a)
Wiki: [Set Health](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetHealth)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | SignedVarInt |  |
-----------------------------------------------------------------------
### Set Spawn Position (0x2b)
Wiki: [Set Spawn Position](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetSpawnPosition)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Spawn Type | SignedVarInt |  |
|Coordinates | BlockCoordinates |  |
|Dimension | SignedVarInt |  |
|Unknown coordinates | BlockCoordinates |  |
-----------------------------------------------------------------------
### Animate (0x2c)
Wiki: [Animate](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Animate)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Respawn (0x2d)
Wiki: [Respawn](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Respawn)

**Sent from server:** true  
**Sent from client:** true



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
### Container Open (0x2e)
Wiki: [Container Open](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerOpen)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Type | byte |  |
|Coordinates | BlockCoordinates |  |
|Actor Unique ID | SignedVarLong |  |
-----------------------------------------------------------------------
### Container Close (0x2f)
Wiki: [Container Close](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerClose)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Window Type | byte |  |
|Server | bool |  |
-----------------------------------------------------------------------
### Player Hotbar (0x30)
Wiki: [Player Hotbar](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerHotbar)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Selected Slot | UnsignedVarInt |  |
|Window ID | byte |  |
|Select Slot  | bool |  |
-----------------------------------------------------------------------
### Container Set Data (0x33)
Wiki: [Container Set Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerSetData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Property | SignedVarInt |  |
|Value | SignedVarInt |  |
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
### Gui Data Pick Item (0x36)
Wiki: [Gui Data Pick Item](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GuiDataPickItem)

**Sent from server:** true  
**Sent from client:** false

 0x35 CraftingEvent deprecated at 630 (Nov 2023); crafting arrives via 0x93 ItemStackRequest 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Item Name | string |  |
|Item Effects | string |  |
|Hotbar Slot | int |  |
-----------------------------------------------------------------------
### Block Entity Data (0x38)
Wiki: [Block Entity Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockEntityData)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Set Commands Enabled (0x3b)
Wiki: [Set Commands Enabled](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetCommandsEnabled)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Enabled | bool |  |
-----------------------------------------------------------------------
### Set Difficulty (0x3c)
Wiki: [Set Difficulty](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetDifficulty)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Change Dimension (0x3d)
Wiki: [Change Dimension](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ChangeDimension)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Dimension | SignedVarInt |  |
|Position | Vector3 |  |
|Respawn | bool |  |
-----------------------------------------------------------------------
### Set Player Game Type (0x3e)
Wiki: [Set Player Game Type](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetPlayerGameType)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Gamemode | SignedVarInt |  |
-----------------------------------------------------------------------
### Simple Event (0x40)
Wiki: [Simple Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SimpleEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event Type | ushort |  |
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
### Spawn Experience Orb (0x42)
Wiki: [Spawn Experience Orb](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SpawnExperienceOrb)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
|Count | SignedVarInt |  |
-----------------------------------------------------------------------
### Map Info Request (0x44)
Wiki: [Map Info Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MapInfoRequest)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Map ID | SignedVarLong |  |
-----------------------------------------------------------------------
### Request Chunk Radius (0x45)
Wiki: [Request Chunk Radius](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RequestChunkRadius)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | SignedVarInt |  |
|Max Radius | byte |  |
-----------------------------------------------------------------------
### Chunk Radius Update (0x46)
Wiki: [Chunk Radius Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ChunkRadiusUpdate)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | SignedVarInt |  |
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
### Camera (0x49)
Wiki: [Camera](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Camera)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown1 | SignedVarLong |  |
|Unknown2 | SignedVarLong |  |
-----------------------------------------------------------------------
### Show Credits (0x4b)
Wiki: [Show Credits](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ShowCredits)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Status | SignedVarInt |  |
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
### Command Block Update (0x4e)
Wiki: [Command Block Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandBlockUpdate)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Is Block | bool |  |
-----------------------------------------------------------------------
### Command Output (0x4f)
Wiki: [Command Output](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandOutput)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Update Trade (0x50)
Wiki: [Update Trade](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateTrade)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Window Type | byte |  |
|Size | SignedVarInt |  |
|Trade Tier | SignedVarInt |  |
|Trader Entity ID | SignedVarLong |  |
|Player Entity ID | SignedVarLong |  |
|Display Name | string |  |
|Use New Trade Screen | bool |  |
|Using Economy Trade | bool |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Update Equipment (0x51)
Wiki: [Update Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateEquipment)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Window Type | byte |  |
|Size | SignedVarInt |  |
|Entity ID | SignedVarLong |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Resource Pack Data Info (0x52)
Wiki: [Resource Pack Data Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackDataInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Max Chunk Size | uint |  |
|Chunk Count | uint |  |
|Compressed Package Size | ulong |  |
|Hash | ByteArray |  |
|Is Premium | bool |  |
|Pack Type | byte |  |
-----------------------------------------------------------------------
### Resource Pack Chunk Data (0x53)
Wiki: [Resource Pack Chunk Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackChunkData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Chunk Index | uint |  |
|Progress | ulong |  |
|Payload | ByteArray |  |
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
### Stop Sound (0x57)
Wiki: [Stop Sound](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StopSound)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Stop All | bool |  |
|Stop Music Legacy | bool |  |
-----------------------------------------------------------------------
### Set Title (0x58)
Wiki: [Set Title](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetTitle)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | SignedVarInt |  |
|Text | string |  |
|Fade In Time | SignedVarInt |  |
|Stay Time | SignedVarInt |  |
|Fade Out Time | SignedVarInt |  |
|Xuid | string |  |
|Platform Online Id | string |  |
|Filtered Title Text | string |  |
-----------------------------------------------------------------------
### Add Behavior Tree (0x59)
Wiki: [Add Behavior Tree](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddBehaviorTree)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|BehaviorTree | string |  |
-----------------------------------------------------------------------
### Show Store Offer (0x5b)
Wiki: [Show Store Offer](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ShowStoreOffer)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offer Id | UUID |  |
|Redirect Type | byte |  |
-----------------------------------------------------------------------
### Purchase Receipt (0x5c)
Wiki: [Purchase Receipt](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PurchaseReceipt)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Sub Client Login (0x5e)
Wiki: [Sub Client Login](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SubClientLogin)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Connection Request | ByteArray |  |
-----------------------------------------------------------------------
### Initiate Web Socket Connection (0x5f)
Wiki: [Initiate Web Socket Connection](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InitiateWebSocketConnection)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server | string |  |
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
### Npc Request (0x62)
Wiki: [Npc Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NpcRequest)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Unknown0 | byte |  |
|Unknown1 | string |  |
|Unknown2 | byte |  |
|Scene Name | string |  |
-----------------------------------------------------------------------
### Modal Form Request (0x64)
Wiki: [Modal Form Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ModalFormRequest)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Form Id | UnsignedVarInt |  |
|Data | string |  |
-----------------------------------------------------------------------
### Modal Form Response (0x65)
Wiki: [Modal Form Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ModalFormResponse)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Form Id | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Server Settings Request (0x66)
Wiki: [Server Settings Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerSettingsRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Server Settings Response (0x67)
Wiki: [Server Settings Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerSettingsResponse)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Form Id | UnsignedVarInt |  |
|Data | string |  |
-----------------------------------------------------------------------
### Show Profile (0x68)
Wiki: [Show Profile](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ShowProfile)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|XUID | string |  |
-----------------------------------------------------------------------
### Set Default Game Type (0x69)
Wiki: [Set Default Game Type](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetDefaultGameType)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Gamemode | SignedVarInt |  |
-----------------------------------------------------------------------
### Set Display Objective (0x6b)
Wiki: [Set Display Objective](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetDisplayObjective)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Display Slot | string |  |
|Objective Name | string |  |
|Display Name | string |  |
|Criteria Name | string |  |
|Sort Order | SignedVarInt |  |
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
### Update Block Synced (0x6e)
Wiki: [Update Block Synced](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateBlockSynced)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Block Runtime ID | UnsignedVarInt |  |
|Block Priority | UnsignedVarInt |  |
|Data Layer ID | UnsignedVarInt |  |
|Unknown0 | UnsignedVarLong |  |
|Unknown1 | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Set Local Player As Initialized (0x71)
Wiki: [Set Local Player As Initialized](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetLocalPlayerAsInitialized)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
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
### Spawn Particle Effect (0x76)
Wiki: [Spawn Particle Effect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SpawnParticleEffect)

**Sent from server:** true  
**Sent from client:** false

 0x75 ScriptCustomEvent removed from the protocol at 594 (Jun 2023) 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Dimension ID | byte |  |
|Entity ID | SignedVarLong |  |
|Position | Vector3 |  |
|Particle name | string |  |
-----------------------------------------------------------------------
### Available Entity Identifiers (0x77)
Wiki: [Available Entity Identifiers](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AvailableEntityIdentifiers)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Network Chunk Publisher Update (0x79)
Wiki: [Network Chunk Publisher Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NetworkChunkPublisherUpdate)

**Sent from server:** true  
**Sent from client:** false

 0x78 LevelSoundEventV2 removed from the protocol at 785 (Feb 2025); use 0x7b LevelSoundEvent 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Radius | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Biome Definition List (0x7a)
Wiki: [Biome Definition List](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BiomeDefinitionList)

**Sent from server:** true  
**Sent from client:** false




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
### Level Event Generic (0x7c)
Wiki: [Level Event Generic](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelEventGeneric)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event ID | SignedVarInt |  |
|Event Data | NbtBody |  |
-----------------------------------------------------------------------
### Lectern Update (0x7d)
Wiki: [Lectern Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LecternUpdate)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Page | byte |  |
|Total Pages | byte |  |
|Block Position | BlockCoordinates |  |
-----------------------------------------------------------------------
### Client Cache Status (0x81)
Wiki: [Client Cache Status](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientCacheStatus)

**Sent from server:** true  
**Sent from client:** true

 0x7e VideoStreamConnect gone since 407 (Jun 2020); the websocket packet is AutomationClientConnect at 0x5f 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Enabled | bool |  |
-----------------------------------------------------------------------
### On Screen Texture Animation (0x82)
Wiki: [On Screen Texture Animation](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-OnScreenTextureAnimation)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Effect Id | uint |  |
-----------------------------------------------------------------------
### Map Create Locked Copy (0x83)
Wiki: [Map Create Locked Copy](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MapCreateLockedCopy)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Original Map Id | SignedVarLong |  |
|New Map Id | SignedVarLong |  |
-----------------------------------------------------------------------
### Structure Template Data Export Request (0x84)
Wiki: [Structure Template Data Export Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StructureTemplateDataExportRequest)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Position | BlockCoordinates |  |
|Settings | StructureSettings |  |
|Request Type | byte |  |
-----------------------------------------------------------------------
### Structure Template Data Export Response (0x85)
Wiki: [Structure Template Data Export Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StructureTemplateDataExportResponse)

**Sent from server:** true  
**Sent from client:** false




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
### Emote (0x8a)
Wiki: [Emote](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Emote)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Emote Id | string |  |
|Emote Length Ticks | UnsignedVarInt |  |
|Xbox User Id | string |  |
|Platform Chat Id | string |  |
|Flags | byte |  |
-----------------------------------------------------------------------
### Multiplayer Settings (0x8b)
Wiki: [Multiplayer Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MultiplayerSettings)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action | SignedVarInt |  |
-----------------------------------------------------------------------
### Settings Command (0x8c)
Wiki: [Settings Command](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SettingsCommand)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Command | string |  |
|Suppress Output | bool |  |
-----------------------------------------------------------------------
### Completed Using Item (0x8e)
Wiki: [Completed Using Item](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CompletedUsingItem)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Item Id | short |  |
|Action | int |  |
-----------------------------------------------------------------------
### Network Settings (0x8f)
Wiki: [Network Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NetworkSettings)

**Sent from server:** true  
**Sent from client:** false



#### Compressionalgorithm constants

| Name | Value |
|:-----|:-----|
|Zlib | 0 |
|Snappy | 1 |
|None | 65535 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Compression threshold | ushort |  |
|Compression algorithm | ushort |  |
|Client throttle enabled | bool |  |
|Client throttle threshold | byte |  |
|Client throttle scalar | float |  |
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
### Code Builder (0x96)
Wiki: [Code Builder](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CodeBuilder)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|URL | string |  |
|Open Code Builder | bool |  |
-----------------------------------------------------------------------
### Update Player Game Type (0x97)
Wiki: [Update Player Game Type](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdatePlayerGameType)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Player Game Type | SignedVarInt |  |
|Target Player Unique ID | SignedVarLong |  |
|Tick | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Emote List (0x98)
Wiki: [Emote List](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EmoteList)

**Sent from server:** false  
**Sent from client:** true

 Sent by the client right after login, listing the emotes it owns so the server can
     validate an Emote packet later. The piece id list is a UUID array, which the generator has
     no type for, so it lives in the partial (Net/McpeEmoteList.cs). 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Position Tracking Db Server Broadcast (0x99)
Wiki: [Position Tracking Db Server Broadcast](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PositionTrackingDbServerBroadcast)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action | byte |  |
|Tracking ID | SignedVarInt |  |
|NBT | NbtBody |  |
-----------------------------------------------------------------------
### Position Tracking Db Client Request (0x9a)
Wiki: [Position Tracking Db Client Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PositionTrackingDbClientRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action | byte |  |
|Tracking ID | SignedVarInt |  |
-----------------------------------------------------------------------
### Debug Info (0x9b)
Wiki: [Debug Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-DebugInfo)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Actor Unique ID | SignedVarLong |  |
|Data | string |  |
-----------------------------------------------------------------------
### Packet Violation Warning (0x9c)
Wiki: [Packet Violation Warning](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PacketViolationWarning)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Violation Type | SignedVarInt |  |
|Severity | SignedVarInt |  |
|Packet Id | SignedVarInt |  |
|Reason | string |  |
-----------------------------------------------------------------------
### Motion Prediction Hints (0x9d)
Wiki: [Motion Prediction Hints](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MotionPredictionHints)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Motion | Vector3 |  |
|On Ground | bool |  |
-----------------------------------------------------------------------
### Animate Entity (0x9e)
Wiki: [Animate Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AnimateEntity)

**Sent from server:** true  
**Sent from client:** false

 Trailing runtime entity id list is a VarInt-count-prefixed array of UnsignedVarLong,
     which the generator has no type for, so it lives in the partial (Net/McpeAnimateEntity.cs). 


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Animation | string |  |
|Next State | string |  |
|Stop Expression | string |  |
|Stop Expression Version | int |  |
|Controller | string |  |
|Blend Out Time | float |  |
-----------------------------------------------------------------------
### Player Fog (0xa0)
Wiki: [Player Fog](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerFog)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Correct Player Move Prediction (0xa1)
Wiki: [Correct Player Move Prediction](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CorrectPlayerMovePrediction)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Prediction Type | byte |  |
|Position | Vector3 |  |
|Delta | Vector3 |  |
|Rotation Pitch | float |  |
|Rotation Yaw | float |  |
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
### Sync Entity Property (0xa5)
Wiki: [Sync Entity Property](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SyncEntityProperty)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Add Volume Entity (0xa6)
Wiki: [Add Volume Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddVolumeEntity)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Network ID | UnsignedVarInt |  |
|Data | Nbt |  |
|JSON Identifier | string |  |
|Instance Name | string |  |
|Min Bounds | BlockCoordinates |  |
|Max Bounds | BlockCoordinates |  |
|Dimension | SignedVarInt |  |
|Engine Version | string |  |
-----------------------------------------------------------------------
### Remove Volume Entity (0xa7)
Wiki: [Remove Volume Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RemoveVolumeEntity)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Network ID | UnsignedVarInt |  |
|Dimension | SignedVarInt |  |
-----------------------------------------------------------------------
### Simulation Type (0xa8)
Wiki: [Simulation Type](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SimulationType)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Simulation Type | byte |  |
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
### Player Start Item Cooldown (0xb0)
Wiki: [Player Start Item Cooldown](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerStartItemCooldown)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Item Category | string |  |
|Cooldown Ticks | SignedVarInt |  |
-----------------------------------------------------------------------
### Script Message (0xb1)
Wiki: [Script Message](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ScriptMessage)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Message Id | string |  |
|Message Value | string |  |
-----------------------------------------------------------------------
### Code Builder Source (0xb2)
Wiki: [Code Builder Source](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CodeBuilderSource)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Operation | byte |  |
|Category | byte |  |
|Code Status | byte |  |
-----------------------------------------------------------------------
### Ticking Areas Load Status (0xb3)
Wiki: [Ticking Areas Load Status](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TickingAreasLoadStatus)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Waiting For Preload | bool |  |
-----------------------------------------------------------------------
### Agent Action Event (0xb5)
Wiki: [Agent Action Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AgentActionEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Request Id | string |  |
|Action | int |  |
|Response Json | string |  |
-----------------------------------------------------------------------
### Change Mob Property (0xb6)
Wiki: [Change Mob Property](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ChangeMobProperty)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Actor Unique Id | SignedVarLong |  |
|Property Name | string |  |
|Bool Value | bool |  |
|String Value | string |  |
|Int Value | SignedVarInt |  |
|Float Value | float |  |
-----------------------------------------------------------------------
### Lesson Progress (0xb7)
Wiki: [Lesson Progress](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LessonProgress)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action | SignedVarInt |  |
|Score | SignedVarInt |  |
|Activity Id | string |  |
-----------------------------------------------------------------------
### Request Permissions (0xb9)
Wiki: [Request Permissions](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RequestPermissions)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Target Actor Unique ID | long |  |
|Player Permission | SignedVarInt |  |
|Custom Flags | ushort |  |
-----------------------------------------------------------------------
### Toast Request (0xba)
Wiki: [Toast Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ToastRequest)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Title | string |  |
|Content | string |  |
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
### Server Stats (0xc0)
Wiki: [Server Stats](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerStats)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server Time | float |  |
|Network Time | float |  |
-----------------------------------------------------------------------
### Request Network Settings (0xc1)
Wiki: [Request Network Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RequestNetworkSettings)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Protocol Version | int |  |
-----------------------------------------------------------------------
### Game Test Request (0xc2)
Wiki: [Game Test Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GameTestRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Max Tests Per Batch | SignedVarInt |  |
|Repeat Count | SignedVarInt |  |
|Rotation | byte |  |
|Stop On Failure | bool |  |
|Test Position | BlockCoordinates |  |
|Tests Per Row | SignedVarInt |  |
|Test Name | string |  |
-----------------------------------------------------------------------
### Game Test Results (0xc3)
Wiki: [Game Test Results](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GameTestResults)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Success | bool |  |
|Error | string |  |
|Test Name | string |  |
-----------------------------------------------------------------------
### Update Client Input Locks (0xc4)
Wiki: [Update Client Input Locks](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateClientInputLocks)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | UnsignedVarInt |  |
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
### Open Sign (0x12f)
Wiki: [Open Sign](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-OpenSign)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Block Position | BlockCoordinates |  |
|Front | bool |  |
-----------------------------------------------------------------------
### Agent Animation (0x130)
Wiki: [Agent Animation](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AgentAnimation)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Animation Type | byte |  |
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Refresh Entitlements (0x131)
Wiki: [Refresh Entitlements](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RefreshEntitlements)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Player Toggle Crafter Slot Request (0x132)
Wiki: [Player Toggle Crafter Slot Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerToggleCrafterSlotRequest)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Pos X | int |  |
|Pos Y | int |  |
|Pos Z | int |  |
|Slot Index | byte |  |
|Is Disabled | bool |  |
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
### Set Hud (0x134)
Wiki: [Set Hud](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetHud)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Award Achievement (0x135)
Wiki: [Award Achievement](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AwardAchievement)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Achievement ID | int |  |
-----------------------------------------------------------------------
### Clientbound Close Form (0x136)
Wiki: [Clientbound Close Form](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundCloseForm)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Server Bound Loading Screen (0x138)
Wiki: [Server Bound Loading Screen](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerBoundLoadingScreen)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | SignedVarInt |  |
-----------------------------------------------------------------------
### Jigsaw Structure Data (0x139)
Wiki: [Jigsaw Structure Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-JigsawStructureData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Structure Data | Nbt |  |
-----------------------------------------------------------------------
### Current Structure Feature (0x13a)
Wiki: [Current Structure Feature](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CurrentStructureFeature)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Current Feature | string |  |
-----------------------------------------------------------------------
### Camera Aim Assist (0x13c)
Wiki: [Camera Aim Assist](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraAimAssist)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Preset ID | string |  |
|View Angle | Vector2 |  |
|Distance | float |  |
|Target Mode | byte |  |
|Action Type | byte |  |
|Show Debug Render | bool |  |
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
### Update Client Options (0x143)
Wiki: [Update Client Options](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateClientOptions)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
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
### Clientbound Control Scheme Set (0x147)
Wiki: [Clientbound Control Scheme Set](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundControlSchemeSet)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Control Scheme | byte |  |
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
### Clientbound Data Driven Ui Show Screen (0x14d)
Wiki: [Clientbound Data Driven Ui Show Screen](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundDataDrivenUiShowScreen)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Screen Id | string |  |
|Form Id | uint |  |
-----------------------------------------------------------------------
### Clientbound Data Driven Ui Close Screen (0x14e)
Wiki: [Clientbound Data Driven Ui Close Screen](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundDataDrivenUiCloseScreen)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Clientbound Data Driven Ui Reload (0x14f)
Wiki: [Clientbound Data Driven Ui Reload](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundDataDrivenUiReload)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Clientbound Texture Shift (0x150)
Wiki: [Clientbound Texture Shift](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundTextureShift)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action Id | byte |  |
|Collection Name | string |  |
|From Step | string |  |
|To Step | string |  |
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
### Resource Packs Ready For Validation (0x154)
Wiki: [Resource Packs Ready For Validation](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePacksReadyForValidation)

**Sent from server:** false  
**Sent from client:** true




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
### Camera Shake (0x9f)
Wiki: [Camera Shake](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CameraShake)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Intensity | float |  |
|Duration | float |  |
|Type | byte |  |
|Action | byte |  |
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
### Sync World Clocks (0x158)
Wiki: [Sync World Clocks](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SyncWorldClocks)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload Type | UnsignedVarInt |  |
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
### Party Destination Cookie Response (0x15e)
Wiki: [Party Destination Cookie Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PartyDestinationCookieResponse)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Cookie | string |  |
|Accepted | bool |  |
-----------------------------------------------------------------------


