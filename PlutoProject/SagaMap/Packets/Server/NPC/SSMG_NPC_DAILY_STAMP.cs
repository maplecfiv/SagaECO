using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_DAILY_STAMP : Packet
    {
        public SSMG_NPC_DAILY_STAMP()
        {
            data = new byte[9]; //470以后为9  1F 75 00 00 00 00 00 0A 02
            offset = 2;
            ID = 0x1F75;
        }

        /*public uint StampCount
        {
            set
            {
                this.PutUInt(value, 2);
            }
        }*/

        public byte StampCount
        {
            set => PutByte(value, 7);
        }

        public byte Type
        {
            set => PutByte(value, 8); //470以后偏移 10
        }

        public byte Balance
        {
            set
            {
                //this.PutByte(value, 11);
            }
        }
    }
}