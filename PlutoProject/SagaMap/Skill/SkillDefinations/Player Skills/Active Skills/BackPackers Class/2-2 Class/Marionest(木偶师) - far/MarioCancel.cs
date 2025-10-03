using SagaDB.Actor;
using SagaMap.Network.Client;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far
{
    /// <summary>
    ///     釋放活動木偶（マリオネットディスチャージ）
    /// </summary>
    public class MarioCancel : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            uint PUPPET_SkillID = 2371;
            var factor = 1.7f + 0.3f * level;
            var sActorPC = (ActorPC)sActor;
            if (sActorPC.Skills2.ContainsKey(PUPPET_SkillID)) factor += 0.3f * sActorPC.Skills2[PUPPET_SkillID].Level;
            if (sActorPC.SkillsReserve.ContainsKey(PUPPET_SkillID))
                factor += 0.3f * sActorPC.SkillsReserve[PUPPET_SkillID].Level;

            SkillHandler.Instance.MagicAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            var rate = 30 + 10 * level;
            if (SagaLib.Global.Random.Next(0, 99) < rate)
                if (dActor.type == ActorType.PC)
                {
                    var dActorPC = (ActorPC)dActor;
                    if (dActorPC.Marionette != null) MapClient.FromActorPC(dActorPC).MarionetteDeactivate();
                }

            rate = 40 + 5 * level;
            if (dActor.type == ActorType.MOB)
                if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stiff, rate))
                {
                    var skill = new Stiff(args.skill, sActor, 4000 + 1000 * level);
                    SkillHandler.ApplyAddition(dActor, skill);
                }
        }

        //#endregion
    }
}