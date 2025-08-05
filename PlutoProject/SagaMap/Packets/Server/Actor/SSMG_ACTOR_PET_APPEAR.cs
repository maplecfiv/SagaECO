using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_ACTOR_PET_APPEAR : Packet
    {
        public SSMG_ACTOR_PET_APPEAR()
        {
            if (Configuration.Configuration.Instance.Version == Version.Saga6)
                data = new byte[30];
            if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                data = new byte[36];
            if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                data = new byte[42];
            offset = 2;
            ID = 0x122F;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Union
        {
            set => PutByte(value, 6);
        }

        public uint OwnerActorID
        {
            set => PutUInt(value, 7);
        }

        public uint OwnerCharID
        {
            set => PutUInt(value, 11);
        }

        public byte OwnerLevel
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga9) PutByte(value, 15);
            }
        }

        public uint OwnerWRP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga9) PutUInt(value, 16);
            }
        }


        public byte X
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutByte(value, 15);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutByte(value, 21);
            }
        }

        public byte Y
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutByte(value, 16);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutByte(value, 22);
            }
        }

        public ushort Speed
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutUShort(value, 17);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutUShort(value, 23);
            }
        }

        public byte Dir
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutByte(value, 19);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutByte(value, 25);
            }
        }

        public uint HP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutUInt(value, 20);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutUInt(value, 26);
                if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                {
                    PutUInt(value, 26); ///0???
                    PutUInt(value, 30);
                }
            }
        }

        public uint MaxHP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version == Version.Saga6)
                    PutUInt(value, 24);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9)
                    PutUInt(value, 30);
                if (Configuration.Configuration.Instance.Version >= Version.Saga17)
                {
                    PutUInt(value, 34); //0???
                    PutUInt(value, 38);
                }
            }
        }
    }
}