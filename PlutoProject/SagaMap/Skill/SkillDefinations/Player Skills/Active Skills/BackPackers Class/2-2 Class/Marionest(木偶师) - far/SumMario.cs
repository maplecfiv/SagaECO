using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far
{
    /// <summary>
    ///     召喚活動木偶皇帝
    /// </summary>
    public class SumMario : ISkill
    {
        private readonly uint MobID;
        private readonly uint NextSkillID;

        public SumMario(uint MobID, uint NextSkillID)
        {
            this.MobID = MobID;
            this.NextSkillID = NextSkillID;
        }

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //招換
            var mob = map.SpawnMob(MobID, (short)(sActor.X + SagaLib.Global.Random.Next(1, 10))
                , (short)(sActor.Y + SagaLib.Global.Random.Next(1, 10))
                , 50, sActor);
            sActor.Slave.Add(mob);
            args.autoCast.Add(SkillHandler.Instance.CreateAutoCastInfo(NextSkillID, level, 500));
        }

        //#endregion
    }
}