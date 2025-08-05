using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_CLOSE : Packet
    {
        public SSMG_PLAYER_SHOP_CLOSE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1916;
        }

        public int Reason
        {
            set => PutInt(value, 2);
        }
    }
}