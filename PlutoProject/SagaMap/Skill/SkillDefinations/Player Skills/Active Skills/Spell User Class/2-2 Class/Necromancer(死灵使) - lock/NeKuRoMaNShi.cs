using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     黑暗魅力（ネクロマンシー）
    /// </summary>
    public class NeKuRoMaNShi : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 15000;
            var rate = 10 + 10 * level;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "NeKuRoMaNShi", rate))
            {
                var skill = new DefaultBuff(args.skill, dActor, "NeKuRoMaNShi", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            //變成不死
            actor.Buff.Undead = true;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            //恢復
            actor.Buff.Undead = false;
        }

        //#endregion
    }
}