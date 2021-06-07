using ChatCore.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Client
{
    public class ClientUserInputHandler : IUserInputHandler
    {
        public uint GetCommand()
        {
            uint last = (uint)m_lastCommand;
            m_lastCommand = CLIENT_USER_INPUT.NONE;
            return last;
        }

        public async void Handle()
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
                        Stop();
                        continue;
                    }

                    m_messages.Enqueue(line);
                }
            });
        }

        public Queue<string> GetMessage()
        {
            return m_messages;
        }

        public void Stop()
        {
            m_stop = true;
            m_lastCommand = CLIENT_USER_INPUT.STOP;
        }

        private CLIENT_USER_INPUT m_lastCommand = CLIENT_USER_INPUT.NONE;
        private bool m_stop = false;
        private Queue<string> m_messages = new Queue<string>();
    }
}
