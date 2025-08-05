using SagaLib;

namespace SagaMap.Packets.Server.Partner
{
    public class SSMG_PARTNER_HPMPSP_STATUS : Packet
    {
        public SSMG_PARTNER_HPMPSP_STATUS()
        {
            data = new byte[32];
            offset = 2;
            ID = 0x218E;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     usually 3
        /// </summary>
        public byte NPLength
        {
            set => PutByte(value, 6);
        }

        public uint HP
        {
            set => PutUInt(value, 7);
        }

        public uint MP
        {
            set => PutUInt(value, 11);
        }

        public uint SP
        {
            set => PutUInt(value, 15);
        }

        public byte NPLength2
        {
            set => PutByte(value, 19);
        }

        public uint MAXHP
        {
            set => PutUInt(value, 20);
        }

        public uint MAXMP
        {
            set => PutUInt(value, 24);
        }

        public uint MAXSP
        {
            set => PutUInt(value, 28);
        }
    }
}