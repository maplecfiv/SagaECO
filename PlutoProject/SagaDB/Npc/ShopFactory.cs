using System;
using System.Xml;
using SagaDB.Item;
using SagaLib;

namespace SagaDB.Npc
{
    public class ShopFactory : Factory<ShopFactory, Shop>
    {
        public ShopFactory()
        {
            loadingTab = "Loading Shop database";
            loadedTab = " shops loaded.";
            databaseName = "shop";
            FactoryType = FactoryType.XML;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, Shop item)
        {
            switch (root.Name.ToLower())
            {
                case "shop":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            item.ID = uint.Parse(current.InnerText);
                            break;
                        case "npc":
                            var npcs = current.InnerText.Split(',');
                            foreach (var i in npcs) item.RelatedNPC.Add(uint.Parse(i));
                            break;
                        case "sellrate":
                            item.SellRate = uint.Parse(current.InnerText);
                            break;
                        case "buyrate":
                            item.BuyRate = uint.Parse(current.InnerText);
                            break;
                        case "buylimit":
                            item.BuyLimit = uint.Parse(current.InnerText);
                            break;
                        case "goods":
                        {
                            if (ItemFactory.Instance.GetItem(uint.Parse(current.InnerText)).BaseData.itemType !=
                                ItemType.POTION
                                && ItemFactory.Instance.GetItem(uint.Parse(current.InnerText)).BaseData.itemType !=
                                ItemType.FOOD)
                            {
                                item.Goods.Add(uint.Parse(current.InnerText));
                            }
                            else
                            {
                                if (ItemFactory.Instance.Items.ContainsKey(uint.Parse(current.InnerText)))
                                    item.Goods.Add(uint.Parse(current.InnerText));
                                else
                                    item.Goods.Add(10022900);
                            }
                        }
                            break;
                        case "shoptype":
                        {
                            item.ShopType = (ShopType)byte.Parse(current.InnerText);
                        }
                            break;
                    }

                    break;
            }
        }

        protected override uint GetKey(Shop item)
        {
            return item.ID;
        }

        protected override void ParseCSV(Shop item, string[] paras)
        {
            throw new NotImplementedException();
        }
    }
}