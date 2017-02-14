using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Net;
using log4net;
using System.IO;
using MiNET.Utils;

namespace MiNET.IO
{
    internal class PackageAssembler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerNetworkSession));

        public void Process(PlayerNetworkSession session, ConnectedPackage package)
        {
            foreach (var message in package.Messages)
            {
                if (message == null)
                    continue;

                if (message is SplitPartPackage)
                {
                    handleSplitMessage(session, (SplitPartPackage)message);
                }
                else
                {
                    message.Timer.Restart();
                    OnPackageAssembled(new PackageAssembledEventArgs(session, message));
                }
            }
        }

        private void handleSplitMessage(PlayerNetworkSession session, SplitPartPackage messagePart)
        {
            var spPackets = tryExtractCompleteMessage(session, messagePart);
            if (spPackets == null)
                return;

            Log.DebugFormat("Got all {0} split packages for split ID: {1}", messagePart.SplitCount, messagePart.SplitId);

            Int24 sequenceNumber = messagePart.DatagramSequenceNumber;
            Reliability reliability = messagePart.Reliability;
            Int24 reliableMessageNumber = messagePart.ReliableMessageNumber;
            Int24 orderingIndex = messagePart.OrderingIndex;
            byte orderingChannel = messagePart.OrderingChannel;

            var buffer = assembleMessage(spPackets);

            byte msgId = buffer[0];
            try
            {
                var package = createFullPackage(
                    sequenceNumber, 
                    reliability, 
                    reliableMessageNumber, 
                    orderingIndex, 
                    orderingChannel, 
                    buffer, 
                    msgId);

                Log.Debug($"Assembled split package {package._reliability} message #{package._reliableMessageNumber}, Chan: #{package._orderingChannel}, OrdIdx: #{package._orderingIndex}");

                Process(session, package);

                package.PutPool();
            }
            catch (Exception e)
            {
                Log.Error("Error during split message parsing", e);

                if (Log.IsDebugEnabled)
                    Log.Debug($"0x{msgId:x2}\n{Package.HexDump(buffer)}");

                session.Disconnect("Bad package received from client.", false);
            }
        }

        private SplitPartPackage[] tryExtractCompleteMessage(PlayerNetworkSession session, SplitPartPackage messagePart)
        {
            SplitPartPackage[] spPackets;
            if (!assembleParts(session, messagePart, out spPackets))
                return null;

            SplitPartPackage[] waste;
            session.Splits.TryRemove(messagePart.SplitId, out waste);

            return spPackets;
        }

        private bool assembleParts(PlayerNetworkSession session, SplitPartPackage messagePart, out SplitPartPackage[] spPackets)
        {
            // Need sync for this part since they come very fast, and very close in time. 
            // If no synk, will often detect complete message two times (or more).
            lock (session.Splits)
            {
                if (!session.Splits.ContainsKey(messagePart.SplitId))
                {
                    session.Splits.TryAdd(messagePart.SplitId, new SplitPartPackage[messagePart.SplitCount]);
                }

                spPackets = session.Splits[messagePart.SplitId];

                if (spPackets[messagePart.SplitIdx] != null)
                {
                    Log.Debug("Already had splitpart (resent). Ignore this part.");
                    return false;
                }

                spPackets[messagePart.SplitIdx] = messagePart;


                var hasMissingPart = false;
                for (int i = 0; i < spPackets.Length; i++)
                {
                    hasMissingPart = hasMissingPart || spPackets[i] == null;
                }

                return !hasMissingPart;
            }
        }

        private static byte[] assembleMessage(SplitPartPackage[] messageParts)
        {
            using (var stream = MiNetServer.MemoryStreamManager.GetStream())
            {
                for (int i = 0; i < messageParts.Length; i++)
                {
                    var messagePart = messageParts[i];

                    if (messagePart.Message == null)
                    {
                        Log.Error("Expected bytes in splitpart, but got none");
                    }
                    else
                    {
                        stream.Write(messagePart.Message, 0, messagePart.Message.Length);
                        messagePart.PutPool(); //TODO: Shouldn't we do this even if .Message == null?
                    }
                }

                return stream.ToArray();
            }
        }

        private static ConnectedPackage createFullPackage(Int24 sequenceNumber, Reliability reliability, Int24 reliableMessageNumber, Int24 orderingIndex, byte orderingChannel, byte[] buffer, byte msgId)
        {
            Package message = PackageFactory.CreatePackage(msgId, buffer, "raknet") ?? new UnknownPackage(msgId, buffer);
            message.DatagramSequenceNumber = sequenceNumber;
            message.Reliability = reliability;
            message.ReliableMessageNumber = reliableMessageNumber;
            message.OrderingIndex = orderingIndex;
            message.OrderingChannel = orderingChannel;

            var package = ConnectedPackage.CreateObject();
            package._datagramSequenceNumber = sequenceNumber;
            package._reliability = reliability;
            package._reliableMessageNumber = reliableMessageNumber;
            package._orderingIndex = orderingIndex;
            package._orderingChannel = (byte)orderingChannel;
            package._hasSplit = false;
            package.Messages = new List<Package> { message };

            return package;
        }



        public event EventHandler<PackageAssembledEventArgs> PackageAssembled;
        protected void OnPackageAssembled(PackageAssembledEventArgs e)
        {
            PackageAssembled(this, e);
        }
    }
}
