using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using SagaDB.Actor;
using SagaDB.ECOShop;
using SagaDB.FlyingGarden;
using SagaDB.Iris;
using SagaDB.Item;
using SagaDB.Map;
using SagaDB.Mob;
using SagaDB.Npc;
using SagaDB.Partner;
using SagaDB.Quests;
using SagaDB.Skill;
using SagaDB.Synthese;
using SagaDB.Theater;
using SagaDB.Treasure;
using SagaLib;
using SagaLib.Tasks;
using SagaLib.VirtualFileSytem;
using SagaMap.ActorEventHandlers;
using SagaMap.FictitiousActors;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Packets.Server.Actor;
using SagaMap.Packets.Server.Another;
using SagaMap.Packets.Server.AnotherAncientArk;
using SagaMap.Packets.Server.Chat;
using SagaMap.Packets.Server.FFGarden;
using SagaMap.Packets.Server.NPC;
using SagaMap.Partner;
using SagaMap.Scripting;
using SagaMap.Skill;
using SagaMap.Skill.Additions;
using SagaMap.Tasks.PC;
using SagaMap.Tasks.System;
using AIFlag = SagaMap.Mob.AIFlag;
using AIMode = SagaMap.Mob.AIMode;
using FurniturePlace = SagaDB.FlyingGarden.FurniturePlace;
using Item = SagaDB.Item.Item;
using StatusFactory = SagaMap.PC.StatusFactory;

namespace SagaMap
{
    public class AtCommand : Singleton<AtCommand>
    {
        private static string MasterName = "Saga";
        private readonly Dictionary<string, CommandInfo> commandTable;


        public Dictionary<string, MultiRunTask> tasklist = new Dictionary<string, MultiRunTask>();

        public AtCommand()
        {
            commandTable = new Dictionary<string, CommandInfo>();

            //#region "Prefixes"

            var OpenCommandPrefix = "/";
            var GMCommandPrefix = "!";
            var RemoteCommandPrefix = "~";

            //#endregion

            //#region "Public Commands"

            commandTable.Add(GMCommandPrefix + "buff", new CommandInfo(ProcessBuffTest, 100));

            // public commands
            commandTable.Add(OpenCommandPrefix + "who", new CommandInfo(ProcessWho, 0));
            commandTable.Add(OpenCommandPrefix + "revive", new CommandInfo(ProcessRevive, 0));
            commandTable.Add(OpenCommandPrefix + "home", new CommandInfo(ProcessHome, 0));
            commandTable.Add(OpenCommandPrefix + "motion", new CommandInfo(ProcessMotion, 0));
            commandTable.Add(OpenCommandPrefix + "dustbox", new CommandInfo(ProcessDustbox, 0));
            commandTable.Add(OpenCommandPrefix + "vcashshop", new CommandInfo(ProcessVcashsop, 0));
            commandTable.Add(OpenCommandPrefix + "ncshop", new CommandInfo(ProcessNCshop, 0));
            commandTable.Add(OpenCommandPrefix + "gshop", new CommandInfo(ProcessGShop, 0));
            commandTable.Add(OpenCommandPrefix + "user", new CommandInfo(ProcessUser, 0));
            commandTable.Add(OpenCommandPrefix + "commandlist", new CommandInfo(ProcessCommandList, 0));
            commandTable.Add(OpenCommandPrefix + "w", new CommandInfo(ProcessTrumpet, 0));
            commandTable.Add(OpenCommandPrefix + "openware", new CommandInfo(ProcessOpenWare, 0));
            commandTable.Add(OpenCommandPrefix + "setshop", new CommandInfo(ProcessSetShop, 0));
            commandTable.Add(OpenCommandPrefix + "autolock", new CommandInfo(ProcessAutoLock, 0));
            commandTable.Add(OpenCommandPrefix + "refuse", new CommandInfo(ProcessRefuse, 0));
            commandTable.Add(OpenCommandPrefix + "acceptpk", new CommandInfo(ProcessAccept, 0));
            commandTable.Add(OpenCommandPrefix + "praise", new CommandInfo(ProcessPraise, 0));
            commandTable.Add(OpenCommandPrefix + "bosstime", new CommandInfo(ProcessBossTime, 0));
            commandTable.Add(OpenCommandPrefix + "queryfame", new CommandInfo(ProcessQueryFame, 0));

            //#endregion

            //#region "GM Commands"

            // gm commands
            //this.commandTable.Add(GMCommandPrefix + "wlevel", new CommandInfo(new ProcessCommandFunc(this.ProcessWlevel), 2));


            //now working

            commandTable.Add(GMCommandPrefix + "wing", new CommandInfo(ProcessOpenWing, 100));
            commandTable.Add(GMCommandPrefix + "ffweather", new CommandInfo(ProcessChangeFFWeather, 100));
            commandTable.Add(GMCommandPrefix + "fg", new CommandInfo(ProcessFG, 20));
            commandTable.Add(GMCommandPrefix + "joinfg", new CommandInfo(ProcessJoinFG, 100));

            commandTable.Add(GMCommandPrefix + "warp", new CommandInfo(ProcessWarp, 20));
            commandTable.Add(GMCommandPrefix + "announce", new CommandInfo(ProcessAnnounce, 100));
            commandTable.Add(GMCommandPrefix + "heal", new CommandInfo(ProcessHeal, 50));
            commandTable.Add(GMCommandPrefix + "level", new CommandInfo(ProcessLevel, 60));
            commandTable.Add(GMCommandPrefix + "joblv", new CommandInfo(ProcessJobLevel, 60));
            commandTable.Add(GMCommandPrefix + "gold", new CommandInfo(ProcessGold, 50));
            commandTable.Add(GMCommandPrefix + "shoppoint", new CommandInfo(ProcessShoppoint, 60));
            commandTable.Add(GMCommandPrefix + "hair", new CommandInfo(ProcessHair, 20));
            commandTable.Add(GMCommandPrefix + "hairstyle", new CommandInfo(ProcessHairstyle, 20));
            commandTable.Add(GMCommandPrefix + "haircolor", new CommandInfo(ProcessHaircolor, 20));
            commandTable.Add(GMCommandPrefix + "job", new CommandInfo(ProcessJob, 60));
            commandTable.Add(GMCommandPrefix + "statpoints", new CommandInfo(ProcessStatPoints, 60));
            commandTable.Add(GMCommandPrefix + "skillpoints", new CommandInfo(ProcessSkillPoints, 60));
            commandTable.Add(GMCommandPrefix + "hide", new CommandInfo(ProcessHide, 60));
            commandTable.Add(GMCommandPrefix + "ban", new CommandInfo(ProcessBan, 80));

            commandTable.Add(GMCommandPrefix + "event", new CommandInfo(ProcessEvent, 20));

            commandTable.Add(GMCommandPrefix + "hairext", new CommandInfo(ProcessHairext, 20));
            commandTable.Add(GMCommandPrefix + "playersize", new CommandInfo(ProcessPlayersize, 20));
            commandTable.Add(GMCommandPrefix + "item", new CommandInfo(ProcessItem, 1));
            commandTable.Add(GMCommandPrefix + "speed", new CommandInfo(ProcessSpeed, 50));
            commandTable.Add(GMCommandPrefix + "gmrevive", new CommandInfo(ProcessRevive, 50));

            commandTable.Add(GMCommandPrefix + "kick", new CommandInfo(ProcessKick, 100));
            commandTable.Add(GMCommandPrefix + "kickall", new CommandInfo(ProcessKickAll, 100));
            commandTable.Add(GMCommandPrefix + "recall", new CommandInfo(ProcessJump, 60));
            commandTable.Add(GMCommandPrefix + "recall2", new CommandInfo(ProcessJump2, 60));
            commandTable.Add(GMCommandPrefix + "jump", new CommandInfo(ProcessJumpTo, 60));
            commandTable.Add(GMCommandPrefix + "jump2", new CommandInfo(ProcessJumpTo2, 60));
            commandTable.Add(GMCommandPrefix + "mob", new CommandInfo(ProcessMob, 60));
            commandTable.Add(GMCommandPrefix + "summon", new CommandInfo(ProcessSummon, 60));
            commandTable.Add(GMCommandPrefix + "summonme", new CommandInfo(ProcessSummonMe, 60));
            commandTable.Add(GMCommandPrefix + "spawn", new CommandInfo(ProcessSpawn, 60));
            commandTable.Add(GMCommandPrefix + "effect", new CommandInfo(ProcessEffect, 60));
            commandTable.Add(GMCommandPrefix + "kickgolem", new CommandInfo(ProcessKickGolem, 60));
            commandTable.Add(GMCommandPrefix + "killallmob", new CommandInfo(ProcessKillAllMob, 60));
            commandTable.Add(GMCommandPrefix + "odwarstart", new CommandInfo(ProcessODWarStart, 60));
            //this.commandTable.Add(GMCommandPrefix + "tweet", new CommandInfo(new ProcessCommandFunc(this.ProcessTweet), 0));


            //for skill test
            commandTable.Add(GMCommandPrefix + "skill", new CommandInfo(ProcessSkill, 60));
            commandTable.Add(GMCommandPrefix + "skillclear", new CommandInfo(ProcessSkillClear, 60));
            commandTable.Add(GMCommandPrefix + "gmob", new CommandInfo(ProcessGridMob, 60));
            commandTable.Add(GMCommandPrefix + "showstatus", new CommandInfo(ProcessShowStatus, 60));

            commandTable.Add(GMCommandPrefix + "who", new CommandInfo(ProcessWho, 1));
            commandTable.Add(GMCommandPrefix + "who2", new CommandInfo(ProcessWho2, 20));
            commandTable.Add(GMCommandPrefix + "who3", new CommandInfo(ProcessWho3, 60));
            commandTable.Add(GMCommandPrefix + "mode", new CommandInfo(ProcessMode, 100));
            commandTable.Add(GMCommandPrefix + "robot", new CommandInfo(ProcessRobot, 100));

            commandTable.Add(GMCommandPrefix + "go", new CommandInfo(ProcessGo, 20));
            commandTable.Add(GMCommandPrefix + "ch", new CommandInfo(ProcessCh, 20));
            //now working
            commandTable.Add(GMCommandPrefix + "info", new CommandInfo(ProcessInfo, 20));

            commandTable.Add(GMCommandPrefix + "reloadscript", new CommandInfo(ProcessReloadScript, 99));
            commandTable.Add(GMCommandPrefix + "reloadconfig", new CommandInfo(ProcessReloadConfig, 99));
            commandTable.Add(GMCommandPrefix + "raw", new CommandInfo(ProcessRaw, 100));
            commandTable.Add(GMCommandPrefix + "test", new CommandInfo(ProcessTest, 100));

            commandTable.Add(GMCommandPrefix + "face", new CommandInfo(ProcessFace, 100));
            commandTable.Add(GMCommandPrefix + "createff", new CommandInfo(ProcessCreateFF, 100));
            commandTable.Add(GMCommandPrefix + "openff", new CommandInfo(ProcessOpenFF, 100));

            commandTable.Add(GMCommandPrefix + "theater", new CommandInfo(ProcessTheater, 100));

            commandTable.Add(GMCommandPrefix + "metamo", new CommandInfo(ProcessMetamo, 100));

            commandTable.Add(GMCommandPrefix + "through", new CommandInfo(ProcessThrough, 100));

            commandTable.Add(GMCommandPrefix + "ta", new CommandInfo(ProcessTaskAnnounce, 100));
            commandTable.Add(GMCommandPrefix + "sta", new CommandInfo(ProcessStopTaskAnnounce, 100));

            commandTable.Add(GMCommandPrefix + "goldto", new CommandInfo(ProcessGoldTo, 100));
            commandTable.Add(GMCommandPrefix + "itemto", new CommandInfo(ProcessItemTo, 100));

            commandTable.Add(GMCommandPrefix + "ring", new CommandInfo(ProcessRing, 100));

            commandTable.Add(GMCommandPrefix + "clearbuff", new CommandInfo(ProcessClearBuff, 100));

            //简化操作！！
            commandTable.Add(GMCommandPrefix + "var", new CommandInfo(ProcessVariable, 100));
            commandTable.Add(GMCommandPrefix + "variable", new CommandInfo(ProcessVariable, 100));
            commandTable.Add(GMCommandPrefix + "title", new CommandInfo(ProcessSetTitle, 100));
            commandTable.Add(GMCommandPrefix + "titleto", new CommandInfo(ProcessSetTitleTo, 100));
            commandTable.Add(GMCommandPrefix + "status", new CommandInfo(ProcessStatus, 100));
            commandTable.Add(GMCommandPrefix + "effect2", new CommandInfo(ProcessEffect2, 30));

            commandTable.Add(GMCommandPrefix + "golem", new CommandInfo(ProcessGolem, 100));
            commandTable.Add(GMCommandPrefix + "debug", new CommandInfo(ProcessDebug, 100));
            commandTable.Add(GMCommandPrefix + "dialog", new CommandInfo(ProcessDialog, 100));

            commandTable.Add(GMCommandPrefix + "idsearch", new CommandInfo(ProcessIDSearch, 60));
            commandTable.Add(GMCommandPrefix + "equiplist", new CommandInfo(ProcessEquipList, 60));
            commandTable.Add(GMCommandPrefix + "inventorylist", new CommandInfo(ProcessInventoryList, 60));
            commandTable.Add(GMCommandPrefix + "recallmap", new CommandInfo(ProcessCallMap, 60));
            commandTable.Add(GMCommandPrefix + "recallall", new CommandInfo(ProcessCallAll, 60));
            commandTable.Add(GMCommandPrefix + "monsterinfo", new CommandInfo(ProcessMonsterInfo, 60));
            commandTable.Add(GMCommandPrefix + "reloadskilldb", new CommandInfo(ProcessReloadSkillDB, 60));
            commandTable.Add(GMCommandPrefix + "skillall", new CommandInfo(ProcessSkillALL, 60));
            commandTable.Add(GMCommandPrefix + "fame", new CommandInfo(ProcessFame, 60));
            commandTable.Add(GMCommandPrefix + "statreset", new CommandInfo(ProcessStatReset, 60));
            commandTable.Add(GMCommandPrefix + "skreset", new CommandInfo(ProcessSkillReset, 60));
            commandTable.Add(GMCommandPrefix + "rwarp", new CommandInfo(ProcessRWarp, 60));
            commandTable.Add(GMCommandPrefix + "ep", new CommandInfo(ProcessEP, 60));

            //#endregion

            //#region 黑白照Addition

            commandTable.Add(GMCommandPrefix + "item2", new CommandInfo(ProcessItem2, 100));
            commandTable.Add(GMCommandPrefix + "itemclear", new CommandInfo(ProcessItemClear, 100));
            commandTable.Add(GMCommandPrefix + "irissearch", new CommandInfo(ProcessIrisSearch, 100));

            //#endregion

            //#region KK測試用

            commandTable.Add(GMCommandPrefix + "mapobj", new CommandInfo(ProcessMapObject, 100));
            commandTable.Add(GMCommandPrefix + "ssmode", new CommandInfo(ProcessSSMode, 100));
            commandTable.Add(GMCommandPrefix + "aaavoice", new CommandInfo(ProcessAAVoice, 100));
            commandTable.Add(GMCommandPrefix + "npcvoice", new CommandInfo(ProcessNPCVoice, 100));
            commandTable.Add(GMCommandPrefix + "raw2", new CommandInfo(ProcessRaw2, 0));
            commandTable.Add(GMCommandPrefix + "pmotion", new CommandInfo(ProcessPartnerMotion, 100));
            commandTable.Add(GMCommandPrefix + "cleards", new CommandInfo(ProcessClearDS, 100));

            //#endregion

            //#region "Remote Commands"

            // remote commands
            //this.commandTable.Add( RemoteCommandPrefix + "jump", new CommandInfo( new ProcessCommandFunc( this.ProcessRJump ), 60 ) );
            //this.commandTable.Add( RemoteCommandPrefix + "cash", new CommandInfo( new ProcessCommandFunc( this.ProcessRCash ), 60 ) );
            //this.commandTable.Add( RemoteCommandPrefix + "info", new CommandInfo( new ProcessCommandFunc( this.ProcessRInfo ), 60 ) );
            //this.commandTable.Add( RemoteCommandPrefix + "res", new CommandInfo( new ProcessCommandFunc(this.ProcessRRes), 60));
            //this.commandTable.Add(RemoteCommandPrefix + "die", new CommandInfo(new ProcessCommandFunc(this.ProcessRDie), 60));
            //this.commandTable.Add(RemoteCommandPrefix + "heal", new CommandInfo(new ProcessCommandFunc(this.ProcessRHeal), 60));

            //#endregion


            //#region "Aliases"

            // Aliases
            //this.commandTable.Add(GMCommandPrefix + "kill", new CommandInfo(new ProcessCommandFunc(this.ProcessDie), 60));
            //this.commandTable.Add(RemoteCommandPrefix + "kill", new CommandInfo(new ProcessCommandFunc(this.ProcessRDie), 60));
            //this.commandTable.Add(GMCommandPrefix + "b", new CommandInfo(new ProcessCommandFunc(this.ProcessBroadcast), 60));
            //this.commandTable.Add(GMCommandPrefix + "gm", new CommandInfo(new ProcessCommandFunc(this.ProcessGMChat), 60));

            //#endregion
        }

