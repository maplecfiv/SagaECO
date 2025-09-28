using System.Collections.Generic;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.SunFlowerAdditions
{
    /// <summary>
    ///     圣母之祈福(Ragnarok)
    /// </summary>
    public class Assumptio : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 500, false); //实测7*7范围内怪物互补情况太差,更改为11*11
            var realAffected = new List<Actor>();
            var ActorlowHP = sActor;
            //realAffected.Add(sActor);
            //foreach (Actor act in affected)
            //{
            //    if ((float)(act.HP / act.MaxHP) < (float)(ActorlowHP.HP / ActorlowHP.MaxHP) && !SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
            //    {
            //        realAffected.Add(act);
            //    }
            //}
            //int nums = realAffected.Count;
            //ActorlowHP = sActor;// realAffected[SagaLib.Global.Random.Next(1, nums) - 1];
            var lifetime = 10000;
            var skill = new DefaultBuff(args.skill, ActorlowHP, "Assumptio", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(ActorlowHP, skill);
            var arg = new EffectArg();
            arg.effectID = 5446;
            arg.actorID = ActorlowHP.ActorID;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, sActor, true);
            //SkillHandler.Instance.MagicAttack(sActor, ActorlowHP, args, SkillHandler.DefType.IgnoreAll, SagaLib.Elements.Holy, factor);
            //SkillHandler.Instance.FixAttack(sActor, ActorlowHP, args, SagaLib.Elements.Holy, factor);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.DefRateUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.DefRateUp = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}