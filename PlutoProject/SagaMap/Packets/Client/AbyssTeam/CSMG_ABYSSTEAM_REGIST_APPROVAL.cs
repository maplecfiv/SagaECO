using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_ABYSSTEAM_REGIST_APPROVAL : Packet
    {
        public CSMG_ABYSSTEAM_REGIST_APPROVAL()
        {
            offset = 2;
        }

        public byte Result => GetByte(2);

        public uint CharID => GetUInt(3);

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_REGIST_APPROVAL();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamRegistApproval(this);
        }
    }
}