
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Connected Ping | 0x00 | 0 |   
| Unconnected Ping | 0x01 | 1 |   
| Mcpe Login | 0x01 | 1 |   
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
| Mcpe Take Item Entity | 0x0e | 14 |   
| Mcpe Move Entity | 0x0f | 15 |   
| Connection Request Accepted | 0x10 | 16 |   
| Mcpe Move Player | 0x10 | 16 |   
| Mcpe Rider Jump | 0x11 | 17 |   
| Mcpe Remove Block | 0x12 | 18 |   
| New Incoming Connection | 0x13 | 19 |   
| Mcpe Update Block | 0x13 | 19 |   
| No Free Incoming Connections | 0x14 | 20 |   
| Mcpe Add Painting | 0x14 | 20 |   
| Disconnection Notification | 0x15 | 21 |   
| Mcpe Explode | 0x15 | 21 |   
| Mcpe Level Event | 0x16 | 22 |   
| Connection Banned | 0x17 | 23 |   
| Mcpe Block Event | 0x17 | 23 |   
| Mcpe Entity Event | 0x18 | 24 |   
| Mcpe Mob Effect | 0x19 | 25 |   
| Mcpe Update Attributes | 0x1a | 26 |   
| Ip Recently Connected | 0x1A | 26 |   
| Mcpe Mob Equipment | 0x1b | 27 |   
| Unconnected Pong | 0x1c | 28 |   
| Mcpe Mob Armor Equipment | 0x1c | 28 |   
| Mcpe Interact | 0x1e | 30 |   
| Mcpe Use Item | 0x1f | 31 |   
| Mcpe Player Action | 0x20 | 32 |   
| Mcpe Hurt Armor | 0x21 | 33 |   
| Mcpe Set Entity Data | 0x22 | 34 |   
| Mcpe Set Entity Motion | 0x23 | 35 |   
| Mcpe Set Entity Link | 0x24 | 36 |   
| Mcpe Set Health | 0x25 | 37 |   
| Mcpe Set Spawn Position | 0x26 | 38 |   
| Mcpe Animate | 0x27 | 39 |   
| Mcpe Respawn | 0x28 | 40 |   
| Mcpe Drop Item | 0x29 | 41 |   
| Mcpe Container Open | 0x2a | 42 |   
| Mcpe Container Close | 0x2b | 43 |   
| Mcpe Container Set Slot | 0x2c | 44 |   
| Mcpe Container Set Data | 0x2d | 45 |   
| Mcpe Container Set Content | 0x2e | 46 |   
| Mcpe Crafting Data | 0x2f | 47 |   
| Mcpe Crafting Event | 0x30 | 48 |   
| Mcpe Adventure Settings | 0x31 | 49 |   
| Mcpe Block Entity Data | 0x32 | 50 |   
| Mcpe Player Input | 0x33 | 51 |   
| Mcpe Full Chunk Data | 0x34 | 52 |   
| Mcpe Set Difficulty | 0x35 | 53 |   
| Mcpe Change Dimension | 0x36 | 54 |   
| Mcpe Set Player Gane Type | 0x37 | 55 |   
| Mcpe Player List | 0x38 | 56 |   
| Mcpe Telemetry Event | 0x39 | 57 |   
| Mcpe Spawn Experience Orb | 0x3a | 58 |   
| Mcpe Clientbound Map Item Data  | 0x3b | 59 |   
| Mcpe Map Info Request | 0x3c | 60 |   
| Mcpe Request Chunk Radius | 0x3d | 61 |   
| Mcpe Chunk Radius Update | 0x3e | 62 |   
| Mcpe Item Fram Drop Item | 0x3f | 63 |   
| Mcpe Replace Selected Item | 0x40 | 64 |   
| Mcpe Add Item | 0x41 | 65 |   
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
|Server Name | string |  |
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
|ClientEndpoint | IPEndPoint |  |
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
|ClientEndpoint | IPEndPoint |  |
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
|System Addresses | IPEndPoint[] |  |
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
-----------------------------------------------------------------------
###Package: Ip Recently Connected (0x1A)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Login (0x01)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Protocol Version | int |  |
|Payload Lenght | int |  |
|Payload | byte[] | 0 |
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
|Random Key Token | string |  |
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
|Payload size | int |  |
|Payload | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Text (0x07)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 

 
The three types are:
0: Raw
1: Chat
2: Translation
3: Popup

