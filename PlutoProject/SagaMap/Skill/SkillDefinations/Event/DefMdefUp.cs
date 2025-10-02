using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     打傘
    /// </summary>
    public class DefMdefUp : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var skill = new DefaultBuff(args.skill, dActor, "DefMdefUp", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //右防禦
            var def_add_add = 10;
            if (skill.Variable.ContainsKey("DefMdefUp_def_add"))
                skill.Variable.Remove("DefMdefUp_def_add");
            skill.Variable.Add("DefMdefUp_def_add", def_add_add);
            actor.Status.def_add_skill += (short)def_add_add;

            //右魔防
            var mdef_add_add = 10;
            if (skill.Variable.ContainsKey("DefMdefUp_mdef_add"))
                skill.Variable.Remove("DefMdefUp_mdef_add");
            skill.Variable.Add("DefMdefUp_mdef_add", mdef_add_add);
            actor.Status.mdef_add_skill += (short)mdef_add_add;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //右防禦
            actor.Status.def_add_skill -= (short)skill.Variable["DefMdefUp_def_add"];

            //右魔防
            actor.Status.mdef_add_skill -= (short)skill.Variable["DefMdefUp_mdef_add"];
        }

        //#endregion
    }
}