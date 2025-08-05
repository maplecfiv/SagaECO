using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_SHOW_HAIR_PREVIEW : Packet
    {
        public SSMG_NPC_SHOW_HAIR_PREVIEW()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0616;
        }

        public byte type
        {
            set => PutByte(value, 5);
        }
    }
}