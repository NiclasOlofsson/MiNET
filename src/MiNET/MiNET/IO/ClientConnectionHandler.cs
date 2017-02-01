using log4net;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace MiNET.IO
{
    internal class ClientConnectionHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));
        readonly IOClient _io;
        readonly MotdProvider MotdProvider;
        readonly GreylistManager GreylistManager;
        readonly ConcurrentDictionary<IPEndPoint, DateTime> _connectionAttemps = new ConcurrentDictionary<IPEndPoint, DateTime>();

        public ClientConnectionHandler(IOClient io, MotdProvider motdProvider, GreylistManager greylistManager)
        {
            this._io = io;
            this.MotdProvider = motdProvider;
            this.GreylistManager = greylistManager;
        }

        public void Process(IPEndPoint sender, byte msgId, byte[] rawRequest)
        {
            var msgIdType = (DefaultMessageIdTypes)msgId;

            // Increase fast, decrease slow on 1s ticks.
            if (_io.ServerInfo.NumberOfPlayers < _io.ServerInfo.PlayerSessions.Count)
                _io.ServerInfo.NumberOfPlayers = _io.ServerInfo.PlayerSessions.Count;

            // Shortcut to reply fast, and no parsing
            if (msgIdType == DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1)
            {
                if (!shortcutOpenConnectionRequest1(sender))
                    return;
            }

            Package request;
            if (!tryCreateRequest(rawRequest, msgId, out request))
            {
                GreylistManager.Blacklist(sender.Address);
                Log.ErrorFormat("Receive bad packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, sender.Address, (DefaultMessageIdTypes)msgId);
                return;
            }

            try
            {
                handleRequest(sender, msgId, msgIdType, request);
            }
            finally
            {
                if (request != null)
                    request.PutPool();
            }
        }

        private void handleRequest(IPEndPoint sender, byte msgId, DefaultMessageIdTypes msgIdType, Package request)
        {
            TraceReceive(request);

            switch (msgIdType)
            {
                case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
                case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
                    handleUnconnectedPing(sender, (UnconnectedPing)request);
                    break;

                case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
                    handleOpenConnectionRequest1(sender, (OpenConnectionRequest1)request);
                    break;

                case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
                    handleOpenConnectionRequest2(sender, (OpenConnectionRequest2)request);
                    break;

                default:
                    GreylistManager.Blacklist(sender.Address);
                    Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, sender.Address, (DefaultMessageIdTypes)msgId);
                    break;
            }
        }

        private bool tryCreateRequest(byte[] rawRequest, byte msgId, out Package request)
        {
            try
            {
                request = PackageFactory.CreatePackage(msgId, rawRequest, "raknet");
            }
            catch (Exception)
            {
                request = null;
            }
            return request != null;
        }

        private bool shortcutOpenConnectionRequest1(IPEndPoint sender)
        {
            if (GreylistManager.AcceptConnection(sender.Address))
                return true;

            var noFree = NoFreeIncomingConnections.CreateObject();
            this._io.Send(noFree, sender);

            Interlocked.Increment(ref this._io.ServerInfo.NumberOfDeniedConnectionRequestsPerSecond);
            return false;
        }

        private void handleUnconnectedPing(IPEndPoint sender, UnconnectedPing request)
        {
            //TODO: This needs to be verified with RakNet first
            //response.sendpingtime = msg.sendpingtime;
            //response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

            var reply = UnconnectedPong.CreateObject();
            reply.serverId = sender.Address.Address + sender.Port;
            reply.pingId = request.pingId;
            reply.serverName = MotdProvider.GetMotd(_io.ServerInfo, sender);

            _io.Send(reply, sender);
        }

        private void handleOpenConnectionRequest1(IPEndPoint sender, OpenConnectionRequest1 request)
        {
            if (!trackConnectionAttempt(sender))
                return;

            if (Log.IsDebugEnabled)
                Log.WarnFormat("New connection from: {0} {1}, MTU: {2}, Ver: {3}", sender.Address, sender.Port, request.mtuSize, request.raknetProtocolVersion);

            var packet = OpenConnectionReply1.CreateObject();
            packet.serverGuid = 12345;
            packet.mtuSize = request.mtuSize;
            packet.serverHasSecurity = 0;

            this._io.Send(packet, sender);
        }

        private void handleOpenConnectionRequest2(IPEndPoint sender, OpenConnectionRequest2 request)
        {
            lock (_connectionAttemps)
            {
                DateTime trash;
                if (!_connectionAttemps.TryRemove(sender, out trash))
                {
                    Log.WarnFormat("Unexpected connection request packet from {0}. Probably a resend.", sender.Address);
                    return;
                }

                if (!OnClientConnecting(new ClientConnectingEventArgs(request, sender)))
                    return;
            }

            var reply = OpenConnectionReply2.CreateObject();
            reply.serverGuid = 12345;
            reply.clientEndpoint = sender;
            reply.mtuSize = request.mtuSize;
            reply.doSecurityAndHandshake = new byte[1];

            _io.Send(reply, sender);
        }

        private bool trackConnectionAttempt(IPEndPoint senderEndpoint)
        {
            lock (_connectionAttemps)
            {
                // Already connecting, then this is just a duplicate
                if (_connectionAttemps.ContainsKey(senderEndpoint))
                {
                    DateTime created;
                    _connectionAttemps.TryGetValue(senderEndpoint, out created);

                    if (DateTime.UtcNow < created + TimeSpan.FromSeconds(3))
                        return false;

                    _connectionAttemps.TryRemove(senderEndpoint, out created);
                }

                if (!_connectionAttemps.TryAdd(senderEndpoint, DateTime.UtcNow))
                    return false;

                return true;
            }
        }


        [Obsolete("Move this somewhere where it makes sense")]
        internal static void TraceReceive(Package message)
        {
            if (!Log.IsDebugEnabled) return;

            string typeName = message.GetType().Name;

            string includePattern = Config.GetProperty("TracePackets.Include", ".*");
            string excludePattern = Config.GetProperty("TracePackets.Exclude", null);
            int verbosity = Config.GetProperty("TracePackets.Verbosity", 0);
            verbosity = Config.GetProperty($"TracePackets.Verbosity.{typeName}", verbosity);

            if (!Regex.IsMatch(typeName, includePattern))
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(excludePattern) && Regex.IsMatch(typeName, excludePattern))
            {
                return;
            }

            if (verbosity == 0)
            {
                Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
            }
            else if (verbosity == 1)
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    Formatting = Formatting.Indented,
                };
                string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
                Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
            }
            else if (verbosity == 2)
            {
                Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Package.HexDump(message.Bytes)}");
            }
        }


        public event EventHandler<ClientConnectingEventArgs> ClientConnecting;
        protected bool OnClientConnecting(ClientConnectingEventArgs e)
        {
            ClientConnecting(this, e);
            return e.Cancel;
        }
    }
}
