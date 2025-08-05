using SagaLib;

namespace SagaMap.Packets.Server.Fish
{
    public class SSMG_FISHING_RESULT : Packet
    {
        public SSMG_FISHING_RESULT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2167;
            PutByte(1, 2);
            PutByte(1, 4);
            PutByte(1, 9);
        }

        public byte IsSucceed
        {
            set => PutByte(value, 3);
        }

        public uint ItemID
        {
            set => PutUInt(value, 5);
        }
    }
}