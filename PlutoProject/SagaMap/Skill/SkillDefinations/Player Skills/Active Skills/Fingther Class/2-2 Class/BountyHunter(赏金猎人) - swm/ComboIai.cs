using System;
using System.Linq;
using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.
    BountyHunter_赏金猎人____swm {
    /// <summary>
    ///     連續居合斬（連続居合い斬り）
    /// </summary>
    public class ComboIai : ISkill {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            if (SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor)) return 0;

            return -14;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            //建立設置型技能實體
            //ActorSkill actor = new ActorSkill(args.skill, sActor);
            //Map map = Manager.MapManager.Instance.GetMap(sActor.MapID);
            ////設定技能位置
            //actor.MapID = dActor.MapID;
            //actor.X = SagaLib.Global.PosX8to16(args.x, map.Width);
            //actor.Y = SagaLib.Global.PosY8to16(args.y, map.Height);
            ////設定技能的事件處理器，由於技能體不需要得到消息廣播，因此建立空處理器
            //actor.e = new ActorEventHandlers.NullEventHandler();
            ////在指定地圖註冊技能Actor
            //map.RegisterActor(actor);
            ////設置Actor隱身屬性為False
            //actor.invisble = false;
            ////廣播隱身屬性改變事件，以便讓玩家看到技能實體
            //map.OnActorVisibilityChange(actor);
            //建立技能效果處理物件
            SkillHandler.Instance.SetNextComboSkill(sActor, 2115);
            uint Iai_SkillID = 2115;
            var sActorPC = (ActorPC)sActor;
            //args.argType = SkillArg.ArgType.Attack;
            args.type = ATTACK_TYPE.SLASH;
            if (sActorPC.Skills.ContainsKey(Iai_SkillID) || sActorPC.DualJobSkills.Exists(x => x.ID == Iai_SkillID)) {
                var info = new AutoCastInfo();
                info.skillID = Iai_SkillID;

                var duallv = 0;
                if (sActorPC.DualJobSkills.Exists(x => x.ID == Iai_SkillID))
                    duallv = sActorPC.DualJobSkills.FirstOrDefault(x => x.ID == Iai_SkillID).Level;

                var mainlv = 0;
                if (sActorPC.Skills.ContainsKey(Iai_SkillID))
                    mainlv = sActorPC.Skills[Iai_SkillID].Level;

                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 300;
                args.autoCast.Add(info);
                //SkillHandler.Instance.SetNextComboSkill(sActor, 2115);//取消吟唱

                //    Activator timer = new Activator(sActor, dActor, args, level);
                //    timer.Activate();
            }

            uint Iai2_SkillID = 2201;
            if (sActorPC.Skills.ContainsKey(Iai2_SkillID) || sActorPC.DualJobSkills.Exists(x => x.ID == Iai2_SkillID)) {
                var info = new AutoCastInfo();
                info.skillID = Iai2_SkillID;
                var duallv = 0;
                if (sActorPC.DualJobSkills.Exists(x => x.ID == Iai2_SkillID))
                    duallv = sActorPC.DualJobSkills.FirstOrDefault(x => x.ID == Iai2_SkillID).Level;

                var mainlv = 0;
                if (sActorPC.Skills.ContainsKey(Iai2_SkillID))
                    mainlv = sActorPC.Skills[Iai2_SkillID].Level;

                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 1000;
                args.autoCast.Add(info);
            }

            uint Iai3_SkillID = 2202;
            if (sActorPC.Skills.ContainsKey(Iai3_SkillID) || sActorPC.DualJobSkills.Exists(x => x.ID == Iai3_SkillID)) {
                var info = new AutoCastInfo();
                info.skillID = Iai3_SkillID;
                var duallv = 0;
                if (sActorPC.DualJobSkills.Exists(x => x.ID == Iai3_SkillID))
                    duallv = sActorPC.DualJobSkills.FirstOrDefault(x => x.ID == Iai3_SkillID).Level;

                var mainlv = 0;
                if (sActorPC.Skills.ContainsKey(Iai3_SkillID))
                    mainlv = sActorPC.Skills[Iai3_SkillID].Level;

                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 1000;
                args.autoCast.Add(info);
            }
        }

        //#endregion
    }
}