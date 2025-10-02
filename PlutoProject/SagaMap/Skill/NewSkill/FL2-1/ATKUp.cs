using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_1
{
    /// <summary>
    ///     攻擊．煥發 アタックバースト
    /// </summary>
    public class ATKUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0; //不显示效果
            var life = 0;
            life = (280 - 20 * level) * 1000;
            var skill = new DefaultBuff(args.skill, dActor, "ATKUp", life);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var values = new short[] { 0, 5, 9, 13, 17, 20, 100 };
            short value;
            value = values[skill.skill.Level];

            if (skill.Variable.ContainsKey("ATK"))
                skill.Variable.Remove("ATK");
            skill.Variable.Add("ATK", value);
            actor.Status.max_atk1_skill += value;
            actor.Status.max_atk2_skill += value;
            actor.Status.max_atk3_skill += value;

            actor.Buff.MaxAtkUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var value = skill.Variable["ATK"];
            actor.Status.max_atk1_skill -= (short)value;
            actor.Status.max_atk2_skill -= (short)value;
            actor.Status.max_atk3_skill -= (short)value;

            actor.Buff.MaxAtkUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}