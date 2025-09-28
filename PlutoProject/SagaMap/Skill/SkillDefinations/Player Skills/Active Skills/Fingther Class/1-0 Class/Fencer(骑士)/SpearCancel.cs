using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._1_0_Class.Fencer_骑士_
{
    /// <summary>
    ///     矛之達人 スピアディレイキャンセル
    /// </summary>
    public class SpearCancel : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (CheckPossible(pc))
                return 0;
            return -5;
        }

        private bool CheckPossible(Actor sActor)
        {
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                pc = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
                if (pc.Inventory.Equipments.ContainsKey(EnumEquipSlot.RIGHT_HAND))
                {
                    if (pc.Inventory.Equipments[EnumEquipSlot.RIGHT_HAND].BaseData.itemType == ItemType.SPEAR ||
                        pc.Inventory.GetContainer(ContainerType.RIGHT_HAND2).Count > 0)
                        return true;
                    return false;
                }

                return false;
            }

            return true;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0;
            Actor realdActor = SkillHandler.Instance.GetPossesionedActor((ActorPC)sActor);
            if (CheckPossible(realdActor))
            {
                var life = 20000;
                var skill = new DefaultBuff(args.skill, realdActor, "WeaponDC", life);
                skill.OnAdditionStart += StartEventHandler;
                skill.OnAdditionEnd += EndEventHandler;
                skill.OnCheckValid += ValidCheck;
                SkillHandler.ApplyAddition(realdActor, skill);
            }
        }

        private void ValidCheck(ActorPC pc, Actor dActor, out int result)
        {
            result = TryCast(pc, dActor, null);
        }

        private void StartEventHandler(Actor actor, DefaultBuff skill)
        {
            actor.Status.aspd_skill_perc += 1.25f + 0.25f * skill.skill.Level;

            actor.Buff.SpearDelayCancel = true;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        private void EndEventHandler(Actor actor, DefaultBuff skill)
        {
            var raspd_skill_perc_restore = 1.25f + 0.25f * skill.skill.Level;
            if (actor.Status.aspd_skill_perc > raspd_skill_perc_restore + 1)
                actor.Status.aspd_skill_perc -= raspd_skill_perc_restore;
            else
                actor.Status.aspd_skill_perc = 1;

            actor.Buff.SpearDelayCancel = false;
            MapManager.Instance.GetMap(actor.MapID)
                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, actor, true);
        }

        #endregion
    }
}