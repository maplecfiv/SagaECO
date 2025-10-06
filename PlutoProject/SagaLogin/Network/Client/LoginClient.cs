using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using SagaDB;
using SagaDB.Actor;
using SagaDB.BBS;
using SagaDB.Config;
using SagaDB.Item;
using SagaDB.Tamaire;
using SagaLib;
using SagaLogin.Manager;
using SagaLogin.Packets.Client.Chat;
using SagaLogin.Packets.Client.FriendList;
using SagaLogin.Packets.Client.Login;
using SagaLogin.Packets.Client.NyaShield;
using SagaLogin.Packets.Client.Ring;
using SagaLogin.Packets.Client.Tamaire;
using SagaLogin.Packets.Client.Tool;
using SagaLogin.Packets.Client.WRP;
using SagaLogin.Packets.Map;
using SagaLogin.Packets.Server.Chat;
using SagaLogin.Packets.Server.FriendList;
using SagaLogin.Packets.Server.Login;
using SagaLogin.Packets.Server.Mail;
using SagaLogin.Packets.Server.NyaShield;
using SagaLogin.Packets.Server.Ring;
using SagaLogin.Packets.Server.Tamaire;
using SagaLogin.Packets.Server.Tool;
using SagaLogin.Packets.Server.WRP;
using Version = SagaLib.Version;

namespace SagaLogin.Network.Client;

public class LoginClient : SagaLib.Client
{
    public enum SESSION_STATE
    {
        LOGIN,
        MAP,
        REDIRECTING,
        DISCONNECTED
    }

    public Account account;
    private string client_Version;

    public uint currentMap;
    public CharStatus currentStatus = CharStatus.ONLINE;
    private LoginClient friendTarget;

    private uint frontWord, backWord;

    public bool IsMapServer;
    public PC_JOB job;
    public byte lv, joblv;

    public ActorPC selectedChar;

    private MapServer server;
    public SESSION_STATE state;

    public LoginClient(Socket mSock, Dictionary<ushort, Packet> mCommandTable)
    {
        NetIo = new NetIO(mSock, mCommandTable, this);
        if (Configuration.Configuration.Instance.Version >= Version.Saga11)
            NetIo.FirstLevelLength = 2;
        NetIo.SetMode(NetIO.Mode.Server);
        if (NetIo.sock.Connected) OnConnect();
    }

