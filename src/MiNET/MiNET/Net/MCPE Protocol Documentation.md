
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
| Inventory Transaction Packet | 0x1e | 30 |   
| Mob Equipment | 0x1f | 31 |   
| Mob Armor Equipment | 0x20 | 32 |   
| Interact | 0x21 | 33 |   
| Block Pick Request | 0x22 | 34 |   
| Entity Pick Request Packet | 0x23 | 35 |   
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
| Player Hotbar Packet | 0x30 | 48 |   
| Inventory Content Packet | 0x31 | 49 |   
| Inventory Slot Packet | 0x32 | 50 |   
| Container Set Data | 0x33 | 51 |   
| Crafting Data | 0x34 | 52 |   
| Crafting Event | 0x35 | 53 |   
| Gui Data Pick Item Packet | 0x36 | 54 |   
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
| Telemetry Event Packet | 0x41 | 65 |   
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
| Command Request Packet | 0x4d | 77 |   
| Command Block Update | 0x4e | 78 |   
| Update Trade | 0x50 | 80 |   
| Update Equipment Packet | 0x51 | 81 |   
| Resource Pack Data Info | 0x52 | 82 |   
| Resource Pack Chunk Data | 0x53 | 83 |   
| Resource Pack Chunk Request | 0x54 | 84 |   
| Transfer | 0x55 | 85 |   
| Play Sound | 0x56 | 86 |   
| Stop Sound | 0x57 | 87 |   
| Set Title | 0x58 | 88 |   
| Add Behavior Tree Packet | 0x59 | 89 |   
| Structure Block Update Packet | 0x5a | 90 |   
| Player Skin Packet | 0x5d | 93 |   
| Sub Client Login Packet | 0x5e | 94 |   
| Initiate Web Socket Connection Packet | 0x5f | 95 |   
| Set Last Hurt By Packet | 0x60 | 96 |   
| Book Edit Packet | 0x61 | 97 |   
| Npc Request Packet | 0x62 | 98 |   


## Constants
	OFFLINE_MESSAGE_DATA_ID
	byte[]
	{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }

## Packages

### Package: Login (0x01)
Wiki: [Package-Login](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Login)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Protocol Version | int |  |
|Payload | ByteArray |  |
-----------------------------------------------------------------------
### Package: Play Status (0x02)
Wiki: [Package-Play Status](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayStatus)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 


The three type of status are:

