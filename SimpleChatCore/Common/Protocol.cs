using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCore.Common
{
    public class Protocol : IProtocol
    {
        public byte[] Pack(string message)
        {
            return System.Text.Encoding.ASCII.GetBytes(message);
        }

        public string Unpack(byte[] data, Int32 bytes)
        {
            return System.Text.Encoding.ASCII.GetString(data, 0, bytes);
        }

        public static UInt32 MaxPackageSize { get { return 4096; } }
    }
}
