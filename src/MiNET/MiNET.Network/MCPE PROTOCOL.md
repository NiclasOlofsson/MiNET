
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID (hex) | ID (dec) | Type |
|:--- |:---------|---------:|:-----|
| ID_CONNECTED_PING | 0x00 | 0 |IdConnectedPing |  
| ID_UNCONNECTED_PING | 0x01 | 1 |IdUnconnectedPing |  
| ID_CONNECTED_PONG | 0x03 | 3 |IdConnectedPong |  
| ACK | 0xc0 | 192 |Ack |  
| NAK | 0xa0 | 160 |Nak |  
| ID_UNCONNECTED_PONG | 0x1c | 28 |IdUnconnectedPong |  
| ID_OPEN_CONNECTION_REQUEST_1 | 0x05 | 5 |IdOpenConnectionRequest1 |  
| ID_OPEN_CONNECTION_REPLY_1 | 0x06 | 6 |IdOpenConnectionReply1 |  
| ID_OPEN_CONNECTION_REQUEST_2 | 0x07 | 7 |IdOpenConnectionRequest2 |  
| ID_OPEN_CONNECTION_REPLY_2 | 0x08 | 8 |IdOpenConnectionReply2 |  
| ID_CONNECTION_REQUEST | 0x09 | 9 |IdConnectionRequest |  
| ID_CONNECTION_REQUEST_ACCEPTED | 0x10 | 16 |IdConnectionRequestAccepted |  
| ID_NEW_INCOMING_CONNECTION | 0x13 | 19 |IdNewIncomingConnection |  
| ID_DISCONNECTION_NOTIFICATION | 0x15 | 21 |IdDisconnectionNotification |  
| ID_MCPE_LOGIN | 0x82 | 130 |IdMcpeLogin |  
| ID_MCPE_LOGIN_STATUS | 0x83 | 131 |IdMcpeLoginStatus |  
| ID_MCPE_READY | 0x84 | 132 |IdMcpeReady |  
| ID_MCPE_SET_TIME | 0x86 | 134 |IdMcpeSetTime |  
| ID_MCPE_SET_HEALTH | 0xaa | 170 |IdMcpeSetHealth |  
| ID_MCPE_SET_SPAWN_POSITION | 0xab | 171 |IdMcpeSetSpawnPosition |  
| ID_MCPE_START_GAME | 0x87 | 135 |IdMcpeStartGame |  
| ID_MCPE_FULL_CHUNK_DATA | 0xba | 186 |IdMcpeFullChunkData |  
| ID_MCPE_MOVE_PLAYER | 0x95 | 149 |IdMcpeMovePlayer |  
| ID_MCPE_ADVENTURE_SETTINGS | 0xb7 | 183 |IdMcpeAdventureSettings |  
| ID_MCPE_CONTAINER_SET_CONTENT | 0xb4 | 180 |IdMcpeContainerSetContent |  
| ID_MCPE_MESSAGE | 0x85 | 133 |IdMcpeMessage |  
| ID_MCPE_ENTITY_DATA | 0xa7 | 167 |IdMcpeEntityData |  
| ID_MCPE_ADD_PLAYER | 0x89 | 137 |IdMcpeAddPlayer |  
| ID_MCPE_REMOVE_PLAYER | 0x8a | 138 |IdMcpeRemovePlayer |  


##Constants
##Packages

##Package: ID_CONNECTED_PING (0x00)



###Fields

| Name | Type |
|:-----|:-----|
|SendPingTime | long |

##Package: ID_UNCONNECTED_PING (0x01)


