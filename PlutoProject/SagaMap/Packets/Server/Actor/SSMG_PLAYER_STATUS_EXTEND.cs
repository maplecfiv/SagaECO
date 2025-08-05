using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_STATUS_EXTEND : Packet
    {
        public SSMG_PLAYER_STATUS_EXTEND()
        {
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                data = new byte[41];
            else
                data = new byte[63];
            offset = 2;
            ID = 0x0217;
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                PutByte(0x13, 2);
            else
                PutByte(0x1E, 2);
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
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 33);
            }
        }

        public ushort HitCritical
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 35);
            }
        }

        public ushort AvoidMelee
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 37);
                else
                    PutUShort(value, 33);
            }
        }

        public ushort AvoidRanged
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 39);
                else
                    PutUShort(value, 35);
            }
        }

        public ushort AvoidMagic
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 41);
            }
        }

        public ushort AvoidCritical
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 43);
            }
        }

        public ushort HealHP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 45);
            }
        }

        public ushort HealMP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 47);
            }
        }

        public ushort HealSP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutUShort(value, 49);
            }
        }

        public short ASPD
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutShort(value, 51);
                else
                    PutShort(value, 37);
            }
        }

        public short CSPD
        {
            set
            {
                if (Configuration.Configuration.Instance.Version < Version.Saga17)
                    PutShort(value, 53);
                else
                    PutShort(value, 39);
            }
        }
    }
}