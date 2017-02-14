using log4net;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.IO
{
    internal class QueryHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

        readonly IOClient _io;
        readonly MotdProvider MotdProvider;


        public QueryHandler(IOClient io, MotdProvider motdProvider)
        {
            this._io = io;
            this.MotdProvider = motdProvider;
        }

        public virtual void Process(IPEndPoint sender, byte msgId, byte[] rawRequest)
        {
            if (rawRequest[0] != 0xFE || rawRequest[1] != 0xFD)
                return;

            byte packetId = rawRequest[2];
            switch (packetId)
            {
                case 0x09:
                    processPackage0x09(rawRequest, sender);
                    break;

                case 0x00:
                    processPackage0x00(rawRequest, sender);
                    break;

                default:
                    return;
            }
        }

        private void processPackage0x09(byte[] receiveBytes, IPEndPoint senderEndpoint)
        {
            const byte ID = 0x09;

            var buffer = new byte[17];
            buffer[0] = ID;
            copySequenceNumber(receiveBytes, buffer, 1);
            fillRandomNullTerminatedToken(buffer, 5);

            _io.SendRaw(buffer, senderEndpoint);
        }

        private static void copySequenceNumber(byte[] source, byte[] destination, int destinationOffset)
        {
            Buffer.BlockCopy(source, 3, destination, destinationOffset, 4);
            //target[1] = source[3];
            //target[2] = source[4];
            //target[3] = source[5];
            //target[4] = source[6];
        }

        private static void fillRandomNullTerminatedToken(byte[] destination, int destinationOffset)
        {
            var token = createRandomNullTerminatedToken();
            Buffer.BlockCopy(token, 0, destination, destinationOffset, 11);
        }

        private static char[] createRandomNullTerminatedToken()
        {
            // Textual representation of int32 (token) with null terminator
            var str = new Random().Next().ToString(CultureInfo.InvariantCulture) + "\x00";
            var token = str.ToCharArray();
            return token;
        }

        private void processPackage0x00(byte[] receiveBytes, IPEndPoint senderEndpoint)
        {
            using (var stream = MiNetServer.MemoryStreamManager.GetStream())
            {
                bool isFullStatRequest = receiveBytes.Length == 15;
                if (Log.IsInfoEnabled) Log.InfoFormat("Full request: {0}", isFullStatRequest);

                // ID
                stream.WriteByte(0x00);

                // Sequence number
                stream.WriteByte(receiveBytes[3]);
                stream.WriteByte(receiveBytes[4]);
                stream.WriteByte(receiveBytes[5]);
                stream.WriteByte(receiveBytes[6]);

                //{
                //	string str = "splitnum\0";
                //	byte[] bytes = Encoding.ASCII.GetBytes(str.ToCharArray());
                //	stream.Write(bytes, 0, bytes.Length);
                //}

                MotdProvider.GetMotd(_io.ServerInfo, senderEndpoint); // Force update the player counts :-)

                var data = new Dictionary<string, string>
                        {
                            {"splitnum", "" + (char) 128},
                            {"hostname", "Minecraft PE Server"},
                            {"gametype", "SMP"},
                            {"game_id", "MINECRAFTPE"},
                            {"version", "0.15.0"},
                            {"server_engine", "MiNET v1.0.0"},
                            {"plugins", "MiNET v1.0.0"},
                            {"map", "world"},
                            {"numplayers", MotdProvider.NumberOfPlayers.ToString()},
                            {"maxplayers", MotdProvider.MaxNumberOfPlayers.ToString()},
                            {"whitelist", "off"},
							//{"hostip", "192.168.0.1"},
							//{"hostport", "19132"}
						};

                foreach (KeyValuePair<string, string> valuePair in data)
                {
                    string key = valuePair.Key + "\x00" + valuePair.Value + "\x00";
                    byte[] bytes = Encoding.ASCII.GetBytes(key.ToCharArray());
                    stream.Write(bytes, 0, bytes.Length);
                }

                {
                    string str = "\x00\x01player_\x00\x00";
                    byte[] bytes = Encoding.ASCII.GetBytes(str.ToCharArray());
                    stream.Write(bytes, 0, bytes.Length);
                }

                // End the stream with 0 byte
                stream.WriteByte(0);
                var buffer = stream.ToArray();

                _io.SendRaw(buffer, senderEndpoint);
            }
        }

    }

    /// <summary>
    /// This is a no-operation version of QueryHandler which does nothing when called.
    /// It'a an implementation of the Special Case Pattern described by Martin Fowler.
    /// </summary>
    internal class QueryHandler_NoOp : QueryHandler
    {
        public QueryHandler_NoOp()
            : base(null, null)
        {
        }

        public override void Process(IPEndPoint senderEndpoint, byte msgId, byte[] receiveBytes)
        {
        }
    }
}
