using SagaDB.Actor;
using SagaDB.Skill;
using SagaLib;
using SagaLib.Tasks;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._3_0_Class.Astralist_星灵使____sha
{
    internal class ElementGun : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var actor = new ActorSkill(SkillFactory.Instance.GetSkill(9247, 1),
                sActor); //Register the substituted groove skill-actor.
            var map = MapManager.Instance.GetMap(sActor.MapID);
            actor.MapID = sActor.MapID;
            actor.X = dActor.X;
            actor.Y = dActor.Y;
            actor.e = new NullEventHandler();
            actor.Name =
                "ElementGun"; //Set a flag that marking not to show the dispperance information when groove disppear. 
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

            switch (level)
            {
                case 1:
                    factor = 3.0f;
                    countMax = 4;
                    period = 1000;
                    break;
                case 2:
                    factor = 3.8f;
                    countMax = 4;
                    period = 800;
                    break;
                case 3:
                    factor = 4.35f;
                    countMax = 4;
                    period = 700;
                    break;
                case 4:
                    factor = 4.56f;
                    period = 500;
                    break;
                case 5:
                    factor = 5.23f;
                    period = 300;
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
                    if (count == 0)
                        SkillFireBolt.skill = SkillFactory.Instance.GetSkill(3009, 1);
                    if (count == 1)
                        SkillFireBolt.skill = SkillFactory.Instance.GetSkill(3017, 1);
                    if (count == 2)
                        SkillFireBolt.skill = SkillFactory.Instance.GetSkill(3036, 1);
                    if (count == 3)
                        SkillFireBolt.skill = SkillFactory.Instance.GetSkill(3044, 1);
                    SkillFireBolt.argType =
                        SkillArg.ArgType
                            .Active; //Configure the skillarg of firebolt, the caster is the skillactor of subsituted groove.
                    SkillFireBolt.sActor = SkillBody.ActorID;
                    SkillFireBolt.dActor = AimActor.ActorID;
                    SkillFireBolt.x = 255;
                    SkillFireBolt.y = 255;
                    if (count == 0)
                        SkillHandler.Instance.MagicAttack(sActor, AimActor, SkillFireBolt, Elements.Fire, factor);
                    if (count == 1)
                        SkillHandler.Instance.MagicAttack(sActor, AimActor, SkillFireBolt, Elements.Wind, factor);
                    if (count == 2)
                        SkillHandler.Instance.MagicAttack(sActor, AimActor, SkillFireBolt, Elements.Water, factor);
                    if (count == 3)
                        SkillHandler.Instance.MagicAttack(sActor, AimActor, SkillFireBolt, Elements.Earth, factor);
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