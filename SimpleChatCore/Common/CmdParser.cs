using System;

namespace Chat
{
    public class CmdParser
    {
        public enum MODE { CLIENT, SERVER, NONE };
        public CmdParser(string[] args)
        {
            this.args = args;
            this.port = -1;
            this.address = string.Empty;
            this.mode = MODE.NONE;
        }
        public void Parse()
        {
            if (this.args.Length <= 2)
                throw new CmdParserException("Insufficient command line arguments");

            /* parse mode */
            string strMode = FindMode();

            if (strMode.ToLower().Equals("client"))
                mode = MODE.CLIENT;
            else if (strMode.ToLower().Equals("server"))
                mode = MODE.SERVER;
            else
            {
                mode = MODE.NONE;
                return;
            }

            /* parse port */
            string strPort = FindPort();

            if (strPort.Length == 0)
                return;

            port = Convert.ToInt32(strPort);

            /* parse address */
            if (mode == MODE.CLIENT)
            {
                string strAddress = FindAddress();

                if (strAddress.Length == 0)
                    return;

                address = strAddress;
            }
            else
            {
                address = "127.0.0.1";
            }

        }
        public string[] args { set; get; }
        public MODE mode { set; get; }
        public string address { set; get; }
        public int port { set; get; }

        private string FindArg(string argName)
        {
            if (this.args.Length <= 2)
                return string.Empty;

            if(argName.Length == 0)
                return string.Empty;

            string result = string.Empty;
            int idx = -1;
            for(int i = 0; i < args.Length; i++)
            {
                if(args[i].Equals(argName))
                {
                    idx = i;
                    continue;
                }

                if(idx != -1 && i == idx + 1)
                {
                    result = args[i];
                    break;
                }
            }
            return result;
        }

        private string FindMode()
        {
            return FindArg("/m");
        }

        private string FindPort()
        {
            return FindArg("/p");
        }

        private string FindAddress()
        {
            return FindArg("/a");
        }
    }
}
