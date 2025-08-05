using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.AbyssTeam
{
    public class CSMG_ABYSSTEAM_LEAVE_REQUEST : Packet
    {
        public CSMG_ABYSSTEAM_LEAVE_REQUEST()
        {
            offset = 2;
        }

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_LEAVE_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamLeaveRequest(this);
        }
    }
}