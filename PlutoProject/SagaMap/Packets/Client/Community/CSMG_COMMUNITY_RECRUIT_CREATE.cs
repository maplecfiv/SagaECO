using SagaLib;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_RECRUIT_CREATE : Packet
    {
        public CSMG_COMMUNITY_RECRUIT_CREATE()
        {
            offset = 2;
        }

        public RecruitmentType Type => (RecruitmentType)GetByte(2);

        public string Title
        {
            get
            {
                var title = Global.Unicode.GetString(GetBytes(GetByte(3), 4));
                return title.Replace("\0", "");
            }
        }

        public string Content
        {
            get
            {
                var size = GetByte(3);
                var title = Global.Unicode.GetString(GetBytes(GetByte((ushort)(4 + size)), (ushort)(5 + size)));
                return title.Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_COMMUNITY_RECRUIT_CREATE();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnRecruitCreate(this);
        }
    }
}