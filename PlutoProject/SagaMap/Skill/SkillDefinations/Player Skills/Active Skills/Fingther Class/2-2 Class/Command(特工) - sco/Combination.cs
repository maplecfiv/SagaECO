using System;
using System.Linq;
using SagaDB.Actor;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._2_2_Class.Command_特工____sco {
    /// <summary>
    ///     組合必殺（コンビネーション）
    /// </summary>
    public class Combination : ISkill {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            uint ASHIBARAI_SkillID = 2136, //扫堂腿
                UPPERCUT_SkillID = 2143, //筋斗踢
                TACKLE_SkillID = 2137; //过肩摔
            var pc = (ActorPC)sActor;
            args.argType = SkillArg.ArgType.Attack;
            //args.type = ATTACK_TYPE.SLASH;
            if (pc.Skills2_2.ContainsKey(ASHIBARAI_SkillID) ||
                pc.DualJobSkills.Exists(x => x.ID == ASHIBARAI_SkillID)) {
                var info = new AutoCastInfo();
                var duallv = 0;
                if (pc.DualJobSkills.Exists(x => x.ID == ASHIBARAI_SkillID))
                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == ASHIBARAI_SkillID).Level;

                var mainlv = 0;
                if (pc.Skills2_2.ContainsKey(ASHIBARAI_SkillID))
                    mainlv = pc.Skills2_2[ASHIBARAI_SkillID].Level;

                info.skillID = ASHIBARAI_SkillID;
                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 1000;
                args.autoCast.Add(info);
                //SkillHandler.Instance.SetNextComboSkill(sActor, UPPERCUT_SkillID);
            }

            if (pc.Skills.ContainsKey(UPPERCUT_SkillID) || pc.DualJobSkills.Exists(x => x.ID == UPPERCUT_SkillID)) {
                var duallv = 0;
                if (pc.DualJobSkills.Exists(x => x.ID == UPPERCUT_SkillID))
                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == UPPERCUT_SkillID).Level;

                var mainlv = 0;
                if (pc.Skills.ContainsKey(UPPERCUT_SkillID))
                    mainlv = pc.Skills[UPPERCUT_SkillID].Level;

                var info = new AutoCastInfo();
                info.skillID = UPPERCUT_SkillID;
                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 1000;
                args.autoCast.Add(info);
                //SkillHandler.Instance.SetNextComboSkill(sActor, TACKLE_SkillID);
            }

            if (pc.Skills2_2.ContainsKey(TACKLE_SkillID) ||
                (pc.DualJobSkills.Exists(x => x.ID == TACKLE_SkillID) && dActor.HP != 0)) {
                var duallv = 0;
                var info = new AutoCastInfo();
                info.skillID = TACKLE_SkillID;
                if (pc.DualJobSkills.Exists(x => x.ID == TACKLE_SkillID))
                    duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == TACKLE_SkillID).Level;

                var mainlv = 0;
                if (pc.Skills2_2.ContainsKey(TACKLE_SkillID))
                    mainlv = pc.Skills2_2[TACKLE_SkillID].Level;


                info.level = (byte)Math.Max(duallv, mainlv);
                info.delay = 1000;
                args.autoCast.Add(info);
            }
        }

        //#endregion
    }
}