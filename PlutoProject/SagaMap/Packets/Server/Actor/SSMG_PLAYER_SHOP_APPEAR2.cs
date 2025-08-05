using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_APPEAR2 : Packet
    {
        public SSMG_PLAYER_SHOP_APPEAR2()
        {
            data = new byte[6]; //TitleBytes.Length+2+4+5
            offset = 2;
            ID = 0x190E;
        }
    }
}