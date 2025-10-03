using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco
{
    /// <summary>
    ///     上段切（アッパーカット）
    /// </summary>
    public class UpperCut : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factors = { 0f, 1.2f, 1.6f, 2.0f, 2.4f, 2.8f };
            uint MartialArtDamUp_SkillID = 125;
            var actorPC = (ActorPC)sActor;
            if (actorPC.Skills2.ContainsKey(MartialArtDamUp_SkillID))
                if (actorPC.Skills2[MartialArtDamUp_SkillID].Level == 3)
                {
                    factors[1] = 1.56f;
                    factors[2] = 2.08f;
                    factors[3] = 2.6f;
                    factors[4] = 3.12f;
                    factors[5] = 3.64f;
                }

            if (actorPC.SkillsReserve.ContainsKey(MartialArtDamUp_SkillID))
                if (actorPC.SkillsReserve[MartialArtDamUp_SkillID].Level == 3)
                {
                    factors[1] = 1.56f;
                    factors[2] = 2.08f;
                    factors[3] = 2.6f;
                    factors[4] = 3.12f;
                    factors[5] = 3.64f;
                }

            var factor = factors[level];
            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            SkillHandler.Instance.PushBack(sActor, dActor, 3);
            var rate = 15 + 5 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                var skill1 = new Stun(args.skill, dActor, 1500);
                SkillHandler.ApplyAddition(dActor, skill1);
            }
        }

        //#endregion
    }
}