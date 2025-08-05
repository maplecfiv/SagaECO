using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_DEMIC_FOOTER : Packet
    {
        public SSMG_DEM_DEMIC_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1E4B;
        }
    }
}