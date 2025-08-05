using System;
using System.Collections.Generic;
using System.Xml;
using SagaLib;

namespace SagaDB.Fish
{
    public class FishFactory : FactoryString<FishFactory, FishList>
    {
        public List<Fish> items = new List<Fish>();
        public int TotalRate;

        public FishFactory()
        {
            loadingTab = "Loading FishList database";
            loadedTab = " Fish groups loaded.";
            databaseName = "Fish";
            FactoryType = FactoryType.XML;
        }

        protected override string GetKey(FishList item)
        {
            return "钓鱼";
        }

        protected override void ParseCSV(FishList item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, FishList item)
        {
            switch (current.Name.ToLower())
            {
                case "item":
                    var fish = new Fish();
                    fish.ID = uint.Parse(current.InnerText);
                    fish.Rate = int.Parse(current.GetAttribute("rate"));
                    fish.Count = int.Parse(current.GetAttribute("count"));
                    items.Add(fish);
                    TotalRate += fish.Rate;
                    break;
            }
        }

        public Fish GetRandomItem(string groupName)
        {
            if (items != null)
            {
                var ran = Global.Random.Next(0, TotalRate);
                var determinator = 0;
                foreach (var i in items)
                {
                    determinator += i.Rate;
                    if (ran <= determinator)
                        return i;
                }

                return null;
            }

            Logger.ShowDebug("Cannot find FishGroup:" + groupName, Logger.defaultlogger);
            return null;
        }
    }
}