using System;
using System.Xml;
using SagaLib;

namespace SagaDB.ECOShop
{
    public class ECOShopFactory : Factory<ECOShopFactory, ShopCategory>
    {
        private ShopItem lastItem;

        public ECOShopFactory()
        {
            loadingTab = "Loading ECO shop database";
            loadedTab = " shop caterories loaded.";
            databaseName = " ECO shop";
            FactoryType = FactoryType.XML;
        }

        protected override uint GetKey(ShopCategory item)
        {
            return item.ID;
        }

        protected override void ParseCSV(ShopCategory item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, ShopCategory item)
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
                    }

                    break;
                case "item":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            var newItem = new ShopItem();
                            var itemID = uint.Parse(current.InnerText);
                            if (!item.Items.ContainsKey(itemID))
                                item.Items.Add(itemID, newItem);
                            else
                                Logger.GetLogger().Warning(string.Format(
                                    "Item:{0} already added for shop category:{1}! overwriting....", itemID, item.ID));
                            lastItem = newItem;
                            break;
                        case "points":
                            lastItem.points = uint.Parse(current.InnerText);
                            break;
                        case "comment":
                            lastItem.comment = current.InnerText;
                            break;
                        case "rentalminutes":
                            lastItem.rental = int.Parse(current.InnerText);
                            break;
                    }

                    break;
            }
        }
    }
}