        public void ProcessRing(MapClient client, string args)
        {
            if (args != "") ;
            RingManager.Instance.CreateRing(client.Character, args);
        }

        public void ProcessClearBuff(MapClient client, string args)
        {
            client.Character.ClearTaskAddition();
            client.SendSystemMessage("所有状态已清除。");
        }

        public void ProcessClearDS(MapClient client, string args)
        {
            client.Character.AStr["DailyStamp_DAY"] = "";
            client.Character.AInt["每日盖章"] = 0;
            client.SendSystemMessage("所有日印状态已清除。");
        }

        public void ProcessVariable(MapClient client, string args)
        {
            var arg = args.Split(' ');
            try
            {
                var pc = client.Character;
                var type = arg[0];
                var varname = arg[1];
                var varval = 0;
                var varstr = "";
                var charID = 0;

                if (varname != "list")
                {
                    if (Regex.IsMatch(varname, @"^\d+$")) //无符号整型（纯数字）
                    {
                        charID = int.Parse(varname);
                        var chr =
                            from c in MapClientManager.Instance.OnlinePlayer
                            where c.Character.CharID == charID
                            select c;
                        if (chr.Count() == 0)
                        {
                            client.SendSystemMessage("错误的CharaID");
                            return;
                        }

                        pc = chr.First().Character;
                        varname = arg[2];
                        if (arg.Count() >= 4)
                            if (type.Contains("s")) //字符串类型
                                varstr = arg[3];
                            else
                                varval = int.Parse(arg[3]);
                    }
                    else
                    {
                        if (arg.Count() >= 3)
                            if (type.Contains("s")) //字符串类型
                                varstr = arg[2];
                            else
                                varval = int.Parse(arg[2]);
                    }
                }

                var changevalue =
                    (charID == 0 && arg.Count() >= 3) ||
                    (charID != 0 && arg.Count() >= 4); //实际上直接判断varval是不是初始值也可以，但是严谨一点…
                if (varname == "list") changevalue = false;
                switch (type)
                {
                    case "a":
                        if (changevalue)
                            pc.AInt[varname] = varval;
                        if (varname == "list")
                        {
                            foreach (var item in pc.AInt)
                                if (item.Value != 0)
                                    client.SendSystemMessage("角色的AInt[" + item.Key + "]变量值为" + item.Value);
                        }
                        else
                        {
                            client.SendSystemMessage("角色的AInt[" + varname + "]变量值为" + pc.AInt[varname]);
                        }

                        break;
                    case "c":
                        if (changevalue)
                            pc.CInt[varname] = varval;
                        if (varname == "list")
                        {
                            foreach (var item in pc.CInt)
                                if (item.Value != 0)
                                    client.SendSystemMessage("角色的CInt[" + item.Key + "]变量值为" + item.Value);
                        }
                        else
                        {
                            client.SendSystemMessage("角色的CInt[" + varname + "]变量值为" + pc.CInt[varname]);
                        }

                        break;
                    case "t":
                        if (changevalue)
                            pc.TInt[varname] = varval;
                        if (varname == "list")
                        {
                            foreach (var item in pc.TInt)
                                if (item.Value != 0)
                                    client.SendSystemMessage("角色的TInt[" + item.Key + "]变量值为" + item.Value);
                        }
                        else
                        {
                            client.SendSystemMessage("角色的TInt[" + varname + "]变量值为" + pc.TInt[varname]);
                        }

                        break;
                    case "as":
                        if (changevalue)
                            pc.AStr[varname] = varstr;
                        if (varname == "list")
                            foreach (var item in pc.AStr)
                                client.SendSystemMessage("角色的AStr[" + item.Key + "]变量值为" + item.Value);
                        else
                            client.SendSystemMessage("角色的AStr[" + varname + "]变量值为" + pc.AStr[varname]);
                        break;
                    case "cs":
                        if (changevalue)
                            pc.CStr[varname] = varstr;
                        if (varname == "list")
                            foreach (var item in pc.CStr)
                                client.SendSystemMessage("角色的CStr[" + item.Key + "]变量值为" + item.Value);
                        else
                            client.SendSystemMessage("角色的CStr[" + varname + "]变量值为" + pc.CStr[varname]);
                        break;
                    case "ts":
                        if (changevalue)
                            pc.TStr[varname] = varstr;
                        if (varname == "list")
                            foreach (var item in pc.TStr)
                                client.SendSystemMessage("角色的TStr[" + item.Key + "]变量值为" + item.Value);
                        else
                            client.SendSystemMessage("角色的TStr[" + varname + "]变量值为" + pc.TStr[varname]);
                        break;
                    default:
                        client.SendSystemMessage("类型错误");
                        break;
                }
            }
            catch (Exception)
            {
                client.SendSystemMessage("参数错误（注意结尾不要有多余的空格！）");
                client.SendSystemMessage("\"!variable 类型[a/c/t/as/cs/ts] 变量名\"—显示自己的对应变量名");
                client.SendSystemMessage("\"!variable 类型 变量名 变量值\"—修改自己对应的变量值");
                client.SendSystemMessage("\"!variable 类型 charID 变量名\"—显示指定角色的对应变量名");
                client.SendSystemMessage("\"!variable 类型 charID 变量名 变量值\"—修改指定角色对应的变量值");
                client.SendSystemMessage("类型：a:AInt c:CInt t:TInt as:AStr cs:CStr ts:TStr");
            }
        }

        public void ProcessSetTitleTo(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "")
            {
                client.SendSystemMessage("参数错误");
                return;
            }

            var charID = int.Parse(arg[0]);
            var index = int.Parse(arg[1]);

            var chr =
                from c in MapClientManager.Instance.OnlinePlayer
                where c.Character.CharID == charID
                select c;
            var tClient = chr.First();

            tClient.SetTitle(index, true);
        }

        public void ProcessSetTitle(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "")
            {
                client.SendSystemMessage("参数错误");
                return;
            }

