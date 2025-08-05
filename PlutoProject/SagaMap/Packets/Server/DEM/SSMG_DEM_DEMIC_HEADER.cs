using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEM_DEMIC_HEADER : Packet
    {
        public SSMG_DEM_DEMIC_HEADER()
        {
            data = new byte[12];
            offset = 2;
            ID = 0x1E46;

            PutShort(9, 4); //Unknown1
            PutShort(0x1C0, 6); //Unknown2
            PutShort(0xD0, 8); //Unknown2
        }

        public short CL
        {
            set => PutShort(value, 2);
        }
    }
}