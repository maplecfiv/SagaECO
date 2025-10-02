using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote
{
    /// <summary>
    ///     リカバリー
    /// </summary>
    internal class Recovery : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rank = 0.3f + 0.1f * level;
            var heal = (uint)(dActor.MaxHP * rank);
            if (dActor.Buff.NoRegen)
                return;
            dActor.HP += heal;
            if (dActor.HP > dActor.MaxHP)
                dActor.HP = dActor.MaxHP;
            args.affectedActors.Add(dActor);
            args.Init();
            if (args.flag.Count > 0)
            {
                args.flag[0] |= AttackFlag.HP_HEAL | AttackFlag.NO_DAMAGE;
                args.hp[0] = (int)heal;
            }

            var map = MapManager.Instance.GetMap(dActor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, dActor, true);
        }

        //#endregion
    }
}