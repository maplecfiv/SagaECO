using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_1_Class.Alchemist_炼金术士____far
{
    /// <summary>
    ///     化工廠（ケミカルプラント）
    /// </summary>
    public class SumChemicalPlant : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(10580004, (short)(dActor.X + SagaLib.Global.Random.Next(1, 10)),
                (short)(dActor.Y + SagaLib.Global.Random.Next(1, 10)), 2500, sActor);
            mob.Status.max_atk1 = (ushort)(sActor.Status.max_atk1 * 0.16 * level);
            mob.Status.max_atk2 = (ushort)(sActor.Status.max_atk2 * 0.16 * level);
            mob.Status.max_atk3 = (ushort)(sActor.Status.max_atk3 * 0.16 * level);
            mob.Status.min_atk1 = (ushort)(sActor.Status.min_atk1 * 0.16 * level);
            mob.Status.min_atk2 = (ushort)(sActor.Status.min_atk2 * 0.16 * level);
            mob.Status.min_atk3 = (ushort)(sActor.Status.min_atk3 * 0.16 * level);
            mob.Status.hit_melee = (ushort)(sActor.Status.hit_melee * 0.16 * level);
            mob.Status.hit_ranged = (ushort)(sActor.Status.hit_ranged * 0.16 * level);
            sActor.Slave.Add(mob);
            var aci = new AutoCastInfo();
            aci.skillID = 3344; //化工廠[接續技能]
            aci.level = level;
            aci.delay = 0;
            args.autoCast.Add(aci);
        }

        //#endregion

        ////#region Timer
        //private class Activator : MultiRunTask
        //{
        //    Actor sActor;
        //    ActorMob mob;
        //    SkillArg skill;
        //    float factor;
        //    Map map;
        //    public Activator(Actor _sActor, ActorMob mob, SkillArg _args, byte level)
        //    {
        //        sActor = _sActor;
        //        this.mob = mob;
        //        skill = _args.Clone();
        //        factor = 0.5f + 1.5f * level;
        //        this.DueTime = 2500;
        //        this.Period = 0;

        //    }
        //    public override void CallBack()
        //    {
        //        //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
        //        ClientManager.EnterCriticalArea();
        //        try
        //        {

        //            this.Deactivate();
        //            map.DeleteActor(mob);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.ShowError(ex);
        //        }
        //        //解開同步鎖
        //        ClientManager.LeaveCriticalArea();
        //    }
        //}
        ////#endregion
    }
}