using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.SoulTaker
{
    internal class DeathSickle : ISkill
    {
        private readonly bool MobUse;

        public DeathSickle()
        {
            MobUse = false;
        }

        public DeathSickle(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            var factor = 1.0f + 3.0f * level;
            var actorS = new ActorSkill(args.skill, sActor);
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(sActor, 300, false);
            var affected = new List<Actor>();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Dark, factor);
        }

        #endregion
    }
}