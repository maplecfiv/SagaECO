using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_EQUIP_START : Packet
    {
        public SSMG_PLAYER_EQUIP_START()
        {
            data = new byte[3];
            ID = 0x0263;
        }

        public uint Result
        {
            set => PutUInt(value, 2);
        }
    }
}