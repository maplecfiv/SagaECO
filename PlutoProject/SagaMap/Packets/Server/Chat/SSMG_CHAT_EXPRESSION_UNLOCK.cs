using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_CHAT_EXPRESSION_UNLOCK : Packet
    {
        public SSMG_CHAT_EXPRESSION_UNLOCK()
        {
            data = new byte[15];
            offset = 2;
            ID = 0x1D06;
            PutByte(3, 2);
        }

        public uint unlock
        {
            set => PutUInt(value, 3);
        }
    }
}