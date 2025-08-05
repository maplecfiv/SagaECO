using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_INITI : Packet
    {
        public SSMG_PPROTECT_INITI()
        {
            data = new byte[2];
            //this.offset = 2;
            ID = 0x235A;
        }
    }
}