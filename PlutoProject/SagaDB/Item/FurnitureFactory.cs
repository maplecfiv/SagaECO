using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Item
{
    public class FurnitureFactory : Factory<FurnitureFactory, Furniture>
    {
        public FurnitureFactory()
        {
            loadingTab = "Loading furniture database";
            loadedTab = " furnitures loaded.";
            databaseName = "furniture";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, Furniture item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(Furniture item)
        {
            return item.ItemID;
        }

        public Furniture GetFurniture(uint id)
        {
            if (Items.ContainsKey(id))
            {
                var f = Items[id];
                return f;
            }

            Logger.ShowError("No Furniture Found! (" + id + ")");
            return null;
        }

        protected override void ParseCSV(Furniture item, string[] paras)
        {
            item.ItemID = uint.Parse(paras[0]);
            if (paras[1] == null || paras[1] == "0" || paras[1] == "")
                item.Name = "_";
            else
                item.Name = paras[1];

            item.PictID = uint.Parse(paras[2]);
            //item.Type = byte.Parse(paras[6]);
            item.EventID = uint.Parse(paras[3]);
            item.Capacity = ushort.Parse(paras[4]);
            item.DefaultMotion = ushort.Parse(paras[5]);
            for (var v = 6; v < 13; v++)
                if (ushort.Parse(paras[v]) > 0)
                    item.Motion.Add(ushort.Parse(paras[v]));
        }
    }
}