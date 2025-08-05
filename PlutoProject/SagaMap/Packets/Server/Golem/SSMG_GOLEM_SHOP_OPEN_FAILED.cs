using SagaLib;

namespace SagaMap.Packets.Server.Golem
{
    public class SSMG_GOLEM_SHOP_OPEN_FAILED : Packet
    {
        public SSMG_GOLEM_SHOP_OPEN_FAILED()
        {
            data = new byte[7];
            offset = 2;
            ID = 0x17FD;
        }

        /// <summary>
        ///     GAME_SMSG_GOLEM_ACCESSERR1,";何らかの原因でゴーレムにアクセスできません";
        ///     GAME_SMSG_GOLEM_ACCESSERR2,";徘徊しているゴーレムです";
        ///     GAME_SMSG_GOLEM_ACCESSERR3,";商品がないのでゴーレムショップは開けません";
        ///     GAME_SMSG_GOLEM_ACCESSERR4,";イベント中はゴーレムショップは開けません";
        ///     GAME_SMSG_GOLEM_ACCESSERR5,";トレード中はゴーレムショップは開けません";
        ///     GAME_SMSG_GOLEM_ACCESSERR6,";憑依中はゴーレムショップは開けません";
        ///     GAME_SMSG_GOLEM_ACCESSERR7,";行動不能時はゴーレムショップは開けません";
        ///     GAME_SMSG_GOLEM_ACCESSERR8,";ゴーレムショップを開ける状態ではありません";
        ///     GAME_SMSG_GOLEM_ACCESSERR9,";他の種類のゴーレムにアクセス中です";
        /// </summary>
        public int Result
        {
            set => PutByte((byte)value, 2);
        }

        public uint ActorID
        {
            set => PutUInt(value, 3);
        }
    }
}