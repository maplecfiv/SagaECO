using SagaDB.Actor;
using SagaDB.Item;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Bard_诗人____vote
{
    /// <summary>
    ///     攻擊進行曲（アトラクトマーチ）
    /// </summary>
    public class AttractMarch : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.isEquipmentRight(sActor, ItemType.STRINGS) ||
                sActor.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0) return 0;
            return -5;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000 + 1000 * level;
            var rate = 20 * level - 10;
            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, "AttractMarch", rate))
            {
                if (dActor.type == ActorType.PC)
                {
                    dActor.HP = 0;
                    dActor.e.OnDie();
                    args.affectedActors.Add(dActor);
                    args.Init();
                    args.flag[0] = AttackFlag.DIE;
                }
                else
                {
                    var skill = new AttractMarchBuff(args.skill, sActor, dActor, lifetime);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
            }
        }

        public class AttractMarchBuff : DefaultBuff
        {
            private readonly Actor sActor;

            public AttractMarchBuff(SagaDB.Skill.Skill skill, Actor sActor, Actor dActor, int lifetime)
                : base(skill, dActor, "AttractMarch", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.sActor = sActor;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                if (actor.type == ActorType.MOB)
                {
                    var mh = (MobEventHandler)actor.e;
                    mh.AI.Master = sActor;
                }
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                if (actor.type == ActorType.MOB)
                {
                    var mh = (MobEventHandler)actor.e;
                    mh.AI.Master = null;
                }
            }
        }

        //#endregion
    }
}