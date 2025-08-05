using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_DEFWAR_RESULT : Packet
    {
        public SSMG_DEFWAR_RESULT()
        {
            data = new byte[17];
            offset = 2;
            ID = 0x1BC6;
        }

        /// <summary>
        ///     攻略结果1:2为夺还,3为攻略
        /// </summary>
        public byte Result1
        {
            set => PutByte(value, 2);
        }

        /// <summary>
        ///     攻略结果2:0为失败,1为成功,2为大成功,3为完全成功
        /// </summary>
        public byte Result2
        {
            set => PutByte(value, 3);
        }

        public int EXP
        {
            set => PutInt(value, 4);
        }

        public int JOBEXP
        {
            set => PutInt(value, 8);
        }

        public int CP
        {
            set => PutInt(value, 12);
        }

        public byte Unknown
        {
            set => PutByte(value, 16);
        }
    }
}