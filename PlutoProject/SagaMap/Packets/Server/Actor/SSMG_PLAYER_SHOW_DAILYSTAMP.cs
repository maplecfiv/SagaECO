using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SHOW_DAILYSTAMP : Packet
    {
        public SSMG_PLAYER_SHOW_DAILYSTAMP()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x1F72;
            //this.PutByte(1, 2);
        }

        public byte Type
        {
            set => PutByte(value, 2);
        }
    }
}