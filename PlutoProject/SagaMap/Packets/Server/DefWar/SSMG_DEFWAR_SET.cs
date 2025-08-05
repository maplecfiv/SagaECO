using SagaLib;

namespace SagaMap.Packets.Server.DefWar
{
    public class SSMG_DEFWAR_SET : Packet
    {
        public SSMG_DEFWAR_SET()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x1BD1;
        }

        public uint MapID
        {
            set => PutUInt(value, 10);
        }

        public SagaDB.DefWar.DefWar Data
        {
            set
            {
                PutByte(value.Number, 2);
                PutUInt(value.ID, 3);
                PutByte(value.Result1, 7);
                PutByte(value.Result2, 8);
                PutByte(value.unknown1, 9);
            }
        }
    }
}