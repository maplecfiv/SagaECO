using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_TITLE_REQ : Packet
    {
        public SSMG_PLAYER_TITLE_REQ()
        {
            data = new byte[6]; //8bytes unknowns
            offset = 2;
            ID = 0x241C;
        }

        public uint tID
        {
            set => PutUInt(value, 2);
        }

        public void PutPrerequisite(List<ulong> prerequisite)
        {
            var buf = new byte[15 + prerequisite.Count * 8];
            data.CopyTo(buf, 0);
            data = buf;
            PutByte((byte)prerequisite.Count, 6);
            offset = 7;
            foreach (var progress in prerequisite)
                PutULong(progress);
        }
    }
}