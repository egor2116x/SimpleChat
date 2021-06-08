using Chat;
using ChatCore.Client;
using ChatCore.Common;
using ChatCore.Server;
using System;
using System.Net;

namespace SimpleChat
{
    class Program
    {
        static void Main(string[] args)
        {
            CmdParser cmd = new CmdParser(args);

            try
            {
                cmd.Parse();
                switch(cmd.mode)
                {
                    case CmdParser.MODE.CLIENT:
                        Client client = new Client(cmd.address, cmd.port, new ClientUserInputHandler(), new Protocol());
                        client.Connect();
                        break;
                    case CmdParser.MODE.SERVER:
                        Server server = new Server(IPAddress.Parse(cmd.address), cmd.port, new ServerUserInputHandler(), new Protocol());
                        server.Listen();
                        break;
                    default:
                        Console.WriteLine("Unknown application mode");
                        break;
                }
            }
            catch(CmdParserException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ServerException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ClientException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
