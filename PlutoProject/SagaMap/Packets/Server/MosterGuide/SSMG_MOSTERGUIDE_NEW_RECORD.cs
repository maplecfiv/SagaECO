using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_MOSTERGUIDE_NEW_RECORD : Packet
    {
        public SSMG_MOSTERGUIDE_NEW_RECORD()
        {
            data = new byte[6];
            ID = 0x2289;
            offset = 2;
        }

        public short guideID
        {
            set => PutShort(value, 4);
        }
    }
}