using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_DEMIC_CONFIRM_RESULT : Packet
    {
        public enum Results
        {
            OK,
            FAILED = -1,
            TOO_MANY_ITEMS = -2,
            NOT_ENOUGH_EP = -3
        }

        public SSMG_DEM_DEMIC_CONFIRM_RESULT()
        {
            data = new byte[4];
            offset = 2;
            ID = 0x1E4F;
        }

        public byte Page
        {
            set => PutByte(value, 2);
        }

        public Results Result
        {
            set => PutByte((byte)value, 3);
        }
    }
}