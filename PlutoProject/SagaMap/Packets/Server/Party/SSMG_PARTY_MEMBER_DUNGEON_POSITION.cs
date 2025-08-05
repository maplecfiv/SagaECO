using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_MEMBER_DUNGEON_POSITION : Packet
    {
        public SSMG_PARTY_MEMBER_DUNGEON_POSITION()
        {
            data = new byte[13];
            offset = 2;
            ID = 0x1C84;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public uint MapID
        {
            set => PutUInt(value, 6);
        }

        public byte X
        {
            set => PutByte(value, 10);
        }

        public byte Y
        {
            set => PutByte(value, 11);
        }

        public byte Dir
        {
            set => PutByte(value, 12);
        }
    }
}