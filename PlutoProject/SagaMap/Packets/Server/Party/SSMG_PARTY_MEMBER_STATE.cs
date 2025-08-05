using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_MEMBER_STATE : Packet
    {
        public SSMG_PARTY_MEMBER_STATE()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x19E6;
        }

        public uint PartyIndex
        {
            set => PutUInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public bool Online
        {
            set
            {
                if (value)
                    PutUInt(1, 10);
                else
                    PutUInt(0, 10);
            }
        }
    }
}