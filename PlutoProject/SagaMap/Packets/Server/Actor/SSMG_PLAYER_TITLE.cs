using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_TITLE : Packet
    {
        public SSMG_PLAYER_TITLE()
        {
            data = new byte[20]; //8bytes unknowns
            offset = 2;
            ID = 0x2419;
            PutByte(4, 3);
        }

        public byte Result
        {
            set => PutByte(value, 2);
        }

        public void PutTitles(List<uint> titles)
        {
            var buf = new byte[data.Length + titles.Count * 4 + 1];
            data.CopyTo(buf, 0);
            data = buf;

            offset = 3;
            PutByte((byte)titles.Count, 3);
            offset++;
            foreach (var title in titles)
            {
                PutUInt(title, offset);
                offset += 4;
            }
        }
    }
}