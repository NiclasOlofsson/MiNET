#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

namespace MiNET.Net.RakNet
{
	///// \sa NetworkIDObject.h
	//typedef unsigned char UniqueIDType; => C# byte
	//typedef unsigned short SystemIndex; => C# ushort
	//typedef unsigned char RPCIndex; => byte
	//const int MAX_RPC_MAP_SIZE=((RPCIndex)-1)-1;
	//const int UNDEFINED_RPC_INDEX=((RPCIndex)-1);

	///// First byte of a network message
	//typedef unsigned char MessageID; => C# byte

	//typedef uint32_t BitSize_t;

	//// Define __GET_TIME_64BIT if you want to use large types for GetTime (takes more bandwidth when you transmit time though!)
	//// You would want to do this if your system is going to run long enough to overflow the millisecond counter (over a month)
	//#if __GET_TIME_64BIT==1
	//typedef uint64_t Time;
	//typedef uint32_t TimeMS;
	//typedef uint64_t TimeUS;
	//#else
	//typedef uint32_t Time;
	//typedef uint32_t TimeMS;
	//typedef uint64_t TimeUS;
	//#endif


	//#if CC_TIME_TYPE_BYTES==8
	//typedef uint64_t CCTimeType;
	//#else
	//typedef uint32_t CCTimeType;
	//#endif

	//typedef uint24_t DatagramSequenceNumberType;
	//typedef double BytesPerMicrosecond;
	//typedef double BytesPerSecond;
	//typedef double MicrosecondsPerByte;


	public enum DefaultMessageIdTypes : byte
	{
		//
		// RESERVED TYPES - DO NOT CHANGE THESE
		// All types from RakPeer
		//
		/// These types are never returned to the user.
		/// Ping from a connected system. Update timestamps (internal use only)
		ID_CONNECTED_PING = 0x00,

		/// Ping from an unconnected system. Reply but do not update timestamps. (internal use only)
		ID_UNCONNECTED_PING = 0x01,

		/// Ping from an unconnected system. Only reply if we have open connections. Do not update timestamps. (internal use only)
		ID_UNCONNECTED_PING_OPEN_CONNECTIONS = 0x02,

		/// Pong from a connected system. Update timestamps (internal use only)
		ID_CONNECTED_PONG = 0x03,

		/// A reliable packet to detect lost connections (internal use only)
		ID_DETECT_LOST_CONNECTIONS = 0x04,

		/// C2S: Initial query: Header(1), OfflineMesageID(16), Protocol number(1), Pad(toMTU), sent with no fragment set.
		/// If protocol fails on server, returns ID_INCOMPATIBLE_PROTOCOL_VERSION to client
		ID_OPEN_CONNECTION_REQUEST_1 = 0x05,

		/// S2C: Header(1), OfflineMesageID(16), server GUID(8), HasSecurity(1), Cookie(4, if HasSecurity)
		/// , public key (if do security is true), MTU(2). If public key fails on client, returns ID_PUBLIC_KEY_MISMATCH
		ID_OPEN_CONNECTION_REPLY_1 = 0x06,

		/// C2S: Header(1), OfflineMesageID(16), Cookie(4, if HasSecurity is true on the server), clientSupportsSecurity(1 bit),
		/// handshakeChallenge (if has security on both server and client), remoteBindingAddress(6), MTU(2), client GUID(8)
		/// Connection slot allocated if cookie is valid, server is not full, GUID and IP not already in use.
		ID_OPEN_CONNECTION_REQUEST_2 = 0x07,

		/// S2C: Header(1), OfflineMesageID(16), server GUID(8), mtu(2), doSecurity(1 bit), handshakeAnswer (if do security is true)
		ID_OPEN_CONNECTION_REPLY_2 = 0x08,

		/// C2S: Header(1), GUID(8), Timestamp, HasSecurity(1), Proof(32)
		ID_CONNECTION_REQUEST = 0x09,

