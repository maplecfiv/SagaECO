using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_EQUIP_END : Packet
    {
        public SSMG_PLAYER_EQUIP_END()
        {
            data = new byte[2];
            ID = 0x0267;
        }
    }
}