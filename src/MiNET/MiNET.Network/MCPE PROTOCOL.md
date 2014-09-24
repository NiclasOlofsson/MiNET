
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
| ID_MCPE_LOGIN | 0x82 | 130 |IdMcpeLogin |  
| ID_MCPE_LOGIN_STATUS | 0x83 | 131 |IdMcpeLoginStatus |  
| ID_MCPE_READY | 0x84 | 132 |IdMcpeReady |  
| ID_MCPE_START_GAME | 0x87 | 135 |IdMcpeStartGame |  


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
###Comments
<field name="Client UDP Port" type="short" />
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

##Package: ID_MCPE_LOGIN (0x82)



###Fields

| Name | Type |
|:-----|:-----|
|login | string |
|protocol | int |
|protocol2 | int |
|Client ID | int |
|logindata | string |

##Package: ID_MCPE_LOGIN_STATUS (0x83)



###Fields

| Name | Type |
|:-----|:-----|
|Status | int |

##Package: ID_MCPE_READY (0x84)



###Fields

| Name | Type |
|:-----|:-----|

##Package: ID_MCPE_START_GAME (0x87)



###Fields

| Name | Type |
|:-----|:-----|
|Seed | int |
|Generator | int |
|Gamemode | int |
|EID | int |
|Spawn X | float |
|Spawn Y | float |
|Spawn Z | float |
|X | float |
|Y | float |
|Z | float |



