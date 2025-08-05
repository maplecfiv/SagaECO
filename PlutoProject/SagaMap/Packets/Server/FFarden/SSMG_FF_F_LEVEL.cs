using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_F_LEVEL : Packet
    {
        //F系教派等级
        public SSMG_FF_F_LEVEL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x201F;
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