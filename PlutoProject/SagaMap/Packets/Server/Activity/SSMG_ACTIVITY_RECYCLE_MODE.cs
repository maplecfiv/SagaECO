using System;
using SagaLib;

namespace SagaMap.Packets.Server.Activity
{
    public class SSMG_ACTIVITY_RECYCLE_MODE : Packet
    {
        public SSMG_ACTIVITY_RECYCLE_MODE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2260;
        }

        public DateTime EndTime
        {
            set
            {
                var date = (uint)(value - new DateTime(1970, 1, 1)).TotalSeconds;
                PutUInt(date, 2);
            }
        }

        public uint Result
        {
            set => PutUInt(value, 6);
        }
    }
}