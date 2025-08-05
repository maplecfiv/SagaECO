using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Knight
{
    /// <summary>
    ///     浴火重生（リヴァイブ）
    /// </summary>
    public class Revive : ISkill
    {
        private readonly int SkillLv;

        /// <summary>
        ///     初始化技能
        /// </summary>
        /// <param name="level">技能等級(0表示由玩家選擇)</param>
        public Revive(int level)
        {
            SkillLv = level;
        }

        public Revive()
        {
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 30000 * level;
            var realdActor = dActor;
            if (sActor.type == ActorType.PET)
            {
                var pet = (ActorPet)sActor;
                realdActor = pet.Owner;
            }

            var skill = new DefaultBuff(args.skill, realdActor, "Revive", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realdActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var theLevel = SkillLv == 0 ? skill.skill.Level : SkillLv;
            actor.Status.autoReviveRate += (short)(10 * theLevel);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var theLevel = SkillLv == 0 ? skill.skill.Level : SkillLv;
            actor.Status.autoReviveRate -= (short)(10 * theLevel);
        }

        #endregion
    }
}