using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_OPEN_REBIRTHREWARD_WINDOW : Packet
    {
        public SSMG_PLAYER_OPEN_REBIRTHREWARD_WINDOW()
        {
            data = new byte[0x0e];
            ID = 0x1edd;
            offset = 2;
        }

        public byte SetOpen
        {
            set => PutByte(value, 3);
        }
    }
}