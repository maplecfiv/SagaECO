using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SHOP_HEADER : Packet
    {
        public SSMG_PLAYER_SHOP_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1914;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}