using ChatCore.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Client
{
    public class ClientUserInputHandler : IClientInputHandler
    {
        public uint GetCommand()
        {
            uint last = (uint)m_lastCommand;
            m_lastCommand = CLIENT_USER_INPUT.NONE;
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

                    m_messages.Enqueue(line);
                }
            });
        }

        public void StopReadUserInput()
        {
            m_stop = true;
            m_lastCommand = CLIENT_USER_INPUT.STOP;
        }

        public Queue<string> GetMessage()
        {
            return m_messages;
        }

        private CLIENT_USER_INPUT m_lastCommand = CLIENT_USER_INPUT.NONE;
        private bool m_stop = false;
        private Queue<string> m_messages = new Queue<string>();
    }
}
