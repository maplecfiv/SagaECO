using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_STATUS : Packet
    {
        public SSMG_PLAYER_STATUS()
        {
            data = new byte[53];
            offset = 2;
            ID = 0x0212;
        }

        public ushort StrBase
        {
            set
            {
                PutByte(8, 2);
                PutByte(8, 19);
                PutByte(8, 36);
                PutUShort(value, 3);
            }
        }

        public ushort DexBase
        {
            set => PutUShort(value, 5);
        }

        public ushort IntBase
        {
            set => PutUShort(value, 7);
        }

        public ushort VitBase
        {
            set => PutUShort(value, 9);
        }

        public ushort AgiBase
        {
            set => PutUShort(value, 11);
        }

        public ushort MagBase
        {
            set => PutUShort(value, 13);
        }

        public ushort LukBase
        {
            set => PutUShort(value, 15);
        }

        public ushort ChaBase
        {
            set => PutUShort(value, 17);
        }

        public short StrRevide
        {
            set => PutShort(value, 20);
        }

        public short DexRevide
        {
            set => PutShort(value, 22);
        }

        public short IntRevide
        {
            set => PutShort(value, 24);
        }

        public short VitRevide
        {
            set => PutShort(value, 26);
        }

        public short AgiRevide
        {
            set => PutShort(value, 28);
        }

        public short MagRevide
        {
            set => PutShort(value, 30);
        }

        public short LukRevide
        {
            set => PutShort(value, 32);
        }

        public short ChaRevide
        {
            set => PutShort(value, 34);
        }

        public ushort StrBonus
        {
            set => PutUShort(value, 37);
        }

        public ushort DexBonus
        {
            set => PutUShort(value, 39);
        }

        public ushort IntBonus
        {
            set => PutUShort(value, 41);
        }

        public ushort VitBonus
        {
            set => PutUShort(value, 43);
        }

        public ushort AgiBonus
        {
            set => PutUShort(value, 45);
        }

        public ushort MagBonus
        {
            set => PutUShort(value, 47);
        }

        public ushort LukBonus
        {
            set => PutUShort(value, 49);
        }

        public ushort ChaBonus
        {
            set => PutUShort(value, 51);
        }
    }
}