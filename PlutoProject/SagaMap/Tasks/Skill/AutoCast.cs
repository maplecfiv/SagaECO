using System;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Packets.Client;
using SagaMap.Packets.Client.Skill;

namespace SagaMap.Tasks.Skill
{
    public class AutoCast : MultiRunTask
    {
        private readonly SkillArg arg;
        private readonly Actor caster;

        public AutoCast(Actor pc, SkillArg arg)
        {
            period = 600000;
            dueTime = 0;
            caster = pc;
            this.arg = arg;
        }

        public override void CallBack()
        {
            try
            {
                Deactivate();
                AutoCastInfo info = null;
                foreach (var i in arg.autoCast)
                {
                    info = i;
                    break;
                }

                if (info != null)
                {
                    arg.x = info.x;
                    arg.y = info.y;
                    arg.autoCast.Remove(info);
                    switch (caster.type)
                    {
                        case ActorType.PC:
                        {
                            var eh = (PCEventHandler)caster.e;
                            eh.Client.SkillDelay = DateTime.Now;
                            var p1 = new CSMG_SKILL_CAST();
                            p1.ActorID = arg.dActor;
                            p1.SkillID = (ushort)info.skillID;
                            p1.SkillLv = info.level;
                            p1.X = arg.x;
                            p1.Y = arg.y;
                            p1.Random = (short)Global.Random.Next();
                            eh.Client.OnSkillCast(p1, false, true);
                        }
                            break;
                        case ActorType.MOB:
                            var eh2 = (MobEventHandler)caster.e;
                            eh2.AI.CastSkill(info.skillID, info.level, arg.dActor,
                                Global.PosX8to16(arg.x, eh2.AI.map.Width), Global.PosY8to16(arg.y, eh2.AI.map.Height));
                            break;
                    }

                    dueTime = info.delay;
                }
                else
                {
                    caster.Tasks.Remove("AutoCast");
                    caster.Buff.CannotMove = false;
                    if (caster.type == ActorType.PC)
                    {
                        var eh = (PCEventHandler)caster.e;
                        eh.Client.map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, caster,
                            true);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }
        }
    }
}