		/// RakPeer - Remote system requires secure connections, pass a public key to RakPeerInterface::Connect()
		ID_REMOTE_SYSTEM_REQUIRES_PUBLIC_KEY = 0x0a,

		/// RakPeer - We passed a public key to RakPeerInterface::Connect(), but the other system did not have security turned on
		ID_OUR_SYSTEM_REQUIRES_SECURITY = 0x0b,

		/// RakPeer - Wrong public key passed to RakPeerInterface::Connect()
		ID_PUBLIC_KEY_MISMATCH = 0x0c,

		/// RakPeer - Same as ID_ADVERTISE_SYSTEM, but intended for internal use rather than being passed to the user.
		/// Second byte indicates type. Used currently for NAT punchthrough for receiver port advertisement. See ID_NAT_ADVERTISE_RECIPIENT_PORT
		ID_OUT_OF_BAND_INTERNAL = 0x0d,

		/// If RakPeerInterface::Send() is called where PacketReliability contains _WITH_ACK_RECEIPT, then on a later call to
		/// RakPeerInterface::Receive() you will get ID_SND_RECEIPT_ACKED or ID_SND_RECEIPT_LOSS. The message will be 5 bytes long,
		/// and bytes 1-4 inclusive will contain a number in native order containing a number that identifies this message.
		/// This number will be returned by RakPeerInterface::Send() or RakPeerInterface::SendList(). ID_SND_RECEIPT_ACKED means that
		/// the message arrived
		ID_SND_RECEIPT_ACKED = 0x0e,

		/// If RakPeerInterface::Send() is called where PacketReliability contains _WITH_ACK_RECEIPT, then on a later call to
		/// RakPeerInterface::Receive() you will get ID_SND_RECEIPT_ACKED or ID_SND_RECEIPT_LOSS. The message will be 5 bytes long,
		/// and bytes 1-4 inclusive will contain a number in native order containing a number that identifies this message. This number
		/// will be returned by RakPeerInterface::Send() or RakPeerInterface::SendList(). ID_SND_RECEIPT_LOSS means that an ack for the
		/// message did not arrive (it may or may not have been delivered, probably not). On disconnect or shutdown, you will not get
		/// ID_SND_RECEIPT_LOSS for unsent messages, you should consider those messages as all lost.
		ID_SND_RECEIPT_LOSS = 0x0f,
		//
		// USER TYPES - DO NOT CHANGE THESE
		//

		/// RakPeer - In a client/server environment, our connection request to the server has been accepted.
		ID_CONNECTION_REQUEST_ACCEPTED = 0x10,

		/// RakPeer - Sent to the player when a connection request cannot be completed due to inability to connect.
		ID_CONNECTION_ATTEMPT_FAILED = 0x11,

		/// RakPeer - Sent a connect request to a system we are currently connected to.
		ID_ALREADY_CONNECTED = 0x12,

		/// RakPeer - A remote system has successfully connected.
		ID_NEW_INCOMING_CONNECTION = 0x13,

		/// RakPeer - The system we attempted to connect to is not accepting new connections.
		ID_NO_FREE_INCOMING_CONNECTIONS = 0x14,

		/// RakPeer - The system specified in Packet::systemAddress has disconnected from us. For the client, this would mean the
		/// server has shutdown.
		ID_DISCONNECTION_NOTIFICATION = 0x15,

		/// RakPeer - Reliable packets cannot be delivered to the system specified in Packet::systemAddress. The connection to that
		/// system has been closed.
		ID_CONNECTION_LOST = 0x16,

		/// RakPeer - We are banned from the system we attempted to connect to.
		ID_CONNECTION_BANNED = 0x17,

		/// RakPeer - The remote system is using a password and has refused our connection because we did not set the correct password.
		ID_INVALID_PASSWORD = 0x18,

