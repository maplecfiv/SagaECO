using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client
{
    public class CSMG_CHAR_SLOT : Packet
    {
        public CSMG_CHAR_SLOT()
        {
            offset = 2;
        }

        public byte Slot => GetByte(6);

        public override Packet New()
        {
            return new CSMG_CHAR_SLOT();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnCharSlot(this);
        }
    }
}