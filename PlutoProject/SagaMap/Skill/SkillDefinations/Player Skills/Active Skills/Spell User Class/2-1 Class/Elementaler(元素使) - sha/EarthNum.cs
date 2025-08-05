using SagaDB.Actor;
using SagaLib;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Elementaler
{
    /// <summary>
    ///     ファシライズ
    /// </summary>
    internal class EarthNum : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 0;
            var lifetime = 0;
            SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Earth, 0);
            args.flag[0] = AttackFlag.NONE;
            switch (level)
            {
                case 1:
                    rate = 20;
                    lifetime = 4000;
                    break;
                case 2:
                    rate = 30;
                    lifetime = 4500;
                    break;
                case 3:
                    rate = 40;
                    lifetime = 5000;
                    break;
                case 4:
                    rate = 55;
                    lifetime = 5500;
                    break;
                case 5:
                    rate = 70;
                    lifetime = 6000;
                    break;
            }

            var rateModify = 0F; //The higher value of elemet the higher rate of freezen possibility.
            var element_dActor = 0;
            element_dActor = dActor.Elements[Elements.Earth];
            if (element_dActor > 1 && element_dActor <= 100)
                rateModify = 0.25F;
            if (element_dActor > 100 && element_dActor <= 200)
                rateModify = 0.5F;
            if (element_dActor > 200 && element_dActor <= 300)
                rateModify = 0.75F;
            if (element_dActor > 300)
                rateModify = 0.9F;
            rate = (int)(100 - (100 - rate) * (1 - rateModify)); //If dActor attach water element, the rate increase. 
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill = new Stone(args.skill, dActor, lifetime);
                SkillHandler.ApplyAddition(dActor, skill);
            }
        }

        #endregion
    }
}