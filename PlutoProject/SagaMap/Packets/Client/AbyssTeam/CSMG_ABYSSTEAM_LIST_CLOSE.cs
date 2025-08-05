using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.AbyssTeam
{
    public class CSMG_ABYSSTEAM_LIST_CLOSE : Packet
    {
        public CSMG_ABYSSTEAM_LIST_CLOSE()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_LIST_CLOSE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamListClose(this);
        }
    }
}