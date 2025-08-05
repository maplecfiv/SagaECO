using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_TITLE_LIST : Packet
    {
        public SSMG_PLAYER_TITLE_LIST()
        {
            data = new byte[214]; //8bytes unknowns
            offset = 2;
            ID = 0x2549;
        }

        public void PutUnknown1(List<ulong> unknown1)
        {
            offset = (ushort)data.Length;
            var buf = new byte[data.Length + unknown1.Count * 8 + 1];
            data.CopyTo(buf, 0);
            data = buf;

            PutByte((byte)unknown1.Count, offset);
            offset++;
            foreach (var unknown in unknown1)
            {
                PutULong(unknown, offset);
                offset += 8;
            }
        }

        public void PutUnknown2(List<ulong> unknown2)
        {
            offset = (ushort)data.Length;
            var buf = new byte[data.Length + unknown2.Count * 8 + 1];
            data.CopyTo(buf, 0);
            data = buf;

            PutByte((byte)unknown2.Count, offset);
            offset++;
            foreach (var unknown in unknown2)
            {
                PutULong(unknown, offset);
                offset += 8;
            }
        }

        public void PutTitles(List<ulong> titles)
        {
            offset = (ushort)data.Length;
            var buf = new byte[data.Length + titles.Count * 8 + 1];
            data.CopyTo(buf, 0);
            data = buf;

            PutByte((byte)titles.Count, offset);
            offset++;
            foreach (var unknown in titles)
            {
                PutULong(unknown, offset);
                offset += 8;
            }
        }

        public void PutNewTitles(List<ulong> newtitles)
        {
            offset = (ushort)data.Length;
            var buf = new byte[data.Length + newtitles.Count * 8 + 1];
            data.CopyTo(buf, 0);
            data = buf;

            PutByte((byte)newtitles.Count, offset);
            offset++;
            foreach (var unknown in newtitles)
            {
                PutULong(unknown, offset);
                offset += 8;
            }
        }
    }
}