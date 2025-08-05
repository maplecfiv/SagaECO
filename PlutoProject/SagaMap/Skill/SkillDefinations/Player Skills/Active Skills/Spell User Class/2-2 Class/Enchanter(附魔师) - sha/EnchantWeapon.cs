using SagaDB.Actor;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Enchanter
{
    /// <summary>
    ///     迷惑武器（エンチャントウエポン）
    /// </summary>
    public class EnchantWeapon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skill = new DefaultBuff(args.skill, dActor, "EnchantWeapon", 120000);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.CInt["EnchantWeaponLevel"] = skill.skill.Level;
            }
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (actor.type == ActorType.PC)
            {
                var pc = (ActorPC)actor;
                pc.CInt["EnchantWeaponLevel"] = 0;
            }
        }

        #endregion
    }
}