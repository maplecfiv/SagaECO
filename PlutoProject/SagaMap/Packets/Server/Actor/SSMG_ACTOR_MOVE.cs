using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_MOVE : Packet
    {
        public SSMG_ACTOR_MOVE()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x11F9;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public short X
        {
            set => PutShort(value, 6);
        }

        public short Y
        {
            set => PutShort(value, 8);
        }

        public ushort Dir
        {
            set => PutUShort(value, 10);
        }

        public MoveType MoveType
        {
            set => PutUShort((ushort)value, 12);
        }
    }
}