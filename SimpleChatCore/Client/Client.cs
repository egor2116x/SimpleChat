using Chat;
using ChatCore.Common;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Client
{
    public class Client : IUserInputListener, IOHandler
    {
        public Client(string addr, Int32 port, ClientUserInputHandler uih, IProtocol p)
        {
            m_serverAddr = addr;
            m_serverPort = port;
            m_inputHandler = uih;
            m_protocol = p;
        }

        public void Connect()
        {
            if (m_serverAddr.Length == 0 || m_serverPort < 1023 || m_serverPort > 49151)
                throw new ClientException("Invalid connection arguments");
            try
            {
                m_client = new TcpClient(m_serverAddr, m_serverPort);
            }
            catch(SocketException e)
            {
                Console.WriteLine("Server is not available. Try later");
                return;
            }
            
            if (!m_client.Connected)
                throw new ClientException(string.Format("Connect to server {0}:{1} failed", m_serverAddr, m_serverPort));

            Console.WriteLine("Successful server connection");
            m_inputHandler.Handle();

            Task<bool> listenUserCommandTask = ListenUserCommand();
            Task<bool> sendTask = Send();
            Task<bool> readTask = Read();

            Task<bool>.WaitAny(listenUserCommandTask, sendTask, readTask);

            if (listenUserCommandTask.IsCompleted)
                Console.WriteLine("Listening user command task completed");
            if (sendTask.IsCompleted)
                Console.WriteLine("Send user messages task completed");
            if (readTask.IsCompleted)
                Console.WriteLine("Read user messages task completed");
        }

        private void Close()
        {
            if (m_client.Connected)
                m_client.Close();
        }

        public Task<bool> ListenUserCommand()
        {
            return Task.Run(() => {
                while (!m_needStop)
                {
                    CLIENT_USER_INPUT userInput = (CLIENT_USER_INPUT)m_inputHandler.GetCommand();
                    if (userInput == CLIENT_USER_INPUT.STOP)
                    {
                        m_needStop = true;
                        if (m_client.Connected)
                            m_client.Close();
                    }
                }
                return true;
            });
        }

        public Task<bool> Read()
        {
            return Task.Run(() => {
                while (!m_needStop)
                {
                    Byte[] data = new Byte[Protocol.MaxPackageSize];
                    String responseData = String.Empty;
                    NetworkStream stream = m_client.GetStream();
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = m_protocol.Unpack(data, bytes);
                    Console.WriteLine("Received: {0}", responseData);
                }
                return true;
            });
        }

        public Task<bool> Send()
        {
            return Task.Run(() => {
                while(!m_needStop)
                {
                    var messages = m_inputHandler.GetMessage();
                    while(messages.Count > 0)
                    {
                        var message = messages.Dequeue();
                        Byte[] data = m_protocol.Pack(message);
                        NetworkStream stream = m_client.GetStream();
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("Sent: {0}", message);
                    }
                }
                return true;
            });
        }

        private string m_serverAddr = string.Empty;
        private Int32 m_serverPort = 0;
        private TcpClient m_client = null;
        private ClientUserInputHandler m_inputHandler = null;
        private bool m_needStop = false;
        private IProtocol m_protocol = null;
    }
}
