using SagaDB.Actor;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR1
{
    /// <summary>
    ///     遠距離迴避
    /// </summary>
    public class LAvoUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (sActor.type == ActorType.PC)
            {
                active = true;
                var skill = new DefaultPassiveSkill(args.skill, sActor, "LAVOUp", active);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }


        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            int value;
            value = 4 + 4 * skill.skill.Level;


            if (skill.Variable.ContainsKey("LAVOUp-HitUp"))
                skill.Variable.Remove("LAVOUp-HitUp");
            skill.Variable.Add("LAVOUp-HitUp", value);
            actor.Status.hit_ranged_skill += (short)value;

            value = 2 + 4 * skill.skill.Level;

            if (skill.Variable.ContainsKey("LAVOUp-VoUp"))
                skill.Variable.Remove("LAVOUp-VoUp");
            skill.Variable.Add("LAVOUp-VoUp", value);
            actor.Status.avoid_ranged_skill += (short)value;
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            if (actor.type == ActorType.PC)
            {
                var value = skill.Variable["LAVOUp-HitUp"];
                actor.Status.hit_ranged_skill -= (short)value;
                value = skill.Variable["LAVOUp-VoUp"];
                actor.Status.avoid_ranged_skill -= (short)value;
            }
        }

        //#endregion
    }
}