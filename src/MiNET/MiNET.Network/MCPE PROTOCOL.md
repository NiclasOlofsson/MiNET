
**WARNING: T4 GENERATED MARKUP - DO NOT EDIT**

##ALL PACKAGES

| ID  | ID(hex) | ID(dec) | Type |
|:--- |:--------|--------:|:-----|
| ID_UNCONNECTED_PING | 0x01 | 1 |IdUnconnectedPing |  
| ID_UNCONNECTED_PONG | 0x1c | 28 |IdUnconnectedPong |  
| ID_OPEN_CONNECTION_REQUEST_1 | 0x05 | 5 |IdOpenConnectionRequest1 |  
| ID_OPEN_CONNECTION_REPLY_1 | 0x06 | 6 |IdOpenConnectionReply1 |  
| ID_OPEN_CONNECTION_REQUEST_2 | 0x07 | 7 |IdOpenConnectionRequest2 |  
| ID_OPEN_CONNECTION_REPLY_2 | 0x08 | 8 |IdOpenConnectionReply2 |  
| ID_CONNECTION_REQUEST | 0x09 | 9 |IdConnectionRequest |  
| ID_CONNECTION_REQUEST_ACCEPTED | 0x10 | 16 |IdConnectionRequestAccepted |  
| ACK | 0xc0 | 192 |Ack |  
| NAK | 0xa0 | 160 |Nak |  


##Constants
##Packages
##Package: IdUnconnectedPing (0x01)
###Fields
	pingId	long
	offlineMessageDataId	byte[]

##Package: IdUnconnectedPong (0x1c)
###Fields
	pingId	long
	serverId	long
	offlineMessageDataId	byte[]

##Package: IdOpenConnectionRequest1 (0x05)
###Fields
	offlineMessageDataId	byte[]
	raknetProtocolVersion	byte
	padToMtuSize	byte[]

##Package: IdOpenConnectionReply1 (0x06)
###Fields
	offlineMessageDataId	byte[]
	serverGuid	long
	serverHasSecurity	byte
###Comments
<field name="Cookie" type="Int32" />
	mtuSize	short

##Package: IdOpenConnectionRequest2 (0x07)
###Fields
	offlineMessageDataId	byte[]
###Comments
<field name="Server Security" type="byte" />
###Comments
<field name="Cookie" type="Int32" />
	clientUdpPort	byte[]
	mtuSize	short
	clientGuid	long

##Package: IdOpenConnectionReply2 (0x08)
###Fields
	offlineMessageDataId	byte[]
	serverGuid	long
###Comments
<field name="Client UDP Port" type="short" />
	mtuSize	short
	doSecurity	byte
###Comments
<field name="handshakeAnswer" type="byte" />

##Package: IdConnectionRequest (0x09)
###Fields
	clientGuid	long
	timestamp	long
	doSecurity	byte
###Comments
<field name="Proof" type="byte[]" size="32" />

##Package: IdConnectionRequestAccepted (0x10)
###Fields
	clientSystemAddress	long
	systemIndex	long
	incomingTimestamp	long
	serverTimestamp	long

##Package: Ack (0xc0)
###Fields
	count	short
	onlyOneSequence	byte
	sequenceNumber	little

##Package: Nak (0xa0)
###Fields
	count	short
	onlyOneSequence	byte
	sequenceNumber	little



