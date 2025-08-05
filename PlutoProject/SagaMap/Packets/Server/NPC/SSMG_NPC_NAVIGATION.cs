using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_NAVIGATION : Packet
    {
        public SSMG_NPC_NAVIGATION()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1A2C;
            Type = 255;
        }

        public byte X
        {
            set => PutByte(value, 2);
        }

        public byte Y
        {
            set => PutByte(value, 3);
        }

        public byte Type
        {
            set => PutByte(value, 4);
        }
    }
}