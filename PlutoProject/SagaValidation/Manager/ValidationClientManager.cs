using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using SagaLib;
using SagaValidation.Network.Client;

namespace SagaValidation.Manager
{
    public sealed class ValidationClientManager : ClientManager
    {
        List<ValidationClient> clients;
        public Thread check;

        ValidationClientManager()
        {
            clients = new List<ValidationClient>();
            CommandTable = new Dictionary<ushort, Packet>();
            CommandTable.Add(0x0001, new Packets.Client.CSMG_SEND_VERSION());
            CommandTable.Add(0x001F, new Packets.Client.CSMG_LOGIN());
            CommandTable.Add(0x0031, new Packets.Client.CSMG_SERVERLET_ASK());
            CommandTable.Add(0x002F, new Packets.Client.CSMG_UNKNOWN_LIST());
            CommandTable.Add(0x000A, new Packets.Client.CSMG_PING());
            WaitressQueue = new AutoResetEvent(true);
        }

        public static ValidationClientManager Instance
        {
            get { return Nested.instance; }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly ValidationClientManager instance = new ValidationClientManager();
        }

        /// <summary>
        /// Connects new clients
        /// </summary>
        public override void NetworkLoop(int maxNewConnections)
        {
            for (int i = 0; Listener.Pending() && i < maxNewConnections; i++)
            {
                Socket sock = Listener.AcceptSocket();
                Logger.getLogger().Information("New client from: " + sock.RemoteEndPoint.ToString(), null);

                ValidationClient client = new ValidationClient(sock, CommandTable);
                clients.Add(client);
            }
        }
    }
}