		// RAKNET_PROTOCOL_VERSION in RakNetVersion.h does not match on the remote system what we have on our system
		// This means the two systems cannot communicate.
		// The 2nd byte of the message contains the value of RAKNET_PROTOCOL_VERSION for the remote system
		ID_INCOMPATIBLE_PROTOCOL_VERSION = 0x19,

		// Means that this IP address connected recently, and can't connect again as a security measure. See
		/// RakPeer::SetLimitIPConnectionFrequency()
		ID_IP_RECENTLY_CONNECTED = 0x1A,

		/// RakPeer - The sizeof(RakNetTime) bytes following this byte represent a value which is automatically modified by the difference
		/// in system times between the sender and the recipient. Requires that you call SetOccasionalPing.
		ID_TIMESTAMP = 0x1B,

		/// RakPeer - Pong from an unconnected system. First byte is ID_UNCONNECTED_PONG, second sizeof(RakNet::TimeMS) bytes is the ping,
		/// following bytes is system specific enumeration data.
		/// Read using bitstreams
		ID_UNCONNECTED_PONG = 0x1C,

		/// RakPeer - Inform a remote system of our IP/Port. On the recipient, all data past ID_ADVERTISE_SYSTEM is whatever was passed to
		/// the data parameter
		ID_ADVERTISE_SYSTEM = 0x1D,

		// RakPeer - Downloading a large message. Format is ID_DOWNLOAD_PROGRESS (MessageID), partCount (unsigned int),
		/// partTotal (unsigned int),
		/// partLength (unsigned int), first part data (length
		/// <
		/// =
		/// MAX_MTU_SIZE
		/// )
		/// .
		/// See
		/// the
		/// three
		/// parameters
		/// partCount
		/// ,
		/// partTotal
		/// and
		/// partLength
		/// in
		/// OnFileProgress
		/// in
		/// FileListTransferCBInterface.h
		ID_DOWNLOAD_PROGRESS,

		/// ConnectionGraph2 plugin - In a client/server environment, a client other than ourselves has disconnected gracefully.
		/// Packet::systemAddress is modified to reflect the systemAddress of this client.
		ID_REMOTE_DISCONNECTION_NOTIFICATION,

		/// ConnectionGraph2 plugin - In a client/server environment, a client other than ourselves has been forcefully dropped.
		/// Packet::systemAddress is modified to reflect the systemAddress of this client.
		ID_REMOTE_CONNECTION_LOST,

		/// ConnectionGraph2 plugin: Bytes 1-4 = count. for (count items) contains {SystemAddress, RakNetGUID}
		ID_REMOTE_NEW_INCOMING_CONNECTION,

		/// FileListTransfer plugin - Setup data
		ID_FILE_LIST_TRANSFER_HEADER,

		/// FileListTransfer plugin - A file
		ID_FILE_LIST_TRANSFER_FILE,

		// Ack for reference push, to send more of the file
		ID_FILE_LIST_REFERENCE_PUSH_ACK,

		/// DirectoryDeltaTransfer plugin - Request from a remote system for a download of a directory
		ID_DDT_DOWNLOAD_REQUEST,

		/// RakNetTransport plugin - Transport provider message, used for remote console
		ID_TRANSPORT_STRING,

		/// ReplicaManager plugin - Create an object
		ID_REPLICA_MANAGER_CONSTRUCTION,

		/// ReplicaManager plugin - Changed scope of an object
		ID_REPLICA_MANAGER_SCOPE_CHANGE,

		/// ReplicaManager plugin - Serialized data of an object
		ID_REPLICA_MANAGER_SERIALIZE,

		/// ReplicaManager plugin - New connection, about to send all world objects
		ID_REPLICA_MANAGER_DOWNLOAD_STARTED,

		/// ReplicaManager plugin - Finished downloading all serialized objects
		ID_REPLICA_MANAGER_DOWNLOAD_COMPLETE,

