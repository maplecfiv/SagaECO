using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     燃燒的靈魂（ソウルバーン）
    /// </summary>
    public class SoulBurn : ISkill
    {
        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var mpdmgpcnt = 0.25f + 0.15f * level;
            var addMP = (uint)(dActor.MaxMP * mpdmgpcnt);
            if (dActor.MP <= addMP)
                addMP = dActor.MP;
            dActor.MP -= addMP;
            SkillHandler.Instance.ShowVessel(dActor, 0, (int)-addMP);
            MapManager.Instance.GetMap(sActor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor, true);
        }
    }
}