using System.Collections.Generic;
using System.Net.Sockets;
using SagaDB;
using SagaLib;

namespace SagaValidation.Network.Client
{
    public partial class ValidationClient : SagaLib.Client
    {
        private string client_Version;

        private uint frontWord, backWord;

        public bool IsMapServer = false;

        //public Account account;

        public enum SESSION_STATE
        {
            LOGIN,
            MAP,
            REDIRECTING,
            DISCONNECTED
        }

        public SESSION_STATE state;

        public ValidationClient(Socket mSock, Dictionary<ushort, Packet> mCommandTable)
        {
            NetIo = new NetIO(mSock, mCommandTable, this);
            NetIo.FirstLevelLength = 2;
            NetIo.SetMode(NetIO.Mode.Server);
            if (NetIo.sock.Connected) OnConnect();
        }

        public override void OnConnect()
        {
        }

        public void OnLogin(Packets.Client.CSMG_LOGIN p)
        {
            p.GetContent();

            //Establish TCP ACK Flag at first handshake
            Packets.Server.SSMG_LOGIN_ACK p0 = new Packets.Server.SSMG_LOGIN_ACK();
            p0.LoginResult = Packets.Server.SSMG_LOGIN_ACK.Result.OK;
            NetIo.SendPacket(p0);

            Account tmp = ValidationServer.accountDB.GetUser(p.UserName.Replace("\\", "").Replace("'", "\\'"));

            if (Configuration.Instance.ServerClose == true)
            {
                if (tmp.GMLevel <= 200)
                {
                    Packets.Server.SSMG_LOGIN_ACK p1 = new Packets.Server.SSMG_LOGIN_ACK();
                    p1.LoginResult = Packets.Server.SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                    NetIo.SendPacket(p1);
                    NetIo.Disconnect();
                    return;
                }
            }


            if (ValidationServer.accountDB.CheckPassword(p.UserName, p.Password, frontWord, backWord))
            {
                //Prepare Account Information 


                //Login ACK should not be here
                /*
                Packets.Server.SSMG_LOGIN_ACK p1 = new SagaValidation.Packets.Server.SSMG_LOGIN_ACK();
                p1.LoginResult = SagaValidation.Packets.Server.SSMG_LOGIN_ACK.Result.OK;
                this.NetIo.SendPacket(p1);
                */
                //Check if Account Banned
                if (tmp.Banned)
                {
                    Packets.Server.SSMG_LOGIN_ACK p2 = new Packets.Server.SSMG_LOGIN_ACK();
                    p2.LoginResult = Packets.Server.SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BFALOCK;
                    NetIo.SendPacket(p2);
                    NetIo.Disconnect();
                    return;
                    //TCP Connection terminated.
                }
            }
            else
            {
                Packets.Server.SSMG_LOGIN_ACK p1 = new Packets.Server.SSMG_LOGIN_ACK();
                p1.LoginResult = Packets.Server.SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BADPASS;
                NetIo.SendPacket(p1);
                NetIo.Disconnect();
                return;
                //TCP Connection terminated.
            }
        }

        public void OnSendVersion(Packets.Client.CSMG_SEND_VERSION p)
        {
            Logger.getLogger().Information("Client(Version:" + p.GetVersion() + ") is trying to connect...");
            client_Version = p.GetVersion();

            string args = "FF FF E8 6A 6A CA DC E8 06 05 2B 29 F8 96 2F 86 7C AB 2A 57 AD 30";
            byte[] buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            Packet p3 = new Packet();
            p3.data = buf;
            NetIo.SendPacket(p3);

            Packets.Server.SSMG_VERSION_ACK p1 = new Packets.Server.SSMG_VERSION_ACK();
            p1.SetResult(Packets.Server.SSMG_VERSION_ACK.Result.OK);
            p1.SetVersion(client_Version);
            NetIo.SendPacket(p1);

            Packets.Server.SSMG_LOGIN_ALLOWED p2 = new Packets.Server.SSMG_LOGIN_ALLOWED();
            frontWord = (uint)Global.Random.Next();
            backWord = (uint)Global.Random.Next();
            p2.FrontWord = frontWord;
            p2.BackWord = backWord;
            NetIo.SendPacket(p2);
        }

        public void OnPing(Packets.Client.CSMG_PING p)
        {
            Packets.Server.SSMG_PONG p1 = new Packets.Server.SSMG_PONG();
            NetIo.SendPacket(p1);
        }

        public void OnUnknownList(Packets.Client.CSMG_UNKNOWN_LIST p)
        {
            Packets.Server.SSMG_UNKNOWN_RETURN p1 = new Packets.Server.SSMG_UNKNOWN_RETURN();
            NetIo.SendPacket(p1);
        }
    }
}