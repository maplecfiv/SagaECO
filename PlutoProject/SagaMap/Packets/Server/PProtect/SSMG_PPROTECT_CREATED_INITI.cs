using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PPROTECT_CREATED_INITI : Packet
    {
        public SSMG_PPROTECT_CREATED_INITI()
        {
            data = new byte[3];
            //this.offset = 2;
            ID = 0x235F;
        }
    }
}