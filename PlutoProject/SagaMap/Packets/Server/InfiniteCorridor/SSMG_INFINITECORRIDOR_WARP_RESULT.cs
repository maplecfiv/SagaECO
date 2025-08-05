using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_INFINITECORRIDOR_WARP_RESULT : Packet
    {
        public SSMG_INFINITECORRIDOR_WARP_RESULT()
        {
            data = new byte[4];
            ID = 0x2295;
            offset = 2;
        }
    }
}