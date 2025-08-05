using System;
using System.Xml;
using SagaLib;

namespace SagaDB.DefWar
{
    public class DefWarFactory : Factory<DefWarFactory, DefWar.DefWarData>
    {
        public DefWarFactory()
        {
            loadingTab = "Loading Defwar database";
            loadedTab = " Defwars loaded.";
            databaseName = "Defwar";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, DefWar.DefWarData item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(DefWar.DefWarData item)
        {
            return item.ID;
        }

        protected override void ParseCSV(DefWar.DefWarData item, string[] paras)
        {
            uint offset = 0;
            item.ID = uint.Parse(paras[0 + offset]);
            item.Title = paras[1 + offset];
        }


        public DefWar GetItem(uint id)
        {
            if (items.ContainsKey(id))
            {
                var item = new DefWar(items[id]);

                return item;
            }

            return null;
        }
    }
}