    public void OnGetGiftsRequest(TOOL_GIFTS p)
    {
        if (account.GMLevel < 250) return;
        var Type = p.type;
        var Title = p.Title;
        var Content = p.Content;
        var AcccountIDs = p.CharIDs;
        var GiftIDs = p.GiftIDs;
        var Days = p.Days;

        try
        {
            var Gifts = new Dictionary<uint, ushort>();
            GiftIDs = GiftIDs.Replace("\r\n", "@");
            var paras = GiftIDs.Split('@');
            for (var i = 0; i < paras.Length; i++)
            {
                var pa = paras[i].Split(',');
                Gifts.Add(uint.Parse(pa[0]), ushort.Parse(pa[1]));
            }

            var Recipients = new Dictionary<string, uint>();
            if (Type == 1) //发给制定账号
            {
                AcccountIDs = AcccountIDs.Replace(",", "@");
                paras = AcccountIDs.Split('@');

                for (var i = 0; i < paras.Length; i++)
                {
                    var ID = uint.Parse(paras[i]);
                    Recipients.Add(ID.ToString(), ID);
                }
            }
            else if (Type == 5)
            {
                AcccountIDs = AcccountIDs.Replace(",", "@");
                paras = AcccountIDs.Split('@');
                for (var i = 0; i < paras.Length; i++)
                {
                    var Accounts = LoginServer.accountDB.GetAllAccount();
                    var a = new Account();
                    foreach (var item in Accounts)
                        if (item.Name == paras[i])
                        {
                            a = item;
                            break;
                        }

                    if (a != null)
                    {
                        var ID = (uint)a.AccountID;
                        Recipients.Add(ID.ToString(), ID);
                    }
                }
            }

            else if (Type == 2 || Type == 12) //发送给在线账号
            {
                var aids = LoginClientManager.Instance.FindAllOnlineAccounts();
                var sd = new Dictionary<string, DateTime>();
                foreach (var item in aids)
                {
                    var ID = (uint)item.account.AccountID;
                    if (Type == 2)
                    {
                        Recipients.Add(ID.ToString(), ID);
                    }
                    else if (Type == 12)
                    {
                        if (Recipients.ContainsKey(item.account.LastIP))
                        {
                            if (sd[item.account.LastIP] < item.account.lastLoginTime)
                            {
                                Recipients[item.account.LastIP] = ID;
                                sd[item.account.LastIP] = item.account.lastLoginTime;
                            }
                        }
                        else
                        {
                            Recipients.Add(item.account.LastIP, ID);
                            sd.Add(item.account.LastIP, item.account.lastLoginTime);
                        }
                    }
                }
            }
            else if (Type == 3 || Type == 13) //发送给所有账号
            {
                var Accounts = LoginServer.accountDB.GetAllAccount();
                var sd = new Dictionary<string, DateTime>();
                foreach (var item in Accounts)
                {
                    var ID = (uint)item.AccountID;
                    if (Type == 3)
                    {
                        Recipients.Add(ID.ToString(), ID);
                    }
                    else if (Type == 13)
                    {
                        if (Recipients.ContainsKey(item.LastIP))
                        {
                            if (sd[item.LastIP] < item.lastLoginTime)
                            {
                                Recipients[item.LastIP] = ID;
                                sd[item.LastIP] = item.lastLoginTime;
                            }
                        }
                        else
                        {
                            Recipients.Add(item.LastIP, ID);
                            sd.Add(item.LastIP, item.lastLoginTime);
                        }
                    }
                }
            }
            else if (Type == 4 || Type == 14) //发送给N天内登录过的账号
            {
                var day = int.Parse(Days);
                var Accounts = LoginServer.accountDB.GetAllAccount();
                var sd = new Dictionary<string, DateTime>();
                foreach (var item in Accounts)
                    if ((DateTime.Now - item.lastLoginTime).Days <= day)
                    {
                        var ID = (uint)item.AccountID;
                        if (Type == 4)
                        {
                            Recipients.Add(ID.ToString(), ID);
                        }
                        else if (Type == 14)
                        {
                            if (Recipients.ContainsKey(item.LastIP))
                            {
                                if (sd[item.LastIP] < item.lastLoginTime)
                                {
                                    Recipients[item.LastIP] = ID;
                                    sd[item.LastIP] = item.lastLoginTime;
                                }
                            }
                            else
                            {
                                Recipients.Add(item.LastIP, ID);
                                sd.Add(item.LastIP, item.lastLoginTime);
                            }
                        }
                    }
            }

            foreach (var item in Recipients.Values)
            {
                var gift = new Gift();
                gift.AccountID = item;
                gift.Date = DateTime.Now;
                gift.Items = Gifts;
                gift.Name = Title;
                gift.Title = Content;
                AddGift(gift);
            }

            SendResult(0, "礼物发送成功！");


            var log = new Logger("礼物记录.txt");
            var logtext = "\r\n-操作者账号：" + account.AccountID;
            logtext += "\r\n-类型：" + Type;
            logtext += "\r\n-标题：" + Title;
            logtext += "\r\n-内容：" + Content;
            logtext += "\r\n-接收者人数：" + Recipients.Count;
            if (Type == 4)
                logtext += "\r\n-设定的天数：" + Days;
            logtext += "\r\n-物品：";
            foreach (var item in Gifts.Keys)
                logtext += "\r\n   -ID:" + item + "   -Stack:" + Gifts[item];
            if (Type == 1)
            {
                logtext += "\r\n-接收者ID:\r\n   ";
                foreach (var item in Recipients.Values)
                    logtext += item + ",";
            }

            logtext += "\r\n=======================================================\r\n";
            log.WriteLog(logtext);
        }
        catch (Exception ex)
        {
            SendResult(1, "礼物处理失败！" + ex.Message);
            Logger.ShowError(ex);
        }
    }

    public void SendResult(byte type, string text)
    {
        var p = new SSMG_TOOL_RESULT();
        p.type = type;
        p.Text = text;
        NetIo.SendPacket(p);
    }

    public void AddGift(Gift gift)
    {
        var ID = LoginServer.charDB.AddNewGift(gift);
        var client = LoginClientManager.Instance.FindClientAccountID(gift.AccountID);
        gift.MailID = ID;
        if (client != null)
            client.SendSingleGift(gift);
    }

