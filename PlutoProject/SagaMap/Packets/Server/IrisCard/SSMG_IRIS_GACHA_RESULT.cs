using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.IrisCard
{
    public class SSMG_IRIS_GACHA_RESULT : Packet
    {
        public SSMG_IRIS_GACHA_RESULT()
        {
            data = new byte[16];
            offset = 2;
            ID = 0x1DDA;
        }

        public List<uint> ItemIDs
        {
            set
            {
                data = new byte[19 + value.Count * 4];
                ID = 0x1DDA;
                PutByte((byte)value.Count, 18);
                for (var i = 0; i < value.Count; i++)
                    PutUInt(value[i], 19 + i * 4);
            }
        }
    }
}