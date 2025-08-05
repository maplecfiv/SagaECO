using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Network.Client;

namespace SagaMap.Skill.SkillDefinations.Machinery
{
    /// <summary>
    ///     瞬間加速（ベイルアウト）
    /// </summary>
    public class RobotTeleport : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null) return -54; //需回傳"需裝備寵物"
            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT")) return 0;
            return -54; //需回傳"需裝備寵物"
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var pet = SkillHandler.Instance.GetPet(sActor);
            if (pet == null)
            {
                MapClient.FromActorPC((ActorPC)sActor).SendSystemMessage("当前的装备无法使用此技能");
                return;
            }

            if (SkillHandler.Instance.CheckMobType(pet, "MACHINE_RIDE_ROBOT"))
            {
                var map = MapManager.Instance.GetMap(sActor.MapID);
                map.TeleportActor(sActor, SagaLib.Global.PosX8to16(args.x, map.Width),
                    SagaLib.Global.PosY8to16(args.y, map.Height));
            }
        }

        #endregion
    }
}