using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     霸邪之阵（Ragnarok）
    /// </summary>
    public class KyrieEleison : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //float factor = -14.3f;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 500, false); //实测7*7范围内怪物互补情况太差,更改为11*11
            var realAffected = new List<Actor>();
            var ActorlowHP = sActor;
            foreach (var act in affected)
                if (act.HP / act.MaxHP < (float)(ActorlowHP.HP / ActorlowHP.MaxHP) &&
                    !SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var lifetime = 120000;
                    var skill = new DefaultBuff(args.skill, dActor, "MobKyrie", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(dActor, skill);
                }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            skill["MobKyrie"] = 10;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}