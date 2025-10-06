using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SagaLib;

namespace SagaDB.DEMIC
{
    public class ChipShopFactory : Factory<ChipShopFactory, ChipShopCategory>
    {
        private ShopChip lastItem;

        public ChipShopFactory()
        {
            loadingTab = "Loading Chip shop database";
            loadedTab = " shop caterories loaded.";
            databaseName = " Chip shop";
            FactoryType = FactoryType.XML;
        }

        public List<ChipShopCategory> GetCategoryFromLv(byte lv)
        {
            var r = from category in Items.Values
                where category.PossibleLv <= lv
                select category;
            return r.ToList();
        }

        protected override uint GetKey(ChipShopCategory item)
        {
            return item.ID;
        }

        protected override void ParseCSV(ChipShopCategory item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, ChipShopCategory item)
        {
            switch (root.Name.ToLower())
            {
                case "category":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            item.ID = uint.Parse(current.InnerText);
                            break;
                        case "name":
                            item.Name = current.InnerText;
                            break;
                        case "lv":
                            item.PossibleLv = byte.Parse(current.InnerText);
                            break;
                    }

                    break;
                case "item":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            var newItem = new ShopChip();
                            var itemID = uint.Parse(current.InnerText);
                            if (!item.Items.ContainsKey(itemID))
                                item.Items.Add(itemID, newItem);
                            else
                                Logger.ShowWarning(string.Format(
                                    "Item:{0} already added for shop category:{1}! overwriting....", itemID, item.ID));
                            newItem.ItemID = itemID;
                            lastItem = newItem;
                            break;
                        case "exp":
                            lastItem.EXP = uint.Parse(current.InnerText);
                            break;
                        case "jexp":
                            lastItem.JEXP = uint.Parse(current.InnerText);
                            break;
                        case "comment":
                            lastItem.Description = current.InnerText;
                            break;
                    }

                    break;
            }
        }
    }
}