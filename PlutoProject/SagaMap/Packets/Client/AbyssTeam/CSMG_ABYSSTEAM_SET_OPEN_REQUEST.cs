using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ABYSSTEAM_SET_OPEN_REQUEST : Packet
    {
        public CSMG_ABYSSTEAM_SET_OPEN_REQUEST()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_SET_OPEN_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamSetOpenRequest(this);
        }
    }
}