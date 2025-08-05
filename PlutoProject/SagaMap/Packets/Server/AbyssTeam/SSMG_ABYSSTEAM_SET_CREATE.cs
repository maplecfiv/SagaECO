using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ABYSSTEAM_SET_CREATE : Packet
    {
        public SSMG_ABYSSTEAM_SET_CREATE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x22EA;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }
    }
}