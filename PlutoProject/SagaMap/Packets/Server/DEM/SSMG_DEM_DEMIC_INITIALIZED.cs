using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_DEMIC_INITIALIZED : Packet
    {
        public enum Results
        {
            OK,
            FAILED = -1,
            TOO_MANY_ITEMS = -2,
            NOT_ENOUGH_EP = -3
        }

        public SSMG_DEM_DEMIC_INITIALIZED()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1E4D;
        }

        public byte Page
        {
            set => PutByte(value, 2);
        }

        public Results Result
        {
            set => PutByte((byte)value, 3);
        }

        public byte EngageTask
        {
            set
            {
                var task = value;
                if (task == 255)
                    task = 0;
                else
                    task++;
                PutByte(task, 4);
            }
        }

        public byte EngageTask2
        {
            set
            {
                var task = value;
                if (task == 255)
                    task = 0;
                else
                    task++;
                PutByte(task, 5);
            }
        }
    }
}