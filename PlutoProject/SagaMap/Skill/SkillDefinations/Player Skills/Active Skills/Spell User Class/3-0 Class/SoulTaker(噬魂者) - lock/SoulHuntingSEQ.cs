using System;
using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.SoulTaker_噬魂者____lock
{
    /// <summary>
    ///     灵魂狩猎(ソウルハント)后段
    /// </summary>
    public class SoulHuntingSEQ : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }


        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float[] factor = { 0, 3.0f, 12.0f, 3.0f, 17.0f, 14.0f };
            var map = MapManager.Instance.GetMap(sActor.MapID);
            var affected = map.GetActorsArea(dActor, 100, true);
            var realAffected = new List<Actor>();
            foreach (var act in affected)
                if (SkillHandler.Instance.CheckValidAttackTarget(sActor, act))
                    realAffected.Add(act);

            if (!sActor.Status.Additions.ContainsKey("Sacrifice")) //献祭状态
            {
                var Healvalue = sActor.Status.max_matk + sActor.Status.max_atk1;
                float[] HPheal = { 0, 2.0f, 1.0f, 2.0f, 1.0f, 1.45f };
                float[] MPSPheal = { 0, 0.02f, 0.04f, 0.02f, 0.05f, 0.03f };
                uint showHP = 0, showSP = 0, ShowMP = 0;
                showHP = (uint)Math.Abs(Healvalue * HPheal[level]);
                showSP = (uint)Math.Abs(Healvalue * MPSPheal[level]);
                ShowMP = (uint)Math.Abs(Healvalue * MPSPheal[level]);
                sActor.HP += (uint)(Healvalue * HPheal[level]);
                if (sActor.HP <= 0) sActor.HP = 0;
                if (sActor.HP >= sActor.MaxHP) sActor.HP = sActor.MaxHP;
                if (sActor.SP <= 0) sActor.SP = 0;
                if (sActor.SP >= sActor.MaxSP) sActor.SP = sActor.MaxSP;
                if (sActor.MP <= 0) sActor.MP = 0;
                if (sActor.MP >= sActor.MaxMP) sActor.MP = sActor.MaxMP;
                sActor.SP += (uint)(Healvalue * MPSPheal[level]);
                sActor.MP += (uint)(Healvalue * MPSPheal[level]);
                SkillHandler.Instance.ShowVessel(sActor, -(int)showHP, -(int)ShowMP, -(int)showSP);
            }


            SkillHandler.Instance.MagicAttack(sActor, realAffected, args, Elements.Dark, factor[level]);
        }

        #endregion
    }
}