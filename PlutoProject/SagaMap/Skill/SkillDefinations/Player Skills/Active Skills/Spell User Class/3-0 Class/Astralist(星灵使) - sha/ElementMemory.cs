using System;
using System.Linq;
using SagaDB.Actor;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    internal class ElementMemory : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var actor = new ActorSkill(args.skill, dActor); //Register the substituted groove skill-actor.
            var map = MapManager.Instance.GetMap(dActor.MapID);
            actor.MapID = dActor.MapID;
            actor.X = dActor.X;
            actor.Y = dActor.Y;
            actor.e = new NullEventHandler();
            actor.Name =
                "ElementMemory"; //Set a flag that marking not to show the dispperance information when groove disppear. 
            map.RegisterActor(actor);
            actor.invisble = false;
            map.OnActorVisibilityChange(actor);
            var timer = new ActivatorB(actor, dActor, sActor, args, level);
            timer.Activate(); //Call ActivatorA.CallBack 500ms later.
        }

        #endregion
    }

    internal class ActivatorB : MultiRunTask
    {
        private readonly Actor AimActor;
        private readonly SkillArg Arg;
        private readonly int attacknumber = 3;
        private readonly float factor;
        private readonly Map map;
        private readonly Actor sActor;
        private readonly ActorSkill SkillBody;
        private int count;
        private SkillArg SkillFireBolt = new SkillArg();

        public ActivatorB(ActorSkill actor, Actor dActor, Actor sActor, SkillArg args, byte level)
        {
            dueTime = 100;
            period = 200;
            AimActor = dActor;
            Arg = args;
            SkillBody = actor;
            this.sActor = sActor;
            map = MapManager.Instance.GetMap(AimActor.MapID);
            var Me = (ActorPC)sActor; //Get the total skill level of skill with fire element.
            if (sActor.WeaponElement == Elements.Dark || sActor.WeaponElement == Elements.Holy)
                factor = 0.2f;
            else if (sActor.WeaponElement == Elements.Earth || sActor.WeaponElement == Elements.Water ||
                     sActor.WeaponElement == Elements.Fire || sActor.WeaponElement == Elements.Wind)
                factor = new[] { 0, 2.6f, 3.6f, 4.6f, 5.6f, 6.6f }[level];
            else
                factor = 0.1f;


            if (sActor is ActorPC)
            {
                var pc = sActor as ActorPC;
                if (pc.Skills3.ContainsKey(3409) || pc.DualJobSkill.Exists(x => x.ID == 3409))
                {
                    var duallv = 0;
                    if (pc.DualJobSkill.Exists(x => x.ID == 3409))
                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3409).Level;

                    var mainlv = 0;
                    if (pc.Skills3.ContainsKey(3409))
                        mainlv = pc.Skills3[3409].Level;

                    factor += 0.5f + 0.1f * Math.Max(duallv, mainlv);
                    var FA = new[] { 0, 1, 2, 3, 3, 4 }[Math.Max(duallv, mainlv)];
                    attacknumber += FA;
                }
            }
        }

        public override void CallBack()
        {
            //测试去除技能同步锁ClientManager.EnterCriticalArea();
            var DistanceA = Map.Distance(SkillBody, AimActor);
            //short[] Diss = new short[] { 400, 500, 600, 700, 800 };
            if (count <= attacknumber)
            {
                if (DistanceA <= 600) //If mob is out the range that FireBolt can cast, skip out.
                {
                    //if (count == attacknumber)
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3124, 1);
                    //else
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3001, 1);
                    //if (sActor.WeaponElement == SagaLib.Elements.Dark)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3085, 1);
                    //}
                    //else if(sActor.WeaponElement == SagaLib.Elements.Holy)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3078, 1);
                    //}
                    //else if (sActor.WeaponElement == SagaLib.Elements.Earth)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3044, 1);
                    //}
                    //else if (sActor.WeaponElement == SagaLib.Elements.Water)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3036, 1);
                    //}
                    //else if (sActor.WeaponElement == SagaLib.Elements.Fire)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3009, 1);
                    //}
                    //else if (sActor.WeaponElement == SagaLib.Elements.Wind)
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3017, 1);
                    //}
                    //else
                    //{
                    //    SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3001, 1);
                    //}


                    //SkillFireBolt.skill = SagaDB.Skill.SkillFactory.Instance.GetSkill(3433, SkillFireBolt.skill.Level);
                    //SkillFireBolt.argType = SkillArg.ArgType.Active;//Configure the skillarg of firebolt, the caster is the skillactor of subsituted groove.
                    //SkillFireBolt.sActor = SkillBody.ActorID;
                    //SkillFireBolt.dActor = AimActor.ActorID;
                    //SkillFireBolt.x = SagaLib.Global.PosX16to8(AimActor.X, Manager.MapManager.Instance.GetMap(AimActor.MapID).Width);
                    //SkillFireBolt.y = SagaLib.Global.PosY16to8(AimActor.Y, Manager.MapManager.Instance.GetMap(AimActor.MapID).Height);

                    SkillHandler.Instance.MagicAttack(sActor, AimActor, Arg, sActor.WeaponElement, factor);
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SKILL, Arg, SkillBody, true);
                    var arg2 = new EffectArg();
                    arg2.effectID = 4353;
                    arg2.actorID = AimActor.ActorID;
                    map.SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg2, AimActor, true);
                    if (Arg.flag.Contains(AttackFlag.DIE | AttackFlag.HP_DAMAGE |
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


//namespace SagaMap.Skill.SkillDefinations.Astralist
//{
//    /// <summary>
//    ///  百鬼哭（百鬼哭）
//    /// </summary>
//    public class ElementMemory : ISkill
//    {
//        #region ISkill Members
//        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
//        {
//            return 0;

//        }
//        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
//        {
//            //待完成
//            //ActorPC pc = (ActorPC)sActor;
//            //int elements = pc.Status.attackElements_item[pc.WeaponElement]
//            //                        + pc.Status.attackElements_skill[pc.WeaponElement]
//            //                        + pc.Status.attackelements_iris[pc.WeaponElement];
//            float factor = 0;
//            SkillArg skill;
//            skill = args.Clone();
//            //SkillArg SkillFireBolt = new SkillArg();
//            //ActorSkill actor=new ActorSkill(args.skill,dActor);

//            Map map = Manager.MapManager.Instance.GetMap(dActor.MapID);
//            if (sActor.WeaponElement == SagaLib.Elements.Dark || sActor.WeaponElement == SagaLib.Elements.Holy)
//            {
//                factor = 0.2f;
//            }
//            else if (sActor.WeaponElement == SagaLib.Elements.Earth || sActor.WeaponElement == SagaLib.Elements.Water || sActor.WeaponElement == SagaLib.Elements.Fire || sActor.WeaponElement == SagaLib.Elements.Wind)
//            {
//                factor = new float[] { 0, 2.6f, 3.6f, 4.6f, 5.6f, 6.6f }[level];
//            }
//            else
//            {
//                factor = 0.1f;
//            }


//            int attacknumber = 3;
//            if (sActor is ActorPC)
//            {
//                int lv = 0;
//                ActorPC pc = sActor as ActorPC;
//                if (pc.Skills3.ContainsKey(3409) || pc.DualJobSkill.Exists(x => x.ID == 3409))
//                {

//                    var duallv = 0;
//                    if (pc.DualJobSkill.Exists(x => x.ID == 3409))
//                        duallv = pc.DualJobSkill.FirstOrDefault(x => x.ID == 3409).Level;

//                    var mainlv = 0;
//                    if (pc.Skills3.ContainsKey(3409))
//                        mainlv = pc.Skills3[3409].Level;

//                    factor += (0.5f + 0.1f * Math.Max(duallv, mainlv));
//                    int FA = new int[] { 0, 1, 2, 3, 3, 4 }[Math.Max(duallv, mainlv)];
//                    attacknumber += FA;
//                }
//            }
//        }

//    }
//}