#define Text

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using SagaDB.Actor;
using SagaDB.Config;
using SagaLib;
using SagaLib.VirtualFileSytem;
using SagaMap.Tasks.System;
using Version = SagaLib.Version;

namespace SagaMap.Configuration
{
    public enum RateOverrideType
    {
        GMLv,
        CLevel
    }

    public class RateOverrideItem
    {
        public RateOverrideType Type { get; set; }

        public int Value { get; set; }

        public float ExpRate { get; set; }

        public float QuestRate { get; set; }

        public float QuestGoldRate { get; set; }

        public float StampDropRate { get; set; }

        public float GlobalDropRate { get; set; } = 1f;

        public float SpecialDropRate { get; set; } = 1f;

        public override string ToString()
        {
            return string.Format("Type:{0} Value:{1}", Type, Value);
        }
    }

    public class Configuration : Singleton<Configuration>
    {
        private readonly Dictionary<RateOverrideType, Dictionary<int, RateOverrideItem>> rateOverride =
            new Dictionary<RateOverrideType, Dictionary<int, RateOverrideItem>>();

        //API
        private int? apiport;
        private string encoding;

        private string prefixes;

        //VShop

        private string whitelist = "127.0.0.1";


        public int MaxCharacterInMapServer { get; set; } = 2;

        public bool AJIMode { get; set; }

        public bool VShopClosed { get; set; }

        public string Host { get; set; }

        public string DBHost { get; set; }

        public string DBUser { get; set; }

        public string DBPass { get; set; }

        public string DBName { get; set; }

        public string LoginPass { get; set; } = "saga";

        public string ClientVersion { get; set; }

        public string APIPass { get; set; } = "saga";

        public string APIKey { get; set; }

        public bool EnhanceMatsuri { get; set; }

        public int APIPort
        {
            get
            {
                if (apiport == null || apiport == 0)
                {
                    Logger.GetLogger().Warning("PORT ARE NOT SET.USEING DEFAULT PORT (8080).");
                    apiport = 8080;
                }

                return apiport.Value;
            }
            set => apiport = value;
        }

        public string Prefixes
        {
            get
            {
                if (prefixes == null)
                {
                    Logger.GetLogger().Warning("PREFIXES ARE NOT SET.USEING DEFAULT PREFIXES (localhost).");
                    prefixes = "http://localhost";
                }

                return prefixes;
            }
            set => prefixes = value;
        }

        public int Port { get; set; }

        public string LoginHost { get; set; }

        public int DBPort { get; set; }

        public int DBType { get; set; }

        public int LoginPort { get; set; }

        public int MaxLevelDifferenceForExp { get; set; } = 99;

        public int FirstLevelLimit { get; set; }

        public int EXPRate { get; set; }

        public int StampDropRate { get; private set; } = 100;

        public int ItemFusionRate { get; private set; } = 80;

        public int MobAmount { get; private set; } = 1;


        public int QuestRate { get; set; }

        public int QuestGoldRate { get; set; } = 100;

        public int WarehouseLimit { get; private set; } = 100;

        public ushort Speed { get; private set; } = 410;

        public Version Version { get; private set; } = Version.Saga6;

        public uint JobSwitchReduceItem { get; private set; } = 10024500;

        public int RingFameNeededForEmblem { get; private set; } = 300;

        public Dictionary<PC_RACE, StartupSetting> StartupSetting { get; set; } =
            new Dictionary<PC_RACE, StartupSetting>();

        public List<string> Motd { get; } = new List<string>();

        public List<string> ScriptReference { get; } = new List<string>();

        public List<string> MonitorAccounts { get; } = new List<string>();

        public string Language { get; set; }

        public List<uint> HostedMaps { get; set; } = new List<uint>();

        public bool SQLLog { get; private set; } = true;

        public int QuestUpdateTime { get; set; } = 24;

        public int QuestUpdateAmount { get; set; } = 5;

        public int QuestPointsMax { get; set; } = 15;

        public uint MaxFurnitureCount { get; set; } = 100;

        public int LogLevel { get; set; }

        public float DeathPenaltyBaseEmil { get; set; } = 0.1f;

        public float DeathPenaltyJobEmil { get; set; } = 0.02f;

        public float DeathPenaltyBaseDominion { get; set; } = 0.1f;

