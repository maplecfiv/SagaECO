using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_BP_LEVEL : Packet
    {
        //BP系教派等级
        public SSMG_FF_BP_LEVEL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2021;
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