using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_TAMAIRE_LIST_UI : Packet
    {
        public SSMG_TAMAIRE_LIST_UI()
        {
            data = new byte[2];
            offset = 2;
            ID = 0x22B0;
        }
    }
}