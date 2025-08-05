using System;
using System.Collections.Generic;
using System.Xml;
using SagaLib;

namespace SagaDB.Iris
{
    public class IrisAbilityFactory : Factory<IrisAbilityFactory, AbilityVector>
    {
        public IrisAbilityFactory()
        {
            loadingTab = "Loading Ability database";
            loadedTab = " abilities loaded.";
            databaseName = "Iris Ability";
            FactoryType = FactoryType.CSV;
        }

        protected override void ParseXML(XmlElement root, XmlElement current, AbilityVector item)
        {
            throw new NotImplementedException();
        }

        protected override uint GetKey(AbilityVector item)
        {
            return item.ID;
        }

        protected override void ParseCSV(AbilityVector ability, string[] paras)
        {
            ability.ID = uint.Parse(paras[0]);
            ability.Name = paras[1];
            var list = new Dictionary<ReleaseAbility, int>();
            for (var i = 0; i < 10; i++)
            {
                if (paras[2 + i * 2] == "0")
                    continue;
                var releaseability = paras[2 + i * 2];
                var value = paras[3 + i * 2];
                var ra = (ReleaseAbility)Enum.Parse(typeof(ReleaseAbility), releaseability);
                if (!list.ContainsKey(ra))
                    list.Add(ra, int.Parse(value));
                else list[ra] = int.Parse(value);
                var listcopy = new Dictionary<ReleaseAbility, int>();
                foreach (var j in list.Keys) listcopy.Add(j, list[j]);
                ability.ReleaseAbilities.Add((byte)(i + 1), listcopy);
            }

            /*item.ID = uint.Parse(paras[0]);
            item.Name = paras[1];
            List<string> lll = new List<string>();
            for (int i = 0; i < 10; i++)
            {
                if (paras[2 + i * 2] == "0")
                    continue;
                string[] abilities = paras[2 + i * 2].Split('|');
                string[] values = paras[2 + i * 2 + 1].Split('|');

                string ab = paras[2 + i * 2];
                if (!lll.Contains(ab))
                    lll.Add(ab);

                Dictionary<ReleaseAbility, int> list = new Dictionary<ReleaseAbility, int>();
                item.Abilities.Add((byte)(i + 1), list);
                for (int j = 0; j < abilities.Length; j++)
                {
                    ReleaseAbility ability = (ReleaseAbility)(int.Parse(abilities[j]) - 1);
                    list.Add(ability, int.Parse(values[j]));
                }
            }*/
        }
    }
}