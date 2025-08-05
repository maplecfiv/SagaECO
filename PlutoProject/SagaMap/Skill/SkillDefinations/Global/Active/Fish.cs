using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Global.Active
{
    /// <summary>
    ///     钓鱼
    /// </summary>
    public class Fish : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (sActor.PossesionedActors.Count > 0)
            {
                MapClient.FromActorPC(sActor).SendSystemMessage("憑依時不能進行釣魚");
                return -13;
            }

            var map = MapManager.Instance.GetMap(sActor.MapID);
            if (map.Info.canfish[args.x, args.y] == 41000003 && map.ID == 10032000)
            {
                var baititem = sActor.EquipedBaitID;
                if (baititem != null) return 0;

                MapClient.FromActorPC(sActor).SendSystemMessage("沒有魚餌");
                return -13;
            }

            //SagaMap.Network.Client.MapClient.FromActorPC(sActor).SendSystemMessage("指定坐标x:" + args.x + "y:" + args.y);
            return -13;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 10000000;
            var skill1 = new Additions.Fish(args.skill, sActor, lifetime, 60000);
            SkillHandler.ApplyAddition(sActor, skill1);
        }

        #endregion
    }
}