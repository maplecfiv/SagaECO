using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Server.FriendList
{
    public class SSMG_FRIEND_STATUS_UPDATE : Packet
    {
        public SSMG_FRIEND_STATUS_UPDATE()
        {
            data = new byte[7];
            ID = 0x00ED;
        }

        public uint CharID
        {
            set => PutUInt(value, 2);
        }

        public CharStatus Status
        {
            set => PutByte((byte)value, 6);
        }
    }
}