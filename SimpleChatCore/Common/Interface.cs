using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Common
{
    public interface IUserInputHandler
    {
        void ReadUserInput();
        void StopReadUserInput();
    }

    public interface IUserInputCommandHandler
    {
        uint GetCommand();
    }

    public interface IUserInputMessageHandler
    {
        Queue<string> GetMessage();
    }

    public interface IClientInputHandler : IUserInputHandler, IUserInputCommandHandler, IUserInputMessageHandler {}

    public interface IServerInputHandler : IUserInputHandler, IUserInputCommandHandler {}

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
