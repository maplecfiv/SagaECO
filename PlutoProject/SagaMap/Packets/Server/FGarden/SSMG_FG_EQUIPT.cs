using SagaDB.FGarden;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_EQUIPT : Packet
    {
        public SSMG_FG_EQUIPT()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x1BF9;
        }

        public uint ItemID
        {
            set => PutUInt(value, 2);
        }

        public FGardenSlot Place
        {
            set => PutUInt((uint)value, 6);
        }

        public byte Color
        {
            set => PutByte(value, 10);
        }
    }
}