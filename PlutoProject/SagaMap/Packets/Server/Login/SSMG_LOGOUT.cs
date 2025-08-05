using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_LOGOUT : Packet
    {
        public enum Results
        {
            START,
            CANCEL = 0xf9
        }

        public SSMG_LOGOUT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x0020;
        }

        public Results Result
        {
            set => PutByte((byte)value, 2);
        }
    }
}