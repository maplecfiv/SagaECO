using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ANO_EQUIP_RESULT : Packet
    {
        public SSMG_ANO_EQUIP_RESULT()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x23AB;
        }

        public ushort PaperID
        {
            set => PutUShort(value, 3);
        }
    }
}