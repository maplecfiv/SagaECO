using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_COST_LIMIT : Packet
    {
        public SSMG_DEM_COST_LIMIT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1E5A;
        }

        public short CurrentEP
        {
            set => PutShort(value, 2);
        }

        public short EPRequired
        {
            set => PutShort(value, 4);
        }
    }
}