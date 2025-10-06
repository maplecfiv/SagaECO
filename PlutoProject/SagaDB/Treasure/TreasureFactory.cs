using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Treasure
{
    public class TreasureFactory : FactoryString<TreasureFactory, TreasureList>
    {
        public TreasureFactory()
        {
            loadingTab = "Loading Treasure database";
            loadedTab = " treasure groups loaded.";
            databaseName = "treasure";
            FactoryType = FactoryType.XML;
        }

        protected override string GetKey(TreasureList item)
        {
            return item.Name;
        }

        protected override void ParseCSV(TreasureList item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, TreasureList item)
        {
            switch (current.Name.ToLower())
            {
                case "treasurelist":
                    item.Name = current.Attributes[0].InnerText;
                    break;
                case "item":
                    var treasure = new TreasureItem();
                    treasure.ID = uint.Parse(current.InnerText);
                    treasure.Rate = int.Parse(current.GetAttribute("rate"));
                    treasure.Count = int.Parse(current.GetAttribute("count"));
                    item.Items.Add(treasure);
                    item.TotalRate += treasure.Rate;
                    break;
            }
        }

        public TreasureItem GetRandomItem(string groupName)
        {
            if (Items.ContainsKey(groupName))
            {
                var list = Items[groupName];
                var ran = Global.Random.Next(0, list.TotalRate);
                var determinator = 0;
                foreach (var i in list.Items)
                {
                    determinator += i.Rate;
                    if (ran <= determinator)
                        return i;
                }

                return null;
            }

            Logger.ShowDebug("Cannot find TreasureGroup:" + groupName, Logger.defaultlogger);
            return null;
        }
    }
}