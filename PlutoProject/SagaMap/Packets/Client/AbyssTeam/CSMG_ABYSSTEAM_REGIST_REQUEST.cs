using System.Text;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ABYSSTEAM_REGIST_REQUEST : Packet
    {
        public CSMG_ABYSSTEAM_REGIST_REQUEST()
        {
            offset = 2;
        }

        public uint LeaderID => GetUInt(3);

        public string Password
        {
            get
            {
                var Length = GetByte(7);
                return Encoding.UTF8.GetString(GetBytes(Length, 8)).Replace("/0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_REGIST_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamRegistRequest(this);
        }
    }
}