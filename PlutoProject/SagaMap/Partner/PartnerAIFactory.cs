using System;
using System.Xml;
using SagaLib;

namespace SagaMap.Partner
{
    public class PartnerAIFactory : Factory<PartnerAIFactory, AIMode>
    {
        public PartnerAIFactory()
        {
            loadingTab = "Loading PartnerAI database";
            loadedTab = " AIs loaded.";
            databaseName = "Partner AI";
            FactoryType = FactoryType.XML;
        }

        protected override uint GetKey(AIMode item)
        {
            return item.PartnerID;
        }

        protected override void ParseCSV(AIMode item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, AIMode item)
        {
            switch (root.Name.ToLower())
            {
                case "mob":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            item.PartnerID = uint.Parse(current.InnerText);
                            break;
                        case "aimode":
                            item.AI = int.Parse(current.InnerText);
                            break;
                        case "eventattackingonskillcast":
                            if (current.Attributes.Count > 0)
                                item.EventAttackingSkillRate = int.Parse(current.Attributes["Rate"].InnerText);
                            else
                                item.EventAttackingSkillRate = 50;
                            break;
                        case "eventmastercombatonskillcast":
                            if (current.Attributes.Count > 0)
                                item.EventMasterCombatSkillRate = int.Parse(current.Attributes["Rate"].InnerText);
                            else
                                item.EventMasterCombatSkillRate = 50;
                            break;
                        case "useskillofshortrange":
                            item.Distance = int.Parse(current.Attributes["Distance"].InnerText);
                            item.ShortCD = int.Parse(current.Attributes["CD"].InnerText);
                            item.isNewAI = true;
                            break;
                        case "useskilloflongrange":
                            item.LongCD = int.Parse(current.Attributes["CD"].InnerText);
                            item.isNewAI = true;
                            break;
                        case "useskilllist":
                            item.isAnAI = true;
                            var ID = uint.Parse(current.Attributes["ID"].InnerText);
                            var MaxHP = int.Parse(current.Attributes["MaxHP"].InnerText);
                            var MinHP = int.Parse(current.Attributes["MinHP"].InnerText);
                            var Rate = int.Parse(current.Attributes["Rate"].InnerText);
                            var sl = new AIMode.SkillList();
                            sl.MaxHP = MaxHP;
                            sl.MinHP = MinHP;
                            sl.Rate = Rate;
                            item.AnAI_SkillAssemblage.Add(ID, sl);
                            switch (current.Name.ToLower())
                            {
                                case "skill":
                                    break;
                            }

                            break;
                    }

                    break;
                case "useskilllist":
                    switch (current.Name.ToLower())
                    {
                        case "skill":
                            var listid = uint.Parse(current.Attributes["ListID"].InnerText);
                            var si = new AIMode.SkillsInfo();
                            si.Delay = int.Parse(current.Attributes["Delay"].InnerText);
                            si.SkillID = uint.Parse(current.InnerText);
                            var Sequence = uint.Parse(current.Attributes["Sequence"].InnerText);
                            item.AnAI_SkillAssemblage[listid].AnAI_SkillList.Add(Sequence, si);
                            break;
                    }

                    break;
                case "eventattackingonskillcast":
                    switch (current.Name.ToLower())
                    {
                        case "skill":
                            var id = uint.Parse(current.InnerText);
                            var rate = int.Parse(current.Attributes["Rate"].InnerText);
                            item.EventAttacking.Add(id, rate);
                            break;
                    }

                    break;
                case "eventmastercombatonskillcast":
                    switch (current.Name.ToLower())
                    {
                        case "skill":
                            var id = uint.Parse(current.InnerText);
                            var rate = int.Parse(current.Attributes["Rate"].InnerText);
                            item.EventMasterCombat.Add(id, rate);
                            break;
                    }

                    break;

                case "useskillofshortrange":
                    switch (current.Name.ToLower())
                    {
                        case "skill":
                            var id = uint.Parse(current.InnerText);
                            var si = new AIMode.SkilInfo();
                            si.CD = int.Parse(current.Attributes["CD"].InnerText);
                            si.Rate = int.Parse(current.Attributes["Rate"].InnerText);
                            si.MaxHP = int.Parse(current.Attributes["MaxHP"].InnerText);
                            si.MinHP = int.Parse(current.Attributes["MinHP"].InnerText);
                            item.SkillOfShort.Add(id, si);
                            break;
                    }

                    break;
                case "useskilloflongrange":
                    switch (current.Name.ToLower())
                    {
                        case "skill":
                            var id = uint.Parse(current.InnerText);
                            var si = new AIMode.SkilInfo();
                            si.CD = int.Parse(current.Attributes["CD"].InnerText);
                            si.Rate = int.Parse(current.Attributes["Rate"].InnerText);
                            si.MaxHP = int.Parse(current.Attributes["MaxHP"].InnerText);
                            si.MinHP = int.Parse(current.Attributes["MinHP"].InnerText);
                            item.SkillOfLong.Add(id, si);
                            break;
                    }

                    break;
            }
        }
    }
}