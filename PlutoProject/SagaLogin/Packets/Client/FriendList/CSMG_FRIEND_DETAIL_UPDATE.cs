using SagaDB.Actor;
using SagaLib;
using SagaLogin.Network.Client;

namespace SagaLogin.Packets.Client.FriendList
{
    public class CSMG_FRIEND_DETAIL_UPDATE : Packet
    {
        public CSMG_FRIEND_DETAIL_UPDATE()
        {
            offset = 2;
        }

        public PC_JOB Job => (PC_JOB)GetUShort(2);

        public byte Level => (byte)GetUShort(4);

        public byte JobLevel => (byte)GetUShort(6);

        public override Packet New()
        {
            return new CSMG_FRIEND_DETAIL_UPDATE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((LoginClient)client).OnFriendDetailUpdate(this);
        }
    }
}