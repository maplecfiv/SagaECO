using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.FriendList
{
    public class CSMG_FRIEND_DELETE : Packet
    {
        public CSMG_FRIEND_DELETE()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FRIEND_DELETE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnFriendDelete(this);
        }
    }
}