using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_AAA_GROUP_RESTART : Packet
    {
        public SSMG_AAA_GROUP_RESTART()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1FB4;
        }

        public ushort DID
        {
            set => PutUShort(value, 2);
        }
    }
}