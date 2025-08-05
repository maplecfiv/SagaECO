using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Tamaire
{
    public class TamaireReward
    {
        public ulong cexp, jexp, cexp2, jexp3, demcexp, demjexp;
        public byte level;
    }

    public class TamaireExpRewardFactory : Factory<TamaireExpRewardFactory, TamaireReward>
    {
        public TamaireExpRewardFactory()
        {
            loadingTab = "Loading Tamaire Rewards";
            loadedTab = " Tamaire Rewards loaded.";
            databaseName = "TamaireReward";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, TamaireReward item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(TamaireReward item)
        {
            return item.level;
        }

        protected override void ParseCSV(TamaireReward item, string[] paras)
        {
            item.level = byte.Parse(paras[0]);
            item.cexp = ulong.Parse(paras[1]);
            item.jexp = ulong.Parse(paras[2]);
            item.cexp2 = ulong.Parse(paras[3]);
            item.jexp3 = ulong.Parse(paras[4]);
            item.demcexp = ulong.Parse(paras[5]);
            item.demjexp = ulong.Parse(paras[6]);
        }
    }
}