using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SagaDB.Actor;
using SagaDB.Config;
using SagaDB.Item;
using SagaLib;
using Version = SagaLib.Version;

namespace SagaLogin.Configuration
{
    public class Configuration : Singleton<Configuration>
    {
        private string encoding;

        public Configuration()
        {
            var list = new Dictionary<PC_GENDER, List<StartItem>>();
            StartItem.Add(PC_RACE.EMIL, list);
            list = new Dictionary<PC_GENDER, List<StartItem>>();
            StartItem.Add(PC_RACE.TITANIA, list);
            list = new Dictionary<PC_GENDER, List<StartItem>>();
            StartItem.Add(PC_RACE.DOMINION, list);
            list = new Dictionary<PC_GENDER, List<StartItem>>();
            StartItem.Add(PC_RACE.DEM, list);
        }

        public string DBHost { get; set; }

        public string DBUser { get; set; }

        public string DBPass { get; set; }

        public string DBName { get; set; }

        public string Password { get; set; } = "saga";

        public int DBPort { get; set; }

        public int Port { get; set; }

        public int DBType { get; set; }

        public Dictionary<PC_RACE, StartupSetting> StartupSetting { get; set; } =
            new Dictionary<PC_RACE, StartupSetting>();

        public Dictionary<PC_RACE, Dictionary<PC_GENDER, List<StartItem>>> StartItem { get; set; } =
            new Dictionary<PC_RACE, Dictionary<PC_GENDER, List<StartItem>>>();

        public Version Version { get; set; }

        public List<string> Motd { get; } = new List<string>();

        public string DBEncoding
        {
            get
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8.EncodingName;
                    Logger.ShowDebug("DB Encoding not set, set to default value: " + encoding, Logger.CurrentLogger);
                }

                return encoding;
            }
            set => encoding = value;
        }

        public int LogLevel { get; set; }

        public void Initialization(string path)
        {
            var xml = new XmlDocument();
            try
            {
                XmlElement root;
                XmlNodeList list;
                var getVersion = false;
                xml.Load(path);
                root = xml["SagaLogin"];
                list = root.ChildNodes;
                foreach (var j in list)
                {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower())
                    {
                        case "dbtype":
                            DBType = int.Parse(i.InnerText);
                            break;
                        case "port":
                            Port = int.Parse(i.InnerText);
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
                        case "dbencoding":
                            encoding = i.InnerText;
                            break;
                        case "password":
                            Password = i.InnerText;
                            break;
                        case "loglevel":
                            LogLevel = int.Parse(i.InnerText);
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
                        case "startstatus":
                            var race = PC_RACE.EMIL;
                            switch (i.Attributes["race"].Value.ToUpper())
                            {
                                case "EMIL":
                                    race = PC_RACE.EMIL;
                                    break;
                                case "TITANIA":
                                    race = PC_RACE.TITANIA;
                                    break;
                                case "DOMINION":
                                    race = PC_RACE.DOMINION;
                                    break;
                                case "DEM":
                                    race = PC_RACE.DEM;
                                    break;
                            }

                            var setting = new StartupSetting();
                            var childs = i.ChildNodes;
                            foreach (var l in childs)
                            {
                                XmlElement k;
                                if (l.GetType() != typeof(XmlElement)) continue;
                                k = (XmlElement)l;
                                switch (k.Name.ToLower())
                                {
                                    case "str":
                                        setting.Str = ushort.Parse(k.InnerText);
                                        break;
                                    case "dex":
                                        setting.Dex = ushort.Parse(k.InnerText);
                                        break;
                                    case "int":
                                        setting.Int = ushort.Parse(k.InnerText);
                                        break;
                                    case "vit":
                                        setting.Vit = ushort.Parse(k.InnerText);
                                        break;
                                    case "agi":
                                        setting.Agi = ushort.Parse(k.InnerText);
                                        break;
                                    case "mag":
                                        setting.Mag = ushort.Parse(k.InnerText);
                                        break;
                                    case "startmap":
                                        setting.StartMap = uint.Parse(k.InnerText);
                                        break;
                                    case "startx":
                                        setting.X = byte.Parse(k.InnerText);
                                        break;
                                    case "starty":
                                        setting.Y = byte.Parse(k.InnerText);
                                        break;
                                }
                            }

                            StartupSetting.Add(race, setting);
                            break;
                        case "startitem":
                            Dictionary<PC_GENDER, List<StartItem>> items = null;
                            var gender = PC_GENDER.FEMALE;
                            switch (i.Attributes["race"].Value.ToUpper())
                            {
                                case "EMIL":
                                    items = StartItem[PC_RACE.EMIL];
                                    break;
                                case "TITANIA":
                                    items = StartItem[PC_RACE.TITANIA];
                                    break;
                                case "DOMINION":
                                    items = StartItem[PC_RACE.DOMINION];
                                    break;
                                case "DEM":
                                    items = StartItem[PC_RACE.DEM];
                                    break;
                            }

                            switch (i.Attributes["gender"].Value.ToUpper())
                            {
                                case "MALE":
                                    gender = PC_GENDER.MALE;
                                    break;
                                case "FEMALE":
                                    gender = PC_GENDER.FEMALE;
                                    break;
                            }

                            var list2 = new List<StartItem>();
                            items.Add(gender, list2);
                            var childs2 = i.ChildNodes;
                            foreach (var o in childs2)
                            {
                                XmlElement p;
                                if (o.GetType() != typeof(XmlElement)) continue;
                                p = (XmlElement)o;
                                var startitem = new StartItem();
                                var childs3 = p.ChildNodes;
                                foreach (var n in childs3)
                                {
                                    XmlElement m;
                                    if (n.GetType() != typeof(XmlElement)) continue;
                                    m = (XmlElement)n;
                                    switch (m.Name.ToLower())
                                    {
                                        case "itemid":
                                            startitem.ItemID = uint.Parse(m.InnerText);
                                            break;
                                        case "slot":
                                            startitem.Slot = (ContainerType)Enum.Parse(typeof(ContainerType),
                                                m.InnerText.ToUpper());
                                            break;
                                        case "count":
                                            startitem.Count = byte.Parse(m.InnerText);
                                            break;
                                    }
                                }

                                list2.Add(startitem);
                            }

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
    }
}