            var index = int.Parse(arg[0]);
            client.SetTitle(index, true);
        }

        public void ProcessBuffTest(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "" || arg[1] == "")
            {
                client.SendSystemMessage("参数错误");
                return;
            }

            try
            {
                var list = byte.Parse(arg[0]);
                var index = int.Parse(arg[1]);
                var s = "";

                byte[] IDbuf;
                var strIDbuf = "";
                byte[] Indexbuf;
                var strIndexbuf = "";
                var nullbuf = "";
                IDbuf = BitConverter.GetBytes(client.Character.ActorID);
                Array.Reverse(IDbuf);
                strIDbuf = Conversions.bytes2HexString(IDbuf);
                Indexbuf = BitConverter.GetBytes(index);
                Array.Reverse(Indexbuf);
                strIndexbuf = Conversions.bytes2HexString(Indexbuf);
                nullbuf = " 00 00 00 00";
                switch (list)
                {
                    case 1:
                        s = "15 7C " + strIDbuf + strIndexbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 2:
                        s = "15 7C " + strIDbuf + nullbuf + strIndexbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 3:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + strIndexbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 4:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + strIndexbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 5:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + strIndexbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 6:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + strIndexbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 7:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            strIndexbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 8:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + strIndexbuf + nullbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 9:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + strIndexbuf + nullbuf + nullbuf + nullbuf;
                        break;
                    case 10:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + strIndexbuf + nullbuf + nullbuf;
                        break;
                    case 11:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + strIndexbuf + nullbuf;
                        break;
                    case 12:
                        s = "15 7C " + strIDbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + nullbuf +
                            nullbuf + nullbuf + nullbuf + nullbuf + nullbuf + strIndexbuf;
                        break;
                }

                var buf = Conversions.HexStr2Bytes(s.Replace(" ", ""));
                var p = new Packet();
                p.data = buf;
                client.NetIo.SendPacket(p);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessOpenWing(MapClient client, string args)
        {
            var p1 = new SSMG_TEST_EVOLVE_OPEN();
            client.NetIo.SendPacket(p1);
            var p2 = new SSMG_TEST_EVOLVE_OPEN2();
            client.NetIo.SendPacket(p2);
            var p3 = new SSMG_TEST_EVOLVE_OPEN3();
            client.NetIo.SendPacket(p3);
        }

        public void ProcessChangeFFWeather(MapClient client, string args)
        {
            var arg = args.Split(' ');

            if (arg.Length > 1)
                ScriptManager.Instance.VariableHolder.AInt["服務器FF天氣"] = int.Parse(arg[1]);
            ScriptManager.Instance.VariableHolder.AInt["服務器FF背景"] = int.Parse(arg[0]);
            var map = MapManager.Instance.GetMap(client.Character.MapID);
            foreach (var pc in map.Actors)
                if (pc.Value.type == ActorType.PC)
                {
                    var mc = MapClient.FromActorPC((ActorPC)pc.Value);
                    CustomMapManager.Instance.EnterFFOnMapLoaded(mc);
                }
        }

        public void ProcessTaskAnnounce(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "" || arg[1] == "" || arg[2] == "")
            {
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_TA_PAEA);
                return;
            }

            try
            {
                var taskname = arg[0];
                if (!tasklist.ContainsKey(taskname))
                {
                    var announce = arg[1];
                    var period = int.Parse(arg[2]) * 1000;
                    var ta = new TaskAnnounce(taskname, announce, period);
                    ta.Activate();
                    tasklist.Add(taskname, ta);
                    client.SendSystemMessage(taskname + "添加成功");
                }
                else
                {
                    client.SendSystemMessage(taskname + "已存在！");
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessStopTaskAnnounce(MapClient client, string args)
        {
            try
            {
                var taskname = args;
                if (tasklist.ContainsKey(taskname))
                {
                    var task = tasklist[taskname];
                    task.Deactivate();
                    tasklist.Remove(taskname);
                    client.SendSystemMessage(taskname + "已移除");
                }
                else
                {
                    client.SendSystemMessage("未找到" + taskname);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessThrough(MapClient client, string args)
        {
            try
            {
                if (!client.Character.Status.Additions.ContainsKey("Through"))
                {
                    var skill = SkillFactory.Instance.GetSkill(100, 1);
                    var Through = new DefaultBuff(skill, client.Character, "Through", 600000);
                    SkillHandler.ApplyAddition(client.Character, Through);
                }
                else
                {
                    client.Character.Status.Additions["Through"].AdditionEnd();
                    client.Character.Status.Additions.Remove("Through");
                }
            }
            catch
            {
            }
        }

        public void ProcessJoinFG(MapClient client, string args)
        {
            var p = new Packet(10); //unknown packet
            p.ID = 0x18E3;
            p.PutUInt(client.Character.ActorID, 2);
            p.PutUInt(client.Character.MapID, 6);
            MapClient.FromActorPC(client.Character).NetIo.SendPacket(p);


            var map = MapManager.Instance.GetMap(client.Character.MapID);
            client.Character.FlyingGarden.MapID = MapManager.Instance.CreateMapInstance(client.Character, 70000000,
                client.Character.MapID, Global.PosX16to8(client.Character.X, map.Width),
                Global.PosY16to8(client.Character.Y, map.Height));
            var pc = client.Character;
            //spawn furnitures
            map = MapManager.Instance.GetMap(pc.FlyingGarden.MapID);
            foreach (var i in pc.FlyingGarden.Furnitures[FurniturePlace.GARDEN])
            {
                i.e = new NullEventHandler();
                map.RegisterActor(i);
                i.invisble = false;
            }

            pc.BattleStatus = 0;
            pc.Speed = 200;
            client.SendChangeStatus();
            var newMap = MapManager.Instance.GetMap(pc.FlyingGarden.MapID);
            client.Map.SendActorToMap(client.Character, newMap, Global.PosX8to16(6, newMap.Width),
                Global.PosY8to16(11, newMap.Height));
        }

        public void ProcessMetamo(MapClient client, string args)
        {
            try
            {
                client.Character.TranceID = uint.Parse(args);
                client.SendCharInfoUpdate();
            }
            catch
            {
            }
        }

        public void ProcessTheater(MapClient client, string command)
        {
            try
            {
                var p = new SSMG_NPC_PLAY_SOUND();
                p.SoundID = 2000;
                p.Loop = 0;
                p.Volume = 100;
                p.Balance = 50;
                client.NetIo.SendPacket(p);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessDialog(MapClient client, string command)
        {
            try
            {
                var args = command.Split(' ');
                if (args.Length == 1)
                {
                    var ID = ushort.Parse(args[0]);
                    var p = new SSMG_ANO_DIALOG_BOX();
                    p.DID = ID;
                    client.NetIo.SendPacket(p);
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessDebug(MapClient client, string command)
        {
            try
            {
                var args = command.Split(' ');
                if (args.Length == 1)
                {
                    var Level = uint.Parse(args[0]);
                    Logger.CurrentLogger.LogLevel = (Logger.LogContent)Level;
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        public void ProcessGolem(MapClient client, string command)
        {
            try
            {
                var args = command.Split(' ');
                if (args.Length == 1)
                {
                    var ActorID = uint.Parse(args[0]);
                    var a = client.map.GetActor(ActorID);
                    if (a != null)
                        client.map.DeleteActor(a);
                    return;
                }

                if (args.Length == 9)
                {
                    var mapid = uint.Parse(args[0]);
                    var x = byte.Parse(args[1]);
                    var y = byte.Parse(args[2]);
                    var pictid = uint.Parse(args[4]);
                    var name = args[5];
                    var shoptype = byte.Parse(args[6]);
                    var title = args[7];
                    var aimode = byte.Parse(args[8]);
                    var golem = new ActorGolem();
                    golem.MapID = mapid;
                    golem.X2 = x;
                    golem.Y2 = y;
                    //golem.EventID = eventid;
                    golem.PictID = pictid;
                    golem.Name = name;
                    if (shoptype == 1)
                        golem.GolemType = GolemType.Sell;
                    else
                        golem.GolemType = GolemType.Buy;
                    golem.Title = title;
                    golem.AIMode = aimode;
                    FictitiousActorsManager.Instance.regionFictitiousSingleActor(golem);
                    client.SendSystemMessage("刷新成功！ActorID：" + golem.ActorID);
                }
                else
                {
                    client.SendSystemMessage(
                        "错误！请按照!golem mapid x y eventid pictid 名字 ShopType(0为收购 1为贩卖) 店名 AIMODE格式输入。");
                    client.SendSystemMessage("例如!golem 10054001 152 203 66000001 16470000 清姬2 1 收购木材啦！ 0");
                    client.SendSystemMessage("或者使用!golem actorID来进行删除石像。actorID使用!who3查看。");
                }
            }
            catch (Exception ex)
            {
                client.SendSystemMessage("错误！请按照!golem mapid x y eventid pictid 名字 ShopType(0为收购 1为贩卖) 店名 AIMODE格式输入。");
                client.SendSystemMessage("例如!golem 10054001 152 203 66000001 16470000 清姬2 1 收购木材啦！ 0");
                client.SendSystemMessage("或者使用!golem actorID来进行删除石像。actorID使用!who3查看。");
                Logger.ShowError(ex);
            }
        }

        public void ProcessStatus(MapClient client, string command)
        {
            try
            {
                var args = command.Split(' ');
                if (args.Length > 1)
                {
                    var pt = int.Parse(args[1]);
                    switch (args[0])
                    {
                        case "str":
                            client.Character.Str = (ushort)pt;
                            break;
                        case "dex":
                            client.Character.Dex = (ushort)pt;
                            break;
                        case "int":
                            client.Character.Int = (ushort)pt;
                            break;
                        case "vit":
                            client.Character.Vit = (ushort)pt;
                            break;
                        case "agi":
                            client.Character.Agi = (ushort)pt;
                            break;
                        case "mag":
                            client.Character.Mag = (ushort)pt;
                            break;
                        case "hp":
                            client.Character.TInt["临时HP"] = pt;
                            break;
                        case "mp":
                            client.Character.TInt["临时MP"] = pt;
                            break;
                        case "sp":
                            client.Character.TInt["临时SP"] = pt;
                            break;
                        case "atk":
                            client.Character.TInt["临时ATK"] = pt;
                            break;
                        case "matk":
                            client.Character.TInt["临时MATK"] = pt;
                            break;
                    }
                }

                StatusFactory.Instance.CalcStatus(client.Character);
                client.SendStatus();
                client.SendStatusExtend();
            }
            catch
            {
            }
        }

        /// <summary>
        ///     讀取設定檔
        /// </summary>
        /// <param name="path"></param>
        public void LoadCommandLevelSetting(string path)
        {
            var cmd = "";
            try
            {
                var sr = new StreamReader(path);
                var count = 0;
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    if (line[0] == '#')
                    {
                        cmd = "";
                        continue;
                    }

                    cmd = line;
                    var sLine = line.Split(',');
                    var cmdInfo = commandTable[sLine[0]];
                    if (cmdInfo != null)
                    {
                        cmdInfo.level = uint.Parse(sLine[1]);
                        count++;
                    }
                }

                Logger.ShowInfo(string.Format("{0} GMCommand Setting Loaded.", count));
            }
            catch (Exception ex)
            {
                Logger.ShowError(cmd + "," + ex);
            }
        }

        public bool ProcessCommand(MapClient client, string command)
        {
            try
            {
                var args = command.Split(" ".ToCharArray(), 2);
                args[0] = args[0].ToLower();

                var log = new Logger("GM命令使用记录.txt");
                var logtext = "\r\n" + client.Character.Name + "：" + command;
                log.WriteLog(logtext);

                if (commandTable.ContainsKey(args[0]))
                {
                    var cInfo = commandTable[args[0]];

                    if (client.Character.Account.GMLevel >= cInfo.level)
                    {
                        if (client.Character.Account.GMLevel >= 1)
                            Logger.LogGMCommand(client.Character.Name + "(" + client.Character.CharID + ")", "",
                                string.Format("Account:{0}({1}) GMLv:{2} Command:{3}",
                                    client.Character.Account.Name,
                                    client.Character.Account.AccountID,
                                    client.Character.Account.GMLevel, command));

                        if (args.Length == 2)
                            cInfo.func(client, args[1]);
                        else cInfo.func(client, "");
                    }
                    else
                    {
                        client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_NO_ACCESS);
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                Logger.ShowError(e, null);
            }

            return false;
        }

        private delegate void ProcessCommandFunc(MapClient client, string args);

        private class CommandInfo
        {
            public readonly ProcessCommandFunc func;
            public uint level;

            public CommandInfo(ProcessCommandFunc func, uint lvl)
            {
                this.func = func;
                level = lvl;
            }
        }

        //#region "Command Processing"

        //#region "Public Commands"

        private void ProcessHome(MapClient client, string args)
        {
            //if (client.Character.MapID == 10054000 && !client.Character.Buff.Dead) return;
            var map = MapManager.Instance.GetMap(client.Character.MapID);
            var pc = client.Character;
            if (pc.HP == 0)
            {
                pc.HP = pc.MaxHP;
                pc.MP = pc.MaxMP;
                pc.SP = pc.MaxSP;
                pc.EP = pc.MaxEP;
                if (pc.Job == PC_JOB.ASTRALIST) //魔法师
                    pc.EP = 0;
            }

            if (pc.Job == PC_JOB.CARDINAL)
                pc.EP = 5000;
            pc.TInt["副本复活标记"] = 0;
            pc.BattleStatus = 0;
            client.SendChangeStatus();
            pc.Buff.Dead = false;
            pc.Buff.TurningPurple = false;
            pc.Motion = MotionType.STAND;
            pc.MotionLoop = false;
            SkillHandler.Instance.ShowVessel(pc, (int)-pc.MaxHP);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, pc, true);

            SkillHandler.Instance.ShowEffectByActor(pc, 5116);
            SkillHandler.Instance.CastPassiveSkills(pc);
            client.SendPlayerInfo();
            if (!pc.Tasks.ContainsKey("Recover")) //自然恢复
            {
                var reg = new Recover(client);
                pc.Tasks.Add("Recover", reg);
                reg.Activate();
            }

            var arg = new EffectArg();
            arg.effectID = 5362;
            arg.actorID = 0xFFFFFFFF;
            arg.x = Global.PosX16to8(client.Character.X, map.Width);
            arg.y = Global.PosY16to8(client.Character.Y, map.Height);
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, client.Character, true);

            if (Configuration.Configuration.Instance.HostedMaps.Contains(10054000))
            {
                var newMap = MapManager.Instance.GetMap(10054000);
                client.Map.SendActorToMap(client.Character, 10054000, Global.PosX8to16(154, newMap.Width),
                    Global.PosY8to16(146, newMap.Height));
            }
        }

        private void ProcessMotion(MapClient client, string args)
        {
            client.SendMotion((MotionType)int.Parse(args), 1);
        }

        public void ProcessPartnerMotion(MapClient client, string args)
        {
            var v = args.Split(" ".ToCharArray(), 2);

            var map = MapManager.Instance.GetMap(client.Character.MapID);
            var pc = client.Character;

            if (pc.Partner == null)
            {
                client.SendSystemMessage("未裝備Partner。");
            }
            else
            {
                byte loop = 0;
                var bloop = false;
                if (int.Parse(v[1]) == 1)
                {
                    loop = 1;
                    bloop = true;
                }

                var partner = pc.Partner;
                var parg = new ChatArg();
                parg.motion = (MotionType)int.Parse(v[0]);
                parg.loop = loop;


                partner.Motion = (MotionType)int.Parse(v[0]);
                partner.MotionLoop = bloop;

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.MOTION, parg, partner, true);
            }
        }

        private void ProcessWhere(MapClient client, string args)
        {
        }

        private void ProcessDustbox(MapClient client, string args)
        {
            client.npcTrade = true;
            var name = "垃圾箱";

            client.SendTradeStartNPC(name);
            client.Character.CInt["垃圾箱记录"] = 1;
            var blocked = ClientManager.Blocked;
            if (blocked)
                ClientManager.LeaveCriticalArea();
            /*while (client.npcTrade)
            {
                System.Threading.Thread.Sleep(500);
            }*/
            if (blocked)
                ClientManager.EnterCriticalArea();
            client.Character.CInt["垃圾箱记录"] = 0;
            client.npcTradeItem = null;
        }

        private void ProcessNCshop(MapClient client, string args)
        {
            SkillEvent.Instance.NCShopOpen(client.Character);
        }

        private void ProcessGShop(MapClient client, string args)
        {
            SkillEvent.Instance.GShopOpen(client.Character);
        }

        private void ProcessVcashsop(MapClient client, string args)
        {
            SkillEvent.Instance.VShopOpen(client.Character);
        }

        private void ProcessUser(MapClient client, string args)
        {
            foreach (var i in MapClientManager.Instance.OnlinePlayer)
                client.SendSystemMessage(i.Character.Name + " [" + i.Map.Name + "]");
            client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                     MapClientManager.Instance.OnlinePlayer.Count);
        }

        private void ProcessGetHeight(MapClient client, string args)
        {
        }

        //#endregion

        //#region "GM Commands"

        public void ProcessCommandList(MapClient client, string args)
        {
            client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_COMMANDLIST);
            foreach (var i in commandTable.Keys)
                if (client.Character.Account.GMLevel >= commandTable[i].level)
                {
                    var desc = "";
                    if (LocalManager.Instance.Strings.ATCOMMAND_DESC.ContainsKey(i))
                        desc = LocalManager.Instance.Strings.ATCOMMAND_DESC[i];
                    client.SendSystemMessage(i + " " + desc);
                }
        }

        private void ProcessRevive(MapClient client, string args)
        {
            if (client.Character.Buff.Dead)
            {
                client.Character.BattleStatus = 0;
                client.SendChangeStatus();
                client.Character.TInt["Revive"] = 5;
                client.EventActivate(0xF1000000);
            }
        }

        private void ProcessSpeed(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_SPEED_PARA);
            else
                try
                {
                    client.Character.Speed = ushort.Parse(args);
                    client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SPEED_UPDATE, null, client.Character,
                        true);
                }

                catch (Exception)
                {
                }
        }

        private void ProcessStatPoints(MapClient client, string args)
        {
            try
            {
                var pt = ushort.Parse(args);
                client.Character.StatsPoint = pt;
            }
            catch
            {
            }
        }

        private void ProcessSkillPoints(MapClient client, string command)
        {
            try
            {
                var args = command.Split(' ');
                if (args.Length > 1)
                {
                    var pt = ushort.Parse(args[1]);
                    switch (args[0])
                    {
                        case "1":
                            client.Character.SkillPoint = pt;
                            break;
                        case "2-1":
                            client.Character.SkillPoint2X = pt;
                            break;
                        case "2-2":
                            client.Character.SkillPoint2T = pt;
                            break;
                        case "3":
                            client.Character.SkillPoint3 = pt;
                            break;
                    }
                }
            }
            catch
            {
            }
        }

        private void ProcessJob(MapClient client, string args)
        {
            try
            {
                MapServer.charDB.SaveSkill(client.Character);
                client.Character.Skills.Clear();
                client.Character.Skills2.Clear();
                client.Character.Skills2_1.Clear();
                client.Character.Skills2_2.Clear();
                client.Character.SkillsReserve.Clear();
                client.Character.Skills3.Clear();
                //client.Character.JobLevel3 = 1;
                client.Character.JEXP = 1;
                client.Character.SkillPoint = 0;
                client.Character.SkillPoint2T = 0;
                client.Character.SkillPoint2X = 0;
                client.Character.SkillPoint3 = 0;
                var job = int.Parse(args);
                client.Character.Job = (PC_JOB)job;
                MapServer.charDB.GetSkill(client.Character);
                client.Character.JEXP = 1;
                MapServer.charDB.SaveSkill(client.Character);
                StatusFactory.Instance.CalcStatus(client.Character);
                client.SendPlayerInfo();
            }
            catch (Exception)
            {
            }
        }

        private void ProcessEvent(MapClient client, string args)
        {
            try
            {
                var Event = uint.Parse(args);
                client.EventActivate(Event);
            }
            catch (Exception)
            {
            }
        }

        //TODO:
        private void ProcessReloadConfig(MapClient client, string args)
        {
            try
            {
                switch (args.ToLower())
                {
                    case "ecoshop":
                        ProcessSettingAnnounce(client, "[系统] 商城DB更新中…");
                        ECOShopFactory.Instance.Reload();
                        NCShopFactory.Instance.Reload();
                        GShopFactory.Instance.Reload();
                        ProcessSettingAnnounce(client, "[系统] 商城DB更新完毕");
                        break;
                    case "shopdb":
                        ProcessSettingAnnounce(client, "[系统] 商店DB更新中…");
                        ShopFactory.Instance.Reload();
                        ShopFactory.Instance.Init("DB/ShopDB.xml",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        GC.Collect();
                        ProcessSettingAnnounce(client, "[系统] 商店DB更新完毕");
                        break;
                    case "monster":
                        ProcessSettingAnnounce(client, "[系统] 怪物DB更新中…");
                        MobFactory.Instance.Mobs.Clear();
                        MobFactory.Instance.Init("./DB/monster.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        MobAIFactory.Instance.Items.Clear();
                        MobAIFactory.Instance.Init("DB/MobAI.xml",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));

                        PartnerAIFactory.Instance.Items.Clear();
                        PartnerAIFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/PartnerAI", "*.xml",
                                SearchOption.AllDirectories),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        MobAIFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/TTMobAI", "*.xml",
                                SearchOption.AllDirectories),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ProcessSettingAnnounce(client, "[系统] 怪物DB更新完毕");
                        break;
                    case "quests":
                        ProcessSettingAnnounce(client, "[系统] 任务DB更新中…");
                        QuestFactory.Instance.Reload();
                        QuestFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/Quests/", "QuestDB_*",
                                SearchOption.TopDirectoryOnly),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ProcessSettingAnnounce(client, "[系统] 任务DB更新完毕");
                        break;
                    case "treasure":
                        ProcessSettingAnnounce(client, "[系统] 宝箱DB更新中…");
                        TreasureFactory.Instance.Reload();
                        TreasureFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/Treasure/", "*.*",
                                SearchOption.AllDirectories),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ProcessSettingAnnounce(client, "[系统] 宝箱DB更新完毕");
                        break;
                    case "spawns":
                        ProcessSettingAnnounce(client, "[系统] 怪物重生點DB更新中…");
                        MobSpawnManager.Instance.Reload();
                        ProcessSettingAnnounce(client, "[系统] 怪物重生點DB更新完毕");
                        break;
                    case "theater":
                        ProcessSettingAnnounce(client, "[系统] 计划任务更新中...");
                        TheaterFactory.Instance.Reload();
                        ProcessSettingAnnounce(client, "[系统] 计划任务更新完成");
                        break;
                    case "synthese":
                        ProcessSettingAnnounce(client, "[系统] 合成DB更新中...");
                        SyntheseFactory.Instance.Reload();
                        ProcessSettingAnnounce(client, "[系统] 合成DB更新完成");
                        break;
                    case "item":
                        ProcessSettingAnnounce(client, "[系统] 物品DB更新中…");
                        ItemAdditionFactory.Instance.Reload();
                        ItemAdditionFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/", "Addition*.csv",
                                SearchOption.TopDirectoryOnly),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ItemFactory.Instance.Reload();
                        ItemFactory.Instance.Init(
                            VirtualFileSystemManager.Instance.FileSystem.SearchFile("DB/", "item*.csv",
                                SearchOption.TopDirectoryOnly),
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        PartnerFactory.Instance.ClearPartnerEquips();
                        PartnerFactory.Instance.InitPartnerEquipDB("DB/partner_Equip.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        //SagaDB.Marionette.MarionetteFactory.Instance.Reload();
                        //SagaDB.Marionette.MarionetteFactory.Instance.Init("DB/marionette.csv", System.Text.Encoding.GetEncoding(Configuration.Instance.DBEncoding));
                        //添加活动木偶资料读取
                        ProcessSettingAnnounce(client, "[系统] 物品DB更新完毕");
                        break;
                    case "iris":
                        ProcessSettingAnnounce(client, "[系统] IRIS DB更新中…");
                        IrisCardFactory.Instance.Items.Clear();
                        IrisAbilityFactory.Instance.Items.Clear();
                        IrisCardFactory.Instance.Items.Clear();
                        IrisGachaFactory.Instance.IrisExchangeInfo.Clear();
                        IrisGachaFactory.Instance.IrisGacha.Clear();
                        IrisDrawRateFactory.Instance.DrawRate.Clear();
                        IrisAbilityFactory.Instance.Init("DB/iris_ability_vector_info.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        IrisCardFactory.Instance.Init("DB/iris_card.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        IrisGachaFactory.Instance.InitBlack("DB/iris_gacha_blank.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        IrisGachaFactory.Instance.InitWindow("DB/iris_gacha_window.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        IrisDrawRateFactory.Instance.Init("DB/irisdrawrate.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ProcessSettingAnnounce(client, "[系统] IRIS DB更新完毕");
                        break;
                    case "kuji":
                        KujiListFactory.Instance.KujiList.Clear();
                        KujiListFactory.Instance.NewKujilist.Clear();
                        KujiListFactory.Instance.InitXML("DB/KujiList.xml",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        break;
                    case "exchange":
                        ProcessSettingAnnounce(client, "[系统] 染色系统DB更新中…");
                        ExchangeFactory.Instance.ExchangeItems.Clear();
                        ExchangeFactory.Instance.Init("DB/exchange.csv",
                            Encoding.GetEncoding(Configuration.Configuration.Instance.DBEncoding));
                        ProcessSettingAnnounce(client, "[系统] 染色系统DB更新完毕");
                        break;
                    case "skillssp":
                        ProcessSettingAnnounce(client, "[系统] 技能DB更新中...");
                        SkillFactory.Instance.items.Clear();
                        SkillFactory.Instance.InitSSP("DB/effect.ssp", Encoding.Unicode);
                        ProcessSettingAnnounce(client, "[系统] 技能DB更新完成");
                        break;
                    case "skilldb":
                        ProcessSettingAnnounce(client, "[系统] 外部技能DB更新中...");
                        ClientManager.NoCheckDeadLock = true;
                        try
                        {
                            SkillHandler.Instance.skillHandlers.Clear();
                            SkillHandler.Instance.LoadSkill("./Skills");
                            SkillHandler.Instance.Init();
                        }
                        catch
                        {
                        }

                        ClientManager.NoCheckDeadLock = false;
                        ProcessSettingAnnounce(client, "[系统] 外部技能DB更新完成");
                        break;
                    default:
                        client.SendSystemMessage("用法：monster,item,quest,shopdb,spawns,exchange");
                        //ProcessSettingAnnounce(client, "Reloading Configs");
                        //Configuration.Instance.Initialization("./Config/SagaMap.xml");
                        //ProcessSettingAnnounce(client, "Reloaded Configs");
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ProcessReloadScript(MapClient client, string args)
        {
            //ProcessAnnounce(client, "Reloading Scripts");
            try
            {
                ScriptManager.Instance.ReloadScript();
            }
            catch (Exception ex)
            {
                client.SendSystemMessage(ex.ToString());
            }
            //ProcessAnnounce(client, "Reloaded Scripts");
        }

        private void ProcessReloadSkillDB(MapClient client, string args)
        {
            ProcessAnnounce(client, "Reloading SkillDB");
            try
            {
                SkillFactory.Instance.ReloadSkillDB();
            }
            catch (Exception ex)
            {
                client.SendSystemMessage(ex.ToString());
            }

            ProcessAnnounce(client, "Reloaded SkillDB");
        }

        private void ProcessEffect(MapClient client, string args)
        {
            var arg = new EffectArg();
            arg.effectID = uint.Parse(args);
            arg.actorID = client.Character.ActorID;
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, client.Character, true);
        }

        private void ProcessEffect2(MapClient client, string args)
        {
            var arg = new EffectArg();
            arg.effectID = uint.Parse(args);
            arg.x = Global.PosX16to8(client.Character.X, client.map.Width);
            arg.y = Global.PosY16to8(client.Character.Y, client.map.Height);
            //arg.actorID = client.Character.ActorID;
            client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, client.Character, true);
        }

        private void ProcessRobot(MapClient client, string args)
        {
            if (client.AI == null)
            {
                client.AI = new MobAI(client.Character);
                client.AI.Mode = new AIMode();
                client.AI.Mode.mask.SetValue(AIFlag.Active, true);
            }

            if (client.AI.Activated)
                client.AI.Pause();
            else
                client.AI.Start();
        }

        private void ProcessMode(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_MODE_PARA);
            else
                try
                {
                    switch (args)
                    {
                        case "1":
                            foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendPkMode();
                            ProcessAnnounce(client, LocalManager.Instance.Strings.ATCOMMAND_PK_MODE_INFO);
                            break;

                        case "2":
                            foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendNormalMode();
                            ProcessAnnounce(client, LocalManager.Instance.Strings.ATCOMMAND_NORMAL_MODE_INFO);
                            break;
                    }
                }
                catch (Exception)
                {
                }
        }

        private void ProcessInfo(MapClient client, string args)
        {
            byte x, y;
            x = Global.PosX16to8(client.Character.X, client.map.Width);
            y = Global.PosY16to8(client.Character.Y, client.map.Height);
            client.SendSystemMessage(client.Map.Name + " [" + x + "," + y + "]");
            client.SendSystemMessage("Fire:" + client.map.Info.fire[x, y]);
            client.SendSystemMessage("Wind:" + client.map.Info.wind[x, y]);
            client.SendSystemMessage("Water:" + client.map.Info.water[x, y]);
            client.SendSystemMessage("Earth:" + client.map.Info.earth[x, y]);
            client.SendSystemMessage("Holy:" + client.map.Info.holy[x, y]);
            client.SendSystemMessage("Dark:" + client.map.Info.dark[x, y]);
        }

        private void ProcessJump(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_JUMP_PARA);
            else
                try
                {
                    uint n_Mapid;
                    short n_X, n_Y;
                    n_X = client.Character.X;
                    n_Y = client.Character.Y;
                    n_Mapid = client.Character.MapID;
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.Name == args
                        select c;
                    client = chr.First();
                    client.Map.SendActorToMap(client.Character, n_Mapid, n_X, n_Y);
                }
                catch (Exception)
                {
                }
        }

        private void ProcessJump2(MapClient client, string args)
        {
            if (args == "")
            {
            }
            else
            {
                try
                {
                    uint n_Mapid;
                    short n_X, n_Y;
                    n_X = client.Character.X;
                    n_Y = client.Character.Y;
                    n_Mapid = client.Character.MapID;
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.CharID == uint.Parse(args)
                        select c;
                    client = chr.First();
                    client.Map.SendActorToMap(client.Character, n_Mapid, n_X, n_Y);
                }
                catch (Exception)
                {
                }
            }
        }

        private void ProcessBan(MapClient client, string args)
        {
            if (args != "")
                try
                {
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.Name == args
                        select c;
                    var tClient = chr.First();
                    tClient.Character.Account.Banned = true;
                    tClient.NetIo.Disconnect();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessJumpTo(MapClient client, string args)
        {
            if (args == "")
            {
            }
            else
            {
                try
                {
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.Name == args
                        select c;
                    var tClient = chr.First();
                    uint n_Mapid;
                    short n_X, n_Y;
                    n_X = tClient.Character.X;
                    n_Y = tClient.Character.Y;
                    n_Mapid = tClient.Character.MapID;
                    client.Map.SendActorToMap(client.Character, n_Mapid, n_X, n_Y);
                }
                catch (Exception)
                {
                }
            }
        }

        private void ProcessJumpTo2(MapClient client, string args)
        {
            if (args == "")
            {
            }
            else
            {
                try
                {
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.CharID == uint.Parse(args)
                        select c;
                    var tClient = chr.First();
                    uint n_Mapid;
                    short n_X, n_Y;
                    n_X = tClient.Character.X;
                    n_Y = tClient.Character.Y;
                    n_Mapid = tClient.Character.MapID;
                    client.Map.SendActorToMap(client.Character, n_Mapid, n_X, n_Y);
                }
                catch (Exception)
                {
                }
            }
        }

        private void ProcessSummon(MapClient client, string args)
        {
            var number = 1;
            uint id = 0;
            if (args != "")
            {
                switch (args.Split(' ').Length)
                {
                    case 1:
                        number = 1;
                        id = uint.Parse(args);
                        break;
                    case 2:
                        id = uint.Parse(args.Split(' ')[0]);
                        number = int.Parse(args.Split(' ')[1]);
                        break;
                    default:
                        number = 1;
                        uint.Parse(args);
                        break;
                }

                try
                {
                    for (var i = 1; i <= number; i++)
                    {
                        var mob = client.map.SpawnMob(id,
                            (short)(client.Character.X + new Random().Next(1, 10)),
                            (short)(client.Character.Y + new Random().Next(1, 10)),
                            2500,
                            client.Character);
                        client.Character.SettledSlave.Add(mob);
                    }
                }
                catch
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_MOB_ERROR);
                }
            }
        }

        private void ProcessSummonMe(MapClient client, string args)
        {
            var pc = client.Character;
            var actor = new ActorShadow(pc);
            var map = MapManager.Instance.GetMap(pc.MapID);
            actor.Name = LocalManager.Instance.Strings.SKILL_DECOY + pc.Name;
            actor.MapID = pc.MapID;
            actor.X = pc.X;
            actor.Y = pc.Y;
            actor.MaxHP = pc.MaxHP;
            actor.HP = pc.HP;
            actor.Speed = pc.Speed;
            actor.range = 1;
            var eh = new PetEventHandler(actor);
            actor.e = eh;

            eh.AI.Mode = new AIMode(1);
            eh.AI.Master = pc;
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            map.SendVisibleActorsToActor(actor);
            eh.AI.Start();
        }

        private void ProcessGridMob(MapClient client, string args)
        {
            var number = 1;
            uint id = 0;
            if (args != "")
            {
                switch (args.Split(' ').Length)
                {
                    case 1:
                        number = 1;
                        id = uint.Parse(args);
                        break;
                    case 2:
                        id = uint.Parse(args.Split(' ')[0]);
                        number = int.Parse(args.Split(' ')[1]);
                        break;
                    default:
                        number = 1;
                        uint.Parse(args);
                        break;
                }

                try
                {
                    var X = client.Character.X;
                    var Y = client.Character.Y;
                    for (var x = X - number * 100; x <= X + number * 100; x += 100)
                        for (var y = Y - number * 100; y <= Y + number * 100; y += 100)
                            if (!(X == x && Y == y))
                            {
                                var m = client.map.SpawnMob(id,
                                    (short)x,
                                    (short)y,
                                    50,
                                    null);
                                var mh = (MobEventHandler)m.e;
                                mh.AI.Mode = new AIMode(4);
                            }
                }
                catch
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_MOB_ERROR);
                }
            }
        }

        private void ProcessMob(MapClient client, string args)
        {
            var number = 1;
            uint id = 0;
            if (args != "")
            {
                switch (args.Split(' ').Length)
                {
                    case 1:
                        number = 1;
                        id = uint.Parse(args);
                        break;
                    case 2:
                        id = uint.Parse(args.Split(' ')[0]);
                        number = int.Parse(args.Split(' ')[1]);
                        break;
                    default:
                        number = 1;
                        uint.Parse(args);
                        break;
                }

                try
                {
                    for (var i = 1; i <= number; i++)
                        client.map.SpawnMob(id,
                            (short)(client.Character.X + new Random().Next(1, 10)),
                            (short)(client.Character.Y + new Random().Next(1, 10)),
                            2500,
                            null);
                }
                catch (Exception ex)
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_MOB_ERROR);
                }
            }
        }

        private void ProcessGoldTo(MapClient client, string args)
        {
            string name;
            uint gold = 0;
            //SagaLib.ClientManager.LeaveCriticalArea();
            if (args == "") client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            if (args.Split(' ').Length != 2)
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            try
            {
                name = args.Split(' ')[0];
                gold = uint.Parse(args.Split(' ')[1]);
                var cp = (MapClient)MapClientManager.Instance.GetClientForName(name);
                if (cp == null)
                {
                    client.SendSystemMessage("错误");
                    return;
                }

                if (gold > 0)
                {
                    cp.Character.Gold += gold;
                    cp.SendGoldUpdate();

                    client.SendSystemMessage("给" + name + " " + gold + " G");
                    cp.SendSystemMessage(client.Character.Name + " 给 " + name + " " + gold + " G");
                }
                else
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_NO_SUCH_ITEM);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessItemTo(MapClient client, string args)
        {
            string name;
            int number;
            uint id = 0;
            uint picid = 0;
            //SagaLib.ClientManager.LeaveCriticalArea();
            if (args == "") client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            if (args.Split(' ').Length != 3)
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            try
            {
                name = args.Split(' ')[0];
                id = uint.Parse(args.Split(' ')[1]);
                number = int.Parse(args.Split(' ')[2]);
                var item = ItemFactory.Instance.GetItem(id);
                var cp = (MapClient)MapClientManager.Instance.GetClientForName(name);
                if (cp == null)
                {
                    client.SendSystemMessage("错误");
                    return;
                }

                if (item != null)
                {
                    item.Stack = (ushort)number;
                    if (picid != 0) item.PictID = picid;
                    cp.AddItem(item, true);
                    client.SendSystemMessage("给" + name + " " + item.BaseData.name + " " + number + " 个");
                }
                else
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_NO_SUCH_ITEM);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ProcessIDSearch(MapClient client, string args)
        {
            if (args == "")
            {
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_IDSEARCH);
                return;
            }

            try
            {
                var value = from x in ItemFactory.Instance.Items
                            where x.Value.name.Contains(args)
                            orderby x.Key descending
                            select new { ItemID = x.Key, ItemName = x.Value.name };


                var coll = value.ToList();
                if (coll.Count == 0)
                {
                    client.SendSystemMessage(string.Format("未找到任何道具名字包含: {0}", args));
                    return;
                }

                var max = coll.Count;
                if (max > 10)
                    max = 10;
                client.SendSystemMessage("-----------所查询的道具ID[最多只显示10条]-----------");
                for (var i = 0; i < max; i++)
                    client.SendSystemMessage(string.Format("{0}. {1}   {2}", i + 1, coll[i].ItemName, coll[i].ItemID));
                client.SendSystemMessage("----------------------------------------------------");
            }
            catch (Exception)
            {
            }
        }

        private void ProcessItem(MapClient client, string args)
        {
            int number;
            uint id = 0;
            uint picid = 0;
            //SagaLib.ClientManager.LeaveCriticalArea();
            if (args == "")
            {
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            }
            else if (args == "food")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.FOOD)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "ufood")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.UNION_FOOD)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "cube")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.UNION_ACTCUBE)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "partner")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.PARTNER || item.Value.itemType == ItemType.RIDE_PARTNER ||
                        item.Value.itemType == ItemType.RIDE_PET ||
                        item.Value.itemType == ItemType.RIDE_PET_ROBOT || item.Value.itemType == ItemType.PET ||
                        item.Value.itemType == ItemType.PET_NEKOMATA)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "socks")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    /*if (item.Value.itemType != ItemType.POTION && item.Value.itemType != ItemType.NONE && item.Value.itemType != ItemType.CSWAR_MARIO
                        && item.Value.itemType != ItemType.FOOD && item.Value.itemType != ItemType.SEED && item.Value.itemType != ItemType.FREESCROLL
                            && item.Value.itemType != ItemType.TREASURE_BOX && item.Value.itemType != ItemType.CONTAINER && item.Value.itemType != ItemType.TIMBER_BOX
                            && item.Value.itemType != ItemType.ARROW && item.Value.itemType != ItemType.USE && item.Value.itemType != ItemType.SCROLL
                        && item.Value.itemType != ItemType.STAMP && item.Value.itemType != ItemType.SCROLL && item.Value.itemType != ItemType.FG_GARDEN_MODELHOUSE
                            && item.Value.itemType != ItemType.FG_GARDEN_FLOOR && item.Value.itemType != ItemType.FG_ROOM_WALL && item.Value.itemType != ItemType.FURNITURE
                            && item.Value.itemType != ItemType.IRIS_CARD && item.Value.itemType != ItemType.DEMIC_CHIP)*/
                    if (item.Value.itemType == ItemType.SOCKS)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "233")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    /*if (item.Value.itemType != ItemType.POTION && item.Value.itemType != ItemType.NONE && item.Value.itemType != ItemType.CSWAR_MARIO
                        && item.Value.itemType != ItemType.FOOD && item.Value.itemType != ItemType.SEED && item.Value.itemType != ItemType.FREESCROLL
                            && item.Value.itemType != ItemType.TREASURE_BOX && item.Value.itemType != ItemType.CONTAINER && item.Value.itemType != ItemType.TIMBER_BOX
                            && item.Value.itemType != ItemType.ARROW && item.Value.itemType != ItemType.USE && item.Value.itemType != ItemType.SCROLL
                        && item.Value.itemType != ItemType.STAMP && item.Value.itemType != ItemType.SCROLL && item.Value.itemType != ItemType.FG_GARDEN_MODELHOUSE
                            && item.Value.itemType != ItemType.FG_GARDEN_FLOOR && item.Value.itemType != ItemType.FG_ROOM_WALL && item.Value.itemType != ItemType.FURNITURE
                            && item.Value.itemType != ItemType.IRIS_CARD && item.Value.itemType != ItemType.DEMIC_CHIP)*/
                    if (item.Value.itemType == ItemType.IRIS_CARD)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "2333")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType != ItemType.POTION && item.Value.itemType != ItemType.NONE &&
                        item.Value.itemType != ItemType.CSWAR_MARIO
                        && item.Value.itemType != ItemType.FOOD && item.Value.itemType != ItemType.SEED &&
                        item.Value.itemType != ItemType.FREESCROLL
                        && item.Value.itemType != ItemType.TREASURE_BOX && item.Value.itemType != ItemType.CONTAINER &&
                        item.Value.itemType != ItemType.TIMBER_BOX
                        && item.Value.itemType != ItemType.ARROW && item.Value.itemType != ItemType.USE &&
                        item.Value.itemType != ItemType.SCROLL
                        && item.Value.itemType != ItemType.STAMP && item.Value.itemType != ItemType.SCROLL &&
                        item.Value.itemType != ItemType.FG_GARDEN_MODELHOUSE
                        && item.Value.itemType != ItemType.FG_GARDEN_FLOOR &&
                        item.Value.itemType != ItemType.FG_ROOM_WALL && item.Value.itemType != ItemType.FURNITURE
                        && item.Value.itemType != ItemType.IRIS_CARD && item.Value.itemType != ItemType.DEMIC_CHIP)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "23333")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (!(item.Value.itemType != ItemType.POTION
                          && item.Value.itemType != ItemType.FOOD && item.Value.itemType != ItemType.SEED
                          && item.Value.itemType != ItemType.ARROW && item.Value.itemType != ItemType.SCROLL
                          && item.Value.itemType != ItemType.STAMP))
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "233333")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.NONE || item.Value.itemType == ItemType.USE ||
                        item.Value.itemType == ItemType.FOOD ||
                        item.Value.itemType == ItemType.POTION || item.Value.itemType == ItemType.SEED ||
                        item.Value.itemType == ItemType.SEED ||
                        item.Value.itemType == ItemType.SCROLL)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "furniture")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.FURNITURE || item.Value.itemType == ItemType.FF_ROOM_FLOOR)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "shoes")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.BOOTS || item.Value.itemType == ItemType.LONGBOOTS ||
                        item.Value.itemType == ItemType.SHOES ||
                        item.Value.itemType == ItemType.HALFBOOTS)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "clothes")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.ARROW || item.Value.itemType == ItemType.ARMOR_UPPER ||
                        item.Value.itemType == ItemType.ARMOR_LOWER ||
                        item.Value.itemType == ItemType.ONEPIECE || item.Value.itemType == ItemType.COSTUME ||
                        item.Value.itemType == ItemType.BODYSUIT ||
                        item.Value.itemType == ItemType.WEDDING || item.Value.itemType == ItemType.OVERALLS ||
                        item.Value.itemType == ItemType.FACEBODYSUIT ||
                        item.Value.itemType == ItemType.SLACKS)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "c1")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (
                        item.Value.itemType == ItemType.ONEPIECE || item.Value.itemType == ItemType.COSTUME)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "c2")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.BODYSUIT ||
                        item.Value.itemType == ItemType.WEDDING || item.Value.itemType == ItemType.OVERALLS ||
                        item.Value.itemType == ItemType.FACEBODYSUIT ||
                        item.Value.itemType == ItemType.SLACKS)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "c3")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.ARROW || item.Value.itemType == ItemType.ARMOR_UPPER)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "c4")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.ARMOR_LOWER)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "stamp")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.STAMP)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "accesory")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.ACCESORY_NECK || item.Value.itemType == ItemType.BACKPACK ||
                        item.Value.itemType == ItemType.ACCESORY_FINGER ||
                        item.Value.itemType == ItemType.SOCKS || item.Value.itemType == ItemType.EFFECT ||
                        item.Value.itemType == ItemType.HELM ||
                        item.Value.itemType == ItemType.JOINT_SYMBOL || item.Value.itemType == ItemType.ACCESORY_FACE ||
                        item.Value.itemType == ItemType.ACCESORY_HEAD)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "weapons")
            {
                foreach (var item in ItemFactory.Instance.Items)
                    if (item.Value.itemType == ItemType.CLAW || item.Value.itemType == ItemType.HAMMER ||
                        item.Value.itemType == ItemType.STAFF ||
                        item.Value.itemType == ItemType.SWORD || item.Value.itemType == ItemType.AXE ||
                        item.Value.itemType == ItemType.SPEAR ||
                        item.Value.itemType == ItemType.BOW || item.Value.itemType == ItemType.GUN ||
                        item.Value.itemType == ItemType.ETC_WEAPON ||
                        item.Value.itemType == ItemType.ACCESORY_FINGER ||
                        item.Value.itemType == ItemType.SHORT_SWORD || item.Value.itemType == ItemType.RAPIER ||
                        item.Value.itemType == ItemType.STRINGS || item.Value.itemType == ItemType.BOOK ||
                        item.Value.itemType == ItemType.DUALGUN ||
                        item.Value.itemType == ItemType.RIFLE || item.Value.itemType == ItemType.THROW ||
                        item.Value.itemType == ItemType.ROPE ||
                        item.Value.itemType == ItemType.CARD || item.Value.itemType == ItemType.SHIELD)
                    {
                        var i = new Item(item.Value);
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
            }
            else if (args == "hair")
            {
                foreach (var item in HairFactory.Instance.Hairs)
                {
                    var i = ItemFactory.Instance.GetItem(item.ItemID);
                    if (i != null)
                    {
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
                }
            }
            else if (args == "color")
            {
                var colors = new List<uint>();
                uint ids = 10031301;
                for (var i = 0; i < 32; i++)
                {
                    colors.Add(ids);
                    ids++;
                }

                colors.Add(10031364);
                colors.Add(10031365);
                colors.Add(10031366);
                colors.Add(10031367);
                colors.Add(10031368);
                foreach (var item in colors)
                {
                    var i = ItemFactory.Instance.GetItem(item);
                    if (i != null)
                    {
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
                }
            }
            else if (args == "face")
            {
                foreach (var item in FaceFactory.Instance.FaceItemIDList)
                {
                    var i = ItemFactory.Instance.GetItem(item);
                    if (i != null)
                    {
                        i.Durability = i.BaseData.durability;
                        i.Stack = 1;
                        i.Identified = true;
                        client.AddItem(i, true, false);
                    }
                }
            }
            else if (args == "clear")
            {
                /*Dictionary<uint, ushort> items = new Dictionary<uint, ushort>();
                foreach (var item in client.Character.Inventory.Items[ContainerType.BODY])
                {
                    items.Add(item.Slot, item.Stack);
                    client.DeleteItem(item.Slot, item.Stack, true);
                }*/
                var count = client.Character.Inventory.Items[ContainerType.BODY].Count;
                for (var i = 0; i < count; i++)
                    client.DeleteItem(client.Character.Inventory.Items[ContainerType.BODY][0].Slot,
                        client.Character.Inventory.Items[ContainerType.BODY][0].Stack, true);
            }
            else if (args == "clearlogout")
            {
                client.Character.Inventory.Items[ContainerType.BODY].Clear();
                client.NetIo.Disconnect();
            }
            else if (args == "clearware")
            {
                client.Character.Inventory.WareHouse.Clear();
            }
            else
            {
                try
                {
                    switch (args.Split(' ').Length)
                    {
                        case 1:
                            number = 1;
                            id = uint.Parse(args);
                            break;
                        case 2:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            break;
                        case 3:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            picid = uint.Parse(args.Split(' ')[2]);
                            break;
                        default:
                            number = 1;
                            uint.Parse(args);
                            break;
                    }

                    var item = ItemFactory.Instance.GetItem(id);
                    if (item != null)
                    {
                        if (item.Stackable)
                        {
                            item.Stack = (ushort)number;
                            if (picid != 0) item.PictID = picid;
                            client.AddItem(item, true, false);
                        }
                        else
                        {
                            for (var i = 0; i < number; i++)
                            {
                                item.Stack = 1;
                                if (picid != 0) item.PictID = picid;
                                client.AddItem(item, true, false);
                            }
                        }
                    }
                    else
                    {
                        client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_NO_SUCH_ITEM);
                    }
                }
                catch (Exception)
                {
                }
                //SagaLib.ClientManager.EnterCriticalArea();
            }
        }

        private void ProcessSSMode(MapClient client, string args)
        {
            var p = new SSMG_NPC_SS_MODE();
            switch (args)
            {
                case "1":

                    p.Toggle = 1;
                    p.UI = 0;
                    p.X = 255;
                    p.Y = 255;
                    p.unknown = 1;
                    client.NetIo.SendPacket(p);
                    break;
                case "0":

                    p.Toggle = 0;
                    p.UI = 0;
                    p.X = 255;
                    p.Y = 255;
                    p.unknown = 1;
                    client.NetIo.SendPacket(p);
                    break;
            }
        }

        private void ProcessAAVoice(MapClient client, string args)
        {
            var p = new SSMG_AAA_VOICE();
            p.VoiceID = ushort.Parse(args);
            client.NetIo.SendPacket(p);
        }

        private void ProcessNPCVoice(MapClient client, string args)
        {
            var p = new SSMG_NPC_VOICE_PLAY();
            p.VoiceID = ushort.Parse(args);
            p.Loop = 0;
            client.NetIo.SendPacket(p);
        }

        private void ProcessMapObject(MapClient client, string args)
        {
            var pc = client.Character;
            byte x, y;
            x = Global.PosX16to8(pc.X, client.map.Width);
            y = Global.PosY16to8(pc.Y, client.map.Height);


            client.SendSystemMessage("當前座標資訊： [" + x + "," + y + "]");
            client.SendSystemMessage("-----地圖屬性-----");
            client.SendSystemMessage("水: " + client.Map.Info.water[x, y]);
            client.SendSystemMessage("火: " + client.Map.Info.fire[x, y]);
            client.SendSystemMessage("地: " + client.Map.Info.earth[x, y]);
            client.SendSystemMessage("風: " + client.Map.Info.wind[x, y]);
            client.SendSystemMessage("暗: " + client.Map.Info.dark[x, y]);
            client.SendSystemMessage("光: " + client.Map.Info.holy[x, y]);
            client.SendSystemMessage("無: " + client.Map.Info.neutral[x, y]);
            client.SendSystemMessage("-----基本----");
            client.SendSystemMessage("可移動: " + client.Map.Info.walkable[x, y]);
            client.SendSystemMessage("可釣魚: " + client.Map.Info.canfish[x, y]);
            client.SendSystemMessage("安全區: " + client.Map.Info.Healing);
            client.SendSystemMessage("水: " + client.Map.Info.Wet);
            client.SendSystemMessage("熱: " + client.Map.Info.Hot);
            client.SendSystemMessage("冷: " + client.Map.Info.Cold);
            client.SendSystemMessage("-----其他-----");

            client.SendSystemMessage("Un_Address: " + client.Map.Info.unknown[x, y]);
            client.SendSystemMessage("14_Address: " + client.Map.Info.unknown14[x, y]);
            client.SendSystemMessage("15_Address: " + client.Map.Info.unknown15[x, y]);
            client.SendSystemMessage("16_Address: " + client.Map.Info.unknown16[x, y]);
            client.SendSystemMessage("Events :" + client.Map.Info.events.Values);
        }

        private void ProcessItemClear(MapClient client, string args)
        {
            client.CleanItem();
        }

        private void ProcessEP(MapClient client, string args)
        {
            var points = 0u;
            if (args == "")
                points = 30u;
            else
                points = uint.Parse(args);
            var pc = client.Character;
            pc.EP = points;
        }

        private void ProcessItem2(MapClient client, string args)
        {
            int number;
            uint id = 0;
            uint picid = 0;
            var refine = 0;
            var lifeench = 0;
            var powerench = 0;
            var critench = 0;
            var magench = 0;
            var carslot = 0;
            var identity = 0;
            byte dye = 0;
            //SagaLib.ClientManager.LeaveCriticalArea();
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_PARA);
            else
                try
                {
                    switch (args.Split(' ').Length)
                    {
                        case 1:
                            number = 1;
                            id = uint.Parse(args);
                            break;
                        case 2:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            break;
                        case 3:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            picid = uint.Parse(args.Split(' ')[2]);
                            break;
                        case 9:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            refine = int.Parse(args.Split(' ')[2]);
                            identity = int.Parse(args.Split(' ')[3]);
                            carslot = int.Parse(args.Split(' ')[4]);
                            lifeench = int.Parse(args.Split(' ')[5]);
                            powerench = int.Parse(args.Split(' ')[6]);
                            critench = int.Parse(args.Split(' ')[7]);
                            magench = int.Parse(args.Split(' ')[8]);
                            break;
                        case 10:
                            id = uint.Parse(args.Split(' ')[0]);
                            number = int.Parse(args.Split(' ')[1]);
                            refine = int.Parse(args.Split(' ')[2]);
                            identity = int.Parse(args.Split(' ')[3]);
                            carslot = int.Parse(args.Split(' ')[4]);
                            lifeench = int.Parse(args.Split(' ')[5]);
                            powerench = int.Parse(args.Split(' ')[6]);
                            critench = int.Parse(args.Split(' ')[7]);
                            magench = int.Parse(args.Split(' ')[8]);
                            dye = byte.Parse(args.Split(' ')[9]);
                            break;
                        default:
                            number = 1;
                            uint.Parse(args);
                            break;
                    }

                    var item = ItemFactory.Instance.GetItem(id);
                    if (item != null)
                    {
                        item.Stack = (ushort)number;
                        item.Refine = (ushort)refine;
                        item.Identified = identity == 1;
                        item.CurrentSlot = (byte)carslot;
                        item.LifeEnhance = (byte)lifeench;
                        item.PowerEnhance = (byte)powerench;
                        item.CritEnhance = (byte)critench;
                        item.MagEnhance = (byte)magench;
                        item.Potential = true;
                        item.Release = true;
                        item.Old = true;
                        item.Str += 3;
                        item.Dye = dye;
                        item.HP = StatusFactory.Instance.GetEnhanceBonus(item, 0);
                        item.Def = StatusFactory.Instance.GetEnhanceBonus(item, 1);
                        item.MDef = StatusFactory.Instance.GetEnhanceBonus(item, 3);
                        item.Atk1 = item.Atk2 = item.Atk3 = StatusFactory.Instance.GetEnhanceBonus(item, 1);
                        item.MAtk = StatusFactory.Instance.GetEnhanceBonus(item, 1);
                        item.HitCritical = StatusFactory.Instance.GetEnhanceBonus(item, 2);
                        item.AvoidCritical = StatusFactory.Instance.GetEnhanceBonus(item, 2);
                        if (picid != 0) item.PictID = picid;
                        client.AddItem(item, true);
                        client.SendItemInfo(item);
                    }
                    else
                    {
                        client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ITEM_NO_SUCH_ITEM);
                    }
                }
                catch (Exception ex)
                {
                    Logger.ShowError(ex.Message);
                }
            //SagaLib.ClientManager.EnterCriticalArea();
        }

        private void ProcessWho(MapClient client, string args)
        {
            /*
            byte x, y;

            x = Global.PosX16to8(client.Character.X, client.map.Width);
            y = Global.PosY16to8(client.Character.Y, client.map.Height);
            client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO + MapClientManager.Instance.OnlinePlayer.Count.ToString());
            client.SendSystemMessage(client.map.ID + ",[" + client.Character.X + "," + client.Character.Y + "],[" + x.ToString() + "," + y.ToString() + "]");
            client.SendSystemMessage("当前在线IP：" + MapClientManager.Instance.OnlinePlayerOnlyIP.Count.ToString());
            */

            client.SendSystemMessage("当前在线IP：" + MapClientManager.Instance.OnlinePlayerOnlyIP.Count);
        }

        private void ProcessWho2(MapClient client, string args)
        {
            foreach (var i in MapClientManager.Instance.OnlinePlayer)
            {
                byte x, y;
                x = Global.PosX16to8(i.Character.X, i.map.Width);
                y = Global.PosY16to8(i.Character.Y, i.map.Height);
                var ip = "{IP:" + i.Character.Account.LastIP + "}";
                if (i.Character.Account.GMLevel > 100)
                    ip = "{IP: 无法获取}";
                var mac = "[MAC:" + i.Character.Account.MacAddress + "]";

                byte count = 0;
                foreach (var j in MapClientManager.Instance.OnlinePlayer)
                    if (j.Character.Account.LastIP == i.Character.Account.LastIP && j.Character.Account.GMLevel < 20)
                        count++;
                byte count2 = 0;
                foreach (var j in MapClientManager.Instance.OnlinePlayer)
                    if (j.Character.Account.MacAddress == i.Character.Account.MacAddress &&
                        j.Character.Account.GMLevel < 20)
                        count2++;
                client.SendSystemMessage(i.Character.Name + "(AccountID:" + i.Character.Account.AccountID + ",CharID:" +
                                         i.Character.CharID + ",ActorID:" + i.Character.ActorID + ")" +
                                         "[" + i.Map.Name + " " + x + "," + y + "," + i.Map.ID + "] " + ip + mac +
                                         " 同IP号数：" + count + " 同MAC号数：" + count2);
            }

            client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ONLINE_PLAYER_INFO +
                                     MapClientManager.Instance.OnlinePlayer.Count);
            client.SendSystemMessage("当前在线IP：" + MapClientManager.Instance.OnlinePlayerOnlyIP.Count);
        }

        private void ProcessWho3(MapClient client, string args)
        {
            var ranged = -1;
            if (args != "")
                try
                {
                    ranged = int.Parse(args);
                }
                catch (Exception)
                {
                }

            try
            {
                var actors = new List<Actor>();
                if (ranged == -1)
                    foreach (var a in client.map.Actors)
                        actors.Add(a.Value);
                else
                    actors = client.map.GetActorsArea(client.Character, (short)(ranged * 100), true);

                foreach (var act in actors)
                {
                    byte x, y;
                    x = Global.PosX16to8(act.X, client.map.Width);
                    y = Global.PosY16to8(act.Y, client.map.Height);
                    switch (act.type)
                    {
                        case ActorType.MOB:
                            var mob = (ActorMob)act;
                            client.SendSystemMessage(mob.BaseData.name + "(ActorID:" + mob.ActorID + ")[" + x + "," +
                                                     y + "]");
                            break;
                        case ActorType.PC:
                            var pc = (ActorPC)act;
                            client.SendSystemMessage(pc.Name + "(ActorID:" + pc.ActorID + ")(CharID:" + pc.CharID +
                                                     ")[" + x + "," + y + "]");
                            break;
                        case ActorType.PET:
                            var pet = (ActorPet)act;
                            client.SendSystemMessage(pet.BaseData.name + "(ActorID:" + pet.ActorID + ")[" + x + "," +
                                                     y + "]");
                            break;
                        case ActorType.SHADOW:
                            var sw = (ActorShadow)act;
                            client.SendSystemMessage(sw.Name + "(ActorID:" + sw.ActorID + ")[" + x + "," + y + "]");
                            break;
                        case ActorType.ITEM:
                            var itm = (ActorItem)act;
                            client.SendSystemMessage(itm.Name + "(ActorID:" + itm.ActorID + ")[" + x + "," + y + "]");
                            break;
                        case ActorType.FURNITURE:
                            var fi = (ActorFurniture)act;
                            client.SendSystemMessage(fi.Name + "(ActorID:" + fi.ActorID + ")[" + x + "," + y + "," +
                                                     fi.Z + "]");
                            break;
                        case ActorType.GOLEM:
                            var go = (ActorGolem)act;
                            client.SendSystemMessage(go.Name + "(ActorID:" + go.ActorID + ")[" + x + "," + y + "]");
                            break;
                        case ActorType.PARTNER:
                            var partner = (ActorPartner)act;
                            client.SendSystemMessage(partner.Name + "(ActorID:" + partner.ActorID + " | PID:" +
                                                     partner.BaseData.id + ")[" + x + "," + y + "]");
                            break;
                    }
                }

                client.SendSystemMessage(string.Format("共：{0} 個Actors", actors.Count));
            }
            catch
            {
            }
        }


        private void ProcessGo(MapClient client, string args)
        {
            var number = uint.Parse(args);
            uint mapid;
            byte x;
            byte y;
            switch (number)
            {
                case 1:
                    mapid = 10024000;
                    x = 127;
                    y = 141;
                    break;
                case 2:
                    mapid = 10023000;
                    x = 127;
                    y = 144;
                    break;
                case 3:
                    mapid = 30082000;
                    x = 15;
                    y = 10;
                    break;
                default:
                    return;
            }

            try
            {
                if (Configuration.Configuration.Instance.HostedMaps.Contains(mapid))
                {
                    var newMap = MapManager.Instance.GetMap(mapid);
                    client.Map.SendActorToMap(client.Character, mapid, Global.PosX8to16(x, newMap.Width),
                        Global.PosY8to16(y, newMap.Height));
                }
            }
            catch (Exception)
            {
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_WARP_ERROR);
            }
        }

        private void ProcessFG(MapClient client, string args)
        {
            SkillHandler.Instance.ShowVessel(client.Character, -100);
            if (client.Character.FlyingGarden == null)
                client.Character.FlyingGarden = new FlyingGarden(client.Character);
            var item = ItemFactory.Instance.GetItem(10022700);
            item.Stack = 1;
            item.Identified = true;
            client.AddItem(item, true);
        }

        private void ProcessWarp(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "" || arg[1] == "" || arg[2] == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_WARP_PARA);
            else
                try
                {
                    var mapid = uint.Parse(arg[0]);
                    var x = byte.Parse(arg[1]);
                    var y = byte.Parse(arg[2]);
                    if (Configuration.Configuration.Instance.HostedMaps.Contains(mapid))
                    {
                        var newMap = MapManager.Instance.GetMap(mapid);
                        client.Map.SendActorToMap(client.Character, mapid, Global.PosX8to16(x, newMap.Width),
                            Global.PosY8to16(y, newMap.Height));
                    }
                }
                catch (Exception)
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_WARP_ERROR);
                }
        }

        private void ProcessPCall(MapClient client, string args)
        {
        }

        private void ProcessHair(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg[0] == "" || arg[1] == "" || arg[2] == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIR_PAEA);
            else
                try
                {
                    var haire = ushort.Parse(arg[0]);
                    var wig = ushort.Parse(arg[1]);
                    var color = byte.Parse(arg[2]);
                    client.Character.HairStyle = haire;
                    client.Character.Wig = wig;
                    client.Character.HairColor = color;
                    client.SendCharInfoUpdate();
                }
                catch (Exception)
                {
                    client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIR_ERROR);
                }
        }

        private void ProcessHairext(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIREXT_PARA);
            else
                try
                {
                    var style = byte.Parse(args);
                    client.Character.Wig = style;
                    client.SendCharInfoUpdate();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessLevel(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_LEVEL_PARA);
            else
                try
                {
                    var lv = byte.Parse(args);
                    if (!client.Character.Rebirth)
                    {
                        client.Character.CEXP = ExperienceManager.Instance.GetExpForLevel(lv, LevelType.CLEVEL) + 1;
                        ExperienceManager.Instance.CheckExp(client, LevelType.CLEVEL);
                    }
                    else
                    {
                        client.Character.CEXP = ExperienceManager.Instance.GetExpForLevel(lv, LevelType.CLEVEL2) + 1;
                        ExperienceManager.Instance.CheckExp(client, LevelType.CLEVEL2);
                    }

                    client.Character.Level = lv;
                    client.SendEXP();
                    client.SendPlayerLevel();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessJobLevel(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_LEVEL_PARA);
            else
                try
                {
                    var lv = byte.Parse(args);
                    if (client.map.Info.Flag.Test(MapFlags.Dominion))
                    {
                        client.Character.DominionJEXP =
                            ExperienceManager.Instance.GetExpForLevel(lv, LevelType.JLEVEL2) + 1;
                        ExperienceManager.Instance.CheckExp(client, LevelType.JLEVEL2);
                        client.Character.DominionJobLevel = lv;
                    }
                    else
                    {
                        if (client.Character.DualJobID != 0)
                        {
                            client.Character.PlayerDualJobList[client.Character.DualJobID].DualJobExp =
                                ExperienceManager.Instance.GetExpForLevel(lv, LevelType.DUALJ) + 1;
                            ExperienceManager.Instance.CheckExp(client, LevelType.DUALJ);
                            client.Character.PlayerDualJobList[client.Character.DualJobID].DualJobLevel = lv;
                        }
                        else if (client.Character.Job == client.Character.JobBasic)
                        {
                            client.Character.JEXP = ExperienceManager.Instance.GetExpForLevel(lv, LevelType.JLEVEL) + 1;
                            ExperienceManager.Instance.CheckExp(client, LevelType.JLEVEL);
                            client.Character.JobLevel1 = lv;
                        }
                        else if (client.Character.Job == client.Character.Job2X ||
                                 client.Character.Job == client.Character.Job2T)
                        {
                            client.Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(lv, LevelType.JLEVEL2) + 1;
                            ExperienceManager.Instance.CheckExp(client, LevelType.JLEVEL2);
                            if (client.Character.Job == client.Character.Job2X)
                                client.Character.JobLevel2X = lv;
                            else if (client.Character.Job == client.Character.Job2T)
                                client.Character.JobLevel2T = lv;
                        }
                        else if (client.Character.Job == client.Character.Job3)
                        {
                            client.Character.JEXP =
                                ExperienceManager.Instance.GetExpForLevel(lv, LevelType.JLEVEL3) + 1;
                            ExperienceManager.Instance.CheckExp(client, LevelType.JLEVEL3);
                            client.Character.JobLevel3 = lv;
                        }
                    }

                    client.SendEXP();
                    client.SendPlayerLevel();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessHaircolor(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIRCOLOR_PARA);
            else
                try
                {
                    if (client.Character.HairStyle == 90 || client.Character.HairStyle == 91 ||
                        client.Character.HairStyle == 92)
                        client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIRCOLOR_ERROR); //
                    else
                        switch (args)
                        {
                            case "1":
                                client.Character.HairColor = 0;
                                client.SendCharInfoUpdate();
                                break;
                            case "2":
                                client.Character.HairColor = 1;
                                client.SendCharInfoUpdate();
                                break;
                            case "3":
                                client.Character.HairColor = 2;
                                client.SendCharInfoUpdate();
                                break;
                            case "4":
                                client.Character.HairColor = 3;
                                client.SendCharInfoUpdate();
                                break;
                            case "5":
                                client.Character.HairColor = 4;
                                client.SendCharInfoUpdate();
                                break;
                            case "6":
                                client.Character.HairColor = 5;
                                client.SendCharInfoUpdate();
                                break;
                            case "7":
                                client.Character.HairColor = 6;
                                client.SendCharInfoUpdate();
                                break;
                            case "8":
                                client.Character.HairColor = 7;
                                client.SendCharInfoUpdate();
                                break;
                            case "9":
                                client.Character.HairColor = 8;
                                client.SendCharInfoUpdate();
                                break;
                            case "10":
                                client.Character.HairColor = 9;
                                client.SendCharInfoUpdate();
                                break;
                            case "11":
                                client.Character.HairColor = 10;
                                client.SendCharInfoUpdate();
                                break;
                            case "12":
                                client.Character.HairColor = 11;
                                client.SendCharInfoUpdate();
                                break;
                            case "13":
                                client.Character.HairColor = 12;
                                client.SendCharInfoUpdate();
                                break;
                            case "14":
                                client.Character.HairColor = 50;
                                client.SendCharInfoUpdate();
                                break;
                            case "15":
                                client.Character.HairColor = 51;
                                client.SendCharInfoUpdate();
                                break;
                            case "16":
                                client.Character.HairColor = 52;
                                client.SendCharInfoUpdate();
                                break;
                            case "17":
                                client.Character.HairColor = 60;
                                client.SendCharInfoUpdate();
                                break;
                            case "18":
                                client.Character.HairColor = 61;
                                client.SendCharInfoUpdate();
                                break;
                            case "19":
                                client.Character.HairColor = 62;
                                client.SendCharInfoUpdate();
                                break;
                            case "20":
                                client.Character.HairColor = 70;
                                client.SendCharInfoUpdate();
                                break;
                            case "21":
                                client.Character.HairColor = 71;
                                client.SendCharInfoUpdate();
                                break;
                            case "22":
                                client.Character.HairColor = 72;
                                client.SendCharInfoUpdate();
                                break;
                        }
                }
                catch (Exception)
                {
                }
        }

        private void ProcessHairstyle(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HAIRSTYLE_PARA);
            else
                try
                {
                    switch (args)
                    {
                        case "1":
                            client.Character.HairStyle = 90;
                            client.Character.HairColor = 0;
                            client.SendCharInfoUpdate();
                            break;
                        case "2":
                            client.Character.HairStyle = 91;
                            client.Character.HairColor = 0;
                            client.SendCharInfoUpdate();
                            break;
                        case "3":
                            client.Character.HairStyle = 92;
                            client.Character.HairColor = 0;
                            client.SendCharInfoUpdate();
                            break;
                        case "4":
                            client.Character.HairStyle = 2;
                            client.SendCharInfoUpdate();
                            break;
                        case "5":
                            client.Character.HairStyle = 6;
                            client.SendCharInfoUpdate();
                            break;
                        case "6":
                            client.Character.HairStyle = 11;
                            client.SendCharInfoUpdate();
                            break;
                        case "7":
                            client.Character.HairStyle = 12;
                            client.SendCharInfoUpdate();
                            break;
                        case "8":
                            client.Character.HairStyle = 13;
                            client.SendCharInfoUpdate();
                            break;
                        case "9":
                            client.Character.HairStyle = 14;
                            client.SendCharInfoUpdate();
                            break;
                        case "10":
                            client.Character.HairStyle = 15;
                            client.SendCharInfoUpdate();
                            break;
                        case "11":
                            client.Character.HairStyle = 16;
                            client.SendCharInfoUpdate();
                            break;
                        case "12":
                            client.Character.HairStyle = 17;
                            client.SendCharInfoUpdate();
                            break;
                        case "13":
                            client.Character.HairStyle = 18;
                            client.SendCharInfoUpdate();
                            break;
                        case "14":
                            client.Character.HairStyle = 19;
                            client.SendCharInfoUpdate();
                            break;
                        case "15":
                            client.Character.HairStyle = 20;
                            client.SendCharInfoUpdate();
                            break;
                            //not working (3,4,5,7,8,9,)
                    }
                }
                catch (Exception)
                {
                }
        }

        private void ProcessPlayersize(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_PLAYERSIZE_PARA);
            else
                try
                {
                    var playersize = uint.Parse(args);
                    client.Character.Size = playersize;
                    client.SendPlayerSizeUpdate();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessShowStatus(MapClient client, string args)
        {
            try
            {
                client.SendSystemMessage("------------------Status----------------");
                var s = client.Character.Status;
                client.SendSystemMessage(string.Format("mp_recover_skill:{0}", s.mp_recover_skill));
                client.SendSystemMessage("----------------------------------------");
            }
            catch (Exception)
            {
            }
        }

        private void ProcessGold(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_GOLD_PARA);
            else
                try
                {
                    var gold = ulong.Parse(args);
                    client.Character.Gold = (long)gold;
                    client.SendGoldUpdate();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessHide(MapClient client, string args)
        {
            Actor actor = client.Character;
            actor.Buff.Transparent = !client.Character.Buff.Transparent;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void ProcessShoppoint(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_SHOPPOINT_PARA);
            else
                try
                {
                    var shopp = uint.Parse(args);
                    client.Character.VShopPoints = shopp;
                    client.Character.CP = shopp;
                }
                catch (Exception)
                {
                }
        }

        private void ProcessHeal(MapClient client, string args)
        {
            client.Character.HP = client.Character.MaxHP;
            client.Character.MP = client.Character.MaxMP;
            client.Character.SP = client.Character.MaxSP;
            client.Character.EP = client.Character.MaxEP;
            if (client.Character.Job == PC_JOB.ASTRALIST) //魔法师
                client.Character.EP = 0;
            client.Map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, client.Character, true);
            client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_HEAL_MESSAGE);
        }

        private void ProcessSkill(MapClient client, string args)
        {
            byte lv = 0;
            uint id = 0;
            if (args != "")
            {
                switch (args.Split(' ').Length)
                {
                    case 1:
                        id = uint.Parse(args);
                        break;
                    case 2:
                        id = uint.Parse(args.Split(' ')[0]);
                        lv = byte.Parse(args.Split(' ')[1]);
                        break;
                    default:
                        uint.Parse(args);
                        break;
                }

                try
                {
                    var skill = SkillFactory.Instance.GetSkill(id, lv);
                    if (skill == null) return;
                    if (lv == 0) skill.Level = skill.MaxLevel;
                    client.Character.Skills.Add(id, skill);
                    StatusFactory.Instance.CalcStatus(client.Character);
                    client.SendPlayerInfo();
                }
                catch (Exception ex)
                {
                }
            }
        }

        private void ProcessSkillALL(MapClient client, string args)
        {
            if (args != "")
                return;
            if (client.Character.JobBasic != PC_JOB.NONE && client.Character.JobBasic != PC_JOB.NOVICE)
                foreach (var item in SkillFactory.Instance.SkillList(client.Character.JobBasic).Keys)
                {
                    var skill = SkillFactory.Instance.GetSkill(item, 0);
                    if (skill == null)
                        continue;
                    skill.Level = skill.MaxLevel;
                    if (client.Character.Skills.ContainsKey(item))
                        client.Character.Skills.Remove(item);
                    client.Character.Skills.Add(item, skill);
                }

            client.Character.SkillPoint = 0;
            StatusFactory.Instance.CalcStatus(client.Character);
            client.SendPlayerInfo();
            if (client.Character.Job2X != PC_JOB.NONE && client.Character.Job2X != PC_JOB.NOVICE)
                foreach (var item in SkillFactory.Instance.SkillList(client.Character.Job2X).Keys)
                {
                    var skill = SkillFactory.Instance.GetSkill(item, 0);
                    if (skill == null)
                        continue;
                    skill.Level = skill.MaxLevel;
                    if (client.Character.Skills2_1.ContainsKey(item))
                        client.Character.Skills2_1.Remove(item);
                    client.Character.Skills2_1.Add(item, skill);
                }

            client.Character.SkillPoint2X = 0;
            StatusFactory.Instance.CalcStatus(client.Character);
            client.SendPlayerInfo();
            if (client.Character.Job2T != PC_JOB.NONE && client.Character.Job2T != PC_JOB.NOVICE)
                foreach (var item in SkillFactory.Instance.SkillList(client.Character.Job2T).Keys)
                {
                    var skill = SkillFactory.Instance.GetSkill(item, 0);
                    if (skill == null)
                        continue;
                    skill.Level = skill.MaxLevel;
                    if (client.Character.Skills2_2.ContainsKey(item))
                        client.Character.Skills2_2.Remove(item);
                    client.Character.Skills2_2.Add(item, skill);
                }

            client.Character.SkillPoint2T = 0;
            StatusFactory.Instance.CalcStatus(client.Character);
            client.SendPlayerInfo();
            if (client.Character.Job3 != PC_JOB.NONE && client.Character.Job3 != PC_JOB.NOVICE)
                foreach (var item in SkillFactory.Instance.SkillList(client.Character.Job3).Keys)
                {
                    var skill = SkillFactory.Instance.GetSkill(item, 0);
                    if (skill == null)
                        continue;
                    skill.Level = skill.MaxLevel;
                    if (client.Character.Skills3.ContainsKey(item))
                        client.Character.Skills3.Remove(item);
                    client.Character.Skills3.Add(item, skill);
                }

            client.Character.SkillPoint3 = 0;
            StatusFactory.Instance.CalcStatus(client.Character);
            client.SendPlayerInfo();
        }

        private void ProcessSkillClear(MapClient client, string args)
        {
            var type = 0;
            if (args != "") type = int.Parse(args);
            switch (type)
            {
                case 0: //all
                    client.Character.Skills.Clear();
                    client.Character.Skills2.Clear();
                    client.Character.Skills2_1.Clear();
                    client.Character.Skills2_2.Clear();
                    client.Character.Skills3.Clear();
                    client.Character.SkillsReserve.Clear();
                    break;
                case 1: //1轉
                    client.Character.Skills.Clear();
                    break;
                case 2: //2轉
                    client.Character.Skills2.Clear();
                    client.Character.Skills2_1.Clear();
                    client.Character.Skills2_2.Clear();
                    break;
                case 3:
                    client.Character.Skills3.Clear();
                    break;
                case 4: //保留技能
                    client.Character.SkillsReserve.Clear();
                    break;
            }

            StatusFactory.Instance.CalcStatus(client.Character);
            client.SendPlayerInfo();
        }

        private void ProcessAnnounce(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ANNOUNCE_PARA);
            else
                try
                {
                    foreach (var i in MapClientManager.Instance.OnlinePlayer) i.SendAnnounce(args);
                }
                catch (Exception)
                {
                }
        }

        private void ProcessSettingAnnounce(MapClient client, string args)
        {
            if (args == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ANNOUNCE_PARA);
            else
                try
                {
                    foreach (var i in MapClientManager.Instance.OnlinePlayer)
                        if (i.Character.Account.GMLevel >= 200)
                            i.SendAnnounce(args);
                }
                catch (Exception)
                {
                }
        }

        private void ProcessSetShop(MapClient client, string args)
        {
            //if (client.Character.Account.AccountID <= 247)
            //{
            //    client.SendSystemMessage("老账号暂时无法交易哦，等确认到没有BUG再开！");
            //    return;
            //}
            //if (client.Character.MapID != 10054000 && client.Character.Account.GMLevel < 30)
            //{
            //    client.SendSystemMessage("该地区无法开设店铺。");
            //    return;
            //}
            var p1 = new SSMG_PLAYER_SETSHOP_OPEN_SETUP();
            var p2 = new SSMG_PLAYER_SHOP_APPEAR();
            p2.ActorID = client.Character.ActorID;
            p2.Title = client.Shoptitle;
            client.Shopswitch = 0;
            p2.button = 0;
            p1.Comment = client.Shoptitle;
            client.NetIo.SendPacket(p1);
            client.NetIo.SendPacket(p2);
        }

        private void ProcessBossTime(MapClient client, string args)
        {
            try
            {
                var mobs = new List<ActorMob>();

                foreach (var item in MobFactory.Instance.BossList)
                    if (item.Tasks.ContainsKey("Respawn"))
                    {
                        mobs.Add(item);
                        client.SendSystemMessage("BOSS『" + item.Name + "』复活时间剩余：" +
                                                 (int)(item.Tasks["Respawn"].NextUpdateTime - DateTime.Now)
                                                 .TotalSeconds + "秒。");
                    }

                //else
                //client.SendSystemMessage("『" + item.Name + "』存活中。");
                if (mobs.Count < 1) client.SendSystemMessage("当前没有BOSS等待重生。");
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }

        private void ProcessQueryFame(MapClient client, string args)
        {
            client.SendSystemMessage("玩家: " + client.Character.Name + " 现有声望: " + client.Character.Fame);
        }

        private void ProcessPraise(MapClient client, string args)
        {
            var item = client.Character.Inventory.GetItem(950000040, Inventory.SearchType.ITEM_ID);
            if (item == null) return;
            if (item.Stack < 1) return;
            //if (client.Character.Level < 65)
            //{
            //    client.SendSystemMessage("等级还不足65级，还不能赞扬玩家哦。");
            //    return;
            //}
            if (args != "")
                try
                {
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.Name == args
                        select c;
                    if (chr == null)
                    {
                        client.SendSystemMessage("指定的玩家不存在或者不在线。");
                        return;
                    }

                    var tClient = chr.First();
                    if (tClient.Character.Name == client.Character.Name)
                    {
                        client.SendSystemMessage("不可以指定自己哦。");
                        return;
                    }

                    client.DeleteItemID(950000040, 1, true);
                    tClient.TitleProccess(tClient.Character, 35, 1);
                    tClient.TitleProccess(tClient.Character, 36, 1);
                    tClient.TitleProccess(tClient.Character, 37, 1);
                    item = ItemFactory.Instance.GetItem(910000115);
                    item.Stack = 30;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(910000116);
                    item.Stack = 10;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(950000000);
                    item.Stack = 3;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(950000001);
                    item.Stack = 3;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(950000025);
                    item.Stack = 20;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(950000028);
                    item.Stack = 1;
                    client.AddItem(item, true);

                    item = ItemFactory.Instance.GetItem(910000104);
                    item.Stack = 1;
                    client.AddItem(item, true);

                    foreach (var i in MapClientManager.Instance.OnlinePlayer)
                        i.SendAnnounce("恭喜萌新玩家 " + client.Character.Name + " 顺利地达到了65级，并使用了「萌新之证」，她所赞扬的玩家是：" +
                                       tClient.Character.Name);
                }
                catch (Exception)
                {
                }
        }

        private void ProcessAccept(MapClient client, string args)
        {
            if (client.Character.Status.Additions.ContainsKey("等待接受挑战") && client.Character.TInt["挑战者AID"] != 0)
            {
                if (client.Character.MapID != 10054000 || client.Character.MapID != 10054000) return;
                ActorPC target = null;
                var map = MapManager.Instance.GetMap(client.Character.MapID);
                target = (ActorPC)map.GetActor((uint)client.Character.TInt["挑战者AID"]);
                SkillHandler.RemoveAddition(client.Character, "等待接受挑战");
                if (target == null) return;
                if (target.Mode != PlayerMode.NORMAL || client.Character.Mode != PlayerMode.NORMAL) return;

                target.Mode = PlayerMode.KNIGHT_WEST;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, target, true);

                client.Character.Mode = PlayerMode.KNIGHT_EAST;

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.PLAYER_MODE, null, client.Character, true);

                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, target, true);
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.CHAR_INFO_UPDATE, null, client.Character, true);

                client.SendCharInfoUpdate();
                MapClient.FromActorPC(target).SendCharInfoUpdate();
            }
        }

        private void ProcessRefuse(MapClient client, string args)
        {
            if (client.Character.CInt["挑战开关"] == 0)
            {
                client.Character.CInt["挑战开关"] = 1;
                client.SendSystemMessage("你将自动拒绝所有挑战邀请。");
            }
            else
            {
                client.Character.CInt["挑战开关"] = 0;
                client.SendSystemMessage("你将开始接受挑战邀请。");
            }
        }

        private void ProcessAutoLock(MapClient client, string args)
        {
            if (client.Character.CInt["自动锁定模式"] == 0)
            {
                client.Character.CInt["自动锁定模式"] = 1;
                client.SendSystemMessage("自动锁定模式开启！没有锁定目标时，将会自动寻找周围目标锁定。");
            }
            else
            {
                client.Character.CInt["自动锁定模式"] = 0;
                client.SendSystemMessage("自动锁定模式关闭。");
            }
        }

        private void ProcessOpenWare(MapClient client, string args)
        {
            client.currentWarehouse = WarehousePlace.Acropolis;

            client.SendWareItems(WarehousePlace.Acropolis);
        }

        private void ProcessTrumpet(MapClient client, string args)
        {
            if (args == "")
                //client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_ANNOUNCE_PARA);
                client.SendSystemMessage("用法: /w 内容 ");
            else
                // if (client.Character.Gold >= 0)
                //{
                //client.Character.Gold -= 50;
                try
                {
                    //if (args == "我最喜欢穿小裙子！")
                    //if (MapClientManager.Instance.OnlinePlayerOnlyIP.Count >= 30)
                    //client.TitleProccess(client.Character, 34, 1);
                    foreach (var i in MapClientManager.Instance.OnlinePlayer)
                    {
                        var p = new SSMG_CHAT_WHOLE();
                        p.Sender = "[公頻]" + client.Character.Name;
                        p.Content = args;
                        i.NetIo.SendPacket(p);
                    }
                }
                catch (Exception)
                {
                }
            // }
            // else
            // client.SendSystemMessage("该功能需要50G!");
        }

        private void ProcessTweet(MapClient client, string args)
        {
            var TweetID = Configuration.Configuration.Instance.TwitterID;
            var TweetPass = Configuration.Configuration.Instance.TwitterPasswd;
            var name = client.Character.Name;
            var namesize = name.Length;
            var argssize = args.Length;
            var allsize = namesize + argssize;
            if (TweetID == null || TweetPass == null)
            {
            }

            if (args == "")
                client.SendSystemMessage("Error: NoTweetComment");
            else if (allsize >= 140)
                client.SendSystemMessage("Error: TweetSizeOver");
            else
                try
                {
                    Encoding.GetEncoding("UTF-8");
                    var user = Convert.ToBase64String(Encoding.UTF8.GetBytes(TweetID + ":" + TweetPass));
                    var bytes = Encoding.UTF8.GetBytes("status=" + name + ":" + args);

                    var request = (HttpWebRequest)HttpWebRequest.Create("http://twitter.com/statuses/update.xml");

                    request.Method = "POST";
                    request.ServicePoint.Expect100Continue = false;

                    request.UserAgent = "SagaECOJP";
                    request.Headers.Add("Authorization", "Basic " + user);
                    request.ContentType = "application/x-www-form-urlencoded";

                    request.ContentLength = bytes.Length;


                    var reqStream = request.GetRequestStream();

                    reqStream.Write(bytes, 0, bytes.Length);

                    reqStream.Close();

                    try
                    {
                        foreach (var i in MapClientManager.Instance.OnlinePlayer)
                        {
                            var p = new SSMG_CHAT_WHOLE();
                            p.Sender = "Tweet";
                            p.Content = name + ":" + args;
                            i.NetIo.SendPacket(p);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                catch (Exception)
                {
                }
        }

        private void ProcessCh(MapClient client, string args)
        {
        }

        private void ProcessEquipList(MapClient client, string args)
        {
            var pc = client.Character;
            Dictionary<EnumEquipSlot, Item> equips;
            if (pc.Form == DEM_FORM.NORMAL_FORM)
                equips = pc.Inventory.Equipments;
            else
                equips = pc.Inventory.Parts;

            if (equips.Count == 0)
            {
                client.SendSystemMessage("角色身上没有装备任何东西哦~");
                return;
            }

            client.SendSystemMessage("-------------[装备清单]--------------");
            foreach (var item in equips.Keys)
            {
                var i = equips[item];
                client.SendSystemMessage(string.Format("{0}: {1}   {2}", item.ToString(),
                    ItemFactory.Instance.Items[i.ItemID].name, i.ItemID));
            }

            client.SendSystemMessage("-------------------------------------");
        }

        private void ProcessInventoryList(MapClient client, string args)
        {
            List<Item> list;
            var inventory = client.Character.Inventory;
            list = inventory.Items[ContainerType.BODY];
            if (list.Count > 0)
            {
                client.SendSystemMessage("-------------[身体清单]--------------");
                foreach (var item in list)
                    if (item.ItemID != 0)
                        client.SendSystemMessage(string.Format("{0}: {1}   堆叠:{2}   价值:{3}   位置: {4}",
                            ItemFactory.Instance.Items[item.ItemID].name, item.ItemID, item.Stack,
                            ItemFactory.Instance.Items[item.ItemID].price, item.Slot));
            }

            list = inventory.Items[ContainerType.RIGHT_BAG];
            if (list.Count > 0)
            {
                client.SendSystemMessage("-------------[右手清单]--------------");
                foreach (var item in list)
                    if (item.ItemID != 0)
                        client.SendSystemMessage(string.Format("{0}: {1}   堆叠:{2}   价值:{3}   位置: {4}",
                            ItemFactory.Instance.Items[item.ItemID].name, item.ItemID, item.Stack,
                            ItemFactory.Instance.Items[item.ItemID].price, item.Slot));
            }

            list = inventory.Items[ContainerType.LEFT_BAG];
            if (list.Count > 0)
            {
                client.SendSystemMessage("-------------[左手清单]--------------");
                foreach (var item in list)
                    if (item.ItemID != 0)
                        client.SendSystemMessage(string.Format("{0}: {1}   堆叠:{2}   价值:{3}   位置: {4}",
                            ItemFactory.Instance.Items[item.ItemID].name, item.ItemID, item.Stack,
                            ItemFactory.Instance.Items[item.ItemID].price, item.Slot));
            }

            list = inventory.Items[ContainerType.BACK_BAG];
            if (list.Count > 0)
            {
                client.SendSystemMessage("-------------[背包清单]--------------");
                foreach (var item in list)
                    if (item.ItemID != 0)
                        client.SendSystemMessage(string.Format("{0}: {1}   堆叠:{2}   价值:{3}   位置:{4}",
                            ItemFactory.Instance.Items[item.ItemID].name, item.ItemID, item.Stack,
                            ItemFactory.Instance.Items[item.ItemID].price, item.Slot));
            }
        }

        private void ProcessFame(MapClient client, string args)
        {
            if (args == "")
            {
                client.SendSystemMessage("请输入声望的变化值");
                return;
            }

            var value = 0;
            if (args.Split(' ').Length != 1 || !int.TryParse(args, out value))
            {
                client.SendSystemMessage("命令格式不正确");
                return;
            }

            client.Character.Fame += (uint)value;
            client.SendSystemMessage("当前的声望为: " + client.Character.Fame);
        }

        private void ProcessRWarp(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (string.IsNullOrEmpty(arg[0]) || string.IsNullOrEmpty(arg[1]))
            {
                client.SendSystemMessage("用法 !rwarp Relative-x Relative-y");
                return;
            }

            short xshort, yshort;
            byte x, y;
            bool isNegativeX, isNegativeY;
            if (short.TryParse(arg[0], out xshort) && short.TryParse(arg[1], out yshort))
            {
                if (xshort < 0)
                    isNegativeX = true;
                else isNegativeX = false;
                if (yshort < 0)
                    isNegativeY = true;
                else isNegativeY = false;
                arg[0] = arg[0].Replace("-", "");
                arg[1] = arg[1].Replace("-", "");
                if (byte.TryParse(arg[0], out x) && byte.TryParse(arg[1], out y))
                {
                    byte NewX = Global.PosX16to8(client.Character.X, client.Map.Width),
                        NewY = Global.PosY16to8(client.Character.Y, client.Map.Height);
                    if (isNegativeX)
                        NewX -= x;
                    else NewX += x;
                    if (isNegativeY)
                        NewY -= y;
                    else NewY += y;
                    client.Map.TeleportActor(client.Character, Global.PosX8to16(NewX, client.Map.Width),
                        Global.PosY8to16(NewY, client.Map.Height));
                }
            }
        }

        private void ProcessIrisSearch(MapClient client, string args)
        {
            if (string.IsNullOrEmpty(args) || string.IsNullOrWhiteSpace(args))
            {
                client.SendSystemMessage("this command require two params");
                return;
            }

            if (args.Split(' ').Length != 2)
            {
                client.SendSystemMessage("this command require two params");
                return;
            }

            var type = args.Split(' ')[0];
            var param = args.Split(' ')[1];
            switch (type.ToLower())
            {
                case "name":
                    var ret = IrisCardFactory.Instance.Items.Values.Where(x => x.Name.Contains(param)).ToList();
                    if (ret.Count == 0)
                    {
                        client.SendSystemMessage("no card found with card name like " + param);
                        return;
                    }

                    client.SendSystemMessage("found " + ret.Count + " cards name like " + param);
                    foreach (var item in ret)
                        client.SendSystemMessage(string.Format("[{0}] - {1}\t{{{2}}}", item.ID, item.Name,
                            item.Serial));
                    break;
                case "number":
                    var sret = IrisCardFactory.Instance.Items.Values.Where(x => x.Serial.Contains(param)).ToList();
                    if (sret.Count == 0)
                    {
                        client.SendSystemMessage("no card found with serial like " + param);
                        return;
                    }

                    client.SendSystemMessage("found " + sret.Count + " cards serial like " + param);
                    foreach (var item in sret)
                        client.SendSystemMessage(string.Format("[{0}] - {1}\t{{{2}}}", item.ID, item.Name,
                            item.Serial));
                    break;
                case "id":
                    var iret = IrisCardFactory.Instance.Items.Values.Where(x => x.ID == uint.Parse(param)).ToList();
                    if (iret.Count == 0)
                    {
                        client.SendSystemMessage("no card found with id like " + param);
                        return;
                    }

                    client.SendSystemMessage("found " + iret.Count + " cards id like " + param);
                    foreach (var item in iret)
                        client.SendSystemMessage(string.Format("[{0}] - {1}\t{{{2}}}", item.ID, item.Name,
                            item.Serial));
                    break;
                case "vector":
                    var aret = IrisCardFactory.Instance.Items.Values
                        .Where(x => x.Abilities.Keys.Where(a => a.Name.Contains(param)).Count() > 0).ToList();
                    if (aret.Count == 0)
                    {
                        client.SendSystemMessage("no card found with vector name like " + param);
                        return;
                    }

                    client.SendSystemMessage("found " + aret.Count + " cards vector name like " + param);
                    foreach (var item in aret)
                        client.SendSystemMessage(string.Format("[{0}] - {1}\t{{{2}}}", item.ID, item.Name,
                            item.Serial));
                    break;
                default:
                    var dret = IrisCardFactory.Instance.Items.Values.Where(x => x.Name.Contains(param)).ToList();
                    if (dret.Count == 0)
                    {
                        client.SendSystemMessage("no card found with card name like " + param);
                        return;
                    }

                    client.SendSystemMessage("found " + dret.Count + " cards name like " + param);
                    foreach (var item in dret)
                        client.SendSystemMessage(string.Format("[{0}] - {1}\t{{{2}}}", item.ID, item.Name,
                            item.Serial));
                    break;
            }
        }

        //#endregion

        //#region "Admin commands"

        private void ProcessMonsterInfo(MapClient client, string args)
        {
            if (args == "")
            {
                client.SendSystemMessage("this command require a monster id/name");
            }
            else
            {
                var id = 0;
                MobData md = null;
                if (!int.TryParse(args, out id))
                    md = MobFactory.Instance.Mobs.First(x => x.Value.name == args).Value;
                else
                    md = MobFactory.Instance.Mobs.First(x => x.Value.id == uint.Parse(args)).Value;
                if (md != null)
                {
                    client.SendSystemMessage(string.Format("编号为:{0} 的魔物:{1} 的数据如下", md.id, md.name));
                    client.SendSystemMessage(string.Format("等级:{0} 视野:{1} 种族:{2} 飞行:{3}", md.level, md.range,
                        md.race.description(), md.fly));
                    client.SendSystemMessage(string.Format("种族语句: {0}", md.mobType));
                    client.SendSystemMessage(string.Format("最大HP:{0} 最大SP:{1} 最大MP:{2} AI模式:{3}", md.hp, md.sp, md.mp,
                        md.aiMode));
                    client.SendSystemMessage(string.Format("STR:{0} AGI:{1} VIT:{2} INT:{3} DEX:{4} MAG:{5}", md.str,
                        md.agi, md.vit, md.intel, md.dex, md.mag));
                    client.SendSystemMessage(string.Format("咏唱速度:{0} 攻击速度:{1} 移动速度:{2}", md.cspd, md.aspd, md.speed));
                    client.SendSystemMessage(string.Format("物理攻击力:{0}-{1} 魔法攻击力:{2}-{3} 命中力: {4}-{5}", md.atk_min,
                        md.atk_max, md.matk_min, md.matk_max, md.hit_melee, md.hit_ranged));
                    client.SendSystemMessage(string.Format("物理防御:{0}-{1} 魔法防御:{2}-{3},回避:{4}-{5}", md.def, md.def_add,
                        md.mdef, md.mdef_add, md.avoid_melee, md.avoid_magic));
                    client.SendSystemMessage(string.Format("是否为BOSS:{0} 暴击发动:{1} 暴击回避:{2}",
                        md.mobType.ToString().Contains("BOSS"), md.cri, md.criavd));
                    client.SendSystemMessage("----------------一般掉落---------------");
                    if (md.dropItems.Count == 0)
                        client.SendSystemMessage("此魔物无一般掉落...");
                    else
                        for (var i = 0; i < md.dropItems.Count; i++)
                            client.SendSystemMessage(string.Format("掉落{0}: {1}|{2} - {3}%", i + 1,
                                md.dropItems[i].ItemID,
                                ItemFactory.Instance.Items.FirstOrDefault(x => x.Key == md.dropItems[i].ItemID).Value
                                    .name, md.dropItems[i].Rate / 100.0f));

                    client.SendSystemMessage("----------------特殊掉落---------------");
                    if (md.dropItemsSpecial.Count == 0)
                        client.SendSystemMessage("此魔物无特殊掉落...");
                    else
                        for (var i = 0; i < md.dropItemsSpecial.Count; i++)
                            client.SendSystemMessage(string.Format("掉落{0}: {1}|{2} - {3}%", i + 1,
                                md.dropItemsSpecial[i].ItemID,
                                ItemFactory.Instance.Items.FirstOrDefault(x => x.Key == md.dropItemsSpecial[i].ItemID)
                                    .Value.name, md.dropItemsSpecial[i].Rate / 100.0f));
                    //client.SendSystemMessage("------------------掉落---------------");
                    //client.SendSystemMessage(string.Format("{0}|{1}\t{2}|{3}\t{4}|{5}\t{6}|{7}\t{8}|{9}\t{10}|{11}",
                    //    md.dropItems[0].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[0].ItemID).Value.name,
                    //    md.dropItems[1].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[1].ItemID).Value.name,
                    //    md.dropItems[2].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[2].ItemID).Value.name,
                    //    md.dropItems[3].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[3].ItemID).Value.name,
                    //    md.dropItems[4].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[4].ItemID).Value.name));
                    //client.SendSystemMessage(string.Format("{0}|{1}\t{2}|{3}\t{4}|{5}\t{6}|{7}\t{8}|{9}\t{10}|{11}",
                    //    md.dropItems[5].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[5].ItemID).Value.name,
                    //    md.dropItems[6].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[6].ItemID).Value.name,
                    //    md.dropItems[7].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[7].ItemID).Value.name,
                    //    md.dropItems[8].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[8].ItemID).Value.name,
                    //    md.dropItems[9].ItemID, ItemFactory.Instance.Items.First(x => x.Key == md.dropItems[9].ItemID).Value.name));
                }
                else
                {
                    client.SendSystemMessage("this monster is not exists.");
                }
            }
        }

        private void ProcessKick(MapClient client, string playername)
        {
            if (playername == "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_KICK_PARA);
            else
                try
                {
                    var chr =
                        from c in MapClientManager.Instance.OnlinePlayer
                        where c.Character.Name == playername
                        select c;
                    client = chr.First();
                    client.NetIo.Disconnect();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessStatReset(MapClient client, string args)
        {
            client.ResetStatusPoint();
        }

        private void ProcessSkillReset(MapClient client, string args)
        {
            client.ResetSkill(1);
            client.ResetSkill(2);
            client.ResetSkill(3);
            client.SendPlayerInfo();
        }

        private void ProcessODWarStart(MapClient client, string arg)
        {
            var map = uint.Parse(arg);
            Tasks.System.ODWar.Instance.StartODWar(map);
        }

        private void ProcessKillAllMob(MapClient client, string arg)
        {
            var loot = false;
            if (arg == "1")
                loot = true;
            var actors = client.map.Actors.Values.ToList();
            var count = 0;
            foreach (var i in actors)
            {
                if (i.type != ActorType.MOB) continue;
                var eh = (MobEventHandler)i.e;
                i.Buff.PlayingDead = true;
                eh.OnDie(loot);
                count++;
            }

            client.SendSystemMessage(count + " mobs killed");
        }

        private void ProcessKickGolem(MapClient client, string arg)
        {
            ClientManager.LeaveCriticalArea();
            try
            {
                foreach (var j in client.map.Actors.Values)
                    if (j.type == ActorType.GOLEM)
                        try
                        {
                            var golem = (ActorGolem)j;
                            if (golem.GolemType >= GolemType.Plant && golem.GolemType <= GolemType.Strange)
                            {
                                var eh = (MobEventHandler)golem.e;
                                golem.e = new NullEventHandler();
                                eh.AI.Pause();
                            }

                            golem.invisble = true;
                            client.map.OnActorVisibilityChange(golem);
                            golem.ClearTaskAddition();
                            MapServer.charDB.SaveChar(golem.Owner, false);
                        }
                        catch
                        {
                        }
            }
            catch
            {
            }

            ClientManager.EnterCriticalArea();
        }

        private void ProcessKickAll(MapClient client, string args)
        {
            if (args != "")
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_KICKALL_PARA);
            else
                try
                {
                    foreach (var i in MapClientManager.Instance.OnlinePlayer) i.NetIo.Disconnect();
                }
                catch (Exception)
                {
                }
        }

        private void ProcessSpawn(MapClient client, string args)
        {
            var arg = args.Split(' ');
            if (arg.Length < 4)
            {
                client.SendSystemMessage(LocalManager.Instance.Strings.ATCOMMAND_SPAWN_PARA);
                return;
            }

            var fs = new FileStream("autospawn.xml", FileMode.Append);
            var sw = new StreamWriter(fs);
            var map = MapManager.Instance.GetMap(client.Character.MapID);
            sw.WriteLine("  <spawn>");
            sw.WriteLine("    <id>{0}</id>", arg[0]);
            sw.WriteLine("    <map>{0}</map>", client.Character.MapID);
            sw.WriteLine("    <x>{0}</x>", Global.PosX16to8(client.Character.X, map.Width));
            sw.WriteLine("    <y>{0}</y>", Global.PosY16to8(client.Character.Y, map.Height));
            sw.WriteLine("    <amount>{0}</amount>", arg[1]);
            sw.WriteLine("    <range>{0}</range>", arg[2]);
            sw.WriteLine("    <delay>{0}</delay>", arg[3]);
            sw.WriteLine("  </spawn>");
            sw.Flush();
            fs.Flush();
            fs.Close();
            client.SendSystemMessage(string.Format(LocalManager.Instance.Strings.ATCOMMAND_SPAWN_SUCCESS, arg[0],
                arg[1], arg[2], arg[3]));
        }

        private void ProcessCallMap(MapClient client, string args)
        {
        }

        //Be careful with this command
        private void ProcessCallAll(MapClient client, string args)
        {
        }

        //#endregion

        //#region "Dev commands"

        private void ProcessRaw(MapClient client, string args)
        {
            if (args == "")
                //args = "12 0C 00 00 01 F4 32 32 00 32 02 00 00 00 00 00 00 00 00 50 00 00 00 50";
                //args = "22 D2 05 02 BE 02 BF 09 BB 03 CD 04 5A 05 05 0A 04 03 05";
                //args = "19 64 05 00 b7 43 4c 00 b7 43 4b 00 b7 43 4a 00 b7 43 49 00 b7 43 48 05 02 02 02 02 02 05 21 e3 81 88 e3 82 93 e3  81 a9 e3 81 86 e8 b1 86  e3 82 92 e6 8e a1 e5 8f 96 e3 81 9b e3 82 88 ef bc 81 33 e4 bf ba e3 81  ab e7 a0 82 e7 b3 96 e5 85 a5 e3 82 8a e3 82 b3 e3 83 bc e3 83 92 e3 83 bc e3 82 92 e5 85 a5 e3 82 8c e3 81 a6 e3 81 8f e3 82 8c ef bc 81 2d e3  82 a8 e3 83 8d e3 83 ab e3 82 ae e3 83 bc e3 83 89 e3 83 aa e3 83 b3 e3 82 af e3 81 8c e9 a3 b2 e3 81 bf e3 81 9f e3 81 84 ef bc 81 2a e3 83 9e e3 82 b8 e3 83 83 e3 82 af e3 83 89 e3 83 aa e3 83 b3 e3 82 af e3 81 8c e9 a3 b2 e3 81 bf e3 81 9f e3 81 84 ef bc 81 2a e3 81 8a e3 81 84 e3 81 97 e3 81 84 e3 82 ab e3 83 ac e3 83 bc e3 82 92 e4 bd 9c e3 81 a3 e3 81 a6 e3 81 8f e3 82 8c ef bc 81 05 00 00 03 84 00 00 03 84 00 00 03 84 00 00 03 84 00 00 03 84 05 2c 2c 27 22 1d";
                args = "1D E4 0D E3 83 9F E3 83 AB E3 82 AF E5 85 8E 00";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            var str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);

            args = "1D E3 00 00 00 00 12 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 ";
            buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
        }

        private void ProcessRaw2(MapClient client, string args)
        {
            if (args == "")
                //args = "12 0C 00 00 01 F4 32 32 00 32 02 00 00 00 00 00 00 00 00 50 00 00 00 50";
                args = "06 00 00 00 00 12  00 00 14 19 00 00 00 00 FF FF 00 00 00 00 00 00 01 00 01";
            //args = "19 64 05 00 b7 43 4c 00 b7 43 4b 00 b7 43 4a 00 b7 43 49 00 b7 43 48 05 02 02 02 02 02 05 21 e3 81 88 e3 82 93 e3  81 a9 e3 81 86 e8 b1 86  e3 82 92 e6 8e a1 e5 8f 96 e3 81 9b e3 82 88 ef bc 81 33 e4 bf ba e3 81  ab e7 a0 82 e7 b3 96 e5 85 a5 e3 82 8a e3 82 b3 e3 83 bc e3 83 92 e3 83 bc e3 82 92 e5 85 a5 e3 82 8c e3 81 a6 e3 81 8f e3 82 8c ef bc 81 2d e3  82 a8 e3 83 8d e3 83 ab e3 82 ae e3 83 bc e3 83 89 e3 83 aa e3 83 b3 e3 82 af e3 81 8c e9 a3 b2 e3 81 bf e3 81 9f e3 81 84 ef bc 81 2a e3 83 9e e3 82 b8 e3 83 83 e3 82 af e3 83 89 e3 83 aa e3 83 b3 e3 82 af e3 81 8c e9 a3 b2 e3 81 bf e3 81 9f e3 81 84 ef bc 81 2a e3 81 8a e3 81 84 e3 81 97 e3 81 84 e3 82 ab e3 83 ac e3 83 bc e3 82 92 e4 bd 9c e3 81 a3 e3 81 a6 e3 81 8f e3 82 8c ef bc 81 05 00 00 03 84 00 00 03 84 00 00 03 84 00 00 03 84 00 00 03 84 05 2c 2c 27 22 1d";
            //args = @"13 92 0D 47 01 00 00 00 00 12 00 00 27 11 03 00 00 27 11 00 00 27 15 00 00 27 14 FF FF 03 00 00 00 00 00 01 E2 40 00 00 00 00 00 0F 12 06 00 00 00 00 00 12 D6 87 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 03 00 00 00 01 00 00 00 01 00 00 00 01 05";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            var str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
        }

        private void ProcessTest(MapClient client, string args)
        {
            args = "1C 20 00 00 01 2C";
            var buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            var p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            var str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
            client.NetIo.SendPacket(p);

            args = @"1C 25";
            buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
            client.NetIo.SendPacket(p);


            args =
                @"1C 26 D4 23 C3 46 A3 01 31 F0 D4 00 00 00 00 33 00 00 02 D0 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 01 00 00 FF FF FF FF 00 01 00";
            buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
            client.NetIo.SendPacket(p);

            args = @"1C 27";
            buf = Conversions.HexStr2Bytes(args.Replace(" ", ""));
            p = new Packet();
            p.data = buf;
            client.NetIo.SendPacket(p);

            str = "Sending Packet : ";
            foreach (var item in p.data) str += item.ToString("X2") + " ";
            str += "\r\n";
            Logger.ShowInfo(str);
            client.NetIo.SendPacket(p);
        }

        private void ProcessFace(MapClient client, string args)
        {
            if (args == "")
            {
                client.SendSystemMessage("用法 !face ID");
            }
            else
            {
                var face = ushort.Parse(args);
                client.Character.Face = face;
                //client.SendCharInfo();
                client.SendCharInfoUpdate();
            }
        }

        private void ProcessCreateFF(MapClient client, string args)
        {
            if (client.Character.FlyingGarden == null) //如果當前帳號還沒創建過飛空庭
                client.Character.FlyingGarden = new FlyingGarden(client.Character); //創建新的飛空庭
            if (client.Character.Ring == null) RingManager.Instance.CreateRing(client.Character, args);
            /*-------------------服務器專有--------------------
            MapClient s = MapClient.FromActorPC(ScriptManager.Instance.VariableHolder);
            SagaDB.Ring.Ring ring = RingManager.Instance.CreateRing(ScriptManager.Instance.VariableHolder, "番茄會");
            if (s.Character.Ring.FFGarden == null)
            {
                s.Character.Ring.FFGarden = new SagaDB.FFGarden.FFGarden();
                s.Character.Ring.FFGarden.Name = args;
                s.Character.Ring.FFGarden.RingID = s.Character.Ring.ID;
                s.Character.Ring.FFGarden.ObMode = 3;
                s.Character.Ring.FFGarden.Content = "飛空城";
                SagaDB.FFGarden.FFGarden r = new SagaDB.FFGarden.FFGarden();
                s.Character.Ring.FFGarden.Furnitures.Add(SagaDB.FFGarden.FurniturePlace.GARDEN, new List<ActorFurniture>());
                s.Character.Ring.FFGarden.Furnitures.Add(SagaDB.FFGarden.FurniturePlace.ROOM, new List<ActorFurniture>());
                MapServer.charDB.CreateFF(s.Character);
            }
            /*-------------------服務器專有--------------------*/
            if (args == "")
            {
                client.SendSystemMessage("请输入飞空城名字");
            }
            else if (client.Character.Ring == null)
            {
                client.SendSystemMessage("没有工会");
            }
            else if (client.Character.Ring.Leader != client.Character)
            {
                client.SendSystemMessage("不是工会队长");
            }
            else
            {
                if (client.Character.Ring.FlyingCastle == null)
                {
                    client.Character.Ring.FlyingCastle = new SagaDB.FlyingCastle.FlyingCastle();
                    client.Character.Ring.FlyingCastle.Name = args;
                    client.Character.Ring.FlyingCastle.RingID = client.Character.Ring.ID;
                    client.Character.Ring.FlyingCastle.ObMode = 3;
                    client.Character.Ring.FlyingCastle.Content = "测试内容";
                    client.Character.Ring.FlyingCastle.Furnitures.Add(SagaDB.FlyingCastle.FurniturePlace.GARDEN,
                        new List<ActorFurniture>());
                    client.Character.Ring.FlyingCastle.Furnitures.Add(SagaDB.FlyingCastle.FurniturePlace.ROOM,
                        new List<ActorFurniture>());
                }

                MapServer.charDB.CreateFF(client.Character);
                client.SendRingFF();
            }
        }

        private void ProcessOpenFF(MapClient client, string args)
        {
            //CustomMapManager.Instance.EnterMap(client.Character);
            int maxPage;
            var page = 1;
            var res = FFGardenManager.Instance.GetFlyingCastles(0, out maxPage);
            var p1 = new SSMG_FF_LIST();
            p1.ActorID = client.Character.ActorID;
            p1.Page = 0;
            p1.MaxPaga = (uint)maxPage;
            p1.Entries = res;
            client.NetIo.SendPacket(p1);
        }

        //#endregion

        //#endregion
    }
}