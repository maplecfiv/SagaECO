using System;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Packets.Client;
using SagaMap.Packets.Server;
using Version = SagaLib.Version;

namespace SagaMap.Network.Client
{
    public partial class MapClient
    {
        private ActorPC partyPartner;

        public void OnPartyRoll(CSMG_PARTY_ROLL p)
        {
            if (Character.Party == null) return;
            if (Character.Party.Leader != Character) return;

            if (p.status == 1)
            {
                Character.Party.Roll = 0;
                foreach (var item in Character.Party.Members.Values)
                    if (item.Online)
                        FromActorPC(item).SendRollInfo(item);
            }

            if (p.status == 0)
            {
                Character.Party.Roll = 1;
                foreach (var item in Character.Party.Members.Values)
                    if (item.Online)
                        FromActorPC(item).SendRollInfo(item);
            }
        }

        public void SendRollInfo(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p2 = new SSMG_PARTY_ROLL();
                    byte roll = 0;
                    if (pc.Party.Roll == 0) roll = 1;
                    p2.status = roll;
                    netIO.SendPacket(p2);
                }
        }

        public void OnPartyName(CSMG_PARTY_NAME p)
        {
            if (Character.Party == null) return;
            if (p.Name == "") return;
            if (Character.Party.Leader != Character) return;
            Character.Party.Name = p.Name;
            PartyManager.Instance.UpdatePartyName(Character.Party);
        }

        public void OnPartyKick(CSMG_PARTY_KICK p)
        {
            if (Character.Party == null)
                return;
            if (Character.Party.Leader != Character)
                return;
            var p1 = new SSMG_PARTY_KICK();
            if (Character.Party.IsMember(p.CharID))
            {
                PartyManager.Instance.DeleteMember(Character.Party, p.CharID, SSMG_PARTY_DELETE.Result.KICKED);
                p1.Result = 0;
            }
            else
            {
                p1.Result = -1; //指定プレイヤーが存在しません
            }

            netIO.SendPacket(p1);
        }

        public void OnPartyQuit(CSMG_PARTY_QUIT p)
        {
            var p1 = new SSMG_PARTY_QUIT();
            if (Character.Party == null)
            {
                p1.Result = -1; //パーティーに所属していません
            }
            else
            {
                if (Character != Character.Party.Leader)
                    PartyManager.Instance.DeleteMember(Character.Party, Character.CharID,
                        SSMG_PARTY_DELETE.Result.QUIT);
                else
                    PartyManager.Instance.PartyDismiss(Character.Party);
            }

            netIO.SendPacket(p1);
        }

        public void OnPartyInviteAnswer(CSMG_PARTY_INVITE_ANSWER p)
        {
            if (partyPartner == null) return;
            if (partyPartner.CharID != p.CharID) return;
            var client = FromActorPC(partyPartner);
            if ((client.Character.Mode == PlayerMode.KNIGHT_EAST || client.Character.Mode == PlayerMode.KNIGHT_FLOWER ||
                 client.Character.Mode == PlayerMode.KNIGHT_NORTH
                 || client.Character.Mode == PlayerMode.KNIGHT_ROCK ||
                 client.Character.Mode == PlayerMode.KNIGHT_SOUTH || client.Character.Mode == PlayerMode.KNIGHT_WEST)
                && (Character.Mode == PlayerMode.KNIGHT_EAST || Character.Mode == PlayerMode.KNIGHT_FLOWER ||
                    Character.Mode == PlayerMode.KNIGHT_NORTH
                    || Character.Mode == PlayerMode.KNIGHT_ROCK || Character.Mode == PlayerMode.KNIGHT_SOUTH ||
                    Character.Mode == PlayerMode.KNIGHT_WEST)
               )
                if (client.Character.Mode != Character.Mode)
                    return;

            if (client.Character.Party != null)
            {
                if (client.Character.Party.MemberCount >= 8)
                    return;
                PartyManager.Instance.AddMember(client.Character.Party, Character);
                PartnerTalking(Character.Partner, TALK_EVENT.JOINPARTY, 100, 0);
            }
            else
            {
                var party = PartyManager.Instance.CreateParty(partyPartner);
                PartyManager.Instance.AddMember(party, Character);
                PartnerTalking(partyPartner.Partner, TALK_EVENT.JOINPARTY, 100, 0);
                PartnerTalking(client.Character.Partner, TALK_EVENT.JOINPARTY, 100, 0);
            }
        }

