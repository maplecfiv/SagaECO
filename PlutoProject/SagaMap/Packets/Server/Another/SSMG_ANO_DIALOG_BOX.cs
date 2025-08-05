using SagaLib;

namespace SagaMap.Packets.Server.Another
{
    public class SSMG_ANO_BUTTON_APPEAR : Packet
    {
        public SSMG_ANO_BUTTON_APPEAR()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x23A0;
        }

        public byte Type
        {
            set => PutByte(value, 2);
        }
    }
}