using SagaDB.Actor;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._1_0_Class.Votes_祭司_
{
    /// <summary>
    ///     复活术
    /// </summary>
    public class Resurrection : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            if (dActor != null)
                if (dActor.type == ActorType.PC)
                {
                    var pc = (ActorPC)dActor;
                    if (pc.Online)
                        if (pc.Buff.Dead)
                        {
                            pc.Buff.TurningPurple = true;
                            MapClient.FromActorPC(pc).Map
                                .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.BUFF_CHANGE, null, pc, true);
                            pc.TInt["Revive"] = level;

                            if (dActor.type == ActorType.PC)
                                MapClient.FromActorPC((ActorPC)dActor)
                                    .SendSystemMessage(string.Format("玩家 {0} 正在使你复活", sActor.Name));
                            pc.TStr["复活者"] = sActor.Name;

                            MapClient.FromActorPC(pc).EventActivate(0xF1000000);
                        }
                }
        }

        #endregion
    }
}