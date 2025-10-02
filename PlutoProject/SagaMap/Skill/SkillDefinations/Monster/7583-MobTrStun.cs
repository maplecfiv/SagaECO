using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Monster
{
    /// <summary>
    ///     為憑依者的昏迷
    /// </summary>
    public class MobTrStun : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 99;
            var lifetime = 5000; //30秒昏迷!?
            if (dActor.type == ActorType.PC)
            {
                var pc = (ActorPC)dActor;
                foreach (Actor act in pc.PossesionedActors)
                    if (SkillHandler.Instance.CanAdditionApply(sActor, act, SkillHandler.DefaultAdditions.Stun, rate))
                    {
                        var skill = new Stun(args.skill, act, lifetime);
                        SkillHandler.ApplyAddition(act, skill);
                    }
            }
        }

        //#endregion
    }
}