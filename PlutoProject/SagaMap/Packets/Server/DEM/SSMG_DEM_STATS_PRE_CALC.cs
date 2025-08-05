using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_STATS_PRE_CALC : Packet
    {
        public SSMG_DEM_STATS_PRE_CALC()
        {
            data = new byte[74];
            offset = 2;
            ID = 0x1E51;

            PutByte(0x1E, 2);
            PutByte(3, 63);
        }

        public ushort Speed
        {
            set => PutUShort(value, 3);
        }

        public ushort ATK1Min
        {
            set => PutUShort(value, 5);
        }

        public ushort ATK2Min
        {
            set => PutUShort(value, 7);
        }

        public ushort ATK3Min
        {
            set => PutUShort(value, 9);
        }

        public ushort ATK1Max
        {
            set => PutUShort(value, 11);
        }

        public ushort ATK2Max
        {
            set => PutUShort(value, 13);
        }

        public ushort ATK3Max
        {
            set => PutUShort(value, 15);
        }

        public ushort MATKMin
        {
            set => PutUShort(value, 17);
        }

        public ushort MATKMax
        {
            set => PutUShort(value, 19);
        }

        public ushort DefBase
        {
            set => PutUShort(value, 21);
        }

        public ushort DefAddition
        {
            set => PutUShort(value, 23);
        }

        public ushort MDefBase
        {
            set => PutUShort(value, 25);
        }

        public ushort MDefAddition
        {
            set => PutUShort(value, 27);
        }

        public ushort HitMelee
        {
            set => PutUShort(value, 29);
        }

        public ushort HitRanged
        {
            set => PutUShort(value, 31);
        }

        public ushort HitMagic
        {
            set => PutUShort(value, 33);
        }

        public ushort HitCritical
        {
            set => PutUShort(value, 35);
        }

        public ushort AvoidMelee
        {
            set => PutUShort(value, 37);
        }

        public ushort AvoidRanged
        {
            set => PutUShort(value, 39);
        }

        public ushort AvoidMagic
        {
            set => PutUShort(value, 41);
        }

        public ushort AvoidCritical
        {
            set => PutUShort(value, 43);
        }

        public ushort HealHP
        {
            set => PutUShort(value, 45);
        }

        public ushort HealMP
        {
            set => PutUShort(value, 47);
        }

        public ushort HealSP
        {
            set => PutUShort(value, 49);
        }

        public short ASPD
        {
            set => PutShort(value, 51);
        }

        public short CSPD
        {
            set => PutShort(value, 53);
        }

        public ushort HP
        {
            set => PutUShort(value, 64);
        }

        public ushort MP
        {
            set => PutUShort(value, 66);
        }

        public ushort SP
        {
            set => PutUShort(value, 68);
        }

        public ushort Capacity
        {
            set => PutUShort(value, 70);
        }

        public ushort Payload
        {
            set => PutUShort(value, 72);
        }
    }
}