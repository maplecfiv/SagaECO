using System;
using System.Collections.Generic;
using SagaDB;
using SagaDB.BBS;
using SagaLib;
using SagaLogin.Manager;
using SagaLogin.Packets.Client;
using SagaLogin.Packets.Server;

namespace SagaLogin.Network.Client
{
    public partial class LoginClient : SagaLib.Client
    {
        public void OnGetGiftsRequest(TOOL_GIFTS p)
        {
            if (account.GMLevel < 250) return;
            var Type = p.type;
            var Title = p.Title;
            var Sender = p.Sender;
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
            netIO.SendPacket(p);
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
            netIO.SendPacket(p);
        }

        public void SendMails()
        {
            if (selectedChar == null) return;
            if (selectedChar.Mails != null && selectedChar.Mails.Count >= 1)
                for (var i = 0; i < selectedChar.Mails.Count; i++)
                {
                    var p = new SSMG_MAIL();
                    p.mail = selectedChar.Mails[i];
                    netIO.SendPacket(p);
                }
        }

        public void SendSingleGift(Gift gift)
        {
            if (selectedChar == null) return;
            var p = new SSMG_GIFT();
            p.mails = gift;
            netIO.SendPacket(p);
        }

        public void SendGifts()
        {
            if (selectedChar == null) return;
            LoginServer.charDB.GetGifts(selectedChar);
            for (var i = 0; i < selectedChar.Gifts.Count; i++)
            {
                var p = new SSMG_GIFT();
                p.mails = selectedChar.Gifts[i];
                netIO.SendPacket(p);
            }
        }
    }
}