Send a ping to the specified unconnected system.
The remote system, if it is Initialized, will respond with ID_UNCONNECTED_PONG.
The final ping time will be encoded in the following sizeof(RakNet::TimeMS) bytes.  (Default is 4 bytes - See __GET_TIME_64BIT in RakNetTypes.h


###Fields

| Name | Type |
|:-----|:-----|
|Ping Id | long |
|Offline Message Data ID | byte[] |

##Package: ID_CONNECTED_PONG (0x03)



###Fields

| Name | Type |
|:-----|:-----|
|SendPingTime | long |
|SendPongTime | long |

##Package: ACK (0xc0)



###Fields

| Name | Type |
|:-----|:-----|
|Count | short |
|Only One Sequence | byte |
|Sequence Number | little |

##Package: NAK (0xa0)



###Fields

| Name | Type |
|:-----|:-----|
|Count | short |
|Only One Sequence | byte |
|Sequence Number | little |

##Package: ID_UNCONNECTED_PONG (0x1c)



###Fields

| Name | Type |
|:-----|:-----|
|Ping Id | long |
|Server ID | long |
|Offline Message Data ID | byte[] |
|Server Name | string |

##Package: ID_OPEN_CONNECTION_REQUEST_1 (0x05)



###Fields

| Name | Type |
|:-----|:-----|
|Offline Message Data ID | byte[] |
|RakNet Protocol Version | byte |
###Comments
<field name="Pad to MTU size" type="byte[]" size="0" />

##Package: ID_OPEN_CONNECTION_REPLY_1 (0x06)



###Fields

| Name | Type |
|:-----|:-----|
|Offline Message Data ID | byte[] |
|Server GUID | long |
|Server Has Security | byte |
###Comments
<field name="Cookie" type="Int32" />
|MTU Size | short |

##Package: ID_OPEN_CONNECTION_REQUEST_2 (0x07)



###Fields

| Name | Type |
|:-----|:-----|
|Offline Message Data ID | byte[] |
###Comments
<field name="Server Security" type="byte" />
###Comments
<field name="Cookie" type="Int32" />
|Client UDP Port | byte[] |
|MTU Size | short |
|Client GUID | long |

##Package: ID_OPEN_CONNECTION_REPLY_2 (0x08)



###Fields

| Name | Type |
|:-----|:-----|
|Offline Message Data ID | byte[] |
|Server GUID | long |
|Client UDP Port | short |
|MTU Size | short |
|Do Security | byte |
###Comments
<field name="handshakeAnswer" type="byte" />

##Package: ID_CONNECTION_REQUEST (0x09)



###Fields

| Name | Type |
|:-----|:-----|
|Client GUID | long |
|Timestamp | long |
|Do Security | byte |
###Comments
<field name="Proof" type="byte[]" size="32" />

##Package: ID_CONNECTION_REQUEST_ACCEPTED (0x10)



###Fields

| Name | Type |
|:-----|:-----|
|Client System Address | long |
|System Index | long |
|Incoming Timestamp | long |
|Server Timestamp | long |

##Package: ID_NEW_INCOMING_CONNECTION (0x13)



###Fields

| Name | Type |
|:-----|:-----|
|cookie | int |
|Do Security | byte |
|Port | short |
|Session | long |
|Session2 | long |

##Package: ID_DISCONNECTION_NOTIFICATION (0x15)



###Fields

| Name | Type |
|:-----|:-----|

##Package: ID_MCPE_LOGIN (0x82)



###Fields

| Name | Type |
|:-----|:-----|
|username | string |
|protocol | int |
|protocol2 | int |
|Client ID | int |
|Logindata | string |

##Package: ID_MCPE_LOGIN_STATUS (0x83)



###Fields

| Name | Type |
|:-----|:-----|
###Comments

		The three type of status are:
		0: Everything is good.
		1: If the server is outdated.
		2. If the game is outdated.
		
|Status | int |

##Package: ID_MCPE_READY (0x84)



###Fields

| Name | Type |
|:-----|:-----|

##Package: ID_MCPE_SET_TIME (0x86)



###Fields

| Name | Type |
|:-----|:-----|
|Time | int |
|Started | byte |

##Package: ID_MCPE_SET_HEALTH (0xaa)



###Fields

| Name | Type |
|:-----|:-----|
|Health | byte |

##Package: ID_MCPE_SET_SPAWN_POSITION (0xab)

FULL_CHUNK_DATA_PACKET

###Fields

| Name | Type |
|:-----|:-----|
|X | int |
|Z | int |
|Y | byte |

##Package: ID_MCPE_START_GAME (0x87)



###Fields

| Name | Type |
|:-----|:-----|
|Seed | int |
|Generator | int |
|Gamemode | int |
|Entity ID | int |
|Spawn X | int |
|Spawn Z | int |
|Spawn Y | int |
|X | float |
|Y | float |
|Z | float |

##Package: ID_MCPE_FULL_CHUNK_DATA (0xba)



###Fields

| Name | Type |
|:-----|:-----|
###Comments
<field name="Chunk X" type="int" />
		<field name="Chunk Z" type="byte" />
|Chunk Data | byte[] |

##Package: ID_MCPE_MOVE_PLAYER (0x95)



###Fields

| Name | Type |
|:-----|:-----|
|Entity ID | int |
|X | float |
|Y | float |
|Z | float |
|Yaw | float |
|Pitch | float |
|Body Yaw | float |
|Teleport | byte |

##Package: ID_MCPE_ADVENTURE_SETTINGS (0xb7)



###Fields

| Name | Type |
|:-----|:-----|
|Flags | int |

##Package: ID_MCPE_CONTAINER_SET_CONTENT (0xb4)



###Fields

| Name | Type |
|:-----|:-----|
|Window ID | byte |
|Slot Count | short |
|Slot Data | byte[] |
|Hotbar Count | short |
|Hotbar Data | byte[] |

##Package: ID_MCPE_MESSAGE (0x85)



###Fields

| Name | Type |
|:-----|:-----|
|Source | string |
|Message | string |

##Package: ID_MCPE_ENTITY_DATA (0xa7)



###Fields

| Name | Type |
|:-----|:-----|
|X | int |
|Y | byte |
|Z | int |
|NamedTag | byte[] |

##Package: ID_MCPE_ADD_PLAYER (0x89)



###Fields

| Name | Type |
|:-----|:-----|
|Client ID | long |
|Username | string |
|Entity ID | int |
|X | float |
|Y | float |
|Z | float |
|Yaw | byte |
|Pitch | byte |
|Unknown1 | short |
|Unknown2 | short |
|Metadata | byte[] |

##Package: ID_MCPE_REMOVE_PLAYER (0x8a)



###Fields

| Name | Type |
|:-----|:-----|
|Entity ID | int |
|Client ID | long |



