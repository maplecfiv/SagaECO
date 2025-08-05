using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_MEMBER_DETAIL : Packet
    {
        public SSMG_PARTY_MEMBER_DETAIL()
        {
            data = new byte[23];
            offset = 2;
            ID = 0x19F5;
        }

        public uint PartyIndex
        {
            set => PutUInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public byte Form
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    PutByte(value, 10);
            }
        }

        public PC_JOB Job
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    PutUInt((uint)value, 11);
                else
                    PutUInt((uint)value, 10);
            }
        }

        public byte Level
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    PutUInt(value, 15);
                else
                    PutUInt(value, 14);
            }
        }

        public byte JobLevel
        {
            set
            {
                if (Configuration.Configuration.Instance.Version >= Version.Saga10)
                    PutUInt(value, 19);
                else
                    PutUInt(value, 18);
            }
        }
    }
}