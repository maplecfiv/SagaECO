using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTIVITY_RECYCLE_WINDOW : Packet
    {
        public SSMG_ACTIVITY_RECYCLE_WINDOW()
        {
            data = new byte[8];
            offset = 2;
            ID = 0x226B;
        }

        public ushort Percent
        {
            set => PutUShort(value, 2);
        }

        public uint PCount
        {
            set => PutUInt(value, 4);
        }
    }
}