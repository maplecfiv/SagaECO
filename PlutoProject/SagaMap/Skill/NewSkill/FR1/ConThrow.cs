using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FR1
{
    public class ConThrow : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            args.type = ATTACK_TYPE.BLOW;
            byte count = 0;
            switch (level)
            {
                case 1:
                    count = 2;
                    factor = 1.1f;
                    break;
                case 2:
                    count = 3;
                    factor = 1.1f;
                    break;
                case 3:
                    count = 4;
                    factor = 1.15f;
                    break;
                case 4:
                    count = 5;
                    factor = 1.15f;
                    break;
                case 5:
                    count = 6;
                    factor = 1.2f;
                    break;
                case 6:

                    factor = 1.2f;
                    break;
            }

            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.STAB;
            args.delayRate = 5f;
            var target = new List<Actor>();
            for (var i = 0; i < count; i++) target.Add(dActor);

            SkillHandler.Instance.PhysicalAttack(sActor, target, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}