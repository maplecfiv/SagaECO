using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ABYSSTEAM_REPLY : Packet
    {
        public SSMG_ABYSSTEAM_REPLY()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x22F0;
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }
    }
}