    public void SendSingMail(Mail mail)
    {
        if (selectedChar == null) return;
        var p = new SSMG_MAIL();
        p.mail = mail;
        NetIo.SendPacket(p);
    }

    public void SendMails()
    {
        if (selectedChar == null) return;
        if (selectedChar.Mails != null && selectedChar.Mails.Count >= 1)
            for (var i = 0; i < selectedChar.Mails.Count; i++)
            {
                var p = new SSMG_MAIL();
                p.mail = selectedChar.Mails[i];
                NetIo.SendPacket(p);
            }
    }

    public void SendSingleGift(Gift gift)
    {
        if (selectedChar == null) return;
        var p = new SSMG_GIFT();
        p.mails = gift;
        NetIo.SendPacket(p);
    }

    public void SendGifts()
    {
        if (selectedChar == null) return;
        LoginServer.charDB.GetGifts(selectedChar);
        for (var i = 0; i < selectedChar.Gifts.Count; i++)
        {
            var p = new SSMG_GIFT();
            p.mails = selectedChar.Gifts[i];
            NetIo.SendPacket(p);
        }
    }

    public void OnTamaireListRequest(CSMG_TAMAIRE_LIST_REQUEST p)
    {
        var p1 = new SSMG_TAMAIRE_LIST();
        int a;
        var data = GetLendings(p.JobType, false, false, p.minlevel, p.maxlevel, p.page, out a);
        p1.PutData(data, selectedChar.Level);
        NetIo.SendPacket(p1);
    }

    public List<TamaireLending> GetLendings(byte jobtype, bool isFriendOnly, bool isRingOnly, byte minlevel,
        byte maxlevel, int page, out int maxPage)
    {
        var items = LoginServer.charDB.GetTamaireLendings();
        var query = from lending in items
            where lending.Baselv >= minlevel && lending.Baselv <= maxlevel
                                             && LoginServer.charDB.GetAccountID(lending.Lender) != account.AccountID
                                             && DateTime.Now < lending.PostDue
                                             && lending.MaxLendings > lending.Renters.Count
            select lending;
        var list = query.ToList();
        if (selectedChar.TamaireRental != null)
            list = (from lending in list
                where lending.Lender != selectedChar.TamaireRental.LastLender
                select lending).ToList();
        if (jobtype != 0xFF)
            list = (from lending in list where lending.JobType == jobtype select lending).ToList();
        if (isFriendOnly)
            list = (from lending in list
                where LoginServer.charDB.GetFriendList(selectedChar)
                    .Contains(LoginServer.charDB.GetChar(lending.Lender))
                select lending).ToList();
        if (isRingOnly)
            list = (from lending in list
                where LoginServer.charDB.GetChar(lending.Lender).Ring == selectedChar.Ring
                select lending).ToList();
        if (list.Count % 10 == 0)
            maxPage = list.Count / 10;
        else
            maxPage = list.Count / 10 + 1;

        var rentals = (from lending in list
            where list.IndexOf(lending) >= page * 10 && list.IndexOf(lending) < (page + 1) * 10
            select lending).ToList();
        return rentals;
    }

    public void OnSendVersion(CSMG_SEND_VERSION p)
    {
        Logger.ShowInfo("Client(Version:" + p.GetVersion() + ") is trying to connect...");
        client_Version = p.GetVersion();

        var args = "FF FF E8 6A 6A CA DC E8 06 05 2B 29 F8 96 2F 86 7C AB 2A 57 AD 30";
        var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
        var p3 = new Packet();
        p3.data = buf;
        NetIo.SendPacket(p3);

        var p1 = new SSMG_VERSION_ACK();
        p1.SetResult(SSMG_VERSION_ACK.Result.OK);
        p1.SetVersion(client_Version);
        NetIo.SendPacket(p1);
        //Official HK server will now request for Hackshield GUID check , we don't know its algorithms, so not implemented
        var p2 = new SSMG_LOGIN_ALLOWED();
        frontWord = (uint)Global.Random.Next();
        backWord = (uint)Global.Random.Next();
        p2.FrontWord = frontWord;
        p2.BackWord = backWord;
        NetIo.SendPacket(p2);

        var pn = new SSMG_REQUEST_NYA();
        NetIo.SendPacket(pn);
    }