        public float DeathPenaltyJobDominion { get; set; } = 0.02f;

        public bool OnlineStatistics { get; set; } = true;

        public string StatisticsPagePath { get; set; } = "index.htm";

        public bool MultipleDrop { get; set; }

        public int BossSlashRate { get; set; } = 10;

        public bool BossSlash { get; set; }

        public bool AtkMastery { get; set; } = true;

        public ushort BasePhysicDef { get; set; } = 50;

        public ushort BaseMagicDef { get; set; } = 50;

        public ushort MaxPhysicDef { get; set; } = 90;

        public ushort MaxMagicDef { get; set; } = 90;

        public float GlobalDropRate { get; set; } = 1f;

        public float SpecialDropRate { get; set; } = 1f;

        public bool ActiveSpecialLoot { get; set; }

        public int BossSpecialLootRate { get; set; }

        public uint BossSpecialLootID { get; set; }

        public byte BossSpecialLootNum { get; set; }

        public int NomalMobSpecialLootRate { get; set; }

        public uint NomalMobSpecialLootID { get; set; }

        public byte NomalMobSpecialLootNum { get; set; }

        public bool ActivceQuestSpecialReward { get; set; }

        public uint QuestSpecialRewardID { get; set; }

        public int QuestSpecialRewardRate { get; set; }

        public string TwitterID
        {
            get => TwitterID;
            set => TwitterID = value;
        }

        public string TwitterPasswd
        {
            get => TwitterPasswd;
            set => TwitterPasswd = value;
        }

        public float PVPDamageRatePhysic { get; set; } = 1f;

        public float PVPDamageRateMagic { get; set; } = 1f;

        public float PayloadRate { get; set; } = 1f;

        public float VolumeRate { get; set; } = 1f;

        public string DBEncoding
        {
            get
            {
                if (encoding == null)
                {
                    Logger.ShowDebug("DB Encoding not set, set to default value: UTF-8", Logger.CurrentLogger);
                    encoding = "utf-8";
                }

                return encoding;
            }
            set => encoding = value;
        }

        public float CalcEXPRateForPC(ActorPC pc)
        {
            var rate = (float)EXPRate / 100;
            /*RateOverrideItem gmlv, lv;
            GetRateOverride(pc, out gmlv, out lv);
            if (gmlv != null)
                rate *= gmlv.ExpRate;
            if (gmlv == null && lv != null)
                rate *= lv.ExpRate;*/
            return rate;
        }

        public float CalcStampDropRateForPC(ActorPC pc)
        {
            var rate = (float)StampDropRate / 100;
            if (pc != null)
            {
                RateOverrideItem gmlv, lv;
                GetRateOverride(pc, out gmlv, out lv);
                if (gmlv != null)
                    rate *= gmlv.StampDropRate;
                if (gmlv == null && lv != null)
                    rate *= lv.StampDropRate;
            }

            return rate;
        }

        public float CalcQuestRateForPC(ActorPC pc)
        {
            var rate = (float)QuestRate / 100;
            RateOverrideItem gmlv, lv;
            GetRateOverride(pc, out gmlv, out lv);
            if (gmlv != null)
                rate *= gmlv.QuestRate;
            if (gmlv == null && lv != null)
                rate *= lv.QuestRate;
            return rate;
        }

        public float CalcQuestGoldRateForPC(ActorPC pc)
        {
            var rate = (float)QuestGoldRate / 100;
            RateOverrideItem gmlv, lv;
            GetRateOverride(pc, out gmlv, out lv);
            if (gmlv != null)
                rate *= gmlv.QuestGoldRate;
            if (gmlv == null && lv != null)
                rate *= lv.QuestGoldRate;
            return rate;
        }

        public float CalcGlobalDropRateForPC(ActorPC pc)
        {
            var rate = GlobalDropRate;
            if (pc != null)
            {
                RateOverrideItem gmlv, lv;
                GetRateOverride(pc, out gmlv, out lv);
                if (gmlv != null)
                    rate *= gmlv.GlobalDropRate;
                if (gmlv == null && lv != null)
                    rate *= lv.GlobalDropRate;
            }

            return rate;
        }

