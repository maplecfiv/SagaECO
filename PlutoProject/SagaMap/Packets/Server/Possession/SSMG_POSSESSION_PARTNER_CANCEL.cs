using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_POSSESSION_PARTNER_CANCEL : Packet
    {
        public SSMG_POSSESSION_PARTNER_CANCEL()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x17A5;
        }

        public PossessionPosition Pos
        {
            set => PutByte((byte)value, 2);
        }
    }
}