using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public void OnGroupJoin()
        {
        }

        public void OnGroupMemberJoin()
        {
        }

        public void OnGroupMemberKick()
        {
        }

        public void OnGroupLeave()
        {
            if (Character.Party.Leader == Character)
            {
                var p2 = new SSMG_AAA_GROUP_DESTROY();
                netIO.SendPacket(p2);
            }
        }

        public void OnGroupSelect()
        {
        }

        public void OnGroupUpdate()
        {
        }

        public void OnGroupChangeState()
        {
        }

        public void OnGroupStart()
        {
        }

        public void OnGroupRestart()
        {
        }
    }
}