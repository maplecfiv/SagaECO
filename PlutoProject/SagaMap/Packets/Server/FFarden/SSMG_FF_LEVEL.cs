using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_LEVEL : Packet
    {
        /// <summary>
        ///     飞空城的经验值和等级
        /// </summary>
        public SSMG_FF_LEVEL()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x2019;
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