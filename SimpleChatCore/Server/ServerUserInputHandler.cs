using ChatCore.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Server
{
    public class ServerUserInputHandler : IServerInputHandler
    {
        public uint GetCommand()
        {
            uint last = (uint)m_lastCommand;
            m_lastCommand = SERVER_USER_INPUT.NONE;
            return last;
        }

        public async void ReadUserInput()
        {
            await Task.Run(() =>
            {
                while (!m_stop)
                {
                    Console.Write("> ");
                    string line = Console.ReadLine();
                    if (line.Length == 0)
                        continue;

                    if (line.Contains(Command.shortStop) || line.Contains(Command.Stop))
                    {
                        StopReadUserInput();
                        continue;
                    }

                    Console.WriteLine("Unknown server command");
                }
            });
        }

        public void StopReadUserInput()
        {
            m_stop = true;
            m_lastCommand = SERVER_USER_INPUT.STOP;
        }

        private SERVER_USER_INPUT m_lastCommand = SERVER_USER_INPUT.NONE;
        private bool m_stop = false;
    }
}
