using SagaLib;

namespace SagaLogin.Packets.Server.FriendList
{
    public class SSMG_FRIEND_ADD_FAILED : Packet
    {
        public enum Result
        {
            CANNOT_FIND_TARGET = -5,
            TARGET_REFUSED = -3,
            NO_FREE_SPACE = -2,
            TARGET_NO_FREE_SPACE = -1
        }

        public SSMG_FRIEND_ADD_FAILED()
        {
            data = new byte[6];
            ID = 0x00D6;
        }

        public Result AddResult
        {
            set => PutUInt((uint)value, 2);
        }
    }
}