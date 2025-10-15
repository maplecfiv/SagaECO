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
        private static readonly Serilog.Core.Logger _logger = Logger.InitLogger<LoginSession>();

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
                    Logger.getLogger().Error("Cannot connect to the loginserver,please check the configuration!", null);
                    return;
                }

                try
                {
                    sock.Connect(new IPEndPoint(IPAddress.Parse(address.ToString()),
                        Configuration.Configuration.Instance.LoginPort));
                    Connected = true;
                }
                catch (Exception e)
                {
                    Logger.getLogger().Error("Failed... Trying again in 5sec", null);
                    Logger.getLogger().Error(e.ToString(), null);
                    Thread.Sleep(5000);
                    Connected = false;
                }

                times--;
            } while (!Connected);

            Logger.getLogger().Information("Successfully connected to the loginserver", null);
            state = SESSION_STATE.CONNECTED;
            try
            {
                NetIo = new NetIO(sock, commandTable, this);
                if (Configuration.Configuration.Instance.Version >= Version.Saga11)
                    NetIo.FirstLevelLength = 2;
                NetIo.SetMode(NetIO.Mode.Client);
                var p = new Packet(8);
                p.data[7] = 0x10;
                NetIo.SendPacket(p, true, true);
            }
            catch (Exception ex)
            {
                Logger.getLogger().Warning(ex.StackTrace, null);
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
                for (var j = i * 200; j < (i + 1) * 200; j++)
                    list.Add(Configuration.Configuration.Instance.HostedMaps[j]);
                p.HostedMaps = list;
                NetIo.SendPacket(p);
            }

            p = new INTERN_LOGIN_REGISTER();
            p.Password = Configuration.Configuration.Instance.LoginPass;
            list = new List<uint>();
            for (var i = count * 200; i < Configuration.Configuration.Instance.HostedMaps.Count; i++)
                list.Add(Configuration.Configuration.Instance.HostedMaps[i]);
            p.HostedMaps = list;
            NetIo.SendPacket(p);

            var p1 = new INTERN_LOGIN_REQUEST_CONFIG();
            p1.Version = Configuration.Configuration.Instance.Version;
            NetIo.SendPacket(p1);
        }

        public void OnGetConfig(INTERN_LOGIN_REQUEST_CONFIG_ANSWER p)
        {
            if (p.AuthOK)
            {
                Configuration.Configuration.Instance.StartupSetting = p.StartupSetting;
                Logger.getLogger().Information("Got Configuration from login server:");
                foreach (var i in Configuration.Configuration.Instance.StartupSetting.Keys)
                {
                    //Console.ForegroundColor = ConsoleColor.Green;
                    _logger.Debug("[Info]");
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    _logger.Debug("Configuration for Race[");
                    //Console.ForegroundColor = ConsoleColor.White;
                    _logger.Debug(i.ToString());
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    _logger.Debug("]");
                    //Console.ForegroundColor = ConsoleColor.White;
                    _logger.Debug(":\r\n      " + Configuration.Configuration.Instance.StartupSetting[i]);
                    //Console.ResetColor();
                }

                state = SESSION_STATE.IDENTIFIED;
            }
            else
            {
                Logger.getLogger().Error("FATAL: Request Rejected from loginserver,terminating");
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