    public void OnSendGUID(CSMG_SEND_GUID p)
    {
        var p1 = new SSMG_LOGIN_ALLOWED();
        NetIo.SendPacket(p1);
    }

    public void OnPing(CSMG_PING p)
    {
        var p1 = new SSMG_PONG();
        NetIo.SendPacket(p1);
    }

    public void OnLogin(CSMG_LOGIN p)
    {
        p.GetContent();
        if (MapServerManager.Instance.MapServers.Count == 0)
        {
            var p1 = new SSMG_LOGIN_ACK();
            p1.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_IPBLOCK;
            NetIo.SendPacket(p1);
            return;
        }

        if (LoginServer.accountDB.CheckPassword(p.UserName, p.Password, frontWord, backWord))
        {
            var tmp = LoginServer.accountDB.GetUser(p.UserName);

            if (LoginClientManager.Instance.FindClientAccount(p.UserName) != null && tmp.GMLevel == 0)
            {
                LoginClientManager.Instance.FindClientAccount(p.UserName).NetIo.Disconnect();
                var p2 = new SSMG_LOGIN_ACK();
                p2.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_ALREADY;
                NetIo.SendPacket(p2);
                return;
            }

            account = tmp;
            if (account.Banned)
            {
                var p2 = new SSMG_LOGIN_ACK();
                p2.LoginResult = SSMG_LOGIN_ACK.Result.GAME_SMSG_LOGIN_ERR_BFALOCK;
                NetIo.SendPacket(p2);
                return;
            }

            var p1 = new SSMG_LOGIN_ACK();
            p1.LoginResult = SSMG_LOGIN_ACK.Result.OK;
            NetIo.SendPacket(p1);

            account.LastIP = NetIo.sock.RemoteEndPoint.ToString().Split(':')[0];
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
            NetIo.SendPacket(p1);
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
                NetIo.Disconnect();
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
                pc.MapID = Configuration.Configuration.Instance.StartupSetting[pc.Race].StartMap;
                //MapInfo info = MapInfoFactory.Instance.MapInfo[pc.MapID];
                pc.X2 = Configuration.Configuration.Instance.StartupSetting[pc.Race].X;
                pc.Y2 = Configuration.Configuration.Instance.StartupSetting[pc.Race].Y;

                pc.Dir = 2;
                pc.HP = 120;
                pc.MaxHP = 120;
                pc.MP = 120;
                pc.MaxMP = 220;
                pc.SP = 100;
                pc.MaxSP = 100;

                pc.Str = Configuration.Configuration.Instance.StartupSetting[pc.Race].Str;
                pc.Dex = Configuration.Configuration.Instance.StartupSetting[pc.Race].Dex;
                pc.Int = Configuration.Configuration.Instance.StartupSetting[pc.Race].Int;
                pc.Vit = Configuration.Configuration.Instance.StartupSetting[pc.Race].Vit;
                pc.Agi = Configuration.Configuration.Instance.StartupSetting[pc.Race].Agi;
                pc.Mag = Configuration.Configuration.Instance.StartupSetting[pc.Race].Mag;
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
                lists = Configuration.Configuration.Instance.StartItem[pc.Race][pc.Gender];
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

        NetIo.SendPacket(p1);
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

        NetIo.SendPacket(p1);
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
        NetIo.SendPacket(p1);
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

        NetIo.SendPacket(p1);
    }

    public void OnCharStatus(CSMG_CHAR_STATUS p)
    {
        var args = "00 2b";
        var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
        var ps1 = new Packet();
        ps1.data = buf;
        NetIo.SendPacket(ps1);

        var p1 = new SSMG_CHAR_STATUS();
        NetIo.SendPacket(p1);
        SendFriendList();
        SendStatusToFriends();

        args = "00 de";
        buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
        var ps = new Packet();
        ps.data = buf;
        NetIo.SendPacket(ps);

        SendGifts();
        SendMails();
    }

    private void SendCharData()
    {
        var p2 = new SSMG_CHAR_DATA();
        p2.Chars = account.Characters;
        NetIo.SendPacket(p2);
        //Logger.ShowInfo(this.netIO.DumpData(p2));

        var p3 = new SSMG_CHAR_EQUIP();
        p3.Characters = account.Characters;
        NetIo.SendPacket(p3);
    }

    public void OnNya(CSMG_NYASHIELD_VERSION p)
    {
        if (p.ver != 1)
        {
            var p1 = new SSMG_NYA_WRONG_VERSION();
            NetIo.SendPacket(p1);
        }
    }

    private void SendSystemMessages(string message, short type)
    {
        var p = new SSMG_CHAT_SYSTEM_MESSAGE();
        p.Type = (SSMG_CHAT_SYSTEM_MESSAGE.MessageType)type;
        p.Content = message;
        NetIo.SendPacket(p);
    }

    private void SendWelcomeMessages()
    {
        SendSystemMessages("------------------------------------------------------", 0);
        foreach (var motd in Configuration.Configuration.Instance.Motd)
            SendSystemMessages(motd, 0);
        SendSystemMessages("------------------------------------------------------", 0);
    }

    public void OnRingEmblemNew(CSMG_RING_EMBLEM_NEW p)
    {
        var result = LoginServer.charDB.GetRingEmblem(p.RingID, new DateTime(1970, 1, 1));
        SendRingEmblem(p.RingID, result.Data, result.NeedUpdate, result.NewTime);
    }

    public void OnRingEmblem(CSMG_RING_EMBLEM p)
    {
        var result = LoginServer.charDB.GetRingEmblem(p.RingID, p.UpdateTime);
        SendRingEmblem(p.RingID, result.Data, result.NeedUpdate, result.NewTime);
    }

    private void SendRingEmblem(uint ringid, byte[] data, bool needUpdate, DateTime newDate)
    {
        var p = new SSMG_RING_EMBLEM();
        if (needUpdate)
            p.Result = 0;
        else
            p.Result = 1;
        p.RingID = ringid;
        if (data != null)
        {
            p.Result2 = 0;
            if (needUpdate)
                p.Data = data;
            p.UpdateTime = newDate;
        }
        else
        {
            p.Result2 = 1;
        }

        NetIo.SendPacket(p);
    }

    public override string ToString()
    {
        try
        {
            if (NetIo != null) return NetIo.sock.RemoteEndPoint.ToString();
            return "LoginClient";
        }
        catch (Exception exception)
        {
            Logger.ShowError(exception, null);
            return "LoginClient";
        }
    }

    public override void OnConnect()
    {
    }

    public override void OnDisconnect()
    {
        if (currentStatus != CharStatus.OFFLINE)
        {
            if (IsMapServer)
            {
                Logger.ShowWarning("A map server has just disconnected...");
                foreach (var i in server.HostedMaps)
                    if (MapServerManager.Instance.MapServers.ContainsKey(i))
                        MapServerManager.Instance.MapServers.Remove(i);
            }
            else
            {
                currentStatus = CharStatus.OFFLINE;
                currentMap = 0;
                try
                {
                    SendStatusToFriends();
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex);
                }

                if (account != null)
                    Logger.ShowInfo(account.Name + " logged out.");
            }
        }

        if (LoginClientManager.Instance.Clients.Contains(this))
            LoginClientManager.Instance.Clients.Remove(this);
    }

