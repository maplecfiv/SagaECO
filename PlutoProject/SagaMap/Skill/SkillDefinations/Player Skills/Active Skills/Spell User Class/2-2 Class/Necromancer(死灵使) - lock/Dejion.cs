using SagaDB.Actor;
using SagaLib;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_2_Class.Necromancer_死灵使____lock
{
    /// <summary>
    ///     蒸發（イヴァペレイト）
    /// </summary>
    public class Dejion : ISkill
    {
        #region ISkill Members

        public int TryCast(ActorPC sActor, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            var rate = 5 * level;
            if (dActor.type == ActorType.MOB)
            {
                var dActorMob = (ActorMob)dActor;
                if (SkillHandler.Instance.isBossMob(dActorMob)) return;
            }

            if (SagaLib.Global.Random.Next(0, 99) < rate)
            {
                //SkillHandler.Instance.MagicAttack (sActor, dActor, args, SagaLib.Elements.Dark ,0f );
                dActor.HP = 0;
                dActor.e.OnDie();
                //ChatArg arg = new ChatArg();
                //arg.motion = SagaLib.MotionType.DEAD;
                //arg.loop = 0;
                args.affectedActors.Add(dActor);
                args.Init();
                args.flag[0] = AttackFlag.DIE;
                //Manager.MapManager.Instance.GetMap(dActor.MapID).SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.EMOTION, null, dActor, false); 
            }
        }

        #endregion
    }
}