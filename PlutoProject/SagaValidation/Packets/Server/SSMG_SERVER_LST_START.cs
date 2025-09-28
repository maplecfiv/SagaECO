using SagaLib;

namespace SagaValidation.Packets.Server
{
    public class SSMG_SERVER_LST_STAER : Packet
    {
        public SSMG_SERVER_LST_STAER()
        {
            this.data = new byte[2];
            this.offset = 2;
            this.ID = 0x32;
        }
    }
}
