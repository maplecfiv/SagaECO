using System;
using System.Xml;
using SagaLib;

namespace SagaMap.Dungeon
{
    public class DungeonMapsFactory : Factory<DungeonMapsFactory, DungeonMap>
    {
        public DungeonMapsFactory()
        {
            loadingTab = "Loading Dungeon Map database";
            loadedTab = " dungeon maps loaded.";
            databaseName = "dungeon map";
            FactoryType = FactoryType.XML;
        }

        protected override uint GetKey(DungeonMap item)
        {
            return item.ID;
        }

        protected override void ParseCSV(DungeonMap item, string[] paras)
        {
            throw new NotImplementedException();
        }

        protected override void ParseXML(XmlElement root, XmlElement current, DungeonMap item)
        {
            switch (root.Name.ToLower())
            {
                case "map":
                    switch (current.Name.ToLower())
                    {
                        case "id":
                            item.ID = uint.Parse(current.InnerText);
                            break;
                        case "type":
                            item.MapType = (MapType)Enum.Parse(typeof(MapType), current.InnerText);
                            break;
                        case "theme":
                            item.Theme = (Theme)Enum.Parse(typeof(Theme), current.InnerText);
                            break;
                        case "gate":
                            var type = (GateType)Enum.Parse(typeof(GateType), current.GetAttribute("type"));
                            var x = byte.Parse(current.GetAttribute("x"));
                            var y = byte.Parse(current.GetAttribute("y"));
                            var npcID = uint.Parse(current.InnerText);
                            if (!item.Gates.ContainsKey(type))
                            {
                                var gate = new DungeonGate();
                                gate.GateType = type;
                                gate.X = x;
                                gate.Y = y;
                                gate.NPCID = npcID;
                                item.Gates.Add(type, gate);
                            }

                            break;
                    }

                    break;
            }
        }
    }
}