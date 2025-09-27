using System.Collections.Generic;
using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    internal class MirageShotSEQ : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (sActor.type != ActorType.PC) level = 3;
            args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.STAB;
            var dest = new List<Actor>();
            float factor = 0;
            var countMax = 0;
            switch (level)
            {
                case 1:
                    factor = 1.7f;
                    countMax = 4;
                    //this.period = 1000;
                    break;
                case 2:
                    factor = 2.15f;
                    countMax = 6;
                    //this.period = 800;
                    break;
                case 3:
                    factor = 2.5f;
                    countMax = 10;
                    //this.period = 700;
                    break;
            }

            for (var i = 0; i < countMax; i++)
                dest.Add(dActor);
            args.delayRate = 4.5f;
            SkillHandler.Instance.PhysicalAttack(sActor, dest, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}