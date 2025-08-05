using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_PPROTECT_LIST_OPEN : Packet
    {
        public CSMG_PPROTECT_LIST_OPEN()
        {
            offset = 2;
        }

        public ushort Page => GetUShort(2);

        public int Search => GetInt(4);

        public override Packet New()
        {
            return new CSMG_PPROTECT_LIST_OPEN();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnPProtectListOpen(this);
        }
    }
}