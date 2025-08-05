using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_PET_SELECT : Packet
    {
        public CSMG_NPC_PET_SELECT()
        {
            offset = 2;
        }

        public uint Result => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_NPC_PET_SELECT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCPetSelect(this);
        }
    }
}