        public void OnPartyInvite(CSMG_PARTY_INVITE p)
        {
            var client = MapClientManager.Instance.FindClient(p.CharID);
            var result = CheckPartyInvite(client);
            var p1 = new SSMG_PARTY_INVITE_RESULT();
            p1.InviteResult = result;
            if (result >= 0)
            {
                var p2 = new SSMG_PARTY_INVITE();
                p2.CharID = Character.CharID;
                p2.Name = Character.Name;
                client.partyPartner = Character;
                client.netIO.SendPacket(p2);
            }

            netIO.SendPacket(p1);
        }

        private int CheckPartyInvite(MapClient client)
        {
            if (client == null)
                return -2; //プレイヤーが存在しません
            if (client.scriptThread != null || client.trading)
                return -3; //相手がパーティに誘えない状態になりました
            if (client.Character.Party != null)
            {
                if (Character.Party != null)
                    if (Character.Party.IsMember(client.Character.CharID))
                        return -11; //既にパーティーが存在しています
                return -10; //既にパーティーに所属しています
            }

            if (Character.Party != null)
                if (Character.Party.MemberCount == 8)
                    return -12; //パーティー人数が限界を超えてます
            return 0;
        }

        public void SendPartyInfo()
        {
            if (Character.Party == null)
                return;
            var p = new SSMG_PARTY_INFO();
            p.Party(Character.Party, Character);
            var p1 = new SSMG_PARTY_NAME();
            p1.Party(Character.Party, Character);
            netIO.SendPacket(p);
            netIO.SendPacket(p1);
            SendPartyMember();
        }

        public void SendPartyMeDelete(SSMG_PARTY_DELETE.Result reason)
        {
            var p = new SSMG_PARTY_DELETE();
            p.PartyID = Character.Party.ID;
            p.PartyName = Character.Party.Name;
            p.Reason = reason;
            netIO.SendPacket(p);
        }

        public void SendPartyMemberDelete(uint pc)
        {
            var p = new SSMG_PARTY_MEMBER();
            p.PartyIndex = -1;
            p.CharID = pc;
            p.CharName = "";
            netIO.SendPacket(p);
        }

