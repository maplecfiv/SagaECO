using SagaLib;

namespace SagaMap.Packets.Server.DEM
{
    public class SSMG_DEM_COST_LIMIT_UPDATE : Packet
    {
        public enum Results
        {
            OK = 0,
            FAILED = -1,
            NOT_ENOUGH_EP = -2,
            CL_MAXIMUM = -3,
            LV_MAXIMUM = -4
        }

        public SSMG_DEM_COST_LIMIT_UPDATE()
        {
            data = new byte[9];
            offset = 2;
            ID = 0x1E5D;
        }

        public Results Result
        {
            set => PutByte((byte)value, 2);
        }

        public short CurrentEP
        {
            set => PutShort(value, 3);
        }

        public short EPRequired
        {
            set => PutShort(value, 5);
        }

        public short CL
        {
            set => PutShort(value, 7);
        }
    }
}