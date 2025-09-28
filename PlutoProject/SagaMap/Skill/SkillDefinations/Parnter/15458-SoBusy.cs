using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Parnter
{
    /// <summary>
    ///     ビビってなんか、いないんだから！(警戒模板)
    /// </summary>
    public class SoBusy : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.Status.Additions.ContainsKey("Warn"))
                return -1;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 60000;
            args.dActor = 0;
            var realdActor = sActor;
            var a = SagaLib.Global.Random.Next(1, 2);
            var pet = (ActorPartner)sActor;
            switch (a)
            {
                case 1:
                    realdActor = pet;
                    break;
                case 2:
                    realdActor = pet.Owner;
                    break;
            }

            var skill = new DefaultBuff(args.skill, realdActor, "Warn", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(realdActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            if (skill.Variable.ContainsKey("ST_LEFT_DEF"))
                skill.Variable.Remove("ST_LEFT_DEF");
            skill.Variable.Add("ST_LEFT_DEF", 9);
            actor.Status.def_skill += 9;
            actor.Buff.Warning = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.def_skill -= (short)skill.Variable["ST_LEFT_DEF"];

            actor.Buff.Warning = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }
    }
}