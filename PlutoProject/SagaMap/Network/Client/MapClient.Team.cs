using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Team;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        private ActorPC teamPartner;

        public void OnAbyssTeamListRequest(CSMG_ABYSSTEAM_LIST_REQUEST p)
        {
        }

        public void OnAbyssTeamListClose(CSMG_ABYSSTEAM_LIST_CLOSE p)
        {
        }

        public void OnAbyssTeamBreakRequest(CSMG_ABYSSTEAM_BREAK_REQUEST p)
        {
            var team = Character.Team;
            var members = new List<ActorPC>();
            var teamName = "";
            if (team != null)
            {
                teamName = team.Name;
                members = team.Members.Values.ToList();
            }

            var Result = unchecked((byte)CheckAbyssTeamBreakRequest(team));

            if (Result == 0)
            {
                var p1 = new SSMG_ABYSSTEAM_BREAK();
                p1.TeamName = teamName;
                MapClient member;
                for (var i = 0; i < members.Count; i++)
                    if (members[i].Online)
                    {
                        member = FromActorPC(members[i]);
                        if (member != null) member.netIO.SendPacket(p1);
                    }
            }
            else
            {
                var p1 = new SSMG_ABYSSTEAM_BREAK();
                p1.Result = Result;
                netIO.SendPacket(p1);
            }
        }

        private int CheckAbyssTeamBreakRequest(Team team)
        {
            if (team == null)
                return -3; //既にチームが解散されています
            try
            {
                AbyssTeamManager.Instance.TeamDismiss(team);
            }
            catch
            {
            }

            return 0; //チーム「%s」を解散しました
        }

        public void OnAbyssTeamLeaveRequest(CSMG_ABYSSTEAM_LEAVE_REQUEST p)
        {
            var team = Character.Team;
            var teamName = "";
            if (team != null)
                teamName = team.Name;

            var Result = unchecked((byte)CheckAbyssTeamLeaveRequest(team));
            var p1 = new SSMG_ABYSSTEAM_LEAVE();
            netIO.SendPacket(p1);
        }

        private int CheckAbyssTeamLeaveRequest(Team team)
        {
            if (team == null)
                return -3; //既にチームが解散されています
            try
            {
                AbyssTeamManager.Instance.DeleteMember(team, Character.CharID);
            }
            catch
            {
            }

            return 0; //チーム「%s」を脱退しました
        }

        public void OnAbyssTeamRegistRequest(CSMG_ABYSSTEAM_REGIST_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamRegistRequest(p.LeaderID, p.Password));
            if (Result == 2)
            {
                var target = MapClientManager.Instance.FindClient(p.LeaderID);
                teamPartner = target.Character;
                var p1 = new SSMG_ABYSSTEAM_REGIST_APPROVAL();
                p1.CharID = Character.CharID;
                p1.Name = Character.Name;
                p1.Level = Character.Level;
                p1.Job = Character.Job;
                target.netIO.SendPacket(p1);
            }

            var p2 = new SSMG_ABYSSTEAM_REGIST_APPLY();
            p2.Result = Result;
            netIO.SendPacket(p2);
        }

        private int CheckAbyssTeamRegistRequest(uint leaderID, string pass)
        {
            var team = AbyssTeamManager.Instance.GetTeam(leaderID);
            if (teamPartner != null)
                return -9; //参加申し込みの上限に達しているため、申請を行えませんでした
            if (Character.Team != null)
                return -8; //既にチームに参加しています
            if (team == null)
                return -7; //チームが解散されているため入れませんでした
            if (!team.Leader.Online)
                return -6; //リーダーがいないため入れませんでした
            if (!team.JobRequirements.Contains(Character.Job))
                return -4; //条件に合わなかったため入れませんでした
            if (pass != team.Pass)
                return -3; //パスワードが違います
            if (team.Members.Count >= team.MaxMember)
                return -2; //チームが満員のため入れませんでした
            var leader = MapClientManager.Instance.FindClient(leaderID);
            if (leader.teamPartner != null)
                return -10; //申請許可を待っているユーザーは多数居たため、加入することは出来ませんでした
            return 2; //加入申請中です
        }

        public void OnAbyssTeamRegistApproval(CSMG_ABYSSTEAM_REGIST_APPROVAL p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var p1 = new SSMG_ABYSSTEAM_REGIST_APPLY();
            var approved = false;
            if (p.Result == 0)
                approved = true;
            var Result = unchecked((byte)CheckAbyssTeamRegistApproval(approved));
            p1.Result = Result;
            if (Result == 1)
                p1.TeamName = Character.Team.Name;
            client.netIO.SendPacket(p1);
        }

        private int CheckAbyssTeamRegistApproval(bool approved)
        {
            if (!approved)
                return -5; //参加できませんでした
            var team = Character.Team;
            try
            {
                AbyssTeamManager.Instance.AddMember(team, teamPartner);
                return 1; //%sに参加しました
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }
            //何らかの原因で失敗しました
            //参加申請を出したユーザーがロビー内に居なかったため、参加申請はキャンセルされました
            //既に他のチームに加入しているため、加入することが出来ませんでした。
        }

        public void OnAbyssTeamSetCreateRequest(CSMG_ABYSSTEAM_SET_CREATE_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamSetCreate(p));
            var p1 = new SSMG_ABYSSTEAM_SET_CREATE();
            p1.Result = Result;
            netIO.SendPacket(p1);
        }

        private int CheckAbyssTeamSetCreate(CSMG_ABYSSTEAM_SET_CREATE_REQUEST p)
        {
            if (!AbyssTeamManager.Instance.CheckRegistLimit())
                return -2; //チーム登録数の上限に達しているため、チームを登録することが出来ませんでした
            try
            {
                var job = new List<PC_JOB>();
                AbyssTeamManager.Instance.CreateTeam(Character, p.TeamName, p.Comment, p.Password, p.IsFromSave,
                    p.MinLV, p.MaxLV, job);
            }
            catch
            {
                return -1; //何らかの原因で失敗しました
            }

            return 0; //チームを登録しました
        }

        public void OnAbyssTeamSetOpenRequest(CSMG_ABYSSTEAM_SET_OPEN_REQUEST p)
        {
            var Result = unchecked((byte)CheckAbyssTeamSetOpen());
            var p1 = new SSMG_ABYSSTEAM_SET_OPEN();
            p1.Result = Result;
            if (Result == 0)
                p1.Floor = 100;
            netIO.SendPacket(p1);
        }

        private int CheckAbyssTeamSetOpen()
        {
            if (Character.Team != null)
            {
                if (Character.Team.Leader == Character)
                    return -2; //既にチームを登録しているため、チーム登録が行えませんでした
                return -3; //既に他のチームに加入しているため、チーム登録が行えませんでした
            }

            if (!AbyssTeamManager.Instance.CheckRegistLimit())
                return -4; //チーム登録数の上限に達しているため、チームを登録することが出来ませんでした
            if (Character.AbyssFloor == 0)
                return -1; //何らかの原因で失敗しました
            return 0;
        }
    }
}