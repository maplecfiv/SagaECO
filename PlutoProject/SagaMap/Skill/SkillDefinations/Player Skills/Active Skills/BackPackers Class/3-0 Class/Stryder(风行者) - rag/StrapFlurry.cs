using System;
using System.Collections.Generic;
using System.Linq;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._3_0_Class.Stryder_风行者____rag
{
    internal class StrapFlurry : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.1f + 0.3f * level;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                //不管是主职还是副职,，检索技能是否存在
                if (pc.Skills2_2.ContainsKey(2337) || pc.DualJobSkill.Exists(x => x.ID == 2337))
                {
                    //这里取副职的2337等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 2337))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 2337).Level;

                    //这里取主职的2337等级
                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(2337))
                        mainlv = pc.Skills2_2[2337].Level;

                    //这里取等级最高的2337等级用来参与运算
                    //factor += 0.3f + 0.05f * Math.Max(duallv, mainlv);
                    //18.01.08修正伤害补正部分
                    factor += 1.9f + 0.1f * Math.Max(duallv, mainlv);
                    //factor += 0.3f + 0.05f * pc.Skills2_2[2337].Level;
                }

                if (pc.Skills3.ContainsKey(992) || pc.DualJobSkill.Exists(x => x.ID == 992))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 992))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 992).Level;
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(992))
                        mainlv = pc.Skills3[992].Level;
                    factor += 0.1f * Math.Max(duallv, mainlv);
                }
            }

            //ActorSkill actorS = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 400, true);
            var affected = new List<Actor>();
            var pos = new short[2];
            pos[0] = sActor.X;
            pos[1] = sActor.Y;
            foreach (var i in actors)
                //这里,应该是判定为可以攻击的对象才会被拉过来.
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    affected.Add(i);
                    //试图修复拉怪
                    //map.MoveActor(Map.MOVE_TYPE.START, dActor, pos, dActor.Dir, 20000, true, SagaLib.MoveType.BATTLE_MOTION);
                    map.MoveActor(Map.MOVE_TYPE.START, i, pos, i.Dir, 20000, true, MoveType.BATTLE_MOTION);
                }

            SkillHandler.Instance.PhysicalAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}