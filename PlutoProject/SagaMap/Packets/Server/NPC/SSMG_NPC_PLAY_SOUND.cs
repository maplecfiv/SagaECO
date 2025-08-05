using SagaLib;

namespace SagaMap.Packets.Server.NPC
{
    public class SSMG_NPC_PLAY_SOUND : Packet
    {
        public SSMG_NPC_PLAY_SOUND()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x05F2;
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
            set => PutByte(value, 12);
        }
    }
}