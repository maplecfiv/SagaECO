using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FG_WARE_SENDCOUNT : Packet
    {
        public SSMG_FG_WARE_SENDCOUNT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1c20;
        }

        public int CurrentCount
        {
            set => PutInt(value);
        }
    }
}