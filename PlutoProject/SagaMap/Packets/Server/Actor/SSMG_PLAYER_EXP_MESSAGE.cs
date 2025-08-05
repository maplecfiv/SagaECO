using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PLAYER_EXP_MESSAGE : Packet
    {
        public enum EXP_MESSAGE_TYPE
        {
            NormalGain,
            PossessionGain,
            Loss,
            TamaireGain
        }

        public SSMG_PLAYER_EXP_MESSAGE()
        {
            data = new byte[27];
            offset = 2;
            ID = 0x0238;
        }

        /// <summary>
        ///     Base EXP
        /// </summary>
        public long EXP
        {
            set => PutLong(value, 2);
        }

        /// <summary>
        ///     Job EXP
        /// </summary>
        public long JEXP
        {
            set => PutLong(value, 10);
        }

        /// <summary>
        ///     Another Page EXP
        /// </summary>
        public long PEXP
        {
            set => PutLong(value, 18);
        }

        public EXP_MESSAGE_TYPE Type
        {
            set => PutByte((byte)value, 26);
        }
    }
}