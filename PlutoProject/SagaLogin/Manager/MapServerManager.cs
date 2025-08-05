using System.Collections.Generic;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Manager
{
    public class MapServer
    {
        public LoginClient Client;
        public List<uint> HostedMaps = new List<uint>();
        public string IP;
        public string Password;
        public int port;
    }

    public class MapServerManager : Singleton<MapServerManager>
    {
        public Dictionary<uint, MapServer> MapServers { get; } = new Dictionary<uint, MapServer>();
    }
}