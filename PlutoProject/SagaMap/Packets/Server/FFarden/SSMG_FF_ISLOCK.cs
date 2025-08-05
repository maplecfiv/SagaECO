using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_FF_ISLOCK : Packet
    {
        //当前飞空城的入场条件
        public SSMG_FF_ISLOCK()
        {
            data = new byte[3];
            offset = 2;
            ID = 0x201A;
        }

        /// <summary>
        ///     00 = 无限制 01 = 需要密码
        /// </summary>
        public byte value
        {
            set => PutByte(value, 2);
        }
    }
}