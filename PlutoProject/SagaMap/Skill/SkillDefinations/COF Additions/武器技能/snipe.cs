using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Mob;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.武器技能
{
    public class Snipe : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            /*if (sActor.Status.Additions.ContainsKey("狙击模式"))
            {
                sActor.Status.Additions["狙击模式"].AdditionEnd();
                sActor.Status.Additions.Remove("狙击模式");
                return;
            }*/
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actor = new ActorSkill(args.skill, sActor);
            actor.MapID = sActor.MapID;
            actor.X = sActor.X;
            actor.Y = sActor.Y;
            var ai = new MobAI(actor, true);
            var path = ai.FindPath(SagaLib.Global.PosX16to8(sActor.X, map.Width),
                SagaLib.Global.PosY16to8(sActor.Y, map.Height),
                SagaLib.Global.PosX16to8(dActor.X, map.Width), SagaLib.Global.PosY16to8(dActor.Y, map.Height));
            if (path.Count >= 2)
            {
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

            if (path.Count == 1)
            {
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
            while (path.Count > count + 1)
            {
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

            if (affected.Contains(dActor))
                affected.Remove(dActor);

            var arg2 = new SkillArg();
            arg2 = args.Clone();
            arg2.skill.BaseData.id = 100;
            SkillHandler.Instance.PhysicalAttack(sActor, affected, arg2, Elements.Neutral, 3f);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, arg2, sActor, true);
            arg2.skill.BaseData.id = 20014;

            var da = new List<Actor>();
            da.Add(dActor);

            SkillHandler.Instance.PhysicalAttack(sActor, da, args, SkillHandler.DefType.Def, Elements.Neutral, 0, 5f,
                false, 0, false, 100, 0);
        }

        #endregion
    }
}