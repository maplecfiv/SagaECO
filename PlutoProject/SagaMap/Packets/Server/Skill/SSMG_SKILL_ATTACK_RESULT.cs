using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Skill
{
    public class SSMG_SKILL_ATTACK_RESULT : Packet
    {
        public SSMG_SKILL_ATTACK_RESULT()
        {
            if (Configuration.Configuration.Instance.Version <= Version.Saga9)
            {
                data = new byte[29];
                offset = 2;
                ID = 0x0FA1;
            }

            if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
            {
                data = new byte[47];
                offset = 2;
                ID = 0x0FA1;
            }
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        public uint TargetID
        {
            set => PutUInt(value, 6);
        }

        public ATTACK_TYPE AttackType
        {
            set => PutByte((byte)value, 10);
        }

        public int HP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutShort((short)value, 11);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                {
                    PutInt(value, 11);
                    PutInt(value, 15);
                }
            }
        }

        public int MP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutShort((short)value, 13);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                {
                    PutInt(value, 19);
                    PutInt(value, 23);
                }
            }
        }

        public int SP
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutShort((short)value, 15);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                {
                    PutInt(value, 27);
                    PutInt(value, 31);
                }
            }
        }

        public AttackFlag AttackFlag
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutUInt((uint)value, 17);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                    PutUInt((uint)value, 35);
            }
        }

        public uint Delay
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutUInt(value, 21);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                    PutUInt(value, 39);
            }
        }

        public uint Unknown
        {
            set
            {
                if (Configuration.Configuration.Instance.Version <= Version.Saga9)
                    PutUInt(value, 25);
                if (Configuration.Configuration.Instance.Version >= Version.Saga9_2)
                    PutUInt(value, 43);
            }
        }
    }
}