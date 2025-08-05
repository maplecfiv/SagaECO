using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_IRIS_ADD_SLOT_RESULT : Packet
    {
        public enum Results
        {
            OK = 1,
            Failed = 0,
            NOT_ENOUGH_GOLD = -1,
            NO_ITEM = 254,
            NO_MATERIAL = 253,
            NO_RIGHT_MATERIAL = -4
        }

        public SSMG_IRIS_ADD_SLOT_RESULT()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x13E4;
        }

        public Results Result
        {
            set
            {
                PutByte((byte)value, 2);
                PutByte(0x00);
                PutByte(0x64);
            }
        }
    }
}