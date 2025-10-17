using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaMap.Manager;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    internal class Fuenriru : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var actors = map.GetActorsArea(SagaLib.Global.PosX8to16(args.x, map.Width),
                SagaLib.Global.PosY8to16(args.y, map.Height), 200, null);
            var affected = new List<Actor>();
            switch (level)
            {
                case 1:
                    var a = new Random().Next(0, 100);
                    if (a <= 30)
                    {
                        var skill = new DefaultBuff(args.skill, dActor, "Pressure", 500000);
                        SkillHandler.ApplyAddition(sActor, skill);
                        SkillHandler.Instance.MagicAttack(sActor, dActor, args, Elements.Dark, 11.0f);
                    }

                    //foreach (Actor i in actors)
                    //{
                    //    if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    //        affected.Add(i);
                    //}
                    //SkillHandler.Instance.MagicAttack(sActor, affected, args, SagaLib.Elements.Earth, 11.0f);
                    break;
                case 2:
                    args = new SkillArg();
                    args.Init();
                    args.skill = SkillFactory.Instance.GetSkill(2066, 5);
                    var hpheal = (int)(sActor.MaxHP * (5.0f / 100.0f));
                    var mpheal = (int)(sActor.MaxMP * (2.0f / 100.0f));
                    var spheal = (int)(sActor.MaxSP * (2.0f / 100.0f));
                    if (sActor.Status.Additions.ContainsKey("Pressure"))
                    {
                        var mul = new Random().Next(1, 10);
                        hpheal = (int)Math.Min(sActor.MaxHP * (5.0f / 100.0f) * mul, sActor.MaxHP * (50.0f / 100.0f));
                        mpheal = (int)Math.Min(sActor.MaxMP * (2.0f / 100.0f) * mul, sActor.MaxMP * (25.0f / 100.0f));
                        spheal = (int)Math.Min(sActor.MaxSP * (2.0f / 100.0f) * mul, sActor.MaxSP * (25.0f / 100.0f));
                        SkillHandler.RemoveAddition(sActor, "Pressure");
                    }

                    args.hp.Add(hpheal);
                    args.mp.Add(mpheal);
                    args.sp.Add(spheal);
                    args.flag.Add(AttackFlag.HP_HEAL | AttackFlag.SP_HEAL | AttackFlag.MP_HEAL | AttackFlag.NO_DAMAGE);
                    sActor.HP += (uint)hpheal;
                    sActor.MP += (uint)mpheal;
                    sActor.SP += (uint)spheal;
                    if (sActor.HP > sActor.MaxHP)
                        sActor.HP = sActor.MaxHP;
                    if (sActor.MP > sActor.MaxMP)
                        sActor.MP = sActor.MaxMP;
                    if (sActor.SP > sActor.MaxSP)
                        sActor.SP = sActor.MaxSP;
                    SkillHandler.Instance.ShowEffectByActor(sActor, 4178);
                    SkillHandler.Instance.ShowVessel(sActor, -hpheal, -mpheal, -spheal);
                    MapManager.Instance.GetMap(sActor.MapID)
                        .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.HPMPSP_UPDATE, args, sActor, true);
                    break;
                case 3:
                    foreach (var i in actors)
                        if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                            affected.Add(i);
                    var factor = 11.0f;
                    if (sActor.Status.Additions.ContainsKey("Pressure"))
                    {
                        var mul = new Random().Next(0, 10);
                        factor += mul;
                        SkillHandler.RemoveAddition(sActor, "Pressure");
                    }

                    SkillHandler.Instance.PhysicalAttack(sActor, affected, args, Elements.Earth, factor);
                    break;
            }
        }

        //#endregion
    }
}