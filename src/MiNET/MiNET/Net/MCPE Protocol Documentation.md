
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

Read more about packets and this specification on the [Protocol Wiki](https://github.com/NiclasOlofsson/MiNET/wiki//ref-protocol)

## ALL PACKAGES

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Login | 0x01 | 1 |   
| Play Status | 0x02 | 2 |   
| Server To Client Handshake | 0x03 | 3 |   
| Client To Server Handshake | 0x04 | 4 |   
| Disconnect | 0x05 | 5 |   
| Resource Packs Info | 0x06 | 6 |   
| Resource Pack Stack | 0x07 | 7 |   
| Resource Pack Client Response | 0x08 | 8 |   
| Text | 0x09 | 9 |   
| Set Time | 0x0a | 10 |   
| Start Game | 0x0b | 11 |   
| Add Player | 0x0c | 12 |   
| Add Entity | 0x0d | 13 |   
| Remove Entity | 0x0e | 14 |   
| Add Item Entity | 0x0f | 15 |   
| Take Item Entity | 0x11 | 17 |   
| Move Entity | 0x12 | 18 |   
| Move Player | 0x13 | 19 |   
| Rider Jump | 0x14 | 20 |   
| Update Block | 0x15 | 21 |   
| Add Painting | 0x16 | 22 |   
| Explode | 0x17 | 23 |   
| Level Sound Event | 0x18 | 24 |   
| Level Event | 0x19 | 25 |   
| Block Event | 0x1a | 26 |   
| Entity Event | 0x1b | 27 |   
| Mob Effect | 0x1c | 28 |   
| Update Attributes | 0x1d | 29 |   
| Inventory Transaction | 0x1e | 30 |   
| Mob Equipment | 0x1f | 31 |   
| Mob Armor Equipment | 0x20 | 32 |   
| Interact | 0x21 | 33 |   
| Block Pick Request | 0x22 | 34 |   
| Entity Pick Request | 0x23 | 35 |   
| Player Action | 0x24 | 36 |   
| Entity Fall | 0x25 | 37 |   
| Hurt Armor | 0x26 | 38 |   
| Set Entity Data | 0x27 | 39 |   
| Set Entity Motion | 0x28 | 40 |   
| Set Entity Link | 0x29 | 41 |   
| Set Health | 0x2a | 42 |   
| Set Spawn Position | 0x2b | 43 |   
| Animate | 0x2c | 44 |   
| Respawn | 0x2d | 45 |   
| Container Open | 0x2e | 46 |   
| Container Close | 0x2f | 47 |   
| Player Hotbar | 0x30 | 48 |   
| Inventory Content | 0x31 | 49 |   
| Inventory Slot | 0x32 | 50 |   
| Container Set Data | 0x33 | 51 |   
| Crafting Data | 0x34 | 52 |   
| Crafting Event | 0x35 | 53 |   
| Gui Data Pick Item | 0x36 | 54 |   
| Adventure Settings | 0x37 | 55 |   
| Block Entity Data | 0x38 | 56 |   
| Player Input | 0x39 | 57 |   
| Full Chunk Data | 0x3a | 58 |   
| Set Commands Enabled | 0x3b | 59 |   
| Set Difficulty | 0x3c | 60 |   
| Change Dimension | 0x3d | 61 |   
| Set Player Game Type | 0x3e | 62 |   
| Player List | 0x3f | 63 |   
| Simple Event | 0x40 | 64 |   
| Telemetry Event | 0x41 | 65 |   
| Spawn Experience Orb | 0x42 | 66 |   
| Clientbound Map Item Data  | 0x43 | 67 |   
| Map Info Request | 0x44 | 68 |   
| Request Chunk Radius | 0x45 | 69 |   
| Chunk Radius Update | 0x46 | 70 |   
| Item Frame Drop Item | 0x47 | 71 |   
| Game Rules Changed | 0x48 | 72 |   
| Camera | 0x49 | 73 |   
| Boss Event | 0x4a | 74 |   
| Show Credits | 0x4b | 75 |   
| Available Commands | 0x4c | 76 |   
| Command Request | 0x4d | 77 |   
| Command Block Update | 0x4e | 78 |   
| Update Trade | 0x50 | 80 |   
| Update Equipment | 0x51 | 81 |   
| Resource Pack Data Info | 0x52 | 82 |   
| Resource Pack Chunk Data | 0x53 | 83 |   
| Resource Pack Chunk Request | 0x54 | 84 |   
| Transfer | 0x55 | 85 |   
| Play Sound | 0x56 | 86 |   
| Stop Sound | 0x57 | 87 |   
| Set Title | 0x58 | 88 |   
| Add Behavior Tree | 0x59 | 89 |   
| Structure Block Update | 0x5a | 90 |   
| Show Store Offer | 0x5b | 91 |   
| Purchase Receipt | 0x5c | 92 |   
| Player Skin | 0x5d | 93 |   
| Sub Client Login | 0x5e | 94 |   
| Initiate Web Socket Connection | 0x5f | 95 |   
| Set Last Hurt By | 0x60 | 96 |   
| Book Edit | 0x61 | 97 |   
| Npc Request | 0x62 | 98 |   
| Modal Form Request | 0x64 | 100 |   
| Modal Form Response | 0x65 | 101 |   
| Server Settings Request | 0x66 | 102 |   
| Server Settings Response | 0x67 | 103 |   


## Data types

| Data type | 
|:--- |
| BlockCoordinates [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-BlockCoordinates) |
| bool [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-bool) |
| byte [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte) |
| byte[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-byte[]) |
| ByteArray [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ByteArray) |
| EntityAttributes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-EntityAttributes) |
| FixedString [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-FixedString) |
| float [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-float) |
| GameRules [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-GameRules) |
| int [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-int) |
| IPEndPoint [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-IPEndPoint) |
| IPEndPoint[] [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-IPEndPoint[]) |
| Item [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Item) |
| ItemStacks [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ItemStacks) |
| Links [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Links) |
| long [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-long) |
| MapInfo [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-MapInfo) |
| MetadataDictionary [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-MetadataDictionary) |
| MetadataInts [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-MetadataInts) |
| Nbt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Nbt) |
| OFFLINE_MESSAGE_DATA_ID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-OFFLINE_MESSAGE_DATA_ID) |
| PlayerAttributes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerAttributes) |
| PlayerLocation [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerLocation) |
| PlayerRecords [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-PlayerRecords) |
| Recipes [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Recipes) |
| Records [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Records) |
| ResourcePackIds [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ResourcePackIds) |
| ResourcePackIdVersions [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ResourcePackIdVersions) |
| ResourcePackInfos [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ResourcePackInfos) |
| short [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-short) |
| SignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarInt) |
| SignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-SignedVarLong) |
| Skin [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Skin) |
| string [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-string) |
| Transaction [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Transaction) |
| uint [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-uint) |
| ulong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ulong) |
| UnsignedVarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarInt) |
| UnsignedVarLong [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UnsignedVarLong) |
| ushort [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-ushort) |
| UUID [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-UUID) |
| VarInt [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-VarInt) |
| Vector2 [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Vector2) |
| Vector3 [(wiki)](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Type-Vector3) |