		/// Serialize construction for an object that already exists on the remote system
		ID_REPLICA_MANAGER_3_SERIALIZE_CONSTRUCTION_EXISTING,
		ID_REPLICA_MANAGER_3_LOCAL_CONSTRUCTION_REJECTED,
		ID_REPLICA_MANAGER_3_LOCAL_CONSTRUCTION_ACCEPTED,

		/// RakVoice plugin - Open a communication channel
		ID_RAKVOICE_OPEN_CHANNEL_REQUEST,

		/// RakVoice plugin - Communication channel accepted
		ID_RAKVOICE_OPEN_CHANNEL_REPLY,

		/// RakVoice plugin - Close a communication channel
		ID_RAKVOICE_CLOSE_CHANNEL,

		/// RakVoice plugin - Voice data
		ID_RAKVOICE_DATA,

		/// Autopatcher plugin - Get a list of files that have changed since a certain date
		ID_AUTOPATCHER_GET_CHANGELIST_SINCE_DATE,

		/// Autopatcher plugin - A list of files to create
		ID_AUTOPATCHER_CREATION_LIST,

		/// Autopatcher plugin - A list of files to delete
		ID_AUTOPATCHER_DELETION_LIST,

		/// Autopatcher plugin - A list of files to get patches for
		ID_AUTOPATCHER_GET_PATCH,

		/// Autopatcher plugin - A list of patches for a list of files
		ID_AUTOPATCHER_PATCH_LIST,

		/// Autopatcher plugin - Returned to the user: An error from the database repository for the autopatcher.
		ID_AUTOPATCHER_REPOSITORY_FATAL_ERROR,

		/// Autopatcher plugin - Finished getting all files from the autopatcher
		ID_AUTOPATCHER_FINISHED_INTERNAL,
		ID_AUTOPATCHER_FINISHED,

		/// Autopatcher plugin - Returned to the user: You must restart the application to finish patching.
		ID_AUTOPATCHER_RESTART_APPLICATION,

		/// NATPunchthrough plugin: internal
		ID_NAT_PUNCHTHROUGH_REQUEST,

		/// NATPunchthrough plugin: internal
		ID_NAT_CONNECT_AT_TIME,

		/// NATPunchthrough plugin: internal
		ID_NAT_GET_MOST_RECENT_PORT,

		/// NATPunchthrough plugin: internal
		ID_NAT_CLIENT_READY,

		/// NATPunchthrough plugin: Destination system is not connected to the server. Bytes starting at offset 1 contains the
		/// RakNetGUID destination field of NatPunchthroughClient::OpenNAT().
		ID_NAT_TARGET_NOT_CONNECTED,

		/// NATPunchthrough plugin: Destination system is not responding to ID_NAT_GET_MOST_RECENT_PORT. Possibly the plugin is not installed.
		/// Bytes starting at offset 1 contains the RakNetGUID destination field of NatPunchthroughClient::OpenNAT().
		ID_NAT_TARGET_UNRESPONSIVE,

		/// NATPunchthrough plugin: The server lost the connection to the destination system while setting up punchthrough.
		/// Possibly the plugin is not installed. Bytes starting at offset 1 contains the RakNetGUID destination
		/// field of NatPunchthroughClient::OpenNAT().
		ID_NAT_CONNECTION_TO_TARGET_LOST,

		/// NATPunchthrough plugin: This punchthrough is already in progress. Possibly the plugin is not installed.
		/// Bytes starting at offset 1 contains the RakNetGUID destination field of NatPunchthroughClient::OpenNAT().
		ID_NAT_ALREADY_IN_PROGRESS,

		/// NATPunchthrough plugin: This message is generated on the local system, and does not come from the network.
		/// packet::guid contains the destination field of NatPunchthroughClient::OpenNAT(). Byte 1 contains 1 if you are the sender, 0 if not
		ID_NAT_PUNCHTHROUGH_FAILED,

