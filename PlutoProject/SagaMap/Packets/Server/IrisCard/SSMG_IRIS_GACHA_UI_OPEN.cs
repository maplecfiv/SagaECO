using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_IRIS_GACHA_UI_OPEN : Packet
    {
        public SSMG_IRIS_GACHA_UI_OPEN()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x1DD8;
        }
    }
}