## Constants
	OFFLINE_MESSAGE_DATA_ID
	byte[]
	{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }

## Packages

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


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Status | int |  |
-----------------------------------------------------------------------
### Server To Client Handshake (0x03)
Wiki: [Server To Client Handshake](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerToClientHandshake)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Token | string |  |
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
|Hide disconnect reason | bool |  |
|Message | string |  |
-----------------------------------------------------------------------
### Resource Packs Info (0x06)
Wiki: [Resource Packs Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePacksInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Must accept | bool |  |
|BehahaviorPackInfos | ResourcePackInfos |  |
|ResourcePackInfos | ResourcePackInfos |  |
-----------------------------------------------------------------------
### Resource Pack Stack (0x07)
Wiki: [Resource Pack Stack](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackStack)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Must accept | bool |  |
|BehaviorPackIdVersions | ResourcePackIdVersions |  |
|ResourcePackIdVersions | ResourcePackIdVersions |  |
-----------------------------------------------------------------------
### Resource Pack Client Response (0x08)
Wiki: [Resource Pack Client Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackClientResponse)

**Sent from server:** false  
**Sent from client:** true



#### Response Status constants

| Name | Value |
|:-----|:-----|
|Refused | 1 |
|Send Packs | 2 |
|Have All Packs | 3 |
|Completed | 4 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Response status | byte |  |
|ResourcePackIds | ResourcePackIds |  |
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


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | byte |  |
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
### Start Game (0x0b)
Wiki: [Start Game](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StartGame)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Runtime Entity ID | UnsignedVarLong |  |
|Player Gamemode | SignedVarInt |  |
|Spawn | Vector3 |  |
|Unknown 1 | Vector2 |  |
|Seed | SignedVarInt |  |
|Dimension | SignedVarInt |  |
|Generator | SignedVarInt |  |
|Gamemode | SignedVarInt |  |
|Difficulty | SignedVarInt |  |
|X | SignedVarInt |  |
|Y | SignedVarInt |  |
|Z | SignedVarInt |  |
|Has achievements disabled | bool |  |
|Day cycle stop time | SignedVarInt |  |
|EDU mode | bool |  |
|Rain level | float |  |
|Lightnig level | float |  |
|Is Multiplayer | bool |  |
|Broadcast To LAN | bool |  |
|Broadcast To XBL | bool |  |
|Enable commands | bool |  |
|Is texturepacks required | bool |  |
|GameRules | GameRules |  |
|Bonus Chest | bool |  |
|Map Enabled | bool |  |
|Trust Players | bool |  |
|Permission Level | SignedVarInt |  |
|Game Publish Setting | SignedVarInt |  |
|Level ID | string |  |
|World name | string |  |
|Premium World Template Id | string |  |
|Unknown0 | bool |  |
|Current Tick | long |  |
|Enchantment Seed | SignedVarInt |  |
-----------------------------------------------------------------------
### Add Player (0x0c)
Wiki: [Add Player](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddPlayer)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|UUID | UUID |  |
|Username | string |  |
|Entity ID Self | SignedVarLong |  |
|Runtime Entity ID | UnsignedVarLong |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Pitch | float |  |
|Yaw | float |  |
|Head Yaw | float |  |
|Item | Item |  |
|Metadata | MetadataDictionary |  |
|Flags | UnsignedVarInt |  |
|User Permission | UnsignedVarInt |  |
|Action Permissions | UnsignedVarInt |  |
|Permission Level | UnsignedVarInt |  |
|Unknown | UnsignedVarInt |  |
|User Id | long |  |
|Links | Links |  |
-----------------------------------------------------------------------
### Add Entity (0x0d)
Wiki: [Add Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddEntity)

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
|Runtime Entity ID | UnsignedVarLong |  |
|Entity Type | UnsignedVarInt |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Pitch | float |  |
|Yaw | float |  |
|Attributes | EntityAttributes |  |
|Metadata | MetadataDictionary |  |
|Links | Links |  |
-----------------------------------------------------------------------
### Remove Entity (0x0e)
Wiki: [Remove Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RemoveEntity)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
-----------------------------------------------------------------------
### Add Item Entity (0x0f)
Wiki: [Add Item Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddItemEntity)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Runtime Entity ID | UnsignedVarLong |  |
|Item | Item |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Metadata | MetadataDictionary |  |
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
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Position | PlayerLocation |  |
|On Ground | bool |  |
|Teleport | bool |  |
-----------------------------------------------------------------------
### Move Player (0x13)
Wiki: [Move Player](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MovePlayer)

**Sent from server:** true  
**Sent from client:** true



#### Mode constants

| Name | Value |
|:-----|:-----|
|Normal | 0 |
|Reset | 1 |
|Rotation | 2 |
|Pitch | 3 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Pitch | float |  |
|Yaw | float |  |
|Head Yaw | float |  |
|Mode | byte |  |
|On Ground | bool |  |
|Other Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Rider Jump (0x14)
Wiki: [Rider Jump](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RiderJump)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown | SignedVarInt |  |
-----------------------------------------------------------------------
### Update Block (0x15)
Wiki: [Update Block](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateBlock)

**Sent from server:** true  
**Sent from client:** false



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
|Block ID | UnsignedVarInt |  |
|Block Meta And Priority | UnsignedVarInt |  |
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
|Coordinates | BlockCoordinates |  |
|Direction | SignedVarInt |  |
|Title | string |  |
-----------------------------------------------------------------------
### Explode (0x17)
Wiki: [Explode](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Explode)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
|Radius | SignedVarInt |  |
|Records | Records |  |
-----------------------------------------------------------------------
### Level Sound Event (0x18)
Wiki: [Level Sound Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelSoundEvent)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Sound ID | byte |  |
|Position | Vector3 |  |
|Block Id | SignedVarInt |  |
|Entity Type | SignedVarInt |  |
|Is baby mob | bool |  |
|Is global | bool |  |
-----------------------------------------------------------------------
### Level Event (0x19)
Wiki: [Level Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelEvent)

**Sent from server:** true  
**Sent from client:** false




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
|Unknown | SignedVarInt |  |
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
-----------------------------------------------------------------------
### Inventory Transaction (0x1e)
Wiki: [Inventory Transaction](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventoryTransaction)

**Sent from server:** true  
**Sent from client:** true



#### Transaction Type constants

| Name | Value |
|:-----|:-----|
|Normal | 0 |
|Inventory Mismatch | 1 |
|Item Use | 2 |
|Item Use On Entity | 3 |
|Item Release | 4 |

#### Inventory Source Type constants

| Name | Value |
|:-----|:-----|
|Container | 0 |
|Global | 1 |
|World Interaction | 2 |
|Creative | 3 |
|Unspecified | 99999 |

#### Normal Action constants

| Name | Value |
|:-----|:-----|
|Put Slot | -2 |
|Get Slot | -3 |
|Get Result | -4 |
|Craft Use | -5 |
|Enchant Item | 29 |
|Enchant Lapis | 31 |
|Enchant Result | 33 |
|Drop | 199 |

#### Item Release Action constants

| Name | Value |
|:-----|:-----|
|Release | 0 |
|Use | 1 |

#### Item Use Action constants

| Name | Value |
|:-----|:-----|
|Place | 0 |
|Use | 1 |
|Destroy | 2 |

#### Item Use On Entity Action constants

| Name | Value |
|:-----|:-----|
|Interact | 0 |
|Attack | 1 |
|Item Interact | 2 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Transaction | Transaction |  |
-----------------------------------------------------------------------
### Mob Equipment (0x1f)
Wiki: [Mob Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobEquipment)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Item | Item |  |
|Slot | byte |  |
|Selected Slot | byte |  |
|Windows Id | byte |  |
-----------------------------------------------------------------------
### Mob Armor Equipment (0x20)
Wiki: [Mob Armor Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobArmorEquipment)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Helmet | Item |  |
|Chestplate | Item |  |
|Leggings | Item |  |
|Boots | Item |  |
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
|Leave Cehicle | 3 |
|Mouse Over | 4 |


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
|Selected Slot | byte |  |
-----------------------------------------------------------------------
### Entity Pick Request (0x23)
Wiki: [Entity Pick Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityPickRequest)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
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
|Face | SignedVarInt |  |
-----------------------------------------------------------------------
### Entity Fall (0x25)
Wiki: [Entity Fall](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityFall)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Fall distance | float |  |
|Unknown | bool |  |
-----------------------------------------------------------------------
### Hurt Armor (0x26)
Wiki: [Hurt Armor](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-HurtArmor)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | SignedVarInt |  |
-----------------------------------------------------------------------
### Set Entity Data (0x27)
Wiki: [Set Entity Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
### Set Entity Motion (0x28)
Wiki: [Set Entity Motion](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityMotion)

**Sent from server:** true  
**Sent from client:** false




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
|Rider ID | UnsignedVarLong |  |
|Ridden ID | UnsignedVarLong |  |
|Link Type | byte |  |
|Unknown | byte |  |
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
|Forced | bool |  |
-----------------------------------------------------------------------
### Animate (0x2c)
Wiki: [Animate](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Animate)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | SignedVarInt |  |
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Respawn (0x2d)
Wiki: [Respawn](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Respawn)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
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
|Unknown Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Container Close (0x2f)
Wiki: [Container Close](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerClose)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
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
|Hotbar Data | MetadataInts |  |
|Unknown | byte |  |
-----------------------------------------------------------------------
### Inventory Content (0x31)
Wiki: [Inventory Content](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventoryContent)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Inventory Id | UnsignedVarInt |  |
|Input | ItemStacks |  |
-----------------------------------------------------------------------
### Inventory Slot (0x32)
Wiki: [Inventory Slot](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventorySlot)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Inventory Id | UnsignedVarInt |  |
|Slot | UnsignedVarInt |  |
|Item | Item |  |
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
-----------------------------------------------------------------------
### Crafting Event (0x35)
Wiki: [Crafting Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CraftingEvent)

**Sent from server:** true  
**Sent from client:** true



#### Recipe Types constants

| Name | Value |
|:-----|:-----|
|Shapeless | 0 |
|Shaped | 1 |
|Furnace | 2 |
|Furnace Data | 3 |
|Multi | 4 |
|Shulker Box | 5 |


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Recipe Type | SignedVarInt |  |
|Recipe ID | UUID |  |
|Input | ItemStacks |  |
|Result | ItemStacks |  |
-----------------------------------------------------------------------
### Gui Data Pick Item (0x36)
Wiki: [Gui Data Pick Item](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GuiDataPickItem)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Adventure Settings (0x37)
Wiki: [Adventure Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AdventureSettings)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | UnsignedVarInt |  |
|Command permission | UnsignedVarInt |  |
|Action permissions | UnsignedVarInt |  |
|Permission level | UnsignedVarInt |  |
|Custom stored permissions | UnsignedVarInt |  |
|User Id | long |  |
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
### Player Input (0x39)
Wiki: [Player Input](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerInput)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Motion X | float |  |
|Motion Z | float |  |
|Flag1 | bool |  |
|Flag2 | bool |  |
-----------------------------------------------------------------------
### Full Chunk Data (0x3a)
Wiki: [Full Chunk Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-FullChunkData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk X | SignedVarInt |  |
|Chunk Z | SignedVarInt |  |
|Chunk Data | ByteArray |  |
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
|Unknown | bool |  |
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
### Player List (0x3f)
Wiki: [Player List](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerList)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | PlayerRecords |  |
-----------------------------------------------------------------------
### Simple Event (0x40)
Wiki: [Simple Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SimpleEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Telemetry Event (0x41)
Wiki: [Telemetry Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TelemetryEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Unk1 | SignedVarInt |  |
|Unk2 | byte |  |
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
### Clientbound Map Item Data  (0x43)
Wiki: [Clientbound Map Item Data ](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundMapItemData)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|MapInfo | MapInfo |  |
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
### Item Frame Drop Item (0x47)
Wiki: [Item Frame Drop Item](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ItemFrameDropItem)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
-----------------------------------------------------------------------
### Game Rules Changed (0x48)
Wiki: [Game Rules Changed](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GameRulesChanged)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Rules | GameRules |  |
-----------------------------------------------------------------------
### Camera (0x49)
Wiki: [Camera](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Camera)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Boss Event (0x4a)
Wiki: [Boss Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BossEvent)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Boss Entity ID | SignedVarLong |  |
|Event Type | UnsignedVarInt |  |
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
|Command type | SignedVarInt |  |
|Request ID | string |  |
|Unknown | bool |  |
-----------------------------------------------------------------------
### Command Block Update (0x4e)
Wiki: [Command Block Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandBlockUpdate)

**Sent from server:** false  
**Sent from client:** true




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
|Unknown0 | VarInt |  |
|Unknown1 | VarInt |  |
|Is Willing | bool |  |
|Trader Entity ID | SignedVarLong |  |
|Player Entity ID | SignedVarLong |  |
|Display Name | string |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Update Equipment (0x51)
Wiki: [Update Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateEquipment)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Resource Pack Data Info (0x52)
Wiki: [Resource Pack Data Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackDataInfo)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Max Chunk Size  | uint |  |
|Chunk Count  | uint |  |
|Compressed Package Size  | ulong |  |
|Hash  | string |  |
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
|Length | uint |  |
|Payload | byte[] | (int) length |
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
### Transfer (0x55)
Wiki: [Transfer](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Transfer)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server Address | string |  |
|Port | ushort |  |
-----------------------------------------------------------------------
### Play Sound (0x56)
Wiki: [Play Sound](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlaySound)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Coordinates | BlockCoordinates |  |
|Volume | float |  |
|Pitch | float |  |
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
### Structure Block Update (0x5a)
Wiki: [Structure Block Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StructureBlockUpdate)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Show Store Offer (0x5b)
Wiki: [Show Store Offer](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ShowStoreOffer)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown0 | string |  |
|Unknown1 | bool |  |
-----------------------------------------------------------------------
### Purchase Receipt (0x5c)
Wiki: [Purchase Receipt](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PurchaseReceipt)

**Sent from server:** false  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Player Skin (0x5d)
Wiki: [Player Skin](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerSkin)

**Sent from server:** true  
**Sent from client:** true




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|UUID | UUID |  |
|Skin ID | string |  |
|Skin Name | string |  |
|Old Skin Name | string |  |
|Skin Data | ByteArray |  |
|Cape Data | ByteArray |  |
|Geometry Model | string |  |
|Geometry Data | string |  |
-----------------------------------------------------------------------
### Sub Client Login (0x5e)
Wiki: [Sub Client Login](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SubClientLogin)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
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
-----------------------------------------------------------------------
### Npc Request (0x62)
Wiki: [Npc Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NpcRequest)

**Sent from server:** true  
**Sent from client:** false




#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Unknown0 | byte |  |
|Unknown1 | string |  |
|Unknown2 | byte |  |
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
|Data | string |  |
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
|Form Id | UnsignedVarLong |  |
|Data | string |  |
-----------------------------------------------------------------------


