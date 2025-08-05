using SagaDB.Actor;
using SagaDB.Ring;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        public ActorPC ringPartner;

        public void OnRingEmblemUpload(CSMG_RING_EMBLEM_UPLOAD p)
        {
            var p1 = new SSMG_RING_EMBLEM_UPLOAD_RESULT();
            if (Character.Ring == null)
                return;

            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.RingMaster) ||
                Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.Ring2ndMaster))
            {
                var data = p.Data;
                if (data[0] == 0x89)
                {
                    if (Character.Ring.Fame >= Configuration.Instance.RingFameNeededForEmblem)
                    {
                        p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.OK;
                        MapServer.charDB.RingEmblemUpdate(Character.Ring, p.Data);
                    }
                    else
                    {
                        p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.FAME_NOT_ENOUGH;
                    }
                }
                else
                {
                    p1.Result = SSMG_RING_EMBLEM_UPLOAD_RESULT.Results.WRONG_FORMAT;
                }
            }

            netIO.SendPacket(p1);
        }

        public void OnChatRing(CSMG_CHAT_RING p)
        {
            if (Character.Ring == null)
                return;
            RingManager.Instance.RingChat(Character.Ring, Character, p.Content);

            //TODO:ECOE用
            //Logger.ShowChat("[R]" + this.Character.Name + " :" + p.Content, null);
        }

        public void OnRingRightSet(CSMG_RING_RIGHT_SET p)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.RingMaster) ||
                Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.Ring2ndMaster))
                RingManager.Instance.SetMemberRight(Character.Ring, p.CharID, p.Right);
        }

        public void OnRingKick(CSMG_RING_KICK p)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.Rights[Character.Ring.IndexOf(Character)].Test(RingRight.KickRight))
                RingManager.Instance.DeleteMember(Character.Ring, Character.Ring.GetMember(p.CharID),
                    SSMG_RING_QUIT.Reasons.KICK);
        }

        public void OnRingQuit(CSMG_RING_QUIT p)
        {
            var p1 = new SSMG_RING_QUIT_RESULT();
            if (Character.Ring == null)
            {
                p1.Result = -1;
            }
            else
            {
                if (Character != Character.Ring.Leader)
                    RingManager.Instance.DeleteMember(Character.Ring, Character, SSMG_RING_QUIT.Reasons.LEAVE);
                else
                    RingManager.Instance.RingDismiss(Character.Ring);
            }

            netIO.SendPacket(p1);
        }

        public void OnRingInviteAnswer(CSMG_RING_INVITE_ANSWER p, bool accepted)
        {
            if (accepted)
            {
                var p1 = new SSMG_RING_INVITE_ANSWER_RESULT();
                var result = CheckRingInviteAnswer();
                p1.Result = (SSMG_RING_INVITE_ANSWER_RESULT.RESULTS)result;
                if (result >= 0)
                    RingManager.Instance.AddMember(ringPartner.Ring, Character);
                netIO.SendPacket(p1);
            }

            ringPartner = null;
        }

        private int CheckRingInviteAnswer()
        {
            if (ringPartner == null)
                return -2;
            if (Character.Ring != null)
                return -11;
            if (ringPartner.Ring.MemberCount >= ringPartner.Ring.MaxMemberCount)
                return -12;
            var index = ringPartner.Ring.IndexOf(ringPartner);
            if (!ringPartner.Ring.Rights[index].Test(RingRight.AddRight))
                return -14;
            return 0;
        }

        public void OnRingInvite(CSMG_RING_INVITE p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var p1 = new SSMG_RING_INVITE_RESULT();
            var index = Character.Ring.IndexOf(Character);
            var result = CheckRingInvite(client);
            p1.Result = result;
            if (result == 0)
            {
                client.ringPartner = Character;
                var p2 = new SSMG_RING_INVITE();
                p2.CharID = Character.CharID;
                p2.CharName = Character.Name;
                p2.RingName = Character.Ring.Name;
                client.netIO.SendPacket(p2);
            }

            netIO.SendPacket(p1);
        }

        private int CheckRingInvite(MapClient client)
        {
            if (client == null)
                return -1; //相手が見つかりません
            if (!client.Character.canRing)
                return -3; //相手がリング招待不許可設定です
            if (client.Character.Ring == null)
                return -4; //相手はリングに加入済みです
            if (Character.Ring != null)
                return -5; //リングを組んでいないので誘えません 
            var index = Character.Ring.IndexOf(Character);
            if (!Character.Ring.Rights[index].Test(RingRight.AddRight))
                return -6; //招待権限を持っていません
            if (Character.Ring.IndexOf(client.Character) >= 0)
                return -9; //既にリングに入っています
            if (Character.Ring.MemberCount >= Character.Ring.MaxMemberCount)
                return -10; //誘った相手に招待権限がありません
            return 0;
        }

        public void SendRingMember()
        {
            if (Character.Ring == null)
                return;
            foreach (var i in Character.Ring.Members.Values)
            {
                var p = new SSMG_RING_MEMBER_INFO();
                p.Member(i, Character.Ring);
                netIO.SendPacket(p);
                SendRingMemberInfo(i);
            }
        }

        public void SendRingInfo(SSMG_RING_INFO.Reason reason)
        {
            if (Character.PlayerTitleID != 0)
            {
                var p1 = new SSMG_RING_NAME();
                p1.Player = Character;
                netIO.SendPacket(p1);
            }

            if (Character.Ring == null)
                return;
            if (reason != SSMG_RING_INFO.Reason.UPDATED)
            {
                var p = new SSMG_RING_INFO();
                var p1 = new SSMG_RING_NAME();
                p.Ring(Character.Ring, reason);
                p1.Player = Character;
                netIO.SendPacket(p);
                netIO.SendPacket(p1);
                SendRingMember();
            }
            else
            {
                var p = new SSMG_RING_INFO_UPDATE();
                p.RingID = Character.Ring.ID;
                p.Fame = Character.Ring.Fame;
                p.CurrentMember = (byte)Character.Ring.MemberCount;
                p.MaxMember = (byte)Character.Ring.MaxMemberCount;
                netIO.SendPacket(p);
            }
        }

        public void SendRingMemberInfo(ActorPC pc)
        {
            if (Character.Ring == null)
                return;
            if (Character.Ring.IsMember(pc))
                if (pc.Online)
                {
                    var i = (uint)Character.Ring.IndexOf(pc);
                    var p = new SSMG_PARTY_MEMBER_STATE();
                    p.PartyIndex = i;
                    p.CharID = pc.CharID;
                    p.Online = pc.Online;
                    netIO.SendPacket(p);
                    var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                    p2.PartyIndex = i;
                    p2.CharID = pc.CharID;
                    if (Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                    p2.Job = pc.Job;
                    p2.Level = pc.Level;
                    p2.JobLevel = pc.CurrentJobLevel;
                    netIO.SendPacket(p2);
                }
        }

        public void SendRingMemberState(ActorPC pc)
        {
            if (Character.Ring == null) return;
            if (Character.Ring.IsMember(pc))
            {
                var i = Character.Ring.IndexOf(pc);
                var p = new SSMG_PARTY_MEMBER_STATE();
                p.PartyIndex = (uint)i;
                p.CharID = pc.CharID;
                p.Online = pc.Online;
                netIO.SendPacket(p);
            }
        }

        public void SendChatRing(string name, string content)
        {
            var p = new SSMG_CHAT_RING();
            p.Sender = name;
            p.Content = content;
            netIO.SendPacket(p);
        }

        public void SendRingMeDelete(SSMG_RING_QUIT.Reasons reason)
        {
            var p = new SSMG_RING_QUIT();
            p.RingID = Character.Ring.ID;
            p.Reason = reason;
            netIO.SendPacket(p);
        }

        public void SendRingMemberDelete(ActorPC pc)
        {
            var p = new SSMG_RING_MEMBER_INFO();
            p.Member(pc, null);
            netIO.SendPacket(p);
        }
    }
}