using SagaLib;

namespace SagaMap.Packets.Server
{
    public class SSMG_PARTY_KICK : Packet
    {
        public SSMG_PARTY_KICK()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x19D3;
        }

        /// <summary>
        ///     -1:GAME_SMSG_PARTY_KICKERR1,"指定プレイヤーが存在しません"
        /// </summary>
        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}