        public float CalcSpecialDropRateForPC(ActorPC pc)
        {
            var rate = SpecialDropRate;
            if (pc != null)
            {
                RateOverrideItem gmlv, lv;
                GetRateOverride(pc, out gmlv, out lv);
                if (gmlv != null)
                    rate *= gmlv.SpecialDropRate;
                if (gmlv == null && lv != null)
                    rate *= lv.SpecialDropRate;
            }

            return rate;
        }

        public void InitAnnounce(string path)
        {
            var sr = new StreamReader(VirtualFileSystemManager.Instance.FileSystem.OpenFile(path),
                Encoding.UTF8);

            string[] paras;
            byte count = 0;
            while (!sr.EndOfStream)
            {
                string line;
                line = sr.ReadLine();
                try
                {
                    if (line == "") continue;
                    if (line.Substring(0, 1) == "#")
                        continue;
                    paras = line.Split(',');

                    var DueTime = int.Parse(paras[0]);
                    var period = int.Parse(paras[1]);
                    var Text = paras[2];

                    count++;
                    var ta = new TaskAnnounce("公告" + count, Text, DueTime, period);
                    ta.Activate();
                }
                catch (Exception ex)
                {
                    SagaLib.Logger.ShowError(ex);
                }
            }

            sr.Close();
        }

        public void Initialization(string path)
        {
            HostedMaps.Clear();
#if Text
            InitXML(path);
#else
            InitDat(path);
#endif
        }

