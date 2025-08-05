using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using SagaDB.Actor;
using SagaLib;
using SagaLib.VirtualFileSytem;

namespace SagaDB.FictitiousActors
{
    public class FictitiousActorsFactory : Singleton<FictitiousActorsFactory>
    {
        public Dictionary<uint, Dictionary<uint, GolemShopItem>> GolemBuyList =
            new Dictionary<uint, Dictionary<uint, GolemShopItem>>();

        public Dictionary<uint, Dictionary<uint, GolemShopItem>> GolemSellList =
            new Dictionary<uint, Dictionary<uint, GolemShopItem>>();

        public Dictionary<uint, List<Actor.Actor>> FictitiousActorsList { get; set; } =
            new Dictionary<uint, List<Actor.Actor>>();

        public void LoadActorsList(string path)
        {
            var file = VirtualFileSystemManager.Instance.FileSystem.SearchFile(path, "*.xml",
                SearchOption.AllDirectories);
            var total = 0;
            foreach (var f in file)
                total += LoadOne(f);
            Logger.ShowInfo("Actors loaded...");
        }

        public void LoadShopLists(string path)
        {
            var file = VirtualFileSystemManager.Instance.FileSystem.SearchFile(path, "*.xml",
                SearchOption.AllDirectories);
            var total = 0;
            foreach (var f in file)
                total += LoadShopListOne(f);
            Logger.ShowInfo("Actors loaded...");
        }

