using SagaLib;
using SagaLogin.Manager;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Map
{
    public class INTERN_LOGIN_REGISTER : Packet
    {
        public INTERN_LOGIN_REGISTER()
        {
            offset = 2;
        }

        public MapServer MapServer
        {
            get
            {
                var server = new MapServer();
                var size = GetByte(2);
                var buf = new byte[size];
                ushort offset;
                buf = GetBytes(size, 3);
                server.Password = Global.Unicode.GetString(buf);

                offset = (ushort)(3 + size);

                var size2 = GetByte(offset);
                buf = new byte[size2];
                buf = GetBytes(size2, (ushort)(offset + 1));
                server.IP = Global.Unicode.GetString(buf);

                offset = (ushort)(4 + size + size2);

                server.port = GetInt(offset);
                size = GetByte((ushort)(offset + 4));
                for (var i = 0; i < size; i++) server.HostedMaps.Add(GetUInt((ushort)(offset + 5 + i * 4)));
                return server;
            }
        }

        public override Packet New()
        {
            return new INTERN_LOGIN_REGISTER();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnInternMapRegister(this);
        }
    }
}