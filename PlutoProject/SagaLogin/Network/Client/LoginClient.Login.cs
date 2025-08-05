using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaLogin.Configurations;
using SagaLogin.Manager;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public ActorPC selectedChar;

        public void OnSendVersion(CSMG_SEND_VERSION p)
        {
            Logger.ShowInfo("Client(Version:" + p.GetVersion() + ") is trying to connect...");
            client_Version = p.GetVersion();

            var args = "FF FF E8 6A 6A CA DC E8 06 05 2B 29 F8 96 2F 86 7C AB 2A 57 AD 30";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var p3 = new Packet();
            p3.data = buf;
            netIO.SendPacket(p3);

            var p1 = new SSMG_VERSION_ACK();
            p1.SetResult(SSMG_VERSION_ACK.Result.OK);
            p1.SetVersion(client_Version);
            netIO.SendPacket(p1);
            //Official HK server will now request for Hackshield GUID check , we don't know its algorithms, so not implemented
            var p2 = new SSMG_LOGIN_ALLOWED();
            frontWord = (uint)Global.Random.Next();
            backWord = (uint)Global.Random.Next();
            p2.FrontWord = frontWord;
            p2.BackWord = backWord;
            netIO.SendPacket(p2);

            var pn = new SSMG_REQUEST_NYA();
            netIO.SendPacket(pn);
        }

        public void OnSendGUID(CSMG_SEND_GUID p)
        {
            var p1 = new SSMG_LOGIN_ALLOWED();
            netIO.SendPacket(p1);
        }

        public void OnPing(CSMG_PING p)
        {
            var p1 = new SSMG_PONG();
            netIO.SendPacket(p1);
        }

        public void OnLogin(CSMG_LOGIN p)
        {
            p.GetContent();
            if (MapServerManager.Instance.MapServers.Count == 0)
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
                netIO.SendPacket(p1);
                return;
            }

            if (LoginServer.accountDB.CheckPassword(p.UserName, p.Password, frontWord, backWord))
            {
                var tmp = LoginServer.accountDB.GetUser(p.UserName);

                if (LoginClientManager.Instance.FindClientAccount(p.UserName) != null && tmp.GMLevel == 0)
                {
                    LoginClientManager.Instance.FindClientAccount(p.UserName).netIO.Disconnect();
                    var p2 = new SSMG_LOGIN_ACK();
                    p2.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_ALREADY;
                    netIO.SendPacket(p2);
                    return;
                }

                account = tmp;
                if (account.Banned)
                {
                    var p2 = new SSMG_LOGIN_ACK();
                    p2.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BFALOCK;
                    netIO.SendPacket(p2);
                    return;
                }

                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.OK;
                netIO.SendPacket(p1);

                account.LastIP = netIO.sock.RemoteEndPoint.ToString().Split(':')[0];
                account.MacAddress = p.MacAddress;


                var charIDs = LoginServer.charDB.GetCharIDs(account.AccountID);

                account.Characters = new List<ActorPC>();
                var maxtime = DateTime.Now;
                for (var i = 0; i < charIDs.Length; i++)
                {
                    var pc = LoginServer.charDB.GetChar(charIDs[i], false);
                    if (pc.QuestNextResetTime > maxtime)
                        maxtime = pc.QuestNextResetTime;
                    if (pc.QuestNextResetTime < DateTime.Now)
                    {
                        if ((DateTime.Now - pc.QuestNextResetTime).TotalDays > 1000)
                        {
                            pc.QuestNextResetTime = DateTime.Now + new TimeSpan(1, 0, 0, 0);
                        }
                        else
                        {
                            var days = (int)(DateTime.Now - pc.QuestNextResetTime).TotalDays;
                            pc.QuestRemaining += (ushort)((days + 1) * 5);
                            if (pc.QuestRemaining > 500)
                                pc.QuestRemaining = 500;
                        }
                    }

                    account.Characters.Add(pc);
                }

                var names = "";
                foreach (var item in account.Characters)
                {
                    item.QuestNextResetTime = maxtime;
                    names += item.Name + ",";
                }

                account.PlayerNames = names;
                LoginServer.accountDB.WriteUser(account);
                SendCharData();
            }
            else
            {
                var p1 = new SSMG_LOGIN_ACK();
                p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BADPASS;
                netIO.SendPacket(p1);
            }
        }

        private bool checkHairStyle(CSMG_CHAR_CREATE p)
        {
            if (p.Gender == PC_GENDER.FEMALE)
            {
                if (p.HairStyle > 9 && p.HairStyle != 14)
                    return false;
                return true;
            }

            if (p.HairStyle > 9)
                return false;
            return true;
        }

        private bool checkHairColor(CSMG_CHAR_CREATE p)
        {
            if (p.Race == PC_RACE.DOMINION)
            {
                if (p.HairColor >= 70 && p.HairColor <= 72)
                    return true;
                return false;
            }

            if (p.Race == PC_RACE.EMIL)
            {
                if (p.HairColor >= 50 && p.HairColor <= 52)
                    return true;
                return false;
            }

            if (p.Race == PC_RACE.TITANIA)
            {
                if (p.HairColor == 7 || p.HairColor == 60 || p.HairColor == 61 || p.HairColor == 62)
                    return true;
                return false;
            }

            return true;
        }

        public void OnCharCreate(CSMG_CHAR_CREATE p)
        {
            var p1 = new SSMG_CHAR_CREATE_ACK();

            if (p.Race != PC_RACE.DEM)
                if (!checkHairColor(p) || !checkHairStyle(p))
                {
                    account.Banned = true;
                    netIO.Disconnect();
                    LoginServer.accountDB.WriteUser(account);
                    return;
                }

            if (LoginServer.charDB.CharExists(p.Name))
            {
                p1.CreateResult = SSMG_CHAR_CREATE_ACK.Result.GAME_SMSG_CHRCREATE_E_NAME_CONFLICT;
            }
            else
            {
                var slot =
                    from a in account.Characters
                    where a.Slot == p.Slot
                    select a;
                if (slot.Count() != 0)
                {
                    p1.CreateResult = SSMG_CHAR_CREATE_ACK.Result.GAME_SMSG_CHRCREATE_E_ALREADY_SLOT;
                }
                else
                {
                    var pc = new ActorPC();
                    pc.Name = p.Name;
                    pc.Face = p.Face;
                    pc.Gender = p.Gender;
                    pc.HairColor = p.HairColor;
                    pc.HairStyle = p.HairStyle;
                    pc.Race = p.Race;
                    pc.Slot = p.Slot;
                    pc.Wig = 0xFF;
                    pc.Level = 1;
                    pc.JobLevel1 = 1;
                    pc.JobLevel2T = 1;
                    pc.JobLevel2X = 1;
                    pc.QuestRemaining = 3;
                    pc.EP = 100;
                    pc.MapID = Configuration.Instance.StartupSetting[pc.Race].StartMap;
                    //MapInfo info = MapInfoFactory.Instance.MapInfo[pc.MapID];
                    pc.X2 = Configuration.Instance.StartupSetting[pc.Race].X;
                    pc.Y2 = Configuration.Instance.StartupSetting[pc.Race].Y;

                    pc.Dir = 2;
                    pc.HP = 120;
                    pc.MaxHP = 120;
                    pc.MP = 120;
                    pc.MaxMP = 220;
                    pc.SP = 100;
                    pc.MaxSP = 100;

                    pc.Str = Configuration.Instance.StartupSetting[pc.Race].Str;
                    pc.Dex = Configuration.Instance.StartupSetting[pc.Race].Dex;
                    pc.Int = Configuration.Instance.StartupSetting[pc.Race].Int;
                    pc.Vit = Configuration.Instance.StartupSetting[pc.Race].Vit;
                    pc.Agi = Configuration.Instance.StartupSetting[pc.Race].Agi;
                    pc.Mag = Configuration.Instance.StartupSetting[pc.Race].Mag;
                    pc.SkillPoint = 3;
                    pc.StatsPoint = 2;
                    pc.Gold = 0;


                    pc.CInt["canTrade"] = 1;
                    pc.CInt["canParty"] = 1;
                    pc.CInt["canPossession"] = 1;
                    pc.CInt["canRing"] = 1;
                    pc.CInt["showRevive"] = 1;
                    pc.CInt["canWork"] = 1;
                    pc.CInt["canMentor"] = 1;
                    pc.CInt["showEquipment"] = 1;
                    pc.CInt["canChangePartnerDisplay"] = 1;
                    pc.CInt["canFriend"] = 1;

                    List<StartItem> lists;
                    lists = Configuration.Instance.StartItem[pc.Race][pc.Gender];
                    foreach (var i in lists)
                    {
                        var item = ItemFactory.Instance.GetItem(i.ItemID);
                        item.Stack = i.Count;
                        pc.Inventory.AddItem(i.Slot, item);
                    }

                    LoginServer.charDB.CreateChar(pc, account.AccountID);

                    account.Characters.Add(pc);
                    p1.CreateResult = SSMG_CHAR_CREATE_ACK.Result.OK;
                }
            }

            netIO.SendPacket(p1);
            SendCharData();
        }

        public void OnCharDelete(CSMG_CHAR_DELETE p)
        {
            var p1 = new SSMG_CHAR_DELETE_ACK();
            var chr =
                from c in account.Characters
                where c.Slot == p.Slot
                select c;
            var pc = chr.First();
            if (account.DeletePassword.ToLower() == p.DeletePassword.ToLower())
            {
                LoginServer.charDB.DeleteChar(pc);
                account.Characters.Remove(pc);
                p1.DeleteResult = SSMG_CHAR_DELETE_ACK.Result.OK;
            }
            else
            {
                p1.DeleteResult = SSMG_CHAR_DELETE_ACK.Result.WRONG_DELETE_PASSWORD;
            }

            netIO.SendPacket(p1);
            SendCharData();
        }

        public void OnCharSelect(CSMG_CHAR_SELECT p)
        {
            var p1 = new SSMG_CHAR_SELECT_ACK();
            var chr =
                from c in account.Characters
                where c.Slot == p.Slot
                select c;
            var pc = chr.First();
            selectedChar = pc;
            selectedChar.Account = account;
            p1.MapID = pc.MapID;
            netIO.SendPacket(p1);
        }

        public void OnRequestMapServer(CSMG_REQUEST_MAP_SERVER p)
        {
            var p1 = new SSMG_SEND_TO_MAP_SERVER();

            if (MapServerManager.Instance.MapServers.ContainsKey(selectedChar.MapID))
            {
                var server = MapServerManager.Instance.MapServers[selectedChar.MapID];
                p1.ServerID = 1;
                p1.IP = server.IP;
                p1.Port = server.port;
            }
            else
            {
                if (MapServerManager.Instance.MapServers.ContainsKey(selectedChar.MapID / 1000 * 1000))
                {
                    var server = MapServerManager.Instance.MapServers[selectedChar.MapID / 1000 * 1000];
                    p1.ServerID = 1;
                    p1.IP = server.IP;
                    p1.Port = server.port;
                }
                else
                {
                    Logger.ShowWarning("No map server registered for mapID:" + selectedChar.MapID);
                    p1.ServerID = 255;
                    p1.IP = "127.0.0.001";
                    p1.Port = 10000;
                }
            }

            netIO.SendPacket(p1);
        }

        public void OnCharStatus(CSMG_CHAR_STATUS p)
        {
            var args = "00 2b";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var ps1 = new Packet();
            ps1.data = buf;
            netIO.SendPacket(ps1);

            var p1 = new SSMG_CHAR_STATUS();
            netIO.SendPacket(p1);
            SendFriendList();
            SendStatusToFriends();

            args = "00 de";
            buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var ps = new Packet();
            ps.data = buf;
            netIO.SendPacket(ps);

            SendGifts();
            SendMails();
        }

        private void SendCharData()
        {
            var p2 = new SSMG_CHAR_DATA();
            p2.Chars = account.Characters;
            netIO.SendPacket(p2);
            //Logger.ShowInfo(this.netIO.DumpData(p2));

            var p3 = new SSMG_CHAR_EQUIP();
            p3.Characters = account.Characters;
            netIO.SendPacket(p3);
        }

        public void OnNya(CSMG_NYASHIELD_VERSION p)
        {
            if (p.ver != 1)
            {
                var p1 = new SSMG_NYA_WRONG_VERSION();
                netIO.SendPacket(p1);
            }
        }

        private void SendSystemMessages(string message, short type)
        {
            var p = new SSMG_CHAT_SYSTEM_MESSAGE();
            p.Type = (SSMG_CHAT_SYSTEM_MESSAGE.MessageType)type;
            p.Content = message;
            netIO.SendPacket(p);
        }

        private void SendWelcomeMessages()
        {
            SendSystemMessages("------------------------------------------------------", 0);
            foreach (var motd in Configuration.Instance.Motd)
                SendSystemMessages(motd, 0);
            SendSystemMessages("------------------------------------------------------", 0);
        }
    }
}