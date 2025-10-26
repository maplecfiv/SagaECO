using System;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Network.Client;
using SagaMap.Skill;

namespace SagaMap.Tasks.System {
    public class 南牢列车 : MultiRunTask {
        private static 南牢列车 instance;

        private uint ss = 0;

        public 南牢列车() {
            Period = 2000;
            DueTime = 0;
        }

        public static 南牢列车 Instance {
            get {
                if (instance == null)
                    instance = new 南牢列车();
                return instance;
            }
        }

        public override void CallBack() {
            //Period = Global.Random.Next(1500, 2000);
            //DateTime now = DateTime.Now;

            var map = MapManager.Instance.GetMap(20020000);
            var map2 = MapManager.Instance.GetMap(20021000);

            /*
            Actor[] actors = map.Actors.Values.ToArray();
            foreach (Actor i in actors)
            {
                if (i.type == ActorType.PC)
                {
                    MapClient.FromActorPC((ActorPC)i).SendAnnounce("老司機要開車囉～～～要上車的人快來吧");
                }
            }

            Actor[] actors2 = map2.Actors.Values.ToArray();
            foreach (Actor i in actors2)
            {
                if (i.type == ActorType.PC)
                {
                    MapClient.FromActorPC((ActorPC)i).SendAnnounce("老司機要開車囉～～～要上車的人快來吧");
                }
            }
            */

            //Logger.getLogger().Information("南牢火車Start");

            create(map, 150, 12, 150, 251);
            create(map, 105, 251, 105, 12);

            create(map2, 213, 16, 213, 23);
            create(map2, 213, 58, 213, 79);
        }

        private void create(Map map, byte x1, byte y1, byte x2, byte y2) {
            var skill = SkillFactory.Instance.GetSkill(8477, 1);
            var actor = new ActorSkill(skill, null);
            actor.Name = "列车";
            actor.MapID = map.ID;
            actor.X = Global.PosX8to16(x1, map.Width);
            actor.Y = Global.PosY8to16(y1, map.Height);
            actor.Speed = 1000;
            actor.e = new NullEventHandler();
            map.RegisterActor(actor);
            //Logger.getLogger().Error(actor.MapID.ToString()+" " +actor.ActorID.ToString());
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            actor.Stackable = false;
            var timer = new Activator(actor, map, x2, y2);
            timer.Activate();
        }

        private class Activator : MultiRunTask {
            private readonly Map map;
            private readonly int maxcount = 1000;
            private readonly ActorSkill skill;
            private readonly byte x;
            private readonly byte y;
            private int count;

            public Activator(ActorSkill skill, Map map, byte x2, byte y2) {
                Period = 45;
                this.skill = skill;
                this.map = map;
                x = x2;
                y = y2;
            }

            public override void CallBack() {
                try {
                    if (count % 2 == 0) {
                        var actors = map.GetRoundAreaActors(skill.X, skill.Y, 50);
                        foreach (var j in actors)
                            if (j != null)
                                if (j.type == ActorType.PC && j.HP > 0) {
                                    var damage = 3000;

                                    SkillHandler.Instance.CauseDamage(j, j, damage);
                                    SkillHandler.Instance.ShowVessel(j, damage);
                                    //SkillHandler.Instance.PushBack(skill, j, 1);
                                    if (j.HP <= 0 && j.Buff.Dead)
                                        MapClient.FromActorPC((ActorPC)j).TitleProccess((ActorPC)j, 66, 1);
                                }

                        var ai = new MobAI(skill, true);
                        var path = ai.FindPath(Global.PosX16to8(skill.X, map.Width),
                            Global.PosY16to8(skill.Y, map.Height),
                            x, y);
                        int deltaX = path[0].x;
                        int deltaY = path[0].y;
                        var node = new MapNode();
                        node.x = (byte)deltaX;
                        node.y = (byte)deltaY;
                        path.Add(node);
                        var pos = new short[2];
                        pos[0] = Global.PosX8to16(path[0].x, map.Width);
                        pos[1] = Global.PosY8to16(path[0].y, map.Height);
                        map.MoveActor(Map.MOVE_TYPE.START, skill, pos, 0, 200);
                    }

                    count++;
                    if ((Global.PosX16to8(skill.X, map.Width) == x && Global.PosY16to8(skill.Y, map.Height) == y) ||
                        count >= maxcount) {
                        map.DeleteActor(skill);
                        Deactivate();
                    }
                }
                catch (Exception ex) {
                    map.DeleteActor(skill);
                    Deactivate();
                    Logger.ShowError(ex);
                }
            }
        }
    }
}