using System;
using System.Xml;
using SagaLib;

namespace SagaDB.Iris
{
    public class IrisCardFactory : Factory<IrisCardFactory, IrisCard>
    {
        public IrisCardFactory()
        {
            loadingTab = "Loading Iris Card database";
            loadedTab = " cards loaded.";
            databaseName = "Iris Card";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, IrisCard item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(IrisCard item)
        {
            return item.ID;
        }

        protected override void ParseCSV(IrisCard item, string[] paras)
        {
            item.ID = uint.Parse(paras[0]);

            item.Name = paras[3];
            item.Serial = paras[5];
            item.Page = uint.Parse(paras[6]);
            item.Slot = uint.Parse(paras[7]);
            item.Rarity = (Rarity)int.Parse(paras[8]);
            item.BeforeCard = uint.Parse(paras[9]);
            item.NextCard = uint.Parse(paras[10]);
            item.Rank = int.Parse(paras[11]);
            if (toBool(paras[12]))
                item.CanWeapon = true;
            if (toBool(paras[13]))
                item.CanArmor = true;
            if (toBool(paras[14]))
                item.CanNeck = true;
            for (var i = 0; i < 7; i++)
            {
                var element = (Elements)i;
                item.Elements.Add(element, int.Parse(paras[15 + i]));
            }

            for (var i = 0; i < 3; i++)
            {
                var id = uint.Parse(paras[22 + i * 2]);
                if (IrisAbilityFactory.Instance.Items.ContainsKey(id))
                {
                    var vector = IrisAbilityFactory.Instance.Items[id];
                    item.Abilities.Add(vector, int.Parse(paras[23 + i * 2]));
                }
            }
        }

        private bool toBool(string input)
        {
            if (input == "1") return true;
            return false;
        }
    }
}