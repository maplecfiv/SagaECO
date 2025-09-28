using SagaLib;

namespace SagaMap.Packets.Server.FFGarden
{
    public class SSMG_FF_EXPAND_POINT : Packet
    {
        /// <summary>
        ///     飞空城的扩展点数
        /// </summary>
        public SSMG_FF_EXPAND_POINT()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x201E;
            PutUInt(0, 2);
        }

        public uint value
        {
            set => PutUInt(value, 6);
        }
    }
}