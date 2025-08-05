using System.Text;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Packets.Client.AbyssTeam
{
    public class CSMG_ABYSSTEAM_SET_CREATE_REQUEST : Packet
    {
        public CSMG_ABYSSTEAM_SET_CREATE_REQUEST()
        {
            offset = 2;
        }

        //奈落隊伍建立 TSTR(隊伍名稱) TSTR(訊息) TSTR(密碼) 00 BYTE(0重新1存點) BYTE(最低LV) BYTE(最高LV) AWORD(04職業許可 2進制 全允許 0F 0F 0F 03)
        public string TeamName
        {
            get
            {
                offset = 2;
                var len = GetByte();
                return Encoding.UTF8.GetString(GetBytes(len)).Replace("/0", "");
            }
        }

        public string Comment
        {
            get
            {
                var len = GetByte();
                return Encoding.UTF8.GetString(GetBytes(len)).Replace("/0", "");
            }
        }

        public string Password
        {
            get
            {
                var len = GetByte();
                return Encoding.UTF8.GetString(GetBytes(len)).Replace("/0", "");
            }
        }

        public bool IsFromSave
        {
            get
            {
                offset += 1;
                if (GetByte() == 1)
                    return true;
                return false;
            }
        }

        public byte MinLV => GetByte();

        public byte MaxLV => GetByte();

        public override Packet New()
        {
            return new CSMG_ABYSSTEAM_SET_CREATE_REQUEST();
        }

        public override void Parse(SagaLib.Client client)
        {
            ((MapClient)client).OnAbyssTeamSetCreateRequest(this);
        }
    }
}