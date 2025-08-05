using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.NPC
{
    public class CSMG_NPC_EVENT_START : Packet
    {
        public CSMG_NPC_EVENT_START()
        {
            offset = 2;
        }

        public uint EventID => GetUInt(2);

        public byte X => GetByte(6);

        public byte Y => GetByte(7);

        public override Packet New()
        {
            return new CSMG_NPC_EVENT_START();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnNPCEventStart(this);
        }
    }
}