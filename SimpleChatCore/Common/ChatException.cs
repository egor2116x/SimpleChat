using System;
using System.Collections.Generic;
using System.Text;

namespace Chat
{
    public class CmdParserException : Exception
    {
        public CmdParserException(string message) : base(message) {}
    }

    public class ServerException : Exception
    {
        public ServerException(string message) : base(message) { }
    }

    public class ClientException : Exception
    {
        public ClientException(string message) : base(message) { }
    }
}
