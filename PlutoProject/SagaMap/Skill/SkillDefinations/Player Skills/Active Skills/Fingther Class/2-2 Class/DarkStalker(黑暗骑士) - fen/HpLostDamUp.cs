using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     血色战刃
    /// </summary>
    public class HpLostDamUp : ISkill
    {
        #region HpLostDamUpBuff

        public class HpLostDamUpBuff : DefaultBuff
        {
            public HpLostDamUpBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, int HPLost, int DamUp)
                : base(skill, actor, "HpLostDamUp", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this["DamUp"] = DamUp;
                this["HPLost"] = HPLost;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                actor.Buff.BloodyWeapon = true;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                actor.Buff.BloodyWeapon = false;
                MapManager.Instance.GetMap(actor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
            }
        }

        #endregion

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000 + 5000 * level;
            var HPLost = new[] { 0, 50, 100, 500 }[level];
            var DamUp = new[] { 0, 50, 100, 500 }[level];
            var skill = new HpLostDamUpBuff(args.skill, dActor, lifetime, HPLost, DamUp);
            SkillHandler.ApplyAddition(dActor, skill);
        }

        #endregion
    }
}