using System;
using System.Collections.Generic;
using System.Text;

namespace ChatCore.Common
{
    public static class Command
    {
        public const string shortStop= "/s";
        public const string Stop = "/stop";
    }

    public enum SERVER_USER_INPUT { STOP, NONE };
    public enum CLIENT_USER_INPUT { STOP, NONE };

}
