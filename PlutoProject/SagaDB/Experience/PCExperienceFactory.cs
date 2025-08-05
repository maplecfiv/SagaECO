using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Experience
{
    public class PCExperienceFactory : Factory<PCExperienceFactory, PCLevel>
    {
        public PCExperienceFactory()
        {
            loadingTab = "Loading Experience table";
            loadedTab = " Experience table loaded.";
            databaseName = "EXP";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, PCLevel item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(PCLevel item)
        {
            return item.level;
        }

        protected override void ParseCSV(PCLevel item, string[] paras)
        {
            item.level = byte.Parse(paras[0]);
            item.cexp = ulong.Parse(paras[1]);
            item.cexp2 = ulong.Parse(paras[2]);
            item.jexp = ulong.Parse(paras[3]);
            item.jexp2 = ulong.Parse(paras[4]);
            item.jexp3 = ulong.Parse(paras[5]);
            item.dualjexp = ulong.Parse(paras[6]);
        }
    }
}