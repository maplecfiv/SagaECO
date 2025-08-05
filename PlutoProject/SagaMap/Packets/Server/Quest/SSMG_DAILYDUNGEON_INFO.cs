using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Quest
{
    public class SSMG_DAILYDUNGEON_INFO : Packet
    {
        public SSMG_DAILYDUNGEON_INFO()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x1F77;
        }

        public uint RemainSecond
        {
            set => PutUInt(value, 2);
        }

        public List<byte> IDs
        {
            set
            {
                PutByte((byte)value.Count, 6);
                for (var i = 0; i < value.Count; i++) PutByte(value[i], 7 + i);
            }
        }
    }
}