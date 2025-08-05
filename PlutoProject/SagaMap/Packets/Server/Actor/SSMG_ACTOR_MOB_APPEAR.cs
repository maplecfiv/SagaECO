using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_MOB_APPEAR : Packet
    {
        public SSMG_ACTOR_MOB_APPEAR()
        {
            data = new byte[31];
            offset = 2;
            ID = 0x1220;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint MobID
        {
            set => PutUInt(value, 6);
        }

        public byte X
        {
            set => PutByte(value, 10);
        }

        public byte Y
        {
            set => PutByte(value, 11);
        }

        public ushort Speed
        {
            set => PutUShort(value, 12);
        }

        public byte Dir
        {
            set => PutByte(value, 14);
        }

        public uint HP
        {
            set
            {
                PutUInt(value, 15);
                PutUInt(value, 23);
            }
        }

        public uint MaxHP
        {
            set
            {
                PutUInt(value, 19);
                PutUInt(value, 27);
            }
        }
    }
}