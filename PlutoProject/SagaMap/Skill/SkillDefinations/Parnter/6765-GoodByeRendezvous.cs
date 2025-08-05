using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Global
{
    /// <summary>
    ///     さよならランデヴー
    /// </summary>
    public class GoodByeRendezvous : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 50;
            var factor = 2.0f;
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