    public void OnWRPRequest(CSMG_WRP_REQUEST p)
    {
        var p1 = new SSMG_WRP_LIST();
        p1.RankingList = LoginServer.charDB.GetWRPRanking();
        NetIo.SendPacket(p1);
    }

    public void OnFriendDelete(CSMG_FRIEND_DELETE p)
    {
        if (selectedChar == null)
            return;
        LoginServer.charDB.DeleteFriend(selectedChar.CharID, p.CharID);
        LoginServer.charDB.DeleteFriend(p.CharID, selectedChar.CharID);
        var p1 = new SSMG_FRIEND_DELETE();
        p1.CharID = p.CharID;
        NetIo.SendPacket(p1);
        var client = LoginClientManager.Instance.FindClient(p.CharID);
        if (client != null)
        {
            p1 = new SSMG_FRIEND_DELETE();
            p1.CharID = selectedChar.CharID;
            client.NetIo.SendPacket(p1);
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
                client.NetIo.SendPacket(p1);
            }
            else
            {
                var p1 = new SSMG_FRIEND_ADD_FAILED();
                p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.TARGET_REFUSED;
                NetIo.SendPacket(p1);
            }
        }
        else
        {
            var p1 = new SSMG_FRIEND_ADD_FAILED();
            p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.CANNOT_FIND_TARGET;
            NetIo.SendPacket(p1);
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
            NetIo.SendPacket(p1);
            return;
        }

