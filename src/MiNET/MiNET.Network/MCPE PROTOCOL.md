
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID (hex) | ID (dec) | 
|:--- |:---------|---------:| 
| Connected Ping | 0x00 | 0 |   
| Unconnected Ping | 0x01 | 1 |   
| Connected Pong | 0x03 | 3 |   
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
| Mcpe Login Status | 0x83 | 131 |   
| Mcpe Ready | 0x84 | 132 |   
| Mcpe Message | 0x85 | 133 |   
| Mcpe Set Time | 0x86 | 134 |   
| Mcpe Start Game | 0x87 | 135 |   
| Mcpe Add Player | 0x89 | 137 |   
| Mcpe Remove Player | 0x8a | 138 |   
| Mcpe Move Player | 0x95 | 149 |   
| Mcpe Place Block | 0x96 | 150 |   
| Mcpe Remove Block | 0x97 | 151 |   
| Mcpe Update Block | 0x98 | 152 |   
| Nak | 0xa0 | 160 |   
| Mcpe Use Item | 0xa3 | 163 |   
| Mcpe Entity Data | 0xa7 | 167 |   
| Mcpe Set Health | 0xaa | 170 |   
| Mcpe Set Spawn Position | 0xab | 171 |   
| Mcpe Animate | 0xac | 172 |   
| Mcpe Container Set Content | 0xb4 | 180 |   
| Mcpe Adventure Settings | 0xb7 | 183 |   
| Mcpe Full Chunk Data | 0xba | 186 |   
| Ack | 0xc0 | 192 |   
| Connected Ping | 0x00 | 0 |   
| Unconnected Ping | 0x01 | 1 |   
| Connected Pong | 0x03 | 3 |   
| Ack | 0xc0 | 192 |   
| Nak | 0xa0 | 160 |   
| Unconnected Pong | 0x1c | 28 |   
| Open Connection Request 1 | 0x05 | 5 |   
| Open Connection Reply 1 | 0x06 | 6 |   
| Open Connection Request 2 | 0x07 | 7 |   
| Open Connection Reply 2 | 0x08 | 8 |   
| Connection Request | 0x09 | 9 |   
| Connection Request Accepted | 0x10 | 16 |   
| New Incoming Connection | 0x13 | 19 |   
| Disconnection Notification | 0x15 | 21 |   
| Mcpe Login | 0x82 | 130 |   
| Mcpe Login Status | 0x83 | 131 |   
| Mcpe Ready | 0x84 | 132 |   
| Mcpe Set Time | 0x86 | 134 |   
| Mcpe Set Health | 0xaa | 170 |   
| Mcpe Set Spawn Position | 0xab | 171 |   
| Mcpe Start Game | 0x87 | 135 |   
| Mcpe Full Chunk Data | 0xba | 186 |   
| Mcpe Move Player | 0x95 | 149 |   
| Mcpe Adventure Settings | 0xb7 | 183 |   
| Mcpe Container Set Content | 0xb4 | 180 |   
| Mcpe Message | 0x85 | 133 |   
| Mcpe Entity Data | 0xa7 | 167 |   
| Mcpe Add Player | 0x89 | 137 |   
| Mcpe Remove Player | 0x8a | 138 |   
| Mcpe Place Block | 0x96 | 150 |   
| Mcpe Remove Block | 0x97 | 151 |   
| Mcpe Update Block | 0x98 | 152 |   
| Mcpe Animate | 0xac | 172 |   
| Mcpe Use Item | 0xa3 | 163 |   


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

**Sent from server:** true
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
###Package: Ack (0xc0)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Count | short |  |
|Only One Sequence | byte |  |
|Sequence Number | little |  |
-----------------------------------------------------------------------
###Package: Nak (0xa0)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Count | short |  |
|Only One Sequence | byte |  |
|Sequence Number | little |  |
-----------------------------------------------------------------------
###Package: Unconnected Pong (0x1c)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Ping Id | long |  |
|Server ID | long |  |
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server Name | string |  |
-----------------------------------------------------------------------
###Package: Open Connection Request 1 (0x05)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|RakNet Protocol Version | byte |  |
-----------------------------------------------------------------------
###Package: Open Connection Reply 1 (0x06)

