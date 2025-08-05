using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_FOOTER : Packet
    {
        public SSMG_PLAYER_SHOP_FOOTER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x1919;
        }
    }
}