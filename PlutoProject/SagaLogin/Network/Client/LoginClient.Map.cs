using SagaLib;
using SagaLogin.Manager;
using SagaLogin.Packets.Map;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        private MapServer server;

        public void OnInternMapRequestConfig(INTERN_LOGIN_REQUEST_CONFIG p)
        {
            Configuration.Instance.Version = p.Version;
            var p1 = new INTERN_LOGIN_REQUEST_CONFIG_ANSWER();
            p1.AuthOK = server.Password == Configuration.Instance.Password;
            p1.StartupSetting = Configuration.Instance.StartupSetting;
            netIO.SendPacket(p1);

            Logger.ShowInfo(string.Format("Mapserver:{0}:{1} is requesting configuration...", server.IP, server.port));
        }

        public void OnInternMapRegister(INTERN_LOGIN_REGISTER p)
        {
            var server = p.MapServer;
            IsMapServer = true;
            if (this.server == null)
            {
                this.server = server;
                if (server.Password != Configuration.Instance.Password)
                {
                    Logger.ShowWarning(string.Format(
                        "Mapserver:{0}:{1} is trying to register maps with wrong password:{2}", server.IP, server.port,
                        server.Password));
                    return;
                }
            }
            else
            {
                if (server.Password != Configuration.Instance.Password)
                {
                    Logger.ShowWarning(string.Format(
                        "Mapserver:{0}:{1} is trying to register maps with wrong password:{2}", server.IP, server.port,
                        server.Password));
                    return;
                }

                foreach (var i in server.HostedMaps)
                    if (!this.server.HostedMaps.Contains(i))
                        this.server.HostedMaps.Add(i);
            }

            var count = 0;
            foreach (var i in server.HostedMaps)
                if (!MapServerManager.Instance.MapServers.ContainsKey(i))
                {
                    MapServerManager.Instance.MapServers.Add(i, this.server);
                    count++;
                }
                else
                {
                    var oldserver = MapServerManager.Instance.MapServers[i];
                    //Logger.ShowWarning(string.Format("MapID:{0} was already hosted by Mapserver:{1}:{2}, skiping...", i, oldserver.IP, oldserver.port));
                }

            Logger.ShowInfo(
                string.Format("{0} maps registered for MapServer:{1}:{2}...", count, server.IP, server.port));
        }
    }
}