using SagaLib;

namespace SagaMap.Packets.Server
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