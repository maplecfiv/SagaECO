using SagaDB.Actor;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Passive_skills.Joint_Class.Gardener_庭园师_
{
    /// <summary>
    ///     ガーデニング
    /// </summary>
    public class Gardening : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var active = true;
            var skill = new DefaultPassiveSkill(args.skill, sActor, "Gardening", active);
            skill.OnAdditionStart += StartEventHandler;
            skill.OnAdditionEnd += EndEventHandler;
            SkillHandler.ApplyAddition(sActor, skill);
        }

        private void StartEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
            /*
             * ガーデニング †
                Passive
                習得JOBLV：25
                効果：飛空庭での植物育成が可能になる。
                飛空庭に設置した鉢に植えた植物に栄養剤を与え育てることができる。
                成長した植物からはアイテムを採取できる。
                鉢は５つまで設置可能
                他人の鉢は育成できない（タンスが開けないのと同じ）
                鉢に植えることができるのは品種改良種とガーデニングで収穫した苗
                与えられる栄養剤（植物栄養剤）は1日1本（0時更新）
                成熟後は栄養剤を与えられない
                成熟後はいつでも収穫可能
                栽培中の鉢は撤去すると消滅(再配置は可能)
                収穫すると鉢は消滅
                植えてから約28日で枯れ木になる
             */
        }

        private void EndEventHandler(Actor actor, DefaultPassiveSkill skill)
        {
        }

        #endregion
    }
}