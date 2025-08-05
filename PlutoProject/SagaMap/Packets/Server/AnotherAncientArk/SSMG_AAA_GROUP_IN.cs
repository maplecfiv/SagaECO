using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_IN : Packet
    {
        public SSMG_AAA_GROUP_IN()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x23E4;
        }

        public int GroupID
        {
            set { }
        }

        public byte Position
        {
            set { }
        }

        public string Name
        {
            set { }
        }

        public int CharID
        {
            set { }
        }
    }
}