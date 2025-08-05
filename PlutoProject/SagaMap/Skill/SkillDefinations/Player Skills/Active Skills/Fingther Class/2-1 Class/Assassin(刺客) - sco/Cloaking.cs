using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     潛行（クローキング）
    /// </summary>
    public class Cloaking : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 3600000;
            var skill = new DefaultBuff(args.skill, sActor, "Cloaking", lifetime);
            skill.OnAdditionStart += StartEvent;
            skill.OnAdditionEnd += EndEvent;
            skill.OnUpdate += TimerUpdate;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
            //隱藏自己
            actor.Buff.Transparent = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
            //顯示自己
            actor.Buff.Transparent = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void TimerUpdate(Actor actor, DefaultBuff skill)
        {
            if (actor.type != ActorType.PC) return;
            var dActorPC = (ActorPC)actor;


            if (actor.SP > 0 && dActorPC.Motion != MotionType.SIT)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                actor.SP -= 1;
                map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, actor, true);
            }
            else
            {
                //顯示自己
                skill.AdditionEnd();
            }
        }

        #endregion
    }
}