using System.Collections.Generic;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.ForceMaster_原力导师____wiz
{
    /// <summary>
    ///     デストラクショングレアー 后续
    /// </summary>
    public class DeathTractionGlareSEQ : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            //设置威力
            //float factor = 12.5f + 0.5f * level;
            var factor = 11.5f + 0.7f * level;
            //获取当前地图
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //获取设置中心3*3的怪物
            var actors = map.GetActorsArea(dActor, 200, true);
            var affected = new List<Actor>();
            args.affectedActors.Clear();
            foreach (var i in actors)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    affected.Add(i);
            //发送一个无属性AOE伤害
            SkillHandler.Instance.MagicAttack(sActor, affected, args, Elements.Neutral, factor);
        }
    }
}