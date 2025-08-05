using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_CHANGE_BGM : Packet
    {
        public SSMG_NPC_CHANGE_BGM()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x05EC;
        }

        public uint SoundID
        {
            set => PutUInt(value, 2);
        }

        public byte Loop
        {
            set => PutByte(value, 6);
        }

        public uint Volume
        {
            set => PutUInt(value, 8);
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