LOGIN_SUCCESS = 0;
LOGIN_FAILED_CLIENT = 1;
LOGIN_FAILED_SERVER = 2;
PLAYER_SPAWN = 3;
LOGIN_FAILED_INVALID_TENANT = 4;
LOGIN_FAILED_VANILLA_EDU = 5;
LOGIN_FAILED_EDU_VANILLA = 6;


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Status | int |  |
-----------------------------------------------------------------------
### Package: Server To Client Handshake (0x03)
Wiki: [Package-Server To Client Handshake](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ServerToClientHandshake)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server Public Key | string |  |
|Token Length | Length |  |
|Token | byte[] | 0 |
-----------------------------------------------------------------------
### Package: Client To Server Handshake (0x04)
Wiki: [Package-Client To Server Handshake](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientToServerHandshake)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Disconnect (0x05)
Wiki: [Package-Disconnect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Disconnect)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Hide disconnect reason | bool |  |
|Message | string |  |
-----------------------------------------------------------------------
### Package: Resource Packs Info (0x06)
Wiki: [Package-Resource Packs Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePacksInfo)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Must accept | bool |  |
|BehahaviorPackInfos | ResourcePackInfos |  |
|ResourcePackInfos | ResourcePackInfos |  |
-----------------------------------------------------------------------
### Package: Resource Pack Stack (0x07)
Wiki: [Package-Resource Pack Stack](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackStack)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Must accept | bool |  |
|BehaviorPackIdVersions | ResourcePackIdVersions |  |
|ResourcePackIdVersions | ResourcePackIdVersions |  |
-----------------------------------------------------------------------
### Package: Resource Pack Client Response (0x08)
Wiki: [Package-Resource Pack Client Response](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackClientResponse)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Response status | byte |  |
|ResourcePackIds | ResourcePackIds |  |
-----------------------------------------------------------------------
### Package: Text (0x09)
Wiki: [Package-Text](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Text)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | byte |  |
-----------------------------------------------------------------------
### Package: Set Time (0x0a)
Wiki: [Package-Set Time](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetTime)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Start Game (0x0b)
Wiki: [Package-Start Game](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StartGame)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



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
|Enable commands | bool |  |
|Is texturepacks required | bool |  |
|GameRules | GameRules |  |
|Level ID | string |  |
|World name | string |  |
|Premium World Template Id | string |  |
|Unknown0 | bool |  |
|Current Tick | long |  |
-----------------------------------------------------------------------
### Package: Add Player (0x0c)
Wiki: [Package-Add Player](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddPlayer)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



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
|Head Yaw | float |  |
|Yaw | float |  |
|Item | Item |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
### Package: Add Entity (0x0d)
Wiki: [Package-Add Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddEntity)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 


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
### Package: Remove Entity (0x0e)
Wiki: [Package-Remove Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RemoveEntity)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
-----------------------------------------------------------------------
### Package: Add Item Entity (0x0f)
Wiki: [Package-Add Item Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddItemEntity)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



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
### Package: Take Item Entity (0x11)
Wiki: [Package-Take Item Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TakeItemEntity)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Target | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Package: Move Entity (0x12)
Wiki: [Package-Move Entity](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MoveEntity)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Position | PlayerLocation |  |
|On Ground | bool |  |
|Teleport | bool |  |
-----------------------------------------------------------------------
### Package: Move Player (0x13)
Wiki: [Package-Move Player](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MovePlayer)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


MODE_NORMAL = 0;
MODE_RESET = 1;
MODE_ROTATION = 2;


