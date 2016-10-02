using System.IO;
using System.IO.Compression;

namespace MiNET.Utils
{
    public sealed class ZLibStream : DeflateStream
    {
        private int adler32A = 1, adler32B;
        private MemoryStream _buffer = MiNetServer.MemoryStreamManager.GetStream();

        private const int ChecksumModulus = 65521;

        public int Checksum
        {
            get
            {
                UpdateChecksum(_buffer.GetBuffer(), 0, _buffer.Length);
                return ((adler32B*65536) + adler32A);
            }
        }


        private void UpdateChecksum(byte[] data, int offset, long length)
        {
            for (long counter = 0; counter < length; ++counter)
            {
                adler32A = (adler32A + (data[offset + counter]))%ChecksumModulus;
                adler32B = (adler32B + adler32A)%ChecksumModulus;
            }
        }


        public ZLibStream(Stream stream, CompressionLevel level, bool leaveOpen)
            : base(stream, level, leaveOpen)
        {
        }


        public override void Write(byte[] array, int offset, int count)
        {
//			UpdateChecksum(array, offset, count);
            _buffer.Write(array, offset, count);
            base.Write(array, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _buffer.Dispose();
        }
    }
}