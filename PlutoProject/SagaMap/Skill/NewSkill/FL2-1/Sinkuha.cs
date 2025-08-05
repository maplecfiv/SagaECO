using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Item;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_1
{
    public class Sinkuha : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
            {
                if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

                return -14;
            }

            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND) ||
                    pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                    return true;
                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            if (CheckPossible(sActor))
            {
                args.type = ATTACK_TYPE.BLOW;
                //args.argType = SkillArg.ArgType.Attack;
                factor = 1.0f + 0.2f * level;
                var actors = new List<Actor>();

                if (level == 6)
                    for (var i = 0; i < 10; i++)
                        actors.Add(dActor);
                else
                    actors.Add(dActor);
                SkillHandler.Instance.PhysicalAttack(sActor, actors, args, sActor.WeaponElement, factor);
            }
        }

        #endregion
    }
}