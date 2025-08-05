using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_CHIP_SHOP_FOOTER : Packet
    {
        public SSMG_DEM_CHIP_SHOP_FOOTER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x063B;
        }
    }
}