**Sent from server:** false
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

**Sent from server:** true
**Sent from client:** true
**Packet size:** 34



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Client UDP Port | byte[] | 6 |
|MTU Size | short |  |
|Client GUID | long |  |
-----------------------------------------------------------------------
###Package: Open Connection Reply 2 (0x08)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 30



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Offline Message Data ID | OFFLINE_MESSAGE_DATA_ID |  |
|Server GUID | long |  |
|Client UDP Port | short |  |
|MTU Size | short |  |
|Do Security | byte |  |
-----------------------------------------------------------------------
###Package: Connection Request (0x09)

**Sent from server:** false
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

**Sent from server:** false
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

**Sent from server:** true
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

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|username | string |  |
|protocol | int |  |
|protocol2 | int |  |
|Client ID | int |  |
|Logindata | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Login Status (0x83)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Status | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Ready (0x84)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
-----------------------------------------------------------------------
###Package: Mcpe Set Time (0x86)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Time | int |  |
|Started | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Health (0xaa)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Health | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Set Spawn Position (0xab)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 

FULL_CHUNK_DATA_PACKET

####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Z | int |  |
|Y | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Start Game (0x87)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Seed | int |  |
|Generator | int |  |
|Gamemode | int |  |
|Entity ID | int |  |
|Spawn X | int |  |
|Spawn Z | int |  |
|Spawn Y | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
-----------------------------------------------------------------------
###Package: Mcpe Full Chunk Data (0xba)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Chunk Data | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Move Player (0x95)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Yaw | float |  |
|Pitch | float |  |
|Body Yaw | float |  |
|Teleport | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Adventure Settings (0xb7)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Flags | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Container Set Content (0xb4)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Window ID | byte |  |
|Slot Count | short |  |
|Slot Data | byte[] | 0 |
|Hotbar Count | short |  |
|Hotbar Data | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Message (0x85)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Source | string |  |
|Message | string |  |
-----------------------------------------------------------------------
###Package: Mcpe Entity Data (0xa7)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | byte |  |
|Z | int |  |
|NamedTag | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Add Player (0x89)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Client ID | long |  |
|Username | string |  |
|Entity ID | int |  |
|X | float |  |
|Y | float |  |
|Z | float |  |
|Yaw | byte |  |
|Pitch | byte |  |
|Unknown1 | short |  |
|Unknown2 | short |  |
|Metadata | byte[] | 0 |
-----------------------------------------------------------------------
###Package: Mcpe Remove Player (0x8a)

**Sent from server:** false
**Sent from client:** false
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | int |  |
|Client ID | long |  |
-----------------------------------------------------------------------
###Package: Mcpe Place Block (0x96)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | int |  |
|X | int |  |
|Z | int |  |
|Y | byte |  |
|Block | byte |  |
|Meta | byte |  |
|Face | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Remove Block (0x97)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Entity ID | int |  |
|X | int |  |
|Z | int |  |
|Y | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Update Block (0x98)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Z | int |  |
|Y | byte |  |
|Block | byte |  |
|Meta | byte |  |
-----------------------------------------------------------------------
###Package: Mcpe Animate (0xac)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|Action ID | byte |  |
|Entity ID | int |  |
-----------------------------------------------------------------------
###Package: Mcpe Use Item (0xa3)

**Sent from server:** true
**Sent from client:** true
**Packet size:** 



####Fields

| Name | Type | Size |
|:-----|:-----|:-----|
|X | int |  |
|Y | int |  |
|Z | int |  |
|Face | int |  |
|Item | short |  |
|Meta | short |  |
|Entity ID | int |  |
|Fx | float |  |
|Fy | float |  |
|Fz | float |  |
|Position X | float |  |
|Position Y | float |  |
|Position Z | float |  |
-----------------------------------------------------------------------