		/// NATPunchthrough plugin: Punchthrough suceeded. See packet::systemAddress and packet::guid. Byte 1 contains 1 if you are the sender,
		/// 0 if not. You can now use RakPeer::Connect() or other calls to communicate with this system.
		ID_NAT_PUNCHTHROUGH_SUCCEEDED,

		/// ReadyEvent plugin - Set the ready state for a particular system
		/// First 4 bytes after the message contains the id
		ID_READY_EVENT_SET,

		/// ReadyEvent plugin - Unset the ready state for a particular system
		/// First 4 bytes after the message contains the id
		ID_READY_EVENT_UNSET,

		/// All systems are in state ID_READY_EVENT_SET
		/// First 4 bytes after the message contains the id
		ID_READY_EVENT_ALL_SET,

		/// \internal, do not process in your game
		/// ReadyEvent plugin - Request of ready event state - used for pulling data when newly connecting
		ID_READY_EVENT_QUERY,

		/// Lobby packets. Second byte indicates type.
		ID_LOBBY_GENERAL,

		// RPC3, RPC4Plugin error
		ID_RPC_REMOTE_ERROR,

		/// Plugin based replacement for RPC system
		ID_RPC_PLUGIN,

		/// FileListTransfer transferring large files in chunks that are read only when needed, to save memory
		ID_FILE_LIST_REFERENCE_PUSH,

		/// Force the ready event to all set
		ID_READY_EVENT_FORCE_ALL_SET,

		/// Rooms function
		ID_ROOMS_EXECUTE_FUNC,
		ID_ROOMS_LOGON_STATUS,
		ID_ROOMS_HANDLE_CHANGE,

		/// Lobby2 message
		ID_LOBBY2_SEND_MESSAGE,
		ID_LOBBY2_SERVER_ERROR,

		/// Informs user of a new host GUID. Packet::Guid contains this new host RakNetGuid. The old host can be read out using BitStream->Read(RakNetGuid) starting on byte 1
		ID_FCM2_NEW_HOST,

		/// \internal For FullyConnectedMesh2 plugin
		ID_FCM2_REQUEST_FCMGUID,

		/// \internal For FullyConnectedMesh2 plugin
		ID_FCM2_RESPOND_CONNECTION_COUNT,

		/// \internal For FullyConnectedMesh2 plugin
		ID_FCM2_INFORM_FCMGUID,

		/// UDP proxy messages. Second byte indicates type.
		ID_UDP_PROXY_GENERAL,

		/// SQLite3Plugin - execute
		ID_SQLite3_EXEC,

		/// SQLite3Plugin - Remote database is unknown
		ID_SQLite3_UNKNOWN_DB,

		/// Events happening with SQLiteClientLoggerPlugin
		ID_SQLLITE_LOGGER,

		/// Sent to NatTypeDetectionServer
		ID_NAT_TYPE_DETECTION_REQUEST,

		/// Sent to NatTypeDetectionClient. Byte 1 contains the type of NAT detected.
		ID_NAT_TYPE_DETECTION_RESULT,

		/// Used by the router2 plugin
		ID_ROUTER_2_INTERNAL,

		/// No path is available or can be established to the remote system
		/// Packet::guid contains the endpoint guid that we were trying to reach
		ID_ROUTER_2_FORWARDING_NO_PATH,

		/// \brief You can now call connect, ping, or other operations to the destination system.
		/// 
		/// Connect as follows:
		/// 
		/// RakNet::BitStream bs(packet->data, packet->length, false);
		/// bs.IgnoreBytes(sizeof(MessageID));
		/// RakNetGUID endpointGuid;
		/// bs.Read(endpointGuid);
		/// unsigned short sourceToDestPort;
		/// bs.Read(sourceToDestPort);
		/// char ipAddressString[32];
		/// packet->systemAddress.ToString(false, ipAddressString);
		/// rakPeerInterface->Connect(ipAddressString, sourceToDestPort, 0,0);
		ID_ROUTER_2_FORWARDING_ESTABLISHED,

