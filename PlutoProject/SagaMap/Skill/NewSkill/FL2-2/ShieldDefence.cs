using SagaDB.Actor;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.FL2_2
{
    public class ShieldDefence : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 0;
            switch (level)
            {
                case 1:
                    lifetime = 30000;
                    break;
                case 2:
                    lifetime = 35000;
                    break;
                case 3:
                    lifetime = 40000;
                    break;
                case 4:
                    lifetime = 45000;
                    break;
                case 5:
                    lifetime = 50000;
                    break;
            }

            var skill = new DefaultBuff(args.skill, dActor, "ShieldDefence", lifetime);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(dActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;

            var def = 0.2f + 0.05f * level;
            if (skill.Variable.ContainsKey("ShieldDefence"))
                skill.Variable.Remove("ShieldDefence");
            actor.Status.damage_atk1_discount += def;
            actor.Status.damage_atk2_discount += def;
            actor.Status.damage_atk3_discount += def;
            actor.Status.speed_skill -= (short)def * 200;
            //Manager.MapManager.Instance.GetMap(actor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            int level = skill.skill.Level;
            var def = 20 + 5 * level;
            actor.Status.damage_atk1_discount -= def;
            actor.Status.damage_atk2_discount -= def;
            actor.Status.damage_atk3_discount -= def;
            actor.Status.speed_skill += (short)def * 200;
            //Manager.MapManager.Instance.GetMap(actor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        //#endregion
    }
}