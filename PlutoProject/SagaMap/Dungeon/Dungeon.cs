using System.Collections.Generic;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Packets.Server.NPC;

namespace SagaMap.Dungeon
{
    public enum DestroyType
    {
        BossDown,
        QuestCancel,
        PartyDismiss,
        PartyMemberChange,
        TimeOver
    }

    public class Dungeon
    {
        public uint ID { get; set; }

        public uint DungeonID { get; set; }

        public int TimeLimit { get; set; }

        public Theme Theme { get; set; }

        public uint StartMap { get; set; }

        public uint EndMap { get; set; }

        public int MaxRoomCount { get; set; }

        public int MaxCrossCount { get; set; }

        public int MaxFloorCount { get; set; }

        public string SpawnFile { get; set; }

        public Tasks.Dungeon.Dungeon DestroyTask { get; private set; }

        public ActorPC Creator { get; set; }

        public List<DungeonMap> Maps { get; } = new List<DungeonMap>();

        public DungeonMap Start { get; set; }

        public DungeonMap End { get; set; }

        public Dungeon Clone()
        {
            var dungeon = new Dungeon();
            dungeon.ID = ID;
            dungeon.TimeLimit = TimeLimit;
            dungeon.Theme = Theme;
            dungeon.StartMap = StartMap;
            dungeon.EndMap = EndMap;
            dungeon.MaxCrossCount = MaxCrossCount;
            dungeon.MaxFloorCount = MaxFloorCount;
            dungeon.MaxRoomCount = MaxRoomCount;
            dungeon.SpawnFile = SpawnFile;
            dungeon.DestroyTask = new Tasks.Dungeon.Dungeon(dungeon, dungeon.TimeLimit);
            return dungeon;
        }

        public void Destory(DestroyType type)
        {
            switch (type)
            {
                case DestroyType.BossDown:
                    {
                        foreach (var j in End.Map.Actors.Values)
                            if (j.type == ActorType.PC)
                                if (((ActorPC)j).Online)
                                {
                                    var eh = (PCEventHandler)j.e;
                                    var p1 = new SSMG_NPC_SET_EVENT_AREA();
                                    if (End.Gates.ContainsKey(GateType.Exit))
                                    {
                                        p1.StartX = End.Gates[GateType.Exit].X;
                                        p1.EndX = End.Gates[GateType.Exit].X;
                                        p1.StartY = End.Gates[GateType.Exit].Y;
                                        p1.EndY = End.Gates[GateType.Exit].Y;
                                    }
                                    else
                                    {
                                        p1.StartX = (byte)(End.Map.Width / 2);
                                        p1.EndX = (byte)(End.Map.Width / 2);
                                        p1.StartY = (byte)(End.Map.Height / 2);
                                        p1.EndY = (byte)(End.Map.Height / 2);
                                    }

                                    p1.EventID = 12001505;
                                    p1.EffectID = 9005;
                                    eh.Client.NetIo.SendPacket(p1);
                                }

                        DestroyTask.counter = DestroyTask.lifeTime - 31;
                    }
                    break;
                case DestroyType.PartyDismiss:
                    {
                        foreach (var i in Maps)
                            foreach (var j in i.Map.Actors.Values)
                                if (j.type == ActorType.PC)
                                    if (((ActorPC)j).Online)
                                    {
                                        var eh = (PCEventHandler)j.e;
                                        eh.Client.SendSystemMessage(LocalManager.Instance.Strings.ITD_PARTY_DISMISSED);
                                    }

                        DestroyTask.counter = DestroyTask.lifeTime - 31;
                    }
                    break;
                case DestroyType.PartyMemberChange:
                    {
                        foreach (var i in Maps)
                            foreach (var j in i.Map.Actors.Values)
                                if (j.type == ActorType.PC)
                                    if (((ActorPC)j).Online)
                                    {
                                        var eh = (PCEventHandler)j.e;
                                        eh.Client.SendSystemMessage("队伍成员发生异常变更!");
                                    }

                        DestroyTask.counter = DestroyTask.lifeTime - 31;
                    }
                    break;
                case DestroyType.QuestCancel:
                    {
                        foreach (var i in Maps)
                            foreach (var j in i.Map.Actors.Values)
                                if (j.type == ActorType.PC)
                                    if (((ActorPC)j).Online)
                                    {
                                        var eh = (PCEventHandler)j.e;
                                        eh.Client.SendSystemMessage(LocalManager.Instance.Strings.ITD_QUEST_CANCEL);
                                    }

                        DestroyTask.counter = DestroyTask.lifeTime - 31;
                    }
                    break;
                case DestroyType.TimeOver:
                    {
                        foreach (var i in Maps)
                        {
                            MapManager.Instance.DeleteMapInstance(i.Map.ID);
                            i.Map.DungeonMap = null;
                            i.Map = null;
                        }

                        Maps.Clear();
                        Creator.DungeonID = 0;
                        DungeonFactory.Instance.RemoveDungeon(DungeonID);
                    }
                    break;
            }
        }
    }
}