using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_POSSESSION_PARTNER_RESULT : Packet
    {
        public SSMG_POSSESSION_PARTNER_RESULT()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x17A3;
        }

        public uint InventorySlot
        {
            set => PutUInt(value, 2);
        }

        public PossessionPosition Pos
        {
            set => PutByte((byte)value, 6);
        }

        public uint FromID
        {
            set => PutUInt(value, 6);
        }
    }
}