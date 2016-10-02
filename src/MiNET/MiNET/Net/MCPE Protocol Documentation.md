
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Connected Ping | 0x00 | 0 |   
| Unconnected Ping | 0x01 | 1 |   
| Mcpe Login | 0x01 | 1 |   
| Ftl Create Player | 0x01 | 1 |   
| Mcpe Player Status | 0x02 | 2 |   
| Connected Pong | 0x03 | 3 |   
| Mcpe Server Exchange | 0x03 | 3 |   
| Detect Lost Connections | 0x04 | 4 |   
| Mcpe Client Magic | 0x04 | 4 |   
| Open Connection Request 1 | 0x05 | 5 |   
| Mcpe Disconnect | 0x05 | 5 |   
| Open Connection Reply 1 | 0x06 | 6 |   
| Mcpe Batch | 0x06 | 6 |   
| Open Connection Request 2 | 0x07 | 7 |   
| Mcpe Text | 0x07 | 7 |   
| Open Connection Reply 2 | 0x08 | 8 |   
| Mcpe Set Time | 0x08 | 8 |   
| Connection Request | 0x09 | 9 |   
| Mcpe Start Game | 0x09 | 9 |   
| Mcpe Add Player | 0x0a | 10 |   
| Mcpe Add Entity | 0x0b | 11 |   
| Mcpe Remove Entity | 0x0c | 12 |   
| Mcpe Add Item Entity | 0x0d | 13 |   
| Mcpe Add Hanging Entity | 0x0e | 14 |   
| Mcpe Take Item Entity | 0x0f | 15 |   
| Connection Request Accepted | 0x10 | 16 |   
| Mcpe Move Entity | 0x10 | 16 |   
| Mcpe Move Player | 0x11 | 17 |   
| Mcpe Rider Jump | 0x12 | 18 |   
| New Incoming Connection | 0x13 | 19 |   
| Mcpe Remove Block | 0x13 | 19 |   
| No Free Incoming Connections | 0x14 | 20 |   
| Mcpe Update Block | 0x14 | 20 |   
| Disconnection Notification | 0x15 | 21 |   
| Mcpe Add Painting | 0x15 | 21 |   
| Mcpe Explode | 0x16 | 22 |   
| Connection Banned | 0x17 | 23 |   
| Mcpe Level Sound Event | 0x17 | 23 |   
| Mcpe Level Event | 0x18 | 24 |   
| Mcpe Block Event | 0x19 | 25 |   
| Mcpe Entity Event | 0x1a | 26 |   
| Ip Recently Connected | 0x1A | 26 |   
| Mcpe Mob Effect | 0x1b | 27 |   
| Unconnected Pong | 0x1c | 28 |   
| Mcpe Update Attributes | 0x1c | 28 |   
| Mcpe Mob Equipment | 0x1d | 29 |   
| Mcpe Mob Armor Equipment | 0x1e | 30 |   
| Mcpe Interact | 0x1f | 31 |   
| Mcpe Use Item | 0x20 | 32 |   
| Mcpe Player Action | 0x21 | 33 |   
| Mcpe Hurt Armor | 0x22 | 34 |   
| Mcpe Set Entity Data | 0x23 | 35 |   
| Mcpe Set Entity Motion | 0x24 | 36 |   
| Mcpe Set Entity Link | 0x25 | 37 |   
| Mcpe Set Health | 0x26 | 38 |   
| Mcpe Set Spawn Position | 0x27 | 39 |   
| Mcpe Animate | 0x28 | 40 |   
| Mcpe Respawn | 0x29 | 41 |   
| Mcpe Drop Item | 0x2a | 42 |   
| Mcpe Inventory Action | 0x2b | 43 |   
| Mcpe Container Open | 0x2c | 44 |   
| Mcpe Container Close | 0x2d | 45 |   
| Mcpe Container Set Slot | 0x2e | 46 |   
| Mcpe Container Set Data | 0x2f | 47 |   
| Mcpe Container Set Content | 0x30 | 48 |   
| Mcpe Crafting Data | 0x31 | 49 |   
| Mcpe Crafting Event | 0x32 | 50 |   
| Mcpe Adventure Settings | 0x33 | 51 |   
| Mcpe Block Entity Data | 0x34 | 52 |   
| Mcpe Player Input | 0x35 | 53 |   
| Mcpe Full Chunk Data | 0x36 | 54 |   
| Mcpe Set Commands Enabled | 0x37 | 55 |   
| Mcpe Set Difficulty | 0x38 | 56 |   
| Mcpe Change Dimension | 0x39 | 57 |   
| Mcpe Set Player Gane Type | 0x3a | 58 |   
| Mcpe Player List | 0x3b | 59 |   
| Mcpe Telemetry Event | 0x3c | 60 |   
| Mcpe Spawn Experience Orb | 0x3d | 61 |   
| Mcpe Clientbound Map Item Data  | 0x3e | 62 |   
| Mcpe Map Info Request | 0x3f | 63 |   
| Mcpe Request Chunk Radius | 0x40 | 64 |   
| Mcpe Chunk Radius Update | 0x41 | 65 |   
| Mcpe Item Fram Drop Item | 0x42 | 66 |   
| Mcpe Replace Selected Item | 0x43 | 67 |   
| Mcpe Game Rules Changed | 0x44 | 68 |   
| Mcpe Camera | 0x45 | 69 |   
| Mcpe Add Item | 0x46 | 70 |   
| Mcpe Boss Event | 0x47 | 71 |   
| Mcpe Available Commands | 0x48 | 72 |   
| Mcpe Command Step | 0x49 | 73 |   
| Mcpe Wrapper | 0xfe | 254 |   


