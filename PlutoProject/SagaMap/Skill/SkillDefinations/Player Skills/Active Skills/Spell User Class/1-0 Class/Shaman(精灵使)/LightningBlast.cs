using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Shaman_精灵使_
{
    public class LightningBlast : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            switch (level)
            {
                case 1:
                    factor = 1.1f;
                    break;
                case 2:
                    factor = 1.8f;
                    break;
                case 3:
                    factor = 2.5f;
                    break;
                case 4:
                    factor = 3.3f;
                    break;
                case 5:
                    factor = 4.0f;
                    break;
            }

            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            //取得有效Actor（即怪物）
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Wind, factor / affected.Count);
        }

        //#endregion
    }
}