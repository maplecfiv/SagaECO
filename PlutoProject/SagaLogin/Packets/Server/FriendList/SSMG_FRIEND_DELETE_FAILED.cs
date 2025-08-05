using SagaLib;

namespace SagaLogin.Packets.Server.FriendList
{
    public class SSMG_FRIEND_DELETE : Packet
    {
        public SSMG_FRIEND_DELETE()
        {
            data = new byte[6];
            ID = 0x00D8;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }
    }
}