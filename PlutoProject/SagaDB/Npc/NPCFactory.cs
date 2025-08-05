using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Npc
{
    public class NPCFactory : Factory<NPCFactory, NPC>
    {
        public NPCFactory()
        {
            loadingTab = "Loading NPC database";
            loadedTab = " npcs loaded.";
            databaseName = "npc";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, NPC item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(NPC item)
        {
            return item.ID;
        }

        protected override void ParseCSV(NPC item, string[] paras)
        {
            item.ID = uint.Parse(paras[0]);
            if (paras[1] == null || paras[1] == "0" || paras[1] == "")
                item.Name = "_";
            else
                item.Name = paras[1];

            item.MapID = uint.Parse(paras[2]);
            item.X = byte.Parse(paras[3]);
            item.Y = byte.Parse(paras[4]);
        }
    }
}