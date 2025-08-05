using SagaLib;

namespace SagaMap.Packets.Server.PProtect
{
    public class SSMG_PPROTECT_CREATED_RESULT : Packet
    {
        public SSMG_PPROTECT_CREATED_RESULT()
        {
            data = new byte[3];
            //this.offset = 2;
            ID = 0x2362;
        }
    }
}