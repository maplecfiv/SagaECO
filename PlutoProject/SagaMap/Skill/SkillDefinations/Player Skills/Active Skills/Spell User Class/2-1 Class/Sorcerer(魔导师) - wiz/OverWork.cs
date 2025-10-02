using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     狂亂時間（オーバーワーク）
    /// </summary>
    public class OverWork : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            var skill = new DefaultBuff(args.skill, dActor, "OverWork", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
            if (dActor.ActorID == sActor.ActorID)
            {
                SkillHandler.ApplyAddition(dActor, skill);
                var arg2 = new EffectArg();
                arg2.effectID = 5170;
                arg2.actorID = dActor.ActorID;
                MapManager.Instance.GetMap(dActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, dActor, true);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var overwork = new[] { 0, 15, 20, 25, 30, 35 }[skill.skill.Level];
            if (skill.Variable.ContainsKey("OverWork"))
                skill.Variable.Remove("OverWork");
            skill.Variable.Add("OverWork", overwork);
            actor.Buff.OverWork = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("OverWork"))
                skill.Variable.Remove("OverWork");
            actor.Buff.OverWork = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}