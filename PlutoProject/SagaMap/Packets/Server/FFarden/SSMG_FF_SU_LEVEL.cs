using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_SU_LEVEL : Packet
    {
        //SU系教派等级
        public SSMG_FF_SU_LEVEL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2020;
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