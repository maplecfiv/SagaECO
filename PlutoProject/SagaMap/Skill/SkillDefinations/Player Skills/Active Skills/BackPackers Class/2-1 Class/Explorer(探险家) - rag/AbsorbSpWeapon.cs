using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Explorer
{
    /// <summary>
    ///     ブレイドマスタリー
    /// </summary>
    public class AbsorbSpWeapon : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, true);
            foreach (var act in affected)
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act) && !act.Buff.Dead)
                {
                    if (act.Status.Additions.ContainsKey("BloodLeech"))
                    {
                        var add = (BloodLeech)act.Status.Additions["BloodLeech"];
                        add.rate = 0;
                    }

                    var time = 30000 + 30000 * level; //SP吸收持续时间
                    var skill = new SpLeech(args.skill, act, time, 0.05f * level);

                    SkillHandler.ApplyAddition(act, skill);
                    var arg2 = new EffectArg();
                    arg2.effectID = 5238;
                    arg2.actorID = act.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, act, true);
                }
        }

        #endregion
    }
}