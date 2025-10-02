using SagaDB.Actor;
using SagaMap.Skill.Additions;
using SagaMap.Skill.SkillDefinations.Global.Active;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._1_0_Class.Ranger_冒险家_
{
    /// <summary>
    ///     濕滑陷阱（スリップトラップ）
    /// </summary>
    public class CswarSleep : Trap
    {
        private readonly bool MobUse;

        public CswarSleep()
            : base(false, PosType.sActor)
        {
            MobUse = false;
        }

        public CswarSleep(bool MobUse)
            : base(false, PosType.sActor)
        {
            this.MobUse = MobUse;
        }

        public override void BeforeProc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            LifeTime = 2000 + 1000 * level;
            uint[] Ranges = { 0, 100, 200, 200, 300, 300 };
            Range = Ranges[level];
        }

        public override void ProcSkill(Actor sActor, Actor mActor, ActorSkill actor, SkillArg args, Map map, int level,
            float factor)
        {
            if (MobUse) level = 5;
            int[] lifetimes = { 0, 3000, 4000, 5000, 5000, 6000 };
            var push = new[] { 0, 3, 4, 4, 5, 5 }[level];
            //int rate = 20 + 10 * level;
            var rate = 100;
            var lifetime = lifetimes[level];
            //if (!mActor.Status.Additions.ContainsKey("Sleep"))
            //{
            //    if (SkillHandler.Instance.CanAdditionApply(sActor, mActor, SkillHandler.DefaultAdditions.Sleep , rate))
            //    {
            //        MoveSpeedDown sk = new MoveSpeedDown(args.skill, mActor, lifetime);
            //        SkillHandler.ApplyAddition(mActor, sk);

            //    }
            //}
            if (!mActor.Status.Additions.ContainsKey("Stiff"))
                if (SkillHandler.Instance.CanAdditionApply(sActor, mActor, SkillHandler.DefaultAdditions.Stiff, rate))
                {
                    var sk2 = new Stiff(args.skill, mActor, lifetime);
                    SkillHandler.ApplyAddition(mActor, sk2);
                    SkillHandler.Instance.PushBack(sActor, mActor, push);
                }
        }
    }
}