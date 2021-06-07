using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Common
{
    public interface IUserInputHandler
    {
        void Handle();
        void Stop();
        uint GetCommand();
    }

    public interface IUserInputListener
    {
        Task<bool> ListenUserCommand();
    }

    public interface IOHandler
    {
        Task<bool> Read();
        Task<bool> Send();
    }

    public interface IProtocol
    {
        Byte[] Pack(string message);
        string Unpack(Byte[] data, Int32 bytes);
    }
}
