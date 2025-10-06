using SagaDB.Actor;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.BountyHunter_赏金猎人____swm
{
    /// <summary>
    ///     3段擊劍（スラッシュコンビネーション）
    /// </summary>
    public class PartsSlash : ISkill
    {
        private static readonly uint[] skills = { 2274, 2272, 2271, 2273 };

        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var times = SagaLib.Global.Random.Next(0, 3);
            MapManager.Instance.GetMap(sActor.MapID);
            int[] delay = { 0, 1000, 700 };
            var sActorPC = (ActorPC)sActor;
            for (var i = 0; i < times; i++)
            {
                var SkillID = skills[SagaLib.Global.Random.Next(0, skills.Length - 1)];
                if (sActorPC.Skills2.ContainsKey(SkillID))
                {
                    var info = SkillHandler.Instance.CreateAutoCastInfo(SkillID, sActorPC.Skills2[SkillID].Level,
                        delay[i]);
                    args.autoCast.Add(info);
                }
                else if (sActorPC.SkillsReserve.ContainsKey(SkillID))
                {
                    var info = SkillHandler.Instance.CreateAutoCastInfo(SkillID, sActorPC.SkillsReserve[SkillID].Level,
                        delay[i]);
                    args.autoCast.Add(info);
                }
            }
        }

        //#endregion

        ////#region Timer
        //private class Activator : MultiRunTask
        //{
        //    Actor sActor;
        //    Actor dActor;
        //    SkillArg skill;
        //    Map map;
        //    int state = 0;
        //    int times = 0;
        //    public Activator(Actor _sActor, Actor _dActor,int times, SkillArg _args, byte level)
        //    {
        //        sActor = _sActor;
        //        dActor = _dActor;
        //        this.times = times;
        //        skill = _args.Clone();
        //        this.DueTime = 0;
        //        this.Period = 1000;
        //        times = 3;
        //        map = Manager.MapManager.Instance.GetMap(sActor.MapID);
        //    }
        //    public override void CallBack()
        //    {
        //        //同步鎖，表示之後的代碼是執行緒安全的，也就是，不允許被第二個執行緒同時訪問
        //        ClientManager.EnterCriticalArea();
        //        try
        //        {
        //            this.Period = 700;
        //            ActorPC sActorPC = (ActorPC)sActor;
        //            if (times > 0)
        //            {
        //                uint SkillID = PartsSlash.skills[SagaLib.Global.Random.Next(0, PartsSlash.skills.Length - 1)];
        //                if (sActorPC.Skills.ContainsKey(SkillID))
        //                {
        //                    //skill.autoCast.Add(SkillID, sActorPC.Skills[SkillID].Level);
        //                }
        //                times--;
        //            }
        //            else
        //            {
        //                this.Deactivate();
        //            }
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