        if (p.Reply == 1)
        {
            var p1 = new SSMG_FRIEND_ADD_OK();
            p1.CharID = friendTarget.selectedChar.CharID;
            NetIo.SendPacket(p1);
            SendFriendAdd(friendTarget);
            LoginServer.charDB.AddFriend(selectedChar, friendTarget.selectedChar.CharID);
            p1 = new SSMG_FRIEND_ADD_OK();
            p1.CharID = selectedChar.CharID;
            friendTarget.NetIo.SendPacket(p1);
            friendTarget.SendFriendAdd(this);
            LoginServer.charDB.AddFriend(friendTarget.selectedChar, selectedChar.CharID);
        }
        else
        {
            var p1 = new SSMG_FRIEND_ADD_FAILED();
            p1.AddResult = SSMG_FRIEND_ADD_FAILED.Result.TARGET_REFUSED;
            friendTarget.NetIo.SendPacket(p1);
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
                client.NetIo.SendPacket(p1);
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
                client.NetIo.SendPacket(p1);
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
        NetIo.SendPacket(p);
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
            NetIo.SendPacket(p);
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
                client.NetIo.SendPacket(p1);
                client.NetIo.SendPacket(p2);
            }
        }
    }

    public void OnChatWhisper(CSMG_CHAT_WHISPER p)
    {
        if (selectedChar == null) return;
        var client = LoginClientManager.Instance.FindClient(p.Receiver);
        if (client != null)
        {
            var p1 = new SSMG_CHAT_WHISPER();
            p1.Sender = selectedChar.Name;
            p1.Content = p.Content;
            client.NetIo.SendPacket(p1);
        }
        else
        {
            var p1 = new SSMG_CHAT_WHISPER_FAILED();
            p1.Receiver = p.Receiver;
            p1.Result = 0xFFFFFFFF;
            NetIo.SendPacket(p1);
        }
    }

    public void OnInternMapRequestConfig(INTERN_LOGIN_REQUEST_CONFIG p)
    {
        Configuration.Configuration.Instance.Version = p.Version;
        var p1 = new INTERN_LOGIN_REQUEST_CONFIG_ANSWER();
        p1.AuthOK = server.Password == Configuration.Configuration.Instance.Password;
        p1.StartupSetting = Configuration.Configuration.Instance.StartupSetting;
        NetIo.SendPacket(p1);

        Logger.ShowInfo(string.Format("Mapserver:{0}:{1} is requesting configuration...", server.IP, server.port));
    }

    public void OnInternMapRegister(INTERN_LOGIN_REGISTER p)
    {
        var server = p.MapServer;
        IsMapServer = true;
        if (this.server == null)
        {
            this.server = server;
            if (server.Password != Configuration.Configuration.Instance.Password)
            {
                Logger.ShowWarning(string.Format(
                    "Mapserver:{0}:{1} is trying to register maps with wrong password:{2}", server.IP, server.port,
                    server.Password));
                return;
            }
        }
        else
        {
            if (server.Password != Configuration.Configuration.Instance.Password)
            {
                Logger.ShowWarning(string.Format(
                    "Mapserver:{0}:{1} is trying to register maps with wrong password:{2}", server.IP, server.port,
                    server.Password));
                return;
            }

            foreach (var i in server.HostedMaps)
                if (!this.server.HostedMaps.Contains(i))
                    this.server.HostedMaps.Add(i);
        }

        var count = 0;
        foreach (var i in server.HostedMaps)
            if (!MapServerManager.Instance.MapServers.ContainsKey(i))
            {
                MapServerManager.Instance.MapServers.Add(i, this.server);
                count++;
            }
            else
            {
                //Logger.ShowWarning(string.Format("MapID:{0} was already hosted by Mapserver:{1}:{2}, skiping...", i, oldserver.IP, oldserver.port));
            }

        Logger.ShowInfo(
            string.Format("{0} maps registered for MapServer:{1}:{2}...", count, server.IP, server.port));
    }
}