using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.
    Elementaler_元素使____sha {
    internal class ChainLightning : ISkill {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args) {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actor = new ActorSkill(args.skill, sActor);
            actor.MapID = sActor.MapID;
            actor.X = sActor.X;
            actor.Y = sActor.Y;
            var ai = new MobAI(actor, true);
            var path = ai.FindPath(SagaLib.Global.PosX16to8(sActor.X, map.Width),
                SagaLib.Global.PosY16to8(sActor.Y, map.Height), args.x, args.y);
            if (path.Count >= 2) {
                //根据现有路径推算一步
                var deltaX = path[path.Count - 1].x - path[path.Count - 2].x;
                var deltaY = path[path.Count - 1].y - path[path.Count - 2].y;
                deltaX = path[path.Count - 1].x + deltaX;
                deltaY = path[path.Count - 1].y + deltaY;
                var node = new MapNode();
                node.x = (byte)deltaX;
                node.y = (byte)deltaY;
                path.Add(node);
            }

            if (path.Count == 1) {
                //根据现有路径推算一步
                var deltaX = path[path.Count - 1].x - SagaLib.Global.PosX16to8(sActor.X, map.Width);
                var deltaY = path[path.Count - 1].y - SagaLib.Global.PosY16to8(sActor.Y, map.Height);
                deltaX = path[path.Count - 1].x + deltaX;
                deltaY = path[path.Count - 1].y + deltaY;
                var node = new MapNode();
                node.x = (byte)deltaX;
                node.y = (byte)deltaY;
                path.Add(node);
            }

            var pos2 = new short[2];
            var affected = new List<Actor>();
            List<Actor> list;
            var count = -1;
            while (path.Count > count + 1) {
                pos2[0] = SagaLib.Global.PosX8to16(path[count + 1].x, map.Width);
                pos2[1] = SagaLib.Global.PosY8to16(path[count + 1].y, map.Height);
                //取得当前格子内的Actor
                list = map.GetActorsArea(pos2[0], pos2[1], 50);
                //筛选有效对象
                foreach (var i in list)
                    if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                        affected.Add(i);
                count++;
            }

            //Sum the total level of wind element skill
            var Me = (ActorPC)sActor;
            var Skill_Shaman = new List<int>();
            Skill_Shaman.Add(3028);
            Skill_Shaman.Add(3025);
            Skill_Shaman.Add(3019);
            Skill_Shaman.Add(3018);
            Skill_Shaman.Add(3020);
            Skill_Shaman.Add(3017);
            var TotalLv = 0;
            foreach (uint j in Skill_Shaman) {
                if (Me.Skills.ContainsKey(j))
                    TotalLv = TotalLv + Me.Skills[j].lv;
                if (Me.Skills2.ContainsKey(j))
                    TotalLv = TotalLv + Me.Skills2[j].lv;
            }

            var factor = 1f;
            if (TotalLv >= 5 && TotalLv >= 1)
                factor = 1f;
            else if (TotalLv >= 8 && TotalLv >= 6)
                factor = 1.5f;
            else if (TotalLv >= 11 && TotalLv >= 9)
                factor = 2.0f;
            else if (TotalLv >= 35 && TotalLv >= 12)
                factor = 2.5f;
            factor = factor * (float)2.5;
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor);
        }

        //#endregion
    }
}