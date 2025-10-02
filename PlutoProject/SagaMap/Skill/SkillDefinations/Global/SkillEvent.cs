using System;
using System.Threading;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Global
{
    public class SkillEvent : ISkill
    {
        //#region ISkill Members

        public class Parameter
        {
            public SkillArg args;
            public Actor dActor;
            public byte level;
            public Actor sActor;
        }

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (MapClient.FromActorPC(pc).scriptThread != null)
                return -59;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            args.dActor = 0;
            args.showEffect = false;
            var para = new Parameter();
            para.sActor = sActor;
            para.dActor = dActor;
            para.args = args;
            para.level = level;
            MapClient.FromActorPC((ActorPC)sActor).scriptThread = new Thread(Run);
            MapClient.FromActorPC((ActorPC)sActor).scriptThread.Start(para);
        }

        private void Run(object par)
        {
            //ClientManager.EnterCriticalArea();
            try
            {
                RunScript((Parameter)par);
            }
            catch (Exception ex)
            {
                Logger.ShowError(ex);
            }

            //ClientManager.LeaveCriticalArea();
            MapClient.FromActorPC((ActorPC)((Parameter)par).sActor).scriptThread = null;
        }

        protected virtual void RunScript(Parameter para)
        {
        }

        //#endregion
    }
}