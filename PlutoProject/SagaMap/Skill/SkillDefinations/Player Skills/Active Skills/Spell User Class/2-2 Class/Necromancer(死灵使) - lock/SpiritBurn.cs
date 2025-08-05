using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Necromancer
{
    /// <summary>
    ///     燃燒的精神（スピリットバーン）
    /// </summary>
    public class SpiritBurn : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var spdmgpcnt = 0.25f + 0.15f * level;
            var addSP = (uint)(dActor.MaxSP * spdmgpcnt);
            if (dActor.SP <= addSP)
                addSP = dActor.SP;
            dActor.SP -= addSP;
            SkillHandler.Instance.ShowVessel(dActor, 0, 0, (int)-addSP);
            MapManager.Instance.GetMap(sActor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, null, sActor, true);
        }

        #endregion
    }
}