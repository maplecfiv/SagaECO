using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOP_APPEAR_SEND : Packet
    {
        public SSMG_PLAYER_SHOP_APPEAR_SEND()
        {
            data = new byte[6]; //TitleBytes.Length+2+4+5
            offset = 2;
            ID = 0x1903;
        }

        public uint ActorID
        {
            set => PutUInt(value, 2);
        }
    }
}