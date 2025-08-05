using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_ANOTHER_MOB_APPEAR : Packet
    {
        public SSMG_ACTOR_ANOTHER_MOB_APPEAR()
        {
            data = new byte[35];
            offset = 2;
            ID = 0x2328;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint MobID
        {
            set => PutUInt(value, 6);
        }

        public uint Camp
        {
            set => PutUInt(value, 10);
        }

        public byte X
        {
            set => PutByte(value, 14);
        }

        public byte Y
        {
            set => PutByte(value, 15);
        }

        public ushort Speed
        {
            set => PutUShort(value, 16);
        }

        public byte Dir
        {
            set => PutByte(value, 18);
        }

        public uint HP
        {
            set =>
                //this.PutUInt(value, 19);
                PutUInt(value, 23);
        }

        public uint MaxHP
        {
            set =>
                //this.PutUInt(value, 19);
                PutUInt(value, 31);
        }
    }
}