using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     魔法革命（マジックリベレイション）
    /// </summary>
    public class MagHitUpCircle : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1000 * level;
            var rate = 1.0f + 0.2f * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 100, false);
            foreach (var act in affected)
                if (act.type == ActorType.PC)
                {
                    var skill = new MagHitUpCircleBuff(args.skill, act, lifetime, rate);
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        public class MagHitUpCircleBuff : DefaultBuff
        {
            public float Rate;

            public MagHitUpCircleBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, float rate)
                : base(skill, actor, "MagHitUpCircle", lifetime)
            {
                Rate = rate;
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
            }
        }

        #endregion
    }
}