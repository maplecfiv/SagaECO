using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     想いの力・メイリー
    /// </summary>
    public class ThePowerOfLoveMayle : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 0;
            var map = MapManager.Instance.GetMap(dActor.MapID);
            if (sActor.type == ActorType.PARTNER)
            {
                lifetime = 12000;
                var affected = map.GetActorsArea(sActor, 500, false);
                var ActorlowHP = sActor;
                foreach (var act in affected)
                    if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    {
                        var a = SagaLib.Global.Random.Next(0, 99);
                        if (a < 40) dActor = act;
                    }
            }

            var skill = new DefaultBuff(args.skill, dActor, "MobKyrie", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            skill["MobKyrie"] = 12;
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            skill["MobKyrie"] = 0;
        }

        //#endregion
    }
}