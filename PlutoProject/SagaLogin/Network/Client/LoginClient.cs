using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SagaDB;
using SagaLib;
using SagaLogin.Manager;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;
using Version = SagaLib.Version;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public enum SESSION_STATE
        {
            LOGIN,
            MAP,
            REDIRECTING,
            DISCONNECTED
        }

        public Account account;
        private string client_Version;

        private uint frontWord, backWord;

        public bool IsMapServer = false;
        public SESSION_STATE state;

        public LoginClient(Socket mSock, Dictionary<ushort, Packet> mCommandTable)
        {
            netIO = new NetIO(mSock, mCommandTable, this);
            if (Configuration.Instance.Version >= Version.Saga11)
                netIO.FirstLevelLength = 2;
            netIO.SetMode(NetIO.Mode.Server);
            if (netIO.sock.Connected) OnConnect();
        }

        public override string ToString()
        {
            try
            {
                if (netIO != null) return netIO.sock.RemoteEndPoint.ToString();
                return "LoginClient";
            }
            catch (Exception)
            {
                return "LoginClient";
            }
        }

        public override void OnConnect()
        {
        }

        public override void OnDisconnect()
        {
            if (currentStatus != CharStatus.OFFLINE)
            {
                if (IsMapServer)
                {
                    Logger.ShowWarning("A map server has just disconnected...");
                    foreach (var i in server.HostedMaps)
                        if (MapServerManager.Instance.MapServers.ContainsKey(i))
                            MapServerManager.Instance.MapServers.Remove(i);
                }
                else
                {
                    currentStatus = CharStatus.OFFLINE;
                    currentMap = 0;
                    try
                    {
                        SendStatusToFriends();
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }

                    if (account != null)
                        Logger.ShowInfo(account.Name + " logged out.");
                }
            }

            if (LoginClientManager.Instance.Clients.Contains(this))
                LoginClientManager.Instance.Clients.Remove(this);
        }

        public void OnWRPRequest(CSMG_WRP_REQUEST p)
        {
            var p1 = new SSMG_WRP_LIST();
            p1.RankingList = LoginServer.charDB.GetWRPRanking();
            netIO.SendPacket(p1);
        }
    }
}