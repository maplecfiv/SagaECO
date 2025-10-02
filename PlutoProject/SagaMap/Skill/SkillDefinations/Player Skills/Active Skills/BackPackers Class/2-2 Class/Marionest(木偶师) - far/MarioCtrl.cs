using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.BackPackers_Class._2_2_Class.Marionest_木偶师____far
{
    /// <summary>
    ///     召喚活動木偶（マリオネット召喚）
    /// </summary>
    public class MarioCtrl : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 25000;
            uint[] MobID =
            {
                0, 26320000 //寵物泰迪
                ,
                26280000 //寵物皮諾
                ,
                26290000 //寵物愛伊斯
                ,
                26300000 //寵物塔依
                ,
                26310000 //寵物虎姆拉
            };
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var mob = map.SpawnMob(MobID[level], SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 2500, sActor);
            var skill = new MarioCtrlBuff(args.skill, sActor, lifetime, mob);
            SkillHandler.ApplyAddition(sActor, skill);
            if (!sActor.Status.Additions.ContainsKey("MarioCtrlMove"))
            {
                var cm = new CannotMove(args.skill, sActor, lifetime);
                SkillHandler.ApplyAddition(sActor, cm);
            }
        }

        public class MarioCtrlBuff : DefaultBuff
        {
            private readonly ActorMob mob;

            public MarioCtrlBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, ActorMob mob)
                : base(skill, actor, "MarioCtrl", lifetime)
            {
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
                this.mob = mob;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                mob.ClearTaskAddition();
                map.DeleteActor(mob);
            }
        }

        //#endregion
    }
}