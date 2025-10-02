using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     转生术（Ragnarok）
    /// </summary>
    public class TurnUndead : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var m = (ActorMob)dActor;
                if (m.Status.undead) return 0;

                return -14;
            }

            if (dActor.type == ActorType.PC)
            {
                var pci = (ActorPC)dActor;
                if (pci.Buff.Undead == false) return -14;
            }
            else
            {
                return 0;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor.type == ActorType.MOB)
            {
                var m = (ActorMob)dActor;
                if (m.Status.undead != true) return;
            }
            else if (dActor.type == ActorType.PC)
            {
                var pci = (ActorPC)dActor;
                if (pci.Buff.Undead != true) return;
            }

            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
            {
                var nums = SagaLib.Global.Random.Next(1, 100);
                if (nums <= 30)
                {
                    //float factor = 100.0f;
                    //SkillHandler.Instance.MagicAttack(sActor, dActor, args, SagaLib.Elements.Holy, factor);
                    var damageHP = dActor.HP;
                    SkillHandler.Instance.FixAttack(sActor, dActor, args, Elements.Holy, damageHP);
                }
                else
                {
                    var factor = 0.01f;
                    SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Holy, factor);
                }
            }
        }

        //#endregion
    }
}