using log4net;
using MiNET.Net;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MiNET.IO
{
    internal class DatagramProcessor
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));
        readonly IOClient _io;
        readonly ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions;
        readonly GreylistManager GreylistManager;

        public DatagramProcessor(IOClient io, ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> playerSessions, GreylistManager greylistManager)
        {
            this._io = io;
            this._playerSessions = playerSessions;
            this.GreylistManager = greylistManager;
        }

        public void Process(IPEndPoint sender, byte msgId, byte[] rawRequest)
        {
            var session = getPlayerSessionFor(sender, msgId);
            if (session == null)
                return;

            session.LastUpdatedTime = DateTime.UtcNow;

            processSessionPackage(sender, msgId, rawRequest, session);
        }

        private PlayerNetworkSession getPlayerSessionFor(IPEndPoint endpoint, byte msgId)
        {
            PlayerNetworkSession playerSession;
            if (!_playerSessions.TryGetValue(endpoint, out playerSession))
            {
                //Log.DebugFormat("Receive MCPE message 0x{1:x2} without session {0}", senderEndpoint.Address, msgId);
                //if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
                //{
                //	_badPacketBans.Add(senderEndpoint.Address, true);
                //}
                return null;
            }

            if (playerSession.MessageHandler == null)
            {
                Log.ErrorFormat("Receive MCPE message 0x{1:x2} without message handler {0}. Session removed.", endpoint.Address, msgId);
                _playerSessions.TryRemove(endpoint, out playerSession);
                //if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
                //{
                //	_badPacketBans.Add(senderEndpoint.Address, true);
                //}
                return null;
            }

            if (playerSession.Evicted)
                return null;

            return playerSession;
        }

        private void processSessionPackage(IPEndPoint sender, byte msgId, byte[] rawRequest, PlayerNetworkSession session)
        {
            var header = new DatagramHeader(msgId);
            if (!header.isValid)
            {
                Log.Warn("!!!! ERROR, Invalid header !!!!!");
            }
            else if (header.isACK)
            {
                handleAck(session, rawRequest);
            }
            else if (header.isNAK)
            {
                handleNak(session, rawRequest);
            }
            else
            {
                handleRawRequest(sender, session, msgId, rawRequest);
            }
        }

        private void handleAck(PlayerNetworkSession session, byte[] receiveBytes)
        {
            if (session == null) return;

            //Ack ack = Ack.CreateObject();
            Ack ack = new Ack();
            //ack.Reset();
            ack.Decode(receiveBytes);

            var queue = session.WaitingForAcksQueue;

            foreach (Tuple<int, int> range in ack.ranges)
            {
                Interlocked.Increment(ref this._io.ServerInfo.NumberOfAckReceive);

                int start = range.Item1;
                int end = range.Item2;

                for (int i = start; i <= end; i++)
                {
                    Datagram datagram;
                    if (queue.TryRemove(i, out datagram))
                    {
                        //if (Log.IsDebugEnabled)
                        //	Log.DebugFormat("ACK, on datagram #{0} for {2}. Queue size={1}", i, queue.Count, player.Username);

                        updateSessionTimers(session, datagram);

                        datagram.PutPool();
                    }
                    else
                    {
                        if (Log.IsDebugEnabled)
                            Log.WarnFormat("ACK, Failed to remove datagram #{0} for {2}. Queue size={1}", i, queue.Count, session.Username);
                    }
                }
            }

            //ack.PutPool();

            session.ResendCount = 0;
            session.WaitForAck = false;
        }

        private void handleNak(PlayerNetworkSession session, byte[] receiveBytes)
        {
            if (session == null) return;

            Nak nak = Nak.CreateObject();
            nak.Reset();
            nak.Decode(receiveBytes);

            var queue = session.WaitingForAcksQueue;

            foreach (Tuple<int, int> range in nak.ranges)
            {
                Interlocked.Increment(ref this._io.ServerInfo.NumberOfNakReceive);

                int start = range.Item1;
                int end = range.Item2;

                for (int i = start; i <= end; i++)
                {
                    session.ErrorCount++;

                    // HACK: Just to make sure we aren't getting unessecary load on the queue during heavy buffering.
                    //if (ServerInfo.AvailableBytes > 1000) continue;

                    Datagram datagram;
                    //if (queue.TryRemove(i, out datagram))
                    if (!session.Evicted && queue.TryRemove(i, out datagram))
                    {
                        updateSessionTimers(session, datagram);

                        MiNetServer.FastThreadPool.QueueUserWorkItem(delegate
                        {
                            var dgram = (Datagram)datagram;
                            if (Log.IsDebugEnabled)
                                Log.WarnFormat("NAK, resent datagram #{0} for {1}", dgram.Header.datagramSequenceNumber, session.Username);
                            Send(session, dgram);
                            Interlocked.Increment(ref this._io.ServerInfo.NumberOfResends);
                        });
                    }
                    else
                    {
                        if (Log.IsDebugEnabled)
                            Log.WarnFormat("NAK, no datagram #{0} for {1}", i, session.Username);
                    }
                }
            }

            nak.PutPool();
        }

        private void handleRawRequest(IPEndPoint sender, PlayerNetworkSession session, byte msgId, byte[] rawRequest)
        {
            if (msgId == 0xa0)
                throw new Exception("Receive ERROR, NAK in wrong place");

            var package = createPackageFrom(rawRequest);
            if (package == null)
            {
                session.Disconnect("Bad package received from client.");
                GreylistManager.Blacklist(sender.Address);
                return;
            }


            // IF reliable code below is enabled, useItem start sending doubles
            // for some unknown reason.

            //Reliability reliability = package._reliability;
            //if (reliability == Reliability.Reliable
            //	|| reliability == Reliability.ReliableSequenced
            //	|| reliability == Reliability.ReliableOrdered
            //	)
            {
                enqueueAck(session, package._datagramSequenceNumber);
                //if (Log.IsDebugEnabled) Log.Debug("ACK on #" + package._datagramSequenceNumber.IntValue());
            }

            OnConnectedPackageReceived(new ConnectedPackageReceivedEventArgs(session, package));

            package.PutPool();
        }

        private void enqueueAck(PlayerNetworkSession session, int sequenceNumber)
        {
            session.PlayerAckQueue.Enqueue(sequenceNumber);
        }

        internal void Send(PlayerNetworkSession session, IEnumerable<Datagram> datagrams)
        {
            foreach (var datagram in datagrams)
            {
                this.Send(session, datagram);
            }
        }

        internal void Send(PlayerNetworkSession session, Datagram datagram)
        {
            if (datagram.MessageParts.Count == 0)
            {
                datagram.PutPool();
                Log.WarnFormat("Failed to resend #{0}", datagram.Header.datagramSequenceNumber.IntValue());
                return;
            }

            if (datagram.TransmissionCount > 10)
            {
                if (Log.IsDebugEnabled)
                    Log.WarnFormat("TIMEOUT, Retransmission count remove from ACK queue #{0} Type: {2} (0x{2:x2}) for {1}",
                        datagram.Header.datagramSequenceNumber.IntValue(),
                        session.Username,
                        datagram.FirstMessageId);

                datagram.PutPool();

                Interlocked.Increment(ref this._io.ServerInfo.NumberOfFails);
                return;
            }

            datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);
            datagram.TransmissionCount++;

            byte[] data = datagram.Encode();

            datagram.Timer.Restart();

            if (!session.WaitingForAcksQueue.TryAdd(datagram.Header.datagramSequenceNumber.IntValue(), datagram))
            {
                Log.Warn(string.Format("Datagram sequence unexpectedly existed in the ACK/NAK queue already {0}", datagram.Header.datagramSequenceNumber.IntValue()));
            }

            lock (session.SyncRoot)
            {
                this._io.Send(data, session.EndPoint);
            }
        }

        private ConnectedPackage createPackageFrom(byte[] rawRequest)
        {
            var package = ConnectedPackage.CreateObject();
            try
            {
                package.Decode(rawRequest);
                return package;
            }
            catch (Exception e)
            {
                Log.Warn($"Bad packet {rawRequest[0]}\n{Package.HexDump(rawRequest)}", e);
                //TODO: Shouldn't we call package.PutPool() here?
            }
            return null;
        }

        private static void updateSessionTimers(PlayerNetworkSession session, Datagram datagram)
        {
            // RTT = RTT * 0.875 + rtt * 0.125
            // RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
            // RTO = RTT + 4 * RTTVar
            long rtt = datagram.Timer.ElapsedMilliseconds;
            long RTT = session.Rtt;
            long RTTVar = session.RttVar;

            session.Rtt = (long)(RTT * 0.875 + rtt * 0.125);
            session.RttVar = (long)(RTTVar * 0.875 + Math.Abs(RTT - rtt) * 0.125);
            session.Rto = session.Rtt + 4 * session.RttVar + 100; // SYNC time in the end
        }


        public event EventHandler<ConnectedPackageReceivedEventArgs> ConnectedPackageReceived;
        protected void OnConnectedPackageReceived(ConnectedPackageReceivedEventArgs e)
        {
            ConnectedPackageReceived(this, e);
        }
    }
}
