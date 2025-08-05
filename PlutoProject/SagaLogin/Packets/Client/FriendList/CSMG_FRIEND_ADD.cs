using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client
{
    public class CSMG_FRIEND_ADD : Packet
    {
        public CSMG_FRIEND_ADD()
        {
            offset = 2;
        }

        public uint CharID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FRIEND_ADD();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnFriendAdd(this);
        }
    }
}