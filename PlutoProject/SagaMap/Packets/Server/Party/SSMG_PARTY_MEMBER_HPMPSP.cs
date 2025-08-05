using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_MEMBER_HPMPSP : Packet
    {
        public SSMG_PARTY_MEMBER_HPMPSP()
        {
            data = new byte[34];
            offset = 2;
            ID = 0x19EB;
        }

        public byte PartyIndex
        {
            set => PutUInt(value, 2);
        }

        public uint CharID
        {
            set => PutUInt(value, 6);
        }

        public uint HP
        {
            set => PutUInt(value, 10);
        }

        public uint MaxHP
        {
            set => PutUInt(value, 14);
        }

        public uint MP
        {
            set => PutUInt(value, 18);
        }

        public uint MaxMP
        {
            set => PutUInt(value, 22);
        }

        public uint SP
        {
            set => PutUInt(value, 26);
        }

        public uint MaxSP
        {
            set => PutUInt(value, 30);
        }
    }
}