using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_NPC_SHOW_EFFECT : Packet
    {
        public SSMG_NPC_SHOW_EFFECT()
        {
            data = new byte[25];
            offset = 2;
            ID = 0x0600;
            PutByte(0xff, 10);
            PutByte(0xff, 11);
            PutUInt(0xffffffff, 12);
            PutByte(0xff, 18);
            PutUInt(0xffffffff, 19);
            PutByte(0xff, 23);
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint EffectID
        {
            set => PutUInt(value, 6);
        }

        public byte X
        {
            set => PutByte(value, 14);
        }

        public byte Y
        {
            set => PutByte(value, 15);
        }

        public ushort height
        {
            set => PutUShort(value, 16);
        }

        public bool OneTime
        {
            set
            {
                if (value)
                    PutByte(1, 24);
                else
                    PutByte(0, 24);
            }
        }
    }
}