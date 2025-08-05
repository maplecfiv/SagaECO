using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_ADD_SLOT_MATERIAL : Packet
    {
        public SSMG_IRIS_ADD_SLOT_MATERIAL()
        {
            data = new byte[11];
            offset = 2;
            ID = 0x13E4;
        }

        public byte Slot
        {
            set => PutByte(value, 2);
        }

        public uint Material
        {
            set => PutUInt(value, 3);
        }

        public int Gold
        {
            set => PutInt(value, 7);
        }
    }
}