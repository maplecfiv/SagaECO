using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_ENTER : Packet
    {
        public SSMG_FF_ENTER()
        {
            data = new byte[37];
            offset = 2;
            ID = 0x2008;
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

        public uint RingID
        {
            set => PutUInt(value, 9);
        }

        public uint RingHouseID
        {
            set
            {
                //Furiture ID
                PutByte(0x03, 13);
                PutUInt(value, 14);
            }
        }

        public uint RingHouseBedID
        {
            set =>
                //Furiture ID
                PutUInt(value, 18);
        }

        public uint RingHouseWallID
        {
            set =>
                //Furiture ID
                PutUInt(value, 22);
        }

        public ushort HouseX
        {
            set => PutUShort(value, 26);
        }

        public ushort HouseY
        {
            set => PutUShort(value, 28);
        }

        public ushort HouseDir
        {
            set => PutUShort(value, 30);
        }
    }
}