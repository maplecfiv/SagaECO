using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_ANSWER : Packet
    {
        public SSMG_PLAYER_SHOP_ANSWER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x191B;
        }

        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}