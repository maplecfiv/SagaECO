using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.Additions.Global;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     反向信號（チャフ）
    /// </summary>
    public class RobotChaff : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
                //return 0;
                return -54; //封印
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 5000 + 5000 * level;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(sActor, 200, false);
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                {
                    var skill = new RobotChaffBuff(args.skill, act, lifetime);
                    SkillHandler.ApplyAddition(act, skill);
                }
        }

        public class RobotChaffBuff : DefaultBuff
        {
            public RobotChaffBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime)
                : base(skill, actor, "RobotChaffBuff", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                int level = skill.skill.Level;
                //近命中
                var hit_melee_add = -(int)(actor.Status.hit_melee * 0.1f * level);
                if (skill.Variable.ContainsKey("RobotChaff_hit_melee"))
                    skill.Variable.Remove("RobotChaff_hit_melee");
                skill.Variable.Add("RobotChaff_hit_melee", hit_melee_add);
                actor.Status.hit_melee_skill += (short)hit_melee_add;

                //遠命中
                var hit_ranged_add = -(int)(actor.Status.hit_ranged * 0.1f * level);
                if (skill.Variable.ContainsKey("RobotChaff_hit_ranged"))
                    skill.Variable.Remove("RobotChaff_hit_ranged");
                skill.Variable.Add("RobotChaff_hit_ranged", hit_ranged_add);
                actor.Status.hit_ranged_skill += (short)hit_ranged_add;
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                //近命中
                actor.Status.hit_melee_skill -= (short)skill.Variable["RobotChaff_hit_melee"];

                //遠命中
                actor.Status.hit_ranged_skill -= (short)skill.Variable["RobotChaff_hit_ranged"];
            }
        }

        #endregion
    }
}