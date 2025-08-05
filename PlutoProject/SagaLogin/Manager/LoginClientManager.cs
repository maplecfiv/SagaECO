//Comment this out to deactivate the dead lock check!
//#define DeadLockCheck

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SagaDB.Actor;
using SagaLib;
using SagaLogin.Network.Client;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Client.Chat;
using SagaLogin.Packets.Client.FriendList;
using SagaLogin.Packets.Client.Login;
using SagaLogin.Packets.Client.NyaShield;
using SagaLogin.Packets.Client.Ring;
using SagaLogin.Packets.Client.Tamaire;
using SagaLogin.Packets.Client.Tool;
using SagaLogin.Packets.Client.WRP;
using SagaLogin.Packets.Map;

namespace SagaLogin.Manager
{
    public sealed class LoginClientManager : ClientManager
    {
        public Thread check;

        private LoginClientManager()
        {
            /*
            this.clients = new Dictionary<uint, GatewayClient>();
            this.commandTable = new Dictionary<ushort, Packet>();

            //here for packets
            this.commandTable.Add(0x0101, new Packets.Client.SendKey());
            this.commandTable.Add(0x0102, new Packets.Client.SendGUID());
            this.commandTable.Add(0x0104, new Packets.Client.SendIdentify());
            this.commandTable.Add(0x0105, new Packets.Client.RequestSession());



            */
            Clients = new List<LoginClient>();
            commandTable = new Dictionary<ushort, Packet>();

            commandTable.Add(0xDDDF, new TOOL_GIFTS());


            commandTable.Add(0x0001, new CSMG_SEND_VERSION());
            commandTable.Add(0x000A, new CSMG_PING());
            commandTable.Add(0x002A, new CSMG_CHAR_STATUS());
            commandTable.Add(0x00A0, new CSMG_CHAR_CREATE());
            commandTable.Add(0x00A5, new CSMG_CHAR_DELETE());
            commandTable.Add(0x00A7, new CSMG_CHAR_SELECT());
            commandTable.Add(0x001F, new CSMG_LOGIN());
            commandTable.Add(0x0032, new CSMG_REQUEST_MAP_SERVER());
            commandTable.Add(0x00C9, new CSMG_CHAT_WHISPER());
            commandTable.Add(0x00D2, new CSMG_FRIEND_ADD());
            commandTable.Add(0x00D4, new CSMG_FRIEND_ADD_REPLY());
            commandTable.Add(0x00D7, new CSMG_FRIEND_DELETE());
            commandTable.Add(0x00E1, new CSMG_FRIEND_DETAIL_UPDATE());
            commandTable.Add(0x00E6, new CSMG_FRIEND_MAP_UPDATE());
            commandTable.Add(0x0104, new CSMG_RING_EMBLEM_NEW());
            commandTable.Add(0x0109, new CSMG_RING_EMBLEM());
            //this.commandTable.Add(0x015F, new Packets.Client.CSMG_SEND_GUID());
            commandTable.Add(0x0172, new CSMG_WRP_REQUEST());

            commandTable.Add(0xFFF0, new INTERN_LOGIN_REGISTER());
            commandTable.Add(0xFFF1, new INTERN_LOGIN_REQUEST_CONFIG());

            commandTable.Add(0x0151, new CSMG_NYASHIELD_VERSION());

            commandTable.Add(0x0226, new CSMG_TAMAIRE_LIST_REQUEST());

            waitressQueue = new AutoResetEvent(true);
            //deadlock check
            check = new Thread(checkCriticalArea);
            check.Name = string.Format("DeadLock checker({0})", check.ManagedThreadId);
#if DeadLockCheck
            check.Start();
#endif
        }

        public static LoginClientManager Instance => Nested.instance;

        /// <summary>
        ///     全部在线客户端，包括Map服务器
        /// </summary>
        public List<LoginClient> Clients { get; }

        /// <summary>
        ///     Connects new clients
        /// </summary>
        public override void NetworkLoop(int maxNewConnections)
        {
            for (var i = 0; listener.Pending() && i < maxNewConnections; i++)
            {
                var sock = listener.AcceptSocket();
                var ip = sock.RemoteEndPoint.ToString().Substring(0, sock.RemoteEndPoint.ToString().IndexOf(':'));
                Logger.ShowInfo("New client from: " + sock.RemoteEndPoint, null);
                var client = new LoginClient(sock, commandTable);
                Clients.Add(client);
            }
        }

        public override void OnClientDisconnect(Client client_t)
        {
            Clients.Remove((LoginClient)client_t);
        }

        public LoginClient FindClient(ActorPC pc)
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.selectedChar != null
                select c;
            chr = from c in chr.ToList()
                where c.selectedChar.CharID == pc.CharID
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public LoginClient FindClient(uint charID)
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.selectedChar != null
                select c;
            chr = from c in chr.ToList()
                where c.selectedChar.CharID == charID
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public LoginClient FindClient(string charName)
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.selectedChar != null
                select c;
            chr = from c in chr.ToList()
                where c.selectedChar.Name == charName
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public List<LoginClient> FindAllOnlineAccounts()
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.account != null
                select c;
            if (chr.Count() != 0)
                return chr.ToList();
            return null;
        }

        public LoginClient FindClientAccountID(uint accountID)
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.account != null
                select c;
            chr = from c in chr.ToList()
                where c.account.AccountID == accountID
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        public LoginClient FindClientAccount(string accountName)
        {
            var chr =
                from c in Clients
                where !c.IsMapServer && c.account != null
                select c;
            chr = from c in chr.ToList()
                where c.account.Name == accountName
                select c;
            if (chr.Count() != 0)
                return chr.First();
            return null;
        }

        private class Nested
        {
            internal static readonly LoginClientManager instance = new LoginClientManager();

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }
        }
    }
}