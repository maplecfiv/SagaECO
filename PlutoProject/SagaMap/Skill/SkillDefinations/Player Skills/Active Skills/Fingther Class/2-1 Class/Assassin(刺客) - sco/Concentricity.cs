using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.Assassin_刺客____sco
{
    /// <summary>
    ///     アサルト
    /// </summary>
    public class Concentricity : ISkill
    {
        private readonly bool MobUse;

        public Concentricity()
        {
            MobUse = false;
        }

        public Concentricity(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        private bool CheckPossible(Actor sActor)
        {
            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            var lifetime = 15000 + 15000 * level;
            if (sActor.type == ActorType.PC)
            {
                args.dActor = 0;
                Actor realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                if (CheckPossible(realdActor))
                {
                    var skill = new DefaultBuff(args.skill, realdActor, "Concentricity", lifetime);
                    skill.OnAdditionStart += StartEventHandler;
                    skill.OnAdditionEnd += EndEventHandler;
                    SkillHandler.ApplyAddition(realdActor, skill);
                }
            }
            else
            {
                var skill = new DefaultBuff(args.skill, sActor, "Concentricity", lifetime);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                SkillHandler.ApplyAddition(sActor, skill);
            }
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            var def_rate = (short)(actor.Status.def * 0.3f);
            if (skill.Variable.ContainsKey("Concentricity"))
                skill.Variable.Remove("Concentricity");
            skill.Variable.Add("Concentricity", def_rate);
            actor.Status.def_skill -= def_rate;

            actor.Status.cri_skill += 30;
            actor.Buff.DefDown = true;
            actor.Buff.CriticalRateUp = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Buff.DefDown = false;
            actor.Buff.CriticalRateUp = false;

            actor.Status.cri_skill -= 30;
            actor.Status.def_skill += (short)skill.Variable["Concentricity"];

            if (skill.Variable.ContainsKey("Concentricity"))
                skill.Variable.Remove("Concentricity");

            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}