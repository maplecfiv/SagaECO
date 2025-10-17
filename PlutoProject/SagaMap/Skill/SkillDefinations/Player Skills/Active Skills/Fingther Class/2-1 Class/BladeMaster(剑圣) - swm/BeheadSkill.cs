using SagaDB.Actor;
using SagaDB.Mob;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_1_Class.BladeMaster_剑圣____swm {
    /// <summary>
    ///     斬擊系共通，會依怪物種類增加傷害
    /// </summary>
    public class BeheadSkill {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level, MobType mobType) {
            var factor = 2.1f;
            if (dActor is ActorMob) {
                var dActorMob = (ActorMob)dActor;
                if (dActorMob.BaseData.mobType == mobType)
                    //加成
                    factor = 4.1f;
            }

            factor = factor + 0.3f * level;
            if (sActor is ActorPC) {
                var pc = sActor as ActorPC;
                //不管是主职还是副职, 只要习得剑圣技能, 都会导致combo成立, 这里一步就行了
                if (pc.Skills3.ContainsKey(1117) || pc.DualJobSkills.Exists(x => x.ID == 1117))
                    //殺界
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2238);
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}