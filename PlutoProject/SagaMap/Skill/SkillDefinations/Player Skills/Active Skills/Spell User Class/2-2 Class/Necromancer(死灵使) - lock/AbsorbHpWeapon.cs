using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     噬血（ライフテイク）
    /// </summary>
    public class AbsorbHpWeapon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(dActor.MapID);
            if (sActor.ActorID == dActor.ActorID)
            {
                var arg2 = new EffectArg();
                arg2.effectID = 5238;
                arg2.actorID = dActor.ActorID;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, dActor, true);
            }

            var skill = new BloodLeech(args.skill, dActor, 50000, 0.1f * level);
            if (sActor.Status.Additions.ContainsKey("SpLeech"))
            {
                var spadd = (SpLeech)sActor.Status.Additions["SpLeech"];
                spadd.rate = 0;
            }

            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}