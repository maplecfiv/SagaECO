using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Harvest
{
    /// <summary>
    ///     槲寄生射击(ヤドリギショット)后段
    /// </summary>
    public class MistletoeShootingSEQ : ISkill
    {
        //未启用代码
        private readonly bool MobUse;

        public MistletoeShootingSEQ()
        {
            MobUse = false;
        }

        public MistletoeShootingSEQ(bool MobUse)
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
            //无法获取物理伤害值,暂时由普通治疗法术参数进行计算
            float[] factor = { 0, -0.6f, -1.48f, -3.96f, -5.355f, -9.28f };
            ;


            var map = MapManager.Instance.GetMap(dActor.MapID);
            var affected = map.GetActorsArea(dActor, 200, true);
            var realAffected = new List<Actor>();
            //ActorPC pc = (ActorPC)sActor;
            foreach (var act in affected)
                if (act.type == ActorType.PC || act.type == ActorType.PARTNER || act.type == ActorType.PET)
                    realAffected.Add(act);

            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                factor[level]);
        }

        #endregion
    }
}