##Constants
	OFFLINE_MESSAGE_DATA_ID
	byte[]
	{ 0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78 }

##Packages

###Package: Connected Ping (0x00)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|SendPingTime | long |  |
-----------------------------------------------------------------------
###Package: Unconnected Ping (0x01)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 


Send a ping to the specified unconnected system.
The remote system, if it is Initialized, will respond with ID_UNCONNECTED_PONG.
The final ping time will be encoded in the following sizeof(RakNet::TimeMS) bytes.  (Default is 4 bytes - See __GET_TIME_64BIT in RakNetTypes.h


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Ping Id | long |  |
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|GUID | long |  |
-----------------------------------------------------------------------
###Package: Connected Pong (0x03)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|SendPingTime | long |  |
|SendPongTime | long |  |
-----------------------------------------------------------------------
###Package: Detect Lost Connections (0x04)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Unconnected Pong (0x1c)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 

<pdu id="0xc0" online="false" client="true" server="true" name="ACK">
<field name="Count" type="short" />
<field name="Only One Sequence" type="byte" />
<field name="Sequence Number" type="little" />
</pdu>

<pdu id="0xa0" online="false" client="true" server="true" name="NAK">
<field name="Count" type="short" />
<field name="Only One Sequence" type="byte" />
<field name="Sequence Number" type="little" />
</pdu>

####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Ping Id | long |  |
|Server ID | long |  |
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server Name | FixedString |  |
-----------------------------------------------------------------------
###Package: Open Connection Request 1 (0x05)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|RakNet Protocol Version | byte |  |
-----------------------------------------------------------------------
###Package: Open Connection Reply 1 (0x06)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 28



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server GUID | long |  |
|Server Has Security | byte |  |
|MTU Size | short |  |
-----------------------------------------------------------------------
###Package: Open Connection Request 2 (0x07)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 34



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Remote Binding Address | IPEndPoint |  |
|MTU Size | short |  |
|Client GUID | long |  |
-----------------------------------------------------------------------
###Package: Open Connection Reply 2 (0x08)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 30



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server GUID | long |  |
|Client Endpoint | IPEndPoint |  |
|MTU Size | short |  |
|Do security and handshake | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Connection Request (0x09)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 33



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Client GUID | long |  |
|Timestamp | long |  |
|Do Security | byte |  |
-----------------------------------------------------------------------
###Package: Connection Request Accepted (0x10)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|System Address | IPEndPoint |  |
|System Index | short |  |
|System Addresses | IPEndPoint[] | 10 |
|Incoming Timestamp | long |  |
|Server Timestamp | long |  |
-----------------------------------------------------------------------
###Package: New Incoming Connection (0x13)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|ClientEndpoint | IPEndPoint |  |
|System Addresses | IPEndPoint[] | 10 |
|Incoming Timestamp | long |  |
|Server Timestamp | long |  |
-----------------------------------------------------------------------
###Package: No Free Incoming Connections (0x14)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server GUID | long |  |
-----------------------------------------------------------------------
###Package: Disconnection Notification (0x15)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Connection Banned (0x17)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server GUID | long |  |
-----------------------------------------------------------------------
###Package: Ip Recently Connected (0x1A)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
-----------------------------------------------------------------------
###Package: Mcpe Login (0x01)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Protocol Version | int |  |
|Edition | byte |  |
|Payload | ByteArray |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Status (0x02)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 


The three type of status are:
0: Everything is good.
1: If the server is outdated.
2: If the game is outdated.
3: If the player is sapwned.


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Status | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Server Exchange (0x03)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Server Public Key | string |  |
|Token Lenght | Lenght |  |
|Token | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Client Magic (0x04)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Disconnect (0x05)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Message | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Batch (0x06)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload | ByteArray |  |
-----------------------------------------------------------------------
###Package: Mcpe Text (0x07)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 

 
The chat types are:
0: Raw
1: Chat
2: Translation
3: Popup
4: Tip


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Type | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Time (0x08)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | VarInt |  |
|Started | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Start Game (0x09)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|Spawn | Vector3 |  |
|Unknown 1 | float |  |
|Unknown 2 | float |  |
|Seed | SignedVarInt |  |
|Dimension | SignedVarInt |  |
|Generator | SignedVarInt |  |
|Gamemode | SignedVarInt |  |
|Difficulty | SignedVarInt |  |
|X | SignedVarInt |  |
|Y | SignedVarInt |  |
|Z | SignedVarInt |  |
|Is loaded in creative | bool |  |
|Day cycle stop time | SignedVarInt |  |
|EDU mode | bool |  |
|Rain level | float |  |
|Lightnig level | float |  |
|Enable commands | bool |  |
|Secret | string |  |
|World name | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Player (0x0a)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|UUID | UUID |  |
|Username | string |  |
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Yaw | float |  |
|Head Yaw | float |  |
|Pitch | float |  |
|Item | Item |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Entity (0x0b)

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


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|Entity Type | UnsignedVarInt |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Yaw | float |  |
|Pitch | float |  |
|Attributes | EntityAttributes |  |
|Metadata | MetadataDictionary |  |
|Links | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Remove Entity (0x0c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Item Entity (0x0d)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|Item | Item |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Hanging Entity (0x0e)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|Coordinates | BlockCoordinates |  |
|Unknown | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Take Item Entity (0x0f)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Target | VarLong |  |
|Entity Id | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Entity (0x10)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Id | VarLong |  |
|Position | PlayerLocation |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Player (0x11)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


MODE_NORMAL = 0;
MODE_RESET = 1;
MODE_ROTATION = 2;


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Yaw | float |  |
|Head Yaw | float |  |
|Pitch | float |  |
|Mode | byte |  |
|On Ground | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Rider Jump (0x12)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Remove Block (0x13)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
-----------------------------------------------------------------------
###Package: Mcpe Update Block (0x14)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 


0x00: None
0x01: Neighbours
0x02: Network
0x04: No Graphic
0x08: Priority


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Block ID | UnsignedVarInt |  |
|Block Meta And Priority | UnsignedVarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Painting (0x15)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Runtime Entity ID | VarLong |  |
|Coordinates | BlockCoordinates |  |
|Direction | VarInt |  |
|Title | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Explode (0x16)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Position | Vector3 |  |
|Radius | float |  |
|Records | Records |  |
-----------------------------------------------------------------------
###Package: Mcpe Level Sound Event (0x17)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Level Event (0x18)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event ID | VarInt |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Data | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Block Event (0x19)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Case 1 | VarInt |  |
|Case 2 | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Entity Event (0x1a)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Event ID | byte |  |
|Unknown | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Effect (0x1b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Event ID | byte |  |
|Effect ID | VarInt |  |
|Amplifier | VarInt |  |
|Particles | byte |  |
|Duration | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Update Attributes (0x1c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Attributes | PlayerAttributes |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Equipment (0x1d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Item | Item |  |
|Slot | byte |  |
|Selected Slot | byte |  |
|Unknown | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Armor Equipment (0x1e)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Helmet | Item |  |
|Chestplate | Item |  |
|Leggings | Item |  |
|Boots | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Interact (0x1f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Entity ID | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Use Item (0x20)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|BlockCoordinates | BlockCoordinates |  |
|Face | SignedVarInt |  |
|FaceCoordinates | Vector3 |  |
|PlayerPosition | Vector3 |  |
|Unknown | byte |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Action (0x21)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Action ID | SignedVarInt |  |
|Coordinates | BlockCoordinates |  |
|Face | SignedVarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Hurt Armor (0x22)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Data (0x23)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Motion (0x24)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | VarLong |  |
|Velocity | Vector3 |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Link (0x25)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Rider ID | VarLong |  |
|Ridden ID | VarLong |  |
|Link Type | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Health (0x26)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Spawn Position (0x27)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown 1 | VarInt |  |
|Coordinates | BlockCoordinates |  |
|Unknown 2 | bool |  |
-----------------------------------------------------------------------
###Package: Mcpe Animate (0x28)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | VarInt |  |
|Entity ID | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Respawn (0x29)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Drop Item (0x2a)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|ItemType | byte |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Inventory Action (0x2b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Unknown | VarInt |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Open (0x2c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Type | byte |  |
|Slot Count | VarInt |  |
|Coordinates | BlockCoordinates |  |
|Unown Entity ID | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Close (0x2d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Slot (0x2e)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot | VarInt |  |
|Unknown | VarInt |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Data (0x2f)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Property | VarInt |  |
|Value | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Content (0x30)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot Data | ItemStacks |  |
|Hotbar Data | MetadataInts |  |
-----------------------------------------------------------------------
###Package: Mcpe Crafting Data (0x31)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Recipes | Recipes |  |
-----------------------------------------------------------------------
###Package: Mcpe Crafting Event (0x32)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Recipe Type | VarInt |  |
|Recipe ID | UUID |  |
|Input | ItemStacks |  |
|Result | ItemStacks |  |
-----------------------------------------------------------------------
###Package: Mcpe Adventure Settings (0x33)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | UnsignedVarInt |  |
|User Permission | UnsignedVarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Block Entity Data (0x34)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Input (0x35)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Motion X | float |  |
|Motion Z | float |  |
|Flags | short |  |
-----------------------------------------------------------------------
###Package: Mcpe Full Chunk Data (0x36)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 

 
ORDER_COLUMNS = 0;
ORDER_LAYERED = 1;


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk X | SignedVarInt |  |
|Chunk Z | SignedVarInt |  |
|Order | byte |  |
|Chunk Data | ByteArray |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Commands Enabled (0x37)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Enabled | bool |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Difficulty (0x38)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | UnsignedVarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Change Dimension (0x39)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Dimension | byte |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Unknown | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Player Gane Type (0x3a)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Player List (0x3b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | PlayerRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Telemetry Event (0x3c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Spawn Experience Orb (0x3d)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
|Count | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Clientbound Map Item Data  (0x3e)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|MapInfo | MapInfo |  |
-----------------------------------------------------------------------
###Package: Mcpe Map Info Request (0x3f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Map ID | VarLong |  |
-----------------------------------------------------------------------
###Package: Mcpe Request Chunk Radius (0x40)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Chunk Radius Update (0x41)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | VarInt |  |
-----------------------------------------------------------------------
###Package: Mcpe Item Fram Drop Item (0x42)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Coordinates | BlockCoordinates |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Replace Selected Item (0x43)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Game Rules Changed (0x44)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Camera (0x45)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Add Item (0x46)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Boss Event (0x47)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Available Commands (0x48)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Commands | string |  |
|Unknown | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Command Step (0x49)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Wrapper (0xfe)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Ftl Create Player (0x01)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Username | string |  |
|ClientUuid | UUID |  |
|Server Address | string |  |
|Client Id | long |  |
|Skin | Skin |  |
-----------------------------------------------------------------------


