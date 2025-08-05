using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_SELECT : Packet
    {
        public CSMG_NPC_SELECT()
        {
            offset = 2;
        }

        public byte Result => GetByte(2);

        public override Packet New()
        {
            return new CSMG_NPC_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCSelect(this);
        }
    }
}