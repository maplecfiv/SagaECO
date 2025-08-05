using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Event
{
    /// <summary>
    ///     う～ら～め～し～や～
    /// </summary>
    public class WeepingWillow2 : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            foreach (var act in affected)
                if (act.type == ActorType.PC || act.type == ActorType.PET || act.type == ActorType.SHADOW)
                {
                    var skill = new Confuse(args.skill, act, 3000);
                    SkillHandler.ApplyAddition(act, skill);
                }

            sActor.ClearTaskAddition();
            map.DeleteActor(sActor);
        }

        #endregion
    }
}