using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SET_TYPE : Packet
    {
        public SSMG_GOLEM_SET_TYPE()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x17DF;
        }

        public GolemType GolemType
        {
            set => PutUShort((ushort)value, 2);
        }
    }
}