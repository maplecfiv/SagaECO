using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Sorcerer_魔导师____wiz
{
    /// <summary>
    ///     瞬間移動（テレポート）
    /// </summary>
    public class Teleport : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (pc.Status.Additions.ContainsKey("Teleport")) return -30;

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            map.TeleportActor(sActor, SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height));
            var skill = new DefaultBuff(args.skill, dActor, "Teleport", 5000);
            skill.OnAdditionStart += StartEvent;
            skill.OnAdditionEnd += EndEvent;
            SkillHandler.ApplyAddition(dActor, skill);
            var arg2 = new EffectArg();
            arg2.effectID = 4011;
            arg2.actorID = dActor.ActorID;
            map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, dActor, true);
        }

        private void StartEvent(Actor actor, DefaultBuff skill)
        {
        }

        private void EndEvent(Actor actor, DefaultBuff skill)
        {
        }

        #endregion
    }
}