        public void SendPartyMemberPosition(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p1 = new SSMG_PARTY_MEMBER_POSITION();
                    p1.PartyIndex = Character.Party.IndexOf(pc);
                    p1.CharID = pc.CharID;
                    var mapid = pc.MapID;
                    var map = MapManager.Instance.GetMap(pc.MapID);
                    if (map.returnori)
                        mapid = map.OriID;
                    p1.MapID = mapid;
                    p1.X = Global.PosX16to8(pc.X, MapManager.Instance.GetMap(pc.MapID).Width);
                    p1.Y = Global.PosY16to8(pc.Y, MapManager.Instance.GetMap(pc.MapID).Height);
                    netIO.SendPacket(p1);
                }
        }

        public void SendPartyMemberDeungeonPosition(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (this.map.IsDungeon)
                {
                    var map = MapManager.Instance.GetMap(pc.MapID);
                    if (map.IsDungeon)
                    {
                        var p = new SSMG_PARTY_MEMBER_DUNGEON_POSITION();
                        p.CharID = pc.CharID;
                        p.MapID = map.ID;
                        p.X = map.DungeonMap.X;
                        p.Y = map.DungeonMap.Y;
                        p.Dir = map.DungeonMap.Dir;
                        netIO.SendPacket(p);
                    }
                }
        }

        public void SendPartyMemberDetail(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                {
                    var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                    p2.PartyIndex = Character.Party.IndexOf(pc);
                    p2.CharID = pc.CharID;
                    if (Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                    p2.Job = pc.Job;
                    p2.Level = pc.Level;
                    p2.JobLevel = pc.CurrentJobLevel;
                    netIO.SendPacket(p2);
                }
        }

        public void SendPartyMemberState(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
            {
                var i = Character.Party.IndexOf(pc);
                var p = new SSMG_PARTY_MEMBER_STATE();
                p.PartyIndex = i;
                p.CharID = pc.CharID;
                p.Online = pc.Online;
                netIO.SendPacket(p);
            }
        }

        public void SendPartyMemberHPMPSP(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
            {
                var i = Character.Party.IndexOf(pc);
                var p3 = new SSMG_PARTY_MEMBER_HPMPSP();
                p3.PartyIndex = i;
                p3.CharID = pc.CharID;
                p3.HP = pc.HP;
                p3.MaxHP = pc.MaxHP;
                p3.MP = pc.MP;
                p3.MaxMP = pc.MaxMP;
                p3.SP = pc.SP;
                p3.MaxSP = pc.MaxSP;
                netIO.SendPacket(p3);
            }
        }

        public void SendPartyMemberInfo(ActorPC pc)
        {
            if (Character.Party == null) return;
            if (Character.Party.IsMember(pc))
                if (pc.Online)
                    try
                    {
                        var i = Character.Party.IndexOf(pc);
                        var p = new SSMG_PARTY_MEMBER_STATE();
                        p.PartyIndex = i;
                        p.CharID = pc.CharID;
                        p.Online = pc.Online;
                        netIO.SendPacket(p);
                        var p1 = new SSMG_PARTY_MEMBER_POSITION();
                        p1.PartyIndex = i;
                        p1.CharID = pc.CharID;
                        p1.MapID = pc.MapID;
                        p1.X = Global.PosX16to8(pc.X, MapManager.Instance.GetMap(pc.MapID).Width);
                        p1.Y = Global.PosY16to8(pc.Y, MapManager.Instance.GetMap(pc.MapID).Height);
                        netIO.SendPacket(p1);
                        var p2 = new SSMG_PARTY_MEMBER_DETAIL();
                        p2.PartyIndex = i;
                        p2.CharID = pc.CharID;
                        if (Configuration.Instance.Version >= Version.Saga10) p2.Form = 0;
                        p2.Job = pc.Job;
                        p2.Level = pc.Level;
                        p2.JobLevel = pc.CurrentJobLevel;
                        netIO.SendPacket(p2);
                        var p3 = new SSMG_PARTY_MEMBER_HPMPSP();
                        p3.PartyIndex = i;
                        p3.CharID = pc.CharID;
                        p3.HP = pc.HP;
                        p3.MaxHP = pc.MaxHP;
                        p3.MP = pc.MP;
                        p3.MaxMP = pc.MaxMP;
                        p3.SP = pc.SP;
                        p3.MaxSP = pc.MaxSP;
                        netIO.SendPacket(p3);
                    }
                    catch (Exception ex)
                    {
                        Logger.ShowError(ex);
                    }
        }

        private void SendPartyMember()
        {
            if (Character.Party == null)
                return;
            foreach (var i in Character.Party.Members.Keys)
            {
                var p = new SSMG_PARTY_MEMBER();
                p.PartyIndex = i;
                p.CharID = Character.Party[i].CharID;
                p.CharName = Character.Party[i].Name;
                p.Leader = Character.Party.Leader == Character.Party[i];
                netIO.SendPacket(p);
            }

            var party = Character.Party;
            foreach (var i in party.Members.Keys) SendPartyMemberInfo(party[i]);
        }
    }
}