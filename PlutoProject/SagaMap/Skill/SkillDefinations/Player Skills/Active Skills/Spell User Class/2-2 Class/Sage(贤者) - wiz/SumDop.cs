using SagaDB.Actor;
using SagaMap.ActorEventHandlers;
using SagaMap.Manager;
using SagaMap.Mob;
using SagaMap.Skill.Additions;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Sage_贤者____wiz
{
    /// <summary>
    ///     活幽靈（ドッペルゲンガー）
    /// </summary>
    public class SumDop : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var lifetime = 300000;
            var skill = new SumDopBuff(args.skill, sActor, lifetime, args);
            SkillHandler.ApplyAddition(sActor, skill);
        }

        public class SumDopBuff : DefaultBuff
        {
            private readonly SkillArg thisarg;
            private ActorShadow shadow;

            public SumDopBuff(SagaDB.Skill.Skill skill, Actor actor, int lifetime, SkillArg args)
                : base(skill, actor, "SumDop", lifetime)
            {
                thisarg = args;
                OnAdditionStart += StartEvent;
                OnAdditionEnd += EndEvent;
            }

            private void StartEvent(Actor actor, DefaultBuff skill)
            {
                shadow = SummonMe((ActorPC)actor, skill.skill.Level);
            }

            private void EndEvent(Actor actor, DefaultBuff skill)
            {
                var map = MapManager.Instance.GetMap(actor.MapID);
                var eh = (PetEventHandler)shadow.e;
                eh.AI.Pause();
                shadow.ClearTaskAddition();
                map.DeleteActor(shadow);
            }

            public ActorShadow SummonMe(ActorPC pc, byte level)
            {
                var actor = new ActorShadow(pc);
                var map = MapManager.Instance.GetMap(pc.MapID);
                actor.Name = pc.Name;
                actor.MapID = pc.MapID;
                actor.X = SagaLib.Global.PosX8to16(thisarg.x, map.Width);
                actor.Y = SagaLib.Global.PosY8to16(thisarg.y, map.Height);
                actor.MaxHP = (uint)(pc.MaxHP * (0.1f + 0.2f * level));
                actor.HP = actor.MaxHP;
                actor.Status.max_matk_skill += (short)(pc.Status.max_matk * 1.5f);
                actor.Status.min_matk_skill += (short)(pc.Status.min_matk * 1.5f);
                actor.Speed = pc.Speed;
                actor.BaseData.range = 1.5f;
                actor.Owner = pc;
                actor.BaseData.id = 26330000;
                actor.PictID = 26330000;
                actor.type = ActorType.ANOTHERMOB;

                var eh = new PetEventHandler(actor);
                actor.e = eh;

                eh.AI.Mode = new AIMode(1);
                switch (skill.Level)
                {
                    case 1:
                        eh.AI.Mode.EventAttacking.Add(3281, 100); //魔法衝擊波
                        break;
                    case 2:
                        eh.AI.Mode.EventAttacking.Add(3281, 50); //魔法衝擊波
                        eh.AI.Mode.EventAttacking.Add(3127, 50); //魔力放出
                        break;
                    case 3:
                        eh.AI.Mode.EventAttacking.Add(3281, 30); //魔法衝擊波
                        eh.AI.Mode.EventAttacking.Add(3127, 30); //魔力放出
                        eh.AI.Mode.EventAttacking.Add(3291, 40); //燃燒生命
                        break;
                }

                eh.AI.Mode.EventAttackingSkillRate = 100;
                eh.AI.Master = pc;
                map.RegisterActor(actor);
                actor.invisble = false;
                map.OnActorVisibilityChange(actor);
                map.SendVisibleActorsToActor(actor);
                eh.AI.Start();
                return actor;
            }
        }

        //#endregion
    }
}