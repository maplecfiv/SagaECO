using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public bool bbsClose;
        public uint bbsCost;
        public int bbsCurrentPage;
        public uint bbsID;
        public uint bbsMinContent;

        public void OnBBSRequestPage(CSMG_COMMUNITY_BBS_REQUEST_PAGE p)
        {
            bbsCurrentPage = p.Page;
            SendBBSPage();
        }

        public void SendBBSPage()
        {
            var p1 = new SSMG_COMMUNITY_BBS_PAGE_INFO();
            p1.Posts = MapServer.charDB.GetBBSPage(bbsID, bbsCurrentPage);
            netIO.SendPacket(p1);
        }

        public void OnBBSPost(CSMG_COMMUNITY_BBS_POST p)
        {
            var p1 = new SSMG_COMMUNITY_BBS_POST_RESULT();
            var result = CheckBBSPost(p.Title, p.Content);
            if (result >= 0)
                Character.Gold -= (int)bbsCost;
            netIO.SendPacket(p1);
            SendBBSPage();
        }

        private int CheckBBSPost(string title, string content)
        {
            if (Character.Gold <= bbsCost)
                return -2; //お金が足りません
            if (content.Length < bbsMinContent)
                return -3; //投稿内容の文字数が足りません
            if (!MapServer.charDB.BBSNewPost(Character, bbsID, title, content))
                return -1; //投稿に失敗しました
            return 0; //投稿しました
            // return -4; //投稿回数が制限に達したため失敗しました
        }

        public void OnBBSClose(CSMG_COMMUNITY_BBS_CLOSE p)
        {
            bbsClose = true;
        }

        public void OnRecruit(CSMG_COMMUNITY_RECRUIT p)
        {
            int maxPage;
            var page = p.Page;
            var res = RecruitmentManager.Instance.GetRecruitments(p.Type, page, out maxPage);
            var p1 = new SSMG_COMMUNITY_RECRUIT();
            p1.Type = p.Type;
            p1.Page = page;
            p1.MaxPage = maxPage;
            p1.Entries = res;
            netIO.SendPacket(p1);
        }

        public void OnRecruitRequestAns(CSMG_COMMUNITY_RECRUIT_REQUEST_ANS p)
        {
            var target = MapClientManager.Instance.FindClient(p.CharID);
            if (target == null || partyPartner == null)
                return;
            if (target.Character.CharID == partyPartner.CharID)
            {
                var result = 0;
                var p1 = new SSMG_COMMUNITY_RECRUIT_JOIN_RES();
                if (Character.Mode != target.Character.Mode)
                    return;
                if (!p.Accept)
                    result = -2; //パーティー参加要請を断られました
                if (result >= 0)
                {
                    if (Character.Party == null)
                    {
                        var party = PartyManager.Instance.CreateParty(Character);
                        PartyManager.Instance.AddMember(party, partyPartner);
                    }
                    else if (Character.Party.MemberCount >= 8)
                    {
                        result = -3; //要請をしたパーティーは満員です
                    }
                }

                if (result >= 0)
                    PartyManager.Instance.AddMember(Character.Party, partyPartner);
                p1.Result = result;
                p1.CharID = Character.CharID;
                target.netIO.SendPacket(p1);
            }
        }

        public void OnRecruitJoin(CSMG_COMMUNITY_RECRUIT_JOIN p)
        {
            var target = MapClientManager.Instance.FindClient(p.CharID);
            var result = CheckRecuitJoin(target);
            var p1 = new SSMG_COMMUNITY_RECRUIT_JOIN_RES();
            p1.Result = result;
            p1.CharID = p.CharID;
            netIO.SendPacket(p1);
            if (result >= 0)
            {
                partyPartner = target.Character;
                target.partyPartner = Character;
                var p2 = new SSMG_COMMUNITY_RECRUIT_REQUEST();
                p2.CharID = Character.CharID;
                p2.CharName = Character.Name;
                target.netIO.SendPacket(p2);
            }
        }

        private int CheckRecuitJoin(MapClient target)
        {
            // return -1; //パーティーサーバーとの接続に失敗しました
            // return -2; //パーティー参加要請を断られました
            if (target == null)
                return -4; //パーティー要請をした相手がオフライン中です
            if (target.Character.Party != null)
                if (target.Character.Party.MemberCount >= 8)
                    return -3; //要請をしたパーティーは満員です
            if (Character.Party != null)
                return -5; //パーティー参加中に要請はできません
            if (target.Character.CharID == Character.CharID)
                return -6; //自分にパーティー申請はできません
            var exist = false;
            var recruitlist = RecruitmentManager.Instance.GetRecruitments(RecruitmentType.Party);
            for (var i = 0; i < recruitlist.Count; i++)
                if (recruitlist[i].Creator.Name == target.Character.Name)
                    exist = true;
            if (!exist)
                return -7; //募集コメントの登録が消えています
            return 0; //参加要請中です
        }

        public void OnRecruitCreate(CSMG_COMMUNITY_RECRUIT_CREATE p)
        {
            var rec = new Recruitment();
            rec.Creator = Character;
            rec.Type = p.Type;
            rec.Title = p.Title;
            rec.Content = p.Content;
            RecruitmentManager.Instance.CreateRecruiment(rec);

            var p1 = new SSMG_COMMUNITY_RECRUIT_CREATE();
            netIO.SendPacket(p1);
        }

        public void OnRecruitDelete(CSMG_COMMUNITY_RECRUIT_DELETE p)
        {
            RecruitmentManager.Instance.DeleteRecruitment(Character);
            var p1 = new SSMG_COMMUNITY_RECRUIT_DELETE();
            netIO.SendPacket(p1);
        }
    }
}