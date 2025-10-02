using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Blacksmith_铁匠____tat
{
    /// <summary>
    ///     緊急治療（ファーストエイド）
    /// </summary>
    public class FirstAid : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            uint itemID = 10048800; //急救用品
            var pc = sActor;
            if (SkillHandler.Instance.CountItem(pc, itemID) > 0)
            {
                SkillHandler.Instance.TakeItem(pc, itemID, 1);
                return 0;
            }

            return -2;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var hp_recovery = (uint)(dActor.MaxHP * (0.06f + 0.04f * level));
            if (dActor.HP + hp_recovery <= dActor.MaxHP)
            {
                if (!dActor.Buff.NoRegen)
                    dActor.HP += hp_recovery;
            }
            else
            {
                dActor.HP = dActor.MaxHP;
            }

            args.affectedActors.Add(dActor);
            args.Init();
            if (args.flag.Count > 0) args.flag[0] |= AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
        }

        //#endregion
    }
}