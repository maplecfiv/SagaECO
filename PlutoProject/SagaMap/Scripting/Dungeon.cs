using SagaDB.Actor;
using SagaDB.Navi;
using SagaMap.Dungeon;
using SagaMap.Manager;

namespace SagaMap.Scripting {
    public class WestFortGate : Event {
        public WestFortGate() {
            EventID = 0xF1000000;
        }

        public override void OnEvent(ActorPC pc) {
            Warp(pc, 32003001, 20, 81);
        }
    }

    public class WestFortField : Event {
        public WestFortField() {
            EventID = 0xF1000001;
        }

        public override void OnEvent(ActorPC pc) {
            Warp(pc, 12019000, 5, 80);
        }
    }

    public class DungeonNorth : Event {
        public DungeonNorth() {
            EventID = 12001501;
        }

        public override void OnEvent(ActorPC pc) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.IsDungeon) {
                var next = map.DungeonMap.Gates[GateType.North].ConnectedMap;
                Warp(pc, next.Map.ID, next.Gates[GateType.South].X, next.Gates[GateType.South].Y);
            }
        }
    }

    public class DungeonEast : Event {
        public DungeonEast() {
            EventID = 12001502;
        }

        public override void OnEvent(ActorPC pc) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.IsDungeon) {
                var next = map.DungeonMap.Gates[GateType.East].ConnectedMap;
                Warp(pc, next.Map.ID, next.Gates[GateType.West].X, next.Gates[GateType.West].Y);
            }
        }
    }

    public class DungeonSouth : Event {
        public DungeonSouth() {
            EventID = 12001503;
        }

        public override void OnEvent(ActorPC pc) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.IsDungeon) {
                var next = map.DungeonMap.Gates[GateType.South].ConnectedMap;
                Warp(pc, next.Map.ID, next.Gates[GateType.North].X, next.Gates[GateType.North].Y);
            }
        }
    }

    public class DungeonWest : Event {
        public DungeonWest() {
            EventID = 12001504;
        }

        public override void OnEvent(ActorPC pc) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.IsDungeon) {
                var next = map.DungeonMap.Gates[GateType.West].ConnectedMap;
                Warp(pc, next.Map.ID, next.Gates[GateType.East].X, next.Gates[GateType.East].Y);
            }
        }
    }

    public class DungeonExit : Event {
        public DungeonExit() {
            EventID = 12001505;
        }

        public override void OnEvent(ActorPC pc) {
            var map = MapManager.Instance.GetMap(pc.MapID);
            if (map.IsDungeon) Warp(pc, map.ClientExitMap, map.ClientExitX, map.ClientExitY);
        }
    }
}