        private void GetRateOverride(ActorPC pc, out RateOverrideItem gmlv, out RateOverrideItem clv)
        {
            gmlv = null;
            clv = null;
            foreach (var i in rateOverride.Keys)
                switch (i)
                {
                    case RateOverrideType.GMLv:
                    {
                        var maxValue = 0;
                        foreach (var j in rateOverride[i].Keys)
                            if (j > maxValue && j <= pc.Account.GMLevel)
                                maxValue = j;
                        if (maxValue > 0)
                            gmlv = rateOverride[i][maxValue];
                    }
                        break;
                    case RateOverrideType.CLevel:
                    {
                        var maxValue = 0;
                        foreach (var j in rateOverride[i].Keys)
                            if (j > maxValue && j <= pc.Level)
                                maxValue = j;
                        if (maxValue > 0)
                            clv = rateOverride[i][maxValue];
                    }
                        break;
                }
        }
#if Text
        private void InitXML(string path)
        {
            var xml = new XmlDocument();
            var getVersion = false;
            try
            {
                XmlElement root;
                XmlNodeList list;
                xml.Load(path);
                root = xml["SagaMap"];
                list = root.ChildNodes;
                foreach (var j in list)
                {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower())
                    {
                        case "host":
                            Host = i.InnerText;
                            break;
                        case "port":
                            Port = int.Parse(i.InnerText);
                            break;
                        case "dbtype":
                            DBType = int.Parse(i.InnerText);
                            break;
                        case "dbhost":
                            DBHost = i.InnerText;
                            break;
                        case "dbport":
                            DBPort = int.Parse(i.InnerText);
                            break;
                        case "dbuser":
                            DBUser = i.InnerText;
                            break;
                        case "dbpass":
                            DBPass = i.InnerText;
                            break;
                        case "dbname":
                            DBName = i.InnerText;
                            break;
                        case "loginhost":
                            LoginHost = i.InnerText;
                            break;
                        case "loginport":
                            LoginPort = int.Parse(i.InnerText);
                            break;
                        case "loginpass":
                            LoginPass = i.InnerText;
                            break;
                        case "prefixes":
                            prefixes = i.InnerText;
                            break;
                        case "apikey":
                            APIKey = i.InnerText;
                            break;
                        case "apiport":
                            apiport = int.Parse(i.InnerText);
                            break;
                        case "vshopclosed":
                            VShopClosed = bool.Parse(i.InnerText);
                            break;
                        case "loglevel":
                            LogLevel = int.Parse(i.InnerText);
                            break;
                        case "clientversion":
                            ClientVersion = i.InnerText;
                            break;
                        case "language":
                            Language = i.InnerText;
                            break;
                        case "dbencoding":
                            encoding = i.InnerText;
                            break;
                        case "atkmastery":
                            AtkMastery = i.InnerText == "1";
                            break;
                        case "levellimit":
                            FirstLevelLimit = int.Parse(i.InnerText);
                            break;
                        case "maxsameplayerinmapserver":
                            MaxCharacterInMapServer = int.Parse(i.InnerText);
                            break;
                        case "motd":
                            var msg = i.InnerText.Split('\n');
                            foreach (var k in msg)
                            {
                                var tmp = k.Replace("\r", "").Replace(" ", "");
                                if (tmp != "")
                                    Motd.Add(tmp);
                            }

                            break;
                        case "monitoraccounts":
                            var account = i.InnerText.Split('\n');
                            foreach (var k in account)
                            {
                                var tmp = k.Replace("\r", "").Replace(" ", "");
                                if (tmp != "")
                                    MonitorAccounts.Add(tmp);
                            }

                            break;
                        case "maxlvdiffforexp":
                            MaxLevelDifferenceForExp = int.Parse(i.InnerText);
                            break;
                        case "rateoverride":
                        {
                            var type = i.Attributes["type"].Value;
                            var value = int.Parse(i.Attributes["value"].Value);
                            var rType = RateOverrideType.GMLv;
                            switch (type.ToLower())
                            {
                                case "gmlv":
                                    rType = RateOverrideType.GMLv;
                                    break;
                                case "clv":
                                    rType = RateOverrideType.CLevel;
                                    break;
                            }

                            Dictionary<int, RateOverrideItem> list2;
                            if (rateOverride.ContainsKey(rType))
                            {
                                list2 = rateOverride[rType];
                            }
                            else
                            {
                                list2 = new Dictionary<int, RateOverrideItem>();
                                rateOverride.Add(rType, list2);
                            }

                            if (!list2.ContainsKey(value))
                            {
                                var item = new RateOverrideItem();
                                item.Type = rType;
                                item.Value = value;
                                var maps = i.ChildNodes;
                                foreach (var l in maps)
                                {
                                    XmlElement k;
                                    if (l.GetType() != typeof(XmlElement)) continue;
                                    k = (XmlElement)l;
                                    switch (k.Name.ToLower())
                                    {
                                        case "exprate":
                                            item.ExpRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                        case "questrate":
                                            item.QuestRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                        case "questgoldrate":
                                            item.QuestGoldRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                        case "stampdroprate":
                                            item.StampDropRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                        case "globaldroprate":
                                            item.GlobalDropRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                        case "specialdroprate":
                                            item.SpecialDropRate = int.Parse(k.InnerText) / 100f;
                                            break;
                                    }
                                }

                                list2.Add(value, item);
                            }
                        }
                            break;
                        case "hostedmaps":
                        {
                            var maps = i.ChildNodes;
                            foreach (var l in maps)
                            {
                                XmlElement k;
                                if (l.GetType() != typeof(XmlElement)) continue;
                                k = (XmlElement)l;
                                switch (k.Name.ToLower())
                                {
                                    case "mapid":
                                        HostedMaps.Add(uint.Parse(k.InnerText));
                                        break;
                                }
                            }
                        }
                            break;
                        case "scriptreference":
                            var dlls = i.ChildNodes;
                            foreach (var l in dlls)
                            {
                                XmlElement k;
                                if (l.GetType() != typeof(XmlElement)) continue;
                                k = (XmlElement)l;
                                switch (k.Name.ToLower())
                                {
                                    case "assembly":
                                        ScriptReference.Add(k.InnerText);
                                        break;
                                }
                            }

                            break;
                        case "exprate":
                            EXPRate = int.Parse(i.InnerText);
                            break;
                        case "enhancematsuri":
                            EnhanceMatsuri = bool.Parse(i.InnerText);
                            break;
                        case "stampdroprate":
                            StampDropRate = int.Parse(i.InnerText);
                            break;
                        case "itemfusionrate":
                            ItemFusionRate = int.Parse(i.InnerText);
                            break;
                        case "questrate":
                            QuestRate = int.Parse(i.InnerText);
                            break;
                        case "questgoldrate":
                            QuestGoldRate = int.Parse(i.InnerText);
                            break;
                        case "warehouselimit":
                            WarehouseLimit = int.Parse(i.InnerText);
                            break;
                        case "version":
                            try
                            {
                                Version = (Version)Enum.Parse(typeof(Version), i.InnerText);
                                getVersion = true;
                            }
                            catch
                            {
                                Logger.GetLogger().Warning(string.Format(
                                    "Cannot find Version:[{0}], using default version:[{1}]", i.InnerText, Version));
                            }

                            break;
                        case "jobswitchreduceitem":
                            JobSwitchReduceItem = uint.Parse(i.InnerText);
                            break;
                        case "ringfameneededforemblem":
                            RingFameNeededForEmblem = int.Parse(i.InnerText);
                            break;
                        case "maxfurniturecount":
                            MaxFurnitureCount = uint.Parse(i.InnerText);
                            break;
                        case "deathpenaltybaseemil":
                            DeathPenaltyBaseEmil = (float)int.Parse(i.InnerText) / 100;
                            break;
                        case "deathpenaltyjobemil":
                            DeathPenaltyJobEmil = (float)int.Parse(i.InnerText) / 100;
                            break;
                        case "deathpenaltybasedominion":
                            DeathPenaltyBaseDominion = (float)int.Parse(i.InnerText) / 100;
                            break;
                        case "deathpenaltyjobdominion":
                            DeathPenaltyJobDominion = (float)int.Parse(i.InnerText) / 100;
                            break;
                        case "sqllog":
                            if (i.InnerText == "1")
                                SQLLog = true;
                            else
                                SQLLog = false;
                            break;
                        case "questupdatetime":
                            QuestUpdateTime = int.Parse(i.InnerText);
                            break;
                        case "questupdateamount":
                            QuestUpdateAmount = int.Parse(i.InnerText);
                            break;
                        case "questpointsmax":
                            QuestPointsMax = int.Parse(i.InnerText);
                            break;
                        case "onlinestatistic":
                            OnlineStatistics = int.Parse(i.InnerText) == 1;
                            break;
                        case "statisticpagepath":
                            StatisticsPagePath = i.InnerText;
                            break;
                        case "sqlloglevel":
                            Logger.SQLLogLevel.Value = int.Parse(i.InnerText);
                            break;
                        case "multipledrop":
                            MultipleDrop = i.InnerText == "1";
                            break;
                        case "bossslash":
                            BossSlash = i.InnerText == "1";
                            break;
                        case "bossslashrate":
                            BossSlashRate = int.Parse(i.InnerText);
                            break;
                        case "globaldroprate":
                            GlobalDropRate = int.Parse(i.InnerText) / 100f;
                            break;
                        case "specialdroprate":
                            SpecialDropRate = int.Parse(i.InnerText) / 100f;
                            break;
                        case "pvpdamageratephysic":
                            PVPDamageRatePhysic = int.Parse(i.InnerText) / 100f;
                            break;
                        case "pvpdamageratemagic":
                            PVPDamageRateMagic = int.Parse(i.InnerText) / 100f;
                            break;
                        case "payloadrate":
                            PayloadRate = int.Parse(i.InnerText) / 100f;
                            break;
                        case "volumerate":
                            VolumeRate = int.Parse(i.InnerText) / 100f;
                            break;
                        case "TwitterID":
                            TwitterID = i.InnerText;
                            break;
                        case "TwitterPasswd":
                            TwitterPasswd = i.InnerText;
                            break;
                        case "speed":
                            Speed = ushort.Parse(i.InnerText);
                            break;
                        case "mobamount":
                            MobAmount = int.Parse(i.InnerText);
                            break;
                        case "basePhysicDef":
                            BasePhysicDef = ushort.Parse(i.InnerText);
                            break;
                        case "basemagicdef":
                            BaseMagicDef = ushort.Parse(i.InnerText);
                            break;
                        case "maxphysicdef":
                            MaxPhysicDef = ushort.Parse(i.InnerText);
                            break;
                        case "maxmagicdef":
                            MaxMagicDef = ushort.Parse(i.InnerText);
                            break;
                        case "activespecialloot":
                            ActiveSpecialLoot = bool.Parse(i.InnerText);
                            break;
                        case "bossspeciallootrate":
                            BossSpecialLootRate = int.Parse(i.InnerText);
                            break;
                        case "bossspeciallootid":
                            BossSpecialLootID = uint.Parse(i.InnerText);
                            break;
                        case "bossspeciallootnum":
                            BossSpecialLootNum = byte.Parse(i.InnerText);
                            break;
                        case "nomalmobspeciallootrate":
                            NomalMobSpecialLootRate = int.Parse(i.InnerText);
                            break;
                        case "nomalmobspeciallootid":
                            NomalMobSpecialLootID = uint.Parse(i.InnerText);
                            break;
                        case "nomalmobspeciallootnum":
                            NomalMobSpecialLootNum = byte.Parse(i.InnerText);
                            break;
                        case "activequestspecialreward":
                            ActivceQuestSpecialReward = bool.Parse(i.InnerText);
                            break;
                        case "questspecialrewardid":
                            QuestSpecialRewardID = uint.Parse(i.InnerText);
                            break;
                        case "questspecialrewardrate":
                            QuestSpecialRewardRate = int.Parse(i.InnerText);
                            break;
                    }
                }

                if (!getVersion)
                    Logger.GetLogger().Warning(string.Format(
                        "Packet Version not set, using default version:[{0}], \r\n         please change Config/SagaMap.xml to set version",
                        Version));
                Logger.GetLogger().Information("Done reading configuration...");
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error(ex, ex.Message);
            }
        }

