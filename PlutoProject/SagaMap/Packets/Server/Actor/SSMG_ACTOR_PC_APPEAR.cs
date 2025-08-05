using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_PC_APPEAR : Packet
    {
        public SSMG_ACTOR_PC_APPEAR()
        {
            data = new byte[24];
            offset = 2;
            ID = 0x120C;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte X
        {
            set => PutByte(value, 6);
        }

        public byte Y
        {
            set => PutByte(value, 7);
        }

        public ushort Speed
        {
            set => PutUShort(value, 8);
        }

        public byte Dir
        {
            set => PutByte(value, 10);
        }

        public uint PossessionActorID
        {
            set => PutUInt(value, 11);
        }

        public PossessionPosition PossessionPosition
        {
            set => PutByte((byte)value, 15);
        }

        public uint HP
        {
            set => PutUInt(value, 16);
        }

        public uint MaxHP
        {
            set => PutUInt(value, 20);
        }
    }
}