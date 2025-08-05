using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_INFINITECORRIDOR_WARP_SET : Packet
    {
        public SSMG_INFINITECORRIDOR_WARP_SET()
        {
            data = new byte[12];
            ID = 0x2292;
            offset = 2;
        }
    }
}