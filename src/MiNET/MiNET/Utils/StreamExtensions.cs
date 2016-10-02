using fNbt;

namespace MiNET.Utils
{
    public static class StreamExtensions
    {

        public static void Write(this NbtBinaryWriter writer, byte[] data)
        {
            writer.Write(data, 0, data.Length);
        }
        
    }
}