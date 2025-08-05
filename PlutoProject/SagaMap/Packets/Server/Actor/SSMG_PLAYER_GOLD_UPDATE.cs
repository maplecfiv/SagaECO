using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_GOLD_UPDATE : Packet
    {
        /// <summary>
        ///     [00][06][09][EC]
        ///     [00][00][27][10] //10000Gold
        /// </summary>
        public SSMG_PLAYER_GOLD_UPDATE()
        {
            data = new byte[10];
            offset = 2;
            ID = 0x09EC;
        }

        public ulong Gold
        {
            set => PutULong(value, 2);
        }
    }
}