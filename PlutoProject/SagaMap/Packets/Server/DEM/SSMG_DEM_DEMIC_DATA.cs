using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_DEMIC_DATA : Packet
    {
        public SSMG_DEM_DEMIC_DATA()
        {
            data = new byte[166];
            offset = 2;
            ID = 0x1E4A;

            Size = 81;
        }

        public byte Page
        {
            set => PutByte(value, 2);
        }

        public byte Size
        {
            set => PutByte(value, 3);
        }

        public short[,] Chips
        {
            set
            {
                offset = 4;
                for (var i = 0; i < 9; i++)
                for (var j = 0; j < 9; j++)
                    PutShort(value[j, i]);
            }
        }
    }
}