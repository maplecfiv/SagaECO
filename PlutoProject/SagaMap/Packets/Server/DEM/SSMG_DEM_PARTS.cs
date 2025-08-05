using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_PARTS : Packet
    {
        public SSMG_DEM_PARTS()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1E82;
        }
    }
}