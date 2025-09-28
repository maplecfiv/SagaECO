using System;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Skill;

namespace SagaMap.Partner
{
    public class PartnerAttack : MultiRunTask
    {
        private readonly PartnerAI partnerai;
        public Actor dActor;

        public PartnerAttack(PartnerAI partnerai, Actor dActor)
        {
            Name = "搭档普攻线程";
            dueTime = 0;
            this.partnerai = partnerai;
            period = calcDelay(partnerai.Partner);
            this.dActor = dActor;
        }

        private int calcDelay(Actor actor)
        {
            var aspd = 0;
            uint delay = 0;
            var partner = (ActorPartner)partnerai.Partner;
            aspd = partner.Status.aspd;
            delay = 2000 - (uint)(2000 * aspd * 0.001f);
            return (int)delay;
        }

        public override void CallBack()
        {
            try
            {
                if (!partnerai.CanAttack)
                    return;
                if (dActor == null || partnerai.Partner == null)
                {
                    if (Activated) Deactivate();
                    return;
                }

                if (partnerai.Partner.HP == 0 || dActor.HP == 0 || partnerai.Partner.Tasks.ContainsKey("AutoCast"))
                {
                    if (partnerai.Hate.ContainsKey(dActor.ActorID)) partnerai.Hate.Remove(dActor.ActorID);
                    if (Activated) Deactivate();
                    return;
                }

                var partner = (ActorPartner)partnerai.Partner;
                if (DateTime.Now < partner.TTime["攻击僵直"]) return;
                if (partner.Owner.ActorID == dActor.ActorID)
                {
                    if (Activated) Deactivate();
                    return;
                }

                if (partnerai.Master != null)
                {
                    if (dActor.ActorID == partnerai.Master.ActorID)
                        return;
                    if (dActor.type == ActorType.MOB)
                        if (((MobEventHandler)dActor.e).AI.Master != null)
                            if (((MobEventHandler)dActor.e).AI.Master.ActorID == partnerai.Master.ActorID)
                                return;
                }

                //if (dActor.type == ActorType.PC)
                //{
                //    ActorPC pc = (ActorPC)dActor;
                //    if (pc.HP == 0)
                //    {
                //        if (partnerai.Hate.ContainsKey(dActor.ActorID)) partnerai.Hate.Remove(dActor.ActorID);
                //        if (this.Activated) this.Deactivate();
                //        return;
                //    }
                //}
                //目前对于parter的几个排除攻击对象进行处理
                if (dActor.type == ActorType.PC)
                {
                    if (Activated) Deactivate();
                    return;
                }

                if (dActor.type == ActorType.PARTNER)
                {
                    if (Activated) Deactivate();
                    return;
                }

                if (dActor.type == ActorType.PET)
                {
                    if (Activated) Deactivate();
                    return;
                }

                var arg = new SkillArg();
                SkillHandler.Instance.Attack(partnerai.Partner, dActor, arg);
                partnerai.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.ATTACK, arg, partnerai.Partner, true);
                period = calcDelay(partnerai.Partner);
                partner.TTime["攻击僵直"] = DateTime.Now + new TimeSpan(0, 0, 0, 0, period - 500);
            }
            catch (Exception ex)
            {
                Deactivate();
                Logger.ShowError(ex);
            }
        }
    }
}