using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     嗚嗚嗚啊~
    /// </summary>
    public class MobBokeboke : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var act = dActor;
            var lifetime = 3000;
            var rate = 4;
            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stone, rate))
            {
                var skill16 = new Stone(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill16);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Frosen, rate))
            {
                var skill17 = new Freeze(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill17);
            }

            rate = 6;
            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Poison, rate))
            {
                var skill18 = new Poison(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill18);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse, rate))
            {
                var skill5 = new Confuse(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill5);
            }

            rate = 8;
            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stun, rate))
            {
                var skill1 = new Stun(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill1);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Sleep, rate))
            {
                var skill4 = new Sleep(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill4);
            }

            rate = 10;
            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.鈍足, rate))
            {
                var skill2 = new MoveSpeedDown(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill2);
            }

            if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Silence, rate))
            {
                var skill3 = new Silence(args.skill, act, lifetime);
                SkillHandler.ApplyAddition(act, skill3);
            }
        }

        #endregion
    }
}