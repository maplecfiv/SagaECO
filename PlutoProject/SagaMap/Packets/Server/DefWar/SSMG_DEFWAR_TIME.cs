using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEFWAR_TIME : Packet
    {
        public SSMG_DEFWAR_TIME()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1BCD;
        }


        public uint Time
        {
            set => PutUInt(value, 2);
        }
    }
}