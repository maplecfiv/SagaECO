using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SHOP_APPEAR_ONLINE2 : Packet
    {
        public SSMG_PLAYER_SHOP_APPEAR_ONLINE2()
        {
            data = new byte[2]; //TitleBytes.Length+2+4+5
            offset = 2;
            ID = 0x0033;
        }
    }
}