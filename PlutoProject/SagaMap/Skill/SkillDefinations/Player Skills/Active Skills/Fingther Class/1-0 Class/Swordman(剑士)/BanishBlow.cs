using SagaDB.Actor;
using SagaDB.Item;

namespace SagaMap.Skill.SkillDefinations.Swordman
{
    /// <summary>
    ///     雷霆突擊
    /// </summary>
    public class BanishBlow : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
                return 0;
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

                factor = 1.2f + 0.3f * level;
                if (sActor.type == ActorType.PC)
                {
                    var pc = (ActorPC)sActor;
                    if (pc.JobBasic == PC_JOB.TATARABE || pc.JobBasic == PC_JOB.FARMASIST ||
                        pc.JobBasic == PC_JOB.RANGER || pc.JobBasic == PC_JOB.MERCHANT) factor += 0.2f;
                }

                SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);

                //推开2格
                SkillHandler.Instance.PushBack(sActor, dActor, 2);
            }
        }

        #endregion
    }
}