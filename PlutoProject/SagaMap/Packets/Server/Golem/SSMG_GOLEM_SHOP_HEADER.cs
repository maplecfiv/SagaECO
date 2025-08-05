using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_HEADER : Packet
    {
        public SSMG_GOLEM_SHOP_HEADER()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1800;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}