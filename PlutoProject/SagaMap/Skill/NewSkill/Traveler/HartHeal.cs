using System.Collections.Generic;
using SagaDB.Actor;
using SagaMap.Manager;
using SagaMap.Skill.SkillDefinations;

namespace SagaMap.Skill.NewSkill.Traveler
{
    internal class HartHeal : ISkill
    {
        //#region ISkill Members

        public int TryCast(ActorPC pc, Actor dActor, SkillArg args)
        {
            return 0;
        }

        public void Proc(Actor sActor, Actor dActor, SkillArg args, byte level)
        {
            float factor = -1;
            var map = MapManager.Instance.GetMap(sActor.MapID);
            //向右的判定矩型

            var actors = map.GetRoundAreaActors(dActor.X, dActor.Y, 300);
            //List<Actor> actors = map.GetRoundAreaActors(SagaLib.Global.PosX8to16(args.x, map.Width), SagaLib.Global.PosY8to16(args.y, map.Height), 300);

            var affected = new List<Actor>();
            foreach (var i in actors)
                //if (SkillHandler.Instance.CheckValidAttackTarget(sActor, i))
                affected.Add(i);
            //SkillHandler.Instance.MagicAttack(sActor, affected, args, SagaLib.Elements.Holy, factor);

            SkillHandler.Instance.MagicAttack(sActor, affected, args, sActor.WeaponElement, factor);
        }

        //#endregion
    }
}