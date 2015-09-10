
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
| Mcpe Transfer | 0x1b | 27 |   
| Unconnected Pong | 0x1c | 28 |   
| Mcpe Login | 0x8f | 143 |   
| Mcpe Player Status | 0x90 | 144 |   
| Mcpe Disconnect | 0x91 | 145 |   
| Mcpe Batch | 0x92 | 146 |   
| Mcpe Text | 0x93 | 147 |   
| Mcpe Set Time | 0x94 | 148 |   
| Mcpe Start Game | 0x95 | 149 |   
| Mcpe Add Player | 0x96 | 150 |   
| Mcpe Remove Player | 0x97 | 151 |   
| Mcpe Add Entity | 0x98 | 152 |   
| Mcpe Remove Entity | 0x99 | 153 |   
| Mcpe Add Item Entity | 0x9a | 154 |   
| Mcpe Take Item Entity | 0x9b | 155 |   
| Mcpe Move Entity | 0x9c | 156 |   
| Mcpe Move Player | 0x9d | 157 |   
| Mcpe Remove Block | 0x9e | 158 |   
| Mcpe Update Block | 0x9f | 159 |   
| Mcpe Add Painting | 0xa0 | 160 |   
| Mcpe Explode | 0xa1 | 161 |   
| Mcpe Level Event | 0xa2 | 162 |   
| Mcpe Tile Event | 0xa3 | 163 |   
| Mcpe Entity Event | 0xa4 | 164 |   
| Mcpe Mob Effect | 0xa5 | 165 |   
| Mcpe Update Attributes | 0xa6 | 166 |   
| Mcpe Player Equipment | 0xa7 | 167 |   
| Mcpe Player Armor Equipment | 0xa8 | 168 |   
| Mcpe Interact | 0xa9 | 169 |   
| Mcpe Use Item | 0xaa | 170 |   
| Mcpe Player Action | 0xab | 171 |   
| Mcpe Hurt Armor | 0xac | 172 |   
| Mcpe Set Entity Data | 0xad | 173 |   
| Mcpe Set Entity Motion | 0xae | 174 |   
| Mcpe Set Entity Link | 0xaf | 175 |   
| Mcpe Set Health | 0xb0 | 176 |   
| Mcpe Set Spawn Position | 0xb1 | 177 |   
| Mcpe Animate | 0xb2 | 178 |   
| Mcpe Respawn | 0xb3 | 179 |   
| Mcpe Drop Item | 0xb4 | 180 |   
| Mcpe Container Open | 0xb5 | 181 |   
| Mcpe Container Close | 0xb6 | 182 |   
| Mcpe Container Set Slot | 0xb7 | 183 |   
| Mcpe Container Set Data | 0xb8 | 184 |   
| Mcpe Container Set Content | 0xb9 | 185 |   
| Mcpe Crafting Data | 0xba | 186 |   
| Mcpe Crafting Event | 0xbb | 187 |   
| Mcpe Adventure Settings | 0xbc | 188 |   
| Mcpe Tile Entity Data | 0xbd | 189 |   
| Mcpe Full Chunk Data | 0xbf | 191 |   
| Mcpe Set Difficulty | 0xc0 | 192 |   
| Mcpe Player List | 0xc3 | 195 |   


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
###Package: Mcpe Login (0x8f)

**Sent from server:** false
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|username | string |  |
|protocol | int |  |
|protocol2 | int |  |
|Client ID | long |  |
|Client UUID | UUID |  |
|Server Address | string |  |
|Client Secret | string |  |
|Skin | Skin |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Status (0x90)

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
###Package: Mcpe Disconnect (0x91)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Message | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Batch (0x92)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Payload size | int |  |
|Payload | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Text (0x93)

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
###Package: Mcpe Set Time (0x94)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | int |  |
|Started | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Start Game (0x95)

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
|unknown | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Player (0x96)

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
|Item Id | short |  |
|Item Meta | short |  |
|Metadata | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Remove Player (0x97)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Client UUID | UUID |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Entity (0x98)

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
###Package: Mcpe Remove Entity (0x99)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Item Entity (0x9a)

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
###Package: Mcpe Take Item Entity (0x9b)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Target | long |  |
|Entity Id | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Entity (0x9c)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityLocations |  |
-----------------------------------------------------------------------
###Package: Mcpe Move Player (0x9d)

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
###Package: Mcpe Remove Block (0x9e)

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
###Package: Mcpe Update Block (0x9f)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 


TODO: can have multiple blocks.


####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Blocks | BlockRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Add Painting (0xa0)

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
###Package: Mcpe Explode (0xa1)

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
###Package: Mcpe Level Event (0xa2)

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
###Package: Mcpe Tile Event (0xa3)

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
###Package: Mcpe Entity Event (0xa4)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Event ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Mob Effect (0xa5)

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
###Package: Mcpe Update Attributes (0xa6)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Player Equipment (0xa7)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Item | MetadataSlot |  |
|Slot | byte |  |
|Selected Slot | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Armor Equipment (0xa8)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Helmet | MetadataSlot |  |
|Chestplate | MetadataSlot |  |
|Leggings | MetadataSlot |  |
|Boots | MetadataSlot |  |
-----------------------------------------------------------------------
###Package: Mcpe Interact (0xa9)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Target Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Use Item (0xaa)

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
|Fx | float |  |
|Fy | float |  |
|Fz | float |  |
|Position X | float |  |
|Position Y | float |  |
|Position Z | float |  |
|Item | MetadataSlot |  |
-----------------------------------------------------------------------
###Package: Mcpe Player Action (0xab)

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
###Package: Mcpe Hurt Armor (0xac)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Data (0xad)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | long |  |
|Metadata | MetadataDictionary |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Motion (0xae)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entities | EntityMotions |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Entity Link (0xaf)

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
###Package: Mcpe Set Health (0xb0)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Spawn Position (0xb1)

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
###Package: Mcpe Animate (0xb2)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Entity ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Respawn (0xb3)

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
###Package: Mcpe Drop Item (0xb4)

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
###Package: Mcpe Container Open (0xb5)

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
###Package: Mcpe Container Close (0xb6)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Slot (0xb7)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot | short |  |
|Item | MetadataSlot |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Data (0xb8)

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
###Package: Mcpe Container Set Content (0xb9)

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
###Package: Mcpe Crafting Data (0xba)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Crafting Event (0xbb)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Adventure Settings (0xbc)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Tile Entity Data (0xbd)

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
###Package: Mcpe Full Chunk Data (0xbf)

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
###Package: Mcpe Set Difficulty (0xc0)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Difficulty | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Player List (0xc3)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Records | PlayerRecords |  |
-----------------------------------------------------------------------
###Package: Mcpe Transfer (0x1b)

**Sent from server:** true
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|EndPoint | IPEndPoint |  |
-----------------------------------------------------------------------


