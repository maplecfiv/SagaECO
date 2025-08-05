using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.FriendList
{
    public class CSMG_FRIEND_MAP_UPDATE : Packet
    {
        public CSMG_FRIEND_MAP_UPDATE()
        {
            offset = 2;
        }

        public uint MapID => GetUInt(2);

        public override Packet New()
        {
            return new CSMG_FRIEND_MAP_UPDATE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnFriendMapUpdate(this);
        }
    }
}