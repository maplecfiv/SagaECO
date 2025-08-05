using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_DEM_LEVEL : Packet
    {
        //DEM系教派等级
        public SSMG_FF_DEM_LEVEL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2022;
        }

        public uint level
        {
            set => PutUInt(value, 2);
        }

        public uint value
        {
            set => PutUInt(value, 6);
        }
    }
}