		/// The IP address for a forwarded connection has changed
		/// Read endpointGuid and port as per ID_ROUTER_2_FORWARDING_ESTABLISHED
		ID_ROUTER_2_REROUTED,

		/// \internal Used by the team balancer plugin
		ID_TEAM_BALANCER_INTERNAL,

		/// Cannot switch to the desired team because it is full. However, if someone on that team leaves, you will
		/// get ID_TEAM_BALANCER_SET_TEAM later. Byte 1 contains the team you requested to join.
		ID_TEAM_BALANCER_REQUESTED_TEAM_CHANGE_PENDING,

		/// Cannot switch to the desired team because all teams are locked. However, if someone on that team leaves,
		/// you will get ID_TEAM_BALANCER_SET_TEAM later. Byte 1 contains the team you requested to join.
		ID_TEAM_BALANCER_TEAMS_LOCKED,

		/// Team balancer plugin informing you of your team. Byte 1 contains the team you requested to join.
		ID_TEAM_BALANCER_TEAM_ASSIGNED,

		/// Gamebryo Lightspeed integration
		ID_LIGHTSPEED_INTEGRATION,

		/// XBOX integration
		ID_XBOX_LOBBY,

		/// The password we used to challenge the other system passed, meaning the other system has called TwoWayAuthentication::AddPassword() with the same password we passed to TwoWayAuthentication::Challenge()
		/// You can read the identifier used to challenge as follows:
		/// RakNet::BitStream bs(packet->data, packet->length, false); bs.IgnoreBytes(sizeof(RakNet::MessageID)); RakNet::RakString password; bs.Read(password);
		ID_TWO_WAY_AUTHENTICATION_INCOMING_CHALLENGE_SUCCESS,
		ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_SUCCESS,

		/// A remote system sent us a challenge using TwoWayAuthentication::Challenge(), and the challenge failed.
		/// If the other system must pass the challenge to stay connected, you should call RakPeer::CloseConnection() to terminate the connection to the other system.
		ID_TWO_WAY_AUTHENTICATION_INCOMING_CHALLENGE_FAILURE,

		/// The other system did not add the password we used to TwoWayAuthentication::AddPassword()
		/// You can read the identifier used to challenge as follows:
		/// RakNet::BitStream bs(packet->data, packet->length, false); bs.IgnoreBytes(sizeof(MessageID)); RakNet::RakString password; bs.Read(password);
		ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_FAILURE,

		/// The other system did not respond within a timeout threshhold. Either the other system is not running the plugin or the other system was blocking on some operation for a long time.
		/// You can read the identifier used to challenge as follows:
		/// RakNet::BitStream bs(packet->data, packet->length, false); bs.IgnoreBytes(sizeof(MessageID)); RakNet::RakString password; bs.Read(password);
		ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_TIMEOUT,

		/// \internal
		ID_TWO_WAY_AUTHENTICATION_NEGOTIATION,

		/// CloudClient / CloudServer
		ID_CLOUD_POST_REQUEST,
		ID_CLOUD_RELEASE_REQUEST,
		ID_CLOUD_GET_REQUEST,
		ID_CLOUD_GET_RESPONSE,
		ID_CLOUD_UNSUBSCRIBE_REQUEST,
		ID_CLOUD_SERVER_TO_SERVER_COMMAND,
		ID_CLOUD_SUBSCRIPTION_NOTIFICATION,

		// So I can add more without changing user enumerations
		ID_RESERVED_1,
		ID_RESERVED_2,
		ID_RESERVED_3,
		ID_RESERVED_4,
		ID_RESERVED_5,
		ID_RESERVED_6,
		ID_RESERVED_7,
		ID_RESERVED_8,
		ID_RESERVED_9,

		// For the user to use. Start your first enumeration at this value.
		ID_USER_PACKET_ENUM,
	};
}