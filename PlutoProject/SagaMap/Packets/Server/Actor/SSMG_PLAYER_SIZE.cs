using SagaLib;

namespace SagaMap.Packets.Server.Actor
{
    public class SSMG_PLAYER_SIZE : Packet
    {
        /// <summary>
        ///     [00][0E][02][0F]
        ///     [00][00][??][??] //キャラID
        ///     [00][00][07][D0] //2000 1000で標準 500チビチビ 2000デカデカ
        ///     [00][00][05][DC] //1500 固定？　値を変えても変化無し
        /// </summary>
        public SSMG_PLAYER_SIZE()
        {
            data = new byte[14];
            offset = 2;
            ID = 0x020F;
        }

        /// <summary>
        ///     キャラID
        /// </summary>
        public uint ActorID
        {
            set => PutUInt(value, 2);
        }

        /// <summary>
        ///     2000 1000で標準 500チビチビ 2000デカデカ
        /// </summary>
        public uint Size
        {
            set => PutUInt(value, 6);
        }

        /// <summary>
        ///     1500 固定？　値を変えても変化無し
        /// </summary>
        public uint unknwon
        {
            set => PutUInt(1500, 10);
        }
    }
}