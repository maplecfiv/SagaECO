using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Ring
{
    public class RingFame
    {
        public uint Level { get; set; }

        public uint Fame { get; set; }
    }

    public class RingFameTable : Factory<RingFameTable, RingFame>
    {
        public RingFameTable()
        {
            loadingTab = "Loading Ring Fame database";
            loadedTab = " entries loaded.";
            databaseName = " Ring fame";
            FactoryType = FactoryType.XML;
        }

        protected override uint GetKey(RingFame item)
        {
            return item.Level;
        }

        protected override void ParseCSV(RingFame item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, RingFame item)
        {
            switch (root.Name.ToLower())
            {
                case "level":
                    switch (current.Name.ToLower())
                    {
                        case "level":
                            item.Level = uint.Parse(current.InnerText);
                            break;
                        case "fame":
                            item.Fame = uint.Parse(current.InnerText);
                            break;
                    }

                    break;
            }
        }
    }
}