#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Pitch | float |  |
|Head Yaw | float |  |
|Yaw | float |  |
|Mode | byte |  |
|On Ground | bool |  |
|Other Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Package: Rider Jump (0x14)
Wiki: [Package-Rider Jump](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RiderJump)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Update Block (0x15)
Wiki: [Package-Update Block](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateBlock)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | BlockUpdateRecords |  |
-----------------------------------------------------------------------
### Package: Add Painting (0x16)
Wiki: [Package-Add Painting](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddPainting)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Runtime Entity ID | UnsignedVarLong |  |
|Coordinates | BlockCoordinates |  |
|Direction | SignedVarInt |  |
|Title | string |  |
-----------------------------------------------------------------------
### Package: Explode (0x17)
Wiki: [Package-Explode](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Explode)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
|Radius | SignedVarInt |  |
|Records | Records |  |
-----------------------------------------------------------------------
### Package: Level Sound Event (0x18)
Wiki: [Package-Level Sound Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelSoundEvent)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



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
### Package: Level Event (0x19)
Wiki: [Package-Level Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-LevelEvent)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event ID | SignedVarInt |  |
|Position | Vector3 |  |
|Data | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Block Event (0x1a)
Wiki: [Package-Block Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockEvent)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Case 1 | SignedVarInt |  |
|Case 2 | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Entity Event (0x1b)
Wiki: [Package-Entity Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityEvent)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Event ID | byte |  |
|Unknown | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Mob Effect (0x1c)
Wiki: [Package-Mob Effect](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobEffect)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



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
### Package: Update Attributes (0x1d)
Wiki: [Package-Update Attributes](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateAttributes)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Attributes | PlayerAttributes |  |
-----------------------------------------------------------------------
### Package: Inventory Transaction Packet (0x1e)
Wiki: [Package-Inventory Transaction Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventoryTransactionPacket)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Transaction | Transaction |  |
-----------------------------------------------------------------------
### Package: Mob Equipment (0x1f)
Wiki: [Package-Mob Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobEquipment)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Item | Item |  |
|Slot | byte |  |
|Selected Slot | byte |  |
|Windows Id | byte |  |
-----------------------------------------------------------------------
### Package: Mob Armor Equipment (0x20)
Wiki: [Package-Mob Armor Equipment](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MobArmorEquipment)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Helmet | Item |  |
|Chestplate | Item |  |
|Leggings | Item |  |
|Boots | Item |  |
-----------------------------------------------------------------------
### Package: Interact (0x21)
Wiki: [Package-Interact](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Interact)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Package: Block Pick Request (0x22)
Wiki: [Package-Block Pick Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockPickRequest)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | SignedVarInt |  |
|Y | SignedVarInt |  |
|Z | SignedVarInt |  |
|Selected Slot | byte |  |
-----------------------------------------------------------------------
### Package: Entity Pick Request Packet (0x23)
Wiki: [Package-Entity Pick Request Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityPickRequestPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Player Action (0x24)
Wiki: [Package-Player Action](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerAction)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Action ID | SignedVarInt |  |
|Coordinates | BlockCoordinates |  |
|Face | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Entity Fall (0x25)
Wiki: [Package-Entity Fall](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-EntityFall)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Fall distance | float |  |
|Unknown | bool |  |
-----------------------------------------------------------------------
### Package: Hurt Armor (0x26)
Wiki: [Package-Hurt Armor](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-HurtArmor)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Set Entity Data (0x27)
Wiki: [Package-Set Entity Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
### Package: Set Entity Motion (0x28)
Wiki: [Package-Set Entity Motion](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityMotion)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Velocity | Vector3 |  |
-----------------------------------------------------------------------
### Package: Set Entity Link (0x29)
Wiki: [Package-Set Entity Link](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetEntityLink)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Rider ID | UnsignedVarLong |  |
|Ridden ID | UnsignedVarLong |  |
|Link Type | byte |  |
-----------------------------------------------------------------------
### Package: Set Health (0x2a)
Wiki: [Package-Set Health](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetHealth)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Set Spawn Position (0x2b)
Wiki: [Package-Set Spawn Position](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetSpawnPosition)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Spawn Type | SignedVarInt |  |
|Coordinates | BlockCoordinates |  |
|Forced | bool |  |
-----------------------------------------------------------------------
### Package: Animate (0x2c)
Wiki: [Package-Animate](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Animate)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | SignedVarInt |  |
|Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Package: Respawn (0x2d)
Wiki: [Package-Respawn](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Respawn)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
-----------------------------------------------------------------------
### Package: Container Open (0x2e)
Wiki: [Package-Container Open](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerOpen)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Type | byte |  |
|Coordinates | BlockCoordinates |  |
|Unknown Runtime Entity ID | UnsignedVarLong |  |
-----------------------------------------------------------------------
### Package: Container Close (0x2f)
Wiki: [Package-Container Close](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerClose)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
-----------------------------------------------------------------------
### Package: Player Hotbar Packet (0x30)
Wiki: [Package-Player Hotbar Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerHotbarPacket)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Selected Slot | byte |  |
|Hotbar Data | MetadataInts |  |
-----------------------------------------------------------------------
### Package: Inventory Content Packet (0x31)
Wiki: [Package-Inventory Content Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventoryContentPacket)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Inventory Id | VarInt |  |
|Input | ItemStacks |  |
-----------------------------------------------------------------------
### Package: Inventory Slot Packet (0x32)
Wiki: [Package-Inventory Slot Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InventorySlotPacket)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Inventory Id | SignedVarInt |  |
|Slot | SignedVarInt |  |
|Item | Item |  |
-----------------------------------------------------------------------
### Package: Container Set Data (0x33)
Wiki: [Package-Container Set Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ContainerSetData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Property | SignedVarInt |  |
|Value | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Crafting Data (0x34)
Wiki: [Package-Crafting Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CraftingData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Recipes | Recipes |  |
-----------------------------------------------------------------------
### Package: Crafting Event (0x35)
Wiki: [Package-Crafting Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CraftingEvent)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Recipe Type | SignedVarInt |  |
|Recipe ID | UUID |  |
|Input | ItemStacks |  |
|Result | ItemStacks |  |
-----------------------------------------------------------------------
### Package: Gui Data Pick Item Packet (0x36)
Wiki: [Package-Gui Data Pick Item Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GuiDataPickItemPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Adventure Settings (0x37)
Wiki: [Package-Adventure Settings](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AdventureSettings)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | UnsignedVarInt |  |
|User Permission | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Package: Block Entity Data (0x38)
Wiki: [Package-Block Entity Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BlockEntityData)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
### Package: Player Input (0x39)
Wiki: [Package-Player Input](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerInput)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Motion X | float |  |
|Motion Z | float |  |
|Flag1 | bool |  |
|Flag2 | bool |  |
-----------------------------------------------------------------------
### Package: Full Chunk Data (0x3a)
Wiki: [Package-Full Chunk Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-FullChunkData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk X | SignedVarInt |  |
|Chunk Z | SignedVarInt |  |
|Chunk Data | ByteArray |  |
-----------------------------------------------------------------------
### Package: Set Commands Enabled (0x3b)
Wiki: [Package-Set Commands Enabled](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetCommandsEnabled)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Enabled | bool |  |
-----------------------------------------------------------------------
### Package: Set Difficulty (0x3c)
Wiki: [Package-Set Difficulty](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetDifficulty)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Package: Change Dimension (0x3d)
Wiki: [Package-Change Dimension](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ChangeDimension)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Dimension | SignedVarInt |  |
|Position | Vector3 |  |
|Unknown | bool |  |
-----------------------------------------------------------------------
### Package: Set Player Game Type (0x3e)
Wiki: [Package-Set Player Game Type](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetPlayerGameType)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Gamemode | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Player List (0x3f)
Wiki: [Package-Player List](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerList)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | PlayerRecords |  |
-----------------------------------------------------------------------
### Package: Simple Event (0x40)
Wiki: [Package-Simple Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SimpleEvent)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Telemetry Event Packet (0x41)
Wiki: [Package-Telemetry Event Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-TelemetryEventPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID Self | SignedVarLong |  |
|Unk1 | SignedVarInt |  |
|Unk2 | byte |  |
-----------------------------------------------------------------------
### Package: Spawn Experience Orb (0x42)
Wiki: [Package-Spawn Experience Orb](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SpawnExperienceOrb)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
|Count | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Clientbound Map Item Data  (0x43)
Wiki: [Package-Clientbound Map Item Data ](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ClientboundMapItemData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|MapInfo | MapInfo |  |
-----------------------------------------------------------------------
### Package: Map Info Request (0x44)
Wiki: [Package-Map Info Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-MapInfoRequest)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Map ID | SignedVarLong |  |
-----------------------------------------------------------------------
### Package: Request Chunk Radius (0x45)
Wiki: [Package-Request Chunk Radius](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-RequestChunkRadius)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Chunk Radius Update (0x46)
Wiki: [Package-Chunk Radius Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ChunkRadiusUpdate)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Item Frame Drop Item (0x47)
Wiki: [Package-Item Frame Drop Item](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ItemFrameDropItem)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
-----------------------------------------------------------------------
### Package: Game Rules Changed (0x48)
Wiki: [Package-Game Rules Changed](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-GameRulesChanged)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Rules | GameRules |  |
-----------------------------------------------------------------------
### Package: Camera (0x49)
Wiki: [Package-Camera](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Camera)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Boss Event (0x4a)
Wiki: [Package-Boss Event](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BossEvent)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Boss Entity ID | SignedVarLong |  |
|Event Type | UnsignedVarInt |  |
-----------------------------------------------------------------------
### Package: Show Credits (0x4b)
Wiki: [Package-Show Credits](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ShowCredits)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Runtime Entity ID | UnsignedVarLong |  |
|Status | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Available Commands (0x4c)
Wiki: [Package-Available Commands](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AvailableCommands)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Commands | string |  |
|Unknown | string |  |
-----------------------------------------------------------------------
### Package: Command Request Packet (0x4d)
Wiki: [Package-Command Request Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandRequestPacket)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Command name | string |  |
|Command overload | string |  |
|Unknown 1 | UnsignedVarInt |  |
|Current Step  | UnsignedVarInt |  |
|Is Output | bool |  |
|Client ID | UnsignedVarLong |  |
|Command Input Json | string |  |
|Command Output Json | string |  |
|Unknown 7 | byte |  |
|Unknown 8 | byte |  |
|Entity ID Self | SignedVarLong |  |
-----------------------------------------------------------------------
### Package: Command Block Update (0x4e)
Wiki: [Package-Command Block Update](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-CommandBlockUpdate)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Update Trade (0x50)
Wiki: [Package-Update Trade](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateTrade)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Update Equipment Packet (0x51)
Wiki: [Package-Update Equipment Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-UpdateEquipmentPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Resource Pack Data Info (0x52)
Wiki: [Package-Resource Pack Data Info](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackDataInfo)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Max Chunk Size  | uint |  |
|Chunk Count  | uint |  |
|Compressed Package Size  | ulong |  |
|Hash  | string |  |
-----------------------------------------------------------------------
### Package: Resource Pack Chunk Data (0x53)
Wiki: [Package-Resource Pack Chunk Data](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackChunkData)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Chunk Index | uint |  |
|Progress | ulong |  |
|Length | uint |  |
|Payload | byte[] | 0 |
-----------------------------------------------------------------------
### Package: Resource Pack Chunk Request (0x54)
Wiki: [Package-Resource Pack Chunk Request](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-ResourcePackChunkRequest)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Package ID | string |  |
|Chunk Index | uint |  |
-----------------------------------------------------------------------
### Package: Transfer (0x55)
Wiki: [Package-Transfer](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-Transfer)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server Address | string |  |
|Port | ushort |  |
-----------------------------------------------------------------------
### Package: Play Sound (0x56)
Wiki: [Package-Play Sound](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlaySound)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Coordinates | BlockCoordinates |  |
|Volume | float |  |
|Pitch | float |  |
-----------------------------------------------------------------------
### Package: Stop Sound (0x57)
Wiki: [Package-Stop Sound](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StopSound)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Name | string |  |
|Stop All | bool |  |
-----------------------------------------------------------------------
### Package: Set Title (0x58)
Wiki: [Package-Set Title](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetTitle)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | SignedVarInt |  |
|Text | string |  |
|Fade In Time | SignedVarInt |  |
|Stay Time | SignedVarInt |  |
|Fade Out Time | SignedVarInt |  |
-----------------------------------------------------------------------
### Package: Add Behavior Tree Packet (0x59)
Wiki: [Package-Add Behavior Tree Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-AddBehaviorTreePacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Structure Block Update Packet (0x5a)
Wiki: [Package-Structure Block Update Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-StructureBlockUpdatePacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Player Skin Packet (0x5d)
Wiki: [Package-Player Skin Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-PlayerSkinPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Sub Client Login Packet (0x5e)
Wiki: [Package-Sub Client Login Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SubClientLoginPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Initiate Web Socket Connection Packet (0x5f)
Wiki: [Package-Initiate Web Socket Connection Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-InitiateWebSocketConnectionPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Set Last Hurt By Packet (0x60)
Wiki: [Package-Set Last Hurt By Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-SetLastHurtByPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Book Edit Packet (0x61)
Wiki: [Package-Book Edit Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-BookEditPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
### Package: Npc Request Packet (0x62)
Wiki: [Package-Npc Request Packet](https://github.com/NiclasOlofsson/MiNET/wiki//Protocol-NpcRequestPacket)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



#### Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------


