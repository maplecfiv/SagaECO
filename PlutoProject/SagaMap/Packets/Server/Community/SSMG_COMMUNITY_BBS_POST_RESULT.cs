using SagaLib;

namespace SagaMap.Packets.Server.Community
{
    public class SSMG_COMMUNITY_BBS_POST_RESULT : Packet
    {
        public enum Results
        {
            SUCCEED = 0, //";投稿しました";
            FAILED = -1, //";投稿に失敗しました";
            NOT_ENOUGH_MONEY = -2 //";お金が足りません";
        }

        public SSMG_COMMUNITY_BBS_POST_RESULT()
        {
            data = new byte[6];
            offset = 2;
            ID = 0x1AFF;
        }

        public Results Result
        {
            set => PutInt((int)value, 2);
        }
    }
}