        public int LoadShopListOne(string f)
        {
            var total = 0;
            var xml = new XmlDocument();
            try
            {
                XmlElement root;
                XmlNodeList list;
                var fs = VirtualFileSystemManager.Instance.FileSystem.OpenFile(f);
                xml.Load(fs);
                root = xml["GolemShop"];
                list = root.ChildNodes;
                byte id = 0;
                byte type = 0;
                uint slotid = 0;
                var gsi = new GolemShopItem();
                var item = new Dictionary<uint, GolemShopItem>();
                foreach (var j in list)
                {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;
                    switch (i.Name.ToLower())
                    {
                        case "id":
                            id = byte.Parse(i.InnerText);
                            break;
                        case "type":
                            if (i.InnerText.ToLower() == "sell")
                                type = 1;
                            break;
                        case "item":
                            var listi = i.ChildNodes;
                            foreach (XmlElement y in listi)
                                switch (y.Name.ToLower())
                                {
                                    case "id":
                                        gsi.ItemID = uint.Parse(y.InnerText);
                                        break;
                                    case "price":
                                        gsi.Price = uint.Parse(y.InnerText);
                                        break;
                                    case "count":
                                        gsi.Count = ushort.Parse(y.InnerText);
                                        break;
                                }

                            slotid++;
                            item.Add(slotid, gsi);
                            break;
                    }

                    if (type == 1)
                    {
                        if (!GolemSellList.ContainsKey(id))
                            GolemSellList.Add(id, item);
                    }
                    else
                    {
                        if (!GolemBuyList.ContainsKey(id))
                            GolemBuyList.Add(id, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            return total;
        }

        public int LoadOne(string f)
        {
            var total = 0;
            var xml = new XmlDocument();
            try
            {
                XmlElement root;
                XmlNodeList list;
                var actor = new Actor.Actor();
                var fi = new ActorFurniture();
                var Golem = new ActorGolem();
                var fs = VirtualFileSystemManager.Instance.FileSystem.OpenFile(f);
                xml.Load(fs);
                root = xml["Actors"];
                list = root.ChildNodes;
                foreach (var j in list)
                {
                    XmlElement i;
                    if (j.GetType() != typeof(XmlElement)) continue;
                    i = (XmlElement)j;

                    var type = i.Attributes["Type"].Value;

                    switch (type)
                    {
                        case "PC":
                            actor = new ActorPC();
                            break;
                        case "FI":
                            actor = new ActorFurniture();
                            break;
                        case "GOLEM":
                            actor = new ActorGolem();
                            break;
                    }

                    var skills = i.ChildNodes;
                    foreach (var j2 in skills)
                    {
                        XmlElement i2;
                        if (j2.GetType() != typeof(XmlElement)) continue;
                        i2 = (XmlElement)j2;

                        switch (i2.Name.ToLower())
                        {
                            case "mapid":
                                actor.MapID = uint.Parse(i2.InnerText);
                                break;
                            case "x":
                                if (actor.type == ActorType.FURNITURE)
                                {
                                    actor.X = short.Parse(i2.InnerText);
                                }
                                else
                                {
                                    actor.X = -1;
                                    actor.X2 = byte.Parse(i2.InnerText);
                                }

                                break;
                            case "y":
                                if (actor.type == ActorType.FURNITURE)
                                {
                                    actor.Y = short.Parse(i2.InnerText);
                                }
                                else
                                {
                                    actor.Y = -1;
                                    actor.Y2 = byte.Parse(i2.InnerText);
                                }

                                break;
                            case "dir":
                                actor.Dir = (ushort)(ushort.Parse(i2.InnerText) * 45);
                                break;
                        }

                        switch (type)
                        {
                            #region PC

                            case "PC":
                                actor.type = ActorType.PC;
                                var pc = (ActorPC)actor;
                                if (pc.Equips == null)
                                    pc.Equips = new uint[12];
                                pc.MaxHP = 100;
                                pc.HP = 100;
                                switch (i2.Name.ToLower())
                                {
                                    case "name":
                                        pc.Name = i2.InnerText;
                                        break;
                                    case "race":
                                        pc.Race = (PC_RACE)Enum.Parse(typeof(PC_RACE), i2.InnerText);
                                        break;
                                    case "gender":
                                        pc.Gender = (PC_GENDER)Enum.Parse(typeof(PC_GENDER), i2.InnerText);
                                        break;
                                    case "hairstyle":
                                        pc.HairStyle = ushort.Parse(i2.InnerText);
                                        break;
                                    case "haircolor":
                                        pc.HairColor = byte.Parse(i2.InnerText);
                                        break;
                                    case "wig":
                                        pc.Wig = ushort.Parse(i2.InnerText);
                                        break;
                                    case "face":
                                        pc.Face = ushort.Parse(i2.InnerText);
                                        break;
                                    case "tailstyle":
                                        pc.TailStyle = byte.Parse(i2.InnerText);
                                        break;
                                    case "wingstyle":
                                        pc.WingStyle = byte.Parse(i2.InnerText);
                                        break;
                                    case "wingcolor":
                                        pc.WingColor = byte.Parse(i2.InnerText);
                                        break;
                                    case "head_acce":
                                        pc.Equips[0] = uint.Parse(i2.InnerText);
                                        break;
                                    case "face_acce":
                                        pc.Equips[1] = uint.Parse(i2.InnerText);
                                        break;
                                    case "chest_acce":
                                        pc.Equips[2] = uint.Parse(i2.InnerText);
                                        break;
                                    case "upper_body":
                                        pc.Equips[3] = uint.Parse(i2.InnerText);
                                        break;
                                    case "lower_body":
                                        pc.Equips[4] = uint.Parse(i2.InnerText);
                                        break;
                                    case "back":
                                        pc.Equips[5] = uint.Parse(i2.InnerText);
                                        break;
                                    case "right_hand":
                                        pc.Equips[6] = uint.Parse(i2.InnerText);
                                        break;
                                    case "left_hand":
                                        pc.Equips[7] = uint.Parse(i2.InnerText);
                                        break;
                                    case "shoes":
                                        pc.Equips[8] = uint.Parse(i2.InnerText);
                                        break;
                                    case "socks":
                                        pc.Equips[9] = uint.Parse(i2.InnerText);
                                        break;
                                    case "pet":
                                        pc.Equips[10] = uint.Parse(i2.InnerText);
                                        break;
                                    case "effect":
                                        pc.Equips[11] = uint.Parse(i2.InnerText);
                                        break;
                                    case "trancel":
                                        pc.TranceID = uint.Parse(i2.InnerText);
                                        break;
                                    case "motion":
                                        pc.Motion = (MotionType)uint.Parse(i2.InnerText);
                                        pc.MotionLoop = true;
                                        break;
                                    case "shoptitle":
                                        var title = i2.InnerText;
                                        if (title != "")
                                        {
                                            pc.TInt["虚构玩家"] = 1;
                                            pc.TStr["虚构玩家店名"] = title;
                                        }

                                        break;
                                    case "titlesid":
                                        pc.AInt["称号_主语"] = int.Parse(i2.InnerText);
                                        break;
                                    case "titlecid":
                                        pc.AInt["称号_连词"] = int.Parse(i2.InnerText);
                                        break;
                                    case "titlepid":
                                        pc.AInt["称号_谓语"] = int.Parse(i2.InnerText);
                                        break;
                                    case "eventid":
                                        pc.TInt["虚拟玩家EventID"] = int.Parse(i2.InnerText);
                                        break;
                                    case "emotionid":
                                        pc.TInt["虚构玩家EmotionID"] = int.Parse(i2.InnerText);
                                        break;
                                }

                                break;

                            #endregion

                            #region FURNITURE

                            case "FI":
                                actor.type = ActorType.FURNITURE;
                                fi = (ActorFurniture)actor;
                                switch (i2.Name.ToLower())
                                {
                                    case "x":
                                        fi.X = short.Parse(i2.InnerText);
                                        break;
                                    case "y":
                                        fi.Y = short.Parse(i2.InnerText);
                                        break;
                                    case "z":
                                        fi.Z = short.Parse(i2.InnerText);
                                        break;
                                    case "xaxis":
                                        fi.Xaxis = short.Parse(i2.InnerText);
                                        break;
                                    case "yaxis":
                                        fi.Yaxis = short.Parse(i2.InnerText);
                                        break;
                                    case "zaxis":
                                        fi.Zaxis = short.Parse(i2.InnerText);
                                        break;
                                    case "name":
                                        fi.Name = i2.InnerText;
                                        break;
                                    case "motion":
                                        fi.Motion = ushort.Parse(i2.InnerText);
                                        break;
                                    case "pictid":
                                        fi.PictID = uint.Parse(i2.InnerText);
                                        break;
                                    case "itemid":
                                        fi.ItemID = uint.Parse(i2.InnerText);
                                        break;
                                }

                                break;

                            #endregion

                            #region GOLEM

                            case "GOLEM":
                                actor.type = ActorType.GOLEM;
                                Golem = (ActorGolem)actor;
                                switch (i2.Name.ToLower())
                                {
                                    case "name":
                                        Golem.Name = i2.InnerText;
                                        break;
                                    case "motion":
                                        Golem.Motion = ushort.Parse(i2.InnerText);
                                        Golem.MotionLoop = true;
                                        break;
                                    case "pictid":
                                        Golem.PictID = uint.Parse(i2.InnerText);
                                        break;
                                    case "eventid":
                                        //Golem.EventID = uint.Parse(i2.InnerText);
                                        break;
                                    case "title":
                                        Golem.Title = i2.InnerText;
                                        break;
                                    case "shoptype":
                                        var t = i2.InnerText.ToLower();
                                        if (t == "sell")
                                            Golem.GolemType = GolemType.Sell;
                                        else if (t == "buy")
                                            Golem.GolemType = GolemType.Buy;
                                        break;
                                    case "aitype":
                                        Golem.AIMode = byte.Parse(i2.InnerText);
                                        break;
                                }

                                break;

                            #endregion
                        }
                    }

                    if (actor.type == ActorType.FURNITURE)
                    {
                        if (!FictitiousActorsList.ContainsKey(fi.MapID))
                            FictitiousActorsList.Add(fi.MapID, new List<Actor.Actor>());
                        FictitiousActorsList[fi.MapID].Add(fi);
                    }
                    else
                    {
                        if (!FictitiousActorsList.ContainsKey(actor.MapID))
                            FictitiousActorsList.Add(actor.MapID, new List<Actor.Actor>());
                        FictitiousActorsList[actor.MapID].Add(actor);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            return total;
        }
    }
}