using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_CHANGE_MAP : Packet
    {
        public SSMG_PLAYER_CHANGE_MAP()
        {
            data = new byte[17];
            offset = 2;
            ID = 0x11FD;

            DungeonDir = 4;
            DungeonX = 255;
            DungeonY = 255;
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

        public byte DungeonDir
        {
            set => PutByte(value, 9);
        }

        public byte DungeonX
        {
            set => PutByte(value, 10);
        }

        public byte DungeonY
        {
            set => PutByte(value, 11);
        }

        public bool FGTakeOff
        {
            set
            {
                if (value) PutByte(1, 16);
            }
        }
    }
}