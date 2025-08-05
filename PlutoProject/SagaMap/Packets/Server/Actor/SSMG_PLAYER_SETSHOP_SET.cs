using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_SETSHOP_SET : Packet
    {
        public SSMG_PLAYER_SETSHOP_SET()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x181E;
        }
    }
}