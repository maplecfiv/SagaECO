using SagaLib;

namespace SagaMap.Packets.Server.VShop
{
    public class SSMG_VSHOP_INFO_HEADER : Packet
    {
        public SSMG_VSHOP_INFO_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x064F;
        }

        public uint Page
        {
            set => PutUInt(value, 2);
        }
    }
}