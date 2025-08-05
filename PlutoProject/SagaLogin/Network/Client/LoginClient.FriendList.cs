using SagaDB.Actor;
using SagaLogin.Manager;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public enum CharStatus
    {
        OFFLINE,
        ONLINE,
        募集中,
        取り込み中,
        お話し中,
        休憩中,
        退席中,
        戦闘中,
        商売中,
        憑依中,
        クエスト中,
        お祭り中,
        連絡求む
    }

    public partial class LoginClient : SagaLib.Client
    {
        public uint currentMap;
        public CharStatus currentStatus = CharStatus.ONLINE;
        private LoginClient friendTarget;
        public PC_JOB job;
        public byte lv, joblv;

        public void OnFriendDelete(CSMG_FRIEND_DELETE p)
        {
            if (selectedChar == null)
                return;
            LoginServer.charDB.DeleteFriend(selectedChar.CharID, p.CharID);
            LoginServer.charDB.DeleteFriend(p.CharID, selectedChar.CharID);
            var p1 = new SSMG_FRIEND_DELETE();
            p1.CharID = p.CharID;
            netIO.SendPacket(p1);
            var client = LoginClientManager.Instance.FindClient(p.CharID);
            if (client != null)
            {
                p1 = new SSMG_FRIEND_DELETE();
                p1.CharID = selectedChar.CharID;
                client.netIO.SendPacket(p1);
            }
        }

        public void OnFriendAdd(CSMG_FRIEND_ADD p)
        {
            var client = LoginClientManager.Instance.FindClient(p.CharID);
            if (client != null)
            {
                if (!LoginServer.charDB.IsFriend(selectedChar.CharID, client.selectedChar.CharID))
                {
                    friendTarget = client;
                    client.friendTarget = this;
                    var p1 = new SSMG_FRIEND_ADD();
                    p1.CharID = selectedChar.CharID;
                    p1.Name = selectedChar.Name;
                    client.netIO.SendPacket(p1);
                }
                else
                {
                    var p1 = new SSMG_FRIEND_ADD_FAILED();
                    p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.TARGET_REFUSED;
                    netIO.SendPacket(p1);
                }
            }
            else
            {
                var p1 = new SSMG_FRIEND_ADD_FAILED();
                p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.CANNOT_FIND_TARGET;
                netIO.SendPacket(p1);
            }
        }

        private int CheckFriendRegist(LoginClient client)
        {
            if (client == null)
                return -1; //相手が見付かりません
            if (LoginServer.charDB.IsFriend(selectedChar.CharID, client.selectedChar.CharID))
                return -2; //既に登録しています
            //return -3; //相手に拒否されました
            //return -4; //フレンドリストに空きがありません
            //return -6; //相手がフレンド招待不許可設定です
            return 0;
        }

        public void OnFriendAddReply(CSMG_FRIEND_ADD_REPLY p)
        {
            if (friendTarget == null)
            {
                var p1 = new SSMG_FRIEND_ADD_FAILED();
                p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.CANNOT_FIND_TARGET;
                netIO.SendPacket(p1);
                return;
            }

            if (p.Reply == 1)
            {
                var p1 = new SSMG_FRIEND_ADD_OK();
                p1.CharID = friendTarget.selectedChar.CharID;
                netIO.SendPacket(p1);
                SendFriendAdd(friendTarget);
                LoginServer.charDB.AddFriend(selectedChar, friendTarget.selectedChar.CharID);
                p1 = new SSMG_FRIEND_ADD_OK();
                p1.CharID = selectedChar.CharID;
                friendTarget.netIO.SendPacket(p1);
                friendTarget.SendFriendAdd(this);
                LoginServer.charDB.AddFriend(friendTarget.selectedChar, selectedChar.CharID);
            }
            else
            {
                var p1 = new SSMG_FRIEND_ADD_FAILED();
                p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.TARGET_REFUSED;
                friendTarget.netIO.SendPacket(p1);
            }

            friendTarget = null;
        }

        public void OnFriendMapUpdate(CSMG_FRIEND_MAP_UPDATE p)
        {
            if (selectedChar == null) return;
            var friendlist = LoginServer.charDB.GetFriendList2(selectedChar);
            currentMap = p.MapID;
            foreach (var i in friendlist)
            {
                var client = LoginClientManager.Instance.FindClient(i);
                var p1 = new SSMG_FRIEND_MAP_UPDATE();
                p1.CharID = selectedChar.CharID;
                if (client != null)
                {
                    p1.MapID = currentMap;
                    client.netIO.SendPacket(p1);
                }
            }
        }

        public void OnFriendDetailUpdate(CSMG_FRIEND_DETAIL_UPDATE p)
        {
            if (selectedChar == null) return;
            var friendlist = LoginServer.charDB.GetFriendList2(selectedChar);
            job = p.Job;
            lv = p.Level;
            joblv = p.Level;
            foreach (var i in friendlist)
            {
                var client = LoginClientManager.Instance.FindClient(i);
                var p1 = new SSMG_FRIEND_DETAIL_UPDATE();
                p1.CharID = selectedChar.CharID;
                if (client != null)
                {
                    p1.Job = job;
                    p1.Level = lv;
                    p1.JobLevel = joblv;
                    client.netIO.SendPacket(p1);
                }
            }
        }

        public void SendFriendAdd(LoginClient client)
        {
            var p = new SSMG_FRIEND_CHAR_INFO();
            p.ActorPC = client.selectedChar;
            p.MapID = client.currentMap;
            p.Status = client.currentStatus;
            p.Comment = "";
            netIO.SendPacket(p);
        }

        public void SendFriendList()
        {
            if (selectedChar == null) return;
            var friendlist = LoginServer.charDB.GetFriendList(selectedChar);
            foreach (var i in friendlist)
            {
                var client = LoginClientManager.Instance.FindClient(i);
                var p = new SSMG_FRIEND_CHAR_INFO();
                p.ActorPC = i;
                if (client != null)
                {
                    p.MapID = client.currentMap;
                    p.Status = client.currentStatus;
                }

                p.Comment = "";
                netIO.SendPacket(p);
            }
        }

        public void SendStatusToFriends()
        {
            if (selectedChar == null) return;
            var friendlist = LoginServer.charDB.GetFriendList2(selectedChar);
            foreach (var i in friendlist)
            {
                var client = LoginClientManager.Instance.FindClient(i);
                var p1 = new SSMG_FRIEND_STATUS_UPDATE();
                var p2 = new SSMG_FRIEND_DETAIL_UPDATE();
                p1.CharID = selectedChar.CharID;
                p2.CharID = selectedChar.CharID;
                if (client != null)
                {
                    p1.Status = currentStatus;
                    p2.Job = selectedChar.Job;
                    p2.Level = selectedChar.Level;
                    p2.JobLevel = selectedChar.CurrentJobLevel;
                    client.netIO.SendPacket(p1);
                    client.netIO.SendPacket(p2);
                }
            }
        }
    }
}