using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    /// <summary>
    ///     各種抗性
    /// </summary>
    public class StatusRegi : ISkill
    {
        private readonly string StatusName;

        public StatusRegi(string StatusName)
        {
            this.StatusName = StatusName;
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 1200000 + 120000 * level;
            var skill = new DefaultBuff(args.skill, dActor, StatusName + "Regi", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            ChangeBuffIcon(actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            ChangeBuffIcon(actor, false);
        }

        private void ChangeBuffIcon(Actor actor, bool OnOff)
        {
            switch (StatusName)
            {
                case "Sleep":
                    actor.Buff.SleepResist = OnOff;
                    break;
                case "Poison":
                    actor.Buff.PoisonResist = OnOff;
                    break;
                case "Stun":
                    actor.Buff.FaintResist = OnOff;
                    break;
                case "Silence":
                    actor.Buff.SilenceResist = OnOff;
                    break;
                case "Stone":
                    actor.Buff.StoneResist = OnOff;
                    break;
                case "Confuse":
                    actor.Buff.ConfuseResist = OnOff;
                    break;
                case "鈍足":
                    actor.Buff.SpeedDownResist = OnOff;
                    break;
                case "Frosen":
                    actor.Buff.FrosenResist = OnOff;
                    break;
            }

            var map = MapManager.Instance.GetMap(actor.MapID);
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}