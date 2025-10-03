using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Wizard_魔法师_
{
    /// <summary>
    ///     エナジーブラスト
    /// </summary>
    public class EnergyBlast : ISkill
    {
        private readonly bool MobUse;

        public EnergyBlast()
        {
            MobUse = false;
        }

        public EnergyBlast(bool MobUse)
        {
            this.MobUse = MobUse;
        }

        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            if (MobUse) level = 5;
            switch (level)
            {
                case 1:
                    factor = 1.9f;
                    break;
                case 2:
                    factor = 2.6f;
                    break;
                case 3:
                    factor = 3.4f;
                    break;
                case 4:
                    factor = 4.1f;
                    break;
                case 5:
                    factor = 4.8f;
                    break;
            }

            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            //取得有效Actor（即怪物）
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Neutral, factor / affected.Count);
        }

        //#endregion
    }
}