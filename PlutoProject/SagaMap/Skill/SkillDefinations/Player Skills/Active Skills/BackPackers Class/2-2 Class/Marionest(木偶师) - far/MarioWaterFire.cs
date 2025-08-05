using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Marionest
{
    /// <summary>
    ///     屬性混合魔法（ミストバーニング）
    /// </summary>
    public class MarioWaterFire : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 0.4f + 0.1f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (sActor is ActorPC)
            {
                var pc = sActor as ActorPC;

                //不管是主职还是副职, 只要习得剑圣技能, 都会导致combo成立, 这里一步就行了
                if (pc.Skills2_2.ContainsKey(981) || pc.DualJobSkill.Exists(x => x.ID == 981))
                {
                    var EleSelect = map.GetActorsArea(sActor, 400, false);
                    var ok = false;
                    foreach (var act in EleSelect)
                        //周边召唤活动木偶判定失败
                        //if (act.type == ActorType.PARTNER)
                        //{
                        //    ActorPartner partner = (ActorPartner)act;
                        //    if (partner.BaseData.id == 16090000)
                        //    {
                        //        continue;
                        //    }
                        //}
                        //判定玩家活动木偶
                        if (act.type == ActorType.PC)
                        {
                            var apc = (ActorPC)act;
                            if (apc.Marionette != null)
                                if (apc.Marionette.ID == 10013850) //烈焰蜥蜴
                                {
                                    ok = true;
                                    break;
                                }
                        }

                    if (ok)
                    {
                        var duallv = 0;
                        if (pc.DualJobSkill.Exists(x => x.ID == 981))
                            duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 981).Level;

                        var mainlv = 0;
                        if (pc.Skills2_2.ContainsKey(981))
                            mainlv = pc.Skills2_2[981].Level;

                        factor += 1.2f + 0.7f * Math.Max(duallv, mainlv);
                    }
                }

                if (pc.Marionette != null)
                    if (pc.Marionette.ID == 10019400) //鱼人
                        factor += 2.0f + 0.1f * level;
            }

            var affected = map.GetActorsArea(sActor, 400, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Earth, factor);
        }

        #endregion
    }
}