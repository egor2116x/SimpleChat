using ChatCore.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatCore.Server
{
    public class Server : IUserInputListener
    {
        public class Connection
        {
            public Connection(Guid _id, TcpClient _client)
            {
                id = _id;
                client = _client;
            }

            public Guid id { set; get; }
            public TcpClient client { set; get; }
        }

        public Server(IPAddress addr, Int32 port, IUserInputHandler uih, IProtocol p)
        {
            m_addr = addr;
            m_port = port;
            m_clients = new List<Connection>();
            m_userInputHandler = uih;
            m_protocol = p;
        }

        public void Listen()
        {
            server = new TcpListener(m_addr, m_port);
            server.Start();
            m_userInputHandler.Handle();
            Task<bool> listenUserCommandTask = ListenUserCommand();

            while (!m_needStop)
            {
                Console.WriteLine("Waiting for a connection... ");

                TcpClient client = server.AcceptTcpClient();
                Guid guid = System.Guid.NewGuid();
                Console.WriteLine("Client connected {0}", guid.ToString());
                Connection cc = new Connection(guid, client);
                m_clients.Add(cc);
                StartClientThread(cc);
            }

            Task<bool>.WaitAny(listenUserCommandTask);

            if (listenUserCommandTask.IsCompleted)
                Console.WriteLine("Listening user command task completed");
        }

        private async void StartClientThread(Connection connection)
        {
            await Task.Run(() => {

                Byte[] bytes = new Byte[Protocol.MaxPackageSize];
                String data = null;
                NetworkStream stream = connection.client.GetStream();
                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = m_protocol.Unpack(bytes, i);
                    Console.WriteLine("Received from: {0}, {1}", connection.id.ToString(), data);
                    CheckActiveConnection();
                    foreach (var clientConnection in m_clients)
                    {
                        if(clientConnection.id != connection.id)
                        {
                            byte[] msg = m_protocol.Pack("<" + connection.id.ToString() + ">" + data);
                            NetworkStream clientStream = clientConnection.client.GetStream();
                            clientStream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Sent to: {0}, {1}", clientConnection.id.ToString(), data);
                        }
                    }
                }
            });
        }

        public Task<bool> ListenUserCommand()
        {
            return Task.Run(() => {
                while (!m_needStop)
                {
                    SERVER_USER_INPUT userInput = (SERVER_USER_INPUT)m_userInputHandler.GetCommand();
                    if (userInput == SERVER_USER_INPUT.STOP)
                    {
                        m_needStop = true;
                        server.Stop();
                        foreach (var connection in m_clients)
                        {
                            if(connection.client.Connected)
                                connection.client.Close();
                        }
                    }
                }
                return true;
            });
        }

        private void CheckActiveConnection()
        {
            foreach (var connection in m_clients)
            {
                if (!connection.client.Connected)
                {
                    m_clients.Remove(connection);
                }
            }
        }

        private IPAddress m_addr = null;
        private Int32 m_port = 0;
        private TcpListener server = null;
        private List<Connection> m_clients = null;
        private IUserInputHandler m_userInputHandler = null;
        private bool m_needStop = false;
        private IProtocol m_protocol = null;
    }
}
