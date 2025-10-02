using SagaLib;

namespace SagaValidation.Packets.Server
{
    public class SSMG_SERVER_LST_STAER : Packet
    {
        public SSMG_SERVER_LST_STAER()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x32;
        }
    }
}
