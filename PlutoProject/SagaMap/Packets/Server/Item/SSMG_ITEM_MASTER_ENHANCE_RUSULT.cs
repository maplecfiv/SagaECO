using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_ITEM_MASTERENHANCE_RESULT : Packet
    {
        public SSMG_ITEM_MASTERENHANCE_RESULT()
        {
            data = new byte[5];
            offset = 2;
            ID = 0x1F58;
            PutShort(0x64, 3);
        }

        /// <summary>
        ///     0成功
        ///     -1錢不夠
        ///     -2無可強化品
        ///     -3無觸媒
        ///     -4不明原因失敗
        ///     -5武具耐久不足
        ///     -6無法獲得經驗
        /// </summary>
        public int Result
        {
            set => PutByte((byte)value, 2);
        }
    }
}