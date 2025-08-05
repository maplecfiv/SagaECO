using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_MAX_HPMPSP : Packet
    {
        public SSMG_PLAYER_MAX_HPMPSP()
        {
            data = new byte[35];
            offset = 2;
            ID = 0x0221;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint MaxHP
        {
            set
            {
                PutByte(3, 6);
                PutUInt(0, 7);
                PutUInt(value, 11);
            }
        }

        public uint MaxMP
        {
            set
            {
                PutUInt(0, 15);
                PutUInt(value, 19);
            }
        }

        public uint MaxSP
        {
            set
            {
                PutUInt(0, 23);
                PutUInt(value, 27);
            }
        }

        public uint MaxEP
        {
            set => PutUInt(value, 31);
        }
    }
}