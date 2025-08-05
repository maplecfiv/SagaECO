using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_MOSTERGUIDE_RECORDS : Packet
    {
        public SSMG_MOSTERGUIDE_RECORDS()
        {
            data = new byte[3];
            ID = 0x2288;
            offset = 2;
        }

        public List<BitMask> Records
        {
            set
            {
                var buf = new byte[data.Length + value.Count * 4];
                data.CopyTo(buf, 0);
                data = buf;
                PutByte((byte)value.Count, 2);
                foreach (var mask in value) PutInt(mask.Value);
            }
        }
    }
}