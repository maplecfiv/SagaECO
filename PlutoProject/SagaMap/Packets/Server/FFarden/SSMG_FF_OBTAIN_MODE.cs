using SagaLib;

namespace SagaMap.Packets.Server.FFarden
{
    public class SSMG_FF_OBTAIN_MODE : Packet //表示当前飞空城的取得状态
    {
        public SSMG_FF_OBTAIN_MODE()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x2017;
        }

        /// <summary>
        ///     00 = 无入手   01 = 作出可能  03 = 已入手
        /// </summary>
        public byte value
        {
            set => PutByte(value, 2);
        }
    }
}