TODO: Parameters


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
|Time | int |  |
|Started | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Start Game (0x09)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Seed | int |  |
|Dimension | byte |  |
|Generator | int |  |
|Gamemode | int |  |
|Entity ID | long |  |
|Spawn X | int |  |
|Spawn Y | int |  |
|Spawn Z | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|b1 | bool |  |
|b2 | bool |  |
|b3 | bool |  |
|unknownstr | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Player (0x0a)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|UUID | UUID |  |
|Username | string |  |
|Entity ID | long |  |
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


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Entity Type | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
|Yaw | float |  |
|Pitch | float |  |
|Metadata | MetadataDictionary |  |
|Links | short |  |
-----------------------------------------------------------------------
###Package: Mcpe Remove Entity (0x0c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Item Entity (0x0d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Id | long |  |
|Item | Item |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Take Item Entity (0x0e)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Target | long |  |
|Entity Id | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Entity (0x0f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityLocations |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Player (0x10)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


MODE_NORMAL = 0;
MODE_RESET = 1;
MODE_ROTATION = 2;


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Yaw | float |  |
|Head Yaw | float |  |
|Pitch | float |  |
|Mode | byte |  |
|On Ground | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Rider Jump (0x11)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Remove Block (0x12)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|X | int |  |
|Z | int |  |
|Y | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Update Block (0x13)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


TODO: can have multiple blocks.


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Blocks | BlockRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Painting (0x14)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|X | int |  |
|Y | int |  |
|Z | int |  |
|Direction | int |  |
|Title | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Explode (0x15)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | float |  |
|Y | float |  |
|Z | float |  |
|Radius | float |  |
|Records | Records |  |
-----------------------------------------------------------------------
###Package: Mcpe Level Event (0x16)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Event ID | short |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Data | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Block Event (0x17)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | int |  |
|Z | int |  |
|Case 1 | int |  |
|Case 2 | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Entity Event (0x18)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Event ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Effect (0x19)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Event ID | byte |  |
|Effect ID | byte |  |
|Amplifier | byte |  |
|Particles | byte |  |
|Duration | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Update Attributes (0x1a)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Attributes | PlayerAttributes |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Equipment (0x1b)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Item | Item |  |
|Slot | byte |  |
|Selected Slot | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Armor Equipment (0x1c)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Helmet | Item |  |
|Chestplate | Item |  |
|Leggings | Item |  |
|Boots | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Interact (0x1e)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Use Item (0x1f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|BlockCoordinates | BlockCoordinates |  |
|Face | byte |  |
|FaceCoordinates | Vector3 |  |
|PlayerPosition | Vector3 |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Action (0x20)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Action ID | int |  |
|X | int |  |
|Y | int |  |
|Z | int |  |
|Face | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Hurt Armor (0x21)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Data (0x22)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Motion (0x23)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityMotions |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Link (0x24)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Rider ID | long |  |
|Ridden ID | long |  |
|Link Type | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Health (0x25)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Spawn Position (0x26)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Z | int |  |
|Y | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Animate (0x27)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Respawn (0x28)

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
###Package: Mcpe Drop Item (0x29)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|ItemType | byte |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Open (0x2a)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Type | byte |  |
|Slot Count | short |  |
|X | int |  |
|Y | int |  |
|Z | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Close (0x2b)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Slot (0x2c)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot | short |  |
|Unknown | short |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Data (0x2d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Property | short |  |
|Value | short |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Content (0x2e)

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
###Package: Mcpe Crafting Data (0x2f)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Recipes | Recipes |  |
-----------------------------------------------------------------------
###Package: Mcpe Crafting Event (0x30)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Recipe Type | int |  |
|Recipe ID | UUID |  |
|Input | ItemStacks |  |
|Result | ItemStacks |  |
-----------------------------------------------------------------------
###Package: Mcpe Adventure Settings (0x31)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
|User Permission | int |  |
|Global Permission | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Block Entity Data (0x32)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | int |  |
|Z | int |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Input (0x33)

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
###Package: Mcpe Full Chunk Data (0x34)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 

 
ORDER_COLUMNS = 0;
ORDER_LAYERED = 1;


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk X | int |  |
|Chunk Z | int |  |
|Order | byte |  |
|Chunk Data Length | int |  |
|Chunk Data | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Set Difficulty (0x35)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Change Dimension (0x36)

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
###Package: Mcpe Set Player Gane Type (0x37)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Player List (0x38)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | PlayerRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Telemetry Event (0x39)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Spawn Experience Orb (0x3a)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|X | int |  |
|Y | int |  |
|Z | int |  |
|Count | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Clientbound Map Item Data  (0x3b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|MapInfo | MapInfo |  |
-----------------------------------------------------------------------
###Package: Mcpe Map Info Request (0x3c)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Map ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Request Chunk Radius (0x3d)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Chunk Radius Update (0x3e)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Radius | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Item Fram Drop Item (0x3f)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | int |  |
|Z | int |  |
|Item | Item |  |
-----------------------------------------------------------------------
###Package: Mcpe Replace Selected Item (0x40)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Add Item (0x41)

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


