using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    //表示当前飞空城的健康状态
    public class SSMG_FF_HEALTH_MODE : Packet
    {
        public SSMG_FF_HEALTH_MODE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x2018;
        }

        /// <summary>
        ///     00 = 正常 01 = 停滞状态 02 = 扣押状态 03 = 维持不能
        /// </summary>
        public byte value
        {
            set => PutByte(value, 2);
        }
    }
}