using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.DarkStalker_黑暗骑士____fen
{
    /// <summary>
    ///     血液吸收（ブラッドアブソーブ）
    /// </summary>
    public class BloodAbsrd : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var factor = 2.25f + 0.75f * level;
            var actors = MapManager.Instance.GetMap(dActor.MapID).GetActorsArea(dActor, 100, true);
            var affected = new List<Actor>();
            //取得有效Actor（即怪物）
            var dmgheal = 0;
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                {
                    //affected.Add(i);
                    var dmg = SkillHandler.Instance.CalcDamage(false, sActor, dActor, args, SkillHandler.DefType.Def,
                        Elements.Neutral, 0, factor);
                    //SkillHandler.Instance.FixAttack(sActor, i, args, SagaLib.Elements.Neutral, dmg);
                    SkillHandler.Instance.CauseDamage(sActor, i, dmg);
                    SkillHandler.Instance.ShowVessel(i, dmg);
                    dmgheal -= dmg;
                }

            SkillHandler.Instance.CauseDamage(sActor, sActor, (int)(dmgheal * 0.3f));
            //SkillHandler.Instance.FixAttack(sActor, sActor, args, SagaLib.Elements.Neutral, dmgheal*0.3f);
            SkillHandler.Instance.ShowVessel(sActor, (int)(dmgheal * 0.3f));
            if (sActor.HP > sActor.MaxHP)
                sActor.HP = sActor.MaxHP;
        }

        #endregion
    }
}