using System.Collections.Generic;
using SagaDB.Actor;
using SagaLib;
using SagaMap.Manager;

namespace SagaMap.Skill.SkillDefinations.Player_Skills.Active_Skills.Spell_User_Class._2_1_Class.Cabalist_秘术使____lock
{
    /// <summary>
    ///     グリムリーパー
    /// </summary>
    public class GrimReaper : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = 0;
            args.type = ATTACK_TYPE.BLOW;

            factor = 1.75f + 0.35f * level;
            var actors = new List<Actor>();
            if (level == 6)
            {
                var arg = new EffectArg();
                arg.effectID = 5236;
                arg.actorID = dActor.ActorID;
                MapManager.Instance.GetMap(sActor.MapID)
                    .SendEventToAllActorsWhoCanSeeActor(Map.EVENT_TYPE.SHOW_EFFECT, arg, (ActorPC)sActor, true);
                factor = 2f;
                for (var i = 0; i < 3; i++) actors.Add(dActor);
                args.delay = 500;
                SkillHandler.Instance.PhysicalAttack(sActor, actors, args, SkillHandler.DefType.Def, Elements.Dark, 0,
                    factor, false, 0.6f, false);
            }
            else
            {
                actors.Add(dActor);
                SkillHandler.Instance.PhysicalAttack(sActor, actors, args, SkillHandler.DefType.Def, Elements.Dark, 0,
                    factor, false, 0.1f, false);
            }
        }

        //#endregion
    }
}