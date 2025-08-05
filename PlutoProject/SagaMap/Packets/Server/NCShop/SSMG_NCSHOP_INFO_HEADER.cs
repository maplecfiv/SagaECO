using SagaLib;

namespace SagaMap.Packets.Server.NCShop
{
    public class SSMG_NCSHOP_INFO_HEADER : Packet
    {
        public SSMG_NCSHOP_INFO_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x062F;
        }

        public uint Page
        {
            set => PutUInt(value, 2);
        }
    }
}