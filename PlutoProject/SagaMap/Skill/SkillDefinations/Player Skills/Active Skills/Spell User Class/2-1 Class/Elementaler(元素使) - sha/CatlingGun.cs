using System.Collections.Generic;
using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Elementaler_元素使____sha
{
    internal class CatlingGun : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var actor = new ActorSkill(SkillFactory.Instance.GetSkill(7700, 1),
                sActor); //Register the substituted groove skill-actor.
            var map = MapManager.Instance.GetMap(sActor.MapID);
            actor.MapID = sActor.MapID;
            actor.X = dActor.X;
            actor.Y = dActor.Y;
            actor.e = new NullEventHandler();
            actor.Name =
                "NOT_SHOW_DISAPPEAR"; //Set a flag that marking not to show the dispperance information when groove disppear. 
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            var timer = new ActivatorA(actor, dActor, sActor, args, level);
            timer.Activate(); //Call ActivatorA.CallBack 500ms later.
        }

        #endregion
    }

    internal class ActivatorA : MultiRunTask
    {
        private readonly Actor AimActor;
        private readonly int countMax = 3;
        private readonly float factor = 1;
        private readonly Map map;
        private readonly Actor sActor;
        private readonly ActorSkill SkillBody;
        private readonly SkillArg SkillFireBolt = new SkillArg();
        private SkillArg Arg;
        private int count;

        public ActivatorA(ActorSkill actor, Actor dActor, Actor sActor, SkillArg args, byte level)
        {
            dueTime = 500;
            period = 1000;
            AimActor = dActor;
            Arg = args;
            SkillBody = actor;
            this.sActor = sActor;
            map = MapManager.Instance.GetMap(AimActor.MapID);
            var Me = (ActorPC)sActor; //Get the total skill level of skill with fire element.
            var Skill_Shaman = new List<int>();
            Skill_Shaman.Add(3006);
            Skill_Shaman.Add(3013);
            Skill_Shaman.Add(3009);
            Skill_Shaman.Add(3016);
            Skill_Shaman.Add(3011);
            Skill_Shaman.Add(3008);
            var TotalLv = 0;
            foreach (uint j in Skill_Shaman)
            {
                if (Me.Skills.ContainsKey(j))
                    TotalLv = TotalLv + Me.Skills[j].BaseData.lv;
                if (Me.Skills2.ContainsKey(j))
                    TotalLv = TotalLv + Me.Skills2[j].BaseData.lv;
            }

            if (TotalLv >= 5 && TotalLv >= 1)
                factor = 1f;
            else if (TotalLv >= 8 && TotalLv >= 6)
                factor = 1.5f;
            else if (TotalLv >= 11 && TotalLv >= 9)
                factor = 2.0f;
            else if (TotalLv >= 35 && TotalLv >= 12)
                factor = 2.5f;

            switch (level)
            {
                case 1:
                    factor *= 1.6f;
                    countMax = 4;
                    break;
                case 2:
                    factor *= 1.7f;
                    countMax = 5;
                    break;
                case 3:
                    factor *= 1.8f;
                    countMax = 5;
                    break;
                case 4:
                    factor *= 1.9f;
                    countMax = 6;
                    break;
                case 5:
                    factor *= 2.0f;
                    countMax = 7;
                    break;
            }
        }

        public override void CallBack()
        {
            //测试去除技能同步锁ClientManager.EnterCriticalArea();
            var DistanceA = Map.Distance(SkillBody, AimActor);
            if (count <= countMax)
            {
                if (DistanceA <= 600) //If mob is out the range that FireBolt can cast, skip out.
                {
                    SkillFireBolt.skill = SkillFactory.Instance.GetSkill(3009, 1);
                    SkillFireBolt.argType =
                        SkillArg.ArgType
                            .Active; //Configure the skillarg of firebolt, the caster is the skillactor of subsituted groove.
                    SkillFireBolt.sActor = SkillBody.ActorID;
                    SkillFireBolt.dActor = AimActor.ActorID;
                    SkillFireBolt.x = 255;
                    SkillFireBolt.y = 255;
                    SkillHandler.Instance.MagicAttack(sActor, AimActor, SkillFireBolt, Elements.Fire, factor);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, SkillFireBolt, SkillBody, true);
                    if (SkillFireBolt.flag.Contains(AttackFlag.DIE | AttackFlag.HP_DAMAGE |
                                                    AttackFlag.ATTACK_EFFECT)) //If mob died,terminate the proccess.
                    {
                        map.DeleteActor(SkillBody);
                        Deactivate();
                    }
                }

                count++;
            }
            else
            {
                map.DeleteActor(SkillBody);
                Deactivate();
            }
            //测试去除技能同步锁ClientManager.LeaveCriticalArea();
        }
    }
}