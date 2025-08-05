using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ACTOR_LEVEL_UP : Packet
    {
        public SSMG_ACTOR_LEVEL_UP()
        {
            if (Configuration.Instance.Version >= Version.Saga11)
                data = new byte[43];
            else if (Configuration.Instance.Version >= Version.Saga10)
                data = new byte[41];
            else
                data = new byte[25];
            offset = 2;
            ID = 0x023F;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public byte Level
        {
            set => PutByte(value, 6);
        }

        public byte JobLevel
        {
            set => PutByte(value, 7);
        }

        public int ExpPerc
        {
            set => PutInt(value, 8);
        }

        public int JExpPerc
        {
            set => PutInt(value, 12);
        }

        public long Exp
        {
            set => PutLong(value, 16);
        }

        public long JExp
        {
            set => PutLong(value, 24);
        }

        public short StatusPoints
        {
            set => PutShort(value, 32);
        }

        public short SkillPoints
        {
            set => PutShort(value, 34);
        }

        public short SkillPoints2X
        {
            set => PutShort(value, 36);
        }

        public short SkillPoints2T
        {
            set => PutShort(value, 38);
        }

        public short UnknownSkillPoints
        {
            set => PutShort(value, 40);
        }

        public byte LvType
        {
            set
            {
                if (Configuration.Instance.Version >= Version.Saga11)
                    PutByte(value, 42);
                else if (Configuration.Instance.Version >= Version.Saga10)
                    PutByte(value, 40);
                else
                    PutByte(value, 24);
            }
        }
    }
}