using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     生命力下降
    /// </summary>
    public class MobVitdownOne : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000;
            var skill = new DefaultBuff(args.skill, dActor, "MobVitdownOne", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                //VIT
                var vit_add = -(int)(pc.Vit * 0.05f);
                if (skill.Variable.ContainsKey("MobVitdownOne_vit"))
                    skill.Variable.Remove("MobVitdownOne_vit");
                skill.Variable.Add("MobVitdownOne_vit", vit_add);
                actor.Status.vit_skill += (short)vit_add;
                actor.Buff.VITDown = true;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                //VIT
                actor.Status.vit_skill -= (short)skill.Variable["MobVitdownOne_vit"];
                actor.Buff.VITDown = false;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        #endregion
    }
}