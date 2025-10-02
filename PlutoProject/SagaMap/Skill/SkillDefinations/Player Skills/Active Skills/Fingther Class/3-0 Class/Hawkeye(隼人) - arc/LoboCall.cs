using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Hawkeye_隼人____arc
{
    public class LoboCall : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var pc = (ActorPC)sActor;
            var Lobo = MapClient.FromActorPC(pc).map.SpawnMob(10130000, SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 2500, pc);
            Lobo.MaxHP = 1500 + 1500 * (uint)level;
            Lobo.HP = Lobo.MaxHP;
            Lobo.Status.min_atk1 = 550;
            Lobo.Status.min_atk2 = 550;
            Lobo.Status.min_atk3 = 550;
            Lobo.Status.max_atk1 = 650;
            Lobo.Status.max_atk2 = 650;
            Lobo.Status.max_atk3 = 650;
        }

        //#endregion
    }
}