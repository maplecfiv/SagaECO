using SagaLib;

namespace SagaMap.Packets.Server.FGarden
{
    public enum FG_Sky
    {
        Default,
        Evening,
        Night,
        Cosmos
    }

    public class SSMG_FG_CHANGE_SKY : Packet
    {
        public SSMG_FG_CHANGE_SKY()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13BD;
        }

        public byte Sky
        {
            set => PutByte(value, 2);
        }
    }
}