using System;
using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_NEXTFEE_DATE : Packet
    {
        //下次自动扣费日期 (次回引き落とし日時)  
        public SSMG_FF_NEXTFEE_DATE()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x2023;
        }

        public DateTime UpdateTime
        {
            set
            {
                var date = (uint)(value - new DateTime(1970, 1, 1)).TotalSeconds;
                PutUInt(date, 2);
            }
        }
    }
}