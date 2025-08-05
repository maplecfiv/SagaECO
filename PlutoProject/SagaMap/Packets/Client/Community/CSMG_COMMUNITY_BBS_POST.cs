using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.Community
{
    public class CSMG_COMMUNITY_BBS_POST : Packet
    {
        public CSMG_COMMUNITY_BBS_POST()
        {
            offset = 2;
        }

        public string Title
        {
            get
            {
                var len = GetByte(2);
                return Global.Unicode.GetString(GetBytes(len, 3)).Replace("\0", "");
            }
        }

        public string Content
        {
            get
            {
                var offset = GetByte(2);
                int len = GetByte((ushort)(3 + offset));
                if (len == 0xfd)
                {
                    len = GetInt((ushort)(4 + offset));
                    return Global.Unicode.GetString(GetBytes((ushort)len, (ushort)(8 + offset))).Replace("\0", "");
                }

                return Global.Unicode.GetString(GetBytes((ushort)len, (ushort)(4 + offset))).Replace("\0", "");
            }
        }

        public override Packet New()
        {
            return new CSMG_COMMUNITY_BBS_POST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnBBSPost(this);
        }
    }
}