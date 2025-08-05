using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_MEMBER_POSITION : Packet
    {
        public SSMG_PARTY_MEMBER_POSITION()
        {
            data = new byte[22];
            offset = 2;
            ID = 0x19F0;
        }

        public byte PartyIndex
        {
            set => PutUInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public uint MapID
        {
            set => PutUInt(value, 10);
        }

        public byte X
        {
            set => PutUInt(value, 14);
        }

        public byte Y
        {
            set => PutUInt(value, 18);
        }
    }
}