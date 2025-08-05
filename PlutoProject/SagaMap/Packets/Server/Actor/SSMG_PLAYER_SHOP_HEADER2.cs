using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SHOP_HEADER2 : Packet
    {
        public SSMG_PLAYER_SHOP_HEADER2()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1917;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}