using SagaLib;

namespace SagaValidation.Packets.Server
{
    public class SSMG_UNKNOWN_RETURN : Packet
    {
        public SSMG_UNKNOWN_RETURN()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x0030;
        }
    }
}

