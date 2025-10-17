using System;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Fingther_Class._3_0_Class.Gladiator_剑斗士____swm {
    /// <summary>
    ///     神速斬り
    /// </summary>
    public class SpeedHit : ISkill {
        //#region ISkill 成員

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args) {
            if (sActor.Status.Additions.ContainsKey("SpeedHit"))
                return -30;
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level) {
            int[] lifetime = { 0, 1000, 1250, 1500, 1750, 2000 };
            var skill2 = new DefaultBuff(args.skill, sActor, "SpeedHit", lifetime[level]);
            SkillHandler.ApplyAddition(sActor, skill2);
            args.type = ATTACK_TYPE.SLASH;
            var factor = 5f + 3f * level;
            if (sActor is ActorPC) {
                var lv = 0;
                var pc = sActor as ActorPC;
                //不管是主职还是副职, 只要习得剑圣技能, 都会导致combo成立, 这里一步就行了
                if (pc.Skills3.ContainsKey(1117) || pc.DualJobSkills.Exists(x => x.ID == 1117)) {
                    //lv = pc.Skills3[1117].Level;
                    //这里取副职的剑圣等级
                    var duallv = 0;
                    if (pc.DualJobSkills.Exists(x => x.ID == 1117))
                        duallv = pc.DualJobSkills.FirstOrDefault(x => x.ID == 1117).Level;

                    //这里取主职的剑圣等级
                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(1117))
                        mainlv = pc.Skills3[1117].Level;

                    //这里取等级最高的剑圣等级用来做居合的倍率加成
                    factor += 0.5f * Math.Max(duallv, mainlv);
                    //factor += 0.5f * lv;

                    //ジリオンブレイド
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2534);
                    //一閃
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2400);
                    //居合
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2115);
                    //居合2
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2201);
                    //居合3
                    SkillHandler.Instance.SetNextComboSkill(sActor, 2202);
                }
            }

            SkillHandler.Instance.PhysicalAttack(sActor, dActor, args, sActor.WeaponElement, factor);

            var pos = new short[2];
            var map = MapManager.Instance.GetMap(sActor.MapID);
            pos[0] = dActor.X;
            pos[1] = dActor.Y;
            //map.MoveActor(Map.MOVE_TYPE.START, sActor, pos, 20000, 1000, true);

            if (SkillHandler.Instance.CanAdditionApply(sActor, dActor, SkillHandler.DefaultAdditions.Stun,
                    10 * level)) {
                var skill = new Stun(args.skill, dActor, 750 + 250 * level);
                SkillHandler.ApplyAddition(dActor, skill);
            }

            map.MoveActor(Map.MOVE_TYPE.START, sActor, pos, sActor.Dir, 20000, true, MoveType.BATTLE_MOTION);
        }

        //#endregion
    }
}