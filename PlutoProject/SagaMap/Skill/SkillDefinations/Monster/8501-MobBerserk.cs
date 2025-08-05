using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     光戰士
    /// </summary>
    public class MobBerserk : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = false;
            if (dActor.HP < (uint)(dActor.MaxHP * 0.6f))
            {
                var rate = 90;
                if (SagaLib.Global.Random.Next(0, 99) < rate) active = true;
            }

            var skill = new DefaultPassiveSkill(args.skill, sActor, "MobBerserk", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            var skill2 = new Berserk(skill.skill, actor, 60000);
            SkillHandler.ApplyAddition(actor, skill2);
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}