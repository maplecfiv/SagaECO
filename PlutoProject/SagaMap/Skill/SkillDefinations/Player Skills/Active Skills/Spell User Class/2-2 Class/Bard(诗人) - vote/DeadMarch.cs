using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote
{
    /// <summary>
    ///     死亡進行曲（デッドマーチ）
    /// </summary>
    public class DeadMarch : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 10 * level;
            var factor = 1.0f + 0.5f * level;
            var lifetime = 4000;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 300, false);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Neutral, factor);
            foreach (var act in realAffected)
            {
                if (SkillHandler.Instance.isBossMob(act)) continue;
                if (act == sActor) continue;
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act)) continue;
                if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stun, rate))
                {
                    var skill1 = new Stun(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill1);
                }

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

                if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.CannotMove, rate))
                {
                    var skill4 = new CannotMove(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill4);
                }

                if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Confuse, rate))
                {
                    var skill5 = new Confuse(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill5);
                }
            }
        }

        #endregion
    }
}