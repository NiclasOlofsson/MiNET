
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Connected Ping | 0x00 | 0 |   
| Unconnected Ping | 0x01 | 1 |   
| Connected Pong | 0x03 | 3 |   
| Detect Lost Connections | 0x04 | 4 |   
| Open Connection Request 1 | 0x05 | 5 |   
| Open Connection Reply 1 | 0x06 | 6 |   
| Open Connection Request 2 | 0x07 | 7 |   
| Open Connection Reply 2 | 0x08 | 8 |   
| Connection Request | 0x09 | 9 |   
| Connection Request Accepted | 0x10 | 16 |   
| New Incoming Connection | 0x13 | 19 |   
| Disconnection Notification | 0x15 | 21 |   
| Unconnected Pong | 0x1c | 28 |   
| Mcpe Login | 0x82 | 130 |   
| Mcpe Player Status | 0x83 | 131 |   
| Mcpe Disconnect | 0x84 | 132 |   
| Mcpe Text | 0x85 | 133 |   
| Mcpe Set Time | 0x86 | 134 |   
| Mcpe Start Game | 0x87 | 135 |   
| Mcpe Add Player | 0x88 | 136 |   
| Mcpe Remove Player | 0x89 | 137 |   
| Mcpe Add Entity | 0x8a | 138 |   
| Mcpe Remove Entity | 0x8b | 139 |   
| Mcpe Add Item Entity | 0x8c | 140 |   
| Mcpe Take Item Entity | 0x8d | 141 |   
| Mcpe Move Entity | 0x8e | 142 |   
| Mcpe Move Player | 0x8f | 143 |   
| Mcpe Remove Block | 0x90 | 144 |   
| Mcpe Update Block | 0x91 | 145 |   
| Mcpe Add Painting | 0x92 | 146 |   
| Mcpe Explode | 0x93 | 147 |   
| Mcpe Level Event | 0x94 | 148 |   
| Mcpe Tile Event | 0x95 | 149 |   
| Mcpe Entity Event | 0x96 | 150 |   
| Mcpe Mob Effect | 0x97 | 151 |   
| Mcpe Player Equipment | 0x98 | 152 |   
| Mcpe Player Armor Equipment | 0x99 | 153 |   
| Mcpe Interact | 0x9a | 154 |   
| Mcpe Use Item | 0x9b | 155 |   
| Mcpe Player Action | 0x9c | 156 |   
| Mcpe Hurt Armor | 0x9d | 157 |   
| Mcpe Set Entity Data | 0x9e | 158 |   
| Mcpe Set Entity Motion | 0x9f | 159 |   
| Mcpe Set Entity Link | 0xa0 | 160 |   
| Mcpe Set Health | 0xa1 | 161 |   
| Mcpe Set Spawn Position | 0xa2 | 162 |   
| Mcpe Animate | 0xa3 | 163 |   
| Mcpe Respawn | 0xa4 | 164 |   
| Mcpe Drop Item | 0xa5 | 165 |   
| Mcpe Container Open | 0xa6 | 166 |   
| Mcpe Container Close | 0xa7 | 167 |   
| Mcpe Container Set Slot | 0xa8 | 168 |   
| Mcpe Container Set Data | 0xa9 | 169 |   
| Mcpe Container Set Content | 0xaa | 170 |   
| Mcpe Adventure Settings | 0xac | 172 |   
| Mcpe Tile Entity Data | 0xad | 173 |   
| Mcpe Full Chunk Data | 0xaf | 175 |   
| Mcpe Set Difficulty | 0xb0 | 176 |   
| Mcpe Batch | 0xb1 | 177 |   


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
|Server Security | byte |  |
|SystemAdress | byte[] | 4 |
|Client UDP Port | short |  |
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
|Client System Address | long |  |
|System Index | long |  |
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
|cookie | int |  |
|Do Security | byte |  |
|Port | short |  |
|Session | long |  |
|Session2 | long |  |
-----------------------------------------------------------------------
###Package: Disconnection Notification (0x15)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Login (0x82)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|username | string |  |
|protocol | int |  |
|protocol2 | int |  |
|Client ID | int |  |
|Slim | byte |  |
|Skin | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Status (0x83)

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
###Package: Mcpe Disconnect (0x84)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Message | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Text (0x85)

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
###Package: Mcpe Set Time (0x86)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | int |  |
|Started | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Start Game (0x87)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Seed | int |  |
|Generator | int |  |
|Gamemode | int |  |
|Entity ID | long |  |
|Spawn X | int |  |
|Spawn Y | int |  |
|Spawn Z | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Player (0x88)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Client ID | long |  |
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
|Item | short |  |
|Meta | short |  |
|Slim | byte |  |
|Skin | string |  |
|Metadata | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Remove Player (0x89)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Client ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Entity (0x8a)

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
###Package: Mcpe Remove Entity (0x8b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Item Entity (0x8c)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Id | long |  |
|Item | MetadataSlot |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Speed X | float |  |
|Speed Y | float |  |
|Speed Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Take Item Entity (0x8d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Target | long |  |
|Entity Id | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Entity (0x8e)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityLocations |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Player (0x8f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



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
|Teleport | byte |  |
|On Ground | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Remove Block (0x90)

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
###Package: Mcpe Update Block (0x91)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


TODO: can have multiple blocks.


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Blocks | BlockRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Painting (0x92)

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
###Package: Mcpe Explode (0x93)

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
###Package: Mcpe Level Event (0x94)

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
###Package: Mcpe Tile Event (0x95)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Z | int |  |
|Y | int |  |
|Case 1 | int |  |
|Case 2 | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Entity Event (0x96)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Event ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Effect (0x97)

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
###Package: Mcpe Player Equipment (0x98)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Item | short |  |
|Meta | short |  |
|Slot | byte |  |
|Selected Slot | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Armor Equipment (0x99)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Helmet | byte |  |
|Chestplate | byte |  |
|Leggings | byte |  |
|Boots | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Interact (0x9a)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Use Item (0x9b)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | int |  |
|Z | int |  |
|Face | byte |  |
|Item | short |  |
|Meta | short |  |
|Entity ID | long |  |
|Fx | float |  |
|Fy | float |  |
|Fz | float |  |
|Position X | float |  |
|Position Y | float |  |
|Position Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Action (0x9c)

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
###Package: Mcpe Hurt Armor (0x9d)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Data (0x9e)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Motion (0x9f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityMotions |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Link (0xa0)

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
###Package: Mcpe Set Health (0xa1)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Spawn Position (0xa2)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Z | int |  |
|Y | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Animate (0xa3)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Respawn (0xa4)

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
###Package: Mcpe Drop Item (0xa5)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity Id | long |  |
|Unknown | byte |  |
|Item | MetadataSlot |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Open (0xa6)

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
###Package: Mcpe Container Close (0xa7)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Slot (0xa8)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot | short |  |
|Item ID | short |  |
|Item Count | byte |  |
|Item Damage | short |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Data (0xa9)

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
###Package: Mcpe Container Set Content (0xaa)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot Data | MetadataSlots |  |
|Hotbar Data | MetadataInts |  |
-----------------------------------------------------------------------
###Package: Mcpe Adventure Settings (0xac)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Tile Entity Data (0xad)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | byte |  |
|Z | int |  |
|NamedTag | Nbt |  |
-----------------------------------------------------------------------
###Package: Mcpe Full Chunk Data (0xaf)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk X | int |  |
|Chunk Z | int |  |
|Chunk Data Length | int |  |
|Chunk Data | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Set Difficulty (0xb0)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Batch (0xb1)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload size | int |  |
|Payload | byte[] | 0 |
-----------------------------------------------------------------------


