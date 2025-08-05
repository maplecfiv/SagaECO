using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.COF_Additions.领主骑士
{
    public class IceDef : MobISkill
    {
        #region ISkill Members

        public void BeforeCast(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var defadd = (short)(sActor.Status.def * 0.35f);
            sActor.Status.def_add_skill += defadd;

            var skill = new DefaultBuff(args.skill, sActor, "冰封坚韧", 9000000);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        #endregion
    }
}