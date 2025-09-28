using System.Collections.Generic;
using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_GOTO_FG : Packet
    {
        public SSMG_PLAYER_GOTO_FG()
        {
            data = new byte[57];
            offset = 2;
            ID = 0x1BE4;

            PutByte(9, 9);
            PutByte(9, 46);
            PutByte(1, 56);
        }

        public uint MapID
        {
            set => PutUInt(value, 2);
        }

        public byte X
        {
            set => PutByte(value, 6);
        }

        public byte Y
        {
            set => PutByte(value, 7);
        }

        public byte Dir
        {
            set => PutByte(value, 8);
        }

        public Dictionary<FGardenSlot, uint> Equiptments
        {
            set
            {
                for (var i = 0; i < 8; i++) PutUInt(value[(FGardenSlot)i], (ushort)(10 + i * 4));
            }
        }
    }
}