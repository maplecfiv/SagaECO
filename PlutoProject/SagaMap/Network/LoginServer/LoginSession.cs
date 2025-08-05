using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SagaLib;
using SagaMap.Packets.Login;
using Version = SagaLib.Version;

namespace SagaMap.Network.LoginServer
{
    public class LoginSession : SagaLib.Client
    {
        public enum SESSION_STATE
        {
            CONNECTED,
            DISCONNECTED,
            NOT_IDENTIFIED,
            IDENTIFIED,
            REJECTED
        }

        private readonly Dictionary<ushort, Packet> commandTable;
        private Socket sock;

        /// <summary>
        ///     The state of this session. Changes from NOT_IDENTIFIED to IDENTIFIED or REJECTED.
        /// </summary>
        public SESSION_STATE state = SESSION_STATE.CONNECTED;

        public LoginSession()
        {
            commandTable = new Dictionary<ushort, Packet>();

            commandTable.Add(0xFFF2, new INTERN_LOGIN_REQUEST_CONFIG_ANSWER());


            var newSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock = newSock;
            Connect();
        }

        public void Connect()
        {
            var address = Dns.GetHostAddresses(Configuration.Configuration.Instance.LoginHost)[0];
            var Connected = false;
            var times = 5;
            do
            {
                if (times < 0)
                {
                    Logger.ShowError("Cannot connect to the loginserver,please check the configuration!", null);
                    return;
                }

                try
                {
                    sock.Connect(new IPEndPoint(IPAddress.Parse(address.ToString()), Configuration.Configuration.Instance.LoginPort));
                    Connected = true;
                }
                catch (Exception e)
                {
                    Logger.ShowError("Failed... Trying again in 5sec", null);
                    Logger.ShowError(e.ToString(), null);
                    Thread.Sleep(5000);
                    Connected = false;
                }

                times--;
            } while (!Connected);

            Logger.ShowInfo("Successfully connected to the loginserver", null);
            state = SESSION_STATE.CONNECTED;
            try
            {
                netIO = new NetIO(sock, commandTable, this);
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    netIO.FirstLevelLength = 2;
                netIO.SetMode(NetIO.Mode.Client);
                var p = new Packet(8);
                p.data[7] = 0x10;
                netIO.SendPacket(p, true, true);
            }
            catch (Exception ex)
            {
                Logger.ShowWarning(ex.StackTrace, null);
            }
        }

        public override void OnConnect()
        {
            state = SESSION_STATE.NOT_IDENTIFIED;
            INTERN_LOGIN_REGISTER p;
            var count = Configuration.Configuration.Instance.HostedMaps.Count / 200;
            List<uint> list;
            for (var i = 0; i < count; i++)
            {
                p = new INTERN_LOGIN_REGISTER();
                p.Password = Configuration.Configuration.Instance.LoginPass;
                list = new List<uint>();
                for (var j = i * 200; j < (i + 1) * 200; j++) list.Add(Configuration.Configuration.Instance.HostedMaps[j]);
                p.HostedMaps = list;
                netIO.SendPacket(p);
            }

            p = new INTERN_LOGIN_REGISTER();
            p.Password = Configuration.Configuration.Instance.LoginPass;
            list = new List<uint>();
            for (var i = count * 200; i < Configuration.Configuration.Instance.HostedMaps.Count; i++)
                list.Add(Configuration.Configuration.Instance.HostedMaps[i]);
            p.HostedMaps = list;
            netIO.SendPacket(p);

            var p1 = new INTERN_LOGIN_REQUEST_CONFIG();
            p1.Version = Configuration.Configuration.Instance.Version;
            netIO.SendPacket(p1);
        }

        public void OnGetConfig(INTERN_LOGIN_REQUEST_CONFIG_ANSWER p)
        {
            if (p.AuthOK)
            {
                Configuration.Configuration.Instance.StartupSetting = p.StartupSetting;
                Logger.ShowInfo("Got Configuration from login server:");
                foreach (var i in Configuration.Configuration.Instance.StartupSetting.Keys)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[Info]");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("Configuration for Race[");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(i.ToString());
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("]");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(":\r\n      " + Configuration.Configuration.Instance.StartupSetting[i]);
                    Console.ResetColor();
                }

                state = SESSION_STATE.IDENTIFIED;
            }
            else
            {
                Logger.ShowError("FATAL: Request Rejected from loginserver,terminating");
                state = SESSION_STATE.REJECTED;
            }
        }

        public override void OnDisconnect()
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connect();
        }
    }
}