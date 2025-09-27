using System;
using System.Collections.Generic;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Cardinal_大主教____vote
{
    /// <summary>
    ///     3436 救赎 (サルベイション)
    /// </summary>
    public class Salvation : ISkill
    {
        public void RemoveAddition(Actor actor, string additionName)
        {
            if (actor.Status.Additions.ContainsKey(additionName))
            {
                var addition = actor.Status.Additions[additionName];
                actor.Status.Additions.Remove(additionName);
                if (addition.Activated) addition.AdditionEnd();
                addition.Activated = false;
            }
        }

        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            if (dActor.type == ActorType.MOB)
            {
                var eh = (MobEventHandler)dActor.e;
                if (eh.AI.Mode.Symbol)
                    return -14;
            }

            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var skillbasefactor = new[] { -0f, -1.8f, -1.5f, -2.8f, -2.5f, -4.1f, -50f };
            var flasfactor = new[] { -0f, -0.4f, -0.5f, -0.6f, -0.7f, -0.8f, -0.9f, -1.0f, -1.1f, -1.3f, -1.5f, -50f };

            var factors = skillbasefactor[level];

            int[] basecurerate = { 0, 20, 25, 30, 35, 40, 100 };
            var flascurerate = new[] { 0, 10, 15, 20, 25, 30, 35, 40, 45, 50, 100, 100 };
            var rate = basecurerate[level];
            if (sActor.type == ActorType.PC)
            {
                var pc = (ActorPC)sActor;
                //不管是主职还是副职
                if (pc.Skills2_1.ContainsKey(3146) || pc.DualJobSkill.Exists(x => x.ID == 3146))
                {
                    //这里取副职的3146等级
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3146))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3146).Level;

                    //这里取主职的3146等级
                    var mainlv = 0;
                    if (pc.Skills2_1.ContainsKey(3146))
                        mainlv = pc.Skills2_1[3146].Level;

                    //这里取等级最高的剑圣等级用来做居合的倍率加成
                    var maxlv = Math.Max(duallv, mainlv);
                    factors += flasfactor[maxlv];
                    rate += flascurerate[maxlv];
                }
            }

            factors += sActor.Status.Cardinal_Rank;


            if (level % 2 == 0)
            {
                var map = MapManager.Instance.GetMap(sActor.MapID);
                List<Actor> actors;
                actors = map.GetActorsArea(dActor, 200, true);
                var affected = new List<Actor>();
                foreach (var i in actors)
                    if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                    {
                        if (i.type == ActorType.PC)
                        {
                            var pc = (ActorPC)i;
                            foreach (var i_p in pc.PossesionedActors)
                            {
                                if (!i_p.Online)
                                    continue;
                                if (i_p.Buff.NoRegen)
                                    continue;
                                switch (i_p.PossessionPosition)
                                {
                                    case PossessionPosition.NECK:
                                        affected.Add(i_p);
                                        break;
                                    case PossessionPosition.CHEST:
                                        affected.Add(i_p);
                                        break;
                                    case PossessionPosition.RIGHT_HAND:
                                        affected.Add(i_p);
                                        break;
                                    case PossessionPosition.LEFT_HAND:
                                        affected.Add(i_p);
                                        break;
                                }
                            }
                        }

                        affected.Add(i);
                    }


                SkillHandler.Instance.MagicAttack(sActor, affected, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                    factors);
                foreach (var item in affected)
                    if (SagaLib.Global.Random.Next(0, 99) < rate)
                    {
                        RemoveAddition(item, "Poison");
                        RemoveAddition(item, "MoveSpeedDown");
                        RemoveAddition(item, "Stone");
                        RemoveAddition(item, "Silence");
                        RemoveAddition(item, "Stun");
                        RemoveAddition(item, "Sleep");
                        RemoveAddition(item, "Frosen");
                        RemoveAddition(item, "Confuse");
                    }
            }
            else
            {
                var affected = new List<Actor>();
                if (!SkillHandler.Instance.CheckValidAttackTarget(sActor, dActor))
                {
                    if (dActor.type == ActorType.PC)
                    {
                        var list = new List<Actor>();
                        var pc = (ActorPC)dActor;
                        foreach (var i_p in pc.PossesionedActors)
                        {
                            if (!i_p.Online)
                                continue;
                            if (i_p.Buff.NoRegen)
                                continue;
                            switch (i_p.PossessionPosition)
                            {
                                case PossessionPosition.NECK:
                                    affected.Add(i_p);
                                    break;
                                case PossessionPosition.CHEST:
                                    affected.Add(i_p);
                                    break;
                                case PossessionPosition.RIGHT_HAND:
                                    affected.Add(i_p);
                                    break;
                                case PossessionPosition.LEFT_HAND:
                                    affected.Add(i_p);
                                    break;
                            }
                        }
                    }

                    affected.Add(dActor);
                }

                SkillHandler.Instance.MagicAttack(sActor, affected, args, SkillHandler.DefType.IgnoreAll, Elements.Holy,
                    factors);

                foreach (var item in affected)
                    if (SagaLib.Global.Random.Next(0, 99) < rate)
                    {
                        RemoveAddition(item, "Poison");
                        RemoveAddition(item, "MoveSpeedDown");
                        RemoveAddition(item, "Stone");
                        RemoveAddition(item, "Silence");
                        RemoveAddition(item, "Stun");
                        RemoveAddition(item, "Sleep");
                        RemoveAddition(item, "Frosen");
                        RemoveAddition(item, "Confuse");
                    }
            }
        }

        #endregion
    }
}