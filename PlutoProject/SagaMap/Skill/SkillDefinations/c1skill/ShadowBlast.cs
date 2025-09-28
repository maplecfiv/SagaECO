using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.c1skill
{
    public class ShadowBlast : ISkill
    {
        #region ISkill Members

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
                    factor = 2.0f;
                    break;
                case 2:
                    factor = 2.1f;
                    break;
                case 3:
                    factor = 2.4f;
                    break;
                case 4:
                    factor = 2.7f;
                    break;
                case 5:
                    factor = 3.0f;
                    break;
            }

            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            //取得有效Actor（即怪物）
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    affected.Add(i);
                    if (dActor.Darks == 1)
                    {
                        MapManager.Instance.GetMap(sActor.MapID).SendEffect(dActor, 5202);
                        var add = new SkillArg();
                        add.argType = SkillArg.ArgType.Actor_Active;
                        add.skill = args.skill;
                        SkillHandler.Instance.MagicAttack(sActor, i, add, Elements.Dark, 1.1f + 0.1f * level);
                    }
                }

            dActor.Darks = 0;
            SkillHandler.Instance.MagicAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        #endregion
    }
}