using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Elementaler
{
    internal class SpellCancel : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            PCEventHandler _e;
            MapClient myClinet;
            var myActor = (ActorPC)sActor;
            _e = (PCEventHandler)sActor.e;
            myClinet = _e.Client;
            myClinet.SendSkillDummy();
            if (myClinet.Character.Tasks.ContainsKey("SkillCast"))
                if (myClinet.Character.Tasks["SkillCast"].Activated)
                {
                    myClinet.Character.Tasks["SkillCast"].Deactivate();
                    myClinet.Character.Tasks.Remove("SkillCast");
                }
        }

        #endregion
    }
}