        //#else
        private void InitDat(string path)
        {
            var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            var br = new BinaryReader(fs);
            var magic = br.ReadInt32();
            if (magic == 0x12345678)
            {
                br.ReadInt32();
                var len = br.ReadByte();
                Host = Global.Unicode.GetString(br.ReadBytes(len));
                Port = br.ReadInt32();
                len = br.ReadByte();
                DBHost = Global.Unicode.GetString(br.ReadBytes(len));
                DBPort = br.ReadInt32();
                LogLevel = br.ReadInt32();
                SQLLog = br.ReadBoolean();
                Logger.SQLLogLevel.Value = br.ReadInt32();
                len = br.ReadByte();
                DBUser = Global.Unicode.GetString(br.ReadBytes(len));
                len = br.ReadByte();
                DBPass = Global.Unicode.GetString(br.ReadBytes(len));
                len = br.ReadByte();
                LoginHost = Global.Unicode.GetString(br.ReadBytes(len));
                LoginPort = br.ReadInt32();
                len = br.ReadByte();
                LoginPass = Global.Unicode.GetString(br.ReadBytes(len));
                len = br.ReadByte();
                Language = Global.Unicode.GetString(br.ReadBytes(len));
                Version = (Version)br.ReadByte();
                len = br.ReadByte();
                encoding = Global.Unicode.GetString(br.ReadBytes(len));
                EXPRate = br.ReadInt32();
                QuestRate = br.ReadInt32();
                QuestGoldRate = br.ReadInt32();
                StampDropRate = br.ReadInt32();
                ItemFusionRate = br.ReadInt32();
                MobAmount = br.ReadInt32();
                QuestUpdateTime = br.ReadInt32();
                QuestUpdateAmount = br.ReadInt32();
                QuestPointsMax = br.ReadInt32();
                WarehouseLimit = br.ReadInt32();
                DeathPenaltyBaseEmil = br.ReadSingle();
                DeathPenaltyJobEmil = br.ReadSingle();
                DeathPenaltyBaseDominion = br.ReadSingle();
                DeathPenaltyJobDominion = br.ReadSingle();
                JobSwitchReduceItem = br.ReadUInt32();
                RingFameNeededForEmblem = br.ReadInt32();
                MaxFurnitureCount = br.ReadUInt32();
                OnlineStatistics = br.ReadBoolean();
                len = br.ReadByte();
                StatisticsPagePath = Global.Unicode.GetString(br.ReadBytes(len));
                MultipleDrop = br.ReadBoolean();
                BossSlash = br.ReadBoolean();
                BossSlashRate = br.ReadInt32();
                GlobalDropRate = br.ReadSingle();
                AtkMastery = br.ReadBoolean();

                var count = br.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    len = br.ReadByte();
                    var txt = Global.Unicode.GetString(br.ReadBytes(len));
                    Motd.Add(txt);
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    len = br.ReadByte();
                    var txt = Global.Unicode.GetString(br.ReadBytes(len));
                    ScriptReference.Add(txt);
                }

                count = br.ReadInt32();
                for (var i = 0; i < count; i++)
                {
                    var mapID = br.ReadUInt32();
                    HostedMaps.Add(mapID);
                }
            }
        }
#endif
    }
}