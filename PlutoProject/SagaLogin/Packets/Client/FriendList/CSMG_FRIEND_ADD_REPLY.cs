using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.FriendList
{
    public class CSMG_FRIEND_ADD_REPLY : Packet
    {
        public CSMG_FRIEND_ADD_REPLY()
        {
            offset = 2;
        }

        public uint Reply => GetUInt(2);

        public uint CharID => GetUInt(6);

        public override Packet New()
        {
            return new CSMG_FRIEND_ADD_REPLY();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnFriendAddReply(this);
        }
    }
}