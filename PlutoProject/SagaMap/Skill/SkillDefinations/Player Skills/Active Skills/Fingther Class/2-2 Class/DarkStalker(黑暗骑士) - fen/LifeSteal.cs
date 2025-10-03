using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     迷魂吸血（ライフスティール）
    /// </summary>
    public class LifeSteal : ISkill
    {
        private readonly bool MobUse;

        public LifeSteal()
        {
            MobUse = false;
        }

        public LifeSteal(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(pc, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (MobUse) level = 5;
            var factor = 1.0f + 0.2f * level;
            int elements;
            //args.type = ATTACK_TYPE.BLOW;
            if (sActor.WeaponElement != Elements.Neutral)
                elements = sActor.Status.attackElements_item[sActor.WeaponElement]
                           + sActor.Status.attackElements_skill[sActor.WeaponElement]
                           + sActor.Status.attackelements_iris[sActor.WeaponElement];
            else
                elements = 0;

            var dmg = SkillHandler.Instance.CalcDamage(true, sActor, dActor, args, SkillHandler.DefType.Def,
                sActor.WeaponElement, elements, factor);
            SkillHandler.Instance.CauseDamage(sActor, dActor, dmg);
            SkillHandler.Instance.ShowVessel(dActor, dmg);
            //SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
            //uint hp_recovery=0;
            //foreach (int hp in args.hp)
            //{
            //    hp_recovery += (uint)(hp * 0.8f);
            //}
            var dmgheal = (int)-(dmg * 0.8f);
            SkillHandler.Instance.CauseDamage(sActor, sActor, dmgheal);
            SkillHandler.Instance.ShowVessel(sActor, dmgheal);
        }

        //#endregion
    }
}