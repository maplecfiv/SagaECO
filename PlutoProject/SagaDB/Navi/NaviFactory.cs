using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Navi
{
    public class NaviFactory : Factory<NaviFactory, Navi>
    {
        private uint i;

        public NaviFactory()
        {
            loadingTab = "Loading navi database";
            loadedTab = " navis loaded.";
            databaseName = "navi";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, Navi item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(Navi item)
        {
            return i;
        }

        protected override void ParseCSV(Navi item, string[] paras)
        {
            var stepUniqueId = uint.Parse(paras[0]);
            var categoryId = uint.Parse(paras[1]);
            var eventId = uint.Parse(paras[2]);
            var stepId = uint.Parse(paras[3]);
            if (!item.Categories.ContainsKey(categoryId)) item.Categories.Add(categoryId, new Category(categoryId));
            var c = item.Categories[categoryId];
            if (!c.Events.ContainsKey(eventId)) c.Events.Add(eventId, new Event(eventId));
            var e = c.Events[eventId];

            var s = new Step(stepId, stepUniqueId, e);
            e.Steps.Add(stepId, s);
            item.UniqueSteps.Add(stepUniqueId, s);
            i++;
        }
    }
}