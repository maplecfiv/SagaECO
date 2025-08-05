using SagaLib;

namespace SagaMap.Packets.Server.Another
{
    public class SSMG_ANO_TAKEOFF_RESULT : Packet
    {
        public SSMG_ANO_TAKEOFF_RESULT()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x23AD;
        }

        public ushort PaperID
        {
            set => PutUShort(value, 3);
        }
    }
}