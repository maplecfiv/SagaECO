using System;
using System.Linq;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.
    TreasureHunter_考古学家____rag
{
    /// <summary>
    ///     草鞭（プルウィップ）
    /// </summary>
    public class PullWhip : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.ROPE) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 1.2f + 0.7f * level;
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Skills2_2.ContainsKey(2337) || pc.DualJobSkill.Exists(x => x.ID == 2337))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 2337))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 2337).Level;

                    var mainlv = 0;
                    if (pc.Skills2_2.ContainsKey(2337))
                        mainlv = pc.Skills2_2[2337].Level;

                    var CATCH_Level = Math.Max(duallv, mainlv);
                    if (CATCH_Level <= 2)
                        factor += 1.15f + 0.75f * level;
                    else if (CATCH_Level > 2 && CATCH_Level < 4)
                        factor = 1.35f + 0.75f * level;
                    else
                        factor = 1.40f + 0.75f * level;
                }

                if (pc.Skills3.ContainsKey(992) || pc.DualJobSkill.Exists(x => x.ID == 992))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 992))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 992).Level;

                    //这里取主职的剑圣等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(992))
                        mainlv = pc.Skills3[992].Level;

                    //这里取等级最高的剑圣等级用来做居合的倍率加成
                    factor += 0.1f * Math.Max(duallv, mainlv);
                }
            }

            //if (sActorPC.Skills2.ContainsKey(CATCH_SkillID))
            //{
            //    int CATCH_Level= sActorPC.Skills2[CATCH_SkillID].Level;
            //    if (CATCH_Level <= 2)
            //    {
            //        factor += 1.15f + 0.75f * level;
            //    }
            //    else if (CATCH_Level > 2 && CATCH_Level < 4)
            //    {
            //        factor = 1.35f + 0.75f * level;
            //    }
            //    else
            //    {
            //        factor = 1.40f + 0.75f * level;
            //    }
            //}
            //if (sActorPC.SkillsReserve.ContainsKey(CATCH_SkillID))
            //{
            //    int CATCH_Level = sActorPC.Skills2[CATCH_SkillID].Level;
            //    if (CATCH_Level <= 2)
            //    {
            //        factor += 1.15f + 0.75f * level;
            //    }
            //    else if (CATCH_Level > 2 && CATCH_Level < 4)
            //    {
            //        factor = 1.35f + 0.75f * level;
            //    }
            //    else
            //    {
            //        factor = 1.40f + 0.75f * level;
            //    }
            //}
            //保留原始职业判定方式以防万一
            var pos = new short[2];
            pos[0] = sActor.X;
            pos[1] = sActor.Y;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var lifetime = 1000;
            var dskill = new Stiff(args.skill, dActor, lifetime);
            map.MoveActor(Map.MOVE_TYPE.START, dActor, pos, dActor.Dir, 20000, true, MoveType.BATTLE_MOTION);
            SkillHandler.ApplyAddition(dActor, dskill);
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            if (dActor.type == ActorType.MOB)
                if (!SkillHandler.Instance.isBossMob((ActorMob)dActor))
                {
                    var skill = new Stiff(args.skill, dActor, 1000);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
        }

        //#endregion
    }
}