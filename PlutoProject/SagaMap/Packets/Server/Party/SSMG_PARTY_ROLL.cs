using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_ROLL : Packet
    {
        public SSMG_PARTY_ROLL()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x1A00;
        }

        public byte status
        {
            set => PutByte(value, 6);
        }
    }
}