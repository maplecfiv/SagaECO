using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_CHIP_SHOP_HEADER : Packet
    {
        public SSMG_DEM_CHIP_SHOP_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0639;
        }

        public uint CategoryID
        {
            set => PutUInt(value, 2);
        }
    }
}