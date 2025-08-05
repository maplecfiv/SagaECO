using SagaLib;

namespace SagaMap.Packets.Server.Party
{
    public class SSMG_PARTY_QUIT : Packet
    {
        public SSMG_PARTY_QUIT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x19CE;
        }

        /// <summary>
        ///     -1 : GAME_SMSG_PARTY_LEAVEERR1,"パーティーに所属していません"
        /// </summary>
        public int Result
        {
            set => PutInt(value, 2);
        }
    }
}